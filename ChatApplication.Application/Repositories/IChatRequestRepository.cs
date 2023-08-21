using ChatApplication.Domain.Entities;

namespace ChatApplication.Application.Repositories
{
    public interface IChatRequestRepository : IBaseRepository<ChatRequest>
    {
        Task<int> Get_Current_Queue_Length_Async();
        Task<List<ChatRequest>> Get_InQueue_ChatRequests_Async();
        Task<List<ChatRequest>> Get_All_Active_ChatRequests_Async();
        Task<List<ChatRequest>> Get_All_Connected_ChatRequests_Async();
    }
}
