using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Models
{
    public class Comment:BaseModel
    {
        public string UserId { get; set; }

        public int productId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Product product { get; set; }
        public ApplicationUser User { get; set; }
    }
}
