using HomeBuddy.Common;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model.RequestDTO;
using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace HomeBuddy.API.Configurations.ChatHub
{
    
    public class ChatHub : Hub
    {
        private readonly UnitOfWork _unitOfWork;
        public ChatHub(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task JoinChat(Chat chat)
        {
            await Clients.All.SendAsync("ReceiveMessage", chat.SenderId, $"{chat.ReceiverId} has joined");
        }

        public async Task SendMessage(MessageRequest messageRequest)
        {

            var chat = await _unitOfWork.ChatRepository.GetAllAsync();
            var chatExisted = chat.FirstOrDefault(c =>
                (c.SenderId == messageRequest.SenderId && c.ReceiverId == messageRequest.ReceiverId) ||
                (c.SenderId == messageRequest.ReceiverId && c.ReceiverId == messageRequest.SenderId));

            if (chatExisted == null)
            {
                chatExisted = new Chat
                {
                    SenderId = messageRequest.SenderId,
                    ReceiverId = messageRequest.ReceiverId,
                    Messages = new List<Message>()
                };
                await _unitOfWork.ChatRepository.CreateAsync(chatExisted);
            }

            var message = new Message
            {
                MessageText = messageRequest.MessageText,
                SentTime = DateTime.Now,
                SenderId = messageRequest.SenderId,
                ChatId = chatExisted.Id
            };

            await _unitOfWork.MessageRepository.CreateAsync(message);

            await Clients.Group(chatExisted.Id.ToString()).SendAsync("ReceiveMessage", message.SenderId, message.MessageText, message.SentTime);
        }

    }
}