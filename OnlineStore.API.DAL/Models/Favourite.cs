using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Models
{
    public class Favourite
    {
        public string UserId { get; set; }
        public int productId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationUser User { get; set; }
        public Product product { get; set; }
    }
}
