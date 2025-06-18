using System.ComponentModel.DataAnnotations;

namespace OnlineStore.API.PL.DTOs
{
    public class SendMessageDTO
    {
        [Required(ErrorMessage = "ReciverId is required")]
        public string ReceiverId { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}
