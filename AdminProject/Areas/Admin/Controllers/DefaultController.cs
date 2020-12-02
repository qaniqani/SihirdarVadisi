using System.Linq;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class DefaultController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly ILanguageService _languageService;
        public DefaultController(RuntimeSettings setting, ILanguageService languageService) : base(setting)
        {
            _setting = setting;
            _languageService = languageService;
        }

        public ActionResult Index()
        {
            SetPageHeader("Dashboard", "");

            return View();
        }

        [ChildActionOnly]
        public ActionResult LoadLanguages()
        {
            var languages = _languageService.ActiveList();

            if (!languages.Any())
            {
                TempData["Warning"] = "Before you add the language!!!";
            }

            ViewData["SelectedLanguage"] = _setting.Language;

            return PartialView("../Partial/Language", languages);
        }

        [ChildActionOnly]
        public ActionResult Authorizations()
        {
            if (Utility.SessionCheck() == null)
                return PartialView("../Partial/LeftMenu");

            var authorization = Utility.SessionCheck().Authorization;
            ViewData["Authorizations"] = authorization;

            return PartialView("../Partial/LeftMenu", authorization);
        }
    }
}