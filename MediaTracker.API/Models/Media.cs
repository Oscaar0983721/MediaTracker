using System;
using System.Collections.Generic;

namespace MediaTracker.API.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // "movie" or "series"
        public string Overview { get; set; }
        public string PosterUrl { get; set; }
        public int ReleaseYear { get; set; }
        public List<string> Genres { get; set; }
        public double Rating { get; set; }
        public int VoteCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}