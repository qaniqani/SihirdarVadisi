using System;

namespace Sihirdar.Service.Draw.ServiceModel
{
    public class DefinitionListResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public int WinCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}