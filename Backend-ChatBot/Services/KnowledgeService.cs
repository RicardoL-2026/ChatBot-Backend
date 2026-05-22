using Backend_ChatBot.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend_ChatBot.Services
{
    public class KnowledgeService
    {
        private readonly AppDbContext _db;

        public KnowledgeService(AppDbContext db) { _db = db; }

        public async Task<string> GetContextAsync()
        {
            var resume = await _db.Resume
                .Select(m => new
                {
                    m.FileName,
                    m.ExtractedText
                })
                .ToListAsync();

            return string.Join("\n\n", resume.Select(m =>
                $"""
                    Documento: {m.FileName}
                    Contenido:
                    {m.ExtractedText}
                """
            ));
        }
    }
}
