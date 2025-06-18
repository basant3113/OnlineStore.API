using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Models
{
    public class ApplicationUser:IdentityUser    //UserName   Email   Password   PhoneNumber
    {
        public int Age { get; set; }
        public ApplicationUserType Type { get; set; }
        public string Role { get; set; }
    }
}
