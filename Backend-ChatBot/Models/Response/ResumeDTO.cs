using Backend_ChatBot.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_ChatBot.Models.Response
{
    public class ResumeDTO
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public string? ContentType { get; set; } = "text/plain";
        public required byte[] FileContent { get; set; }
    }
}
