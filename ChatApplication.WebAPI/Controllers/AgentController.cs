using ChatApplication.Application.Contracts;
using ChatApplication.Application.DTOs.Agent;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatApplication.WebAPI.Controllers
{
    public class AgentController : BaseAPIController
    {
        private readonly IAgentService _AgentService;

        public AgentController(IAgentService AgentService)
        {
            _AgentService = AgentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAgent(Create_Update_Agent_DTO agentDTO)
        {
            return HandleResult(await _AgentService.Create_Agent_Async(agentDTO));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgents()
        {
            return HandleResult(await _AgentService.Get_All_Agents_Async());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgent(Guid id, Create_Update_Agent_DTO agentDTO)
        {
            var result = await _AgentService.Update_Agent_Async(id, agentDTO);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return HandleResult(result);
        }
    }
}
