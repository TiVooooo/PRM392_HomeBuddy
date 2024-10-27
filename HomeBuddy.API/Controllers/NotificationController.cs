using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(string token, string title, string body)
        {
            var response = await _notificationService.SendNotification(token, title, body);
            return Ok(response);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetAllNoti([FromRoute]int id)
        {
            var response = await _notificationService.GetNotification(id);
            return Ok(response);
        }

    }

}
