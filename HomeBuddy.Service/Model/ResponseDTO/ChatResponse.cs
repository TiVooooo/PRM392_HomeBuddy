using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model.ResponseDTO
{
    public class ChatResponse
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string senderName { get; set; }
        public string receiverName { get; set; }
        public List<MessageResponse> Messages { get; set; }
    }
}
