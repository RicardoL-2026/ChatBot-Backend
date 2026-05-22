using Backend_ChatBot.Entities;

namespace Backend_ChatBot.Models.Response
{
    public class ConversationDTO
    {
        public required int ID { get; set; }
        public required string Title { get; set; }
        public List<MessageDTO> Messages { get; set; } = new();

    }
}
