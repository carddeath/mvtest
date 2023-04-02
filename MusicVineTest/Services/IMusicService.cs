using MusicVineTest.Models;

namespace MusicVineTest.Services
{
    public interface IMusicService
    {
        Task<List<MusicEntry>> GetSongsAsync();
        Task AddAsync(MusicEntry entry);
        Task UpdateAsync(MusicEntry entry);
        Task DeleteAsync(MusicEntry entry);
        ValueTask<MusicEntry?> FindMusicEntryAsync(int id);
    }
}
