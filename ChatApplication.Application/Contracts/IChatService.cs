using ChatApplication.Application.DTOs.Chat;
using ChatApplication.Application.Helper;
using ChatApplication.Domain.Entities;

namespace ChatApplication.Application.Contracts
{
    public interface IChatService
    {
        Task<Result<string>> Create_Chat_Session_Async(InitiateChatDTO InitiateChatDTO);
        Task<Result<List<ChatRequest>>> Get_All_ChatRequests_Async();
        Task<Result<List<ChatRequest>>> Get_All_Active_ChatRequests_Async();
        Task<Result<string>> End_Chat_Async(Guid Id);
        Task<Result<string>> End_All_Connected_Chats_Async();
        Task<Result<string>> ChatRequest_Polling_Async(Guid Id);
    }
}
