﻿using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class Content
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int LanguageId { get; set; }
        public string LanguageTag { get; set; }
        public ContentTypes ContentType { get; set; }
        public GameTypes GameType { get; set; }
        public ContentFilterTypes FilterType { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
        public string VideoUrl { get; set; }
        public bool IsShowcase { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime PublishDate { get; set; } = new DateTime(1970, 01, 01);
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public int SequenceNumber { get; set; }
        public StatusTypes Status { get; set; }
        public int Hit { get; set; } = 0;
    }
}