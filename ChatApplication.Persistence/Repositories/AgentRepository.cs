using ChatApplication.Application.Repositories;
using ChatApplication.Domain.Entities;
using ChatApplication.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Persistence.Repositories
{
    public class AgentRepository : BaseRepository<Agent>, IAgentRepository
    {
        protected readonly DataContext _context;

        public AgentRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Agent>> Get_Agents_Without_OverFlow_To_Assign_Chat_Async()
        {
            return await _context.Agents
                         .Where(a => !a.IsOverFlowMember
                                     && a.CurrentChatLoad < (a.MaxChatLoad * a.Seniority))
                         .OrderBy(a => a.Seniority)
                         .ToListAsync();
        }

        public async Task<List<Agent>> Get_Agents_With_OverFlow_To_AssignChat_Async()
        {
            var agents = await Get_Agents_Without_OverFlow_To_Assign_Chat_Async();

            var agent = await _context.Agents
                          .Where(a => a.IsOverFlowMember
                                     && a.CurrentChatLoad < (a.MaxChatLoad * a.Seniority))
                          .FirstOrDefaultAsync();

            if (agent != null)
                agents.Add(agent);

            return agents;
        }

        public async Task<List<Agent>> Get_All_Agents_Without_OverFlow_Async()
        {
            return await _context.Agents
                                 .Where(a => !a.IsOverFlowMember)
                                 .ToListAsync();
        }
    }
}
