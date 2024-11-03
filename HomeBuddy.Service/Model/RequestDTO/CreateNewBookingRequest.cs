using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.RequestDTO
{
    public class CreateNewBookingRequest
    {
        public double Price { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        public DateTime? BookingDay { get; set; }

        public DateTime ServiceDate { get; set; }

        public int HelperId { get; set; }

        public int UserId { get; set; }
    }
}
