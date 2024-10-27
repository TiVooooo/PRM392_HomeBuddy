using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model
{
    public class ServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public double Duration { get; set; }

        public string HelperName { get; set; }

        public string Image { get; set; }
    }
}
