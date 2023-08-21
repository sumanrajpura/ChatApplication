using ChatApplication.Domain.Entities;

namespace ChatApplication.Application.Repositories
{
    public interface IAgentRepository : IBaseRepository<Agent>
    {
        Task<List<Agent>> Get_Agents_Without_OverFlow_To_Assign_Chat_Async();
        Task<List<Agent>> Get_Agents_With_OverFlow_To_AssignChat_Async();
        Task<List<Agent>> Get_All_Agents_Without_OverFlow_Async();
    }
}
