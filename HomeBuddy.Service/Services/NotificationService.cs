using FirebaseAdmin.Messaging;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Model;
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
        Task<List<NotiModel>> GetNotification(int u);

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

        public async Task<List<NotiModel>> GetNotification(int id)
        {
            var list =  await _unitOfWork.NotificationRepository.GetAllAsync();
            var noti = list.Where(x => x.UserId == id).ToList();
            var result = new List<NotiModel>();
            foreach (var item in noti) {
                var notiModel = new NotiModel
                {
                    Date = item.Date.ToString("dd-MM-yyyy"),
                    Description = item.Description,
                    Status = item.Status,
                    Tittle = item.Tittle,
                    UserName = item.User.Name
                };
                result.Add(notiModel);
            }
            
            if (result == null)
            {
                return null;
            }
            return result;
        }

    }
}
