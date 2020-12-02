using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class DrawMember
    {
        public int Id { get; set; }
        public string ApiKey { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SiteUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }
        public string ProjectDetail { get; set; }
        public DrawApiStatusTypes StatusType { get; set; } = DrawApiStatusTypes.Pending;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = new DateTime(1970, 1, 1);
    }
}