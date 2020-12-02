using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Areas.Admin.Models;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class PictureController : BaseController
    {
        private readonly IPictureService _pictureService;
        private readonly IPictureSizeService _pictureSizeService;
        private readonly IContentService _contentService;
        private readonly ICategoryService _categoryService;

        public PictureController(IPictureService pictureService, RuntimeSettings setting, IContentService contentService,
            IPictureSizeService pictureSizeService, ICategoryService categoryService) : base(setting)
        {
            _pictureService = pictureService;
            _contentService = contentService;
            _pictureSizeService = pictureSizeService;
            _categoryService = categoryService;
        }

        public ActionResult Crop(int? id, string width = "0", string height = "0", string edit = "0")
        {
            SetPageHeader("Picture", "Crop");

            if (id == null)
            {
                Warning();
                Redirect("/Admin/Default");
            }

            var content = _contentService.GetItem(Convert.ToInt32(id));

            if (content == null)
                return Redirect("/Admin/Default");

            var categoryId = content.CategoryId;
            var category = _categoryService.GetItem(categoryId);

            if (category == null)
                return Redirect("/Admin/Default");

            var categoryType = category.CategoryType;

            ContentTypes contentType = ContentTypes.Content;

            if (categoryType == CategoryTypes.Video)
                contentType = ContentTypes.Video;
            else if (categoryType == CategoryTypes.Gallery)
                contentType = ContentTypes.Gallery;

            var pictureSize = _pictureSizeService.List(contentType, StatusTypes.Active).FirstOrDefault();
            if (pictureSize == null)
            {
                Warning();
                return Redirect("/Admin/Default");
            }

            GetPictureSizes(pictureSize.Id, Convert.ToInt32(width), Convert.ToInt32(height));

            if (edit != "0")
            {
                //var oldPicturesIds = _pictureService.List(Convert.ToInt32(id)).Select(a => a.Id).ToArray();
                //_pictureService.Delete(oldPicturesIds);

                Warning("Since you have selected a new image, you must perform the cropping again. Old pictures have been deleted.");
            }

            return View(content);
        }

        private void GetPictureSizes(int sizeId, int width, int height)
        {
            var sizes = _pictureSizeService.ActiveListSizeDetail(sizeId).Select(a => new PictureSizeItemDto
            {
                Id = a.Id,
                Name = a.Name,
                Width = a.Width,
                Height = a.Height,
                SizeId = a.SizeId,
                Selected = false
            }).ToList();

            if (width == 0 && height == 0 && sizes.Count > 0)
            {
                var defaultSize = sizes.FirstOrDefault();

                var defaultIndex = sizes.FindIndex(a => a.Width == defaultSize.Width && a.Height == defaultSize.Height);

                defaultSize.Selected = true;

                ViewBag.SelectedSize = defaultSize;
                ViewBag.SelectedIndex = defaultIndex;
                Session["Sizes"] = sizes;
            }

            var size = sizes.FirstOrDefault(a => a.Width == width && a.Height == height);
            if (size == null)
                return;

            var index = sizes.FindIndex(a => a.Width == width && a.Height == height);

            size.Selected = true;

            ViewBag.SelectedSize = size;
            ViewBag.SelectedIndex = index;
            Session["Sizes"] = sizes;
        }

        public JsonResult SendPicture(int id, int sizeId, int index, string width, string height, string name, string imageBaseData)
        {
            try
            {
                var filename = $"{name}_{width}x{height}.jpg";

                var convert = imageBaseData.Replace("data:image/png;base64,", string.Empty);
                convert = convert.Replace("data:image/jpg;base64,", string.Empty);
                convert = convert.Replace("data:image/jpeg;base64,", string.Empty);

                SaveImage(filename, convert);

                var picture = new Picture
                {
                    ContentId = id,
                    Height = Convert.ToInt32(height),
                    PicturePath = filename,
                    PictureType = ContentPictureTypes.Content,
                    SizeId = sizeId,
                    Width = Convert.ToInt32(width)
                };
                _pictureService.Add(picture);

                var sizes = Session["Sizes"] as List<PictureSizeItemDto>;

                var sizeCount = sizes.Count;
                var nextIndex = index + 1;
                if (nextIndex == sizeCount)
                {
                    return Json(new { isNext = false, width, height, fileName = $"/Content/{filename}" }, JsonRequestBehavior.AllowGet);
                }

                sizes.ForEach(a =>
                {
                    a.Selected = false;
                });

                var nextSize = sizes[nextIndex];

                sizes[nextIndex].Selected = true;

                ViewBag.SelectedSize = nextSize;
                ViewBag.SelectedIndex = nextIndex;
                Session["Sizes"] = sizes;

                return
                    Json(
                        new
                        {
                            isNext = true,
                            width = nextSize.Width,
                            height = nextSize.Height,
                            fileName = $"/Content/{filename}"
                        }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //There was an error adding the image
                return Json(new { Success = "False", responseText = "There was an error adding the image. Error description: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public void SaveImage(string saveName, string base64)
        {
            Tool.SaveImage(saveName, base64);
        }
    }
}