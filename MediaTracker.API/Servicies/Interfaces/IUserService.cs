using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;

namespace MediaTracker.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<List<Media>> GetWatchlistAsync(int userId);
        Task<WatchlistItem> AddToWatchlistAsync(int userId, int mediaId, string mediaType);
        Task<bool> RemoveFromWatchlistAsync(int userId, int mediaId);
    }
}