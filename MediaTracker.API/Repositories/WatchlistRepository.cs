using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Utils;

namespace MediaTracker.API.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly JsonFileManager<WatchlistItem> _fileManager;

        public WatchlistRepository()
        {
            _fileManager = new JsonFileManager<WatchlistItem>("Data/watchlist.json");
        }

        public async Task<List<WatchlistItem>> GetAllAsync()
        {
            return await _fileManager.ReadAllAsync();
        }

        public async Task<List<WatchlistItem>> GetByUserIdAsync(int userId)
        {
            var watchlist = await GetAllAsync();
            return watchlist.Where(w => w.UserId == userId).ToList();
        }

        public async Task<WatchlistItem> GetByUserAndMediaIdAsync(int userId, int mediaId)
        {
            var watchlist = await GetAllAsync();
            return watchlist.FirstOrDefault(w => w.UserId == userId && w.MediaId == mediaId);
        }

        public async Task<WatchlistItem> AddAsync(WatchlistItem watchlistItem)
        {
            var watchlist = await GetAllAsync();
            watchlistItem.Id = watchlist.Count > 0 ? watchlist.Max(w => w.Id) + 1 : 1;

            watchlist.Add(watchlistItem);
            await _fileManager.WriteAllAsync(watchlist);

            return watchlistItem;
        }

        public async Task<bool> DeleteAsync(int userId, int mediaId)
        {
            var watchlist = await GetAllAsync();
            var index = watchlist.FindIndex(w => w.UserId == userId && w.MediaId == mediaId);

            if (index >= 0)
            {
                watchlist.RemoveAt(index);
                await _fileManager.WriteAllAsync(watchlist);
                return true;
            }

            return false;
        }
    }
}