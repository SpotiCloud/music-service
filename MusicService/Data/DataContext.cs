using Microsoft.EntityFrameworkCore;
using MusicService.Models;

namespace MusicService.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=music;Username=postgres;Password=example");

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<Song> song { get; set; }
    }
}
