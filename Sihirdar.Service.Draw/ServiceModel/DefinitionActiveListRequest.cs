using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihirdar.Service.Draw.ServiceModel
{
    public class DefinitionActiveListRequest
    {
        public int MemberId { get; set; }
        public string ApiKey { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.Date.AddMonths(-3);
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
    }
}