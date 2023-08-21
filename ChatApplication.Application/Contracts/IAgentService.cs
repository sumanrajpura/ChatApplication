using ChatApplication.Application.DTOs.Agent;
using ChatApplication.Application.Helper;
using ChatApplication.Domain.Entities;

namespace ChatApplication.Application.Contracts
{
    public interface IAgentService
    {
        Task<Result<Agent>> Create_Agent_Async(Create_Update_Agent_DTO createAgentDTO);
        Task<Result<List<Agent>>> Get_All_Agents_Async();
        Task<int> Get_Max_Queue_Length_Within_Office_Hours_Async();
        Task<int> Get_Max_Queue_Length_Outside_Office_Hours_Async();
        Task<Result<string>> Update_Agent_Async(Guid Id, Create_Update_Agent_DTO agent);
    }
}
