using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Helpers;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly ISliderService _sliderService;

        public SliderController(RuntimeSettings setting, ISliderService sliderService) : base(setting)
        {
            _setting = setting;
            _sliderService = sliderService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Slider", "Add New Slider");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(VideoTypes.IsNotVideo);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(PictureTypes.Slider);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameTypes.All);

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(
            VideoTypes VideoType, 
            string Name, 
            string Detail1, 
            string Detail2, 
            string Detail3, 
            string VideoUrl, 
            string VideoEmbedCode, 
            PictureTypes PictureType,
            GameTypes GameType,
            HttpPostedFileBase Picture1, 
            HttpPostedFileBase Picture2, 
            HttpPostedFileBase Picture3, 
            HttpPostedFileBase Picture4, 
            HttpPostedFileBase Picture5, 
            HttpPostedFileBase Picture6, 
            HttpPostedFileBase Picture7, 
            HttpPostedFileBase Picture8, 
            HttpPostedFileBase Picture9, 
            HttpPostedFileBase Picture10, 
            string SequenceNumber, 
            StatusTypes Status)
        {
            SetPageHeader("Slider", "Add New Slider");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(VideoType);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(PictureType);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (VideoType == VideoTypes.IsEmbedCode)
            {
                if (string.IsNullOrEmpty(VideoEmbedCode))
                    ModelState.AddModelError("VideoEmbedCode", "Video embed code is required.");

                if (Picture9 == null)
                    ModelState.AddModelError("Thumbnail", "Video thumbnail is required.");
            }

            if (VideoType == VideoTypes.IsVideo)
            {
                if (string.IsNullOrEmpty(VideoUrl))
                    ModelState.AddModelError("VideoUrl", "Video url is required.");

                if (Picture10 == null)
                    ModelState.AddModelError("Thumbnail", "Video thumbnail is required.");
            }

            if (VideoType == VideoTypes.IsNotVideo)
                if (Picture1 == null)
                    ModelState.AddModelError("Picture1", "Picture 1 is required.");

            if (!ModelState.IsValid)
                return View();

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "10";

            var slider = new Slider
            {
                GameType = GameType,
                CreateDate = DateTime.Now,
                Detail1 = Detail1,
                Detail2 = Detail2,
                Detail3 = Detail3,
                IsVideoLink = VideoType,
                LanguageId = _setting.LanguageId,
                Name = Name,
                PictureType = PictureType,
                SequenceNumber = Convert.ToInt32(SequenceNumber),
                Status = Status,
                VideoEmbedCode = VideoEmbedCode,
                VideoUrl = VideoUrl
            };

            #region //Picture 1
            if (Picture1 != null)
            {
                var fileName = Picture1.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture1.ContentType))
                    ModelState.AddModelError("MimeType", $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture1, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture1 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 2
            if (Picture2 != null)
            {
                var fileName = Picture2.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture2.ContentType))
                    ModelState.AddModelError("MimeType",
                        $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension",
                        $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture2, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture2 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 3
            if (Picture3 != null)
            {
                var fileName = Picture3.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture3.ContentType))
                    ModelState.AddModelError("MimeType",
                        $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension",
                        $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture3, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture3 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 4
            if (Picture4 != null)
            {
                var fileName = Picture4.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture4.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture4, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture4 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 5
            if (Picture5 != null)
            {
                var fileName = Picture5.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture5.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture5, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture5 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 6
            if (Picture6 != null)
            {
                var fileName = Picture6.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture6.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture6, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture6 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 7
            if (Picture7 != null)
            {
                var fileName = Picture7.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture7.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture7, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture7 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 8
            if (Picture8 != null)
            {
                var fileName = Picture8.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture8.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture8, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture8 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 9
            if (Picture9 != null)
            {
                var fileName = Picture9.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture9.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture9, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture9 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 10
            if (Picture10 != null)
            {
                var fileName = Picture10.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture10.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture10, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture10 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            _sliderService.Add(slider);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Slider", "Edit Slider");

            var slider = _sliderService.GetItem(id);
            if (slider == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(slider.Status);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(slider.IsVideoLink);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(slider.PictureType);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(slider.GameType);

            return View(slider);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(
            int id,
            VideoTypes VideoType,
            string Name,
            string Detail1,
            string Detail2,
            string Detail3,
            string VideoUrl,
            string VideoEmbedCode,
            PictureTypes PictureType,
            GameTypes GameType,
            HttpPostedFileBase Picture1,
            HttpPostedFileBase Picture2,
            HttpPostedFileBase Picture3,
            HttpPostedFileBase Picture4,
            HttpPostedFileBase Picture5,
            HttpPostedFileBase Picture6,
            HttpPostedFileBase Picture7,
            HttpPostedFileBase Picture8,
            HttpPostedFileBase Picture9,
            HttpPostedFileBase Picture10,
            string SequenceNumber,
            StatusTypes Status)
        {
            var slider = _sliderService.GetItem(id);
            if (slider == null)
            {
                ModelState.AddModelError("SliderNotFound", "Slider was not found.");
                return RedirectToAction("List");
            }

            SetPageHeader("Slider", "Edit Slider");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(VideoType);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(PictureType);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (VideoType == VideoTypes.IsEmbedCode)
                if (string.IsNullOrEmpty(VideoEmbedCode))
                    ModelState.AddModelError("VideoEmbedCode", "Video embed code is required.");

            if (VideoType == VideoTypes.IsVideo)
            {
                if (string.IsNullOrEmpty(VideoUrl))
                    ModelState.AddModelError("VideoUrl", "Video url is required.");

                if (VideoUrl.Length < 20)
                    ModelState.AddModelError("VideoUrlLength", $"{VideoUrl} can be min {20} characters.");
            }

            if (!ModelState.IsValid)
                return View(slider);

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "99";

            slider.GameType = GameType;
            slider.Detail1 = Detail1;
            slider.Detail2 = Detail2;
            slider.Detail3 = Detail3;
            slider.IsVideoLink = VideoType;
            slider.LanguageId = _setting.LanguageId;
            slider.Name = Name;
            slider.PictureType = PictureType;
            slider.SequenceNumber = Convert.ToInt32(SequenceNumber);
            slider.Status = Status;
            slider.VideoEmbedCode = VideoEmbedCode;
            slider.VideoUrl = VideoUrl;

            #region //Picture 1
            if (Picture1 != null)
            {
                var fileName = Picture1.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture1.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture1, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture1 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 2
            if (Picture2 != null)
            {
                var fileName = Picture2.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture2.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture2, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture2 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 3
            if (Picture3 != null)
            {
                var fileName = Picture3.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture3.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture3, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture3 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 4
            if (Picture4 != null)
            {
                var fileName = Picture4.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture4.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture4, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture4 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 5
            if (Picture5 != null)
            {
                var fileName = Picture5.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture5.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture5, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture5 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 6
            if (Picture6 != null)
            {
                var fileName = Picture6.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture6.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture6, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture6 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 7
            if (Picture7 != null)
            {
                var fileName = Picture7.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture7.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture7, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture7 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 8
            if (Picture8 != null)
            {
                var fileName = Picture8.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture8.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture8, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture8 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 9
            if (Picture9 != null)
            {
                var fileName = Picture9.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture9.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture9, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture9 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 10
            if (Picture10 != null)
            {
                var fileName = Picture10.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture10.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture10, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture10 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            _sliderService.Edit(id, slider);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("Slider", "List Slider");

            var sliders = _sliderService.List();

            return View(sliders);
        }

        public ActionResult SliderOrder()
        {
            SetPageHeader("Slider", "Slider Order");

            var sliders = _sliderService.SliderOrder();

            return View(sliders);
        }

        [HttpPost]
        public ActionResult SliderOrder(string[] order)
        {
            SetPageHeader("Slider", "Slider Order");

            _sliderService.SliderOrder(order);

            Updated();

            return RedirectToAction("SliderOrder");
        }

        public ActionResult VideoOrder()
        {
            SetPageHeader("Slider", "Video Order");

            var sliders = _sliderService.VideoOrder();

            return View(sliders);
        }

        [HttpPost]
        public ActionResult VideoOrder(string[] order)
        {
            SetPageHeader("Slider", "Video Order");

            _sliderService.VideoOrder(order);

            Updated();

            return RedirectToAction("VideoOrder");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _sliderService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}