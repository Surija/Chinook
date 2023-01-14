using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Database.Persistence
{
    public interface IPlaylistRepository
    {
        Task<PlaylistDto> GetPlaylist(string userId, long playlistId);
        Task<List<Playlist>> GetAllPlaylist();
        Task<Playlist> AddPlaylist(string playlistName);
    }

    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly IDbContextFactory<ChinookContext> _contextFactory;
        public PlaylistRepository(IDbContextFactory<ChinookContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PlaylistDto> GetPlaylist(string userId, long playlistId)
        {
            var playlist = new PlaylistDto();
            var dbContext = await _contextFactory.CreateDbContextAsync();
            playlist = dbContext.Playlists
                .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == playlistId)
                .Select(p => new PlaylistDto()
                {
                    Name = p.Name == null ? "" : p.Name,
                    Tracks = p.Tracks.Select(t => new PlaylistTrack()
                    {
                        AlbumTitle = t.Album.Title,
                        ArtistName = t.Album.Artist.Name,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "My favorite tracks")).Any()
                    }).ToList()
                })
                .FirstOrDefault();

            return playlist;
        }

        public async Task<List<Playlist>> GetAllPlaylist()
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            var playlists = dbContext.Playlists
            .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist).ToList();
            return playlists;
        }

        public async Task<Playlist> AddPlaylist(string playlistName)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            var max = dbContext.Playlists.DefaultIfEmpty().Max(r => r == null ? 0 : r.PlaylistId);
            var playlist = new Playlist { PlaylistId = (max + 1), Name = playlistName };
            await dbContext.Playlists.AddAsync(playlist);
            dbContext.SaveChanges();
            return playlist;
        }
    }
}
