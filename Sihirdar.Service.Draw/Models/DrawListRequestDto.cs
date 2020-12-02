using System.ComponentModel.DataAnnotations;
using Sihirdar.DataAccessLayer;

namespace Sihirdar.Service.Draw.Models
{
    public class DrawListRequestDto
    {
        [Required(ErrorMessage = "ApiKey is required.")]
        public string ApiKey { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public StatusTypes Status { get; set; } = StatusTypes.Active;
    }
}