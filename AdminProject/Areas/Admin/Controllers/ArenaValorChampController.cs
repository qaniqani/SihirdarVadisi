using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminProject.Areas.Admin.Controllers
{
    public class ArenaValorChampController : BaseController
    {
        private readonly IArenaValorChampService _arenaService;
        private readonly RuntimeSettings _setting;

        public ArenaValorChampController(IArenaValorChampService arenaService, RuntimeSettings setting) : base(setting)
        {
            _arenaService = arenaService;
            _setting = setting;
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Add()
        {
            SetPageHeader("Arena of Valor", "Add New Arena of Valor Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(string Name, string Detail, string Url, HttpPostedFileBase Picture, StatusTypes Status)
        {
            SetPageHeader("Arena of Valor", "Add New Arena of Valor Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            var detailText = Utility.StripHtml(Detail);
            if (string.IsNullOrEmpty(detailText))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (Picture == null)
                ModelState.AddModelError("Picture", "Picture is required.");

            if (!ModelState.IsValid)
                return View();

            var url = string.IsNullOrEmpty(Url.Trim()) ? Utility.UrlSeo(Name.Trim()) : Utility.UrlSeo(Url.Trim());

            var arena = new ArenaValorChamp {
                CreateDate = DateTime.Now,
                Detail = Detail,
                Name = Name,
                Status = Status,
                Url = url
            };

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType",
                        $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension",
                        $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    arena.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }

            _arenaService.Add(arena);

            Added();

            return View();
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Edit(int id)
        {
            var arena = _arenaService.GetItem(id);
            if (arena == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            SetPageHeader("Arena of Valor", "Edit Arena of Valor Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(arena.Status);

            return View(arena);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string Name, string Detail, string Url, HttpPostedFileBase Picture, StatusTypes Status)
        {
            var arena = _arenaService.GetItem(id);
            if (arena == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            SetPageHeader("Arena of Valor", "Edit Arena of Valor Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(arena.Status);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            var detailText = Utility.StripHtml(Detail);
            if (string.IsNullOrEmpty(detailText))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (string.IsNullOrEmpty(Url))
                ModelState.AddModelError("Url", "Url is required.");

            if (!ModelState.IsValid)
                return View(arena);

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType",
                        $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension",
                        $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View(arena);

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    arena.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(arena);
                }
            }

            arena.Name = Name;
            arena.Detail = Detail;
            arena.Status = Status;
            arena.Url = Url;

            _arenaService.Edit(id, arena);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult List()
        {
            SetPageHeader("Arena of Valor", "List of Arena of Valor Champ");

            var arenas = _arenaService.List();

            return View(arenas);
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Delete(int id)
        {
            var areana = _arenaService.GetItem(id);
            if (areana == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            _arenaService.Delete(id);
            Deleted();

            return RedirectToAction("List");
        }
    }
}