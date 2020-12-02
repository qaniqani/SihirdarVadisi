using System;

namespace AdminProject.Models
{
    public class RssViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryUrl { get; set; }
        public string ContentUrl { get; set; }
        public string Picture { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}