using System.ComponentModel.DataAnnotations;
using Sihirdar.DataAccessLayer;

namespace Sihirdar.Service.Draw.Models
{
    public class DrawStatusChangeRequestDto
    {
        [Required(ErrorMessage = "Definition id is required.")]
        public int DefinitionId { get; set; }
        [Required(ErrorMessage = "ApiKey is required.")]
        public string ApiKey { get; set; }
        [Required(ErrorMessage = "Status is required. Allowed types: Active - 1, Deactive - 0, Delete = 2")]
        public StatusTypes Status { get; set; } = StatusTypes.Active;
    }
}