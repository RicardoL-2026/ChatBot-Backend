using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_ChatBot.Entities
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; } = string.Empty;
        public ICollection<Message> Messages { get; set; }
           = new List<Message>();

    }
}
