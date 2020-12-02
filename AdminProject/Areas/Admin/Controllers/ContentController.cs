using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Areas.Admin.Models;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly ICategoryService _categoryService;
        private readonly IContentService _contentService;
        private readonly IPictureService _pictureService;
        private readonly IVideoEmbedService _videoEmbeddedService;

        public ContentController(RuntimeSettings setting, IContentService contentService,
            ICategoryService categoryService, IPictureService pictureService, IVideoEmbedService videoEmbeddedService)
            : base(setting)
        {
            _setting = setting;
            _contentService = contentService;
            _categoryService = categoryService;
            _pictureService = pictureService;
            _videoEmbeddedService = videoEmbeddedService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Content", "Add New Content");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(ContentTypes.Content);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameTypes.All);
            ViewBag.FilterTypeList = DropdownTypes.GetContentFilterType(ContentFilterTypes.All);

            var model = new Content
            {
                PublishDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        //string VideoUrl, 
        public ActionResult Add(int CategoryId, ContentTypes ContentType, GameTypes GameType,
            ContentFilterTypes FilterType, StatusTypes Status, string Name, string Detail, string IsShowcase,
            HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, string Tags,
            string hfVideoEmbed, string hfPicName, string PublishDate)
        {
            SetPageHeader("Content", "Add New Content");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(ContentType);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameType);
            ViewBag.FilterTypeList = DropdownTypes.GetContentFilterType(FilterType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Subject", "Subject is required.");

            if (CategoryId == 0)
                ModelState.AddModelError("CategoryId", "Category is not selected.");

            if (Picture == null && ContentType == ContentTypes.Content)
                ModelState.AddModelError("Picture", "Picture is required.");

            if (string.IsNullOrEmpty(Detail))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (string.IsNullOrEmpty(PublishDate))
                ModelState.AddModelError("PublishDate", "Publish Date is required.");

            if (ContentType == ContentTypes.Video && string.IsNullOrEmpty(hfVideoEmbed))
                ModelState.AddModelError("VideoUrl", "Video url is required.");

            if (GameType == GameTypes.All)
                ModelState.AddModelError("GameType", "Game type can not be all.");

            if (!ModelState.IsValid)
                return View();

            if (!Utility.DateTimeParsing(PublishDate, out DateTime p))
            {
                ModelState.AddModelError("PublishDate", "Incorrect format Publish Date. Formats: dd.MM.yyyy HH:mm - dd.MM.yyyy HH:mm:ss");
                return View();
            }

            //escape html tags
            var detailText = Utility.StripHtml(Detail);

            var url = string.IsNullOrEmpty(Url.Trim()) ? Utility.UrlSeo(Name.Trim()) : Utility.UrlSeo(Url.Trim());
            url = _contentService.UrlCheck(url);

            var content = new Content
            {
                IsShowcase = !string.IsNullOrEmpty(IsShowcase),
                Tags = Tags,
                CategoryId = CategoryId,
                ContentType = ContentType,
                GameType = GameType,
                FilterType = FilterType,
                CreateDate = DateTime.Now,
                CreateUser = Utility.SessionCheck().Id,
                Description =
                    !string.IsNullOrEmpty(Description)
                        ? Description
                        : (detailText.Length > 160 ? detailText.Substring(0, 160) : detailText),
                Detail = Detail,
                Hit = 0,
                Keyword = string.IsNullOrEmpty(Keyword) ? Name.Replace(' ', ',') : Keyword,
                LanguageId = _setting.LanguageId,
                LanguageTag = _setting.Language,
                ModifiedDate = new DateTime(1970, 1, 1),
                ModifiedUser = 0,
                Name = Name?.Trim(),
                SequenceNumber = 9999,
                Status = StatusTypes.Active,
                Title = string.IsNullOrEmpty(Title) ? Name : Title,
                Url = url,
                PublishDate = p
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
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    content.Picture = pictureName;
                    content.Status = StatusTypes.Deactive;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }

                if (!string.IsNullOrEmpty(hfVideoEmbed))
                {
                    var downloadFilePath = $"/Content/{hfPicName}";
                    Utility.DeleteFile(downloadFilePath);

                    content.VideoUrl = hfVideoEmbed;
                }
            }
            else if (ContentType == ContentTypes.Video && !string.IsNullOrEmpty(hfVideoEmbed))
            {
                var downloadFilePath = $"/Content/{hfPicName}";
                var newPictureName = Utility.UrlSeo($"{Name}-{DateTime.Now}");
                var newPicturePath = $"/Content/{newPictureName}.jpg";

                Utility.ChangeFileName(downloadFilePath, newPicturePath);

                content.VideoUrl = hfVideoEmbed;
                content.Picture = newPictureName;
                content.Status = StatusTypes.Deactive;
            }

            _contentService.Add(content);

            if (ContentType == ContentTypes.Content && string.IsNullOrEmpty(content.Picture))
            {
                Added();
                return RedirectToAction("Add");
            }

            return Redirect($"/Admin/Picture/Crop/{content.Id}");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Content", "Edit Content");

            GetCategories();

            var content = _contentService.GetItem(id);
            if (content == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(content.Status);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(content.ContentType);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(content.GameType);
            ViewBag.FilterTypeList = DropdownTypes.GetContentFilterType(content.FilterType);

            return View(content);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int Id, int CategoryId, ContentTypes ContentType, GameTypes GameType,
            ContentFilterTypes FilterType, string Name, string Detail, string Tags, string EmbeddedUrl,
            HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, StatusTypes Status,
            string IsShowcase, string hfPicName, string VideoUrl, string PublishDate)
        {
            var content = _contentService.GetItem(Id);

            if (content == null)
            {
                ModelState.AddModelError("ContentNotFound", "Content was not found.");
                return RedirectToAction("List");
            }

            SetPageHeader("Content", "Edit Content");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(ContentType);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameType);
            ViewBag.FilterTypeList = DropdownTypes.GetContentFilterType(FilterType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Subject", "Subject is required.");

            if (CategoryId == 0)
                ModelState.AddModelError("CategoryId", "Category is not selected.");

            if (string.IsNullOrEmpty(Detail))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (GameType == GameTypes.All)
                ModelState.AddModelError("GameType", "Game type can not be all.");

            if (!ModelState.IsValid)
                return View(content);

            if (!Utility.DateTimeParsing(PublishDate, out DateTime p))
            {
                ModelState.AddModelError("PublishDate", "Incorrect format Publish Date. Formats: dd.MM.yyyy HH:mm - dd.MM.yyyy HH:mm:ss");
                return View();
            }

            var newContent = new Content
            {
                Picture = content.Picture,
                VideoUrl = EmbeddedUrl,
                IsShowcase = !string.IsNullOrEmpty(IsShowcase),
                Tags = Tags,
                CategoryId = CategoryId,
                ContentType = ContentType,
                GameType = GameType,
                FilterType = FilterType,
                Description = Description,
                Detail = Detail,
                Keyword = Keyword,
                LanguageId = _setting.LanguageId,
                LanguageTag = _setting.Language,
                ModifiedDate = DateTime.Now,
                ModifiedUser = Utility.SessionCheck().Id,
                Name = Name?.Trim(),
                Status = Status,
                Title = Title,
                Url = Utility.UrlSeo(Url.Trim()),
                PublishDate = p
            };

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);
                var oldPicture = content.Picture;

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType", $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View(content);

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    newContent.Picture = pictureName;

                    //Delete pictures
                    _pictureService.DeletePictures(Id);
                    Utility.DeleteFile($"~/Content/{oldPicture}.jpg");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(content);
                }

                if (!string.IsNullOrEmpty(VideoUrl))
                {
                    newContent.VideoUrl = EmbeddedUrl;
                }

                newContent.Status = StatusTypes.Deactive;
                _contentService.Edit(Id, newContent);

                return Redirect($"/Admin/Picture/Crop/{Id}?edit=1");
            }
            if (ContentType == ContentTypes.Video && !string.IsNullOrEmpty(hfPicName))
            {
                var downloadFilePath = $"/Content/{hfPicName}";
                var newPictureName = Utility.UrlSeo($"{Name}-{DateTime.Now}");
                var newPicturePath = $"/Content/{newPictureName}.jpg";

                Utility.ChangeFileName(downloadFilePath, newPicturePath);

                //Delete pictures
                _pictureService.DeletePictures(Id);
                Utility.DeleteFile($"~/Content/{content.Picture}.jpg");

                newContent.Picture = newPictureName;
                newContent.Status = StatusTypes.Deactive;
            }

            _contentService.Edit(Id, newContent);

            if (ContentType == ContentTypes.Video && !string.IsNullOrEmpty(hfPicName))
                return Redirect($"/Admin/Picture/Crop/{Id}?edit=1");

            Updated();
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List(ContentSearchRequestDto request)
        {
            SetPageHeader("Content", "List Content");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(request.ContentType);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(request.GameType);

            var contents = _contentService.ListCategories(request);
            ViewBag.Contents = contents;

            var queryStringsList = Request.QueryString.ToEnumerable().Where(a => a.Key != "Skip").ToList();
            var nameValue = new NameValueCollection();
            queryStringsList.ForEach(a =>
            {
                nameValue.Add(a.Key, a.Value);
            });

            ViewBag.UrlAddress = $"/Admin/Content/List?{nameValue.ToQueryString()}";

            return View(request);
        }

        public ActionResult Delete(int id)
        {
            _contentService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }

        public ActionResult Order()
        {
            SetPageHeader("Content", "Content Order");

            var contents = _contentService.OrderList();

            return View(contents);
        }

        [HttpPost]
        public ActionResult Order(string[] order)
        {
            SetPageHeader("Content", "Content Order");

            _contentService.SetOrderList(order);

            Updated();

            return RedirectToAction("Order");
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            SetPageHeader("Content", "Pre-view Content");

            GetCategories();

            var content = _contentService.GetItem(id);
            if (content == null)
            {
                Warning();

                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(content.Status);

            var pictures = _pictureService.List(content.Id);
            ViewBag.Pictures = pictures;

            return View(content);
        }

        [HttpPost]
        public ActionResult View(int id, StatusTypes Status)
        {
            SetPageHeader("Content", "Pre-view Content");

            GetCategories();

            var content = _contentService.GetItem(id);
            if (content == null)
            {
                Warning();

                return RedirectToAction("List");
            }

            content.Status = Status;

            _contentService.Edit(id, content);

            ViewBag.StatusList = DropdownTypes.GetStatus(content.Status);

            var pictures = _pictureService.List(content.Id);
            ViewBag.Pictures = pictures;

            Added();

            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult GetVideoDetail(string videoUrl)
        {
            if (string.IsNullOrEmpty(videoUrl))
                return Json(new VideoEmbedResult(), JsonRequestBehavior.AllowGet);

            var fakeName = Guid.NewGuid().ToString();
            try
            {
                var result = _videoEmbeddedService.GetVideoDetail(videoUrl, fakeName);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Response.StatusCode = 400;
                return Json("Please check the url address.", JsonRequestBehavior.AllowGet);
            }
        }

        public void GetCategories()
        {
            var treeView = _categoryService.GetCategories(CategoryTypes.All, StatusTypes.Active);
            ViewBag.Menu = treeView;
        }
    }
}