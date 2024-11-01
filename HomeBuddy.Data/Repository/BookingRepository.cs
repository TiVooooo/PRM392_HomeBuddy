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
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository() { }
        public BookingRepository(PRM392_HomeBuddyContext context)
        {
            _context = context;
        }

        public IQueryable<Booking> GetAllBookingsWithOthers()
        {
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Helper)
                       .ThenInclude(b => b.User)
                .Include(b => b.Helper)
                .Include(b => b.Service);
        }
        public async Task<int> CountBooking()
        {
            return await _context.Bookings.CountAsync();
        }
        public async Task<double> GetTotal()
        {
            return await _context.Bookings.SumAsync(b => b.Price);
        }
    }
}
