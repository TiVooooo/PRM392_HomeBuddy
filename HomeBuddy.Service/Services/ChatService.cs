using AutoMapper;
using HomeBuddy.Common;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model.RequestDTO;
using HomeBuddy.Service.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface IChatService
    {
        Task<IBusinessResult> GetAllChat();
        Task<IBusinessResult> SendMessage(MessageRequest request);
        Task<IBusinessResult> GetChatFromUserId(int userid);
        Task<IBusinessResult> GetAllMessageFromChat(int chatid);
    }

    public class ChatService : IChatService
    {
        private readonly UnitOfWork _unitOfWork;
        public ChatService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IBusinessResult> GetAllChat()
        {
            var chats = _unitOfWork.ChatRepository.GetAllChatWithMessages();
            var chatDto = chats.Select(c => new ChatResponse
            {
                Id = c.Id,
                SenderId = c.SenderId,
                ReceiverId = c.ReceiverId,
                Messages = c.Messages.Select(m => new MessageResponse
                {
                    Id = m.Id,
                    MessageText = m.MessageText,
                    SentTime = m.SentTime,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.Name
                }).ToList()
            }).ToList();

            return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, chatDto);
        }

        public async Task<IBusinessResult> SendMessage(MessageRequest messageRequest)
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

            return new BusinessResult(Const.SUCCESS_CREATE, "Message sent successfully");
        }

        public async Task<IBusinessResult> GetChatFromUserId(int userid)
        {
            var chatresponse = await _unitOfWork.ChatRepository.GetChatFromUserId(userid).ToListAsync();
            var chatDto = chatresponse.Select(c => new ChatResponse
            {
                Id = c.Id,
                SenderId = c.SenderId,
                ReceiverId = c.ReceiverId,
                receiverName = c.SenderId == userid ? c.Receiver.Name : c.Sender.Name,

            }).ToList();
            if (chatresponse == null || !chatresponse.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
            }
            return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, chatDto);
        }

        public async Task<IBusinessResult> GetAllMessageFromChat(int chatid)
        {
            var chatresponse = await _unitOfWork.MessageRepository.GetAllMessageByChatId(chatid).ToListAsync();         
           
            var chatDto = chatresponse.Select(c => new MessageResponse
            {
                Id = c.Id,
                MessageText = c.MessageText,
                SentTime = c.SentTime,
                SenderId = c.SenderId,
                SenderName = c.Sender.Name
            }).ToList();
            if (chatresponse == null || !chatresponse.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
            }
            return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, chatDto);
        }

    }
}
