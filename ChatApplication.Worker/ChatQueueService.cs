using ChatApplication.Application.Common;
using ChatApplication.Application.Contracts;
using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ChatApplication.Worker
{
    public class ChatQueueService : BackgroundService
    {
        private readonly OfficeHours _officeHours;
        private IServiceScopeFactory _serviceScopeFactory;

        public ChatQueueService(IServiceProvider serviceProvider, IOptions<OfficeHours> officeHours,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _officeHours = officeHours.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            while (true)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _agentRepository = scope.ServiceProvider.GetRequiredService<IAgentRepository>();
                    var _agentService = scope.ServiceProvider.GetRequiredService<IAgentService>();
                    var _chatRequestRepository = scope.ServiceProvider.GetRequiredService<IChatRequestRepository>();
                    var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    Agent? agent = null;

                    var in_Queue_Chats = await _chatRequestRepository.Get_InQueue_ChatRequests_Async();
                    foreach (var in_Queue_Chat in in_Queue_Chats)
                    {

                        var max_Queue_Length = await _agentService.Get_Max_Queue_Length_Outside_Office_Hours_Async();
                        var current_Queue_Length = await _chatRequestRepository.Get_Current_Queue_Length_Async();
                        if (current_Queue_Length < max_Queue_Length)
                        {
                            agent = Get_Agent_To_Assign_Chat(await _agentRepository.Get_Agents_Without_OverFlow_To_Assign_Chat_Async());
                        }
                        else if (DateTime.Now.Hour >= _officeHours.StartHour && DateTime.Now.Hour < _officeHours.EndHour)
                        {
                            agent = Get_Agent_To_Assign_Chat(await _agentRepository.Get_Agents_With_OverFlow_To_AssignChat_Async());
                        }

                        if (agent != null)
                        {
                            in_Queue_Chat.AgentId = agent.Id;
                            in_Queue_Chat.Status = (int)ChatRequestStatus.Connected;
                            agent.CurrentChatLoad++;
                            _agentRepository.Update(agent);
                            _chatRequestRepository.Update(in_Queue_Chat);
                            await _unitOfWork.Save_Async();
                        }
                        else
                            break;
                    }
                }
            }
        }

        private Agent? Get_Agent_To_Assign_Chat(List<Agent> agents)
        {
            var datetimenow = DateTime.Now;
            List<Agent> available_Agents = new List<Agent>();

            foreach (Agent agent in agents)
            {
                if (!agent.IsOverFlowMember)
                {
                    var shift_Star_Datetime = agent.ShiftEndHour < 8 ?
                           new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day - 1, agent.ShiftStartHour, 00, 00) :
                           new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day, agent.ShiftStartHour, 00, 00);

                    var shift_End_Datetime = agent.ShiftStartHour > 16 ?
                        new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day + 1, agent.ShiftEndHour, 00, 00) :
                        new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day, agent.ShiftEndHour, 00, 00);

                    if (datetimenow >= shift_Star_Datetime && datetimenow < shift_End_Datetime)
                    {
                        available_Agents.Add(agent);
                    }
                }
                else
                    available_Agents.Add(agent);
            }

            return available_Agents
                        .OrderBy(a => a.Seniority)
                        .FirstOrDefault();
        }
    }
}
