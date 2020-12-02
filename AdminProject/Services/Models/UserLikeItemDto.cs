using System;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Models
{
    public class UserLikeItemDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public DateTime DateTime { get; set; }
        public GameTypes GameType { get; set; }
        public ContentTypes ContentType { get; set; }
        public string GameTypeText { get; set; }
    }
}