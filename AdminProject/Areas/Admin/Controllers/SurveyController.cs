using System;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class SurveyController : BaseController
    {
        private readonly ISurveyService _surveyService;
        private readonly IAnswerService _answerService;
        private readonly RuntimeSettings _settings;

        public SurveyController(ISurveyService surveyService, RuntimeSettings settings, IAnswerService answerService) : base(settings)
        {
            _surveyService = surveyService;
            _settings = settings;
            _answerService = answerService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Survey", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.QuestionTypeList = DropdownTypes.GetQuestionType(QuestionTypes.Single);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Question, string StartDate, string EndDate, StatusTypes Status, QuestionTypes QuestionType)
        {
            SetPageHeader("Survey", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.QuestionTypeList = DropdownTypes.GetQuestionType(QuestionType);

            if (string.IsNullOrEmpty(Question))
                ModelState.AddModelError("Question", "Question is required.");

            if (string.IsNullOrEmpty(StartDate))
                ModelState.AddModelError("StartDate", "Start Date is required.");

            if (string.IsNullOrEmpty(EndDate))
                ModelState.AddModelError("EndDate", "End Date is required.");

            if (!ModelState.IsValid)
                return View();

            DateTime startDate;
            DateTime endDate;
            if(!Utility.DateTimeParsing(StartDate, out startDate))
                ModelState.AddModelError("StartDateFormat", "Start date format is incorrect.");

            if (!Utility.DateTimeParsing(EndDate, out endDate))
                ModelState.AddModelError("EndDateFormat", "End date format is incorrect.");

            if (!ModelState.IsValid)
                return View();

            var survey = new Survey
            {
                CreatedDate = DateTime.Now,
                CreateEditorId = Utility.SessionCheck().Id,
                EndDate = endDate,
                Language = _settings.Language,
                LanguageId = _settings.LanguageId,
                Question = Question,
                QuestionType = QuestionType,
                StartDate = startDate,
                Status = Status,
                UpdatedDate = new DateTime(1970, 1, 1),
                UpdateEditorId = 0
            };

            _surveyService.Add(survey);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Survey", "Edit");

            var survey = _surveyService.GetItem(id);
            if (survey == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(survey.Status);
            ViewBag.QuestionTypeList = DropdownTypes.GetQuestionType(survey.QuestionType);

            return View(survey);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Question, string StartDate, string EndDate, StatusTypes Status, QuestionTypes QuestionType)
        {
            SetPageHeader("Survey", "Edit");

            var survey = _surveyService.GetItem(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.QuestionTypeList = DropdownTypes.GetQuestionType(QuestionType);

            if (string.IsNullOrEmpty(Question))
                ModelState.AddModelError("Question", "Question is required.");

            if (string.IsNullOrEmpty(StartDate))
                ModelState.AddModelError("StartDate", "Start Date is required.");

            if (string.IsNullOrEmpty(EndDate))
                ModelState.AddModelError("EndDate", "End Date is required.");

            if (!ModelState.IsValid)
                return View(survey);

            DateTime startDate;
            DateTime endDate;
            if (!Utility.DateTimeParsing(StartDate, out startDate))
                ModelState.AddModelError("StartDateFormat", "Start date format is incorrect.");

            if (!Utility.DateTimeParsing(EndDate, out endDate))
                ModelState.AddModelError("EndDateFormat", "End date format is incorrect.");

            if (!ModelState.IsValid)
                return View(survey);

            survey.EndDate = endDate;
            survey.Language = _settings.Language;
            survey.LanguageId = _settings.LanguageId;
            survey.Question = Question;
            survey.QuestionType = QuestionType;
            survey.StartDate = startDate;
            survey.Status = Status;

            _surveyService.Edit(id, survey);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            _surveyService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("Survey", "List");

            var surveys = _surveyService.List();

            return View(surveys);
        }

        [HttpGet]
        public ActionResult Answer(int id, string answerId)
        {
            SetPageHeader("Question", "Answer List");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            var selectedQuestion = _surveyService.GetItem(id);
            ViewBag.Question = selectedQuestion;

            GetAnswers(id);

            if (!string.IsNullOrEmpty(answerId))
            {
                ViewBag.AnswerId = answerId;
                var answer = _answerService.GetItem(answerId.ToInt32());
                if (answer == null)
                {
                    Warning();
                    return Redirect("/Admin/Survey/Answer/" + id);
                }

                return View(answer);
            }

            return View(new Answer {Vote = 0, Status = StatusTypes.Active});
        }

        [HttpGet]
        public ActionResult AnswerDelete(int id, string answerId)
        {
            SetPageHeader("Question", "Answer List");

            if (string.IsNullOrEmpty(answerId))
            {
                Warning();
                return RedirectToAction("Answer", new { id });
            }

            _answerService.Delete(answerId.ToInt32());

            Deleted();

            return RedirectToAction("Answer", new {id});
        }

        [HttpPost]
        public ActionResult Answer(int id, string answerId, string Response, int Vote, int SequenceNumber, StatusTypes Status)
        {
            SetPageHeader("Question", "Answer List");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            var selectedQuestion = _surveyService.GetItem(id);
            ViewBag.Question = selectedQuestion;

            if (string.IsNullOrEmpty(Response))
                ModelState.AddModelError("Response", "Response is required.");

            if (!ModelState.IsValid)
            {
                GetAnswers(id);
                return View();
            }

            if (!string.IsNullOrEmpty(answerId))
            {
                var oldAnswer = _answerService.GetItem(answerId.ToInt32());
                oldAnswer.Response = Response;
                oldAnswer.SequenceNumber = SequenceNumber;
                oldAnswer.Status = Status;
                oldAnswer.Vote = Vote;

                _answerService.Edit(answerId.ToInt32(), oldAnswer);

                Updated();

                return Redirect("/Admin/Survey/Answer/" + id);
            }

            var answer = new Answer
            {
                SequenceNumber = SequenceNumber,
                Language = _settings.Language,
                LanguageId = _settings.LanguageId,
                Response = Response,
                Status = Status,
                SurveyId = id,
                Vote = Vote
            };

            _answerService.Add(answer);

            Added();

            GetAnswers(id);

            return View(new Answer {Vote = 0, Status = StatusTypes.Active});
        }

        private void GetAnswers(int id)
        {
            var answers = _answerService.List(id);
            ViewBag.Answers = answers;
        }
    }
}