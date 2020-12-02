using Sihirdar.DataAccessLayer;

namespace Sihirdar.Service.Draw.Models
{
    public class MemberCreateResultDto
    {
        public string ApiKey { get; set; }
        public DrawApiStatusTypes Status { get; set; }
    }
}