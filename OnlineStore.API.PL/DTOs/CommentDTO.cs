using System.ComponentModel.DataAnnotations;

namespace OnlineStore.API.PL.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "ApartmentId is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Text is required")]
        [MaxLength(500, ErrorMessage = "Maximim length of text is 500")]
        public string Text { get; set; }
    }
}
