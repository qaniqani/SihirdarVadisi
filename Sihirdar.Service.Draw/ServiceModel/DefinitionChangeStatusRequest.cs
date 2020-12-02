using Sihirdar.DataAccessLayer;

namespace Sihirdar.Service.Draw.ServiceModel
{
    public class DefinitionChangeStatusRequest
    {
        public int DefinitionId { get; set; }
        public string ApiKey { get; set; }
        public StatusTypes Status { get; set; }
    }
}