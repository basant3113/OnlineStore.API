using System.ComponentModel.DataAnnotations;

namespace OnlineStore.API.PL.DTOs
{
    public class TypeDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
