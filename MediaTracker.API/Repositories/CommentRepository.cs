using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Utils;

namespace MediaTracker.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly JsonFileManager<Comment> _fileManager;

        public CommentRepository()
        {
            _fileManager = new JsonFileManager<Comment>("Data/comments.json");
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _fileManager.ReadAllAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            var comments = await GetAllAsync();
            return comments.FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<Comment>> GetByMediaIdAsync(int mediaId)
        {
            var comments = await GetAllAsync();
            return comments.Where(c => c.MediaId == mediaId).ToList();
        }

        public async Task<List<Comment>> GetByUserIdAsync(int userId)
        {
            var comments = await GetAllAsync();
            return comments.Where(c => c.UserId == userId).ToList();
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            var comments = await GetAllAsync();
            comment.Id = comments.Count > 0 ? comments.Max(c => c.Id) + 1 : 1;

            comments.Add(comment);
            await _fileManager.WriteAllAsync(comments);

            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var comments = await GetAllAsync();
            var index = comments.FindIndex(c => c.Id == comment.Id);

            if (index >= 0)
            {
                comments[index] = comment;
                await _fileManager.WriteAllAsync(comments);
                return comment;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comments = await GetAllAsync();
            var index = comments.FindIndex(c => c.Id == id);

            if (index >= 0)
            {
                comments.RemoveAt(index);
                await _fileManager.WriteAllAsync(comments);
                return true;
            }

            return false;
        }
    }
}