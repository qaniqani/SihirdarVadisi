using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class DrawUser
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int DrawId { get; set; }
        public string UserGuid { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Win { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}