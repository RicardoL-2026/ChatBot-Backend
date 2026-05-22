using Backend_ChatBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_ChatBot.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions option) : base(option) { }

        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Resume> Resume { get; set; }
    }
}
