using System.ComponentModel.DataAnnotations;

namespace Sihirdar.Service.Draw.Models
{
    public class DefinitionCreateDto
    {
        [Required(ErrorMessage = "ApiKey is required.")]
        public string ApiKey { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Detail { get; set; }

        [Required(ErrorMessage = "Win count is required.")]
        public int WinCount { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public string StartDate { get; set; }
        [Required(ErrorMessage = "End date is required.")]
        public string EndDate { get; set; }
    }
}