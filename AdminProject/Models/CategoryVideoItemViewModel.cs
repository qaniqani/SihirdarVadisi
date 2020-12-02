using Sihirdar.DataAccessLayer;
using System;

namespace AdminProject.Models
{
    public class CategoryVideoItemViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int LikedCount { get; set; } = 0;
        public string Subject { get; set; }
        public string GameType { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string Url { get; set; }
        public string VideoUrl { get; set; }
        public string Picture { get; set; }
        public ContentFilterTypes FilterTypes { get; set; }
        public GameTypes GameTypes { get; set; }
        public ContentTypes ContentType { get; set; }
    }
}