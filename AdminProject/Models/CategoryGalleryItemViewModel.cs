using System.Collections.Generic;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Models
{
    public class CategoryGalleryItemViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int LikedCount { get; set; } = 0;
        public string Subject { get; set; }
        public string GameType { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public GameTypes GameTypes { get; set; }
        public ContentTypes ContentType { get; set; }
        public List<GalleryDetailItemDto> Pictures { get; set; }
    }
}