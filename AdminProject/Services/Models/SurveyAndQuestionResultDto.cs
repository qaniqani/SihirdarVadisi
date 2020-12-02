using System.Collections.Generic;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Models
{
    public class SurveyAndQuestionResultDto
    {
        public SurveyDto Survey { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }

    public class SurveyDto
    {
        public int Id { get; set; }
        public string Survey { get; set; }
        public QuestionTypes QuestionType { get; set; }
    }

    public class AnswerDto
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public int Vote { get; set; }
    }
}