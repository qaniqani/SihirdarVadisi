using System;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Models;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;

        public SettingsController(Func<AdminDbContext> dbFactory, RuntimeSettings setting) : base(setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Index()
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var setting =
                db.Settings.Where(a => a.LanguageId == languageId).OrderByDescending(a => a.Id).FirstOrDefault();

            return View(setting ?? new Settings());
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Index(string Title, string Description, string Keyword, string MailAddress, string MailPassword, string Smtp, string Port)
        {
            SetPageHeader("Settings", "Update Settings");

            if (string.IsNullOrEmpty(Title))
                ModelState.AddModelError("Title", "Title is required");

            if (Title.Length > 200 || Title.Length < 10)
                ModelState.AddModelError("TitleLength", string.Format("At least {1} {0} can be max {2} characters.", "Title", 10, 200));

            if (Description.Length > 200)
                ModelState.AddModelError("DescriptionLength", string.Format("{0} can be max {1} characters.", "Description", 200));

            if (!ModelState.IsValid)
                return View();

            var setting = new Settings
            {
                CreateDate = DateTime.Now,
                Description = Description,
                Keyword = Keyword,
                LanguageId = _setting.LanguageId,
                MailAddress = MailAddress,
                MailPassword = MailPassword,
                Port = Port,
                Smtp = Smtp,
                Status = StatusTypes.Active,
                Title = Title
            };

            var db = _dbFactory();

            db.Settings
            .Where(a => a.LanguageId == _setting.LanguageId)
            .ToList()
            .ForEach(a => a.Status = StatusTypes.Deactive);

            db.SaveChanges();

            db.Settings.Add(setting);
            db.SaveChanges();

            Added();

            return View();
        }
    }
}