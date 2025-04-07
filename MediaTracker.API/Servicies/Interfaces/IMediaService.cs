using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;

namespace MediaTracker.API.Services.Interfaces
{
    public interface IMediaService
    {
        Task<List<Media>> GetAllMoviesAsync();
        Task<List<Media>> GetAllSeriesAsync();
        Task<Media> GetByIdAsync(int id);
        Task<List<Media>> GetByGenreAsync(string genre);
        Task<List<Media>> SearchAsync(string query);
        Task<Media> AddAsync(Media media);
        Task<Media> UpdateAsync(Media media);
        Task<bool> DeleteAsync(int id);
        Task<List<Comment>> GetCommentsAsync(int mediaId);
        Task<Comment> AddCommentAsync(int mediaId, int userId, string content);
    }
}