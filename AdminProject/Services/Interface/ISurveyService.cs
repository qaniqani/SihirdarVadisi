using System.Collections.Generic;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public interface ISurveyService : IBaseInterface<Survey>
    {
        IList<Survey> List(StatusTypes status);
        SurveyAndQuestionResultDto GetActiveSurvey();
        void SurveyUsedVote(int userId, int surveyId, int answerId);
        SurveyResultDto GetSurveyResult(int userId, int surveyId);
    }
}