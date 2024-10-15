using HomeBuddy.Data.Base;
using HomeBuddy.Data.Models;
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
    }
}
