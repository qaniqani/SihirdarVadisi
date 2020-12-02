using System;
using System.ComponentModel.DataAnnotations;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Areas.Admin.Models
{
    public class ContentSearchRequestDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public GameTypes GameType { get; set; } = GameTypes.All;
        public StatusTypes Status { get; set; } = StatusTypes.Active;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? CreateDate { get; set; }
        public ContentTypes ContentType { get; set; } = ContentTypes.Content;
        public bool IsShowcase { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 20;
    }
}