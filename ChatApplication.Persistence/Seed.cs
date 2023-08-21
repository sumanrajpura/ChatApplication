using ChatApplication.Application.Contracts;
using ChatApplication.Application.DTOs.Agent;
using ChatApplication.Persistence.Context;

namespace ChatApplication.Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, IAgentService agentService)
        {
            if (!context.Agents.Any())
            {
                var agents = new List<Create_Update_Agent_DTO>
                {
                    new Create_Update_Agent_DTO
                    {
                        Name = "Bob",
                        Seniority = 0.4,
                        IsOverFlowMember = false,
                        ShiftStartHour = 9
                    },
                    new Create_Update_Agent_DTO
                    {
                         Name = "Jane",
                        Seniority = 0.6,
                        IsOverFlowMember = false,
                        ShiftStartHour = 12
                    },
                    new Create_Update_Agent_DTO
                    {
                        Name = "Tom",
                        Seniority = 0.8,
                        IsOverFlowMember = false,
                        ShiftStartHour = 17
                    },
                    new Create_Update_Agent_DTO
                    {
                        Name = "Nick",
                        Seniority = 0.4,
                        IsOverFlowMember = true,
                        ShiftStartHour = 9
                    },
                };

                foreach (var agent in agents)
                {
                    await agentService.Create_Agent_Async(agent);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
