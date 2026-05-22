using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_ChatBot.Models.Request.Resume
{
    public class ResumeCreateDTO
    {
        [Required]
        public required string FileName { get; set; }
        [Required]
        public required IFormFile FileContent { get; set; }
    }
}
