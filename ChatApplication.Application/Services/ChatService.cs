using AutoMapper;
using ChatApplication.Application.Common;
using ChatApplication.Application.Contracts;
using ChatApplication.Application.DTOs.Chat;
using ChatApplication.Application.Helper;
using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using Microsoft.Extensions.Options;

namespace ChatApplication.Application.Services
{
    internal class ChatService : IChatService
    {
        private readonly IMapper _mapper;
        private readonly IChatRequestRepository _chatRequestRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAgentService _agentService;
        private readonly IAgentRepository _agentRepository;
        private readonly OfficeHours _officeHours;
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(IMapper mapper, IChatRequestRepository chatRequestRepository,
           ICustomerRepository customerRepository, IAgentRepository agentRepository,
           IAgentService agentService, IOptions<OfficeHours> officeHours,
           IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _chatRequestRepository = chatRequestRepository;
            _customerRepository = customerRepository;
            _agentService = agentService;
            _agentRepository = agentRepository;
            _officeHours = officeHours.Value;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Create_Chat_Session_Async(InitiateChatDTO InitiateChatDTO)
        {
            var datetimenow = DateTime.Now;
            int max_Queue_Length = 0;

            var office_Start_DateTime = _officeHours.EndHour < 8 ?
                             new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day - 1, _officeHours.StartHour, 00, 00) :
                             new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day, _officeHours.StartHour, 00, 00);

            var office_End_DateTime = _officeHours.StartHour > 16 ?
                             new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day + 1, _officeHours.EndHour, 00, 00) :
                             new DateTime(datetimenow.Year, datetimenow.Month, datetimenow.Day, _officeHours.EndHour, 00, 00);

            if (datetimenow >= office_Start_DateTime && datetimenow < office_End_DateTime)
            {
                max_Queue_Length = await _agentService.Get_Max_Queue_Length_Within_Office_Hours_Async();
            }
            else
            {
                max_Queue_Length = await _agentService.Get_Max_Queue_Length_Outside_Office_Hours_Async();
            }

            var current_Queue_Length = await _chatRequestRepository.Get_Current_Queue_Length_Async();

            if (current_Queue_Length < max_Queue_Length)
            {
                var customer = _mapper.Map<Customer>(InitiateChatDTO);
                customer.Id = Guid.NewGuid();
                await _customerRepository.Create_Async(customer);

                var chatRequest = new ChatRequest()
                {
                    Id = Guid.NewGuid(),
                    CreatedDateTime = DateTime.Now,
                    CustomerId = customer.Id,
                    Status = (int)ChatRequestStatus.InQueue
                };
                await _chatRequestRepository.Create_Async(chatRequest);
                await _unitOfWork.Save_Async();

                return Result<string>.Success(Convert.ToString(chatRequest.Id));
            }
            else
                return Result<string>.Failure("No agents available at the moment.Please try again later.");
        }

        public async Task<Result<string>> End_All_Connected_Chats_Async()
        {
            var chats = await _chatRequestRepository.Get_All_Connected_ChatRequests_Async();
            foreach (var chat in chats)
            {
                await End_Chat_Async(chat.Id);
            }
            return Result<string>.Success("");
        }

        public async Task<Result<string>> End_Chat_Async(Guid Id)
        {
            var chatRequest = await _chatRequestRepository.Get_Async(Id);
            if (chatRequest != null)
            {
                chatRequest.Status = (int)ChatRequestStatus.Terminated;
                _chatRequestRepository.Update(chatRequest);
                var agent = await _agentRepository.Get_Async(chatRequest.AgentId);
                if (agent != null)
                {
                    agent.CurrentChatLoad--;
                    if (agent.CurrentChatLoad < 0)
                        agent.CurrentChatLoad = 0;
                    _agentRepository.Update(agent);
                }
                await _unitOfWork.Save_Async();
                return Result<string>.Success("");
            }
            else
            {
                return Result<string>.Failure("Chat not found.");
            }
        }

        public async Task<Result<List<ChatRequest>>> Get_All_Active_ChatRequests_Async()
        {
            return Result<List<ChatRequest>>.Success(await _chatRequestRepository.Get_All_Active_ChatRequests_Async());
        }

        public async Task<Result<List<ChatRequest>>> Get_All_ChatRequests_Async()
        {
            return Result<List<ChatRequest>>.Success(await _chatRequestRepository.GetAll_Async());
        }

        public async Task<Result<string>> ChatRequest_Polling_Async(Guid Id)
        {
            var chatRequest = await _chatRequestRepository.Get_Async(Id);
            if (chatRequest != null && chatRequest.Status == (int)ChatRequestStatus.InQueue)
            {
                if (chatRequest.PollCounter < 3)
                {
                    chatRequest.PollCounter++;
                    _chatRequestRepository.Update(chatRequest);
                    await _unitOfWork.Save_Async();
                    return Result<string>.Success("");
                }
                else
                {
                    chatRequest.Status = (int)ChatRequestStatus.Terminated;
                    _chatRequestRepository.Update(chatRequest);
                    await _unitOfWork.Save_Async();
                    return Result<string>.Failure("Chat is terminated.");
                }
            }
            else
                return Result<string>.Failure("Chat not found.");
        }
    }
}
