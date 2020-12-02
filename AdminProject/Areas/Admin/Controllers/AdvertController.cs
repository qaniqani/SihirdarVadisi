using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class AdvertController : BaseController
    {
        private readonly IAdvertService _advertService;
        private readonly ICategoryService _categoryService;
        private readonly RuntimeSettings _setting;

        public AdvertController(IAdvertService advertService, RuntimeSettings setting, ICategoryService categoryService) : base(setting)
        {
            _advertService = advertService;
            _setting = setting;
            _categoryService = categoryService;
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Add()
        {
            SetPageHeader("Advert", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.AdvertLocationList = DropdownTypes.GetAdvertLocationType(AdvertLocationTypes.Header);
            ViewBag.AdvertList = DropdownTypes.GetAdvertType(AdvertTypes.Adsense);

            GetCategories();

            return
                View(new Advert
                {
                    AdGuid = Guid.NewGuid().ToString(),
                    AdLocation = AdvertLocationTypes.Header,
                    AdType = AdvertTypes.Adsense,
                    EndDate = DateTime.Now.AddMonths(3),
                    SequenceNr = 9999,
                    Status = StatusTypes.Active,
                    StartDate = DateTime.Now
                });
        }

        [HttpPost]
        [AdminAuth]
        [ValidateInput(false)]
        public ActionResult Add(int CategoryId, string Name, string AdGuid, AdvertLocationTypes AdLocation, AdvertTypes AdType, string AdCode, string AdLink, HttpPostedFileBase AdFilePath, string StartDate, string EndDate, int SequenceNr, StatusTypes Status)
        {
            SetPageHeader("Advert", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.AdvertLocationList = DropdownTypes.GetAdvertLocationType(AdLocation);
            ViewBag.AdvertList = DropdownTypes.GetAdvertType(AdType);

            GetCategories();

            if (AdLocation == AdvertLocationTypes.Embedded || AdLocation == AdvertLocationTypes.RightBlock || AdLocation == AdvertLocationTypes.SiteMiddle)
                if (CategoryId == 0)
                    ModelState.AddModelError("Category", "Category is required.");

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(AdGuid))
                ModelState.AddModelError("AdGuid", "Guid is required.");

            if (string.IsNullOrEmpty(StartDate))
                ModelState.AddModelError("StartDate", "Start Date is required.");

            if (string.IsNullOrEmpty(EndDate))
                ModelState.AddModelError("EndDate", "End Date is required.");

            if (AdType == AdvertTypes.AdMatic || AdType == AdvertTypes.Adsense || AdType == AdvertTypes.Embedded)
            {
                if (string.IsNullOrEmpty(AdCode))
                    ModelState.AddModelError("AdCode", "Embedded code is required.");
            }
            else if (AdType == AdvertTypes.Image)
            {
                if (string.IsNullOrEmpty(AdLink))
                    ModelState.AddModelError("AdLink", "Link is required.");

                if (AdFilePath == null)
                    ModelState.AddModelError("AdFilePath", "File is required.");
            }

            if (!ModelState.IsValid)
                return View();

            DateTime sDate, eDate;
            if (!Utility.DateTimeParsing(StartDate, out sDate))
                ModelState.AddModelError("StartDate", "Start Date format is incorrect.");

            if (!Utility.DateTimeParsing(EndDate, out eDate))
                ModelState.AddModelError("EndDate", "End Date format is incorrect.");

            if (!ModelState.IsValid)
                return View();

            string filePath = "", contentType = "";
            if (AdType == AdvertTypes.Image)
            {
                var fileName = AdFilePath.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.FileExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", $"Only upload types: {string.Join(",", _setting.FileExtensionTypes)}");

                if (!_setting.FileMimeTypes.Contains(AdFilePath.ContentType))
                    ModelState.AddModelError("MimeType", $"Only {string.Join(", ", _setting.FileMimeTypes)} mime type upload.");

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo($"{Name}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/File/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(AdFilePath, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    filePath = pictureName + extension;
                    contentType = AdFilePath.ContentType;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("FileUploadError", ex.Message);
                    return View();
                }
            }

            var advert = new Advert
            {
                CategoryId = CategoryId,
                AdCode = AdCode,
                AdFilePath = filePath,
                AdFileType = contentType,
                AdType = AdType,
                AdGuid = AdGuid,
                AdLink = AdLink,
                AdLocation = AdLocation,
                ClickHit = 0,
                CreatedDate = DateTime.Now,
                CreateEditorId = Utility.SessionCheck().Id,
                EndDate = eDate,
                StartDate = sDate,
                Language = _setting.Language,
                LanguageId = _setting.LanguageId,
                Name = Name,
                SequenceNr = SequenceNr,
                Status = Status,
                UpdatedDate = new DateTime(1970, 1, 1),
                UpdateEditorId = 0,
                ViewHit = 0
            };

            _advertService.Add(advert);

            Added();

            return
                View(new Advert
                {
                    AdGuid = Guid.NewGuid().ToString(),
                    AdLocation = AdvertLocationTypes.Header,
                    AdType = AdvertTypes.Adsense,
                    EndDate = DateTime.Now.AddMonths(3),
                    SequenceNr = 9999,
                    Status = StatusTypes.Active,
                    StartDate = DateTime.Now
                });
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Advert", "Edit");

            var advert = _advertService.GetItem(id);
            if (advert == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            GetCategories();

            ViewBag.StatusList = DropdownTypes.GetStatus(advert.Status);
            ViewBag.AdvertLocationList = DropdownTypes.GetAdvertLocationType(advert.AdLocation);
            ViewBag.AdvertList = DropdownTypes.GetAdvertType(advert.AdType);

            return View(advert);
        }

        [HttpPost]
        [AdminAuth]
        [ValidateInput(false)]
        public ActionResult Edit(int CategoryId, int id, string Name, string AdGuid, AdvertLocationTypes AdLocation, AdvertTypes AdType, string AdCode, string AdLink, HttpPostedFileBase AdFilePath, string StartDate, string EndDate, int SequenceNr, StatusTypes Status)
        {
            SetPageHeader("Advert", "Edit");

            var advert = _advertService.GetItem(id);
            if (advert == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            GetCategories();

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.AdvertLocationList = DropdownTypes.GetAdvertLocationType(AdLocation);
            ViewBag.AdvertList = DropdownTypes.GetAdvertType(AdType);

            if (AdLocation == AdvertLocationTypes.Embedded || AdLocation == AdvertLocationTypes.RightBlock || AdLocation == AdvertLocationTypes.SiteMiddle)
                if (CategoryId == 0)
                    ModelState.AddModelError("Category", "Category is required.");

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(AdGuid))
                ModelState.AddModelError("AdGuid", "Guid is required.");

            if (string.IsNullOrEmpty(StartDate))
                ModelState.AddModelError("StartDate", "Start Date is required.");

            if (string.IsNullOrEmpty(EndDate))
                ModelState.AddModelError("EndDate", "End Date is required.");

            if (AdType == AdvertTypes.AdMatic || AdType == AdvertTypes.Adsense || AdType == AdvertTypes.Embedded)
            {
                if (string.IsNullOrEmpty(AdCode))
                    ModelState.AddModelError("AdCode", "Embedded code is required.");
            }
            else if (AdType == AdvertTypes.Image)
            {
                if (string.IsNullOrEmpty(AdLink))
                    ModelState.AddModelError("AdLink", "Link is required.");

                if (AdFilePath == null)
                    ModelState.AddModelError("AdFilePath", "File is required.");
            }

            if (!ModelState.IsValid)
                return View();

            DateTime sDate, eDate;
            if (!Utility.DateTimeParsing(StartDate, out sDate))
                ModelState.AddModelError("StartDate", "Start Date format is incorrect.");

            if (!Utility.DateTimeParsing(EndDate, out eDate))
                ModelState.AddModelError("EndDate", "End Date format is incorrect.");

            if (!ModelState.IsValid)
                return View();

            advert.CategoryId = CategoryId;
            advert.AdCode = AdCode;
            advert.AdType = AdType;
            advert.AdGuid = AdGuid;
            advert.AdLink = AdLink;
            advert.AdLocation = AdLocation;
            advert.EndDate = eDate;
            advert.StartDate = sDate;
            advert.Language = _setting.Language;
            advert.LanguageId = _setting.LanguageId;
            advert.Name = Name;
            advert.SequenceNr = SequenceNr;
            advert.Status = Status;
            advert.UpdateEditorId = Utility.SessionCheck().Id;

            if (AdType == AdvertTypes.Image)
            {
                var fileName = AdFilePath.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.FileExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", $"Only upload types: {string.Join(",", _setting.FileExtensionTypes)}");

                if (!_setting.FileMimeTypes.Contains(AdFilePath.ContentType))
                    ModelState.AddModelError("MimeType", $"Only {string.Join(", ", _setting.FileMimeTypes)} mime type upload.");

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo($"{Name}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/File/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(AdFilePath, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    advert.AdFilePath = pictureName + extension;
                    advert.AdFileType = AdFilePath.ContentType;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("FileUploadError", ex.Message);
                    return View();
                }
            }

            _advertService.Edit(id, advert);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Delete(int id)
        {
            _advertService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult List()
        {
            SetPageHeader("Advert", "List");

            var adverts = _advertService.List();

            return View(adverts);
        }

        public void GetCategories()
        {
            var treeView = _categoryService.GetCategories(CategoryTypes.All, StatusTypes.Active);
            ViewBag.Menu = treeView;
        }
    }
}