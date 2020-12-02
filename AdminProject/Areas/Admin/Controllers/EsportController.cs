using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class EsportController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly IEsportCalendarService _esportService;

        public EsportController(IEsportCalendarService esportService, RuntimeSettings setting) : base(setting)
        {
            _esportService = esportService;
            _setting = setting;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("E-Sport Calendar", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.GameList = DropdownTypes.GetGameType("lol-color");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(string Name, HttpPostedFileBase Picture, string Description, string StartDateTime, string Color, StatusTypes Status)
        {
            SetPageHeader("E-Sport Calendar", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.GameList = DropdownTypes.GetGameType(Color);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Description))
                ModelState.AddModelError("Description", "Description is required.");

            if (string.IsNullOrEmpty(StartDateTime))
                ModelState.AddModelError("StartDateTime", "Start date is required.");

            if (string.IsNullOrEmpty(Color))
                ModelState.AddModelError("Color", "Color is required.");

            if (!ModelState.IsValid)
                return View();

            DateTime startDate;
            if (!Utility.DateTimeParsing(StartDateTime, out startDate))
            {
                ModelState.AddModelError("StartDateTimeParsing", "Start date format is incorrect. Sample format: dd.MM.yyyy HH:mm");
                return View();
            }

            var esport = new EsportCalendar
            {
                CreatedDate = DateTime.Now,
                Color = Color,
                CreatedEditorId = Utility.SessionCheck().Id,
                Description = Description,
                Language = _setting.Language,
                LanguageId = _setting.LanguageId,
                Name = Name,
                StartDateTime = startDate,
                Status = Status
            };

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType", $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    esport.Picture = pictureName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }

            _esportService.Add(esport);

            Added();

            return RedirectToAction("Add");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("E-Sport Calendar", "Edit");

            var esport = _esportService.GetItem(id);
            if (esport == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(esport.Status);
            ViewBag.GameList = DropdownTypes.GetGameType(esport.Color);

            return View(esport);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string Name, HttpPostedFileBase Picture, string Description, string StartDateTime, string Color, StatusTypes Status)
        {
            SetPageHeader("E-Sport Calendar", "Edit");

            var esport = _esportService.GetItem(id);
            if (esport == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.GameList = DropdownTypes.GetGameType(Color);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Description))
                ModelState.AddModelError("Description", "Description is required.");

            if (string.IsNullOrEmpty(StartDateTime))
                ModelState.AddModelError("StartDateTime", "Start date is required.");

            if (string.IsNullOrEmpty(Color))
                ModelState.AddModelError("Color", "Color is required.");

            if (!ModelState.IsValid)
                return View(esport);

            DateTime startDate;
            if (!Utility.DateTimeParsing(StartDateTime, out startDate))
            {
                ModelState.AddModelError("StartDateTimeParsing", "Start date format is incorrect. Sample format: dd.MM.yyyy HH:mm");
                return View(esport);
            }

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType", $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View(esport);

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    esport.Picture = pictureName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(esport);
                }
            }

            esport.Color = Color;
            esport.Description = Description;
            esport.Language = _setting.Language;
            esport.LanguageId = _setting.LanguageId;
            esport.Name = Name;
            esport.StartDateTime = startDate;
            esport.Status = Status;
            esport.UpdatedEditorId = Utility.SessionCheck().Id;

            _esportService.Edit(id, esport);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("E-Sport Calendar", "List");

            var esports = _esportService.List();

            return View(esports);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var esport = _esportService.GetItem(id);
            if (esport == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            _esportService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}