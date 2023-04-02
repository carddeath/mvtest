using Microsoft.EntityFrameworkCore;
using MusicVineTest.Models;

namespace MusicVineTest.Database
{
    public class MusicDbContext : DbContext
    {
        public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

        public DbSet<MusicEntry> MusicEntries => Set<MusicEntry>();
    }
}
