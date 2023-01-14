using Microsoft.EntityFrameworkCore;

namespace Chinook.Database.Persistence
{
    public interface ITrackRepository
    {
    }

    public class TrackRepository : ITrackRepository
    {
        private readonly IDbContextFactory<ChinookContext> _contextFactory;
        public TrackRepository(IDbContextFactory<ChinookContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
