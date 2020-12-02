using Sihirdar.DataAccessLayer;
using System;

namespace AdminProject.Models
{
    public class CategoryGalleryViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int CategoryId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public GameTypes GameType { get; set; }
        public ContentTypes ContentType { get; set; }
        public string GameTypeText { get; set; }
        public DateTime CreateDate { get; set; }
    }
}