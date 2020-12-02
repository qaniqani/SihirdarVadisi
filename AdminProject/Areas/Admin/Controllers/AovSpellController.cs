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
    public class AovSpellController : BaseController
    {
        private readonly IAovSpellService _spellService;
        private readonly RuntimeSettings _setting;

        public AovSpellController(RuntimeSettings setting, IAovSpellService spellService) : base(setting)
        {
            _setting = setting;
            _spellService = spellService;
        }

        public ActionResult Add()
        {
            SetPageHeader("AOV Spell", "Add New Spell");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, int Num, string Description, HttpPostedFileBase Image, StatusTypes Status)
        {
            SetPageHeader("AOV Spell", "Add New Spell");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (Image == null)
                ModelState.AddModelError("Image", "Image is required.");

            if (!ModelState.IsValid)
                return View();

            var spell = new AovSpell
            {
                Description = Description,
                Name = Name,
                Num = Num,
                Status = Status
            };

            var fileName = Image.FileName;
            var extension = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(extension))
                ModelState.AddModelError("Extension", "Image extension not found.");

            if (!_setting.PictureMimeType.Contains(Image.ContentType))
                ModelState.AddModelError("MimeType",
                    $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

            if (!_setting.PictureExtensionTypes.Contains(extension))
                ModelState.AddModelError("Extension",
                    $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

            if (!ModelState.IsValid)
                return View();

            var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
            var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
            try
            {
                Utility.FileUpload(Image, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                spell.Image = pictureName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("PictureUploadError", ex.Message);
                return View();
            }

            _spellService.Add(spell);

            Added();

            return RedirectToAction("Add");
        }

        public ActionResult Edit(int id)
        {
            SetPageHeader("AOV Spell", "Edit Spell");

            var spell = _spellService.Get(id);
            if (spell == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(spell.Status);

            return View(spell);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, int Num, string Description, HttpPostedFileBase Image, StatusTypes Status)
        {
            var spell = _spellService.Get(id);
            if (spell == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            SetPageHeader("AOV Spell", "Edit Spell");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (!ModelState.IsValid)
                return View(spell);

            if (Image != null)
            {
                var fileName = Image.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "Image extension not found.");

                if (!_setting.PictureMimeType.Contains(Image.ContentType))
                    ModelState.AddModelError("MimeType",
                        $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension",
                        $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View(spell);

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
                try
                {
                    Utility.FileUpload(Image, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    spell.Image = pictureName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(spell);
                }
            }

            spell.Description = Description;
            spell.Name = Name;
            spell.Num = Num;
            spell.Status = Status;

            _spellService.Edit(id, spell);

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("AOV Spell", "List Spell");

            var list = _spellService.List();

            return View(list);
        }

        public ActionResult Delete(int id)
        {
            var spell = _spellService.Get(id);
            if (spell == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            _spellService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}