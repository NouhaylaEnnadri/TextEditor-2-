using CollaborationApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CollaborationApp.Data
{
    public class TextDbContext : DbContext
    {
        public TextDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Text> Text{ get; set; }
    }

}