using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.ResponseDTO
{
    public class RelativeService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int HelperId { get; set; }
        public string Image { get; set; }
    }
}
