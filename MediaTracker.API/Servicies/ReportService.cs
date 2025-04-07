using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Services.Interfaces;
using MediaTracker.API.Utils;

namespace MediaTracker.API.Services
{
    public class ReportService : IReportService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly PdfGenerator _pdfGenerator;

        public ReportService(
            IUserRepository userRepository,
            IMediaRepository mediaRepository,
            ICommentRepository commentRepository,
            IWatchlistRepository watchlistRepository)
        {
            _userRepository = userRepository;
            _mediaRepository = mediaRepository;
            _commentRepository = commentRepository;
            _watchlistRepository = watchlistRepository;
            _pdfGenerator = new PdfGenerator();
        }

        public async Task<object> GenerateReportAsync(string reportType, DateTime dateFrom, DateTime dateTo)
        {
            switch (reportType.ToLower())
            {
                case "users":
                    return await GenerateUserReportAsync(dateFrom, dateTo);
                case "interactions":
                    return await GenerateInteractionsReportAsync(dateFrom, dateTo);
                case "genres":
                    return await GenerateGenresReportAsync(dateFrom, dateTo);
                case "content":
                    return await GenerateContentReportAsync(dateFrom, dateTo);
                default:
                    throw new ArgumentException($"Invalid report type: {reportType}");
            }
        }

        public async Task<byte[]> GenerateReportPdfAsync(string reportType, DateTime dateFrom, DateTime dateTo, object reportData)
        {
            return await _pdfGenerator.GenerateReportPdfAsync(reportType, dateFrom, dateTo, reportData);
        }

        private async Task<object> GenerateUserReportAsync(DateTime dateFrom, DateTime dateTo)
        {
            var users = await _userRepository.GetAllAsync();
            var newUsers = users.Where(u => u.CreatedAt >= dateFrom && u.CreatedAt <= dateTo).ToList();

            // Group users by day
            var usersByDay = newUsers
                .GroupBy(u => u.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToList();

            var labels = usersByDay.Select(u => u.Date.ToString("MMM dd")).ToList();
            var values = usersByDay.Select(u => u.Count).ToList();

            var summary = new List<string>
            {
                $"Total new users in period: {newUsers.Count}",
                $"Average new users per day: {(usersByDay.Count > 0 ? (double)newUsers.Count / usersByDay.Count : 0):F2}",
            };

            if (usersByDay.Any())
            {
                var peakDay = usersByDay.OrderByDescending(u => u.Count).First();
                summary.Add($"Peak day: {peakDay.Date:MMM dd} with {peakDay.Count} new users");
            }

            return new
            {
                ChartData = new
                {
                    Title = "New Users",
                    Labels = labels,
                    Values = values
                },
                Summary = summary
            };
        }

        private async Task<object> GenerateInteractionsReportAsync(DateTime dateFrom, DateTime dateTo)
        {
            var watchlistItems = await _watchlistRepository.GetAllAsync();
            var comments = await _commentRepository.GetAllAsync();

            var watchlistAdds = watchlistItems.Count(w => w.AddedAt >= dateFrom && w.AddedAt <= dateTo);
            var commentCount = comments.Count(c => c.CreatedAt >= dateFrom && c.CreatedAt <= dateTo);

            // For demonstration purposes, we'll add some mock data for other interaction types
            var ratings = 156;
            var profileViews = 98;
            var searches = 312;

            var labels = new List<string> { "Watchlist Adds", "Comments", "Ratings", "Profile Views", "Search" };
            var values = new List<int> { watchlistAdds, commentCount, ratings, profileViews, searches };

            var summary = new List<string>
            {
                $"Total interactions in period: {values.Sum()}",
                $"Most common interaction: {labels[values.IndexOf(values.Max())]} ({values.Max()} times)",
                $"Least common interaction: {labels[values.IndexOf(values.Min())]} ({values.Min()} times)"
            };

            return new
            {
                ChartData = new
                {
                    Title = "User Interactions",
                    Labels = labels,
                    Values = values
                },
                Summary = summary
            };
        }

        private async Task<object> GenerateGenresReportAsync(DateTime dateFrom, DateTime dateTo)
        {
            var movies = await _mediaRepository.GetAllMoviesAsync();
            var series = await _mediaRepository.GetAllSeriesAsync();
            var allMedia = movies.Concat(series).ToList();

            // Extract all unique genres
            var allGenres = allMedia
                .SelectMany(m => m.Genres)
                .Distinct()
                .ToList();

            // Count media by genre
            var genreCounts = new Dictionary<string, int>();
            foreach (var genre in allGenres)
            {
                genreCounts[genre] = allMedia.Count(m => m.Genres.Contains(genre));
            }

            var sortedGenres = genreCounts.OrderByDescending(g => g.Value).ToList();
            var labels = sortedGenres.Select(g => g.Key).ToList();
            var values = sortedGenres.Select(g => g.Value).ToList();

            var summary = new List<string>();
            if (sortedGenres.Any())
            {
                summary.Add($"Most popular genre: {sortedGenres.First().Key} ({sortedGenres.First().Value} media)");
                summary.Add($"Least popular genre: {sortedGenres.Last().Key} ({sortedGenres.Last().Value} media)");
                summary.Add($"Average media per genre: {sortedGenres.Average(g => g.Value):F2}");
            }

            return new
            {
                ChartData = new
                {
                    Title = "Popular Genres",
                    Labels = labels,
                    Values = values
                },
                Summary = summary
            };
        }

        private async Task<object> GenerateContentReportAsync(DateTime dateFrom, DateTime dateTo)
        {
            var movies = await _mediaRepository.GetAllMoviesAsync();
            var series = await _mediaRepository.GetAllSeriesAsync();
            var allMedia = movies.Concat(series).ToList();

            // For demonstration purposes, we'll assign random view counts to media
            var random = new Random(DateTime.Now.Millisecond);
            var mediaWithViews = allMedia
                .Select(m => new { Media = m, Views = random.Next(50, 500) })
                .OrderByDescending(m => m.Views)
                .Take(10)
                .ToList();

            var labels = mediaWithViews.Select(m => m.Media.Title).ToList();
            var values = mediaWithViews.Select(m => m.Views).ToList();

            var summary = new List<string>();
            if (mediaWithViews.Any())
            {
                summary.Add($"Most viewed content: {mediaWithViews.First().Media.Title} ({mediaWithViews.First().Views} views)");
                summary.Add($"Average views per content: {mediaWithViews.Average(m => m.Views):F2}");
                summary.Add($"Total views in period: {mediaWithViews.Sum(m => m.Views)}");
            }

            return new
            {
                ChartData = new
                {
                    Title = "Popular Content",
                    Labels = labels,
                    Values = values
                },
                Summary = summary
            };
        }
    }
}