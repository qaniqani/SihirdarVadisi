using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Counter
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public string ContentUrl { get; set; }
        public ContentTypes ContentType { get; set; }
        public DateTime DateTime { get; set; }
    }
}