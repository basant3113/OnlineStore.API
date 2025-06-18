using System.ComponentModel.DataAnnotations;

namespace OnlineStore.API.PL.DTOs
{
    public class FavouriteDTO
    {
        [Required(ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
    }
}
