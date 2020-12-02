namespace AdminProject.Models
{
    public class SurveyResultItemDto
    {
        public string Answer { get; set; }
        public int Vote { get; set; }
        public decimal PercentageVote { get; set; }
        public bool Used { get; set; }
    }
}