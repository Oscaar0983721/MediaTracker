using System.Collections.Generic;
using System.Threading.Tasks;
using MediaTracker.API.Models;

namespace MediaTracker.API.Repositories.Interfaces
{
    public interface IMediaRepository
    {
        Task<List<Media>> GetAllMoviesAsync();
        Task<List<Media>> GetAllSeriesAsync();
        Task<Media> GetByIdAsync(int id);
        Task<List<Media>> GetByGenreAsync(string genre);
        Task<List<Media>> SearchAsync(string query);
        Task<Media> AddAsync(Media media);
        Task<Media> UpdateAsync(Media media);
        Task<bool> DeleteAsync(int id);
    }
}