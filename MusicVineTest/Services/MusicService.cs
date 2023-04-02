using Microsoft.EntityFrameworkCore;
using MusicVineTest.Database;
using MusicVineTest.Models;

namespace MusicVineTest.Services
{
    public class MusicService: IMusicService
    {
        private readonly MusicDbContext _dbContext;

        public MusicService(MusicDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(MusicEntry entry)
        {
            _dbContext.MusicEntries.Add(entry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(MusicEntry entry)
        {
            _dbContext.MusicEntries.Remove(entry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<MusicEntry>> GetSongsAsync()
        {
            return await _dbContext.MusicEntries.ToListAsync();
        }

        public async Task UpdateAsync(MusicEntry entry)
        {
            _dbContext.MusicEntries.Update(entry);
            await _dbContext.SaveChangesAsync();
        }
        public async ValueTask<MusicEntry?> FindMusicEntryAsync(int id)
        {
            return await _dbContext.MusicEntries.FindAsync(id);
        }

    }
}
