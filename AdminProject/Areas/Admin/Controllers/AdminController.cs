using System;
using System.IO;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using System.Linq;
using System.Web;
using AdminProject.Attributes;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;

        public AdminController(Func<AdminDbContext> dbFactory, RuntimeSettings setting) : base(setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Add()
        {
            SetPageHeader("Admin", "Add New Admin");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.AdminTypeList = DropdownTypes.GetAdminType(AdminTypes.Writer);

            return View();
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Add(string Name, string Username, string Password, string Password2, string MasterMenu, string MediaMenu, string SettingMenu, string Facebook, string Motto, string Twitter, StatusTypes Status, AdminTypes AdminType, HttpPostedFileBase Picture)
        {
            SetPageHeader("Admin", "Add New Admin");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.AdminTypeList = DropdownTypes.GetAdminType(AdminType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Username))
                ModelState.AddModelError("Username", "Username is required.");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError("Password", "Password is required.");

            if (string.IsNullOrEmpty(Password2))
                ModelState.AddModelError("PasswordAgain", "Password again is required.");

            if (Password != Password2)
                ModelState.AddModelError("PasswordNotMatch", "Passwords is not match.");

            if (Name.Length > 20 || Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 20));

            if (Username.Length > 20 || Username.Length < 3)
                ModelState.AddModelError("UsernameLength", string.Format("At least {1} {0} can be max {2} characters.", "Username", 3, 20));

            if (Password.Length > 20 || Password.Length < 4)
                ModelState.AddModelError("PasswordLength", string.Format("At least {1} {0} can be max {2} characters.", "Password", 4, 20));

            if (!ModelState.IsValid)
                return View();

            const string masterMenu = "Master";
            const string mediaMenu = "Media";
            const string settingMenu = "Setting";

            var authorization = !string.IsNullOrEmpty(MasterMenu) ? masterMenu + "," : "";
            authorization += !string.IsNullOrEmpty(MediaMenu) ? mediaMenu + "," : "";
            authorization += !string.IsNullOrEmpty(SettingMenu) ? settingMenu + "," : "";

            var admin = new Sihirdar.DataAccessLayer.Infrastructure.Models.Admin
            {
                CreatedDate = DateTime.Now,
                LastLoginDate = new DateTime(1970, 1, 1),
                Name = Name,
                Password = Password,
                Status = Status,
                Username = Username,
                Authorization = authorization,
                AdminType = AdminType,
                Facebook = Facebook,
                Motto = Motto,
                Twitter = Twitter
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
                {
                    return View();
                }

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    admin.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }

            var db = _dbFactory();
            db.Admins.Add(admin);
            db.SaveChanges();

            Added();

            return RedirectToAction("Add");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Admin", "Edit Admin");

            var db = _dbFactory();
            var admin = db.Admins.FirstOrDefault(a => a.Id == id);
            if (admin == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(admin.Status);
            ViewBag.AdminTypeList = DropdownTypes.GetAdminType(admin.AdminType);

            return View(admin);
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Edit(int id, string Name, string Username, string Password, string Password2, string MasterMenu, string MediaMenu, string SettingMenu, string Facebook, string Motto, string Twitter, StatusTypes Status, AdminTypes AdminType, HttpPostedFileBase Picture)
        {
            var db = _dbFactory();
            var admin = db.Admins.FirstOrDefault(a => a.Id == id);
            if (admin == null)
            {
                ModelState.AddModelError("AdminNotFound", "Admin was not found.");
                return RedirectToAction("List");
            }

            SetPageHeader("Admin", "Edit Admin");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.AdminTypeList = DropdownTypes.GetAdminType(AdminType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Username))
                ModelState.AddModelError("Username", "Username is required.");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError("Password", "Password is required.");

            if (string.IsNullOrEmpty(Password2))
                ModelState.AddModelError("PasswordAgain", "Password again is required.");

            if (Password != Password2)
                ModelState.AddModelError("PasswordNotMatch", "Passwords do not match.");

            if (Name.Length > 20 || Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 20));

            if (Username.Length > 20 || Username.Length < 3)
                ModelState.AddModelError("UsernameLength", string.Format("At least {1} {0} can be max {2} characters.", "Username", 3, 20));

            if (Password.Length > 20 || Password.Length < 4)
                ModelState.AddModelError("PasswordLength", string.Format("At least {1} {0} can be max {2} characters.", "Password", 4, 20));

            if (!ModelState.IsValid)
                return View(admin);

            const string masterMenu = "Master";
            const string mediaMenu = "Media";
            const string settingMenu = "Setting";

            var authorization = !string.IsNullOrEmpty(MasterMenu) ? masterMenu + "," : "";
            authorization += !string.IsNullOrEmpty(MediaMenu) ? mediaMenu + "," : "";
            authorization += !string.IsNullOrEmpty(SettingMenu) ? settingMenu + "," : "";

            admin.Authorization = authorization;
            admin.Name = Name;
            admin.Password = Password;
            admin.Status = Status;
            admin.Username = Username;
            admin.AdminType = AdminType;
            admin.Facebook = Facebook;
            admin.Motto = Motto;
            admin.Twitter = Twitter;

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);
                fileName = fileName.Replace(fileName, "");

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType", $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                {
                    return View(admin);
                }

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    admin.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(admin);
                }
            }

            db.SaveChanges();

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult List()
        {
            SetPageHeader("Admin", "List Admin");

            var db = _dbFactory();

            var admins = db.Admins.OrderByDescending(a => a.Id).ToList();

            return View(admins);
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Delete(int id)
        {
            var db = _dbFactory();

            var admin = db.Admins.FirstOrDefault(a => a.Id == id);
            if (admin == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            db.Admins.Remove(admin);
            db.SaveChanges();

            Deleted();

            return RedirectToAction("List");
        }
    }
}