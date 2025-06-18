using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Models
{
    public class ChatMessage:BaseModel
    {

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public ApplicationUser Sender { get; set; }

        public ApplicationUser Receiver { get; set; }
    }
}
