using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;

namespace MediaTracker.API.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int id);
        Task<List<Comment>> GetByMediaIdAsync(int mediaId);
        Task<List<Comment>> GetByUserIdAsync(int userId);
        Task<Comment> AddAsync(Comment comment);
        Task<Comment> UpdateAsync(Comment comment);
        Task<bool> DeleteAsync(int id);
    }
}