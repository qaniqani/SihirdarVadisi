using System;
using Sihirdar.DataAccessLayer;

namespace Sihirdar.Service.Draw.ServiceModel
{
    public class DefinitionListRequest
    {
        public int MemberId { get; set; }
        public string ApiKey { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.Date.AddMonths(-3);
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
        public StatusTypes Status { get; set; }
    }
}