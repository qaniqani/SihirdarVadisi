using System;

namespace Sihirdar.Service.Draw.ServiceModel
{
    public class DefinitionAddRequest
    {
        public int MemberId { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public int WinCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}