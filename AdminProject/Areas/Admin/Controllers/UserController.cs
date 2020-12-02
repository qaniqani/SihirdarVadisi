using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;

        public UserController(IUserService userService, RuntimeSettings settings, ILanguageService languageService) : base(settings)
        {
            _userService = userService;
            _languageService = languageService;
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Edit(int id)
        {
            SetPageHeader("User", "Edit User");

            var user = _userService.GetItem(id);
            if (user == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.UserTypes = DropdownTypes.GetUserType(user.Status);
            ViewBag.GenderTypes = DropdownTypes.GetGenderType(user.Gender);
            ViewBag.LanguageTypes = _languageService.ActiveList(user.DefaultLanguageId);

            return View(user);
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Edit(int id, User requestUser)
        {
            SetPageHeader("User", "Edit User");

            var user = _userService.GetItem(id);
            if (user == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.UserTypes = DropdownTypes.GetUserType(user.Status);
            ViewBag.GenderTypes = DropdownTypes.GetGenderType(user.Gender);
            ViewBag.LanguageTypes = _languageService.ActiveList(user.DefaultLanguageId);

            if (string.IsNullOrEmpty(requestUser.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(requestUser.Surname))
                ModelState.AddModelError("Surname", "Surname is required.");

            if (string.IsNullOrEmpty(requestUser.Email))
                ModelState.AddModelError("Email", "Email is required.");

            if (string.IsNullOrEmpty(requestUser.Gsm))
                ModelState.AddModelError("Gsm", "Gsm is required.");

            if (string.IsNullOrEmpty(requestUser.Password))
                ModelState.AddModelError("Password", "Password is required.");

            if (!ModelState.IsValid)
                return View(requestUser);

            var language = _languageService.GetItem(requestUser.DefaultLanguageId);
            user.DefaultLanguage = language.UrlTag;
            user.DefaultLanguageId = requestUser.DefaultLanguageId;
            user.InterestAreas = requestUser.InterestAreas;
            user.Email = requestUser.Email;
            user.Gender = requestUser.Gender;
            user.Gsm = requestUser.Gsm;
            user.Name = requestUser.Name;
            user.Password = requestUser.Password;
            user.Status = requestUser.Status;
            user.Surname = requestUser.Surname;
            user.UpdatedDate = DateTime.Now;

            if (requestUser.Status == UserStatusTypes.Banned)
                user.BannedMessage = requestUser.BannedMessage;

            _userService.Edit(id, user);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult Tournament()
        {
            var path = Server.MapPath("~/Content/Tournament.xlsx");
            _userService.TournamentExport(path);
            return Redirect("/Content/Tournament.xlsx");
        }

        [AdminAuth]
        public ActionResult Detail(int id)
        {
            SetPageHeader("User", "Edit User");

            var user = _userService.GetItem(id);
            if (user == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            return View(user);
        }

        [AdminAuth]
        public ActionResult List(string Name, string Surname, string Email, UserStatusTypes Status = UserStatusTypes.Active, int Take = 20, int Skip = 1)
        {
            SetPageHeader("User", "List");

            ViewBag.UserTypes = DropdownTypes.GetUserType(Status);
            ViewBag.TakeList = DropdownTypes.TakeCount(Take);

            var request = new UserSearchRequestDto
            {
                Email = Email,
                Name = Name,
                Skip = Skip,
                Status = Status,
                Surname = Surname,
                Take = Take
            };

            var result = _userService.AllUserList(request);
            ViewBag.Users = result;

            var queryStringsList = Request.QueryString.ToEnumerable().Where(a => a.Key != "Skip").ToList();
            var nameValue = new NameValueCollection();
            queryStringsList.ForEach(a =>
            {
                nameValue.Add(a.Key, a.Value);
            });

            ViewBag.UrlAddress = $"/Admin/User/List?{nameValue.ToQueryString()}";

            return View(request);
        }

        [AdminAuth]
        public ActionResult Delete(int id)
        {
            _userService.ChangeStatus(id, UserStatusTypes.Deleted);

            Deleted();

            return RedirectToAction("List");
        }
    }
}