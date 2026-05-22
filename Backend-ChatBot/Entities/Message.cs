using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_ChatBot.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Messa { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
        public required string Type { get; set; } = string.Empty; // type = "Response" | "Request"
        
        [ForeignKey("Conversation")]
        [Required]
        public required int ConversacionID { get; set; }
        public required Conversation Conversation { get; set; }

    }
}
