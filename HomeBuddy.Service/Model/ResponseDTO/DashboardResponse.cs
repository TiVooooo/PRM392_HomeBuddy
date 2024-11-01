using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.ResponseDTO
{
    public class DashboardResponse
    {
        public int UserCount { get; set; }
        public int BookingCount { get; set; }
        public int ServiceCount { get; set; }
        public double TotalIncome { get; set; }
    }
}
