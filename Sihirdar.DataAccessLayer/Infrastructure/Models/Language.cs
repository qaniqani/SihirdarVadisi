using System.ComponentModel.DataAnnotations;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class Language
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tag is required.")]
        public string UrlTag { get; set; }
        public StatusTypes Status { get; set; }
    }
}