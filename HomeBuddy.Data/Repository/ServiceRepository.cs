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
    public class ServiceRepository : GenericRepository<Service>
    {
        public ServiceRepository() { }
        public ServiceRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }

        public async Task<List<HomeBuddy.Data.Models.Service>> GetServicesAsync()
        {
            return await _context.Services.Include(s=>s.Helper).ToListAsync();
        }
        public async Task<int> CountService()
        {
            return await _context.Services.CountAsync();
        }
    }
}
