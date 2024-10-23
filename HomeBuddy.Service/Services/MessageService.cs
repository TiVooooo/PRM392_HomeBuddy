using HomeBuddy.Common;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{

    public interface IMessageService
    {
        Task<IBusinessResult> GetAllMessagesByChatId(int chatId);
    }
    public class MessageService : IMessageService
    {
        private readonly UnitOfWork _unitOfWork;
        public MessageService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IBusinessResult> GetAllMessagesByChatId(int chatId)
        {
            var chat = await _unitOfWork.ChatRepository.GetByIdAsync(chatId);
            if (chat == null)
            {
                return new BusinessResult(Const.FAIL_READ, "Chat not found");
            }

            var messages = _unitOfWork.MessageRepository.GetAllMessageByChatId(chat.Id);
            var messageDto = messages.Select(m => new MessageResponse
            {
                Id = m.Id,
                MessageText = m.MessageText,
                SentTime = m.SentTime,
                SenderId = m.SenderId
            }).ToList();

            return new BusinessResult(Const.SUCCESS_READ, "Messages retrieved successfully", messageDto);
        }
    }
}
