using System.ComponentModel.DataAnnotations;

namespace OnlineStore.API.PL.DTOs.CartDto
{
    public class AddCartDto
    {
        [Required(ErrorMessage ="ProductId is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage ="Quantity is required")]
        public int Quantity { get; set; }
    }
}