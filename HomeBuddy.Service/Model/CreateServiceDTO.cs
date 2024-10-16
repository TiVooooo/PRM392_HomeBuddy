using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model
{
    public class CreateServiceDTO
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public double Duration { get; set; }

        public int HelperId { get; set; }
    }
}
