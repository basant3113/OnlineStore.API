using System.ComponentModel.DataAnnotations;

namespace OnlineStore.API.PL.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "OldPrice is required")]
        public double OldPrice { get; set; }

        [Required(ErrorMessage = "NewPrice is required")]
        public double NewPrice { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string? PictureUrl { get; set; }

        [Required(ErrorMessage = "Picture is required")]
        public IFormFile Picture { get; set; }
        public int TypeId { get; set; }
    }
}
