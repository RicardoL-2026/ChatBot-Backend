using System.ComponentModel.DataAnnotations;

namespace Backend_ChatBot.Models.Request.Message
{
    public class MessageCreateDTO
    {
        [Required]
        public required string Messa { get; set; }
        [Required]
        public required string Type { get; set; } = "Request"; // type = "Response" | "Request"
        [Required]
        public required int ConversacionID { get; set; }
    }
}
