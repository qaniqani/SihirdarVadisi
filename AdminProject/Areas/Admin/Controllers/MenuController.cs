using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly ICategoryService _categoryService;

        public MenuController(RuntimeSettings setting, ICategoryService categoryService) : base(setting)
        {
            _setting = setting;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Menu", "Add New Menu");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(CategoryTypes.Story);
            ViewBag.CategoryTagTypeList = DropdownTypes.GetCategoryTagType(CategoryTagTypes.Normal);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, HttpPostedFileBase Picture, string Title, string Description, string Keyword,  string Url, StatusTypes Status, CategoryTypes CategoryType, CategoryTagTypes CategoryTagType)
        {
            SetPageHeader("Menu", "Add New Menu");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(CategoryType);
            ViewBag.CategoryTagTypeList = DropdownTypes.GetCategoryTagType(CategoryTagType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (!ModelState.IsValid)
            {
                GetCategories();
                return View();
            }

            var category = new Category
            {
                CategoryTagType = CategoryTagType,
                CategoryType = CategoryType,
                CreateDate = DateTime.Now,
                Description = string.IsNullOrEmpty(Description) ? Name : Description,
                Hit = 0,
                Keyword = string.IsNullOrEmpty(Keyword) ? Name : Keyword,
                LanguageId = _setting.LanguageId,
                LanguageTag = _setting.Language,
                ModifiedDate = new DateTime(1970, 1, 1),
                Name = Name,
                ParentId = 0,
                SequenceNumber = 9999,
                Status = Status,
                Title = string.IsNullOrEmpty(Title) ? Name : Title,
                Url = Utility.UrlSeo(Name),
                CreateUser = Utility.SessionCheck().Id
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
                {
                    GetCategories();
                    return View();
                }

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    category.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    GetCategories();
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }

            _categoryService.Add(category);

            GetCategories();

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Menu", "Edit Menu");

            GetCategories();

            var cat = _categoryService.GetItem(id);
            if (cat == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(cat.Status);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(cat.CategoryType);
            ViewBag.CategoryTagTypeList = DropdownTypes.GetCategoryTagType(cat.CategoryTagType);

            return View(cat);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, StatusTypes Status, CategoryTypes CategoryType, CategoryTagTypes CategoryTagType)
        {
            var cat = _categoryService.GetItem(id);

            if (cat == null)
            {
                ModelState.AddModelError("CategoryNotFound", "Category was not found.");
                return RedirectToAction("List");
            }

            SetPageHeader("Menu", "Edit Menu");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(CategoryType);
            ViewBag.CategoryTagTypeList = DropdownTypes.GetCategoryTagType(CategoryTagType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (!ModelState.IsValid)
            {
                GetCategories();
                return View(cat);
            }

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
                {
                    GetCategories();
                    return View(cat);
                }

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    cat.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    GetCategories();
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(cat);
                }
            }

            cat.CategoryTagType = CategoryTagType;
            cat.CategoryType = CategoryType;
            cat.Description = Description;
            cat.Keyword = Keyword;
            cat.ModifiedDate = DateTime.Now;
            cat.Name = Name;
            cat.Status = Status;
            cat.Title = Title;
            cat.Url = Url;
            cat.Status = Status;
            cat.LanguageId = _setting.LanguageId;
            cat.LanguageTag = _setting.Language;
            cat.ModifiedUser = Utility.SessionCheck().Id;

            _categoryService.Edit(id, cat);

            GetCategories();

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var cat = _categoryService.GetItem(id);
            if (cat == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            _categoryService.Delete(id);
            Deleted();

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult OrderMenu(List<SortedMenu> order)
        {
            if (order == null || !order.Any()) return Json(order, JsonRequestBehavior.AllowGet);

            _categoryService.OrderCategory(order);

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            SetPageHeader("Menu", "All Menus");

            var categories = _categoryService.ListCategory();

            return View(categories);
        }

        public ActionResult Order()
        {
            SetPageHeader("Menu", "Menu Order");

            GetCategories();

            return View();
        }

        public void GetCategories()
        {
            var menus = _categoryService.GetCategories(CategoryTypes.All, StatusTypes.Active);
            ViewBag.Menu = menus;
        }
    }
}