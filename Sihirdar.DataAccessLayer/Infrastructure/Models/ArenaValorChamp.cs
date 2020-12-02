using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class ArenaValorChamp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public StatusTypes Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
