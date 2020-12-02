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
    public class AovSkinController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly IAovSkinService _skinService;

        public AovSkinController(IAovSkinService skinService, RuntimeSettings setting) : base(setting)
        {
            _setting = setting;
            _skinService = skinService;
        }

        public ActionResult Add()
        {
            SetPageHeader("AOV Skin", "Add New Skin");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, int Num, HttpPostedFileBase Picture,  StatusTypes Status)
        {
            SetPageHeader("AOV Skin", "Add New Skin");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (!ModelState.IsValid)
                return View();

            var skin = new AovSkin
            {
                Name = Name,
                Num = Num,
                Status = Status
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
                    return View(skin);

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    skin.Picture = pictureName + ".jpg";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(skin);
                }
            }

            _skinService.Add(skin);

            Added();

            return RedirectToAction("Add");
        }

        public ActionResult Edit(int id)
        {
            SetPageHeader("AOV Skin", "Edit Skin");

            var skin = _skinService.Get(id);
            if (skin == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(skin.Status);

            return View(skin);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, int Num, HttpPostedFileBase Picture, StatusTypes Status)
        {
            SetPageHeader("AOV Skin", "Edit Skin");

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            var skin = _skinService.Get(id);
            if (skin == null)
                ModelState.AddModelError("SkinNotFound", "Skin was not found.");

            if (!ModelState.IsValid)
                return RedirectToAction("List");

            skin.Name = Name;
            skin.Num = Num;
            skin.Status = Status;

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
                    return View(skin);

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    skin.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(skin);
                }
            }

            _skinService.Edit(id, skin);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("AOV Skin", "Skin List");

            var list = _skinService.List();

            return View(list);
        }

        public ActionResult Delete(int id)
        {
            var skin = _skinService.Get(id);
            if (skin == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            _skinService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}