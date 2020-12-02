using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class DrawDefinition
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public int WinCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusTypes Status { get; set; } = StatusTypes.Active;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
