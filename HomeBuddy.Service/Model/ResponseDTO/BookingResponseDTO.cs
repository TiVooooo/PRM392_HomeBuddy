using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.ResponseDTO
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string BookingDate { get; set; }
        public string BookingTime { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public double LongItude { get; set; }
        public double Latitude { get; set; }
        public string HelperName { get; set; }
        public string UserName { get; set; }
        public int CartID {  get; set; }
    }
}
