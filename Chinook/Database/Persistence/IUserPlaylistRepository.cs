using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Database.Persistence
{
    public interface IUserPlaylistRepository
    {
        Task<UserPlaylist?> GetPlaylist(string userId, long playlistId);
        Task<UserPlaylist> AddPlaylist(UserPlaylist playlist);
        Task<List<UserPlaylist>> GetAllPlaylists(string userId);
        Task<bool> RemovePlaylist(string userId, long playlistId);
    }

    public class UserPlaylistRepository : IUserPlaylistRepository
    {
        private readonly IDbContextFactory<ChinookContext> _contextFactory;
        public UserPlaylistRepository(IDbContextFactory<ChinookContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserPlaylist?> GetPlaylist(string userId, long playlistId)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            return dbContext.UserPlaylists.Include(a => a.Playlist).ThenInclude(a => a.Tracks).FirstOrDefault(p => p.UserId == userId && p.PlaylistId == playlistId);
        }

        public async Task<UserPlaylist> AddPlaylist(UserPlaylist playlist)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            await dbContext.UserPlaylists.AddAsync(playlist);
            dbContext.SaveChanges();
            return playlist;
        }

        public async Task<List<UserPlaylist>> GetAllPlaylists(string userId)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            return dbContext.UserPlaylists.Where(up => up.UserId == userId).Include(a => a.User).Include(a => a.Playlist).ToList();
        }

        public async Task<bool> RemovePlaylist(string userId, long playlistId)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            var playlist = dbContext.UserPlaylists.FirstOrDefault(x => x.UserId == userId && x.PlaylistId == playlistId);
            if (playlist != null)
            {
                dbContext.UserPlaylists.Remove(playlist);
                dbContext.SaveChanges();
                return true;
            }

            return false;

        }
    }
}
