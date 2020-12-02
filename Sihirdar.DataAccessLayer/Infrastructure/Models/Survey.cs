using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Question { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatusTypes Status { get; set; }
        public int CreateEditorId { get; set; }
        public int UpdateEditorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}