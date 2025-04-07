using System;

namespace MediaTracker.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int MediaId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}