using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AnswerService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Answer instance)
        {
            var db = _dbFactory();
            db.Answers.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Answer newInstance)
        {
            var db = _dbFactory();
            var answer = GetItem(id);
            if (answer == null)
                return;

            answer.Language = newInstance.Language;
            answer.LanguageId = newInstance.LanguageId;
            answer.Response = newInstance.Response;
            answer.Status = newInstance.Status;
            answer.Vote = newInstance.Vote;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var answer = GetItem(id);
            db.Answers.Remove(answer);
            db.SaveChanges();
        }

        public IList<Answer> List()
        {
            var db = _dbFactory();
            var answers = db.Answers.ToList();
            return answers;
        }

        public IList<Answer> List(int surveyId)
        {
            var db = _dbFactory();
            var answers = db.Answers.OrderBy(a => a.SequenceNumber).Where(a => a.SurveyId == surveyId).ToList();
            return answers;
        }

        public IList<Answer> List(int surveyId, StatusTypes status)
        {
            var db = _dbFactory();
            var answers = db.Answers.Where(a => a.Status == status && a.SurveyId == surveyId).ToList();
            return answers;
        }

        public Answer GetItem(int instanceId)
        {
            var db = _dbFactory();
            var answer = db.Answers.FirstOrDefault(a => a.Id == instanceId);
            return answer;
        }
    }
}