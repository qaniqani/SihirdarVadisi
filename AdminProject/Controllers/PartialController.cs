using System.Linq;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    public class PartialController : Controller
    {
        private readonly ISurveyService _surveyService;
        private readonly ICategoryService _categoryService;
        private readonly IContentService _contentService;
        private readonly RuntimeSettings _settings;
        private readonly ILiveBroadcastService _streamService;

        public PartialController(ICategoryService categoryService, IContentService contentService, RuntimeSettings settings, ISurveyService surveyService, ILiveBroadcastService streamService)
        {
            _categoryService = categoryService;
            _contentService = contentService;
            _settings = settings;
            _surveyService = surveyService;
            _streamService = streamService;
        }

        [ChildActionOnly]
        public ActionResult GetMenus()
        {
            var categories = _categoryService.ActiveCategoryList(_settings.Language);
            var treeView = Utility.CreateTree(categories).ToList();

            return PartialView("../Partial/Category", treeView);
        }

        [ChildActionOnly]
        public ActionResult GetFooterCategory()
        {
            var categories = _categoryService.ActiveCategoryList(_settings.Language);
            var treeView = Utility.CreateTree(categories).Take(5).ToList();

            return PartialView("../Partial/FooterCategory", treeView);
        }

        [ChildActionOnly]
        public ActionResult GetTopContent()
        {
            var categoryUrl = TempData["CategoryUrl"].ToString();

            var contents = _contentService.GetTopContent(categoryUrl);

            return PartialView("../Partial/TopContent", contents);
        }

        [ChildActionOnly]
        public ActionResult GetSurvey()
        {
            var currentSurvey = _surveyService.GetActiveSurvey();

            return PartialView("../Partial/Survey", currentSurvey);
        }

        [ChildActionOnly]
        public ActionResult GetLiveStream()
        {
            var streams = _streamService.GetActiveStream();

            return PartialView("../Partial/Stream", streams);
        }
    }
}