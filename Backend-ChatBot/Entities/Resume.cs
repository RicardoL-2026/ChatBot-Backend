using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_ChatBot.Entities
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string FileName { get; set; }
        public string? ContentType { get; set; } = "text/plain";
        public int Size { get; set; }
        [Required]
        public required byte[] FileContent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ExtractedText { get; set; }
    }
}
