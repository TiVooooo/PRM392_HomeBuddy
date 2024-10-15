using HomeBuddy.Data.Base;
using HomeBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Data.Repository
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository() { }
        public BookingRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }

    }
}
