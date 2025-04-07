using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Services.Interfaces;

namespace MediaTracker.API.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public MediaService(
            IMediaRepository mediaRepository,
            ICommentRepository commentRepository,
            IUserRepository userRepository)
        {
            _mediaRepository = mediaRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Media>> GetAllMoviesAsync()
        {
            return await _mediaRepository.GetAllMoviesAsync();
        }

        public async Task<List<Media>> GetAllSeriesAsync()
        {
            return await _mediaRepository.GetAllSeriesAsync();
        }

        public async Task<Media> GetByIdAsync(int id)
        {
            return await _mediaRepository.GetByIdAsync(id);
        }

        public async Task<List<Media>> GetByGenreAsync(string genre)
        {
            return await _mediaRepository.GetByGenreAsync(genre);
        }

        public async Task<List<Media>> SearchAsync(string query)
        {
            return await _mediaRepository.SearchAsync(query);
        }

        public async Task<Media> AddAsync(Media media)
        {
            return await _mediaRepository.AddAsync(media);
        }

        public async Task<Media> UpdateAsync(Media media)
        {
            return await _mediaRepository.UpdateAsync(media);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _mediaRepository.DeleteAsync(id);
        }

        public async Task<List<Comment>> GetCommentsAsync(int mediaId)
        {
            return await _commentRepository.GetByMediaIdAsync(mediaId);
        }

        public async Task<Comment> AddCommentAsync(int mediaId, int userId, string content)
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

            var comment = new Comment
            {
                MediaId = mediaId,
                UserId = userId,
                UserName = user.Name,
                Content = content,
                CreatedAt = DateTime.Now
            };

            return await _commentRepository.AddAsync(comment);
        }
    }
}