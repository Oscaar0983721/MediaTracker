using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;

namespace MediaTracker.API.Repositories.Interfaces
{
    public interface IWatchlistRepository
    {
        Task<List<WatchlistItem>> GetAllAsync();
        Task<List<WatchlistItem>> GetByUserIdAsync(int userId);
        Task<WatchlistItem> GetByUserAndMediaIdAsync(int userId, int mediaId);
        Task<WatchlistItem> AddAsync(WatchlistItem watchlistItem);
        Task<bool> DeleteAsync(int userId, int mediaId);
    }
}