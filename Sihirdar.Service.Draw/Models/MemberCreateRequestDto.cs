using System.ComponentModel.DataAnnotations;

namespace Sihirdar.Service.Draw.Models
{
    public class MemberCreateRequestDto
    {
        public string Title { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Site url is required.")]
        public string SiteUrl { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password again is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string Password2 { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gsm is required.")]
        public string Gsm { get; set; }

        public string ProjectDetail { get; set; }
    }
}