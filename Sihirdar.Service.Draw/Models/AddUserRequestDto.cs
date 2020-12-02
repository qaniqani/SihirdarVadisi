using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sihirdar.Service.Draw.Models
{
    public class AddUserRequestDto
    {
        [Required(ErrorMessage = "ApiKey is required.")]
        public string ApiKey { get; set; }
        [Required(ErrorMessage = "Draw id is required.")]
        public int DrawId { get; set; }
        [Required(ErrorMessage = "Guid auto generate is required. Default value: false")]
        public bool AutoGenerateGuid { get; set; }
        [Required(ErrorMessage = "Users is not null.")]
        public List<UserViewModelDto> Users { get; set; }
    }

    public class UserViewModelDto
    {
        public string UserGuid { get; set; }
        [Required(ErrorMessage = "User name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "User email is required.")]
        public string Email { get; set; }
    }
}