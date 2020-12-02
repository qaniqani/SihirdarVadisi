using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;

        public SurveyService(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public void Add(Survey instance)
        {
            var db = _dbFactory();
            db.Surveys.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Survey newInstance)
        {
            var survey = GetItem(id);
            if (survey == null)
                return;

            var db = _dbFactory();

            survey.EndDate = newInstance.EndDate;
            survey.Language = newInstance.Language;
            survey.LanguageId = newInstance.LanguageId;
            survey.Question = newInstance.Question;
            survey.QuestionType = newInstance.QuestionType;
            survey.StartDate = newInstance.StartDate;
            survey.Status = newInstance.Status;
            survey.UpdatedDate = DateTime.Now;
            survey.UpdateEditorId = newInstance.UpdateEditorId;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var survey = GetItem(id);
            if (survey == null)
                return;

            var db = _dbFactory();
            db.Surveys.Remove(survey);
            db.SaveChanges();
        }

        public IList<Survey> List()
        {
            var db = _dbFactory();
            var surveys =
                db.Surveys.Where(a => a.LanguageId == _setting.LanguageId)
                    .OrderByDescending(a => a.StartDate)
                    .ToList();

            return surveys;
        }

        public IList<Survey> List(StatusTypes status)
        {
            var db = _dbFactory();
            var surveys =
                db.Surveys.Where(a => a.LanguageId == _setting.LanguageId && a.Status == status)
                    .OrderByDescending(a => a.StartDate)
                    .ToList();

            return surveys;
        }

        public Survey GetItem(int instanceId)
        {
            var db = _dbFactory();
            var survey = db.Surveys.FirstOrDefault(a => a.Id == instanceId);
            return survey;
        }

        public SurveyAndQuestionResultDto GetActiveSurvey()
        {
            var languageId = _setting.LanguageId;
            var db = _dbFactory();
            var survey = db.Surveys.OrderByDescending(a => a.Id).FirstOrDefault(a => a.Status == StatusTypes.Active && a.StartDate <= DateTime.Now && a.EndDate >= DateTime.Now && a.LanguageId == languageId);
            if (survey == null)
                return null;

            var surveyId = survey.Id;
            var answers = db.Answers.OrderBy(a => a.SequenceNumber).Where(a => a.SurveyId == surveyId && a.Status == StatusTypes.Active).ToList();
            var result = new SurveyAndQuestionResultDto
            {
                Survey = new SurveyDto
                {
                    Id = survey.Id,
                    Survey = survey.Question,
                    QuestionType = survey.QuestionType
                },
                Answers = answers.OrderBy(a => a.SequenceNumber).Select(a => new AnswerDto
                {
                    Answer = a.Response,
                    Id = a.Id,
                    Vote = a.Vote
                }).ToList()
            };

            return result;
        }

        public void SurveyUsedVote(int userId, int surveyId, int answerId)
        {
            var db = _dbFactory();
            db.UserVoteAssgns.Add(new UserVoteAssgn
            {
                AnswerId = answerId,
                SurveyId = surveyId,
                UserId = userId
            });

            db.SaveChanges();
        }

        public SurveyResultDto GetSurveyResult(int userId, int surveyId)
        {
            var db = _dbFactory();

            var survey = db.Surveys.FirstOrDefault(a => a.Id == surveyId);
            if (survey == null)
                return null;

            var totalVotes = GetTotalResponse(surveyId);

            var totalVotesCount = totalVotes.Count;

            var selectedAnswer = totalVotes.FirstOrDefault(a => a.SurveyId == surveyId && a.UserId == userId);

            var result = new SurveyResultDto
            {
                Question = survey.Question
            };

            var answers =
                db.Answers.OrderBy(a => a.SequenceNumber)
                    .Where(a => a.SurveyId == surveyId)
                    .ToList()
                    .Select(a =>
                    {
                        var totalVote = totalVotes.Count(d => d.AnswerId == a.Id);

                        var percentage = Convert.ToDecimal(0);
                        
                        if(totalVotesCount != 0)
                            percentage = (Convert.ToDecimal(totalVote)/Convert.ToDecimal(totalVotesCount))*100;

                        var checkUsed = selectedAnswer != null && (selectedAnswer.AnswerId == a.Id);

                        if (checkUsed)
                            result.Used = true;

                        var votes = new SurveyResultItemDto
                        {
                            Answer = a.Response,
                            Vote = totalVote,
                            PercentageVote = percentage,
                            Used = checkUsed
                        };
                        return votes;
                    }).ToList();

            result.Answers = answers;

            return result;
        }

        private List<UserVoteAssgn> GetTotalResponse(int surveyId)
        {
            var db = _dbFactory();
            var totalVoteCount = db.UserVoteAssgns.Where(a => a.SurveyId == surveyId).ToList();
            return totalVoteCount;
        }
    }
}