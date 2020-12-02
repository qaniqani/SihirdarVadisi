using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Ninject;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Controllers
{
    public class DefaultController : BaseController
    {
        private readonly IKernel _kernel;
        private readonly IAdvertService _advertService;
        private readonly ISliderService _sliderService;
        private readonly ILanguageService _languageService;
        private readonly ISurveyService _surveyService;
        private readonly IContentService _contentService;
        private readonly IPromiseDayService _promiseDay;

        public DefaultController(ISliderService sliderService, IKernel kernel, ILanguageService languageService, ISurveyService surveyService, IContentService contentService, IPromiseDayService promiseDay, IAdvertService advertService)
        {
            _sliderService = sliderService;
            _kernel = kernel;
            _languageService = languageService;
            _surveyService = surveyService;
            _promiseDay = promiseDay;
            _advertService = advertService;
            _contentService = contentService;
        }

        [Route("")]
        //[CookieCheck]
        public ActionResult Index(string authorization)
        {
            if (!string.IsNullOrEmpty(authorization))
            {
                ModelState.AddModelError("error", "Başarısız giriş. Lütfen tekrar deneyiniz.");
                ViewBag.Error = "asd";
            }

            var sliders = GetSliders();
            GetCategoryAdverts();
            var dayOfVideo = _contentService.GetDayOfVideo();
            var secondSlider = _sliderService.SecondSlider();
            var subContent = _contentService.GetFourSubContent();
            var lastedVideos = _contentService.GetLastedVideos();
            var topLastedVideos = _contentService.GetHomeStaticVideos();
            var promiseDay = _promiseDay.GetDayPromise();

            return View(new DefaultModelViewModel
            {
                TopLastedVideos = topLastedVideos,
                PromiseDay = promiseDay,
                Sliders = sliders,
                DayOfVideo = dayOfVideo,
                SecondSlider = secondSlider,
                FourSubContent = subContent,
                LastedVideos = lastedVideos
            });
        }

        public ActionResult ChangeLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
                language = "tr";

            var ci = CultureInfo.GetCultureInfo(language);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            var selectedLanguage = _languageService.GetItem(language);

            if (selectedLanguage == null)
            {
                TempData["Warning"] = "Selected language not found.";
                return Redirect("/");
            }

            _kernel.Get<RuntimeSettings>().Language = selectedLanguage.UrlTag;
            _kernel.Get<RuntimeSettings>().LanguageId = selectedLanguage.Id;

            var cookie = new HttpCookie("language")
            {
                Value = language
            };

            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }

        private IList<Slider> GetSliders()
        {
            var sliders = _sliderService.ActiveTop4List();
            return sliders;
        }

        [Route("anket/{surveyId}/sonucu")]
        [CookieCheck]
        public JsonResult GetSurveyResult(int surveyId)
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            var userId = Tool.UserCheck().Id;
            var result = _surveyService.GetSurveyResult(userId, surveyId);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [Route("anket/{surveyId}/{answerId}")]
        [CookieCheck]
        public JsonResult GetSurveyResponse(int surveyId, int answerId)
        {
            if (Tool.UserCheck() == null)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.userLoginCheck;
                return Json(Resources.Lang.userLoginCheck, JsonRequestBehavior.AllowGet);
            }

            if (surveyId == 0 || answerId == 0)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.surveyOrAnswerRequired;
                return Json(Resources.Lang.surveyOrAnswerRequired, JsonRequestBehavior.AllowGet);
            }

            var userId = Tool.UserCheck().Id;

            var checkSurvey = _surveyService.GetSurveyResult(userId, surveyId);
            if (checkSurvey.Used)
            {
                Response.StatusCode = 400;
                Response.StatusDescription = Resources.Lang.afterVoteSurvey;
                return Json(Resources.Lang.afterVoteSurvey, JsonRequestBehavior.AllowGet);
            }

            _surveyService.SurveyUsedVote(userId, surveyId, answerId);

            var result = _surveyService.GetSurveyResult(userId, surveyId);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        private void GetCategoryAdverts()
        {
            var advert = _advertService.GetCategoryAdverts("tr", AdvertLocationTypes.Header).FirstOrDefault();

            if (advert != null)
                HeaderAdvert(advert);

            var rightAdvert300X450 = _advertService.GetCategoryAdverts("tr", AdvertLocationTypes.HomePage300X450).FirstOrDefault();
            if (rightAdvert300X450 != null)
                Home300Advert(rightAdvert300X450);

            var rightAdvert728X90 = _advertService.GetCategoryAdverts("tr", AdvertLocationTypes.HomePage728X90).FirstOrDefault();
            if (rightAdvert728X90 != null)
                Home728Advert(rightAdvert728X90);
        }
    }
}