using HomeBuddy.Service.Model.RequestDTO;
using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        [HttpGet]
        public async Task<IActionResult> GetALl()
        {
            var result = await _chatService.GetAllChat();

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
        {
            var result = await _chatService.SendMessage(messageRequest);
            
                return Ok(result.Message);
        }
    }
}