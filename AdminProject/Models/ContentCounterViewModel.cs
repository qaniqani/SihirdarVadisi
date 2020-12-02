using Sihirdar.DataAccessLayer;
using System;

namespace AdminProject.Models
{
    public class ContentCounterViewModel
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public ContentTypes ContentType { get; set; }
        public string ContentUrl { get; set; }
        public string ContentSubject { get; set; }
        public bool ContentStatus { get; set; }
        public DateTime DateTime { get; set; }
    }
}