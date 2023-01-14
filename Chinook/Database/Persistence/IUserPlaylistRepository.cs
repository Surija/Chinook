using Microsoft.EntityFrameworkCore;

namespace Chinook.Database.Persistence
{
    public interface IUserPlaylistRepository
    {
    }

    public class UserPlaylistRepository : IUserPlaylistRepository
    {
        private readonly IDbContextFactory<ChinookContext> _contextFactory;
        public UserPlaylistRepository(IDbContextFactory<ChinookContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
