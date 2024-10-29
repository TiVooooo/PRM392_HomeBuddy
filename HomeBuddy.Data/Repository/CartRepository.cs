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
    public class CartRepository : GenericRepository<Cart>
    {
        public CartRepository() { }
        public CartRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }

        public IQueryable<Cart> GetAllCartWithOthers()
        {
            return _context.Carts
                .Include(b => b.Service);
        }
    }
}
