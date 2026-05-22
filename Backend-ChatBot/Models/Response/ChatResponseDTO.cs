namespace Backend_ChatBot.Models.Response
{
    public class ChatResponseDTO
    {
        public required MessageDTO Question { get; set; }
        public required string Answer { get; set; }
    }
}
