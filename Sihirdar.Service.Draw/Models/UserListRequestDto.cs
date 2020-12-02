using System.ComponentModel.DataAnnotations;

namespace Sihirdar.Service.Draw.Models
{
    public class UserListRequestDto
    {
        [Required(ErrorMessage = "ApiKey is required.")]
        public string ApiKey { get; set; }
        [Required(ErrorMessage = "Draw id is required.")]
        public int DrawId { get; set; }
    }
}