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
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository() { }
        public UserRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllExceptAdminAsync()
        {
            return await _context.Set<User>()
                                 .Where(u => u.Role != "Admin")
                                 .ToListAsync();
        }
    }
}
