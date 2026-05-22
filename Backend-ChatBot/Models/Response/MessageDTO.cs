namespace Backend_ChatBot.Models.Response
{
    public class MessageDTO
    {
        public required int ID { get; set; }
        public required string Messa { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;
        public required string Type { get; set; } // type = "Response" | "Request"
        public required int ConversacionID { get; set; }

    }
}
