using ChatApplication.Application.Contracts;
using ChatApplication.Application.DTOs.Chat;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseAPIController
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Initiate_Chat(InitiateChatDTO initiateChatDTO)
        {
            return HandleResult(await _chatService.Create_Chat_Session_Async(initiateChatDTO));
        }

        [HttpGet("ActiveChats")]
        public async Task<IActionResult> Get_All_Active_ChatRequests()
        {
            return HandleResult(await _chatService.Get_All_Active_ChatRequests_Async());
        }

        [HttpGet("ChatPolling/{Id}")]
        public async Task<IActionResult> Chat_Polling(Guid Id)
        {
            return HandleResult(await _chatService.ChatRequest_Polling_Async(Id));
        }


        [HttpPut("EndChat/{Id}")]
        public async Task<IActionResult> EndChat(Guid Id)
        {
            var result = await _chatService.End_Chat_Async(Id);
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleResult(result);
        }

        [HttpPut("EndAllConnectedChats")]
        public async Task<IActionResult> End_All_Connected_Chats()
        {
            var result = await _chatService.End_All_Connected_Chats_Async();
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleResult(result);
        }
    }
}
