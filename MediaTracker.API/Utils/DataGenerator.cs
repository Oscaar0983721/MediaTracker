using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Bogus;
using MediaTracker.API.Models;

namespace MediaTracker.API.Utils
{
    public static class DataGenerator
    {
        public static void GenerateSampleData(string dataPath)
        {
            GenerateUsers(Path.Combine(dataPath, "users.json"));
            GenerateMovies(Path.Combine(dataPath, "movies.json"));
            GenerateSeries(Path.Combine(dataPath, "series.json"));
            GenerateEmptyFile(Path.Combine(dataPath, "comments.json"));
            GenerateEmptyFile(Path.Combine(dataPath, "watchlist.json"));
        }

        private static void GenerateUsers(string filePath)
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.IndexFaker + 2) // Start from 2 (admin is 1)
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.IsAdmin, f => false)
                .RuleFor(u => u.CreatedAt, f => f.Date.Past(2));

            var users = faker.Generate(100);

            // Add admin user
            users.Insert(0, new User
            {
                Id = 1,
                Name = "Admin User",
                Username = "admin",
                Email = "admin@mediatracker.com",
                Password = "admin123",
                IsAdmin = true,
                CreatedAt = DateTime.Now.AddYears(-1)
            });

            WriteJsonFile(filePath, users);
        }

        private static void GenerateMovies(string filePath)
        {
            var genres = new[] { "Action", "Adventure", "Comedy", "Crime", "Drama", "Fantasy", "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller", "Western" };

            var faker = new Faker<Media>()
                .RuleFor(m => m.Id, f => f.IndexFaker + 1)
                .RuleFor(m => m.Title, f => f.Commerce.ProductName())
                .RuleFor(m => m.Type, f => "movie")
                .RuleFor(m => m.Overview, f => f.Lorem.Paragraph())
                .RuleFor(m => m.PosterUrl, f => "/placeholder.svg?height=300&width=200")
                .RuleFor(m => m.ReleaseYear, f => f.Random.Int(1990, 2023))
                .RuleFor(m => m.Genres, f => f.Random.ListItems(genres, f.Random.Int(1, 3)).ToList())
                .RuleFor(m => m.Rating, f => Math.Round(f.Random.Double(5.0, 10.0), 1))
                .RuleFor(m => m.VoteCount, f => f.Random.Int(100, 5000))
                .RuleFor(m => m.CreatedAt, f => f.Date.Past(1))
                .RuleFor(m => m.UpdatedAt, (f, m) => m.CreatedAt);

            var movies = faker.Generate(50);
            WriteJsonFile(filePath, movies);
        }

        private static void GenerateSeries(string filePath)
        {
            var genres = new[] { "Action", "Adventure", "Comedy", "Crime", "Drama", "Fantasy", "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller", "Western" };

            var faker = new Faker<Media>()
                .RuleFor(m => m.Id, f => f.IndexFaker + 1)
                .RuleFor(m => m.Title, f => f.Commerce.ProductName())
                .RuleFor(m => m.Type, f => "series")
                .RuleFor(m => m.Overview, f => f.Lorem.Paragraph())
                .RuleFor(m => m.PosterUrl, f => "/placeholder.svg?height=300&width=200")
                .RuleFor(m => m.ReleaseYear, f => f.Random.Int(1990, 2023))
                .RuleFor(m => m.Genres, f => f.Random.ListItems(genres, f.Random.Int(1, 3)).ToList())
                .RuleFor(m => m.Rating, f => Math.Round(f.Random.Double(5.0, 10.0), 1))
                .RuleFor(m => m.VoteCount, f => f.Random.Int(100, 5000))
                .RuleFor(m => m.CreatedAt, f => f.Date.Past(1))
                .RuleFor(m => m.UpdatedAt, (f, m) => m.CreatedAt);

            var series = faker.Generate(50);
            WriteJsonFile(filePath, series);
        }

        private static void GenerateEmptyFile(string filePath)
        {
            WriteJsonFile(filePath, new List<object>());
        }

        private static void WriteJsonFile<T>(string filePath, List<T> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
    }
}