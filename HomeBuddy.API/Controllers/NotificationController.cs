using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{
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
    }

}
