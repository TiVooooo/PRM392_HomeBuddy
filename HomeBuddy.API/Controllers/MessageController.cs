using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
        [HttpGet("chatId")]
        public async Task<IActionResult> GetMessageByChatId(int chatId)
        {
            var result = await _messageService.GetAllMessagesByChatId(chatId);

            return Ok(result.Data);
        }
    }
}
