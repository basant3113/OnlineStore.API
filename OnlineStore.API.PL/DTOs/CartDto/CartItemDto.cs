
namespace OnlineStore.API.PL.DTOs.CartDto
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string ProductPictureUrl { get; set; }
        public int Quantity { get; set; }
    }
}
