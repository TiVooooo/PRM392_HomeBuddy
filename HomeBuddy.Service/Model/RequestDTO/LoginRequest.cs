using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.RequestDTO
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? DeviceToken { get; set; } 
    }
}
