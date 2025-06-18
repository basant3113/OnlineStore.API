using OnlineStore.API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.DAL.Models
{
    public class Product:BaseModel
    {
        public string Name { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }


        public int? TypeId { get; set; }
        public ProductType Type { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
