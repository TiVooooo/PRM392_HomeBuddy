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
    public class NotificationRepository : GenericRepository<Notification>
    {
        public NotificationRepository() { }
        public NotificationRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }

            public async Task<List<HomeBuddy.Data.Models.Notification>> GetAllNotificationRepo()
            {
                return await _context.Notifications.Include(n=>n.User).ToListAsync();
            }
    }
}
