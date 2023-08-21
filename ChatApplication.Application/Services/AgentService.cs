using AutoMapper;
using ChatApplication.Application.Contracts;
using ChatApplication.Application.DTOs.Agent;
using ChatApplication.Application.Helper;
using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Entities;

namespace ChatApplication.Application.Services
{
    public class AgentService : IAgentService
    {
        private readonly IMapper _mapper;
        private readonly IAgentRepository _agentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AgentService(IMapper mapper, IAgentRepository agentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _agentRepository = agentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Agent>> Create_Agent_Async(Create_Update_Agent_DTO createAgentDTO)
        {
            var agent = _mapper.Map<Agent>(createAgentDTO);
            agent.Id = Guid.NewGuid();
            await _agentRepository.Create_Async(agent);
            await _unitOfWork.Save_Async();
            return Result<Agent>.Success(agent);
        }

        public async Task<Result<List<Agent>>> Get_All_Agents_Async()
        {
            return Result<List<Agent>>.Success(await _agentRepository.GetAll_Async());
        }

        public async Task<int> Get_Max_Queue_Length_Outside_Office_Hours_Async()
        {
            var agents = await _agentRepository.Get_All_Agents_Without_OverFlow_Async();
            return Get_Max_Queue_Length_Based_On_Agents_ShiftTime_Async(agents);
        }

        public async Task<int> Get_Max_Queue_Length_Within_Office_Hours_Async()
        {
            var agents = await _agentRepository.GetAll_Async();
            return Get_Max_Queue_Length_Based_On_Agents_ShiftTime_Async(agents);
        }

        public async Task<Result<string>> Update_Agent_Async(Guid Id, Create_Update_Agent_DTO agentDTO)
        {
            var agent = await _agentRepository.Get_Async(Id);
            if (agent != null)
            {
                var updateagent = _mapper.Map(agentDTO, agent);
                updateagent.Id = Id;
                _agentRepository.Update(updateagent);
                await _unitOfWork.Save_Async();
                return Result<string>.Success("");
            }
            else
                return Result<string>.Failure("Agent not found.");
        }

        private int Get_Max_Queue_Length_Based_On_Agents_ShiftTime_Async(List<Agent> agents)
        {
            var datetimenow = DateTime.Now;
            double max_Queue_Length = 0;

            foreach (var agent in agents)
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
                        max_Queue_Length += agent.MaxChatLoad * agent.Seniority * 1.5;
                    }
                }
                else
                {
                    max_Queue_Length += agent.MaxChatLoad * agent.Seniority;
                }
            }
            return Convert.ToInt32(Math.Round(max_Queue_Length));
        }
    }
}
