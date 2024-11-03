using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.ResponseDTO
{
    public class CartResponseDTO
    {
        public int CartId {  get; set; }
        public string ServiceName { get; set; }
        public string ServiceImage { get; set; }
        public double ServicePrice { get; set; }
        public int Quantity { get; set; }
        public double Subtotal { get; set; }
    }
}
