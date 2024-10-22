using FirebaseAdmin.Messaging;
using HomeBuddy.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{

    public interface INotificationService
    {
        Task<string> SendNotification(string token, string title, string body);

    }

    public class NotificationService : INotificationService
    {
        private readonly UnitOfWork _unitOfWork;
        public NotificationService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> SendNotification(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>()
                {
                    { "key1", "value1" }
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
    }
}
