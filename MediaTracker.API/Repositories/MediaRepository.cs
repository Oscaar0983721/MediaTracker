using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Utils;

namespace MediaTracker.API.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly JsonFileManager<Media> _moviesFileManager;
        private readonly JsonFileManager<Media> _seriesFileManager;

        public MediaRepository()
        {
            _moviesFileManager = new JsonFileManager<Media>("Data/movies.json");
            _seriesFileManager = new JsonFileManager<Media>("Data/series.json");
        }

        public async Task<List<Media>> GetAllMoviesAsync()
        {
            return await _moviesFileManager.ReadAllAsync();
        }

        public async Task<List<Media>> GetAllSeriesAsync()
        {
            return await _seriesFileManager.ReadAllAsync();
        }

        public async Task<Media> GetByIdAsync(int id)
        {
            var movies = await GetAllMoviesAsync();
            var series = await GetAllSeriesAsync();

            var media = movies.FirstOrDefault(m => m.Id == id);
            if (media == null)
            {
                media = series.FirstOrDefault(s => s.Id == id);
            }

            return media;
        }

        public async Task<List<Media>> GetByGenreAsync(string genre)
        {
            var movies = await GetAllMoviesAsync();
            var series = await GetAllSeriesAsync();

            var filteredMovies = movies.Where(m => m.Genres.Contains(genre, StringComparer.OrdinalIgnoreCase)).ToList();
            var filteredSeries = series.Where(s => s.Genres.Contains(genre, StringComparer.OrdinalIgnoreCase)).ToList();

            return filteredMovies.Concat(filteredSeries).ToList();
        }

        public async Task<List<Media>> SearchAsync(string query)
        {
            var movies = await GetAllMoviesAsync();
            var series = await GetAllSeriesAsync();

            var filteredMovies = movies.Where(m =>
                m.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                m.Overview.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            var filteredSeries = series.Where(s =>
                s.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                s.Overview.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            return filteredMovies.Concat(filteredSeries).ToList();
        }

        public async Task<Media> AddAsync(Media media)
        {
            if (media.Type == "movie")
            {
                var movies = await GetAllMoviesAsync();
                media.Id = movies.Count > 0 ? movies.Max(m => m.Id) + 1 : 1;
                media.CreatedAt = DateTime.Now;
                media.UpdatedAt = DateTime.Now;

                movies.Add(media);
                await _moviesFileManager.WriteAllAsync(movies);
            }
            else if (media.Type == "series")
            {
                var series = await GetAllSeriesAsync();
                media.Id = series.Count > 0 ? series.Max(s => s.Id) + 1 : 1;
                media.CreatedAt = DateTime.Now;
                media.UpdatedAt = DateTime.Now;

                series.Add(media);
                await _seriesFileManager.WriteAllAsync(series);
            }

            return media;
        }

        public async Task<Media> UpdateAsync(Media media)
        {
            if (media.Type == "movie")
            {
                var movies = await GetAllMoviesAsync();
                var index = movies.FindIndex(m => m.Id == media.Id);

                if (index >= 0)
                {
                    media.CreatedAt = movies[index].CreatedAt;
                    media.UpdatedAt = DateTime.Now;
                    movies[index] = media;
                    await _moviesFileManager.WriteAllAsync(movies);
                    return media;
                }
            }
            else if (media.Type == "series")
            {
                var series = await GetAllSeriesAsync();
                var index = series.FindIndex(s => s.Id == media.Id);

                if (index >= 0)
                {
                    media.CreatedAt = series[index].CreatedAt;
                    media.UpdatedAt = DateTime.Now;
                    series[index] = media;
                    await _seriesFileManager.WriteAllAsync(series);
                    return media;
                }
            }

            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movies = await GetAllMoviesAsync();
            var movieIndex = movies.FindIndex(m => m.Id == id);

            if (movieIndex >= 0)
            {
                movies.RemoveAt(movieIndex);
                await _moviesFileManager.WriteAllAsync(movies);
                return true;
            }

            var series = await GetAllSeriesAsync();
            var seriesIndex = series.FindIndex(s => s.Id == id);

            if (seriesIndex >= 0)
            {
                series.RemoveAt(seriesIndex);
                await _seriesFileManager.WriteAllAsync(series);
                return true;
            }

            return false;
        }
    }
}