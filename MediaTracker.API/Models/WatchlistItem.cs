using System;

namespace MediaTracker.API.Models
{
    public class WatchlistItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public string MediaType { get; set; }
        public DateTime AddedAt { get; set; }
    }
}