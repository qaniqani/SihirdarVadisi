using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Attributes;
using AdminProject.Services.Interface;
using Ninject;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Areas.Admin.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly IKernel _kernel;
        private readonly ILanguageService _languageService;
        public LanguageController(IKernel kernel, RuntimeSettings setting, ILanguageService languageService) : base(setting)
        {
            _kernel = kernel;
            _languageService = languageService;
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Add()
        {
            SetPageHeader("Language", "Add New Language");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Add(Language language)
        {
            SetPageHeader("Language", "Add New Language");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (language.Name.Length > 200 || language.Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 200));

            if (language.UrlTag.Length > 10 || language.UrlTag.Length < 2)
                ModelState.AddModelError("UrlTagLength", string.Format("At least {1} {0} can be max {2} characters.", "Url Tag", 2, 10));

            if (!ModelState.IsValid)
                return View(language);

            var languageCheck = _languageService.GetItem(language.UrlTag);
            if(languageCheck != null)
                ModelState.AddModelError("UrlTagIsMatch", "Available languages with the same label.");

            if (!ModelState.IsValid)
                return View(language);

            _languageService.Add(language);

            Added();

            return View();
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Language", "Edit Language");

            var lang = _languageService.GetItem(id);
            if (lang == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(lang.Status);

            return View(lang);
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Edit(Language language)
        {
            SetPageHeader("Language", "Edit Language");

            ViewBag.StatusList = DropdownTypes.GetStatus(language.Status);

            if (language.Name.Length > 200 || language.Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 200));

            if (language.UrlTag.Length > 10 || language.UrlTag.Length < 2)
                ModelState.AddModelError("UrlTagLength", string.Format("At least {1} {0} can be max {2} characters.", "Url Tag", 2, 10));

            if (!ModelState.IsValid)
                return View(language);

            var id = language.Id;
            
            var lang = _languageService.GetItem(id);
            if (lang == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            var languageCheck = _languageService.GetItem(id, language.UrlTag);
            if (languageCheck != null)
                ModelState.AddModelError("UrlTagIsMatch", "Available languages with the same label.");

            if (!ModelState.IsValid)
                return View(language);

            _languageService.Edit(id, language);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Delete(int id)
        {
            _languageService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }

        [AdminAuth]
        public ActionResult List()
        {
            SetPageHeader("Language", "All Languages");

            var languages = _languageService.List();

            return View(languages);
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Change(string language)
        {
            var selectedLanguage = _languageService.GetItem(language);

            if (selectedLanguage == null)
            {
                TempData["Warning"] = "Selected language not found.";
                return Redirect("/Admin/Default");
            }

            _kernel.Get<RuntimeSettings>().Language = selectedLanguage.UrlTag;
            _kernel.Get<RuntimeSettings>().LanguageId = selectedLanguage.Id;

            return Redirect("/Admin/Default");
        }
    }
}