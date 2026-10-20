using Microsoft.EntityFrameworkCore;
using Entertainments.Models;

namespace Entertainments.Datas
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Song>();
        }
    }
}
