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
    public class HelperRepository : GenericRepository<Helper>
    {
        public HelperRepository() { }
        public HelperRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }
        public async Task<List<Helper>> GetHelpersWithParentAsync()
        {
            return await _context.Helpers
                                 .Include(h => h.User)
                                 .ThenInclude(u => u.Parent)
                                 .ToListAsync();
        }
    }
}
