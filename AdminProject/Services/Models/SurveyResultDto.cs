using System.Collections.Generic;
using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class SurveyResultDto
    {
        public string Question { get; set; }
        public bool Used { get; set; }
        public List<SurveyResultItemDto> Answers { get; set; }
    }
}