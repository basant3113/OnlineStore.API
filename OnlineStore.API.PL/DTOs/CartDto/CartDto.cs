using OnlineStore.API.Core.Models;

namespace OnlineStore.API.PL.DTOs.CartDto
{
    public class CartDto
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public double TotalAmount { get; set; }
    }
}
