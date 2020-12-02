using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public interface IAnswerService : IBaseInterface<Answer>
    {
        IList<Answer> List(int surveyId);
        IList<Answer> List(int surveyId, StatusTypes status);
    }
}