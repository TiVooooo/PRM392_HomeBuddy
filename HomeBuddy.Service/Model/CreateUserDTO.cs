using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model
{
    public class CreateUserDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public bool Gender { get; set; }

        public string Address { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? ParentId { get; set; }
    }
}
