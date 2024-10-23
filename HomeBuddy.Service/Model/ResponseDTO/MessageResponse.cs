using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.ResponseDTO
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
        public int SenderId { get; set; }
    }
}
