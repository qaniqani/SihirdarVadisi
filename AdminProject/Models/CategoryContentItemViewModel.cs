using Sihirdar.DataAccessLayer;
using System;

namespace AdminProject.Models
{
    public class CategoryContentItemViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string GameType { get; set; }
        public GameTypes GameTypes { get; set; }
        public string EditorName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string PictureName { get; set; }
    }
}