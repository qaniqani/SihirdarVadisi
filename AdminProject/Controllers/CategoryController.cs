using System;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Helpers;
using System.Collections.Generic;
using AdminProject.Services.Interface;
using AdminProject.Services.CustomExceptions;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IAdvertService _advertService;
        private readonly IEsportCalendarService _esportService;
        private readonly IContentService _contentService;
        private readonly ICategoryService _categoryService;
        private readonly ICounterService _counterService;
        private readonly int _take;
        private readonly int _videoTake;
        private readonly int _galleryTake;

        public CategoryController(IContentService contentService, ICategoryService categoryService, IEsportCalendarService esportService, ICounterService counterService, IAdvertService advertService)
        {
            _contentService = contentService;
            _categoryService = categoryService;
            _esportService = esportService;
            _counterService = counterService;
            _advertService = advertService;
            _take = 6;
            _videoTake = 24;
            _galleryTake = 24;
        }

        [Route("haber")]
        public ActionResult DefaultList()
        {
            //vadiden-haberler
            return RedirectToActionPermanent("List", "Category", new {categoryUrl = "vadiden-haberler", page = 1});
        }

        [Route("haber/{categoryUrl}")]
        public ActionResult List(string categoryUrl, int page = 1)
        {
            if (string.IsNullOrEmpty(categoryUrl))
                return Redirect("/");

            var category = _categoryService.GetCategory(categoryUrl);
            if (category == null)
            {
                SetErrorMessage(Resources.Lang.categoryNotFound);
                return Redirect("/");
            }

            switch (category.CategoryType)
            {
                case CategoryTypes.Video:
                    return RedirectToActionPermanent("Video", new { categoryUrl });
                case CategoryTypes.ESportCalendar:
                    return RedirectToActionPermanent("EsportCalendar");
                case CategoryTypes.Story:
                    try
                    {
                        ViewBag.Take = _take;
                        var result = _contentService.GetCategoryContent(categoryUrl, category.CategoryType, page, _take);

                        TempData["CategoryUrl"] = categoryUrl;
                        SetTitle(category.Title);
                        ViewBag.Category = category;
                        ViewBag.Page = page;

                        GetCategoryAdverts(categoryUrl);

                        return View("List", result);
                    }
                    catch (CustomException cEx)
                    {
                        SetErrorMessage(cEx.Message);
                        return Redirect("/");
                    }
                case CategoryTypes.Gallery:
                    try
                    {
                        return RedirectToActionPermanent("Gallery", new { categoryUrl });
                    }
                    catch (CustomException cEx)
                    {
                        SetErrorMessage(cEx.Message);
                        return Redirect("/");
                    }
                default:
                    return Redirect("/");
            }
        }

        [Route("video/{categoryUrl}")]
        public ActionResult Video(string categoryUrl, int page = 1)
        {
            if (string.IsNullOrEmpty(categoryUrl))
                return Redirect("/");

            var category = _categoryService.GetCategory(categoryUrl);
            if (category == null)
            {
                SetErrorMessage(Resources.Lang.categoryNotFound);
                return Redirect("/");
            }

            ViewBag.GameType = DropdownTypes.GetUserGameType(GameTypes.All);
            ViewBag.FilterType = DropdownTypes.GetUserContentFilterType(ContentFilterTypes.All);

            var result = _contentService.GetCategoryVideo(categoryUrl, category.CategoryType, page, _videoTake);

            SetTitle(category.Title);

            var firstVideo = _contentService.GetLastAndNextVideo();

            TempData["CategoryUrl"] = categoryUrl;
            ViewBag.FirstVideo = firstVideo;
            ViewBag.Category = category;
            ViewBag.Take = _videoTake;
            ViewBag.Page = page;
            ViewBag.CheckLiked = false;

            CheckLiked(firstVideo?.FirstVideo?.Url, ContentTypes.Video);

            GetCategoryAdverts(categoryUrl);

            return View("Video", result);
        }

        [Route("fotograf-galerileri/{categoryUrl}")]
        public ActionResult Gallery(string categoryUrl, int page = 1)
        {
            if (string.IsNullOrEmpty(categoryUrl))
                return Redirect("/");

            var category = _categoryService.GetCategory(categoryUrl);
            if (category == null)
            {
                SetErrorMessage(Resources.Lang.categoryNotFound);
                return Redirect("/");
            }

            ViewBag.GameType = DropdownTypes.GetUserGameType(GameTypes.All);
            ViewBag.FilterType = DropdownTypes.GetUserContentFilterType(ContentFilterTypes.All);

            var result = _contentService.GetCategoryGallery(categoryUrl, category.CategoryType, page, _galleryTake);

            SetTitle(category.Title);

            var firstGallery = _contentService.GetLastAndNextGallery();

            TempData["CategoryUrl"] = categoryUrl;
            ViewBag.FirstGallery = firstGallery;
            ViewBag.Category = category;
            ViewBag.Take = _galleryTake;
            ViewBag.Page = page;
            ViewBag.Title = category.Title;
            ViewBag.CheckLiked = false;

            CheckLiked(firstGallery?.FirstGallery?.Url, ContentTypes.Gallery);

            GetCategoryAdverts(categoryUrl);

            return View("Gallery", result);
        }

        [Route("fotograf-galerileri/{categoryUrl}/ajax")]
        public JsonResult GalleryAjax(string categoryUrl, GameTypes gameType, ContentFilterTypes filterType = ContentFilterTypes.All, int page = 1)
        {
            if (string.IsNullOrEmpty(categoryUrl))
                return Json(new { status = 0, buttonStatus = 0, result = new List<CategoryGalleryItemViewModel>() }, JsonRequestBehavior.AllowGet);

            var category = _categoryService.GetCategory(categoryUrl);
            if (category == null)
                return Json(new { status = 0, buttonStatus = 0, result = new List<CategoryGalleryItemViewModel>() }, JsonRequestBehavior.AllowGet);

            var result = _contentService.GetCategoryGallery(categoryUrl, category.CategoryType, gameType, page, _galleryTake);

            var buttonStatus = result.Count > 0 ? 1 : 0;

            return Json(new { status = 1, buttonStatus, result }, JsonRequestBehavior.AllowGet);
        }

        [Route("haber/espor-takvimi")]
        public ActionResult EsportCalendar()
        {
            GetCategoryAdverts("espor-takvimi");
            return View("Calendar");
        }

        [Route("esport/calendar")]
        public ActionResult Calendar()
        {
            var startDate = DateTime.Now.AddYears(-1);
            var calendar = _esportService.List(startDate);

            return Json(calendar, JsonRequestBehavior.AllowGet);
        }

        [Route("video/{videoUrl}/detay")]
        public ActionResult VideoDetail(string videoUrl, int page = 1)
        {
            if (string.IsNullOrEmpty(videoUrl))
                return Redirect("/");

            var video = _contentService.GetLastAndNextVideo(videoUrl);
            if (video == null)
            {
                SetErrorMessage(Resources.Lang.contentNotFound);
                return Redirect("/");
            }

            if (video.FirstVideo == null)
            {
                SetErrorMessage(Resources.Lang.contentNotFound);
                return Redirect("/");
            }

            var categoryId = video.FirstVideo.CategoryId;

            var category = _categoryService.GetItem(categoryId);
            if (category == null)
            {
                SetErrorMessage(Resources.Lang.categoryNotFound);
                return Redirect("/");
            }
            var categoryUrl = category.Url;

            ViewBag.GameType = DropdownTypes.GetUserGameType(video.FirstVideo.GameTypes);
            ViewBag.FilterType = DropdownTypes.GetUserContentFilterType(video.FirstVideo.FilterTypes);

            var result = _contentService.GetCategoryVideo(categoryUrl, category.CategoryType, video.FirstVideo.GameTypes, video.FirstVideo.FilterTypes, page, _videoTake);

            SetTitle(category.Title);

            TempData["CategoryUrl"] = categoryUrl;
            ViewBag.FirstVideo = video;
            ViewBag.Category = category;
            ViewBag.Take = _videoTake;
            ViewBag.Page = page;
            ViewBag.CheckLiked = false;

            CheckLiked(videoUrl, ContentTypes.Video);

            GetCategoryAdverts(categoryUrl);

            return View("Video", result);
        }

        [Route("video/{categoryUrl}/ajax")]
        public JsonResult VideoAjax(string categoryUrl, GameTypes gameType, ContentFilterTypes filterType = ContentFilterTypes.All, int page = 1)
        {
            if (string.IsNullOrEmpty(categoryUrl))
                return Json(new {status = 0, buttonStatus = 0, result = new List<CategoryVideoItemViewModel>()}, JsonRequestBehavior.AllowGet);

            var category = _categoryService.GetCategory(categoryUrl);
            if (category == null)
                return Json(new { status = 0, buttonStatus = 0, result = new List<CategoryVideoItemViewModel>() }, JsonRequestBehavior.AllowGet);

            var result = _contentService.GetCategoryVideo(categoryUrl, category.CategoryType, gameType, filterType, page, _videoTake);

            var buttonStatus = result.Count > 0 ? 1 : 0;

            return Json(new { status = 1, buttonStatus, result }, JsonRequestBehavior.AllowGet);
        }

        [Route("fotograf-galerileri/{categoryUrl}/{galleryUrl}/detay")]
        public ActionResult GalleryDetail(string categoryUrl, string galleryUrl, int page = 1)
        {
            if (string.IsNullOrEmpty(categoryUrl) || string.IsNullOrEmpty(galleryUrl))
                return Redirect("/");

            var gallery = _contentService.GetLastAndNextGallery(galleryUrl);
            if (gallery == null)
            {
                SetErrorMessage(Resources.Lang.contentNotFound);
                return Redirect("/");
            }

            if (gallery.FirstGallery == null)
            {
                SetErrorMessage(Resources.Lang.contentNotFound);
                return Redirect("/");
            }

            var categoryId = gallery.FirstGallery.CategoryId;

            var category = _categoryService.GetItem(categoryId);
            if (category == null)
            {
                SetErrorMessage(Resources.Lang.categoryNotFound);
                return Redirect("/");
            }
            
            ViewBag.GameType = DropdownTypes.GetUserGameType(gallery.FirstGallery.GameTypes);
            ViewBag.FilterType = DropdownTypes.GetUserContentFilterType(ContentFilterTypes.All);

            var result = _contentService.GetCategoryGallery(categoryUrl, category.CategoryType, gallery.FirstGallery.GameTypes, page, _galleryTake);

            SetTitle(category.Title);

            TempData["CategoryUrl"] = categoryUrl;
            ViewBag.FirstGallery = gallery;
            ViewBag.Category = category;
            ViewBag.Take = _videoTake;
            ViewBag.Page = page;
            ViewBag.Title = gallery.FirstGallery.Subject;
            ViewBag.CheckLiked = false;

            CheckLiked(galleryUrl, ContentTypes.Gallery);

            GetCategoryAdverts(categoryUrl);

            return View("Gallery", result);
        }

        private void CheckLiked(string url, ContentTypes contentType)
        {
            if (Tool.UserCheck() == null)
            {
                ViewBag.CheckLiked = false;
                return;
            }

            var userId = Tool.UserCheck().Id;
            ViewBag.CheckLiked = _counterService.CheckUserLiked(userId, url, contentType);
            if (ViewBag.CheckLiked)
            {
                ViewBag.LikeId = _counterService.UserContentLike(userId, url, contentType).Id;
            }
        }

        private void GetCategoryAdverts(string url)
        {
            var adverts = _advertService.GetCategoryAdverts(url, "tr");

            var rightBlockAdverts = adverts.Where(a => a.Location == AdvertLocationTypes.RightBlock).ToList();
            var centerAdvert = adverts.FirstOrDefault(a => a.Location == AdvertLocationTypes.SiteMiddle);

            if (rightBlockAdverts.Any())
                RightBlockAdvert(rightBlockAdverts);

            if (centerAdvert != null)
                CenterAdvert(centerAdvert);
        }
    }
}