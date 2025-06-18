using OnlineStore.API.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.API.PL.DTOs
{
    public class RegisterDTO
    {

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public string Age { get; set; }


        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public ApplicationUserType Type { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}