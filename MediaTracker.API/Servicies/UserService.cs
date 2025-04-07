using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Services.Interfaces;

namespace MediaTracker.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IMediaRepository _mediaRepository;

        public UserService(
            IUserRepository userRepository,
            IWatchlistRepository watchlistRepository,
            IMediaRepository mediaRepository)
        {
            _userRepository = userRepository;
            _watchlistRepository = watchlistRepository;
            _mediaRepository = mediaRepository;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<List<Media>> GetWatchlistAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var watchlistItems = await _watchlistRepository.GetByUserIdAsync(userId);
            var mediaList = new List<Media>();

            foreach (var item in watchlistItems)
            {
                var media = await _mediaRepository.GetByIdAsync(item.MediaId);
                if (media != null)
                {
                    mediaList.Add(media);
                }
            }

            return mediaList;
        }

        public async Task<WatchlistItem> AddToWatchlistAsync(int userId, int mediaId, string mediaType)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var media = await _mediaRepository.GetByIdAsync(mediaId);
            if (media == null)
            {
                throw new Exception("Media not found");
            }

            // Check if item already exists in watchlist
            var existingItem = await _watchlistRepository.GetByUserAndMediaIdAsync(userId, mediaId);
            if (existingItem != null)
            {
                throw new Exception("Item already in watchlist");
            }

            var watchlistItem = new WatchlistItem
            {
                UserId = userId,
                MediaId = mediaId,
                MediaType = mediaType,
                AddedAt = DateTime.Now
            };

            return await _watchlistRepository.AddAsync(watchlistItem);
        }

        public async Task<bool> RemoveFromWatchlistAsync(int userId, int mediaId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return await _watchlistRepository.DeleteAsync(userId, mediaId);
        }
    }
}