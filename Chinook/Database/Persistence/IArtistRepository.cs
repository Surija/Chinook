using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Database.Persistence
{
    public interface IArtistRepository
    {
        Task<List<Artist>> GetAllArtist();
        Task<Artist?> GetArtist(long artistId);
        Task<List<PlaylistTrack>> GetTracks(string userId, long artistId);
    }

    public class ArtistRepository : IArtistRepository
    {
        private readonly IDbContextFactory<ChinookContext> _contextFactory;
        public ArtistRepository(IDbContextFactory<ChinookContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Artist>> GetAllArtist()
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            var users = dbContext.Users.Include(a => a.UserPlaylists).ToList();
            return dbContext.Artists.ToList();
        }

        public async Task<Artist?> GetArtist(long artistId)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            return dbContext.Artists.SingleOrDefault(a => a.ArtistId == artistId);
        }

        public async Task<List<PlaylistTrack>> GetTracks(string userId, long artistId)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            return dbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
            .Include(a => a.Album)
            .Select(t => new PlaylistTrack()
            {
                AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                TrackId = t.TrackId,
                TrackName = t.Name,
                IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "My favorite tracks")).Any()
            })
            .ToList();
        }
    }
}
