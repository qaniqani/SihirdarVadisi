namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Response { get; set; }
        public int Vote { get; set; } = 0;
        public int SequenceNumber { get; set; } = 9999;
        public StatusTypes Status { get; set; }
    }
}