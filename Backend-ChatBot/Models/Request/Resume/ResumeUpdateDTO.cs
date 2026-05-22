using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_ChatBot.Models.Request.Resume
{
    public class ResumeUpdateDTO
    {
        [Required]
        public int ID { get; set; }
        public string? FileName { get; set; }
        public IFormFile? FileContent { get; set; }
    }
}
