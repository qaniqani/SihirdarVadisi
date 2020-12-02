using System.ComponentModel.DataAnnotations;

namespace Sihirdar.Service.Draw.Models
{
    public class UserWinRequestDto
    {
        [Required(ErrorMessage = "ApiKey is required.")]
        public string ApiKey { get; set; }
        [Required(ErrorMessage = "User guid is required.")]
        public string UserGuid { get; set; }
    }
}