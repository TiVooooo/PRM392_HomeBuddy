using HomeBuddy.Data.Base;
using HomeBuddy.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Data.Repository
{
    public class MessageRepository : GenericRepository<Message>
    {
        public MessageRepository() { }
        public MessageRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }
        public IQueryable<Message> GetAllMessageByChatId(int chatId)
        {
            return _context.Messages.Include(m => m.Chat).Where(m => m.ChatId == chatId);
        }
    }
}
