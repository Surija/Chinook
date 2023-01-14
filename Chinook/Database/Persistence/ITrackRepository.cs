using Microsoft.EntityFrameworkCore;

namespace Chinook.Database.Persistence
{
    public interface ITrackRepository
    {
        Task<bool> AddTrack(long playlistId, long trackId);
        Task<bool> RemoveTrack(long playlistId, long trackId);
    }

    public class TrackRepository : ITrackRepository
    {
        private readonly IDbContextFactory<ChinookContext> _contextFactory;
        public TrackRepository(IDbContextFactory<ChinookContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> AddTrack(long playlistId, long trackId)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            var playlist = dbContext.Playlists.Include(a => a.Tracks).FirstOrDefault(x => x.PlaylistId == playlistId);
            if (playlist != null)
            {
                var track = dbContext.Tracks.FirstOrDefault(x => x.TrackId == trackId);
                if (track != null)
                {
                    var existingTrack = playlist.Tracks.FirstOrDefault(x => x.TrackId == track.TrackId);
                    if (existingTrack == null)
                    {
                        playlist.Tracks.Add(track);
                        dbContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            return false;

        }

        public async Task<bool> RemoveTrack(long playlistId, long trackId)
        {
            var dbContext = await _contextFactory.CreateDbContextAsync();
            var playlist = dbContext.Playlists.Include(a => a.Tracks).FirstOrDefault(x => x.PlaylistId == playlistId);
            if (playlist != null)
            {
                var existingTrack = playlist.Tracks.FirstOrDefault(x => x.TrackId == trackId);
                if (existingTrack != null)
                {
                    playlist.Tracks.Remove(existingTrack);
                    dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            return false;

        }
    }
}
