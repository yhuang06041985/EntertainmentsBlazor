using Entertainments.Datas;
using Entertainments.Models;
using Microsoft.EntityFrameworkCore;

namespace Entertainments.Repository
{
    public class SongRepostitory
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public SongRepostitory(
            IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Song>> GetSongsAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.Songs
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Song?> GetFromIdAsync(int id)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.Songs.FindAsync(id);
        }

        public async Task AddSongAsync(Song song)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            context.Songs.Add(song);
            await context.SaveChangesAsync();
        }

        public async Task DeleteSongAsync(int id)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var song = await context.Songs.FindAsync(id);

            if (song is not null)
            {
                context.Songs.Remove(song);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateSongAsync(int id, Song song)
        {
            if (id != song.Id)
                return;

            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var existingSong = await context.Songs.FindAsync(id);

            if (existingSong is not null)
            {
                existingSong.Name = song.Name;
                existingSong.Artist = song.Artist;
                existingSong.FilePath = song.FilePath;

                await context.SaveChangesAsync();
            }
        }
    }
}