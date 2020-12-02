using Sihirdar.DataAccessLayer;
using System;

namespace AdminProject.Models
{
    public class ContentViewModel
    {
        public int Id { get; set; }
        public GameTypes GameTypes { get; set; }
        public string GameType { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string Url { get; set; }
        public string[] Tags { get; set; }
        public string EmbeddedCode { get; set; }
        public bool IsShowcase { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public int Hit { get; set; } = 0;
    }
}