using System;
using System.Web.Mvc;
using System.Web.Security;
using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;
using Facebook;
using Newtonsoft.Json;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Controllers
{
    public class LoginController : BaseController
    {
        private readonly RuntimeSettings _settings;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ICounterService _counterService;

        public LoginController(IUserService userService, RuntimeSettings settings, IEmailService emailService,
            ICounterService counterService)
        {
            _userService = userService;
            _settings = settings;
            _emailService = emailService;
            _counterService = counterService;
        }

        private Uri RedirectUri => new Uri($"{_settings.Domain}/kullanici/fb/callback");
        //private Uri RedirectUri => new Uri($"http://localhost:2437/kullanici/fb/callback");

        [Route("kullanici")]
        [HttpPost]
        public JsonResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var oldUser = _userService.GetItem(email);
                if (oldUser != null && oldUser.Status == UserStatusTypes.OldUser)
                {
                    if (string.IsNullOrEmpty(oldUser.Password))
                    {
                        var r = new Random();
                        var newPassword = r.Next(100000, 999999).ToString();
                        oldUser.Password = newPassword;
                        oldUser.Status = UserStatusTypes.Active;
                        var oldUserId = oldUser.Id;
                        _userService.Edit(oldUserId, oldUser);

                        _emailService.SendNewPasswordMail(email, oldUser.Name, oldUser.Surname, newPassword);

                        var message = string.Format(Resources.Lang.oldUserNewPassword, email);

                        Response.StatusCode = 400;
                        Response.StatusDescription = message;
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.usernameOrPasswordRequired;
                return Json(Resources.Lang.usernameOrPasswordRequired, JsonRequestBehavior.AllowGet);
            }

            var user = _userService.Login(email, password);
            if (user == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.loginError;
                return Json(Resources.Lang.loginError, JsonRequestBehavior.AllowGet);
            }

            if (user.Status == UserStatusTypes.Banned)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = string.Format(Resources.Lang.userBannedMessage, user.BannedMessage);
                return Json(string.Format(Resources.Lang.userBannedMessage, user.BannedMessage),
                    JsonRequestBehavior.AllowGet);
            }

            if (user.Status == UserStatusTypes.Deleted)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userDeleted;
                return Json(Resources.Lang.userDeleted, JsonRequestBehavior.AllowGet);
            }

            if (user.Status == UserStatusTypes.Unapproved)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userUnapproved;
                return Json(Resources.Lang.userUnapproved, JsonRequestBehavior.AllowGet);
            }

            if (user.Status == UserStatusTypes.Deactive)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userDeactive;
                return Json(Resources.Lang.userDeactive, JsonRequestBehavior.AllowGet);
            }

            var userModel = new UserResultDto
            {
                Picture = string.IsNullOrEmpty(user.PicturePath) ? "" : user.PicturePath,
                Email = user.Email,
                Gender = user.Gender,
                Gsm = user.Gsm,
                Id = user.Id,
                Day = user.BirthDate.Day.ToString(),
                Month = user.BirthDate.Month.ToString(),
                Year = user.BirthDate.Year.ToString(),
                City = user.City,
                Country = user.Country,
                InterestAreas = user.InterestAreas,
                LastLoginDate = user.LastLoginDate,
                Name = user.Name,
                SignInType = user.SignInType,
                Surname = user.Surname,
                LikeCount = _counterService.CounterCount(user.Id)
            };

            Session["User"] = userModel;

            //if (isRememberMe)
            //{
            //    var cookie = new HttpCookie("User")
            //    {
            //        Expires = DateTime.Now.AddMonths(12)
            //    };
            //    cookie.Values["Email"] = user.Email;

            //    Response.Cookies.Add(cookie);
            //}

            return Json(userModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("kullanici/fb/callback")]
        public ActionResult FacebookCallback(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Redirect("/kullanici/fb/error");

            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "244843595925420",
                client_secret = "840ac5253d47707162b4804869906809",
                redirect_uri = RedirectUri.AbsoluteUri,
                code
            });

            Session["AccessToken"] = result.access_token;
            fb.AccessToken = result.access_token;
            
            dynamic me = fb.Get("me?fields=email,id,cover,name,first_name,last_name,age_range,link,gender,picture,locale");

            var getUser = _userService.GetItem(me.email);
            if (getUser != null)
            {
                var userModel = new UserResultDto
                {
                    Picture = string.IsNullOrEmpty(getUser.PicturePath) ? "" : getUser.PicturePath,
                    Email = getUser.Email,
                    Gender = getUser.Gender,
                    Gsm = getUser.Gsm,
                    Id = getUser.Id,
                    Day = getUser.BirthDate.Day.ToString(),
                    Month = getUser.BirthDate.Month.ToString(),
                    Year = getUser.BirthDate.Year.ToString(),
                    City = getUser.City,
                    Country = getUser.Country,
                    InterestAreas = getUser.InterestAreas,
                    LastLoginDate = getUser.LastLoginDate,
                    Name = getUser.Name,
                    SignInType = getUser.SignInType,
                    Surname = getUser.Surname,
                    LikeCount = _counterService.CounterCount(getUser.Id)
                };

                Session["User"] = userModel;
                return Redirect("/");
            }

            #region
            var request = new UserCreateRequestDto
            {
                City = string.Empty,
                Country = string.Empty,
                Email = me.email,
                Gsm = string.Empty,
                Gender = me.gender == "male" ? GenderTypes.Man : GenderTypes.Woman,
                Name = me.first_name,
                Surname = me.last_name,
                Password = ""
            };

            try
            {
                var birthDate = new DateTime(request.Year, request.Month, request.Day);

                var user = new User
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.Now,
                    DefaultLanguage = _settings.Language,
                    DefaultLanguageId = _settings.LanguageId,
                    Email = request.Email,
                    Gender = request.Gender,
                    Gsm = request.Gsm,
                    BirthDate = birthDate,
                    City = request.City,
                    Country = request.Country,
                    InterestAreas = request.InterestAreas,
                    LastLoginDate = DateTime.Now,
                    Name = request.Name,
                    PicturePath = string.Empty,
                    Password = request.Password,
                    SignInType = SignInTypes.Facebook,
                    Status = UserStatusTypes.Active,
                    Surname = request.Surname,
                    FacebookDetail = JsonConvert.SerializeObject(me),
                    UpdatedDate = new DateTime(1970, 1, 1),
                };

                _userService.Add(user);

                var userModel = new UserResultDto
                {
                    Picture = string.IsNullOrEmpty(user.PicturePath) ? "" : user.PicturePath,
                    Email = user.Email,
                    Gender = user.Gender,
                    Gsm = user.Gsm,
                    Id = user.Id,
                    Day = user.BirthDate.Day.ToString(),
                    Month = user.BirthDate.Month.ToString(),
                    Year = user.BirthDate.Year.ToString(),
                    City = user.City,
                    Country = user.Country,
                    InterestAreas = user.InterestAreas,
                    LastLoginDate = user.LastLoginDate,
                    Name = user.Name,
                    SignInType = user.SignInType,
                    Surname = user.Surname,
                    LikeCount = _counterService.CounterCount(user.Id)
                };

                Session["User"] = userModel;

                return Redirect("/");
            }
            catch (CustomException cex)
            {
                var errorMessage = "Kullanıcı eklerken bir hata oluştu. Hata detayı: " + cex.Message;
                ModelState.AddModelError("error", errorMessage);
                ViewBag.Error = "asd";
            }
            catch (Exception ex)
            {
                var errorMessage = "Kullanıcı eklerken bir hata oluştu. Hata detayı: " + ex.Message;
                ModelState.AddModelError("error", errorMessage);
                ViewBag.Error = "asd";
            }
            #endregion

            return Redirect("/");
        }

        [HttpGet]
        [Route("kullanici/fb")]
        public ActionResult UserFacebookRedirect()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "244843595925420",
                client_secret = "840ac5253d47707162b4804869906809",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        [HttpGet]
        [Route("kullanici/fb/error")]
        public ActionResult UserFacebookError()
        {
            ModelState.AddModelError("error", "Başarısız giriş. Lütfen tekrar deneyiniz.");
            ViewBag.Error = "asd";

            return Redirect("/?authorization=false");
        }

        [HttpPost]
        [Route("kullanici/olustur")]
        public JsonResult UserCreate(UserCreateRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
                ModelState.AddModelError("Email", Resources.Lang.emailRequired);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", Resources.Lang.nameRequired);

            if (string.IsNullOrEmpty(request.Password))
                ModelState.AddModelError("Password", Resources.Lang.passwordRequired);

            if (request.Password.Length < 6 && request.Password.Length >= 50)
                ModelState.AddModelError("PasswordLenght", Resources.Lang.passwordCharacterLenght);

            if (string.IsNullOrEmpty(request.Password2))
                ModelState.AddModelError("Password2", Resources.Lang.passwordAgainRequired);

            if (string.IsNullOrEmpty(request.Surname))
                ModelState.AddModelError("Surname", Resources.Lang.surnameRequired);

            if (request.Password != request.Password2)
                ModelState.AddModelError("PasswordNotMatch", Resources.Lang.passwordNotMatch);

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                Response.StatusDescription = errors;
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            if (!_userService.EmailCheck(request.Email))
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userEmailUsed;
                return Json(Resources.Lang.userEmailUsed, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var filename = Utility.UrlSeo($"{request.Name}_{request.Surname}_{DateTime.Now}") + ".jpg";
                if (!string.IsNullOrEmpty(request.Picture))
                {
                    var convert = request.Picture.Replace("data:image/png;base64,", string.Empty);
                    convert = convert.Replace("data:image/jpg;base64,", string.Empty);
                    convert = convert.Replace("data:image/jpeg;base64,", string.Empty);

                    Tool.SaveUserImage(filename, convert);
                }
                else
                    filename = string.Empty;

                var birthDate = new DateTime(request.Year, request.Month, request.Day);

                var user = new User
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.Now,
                    DefaultLanguage = _settings.Language,
                    DefaultLanguageId = _settings.LanguageId,
                    Email = request.Email,
                    Gender = request.Gender,
                    Gsm = request.Gsm,
                    BirthDate = birthDate,
                    City = request.City,
                    Country = request.Country,
                    InterestAreas = request.InterestAreas,
                    LastLoginDate = DateTime.Now,
                    Name = request.Name,
                    PicturePath = string.IsNullOrEmpty(filename) ? string.Empty : filename,
                    Password = request.Password,
                    SignInType = SignInTypes.Site,
                    Status = UserStatusTypes.Unapproved,
                    Surname = request.Surname,
                    UpdatedDate = new DateTime(1970, 1, 1),
                };

                _userService.Add(user);

                return Json(Resources.Lang.userCreatedMessage, JsonRequestBehavior.AllowGet);
            }
            catch (CustomException cex)
            {
                var errorMessage = "Kullanıcı eklerken bir hata oluştu. Hata detayı: " + cex.Message;
                Response.StatusCode = 400;
                Response.StatusDescription = errorMessage;
                return Json(errorMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errorMessage = "Kullanıcı eklerken bir hata oluştu. Hata detayı: " + ex.Message;
                Response.StatusCode = 400;
                Response.StatusDescription = errorMessage;
                return Json(errorMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("kullanici/bilgileri")]
        public JsonResult UserInfo()
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            var user = Tool.UserCheck();

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("kullanici/aktivasyon/{activationCode}")]
        public ActionResult UserActivation(string activationCode)
        {
            if (string.IsNullOrEmpty(activationCode))
                return RedirectPermanent("/");

            if (_userService.EmailActivation(activationCode))
            {
                ViewBag.Message = "Hesabınız aktif hale getirilmiştir. Hesabınıza giriş yaparak devam edebilirsiniz.";
                return View();
            }

            ViewBag.Message = "Böyle bir hesap bulunamamıştır. Lütfen mail adresinizi kontrol edip, tekrar deneyiniz.";
            return View();
        }

        [HttpPost]
        [Route("kullanici/bilgileri")]
        public JsonResult UserInfoUpdate(UserRequestDto request)
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", Resources.Lang.nameRequired);

            if (string.IsNullOrEmpty(request.Surname))
                ModelState.AddModelError("Surname", Resources.Lang.surnameRequired);

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                Response.StatusDescription = errors;
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var userId = Tool.UserCheck().Id;
            var user = _userService.GetItem(userId);
            if (user == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            var birthDate = new DateTime(request.Year, request.Month, request.Day);
            user.Gender = request.Gender;
            user.Gsm = request.Gsm;
            user.InterestAreas = request.InterestAreas;
            user.LastLoginDate = DateTime.Now;
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.UpdatedDate = DateTime.Now;
            user.BirthDate = birthDate;
            user.City = request.City;
            user.Country = request.Country;

            var filename = !string.IsNullOrEmpty(user.PicturePath) ? user.PicturePath : Utility.UrlSeo($"{request.Name}_{request.Surname}_{DateTime.Now}") + ".jpg";
            if (!string.IsNullOrEmpty(request.Picture))
            {
                var convert = request.Picture.Replace("data:image/png;base64,", string.Empty);
                convert = convert.Replace("data:image/jpg;base64,", string.Empty);
                convert = convert.Replace("data:image/jpeg;base64,", string.Empty);

                Tool.SaveUserImage(filename, convert);
                user.PicturePath = filename;
            }

            _userService.Edit(userId, user);

            var userModel = new UserResultDto
            {
                Picture = string.IsNullOrEmpty(user.PicturePath) ? "" : user.PicturePath,
                Email = user.Email,
                Gender = user.Gender,
                Gsm = user.Gsm,
                Id = user.Id,
                Day = user.BirthDate.Day.ToString(),
                Month = user.BirthDate.Month.ToString(),
                Year = user.BirthDate.Year.ToString(),
                City = user.City,
                Country = user.Country,
                InterestAreas = user.InterestAreas,
                LastLoginDate = user.LastLoginDate,
                Name = user.Name,
                SignInType = user.SignInType,
                Surname = user.Surname,
                LikeCount = _counterService.CounterCount(user.Id)
            };

            Session["User"] = userModel;

            return Json(Resources.Lang.userUpdateMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("kullanici/sifre")]
        public JsonResult UserChangePassword(string password, string password2, string oldPassword)
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(password))
                ModelState.AddModelError("Password", Resources.Lang.passwordRequired);

            if (string.IsNullOrEmpty(password2))
                ModelState.AddModelError("Password2", Resources.Lang.passwordAgainRequired);

            if (string.IsNullOrEmpty(oldPassword))
                ModelState.AddModelError("OldPassword", Resources.Lang.oldPasswordRequired);

            if (password != password2)
                ModelState.AddModelError("PasswordNotMatch", Resources.Lang.passwordNotMatch);

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                Response.StatusDescription = errors;
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var userId = Tool.UserCheck().Id;
            var user = _userService.GetItem(userId);
            if (user == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            if (user.Password != oldPassword)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.oldPasswordIncorrect;
                return Json(Resources.Lang.oldPasswordIncorrect, JsonRequestBehavior.AllowGet);
            }

            user.Password = password;

            _userService.Edit(userId, user);

            return Json(Resources.Lang.userUpdateMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("kullanici/sifremi-unuttum")]
        public JsonResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                ModelState.AddModelError("Email", Resources.Lang.emailRequired);

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                Response.StatusDescription = errors;
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var user = _userService.GetItem(email);
            if (user == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.emailNotFound;
                return Json(Resources.Lang.emailNotFound, JsonRequestBehavior.AllowGet);
            }

            if (user.Status == UserStatusTypes.Deactive || user.Status == UserStatusTypes.Unapproved || user.Status == UserStatusTypes.Deleted || user.Status == UserStatusTypes.Banned)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.emailNotFound;
                return Json(Resources.Lang.emailNotFound, JsonRequestBehavior.AllowGet);
            }

            try
            {
                _emailService.SendForgotPasswordMail(email, user.Name, user.Surname, user.Password);
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.emailSendingError;
                return Json(Resources.Lang.emailSendingError, JsonRequestBehavior.AllowGet);
            }

            return Json(Resources.Lang.emailSendPassword, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("kullanici/cikis")]
        public JsonResult LogOut()
        {
            //Response.Cookies.Clear();
            //var c = new HttpCookie("User") { Expires = DateTime.Now.AddDays(-1) };
            //Response.Cookies.Add(c);

            Session.Remove("User");
            Session.Clear();
            Session.Abandon();

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [Route("kullanici/begenilenler")]
        [AuthorizationFilter]
        public ActionResult UserLiked()
        {
            var userId = Tool.UserCheck().Id;
            var likes = _counterService.CounterList(userId);

            return View(likes);
        }

        [Route("kullanici/begenilenler/{contentId}/{contentUrl}/{contentType}")]
        [HttpPost]
        public ActionResult UserLiked(int contentId, string contentUrl, ContentTypes contentType)
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            var userId = Tool.UserCheck().Id;
            var counter = new Counter
            {
                ContentId = contentId,
                ContentType = contentType,
                ContentUrl = contentUrl,
                DateTime = DateTime.Now,
                UserId = userId
            };

            _counterService.Add(counter);

            return Json(counter.Id, JsonRequestBehavior.AllowGet);
        }

        [Route("kullanici/begenilenler/{likeId}/kaldir")]
        [HttpPost]
        public JsonResult UserLiked(int likeId)
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            if (likeId == 0)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.likeNotFound;
                return Json(Resources.Lang.likeNotFound, JsonRequestBehavior.AllowGet);
            }

            var userId = Tool.UserCheck().Id;
            _counterService.Delete(userId, likeId);

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [Route("kullanici/begenilenler/{likeId}")]
        [AuthorizationFilter]
        public ActionResult UserLiked1(int likeId)
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            if (likeId == 0)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.likeNotFound;
                return Json(Resources.Lang.likeNotFound, JsonRequestBehavior.AllowGet);
            }

            var userId = Tool.UserCheck().Id;
            _counterService.Delete(userId, likeId);

            return RedirectToAction("UserLiked");
        }
    }
}