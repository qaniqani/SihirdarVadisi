using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int LanguageId { get; set; }
        public GameTypes GameType { get; set; }
        public string LanguageTag { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string Tags { get; set; }
        public string Url { get; set; }
        public int SequenceNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public int Hit { get; set; }
        public StatusTypes Status { get; set; }
    }
}