using System.ComponentModel.DataAnnotations;

namespace Backend_ChatBot.Models.Request.Conversation
{
    public class ConversationCreateDTO
    {
        [Required]
        public required string Title { get; set; }
    }
}
