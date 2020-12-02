using System;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class PromiseDayController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly IPromiseDayService _promiseService;

        public PromiseDayController(IPromiseDayService promiseService, RuntimeSettings setting) : base(setting)
        {
            _promiseService = promiseService;
            _setting = setting;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Promise", "Add New");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Promise, string Teller, string PublishDate, StatusTypes Status)
        {
            SetPageHeader("Promise", "Add New");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(Promise))
                ModelState.AddModelError("Promise", "Promise is required.");

            if (string.IsNullOrEmpty(Teller))
                ModelState.AddModelError("Teller", "Teller is required.");

            if (string.IsNullOrEmpty(PublishDate))
                ModelState.AddModelError("PublishDate", "Publish Date is required.");

            if (!ModelState.IsValid)
                return View();

            var publishDate = new DateTime();
            if (!Utility.DateTimeParsing(PublishDate, out publishDate))
                ModelState.AddModelError("PublishDateFormat", "Publish date format is incorrect.");

            if (!ModelState.IsValid)
                return View();

            var promise = new PromiseDay
            {
                CreatedDate = DateTime.Now,
                CreateEditorId = Utility.SessionCheck().Id,
                Language = _setting.Language,
                LanguageId = _setting.LanguageId,
                Promise = Promise,
                PublishDate = publishDate,
                Status = Status,
                Teller = Teller
            };

            _promiseService.Add(promise);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Promise", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            var promise = _promiseService.GetItem(id);
            if (promise == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            return View(promise);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Promise, string Teller, string PublishDate, StatusTypes Status)
        {
            SetPageHeader("Promise", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            var p = _promiseService.GetItem(id);
            if (p == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            if (string.IsNullOrEmpty(Promise))
                ModelState.AddModelError("Promise", "Promise is required.");

            if (string.IsNullOrEmpty(Teller))
                ModelState.AddModelError("Teller", "Teller is required.");

            if (string.IsNullOrEmpty(PublishDate))
                ModelState.AddModelError("PublishDate", "Publish Date is required.");

            if (!ModelState.IsValid)
                return View(p);

            var publishDate = new DateTime();
            if (!Utility.DateTimeParsing(PublishDate, out publishDate))
                ModelState.AddModelError("PublishDateFormat", "Publish date format is incorrect.");

            if (!ModelState.IsValid)
                return View(p);

            p.Language = _setting.Language;
            p.LanguageId = _setting.LanguageId;
            p.Promise = Promise;
            p.PublishDate = publishDate;
            p.Status = Status;
            p.Teller = Teller;
            p.UpdateDate = DateTime.Now;
            p.UpdateEditorId = Utility.SessionCheck().Id;

            _promiseService.Edit(id, p);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Promise", "List");

            var promises = _promiseService.List();

            return View(promises);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _promiseService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}