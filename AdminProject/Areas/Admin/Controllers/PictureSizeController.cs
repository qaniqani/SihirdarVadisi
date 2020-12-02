using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class PictureSizeController : BaseController
    {
        private readonly IPictureSizeService _pictureSizeService;

        public PictureSizeController(IPictureSizeService pictureSizeService, RuntimeSettings setting) : base(setting)
        {
            _pictureSizeService = pictureSizeService;
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Add()
        {
            SetPageHeader("Picture Size", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.ContentPictureList = DropdownTypes.GetContentType(ContentTypes.Content);

            return View();
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Add(string Name, ContentTypes PictureType, StatusTypes Status)
        {
            SetPageHeader("Picture Size", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.ContentPictureList = DropdownTypes.GetContentType(ContentTypes.Content);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (!ModelState.IsValid)
                return View();

            var pictureSize = new PictureSize
            {
                Name = Name,
                PictureType = PictureType,
                Status = Status
            };

            _pictureSizeService.Add(pictureSize);

            Added();

            return View();
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Picture Size", "Edit");

            var size = _pictureSizeService.GetItem(id);
            if (size == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(size.Status);
            ViewBag.ContentPictureList = DropdownTypes.GetContentType(size.PictureType);

            return View(size);
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult Edit(int id, string Name, ContentTypes PictureType, StatusTypes Status)
        {
            SetPageHeader("Picture Size", "Edit");

            var size = _pictureSizeService.GetItem(id);
            if (size == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            size.Name = Name;
            size.PictureType = PictureType;
            size.Status = Status;
            _pictureSizeService.Edit(id, size);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult List()
        {
            SetPageHeader("Picture Size", "List");

            var sizes = _pictureSizeService.List();

            return View(sizes);
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult Delete(int id)
        {
            _pictureSizeService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult AddSize(int id)
        {
            ViewBag.Id = id;

            SetPageHeader("Picture Size Detail", "Add");

            var size = _pictureSizeService.GetItem(id);

            ViewBag.Size = size;

            //ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult AddSize(int id, string Width, string Height)
        {
            ViewBag.Id = id;

            SetPageHeader("Picture Size Detail", "Add");

            var selectedSize = _pictureSizeService.GetItem(id);

            ViewBag.Size = selectedSize;

            if (string.IsNullOrEmpty(Width))
                ModelState.AddModelError("Width", "Width is required.");

            if (string.IsNullOrEmpty(Width))
                ModelState.AddModelError("Height", "Height is required.");

            if (!ModelState.IsValid)
                return View();

            var size = new PictureSizeDetail
            {
                Height = Height.ToInt32(),
                Name = $"_{Width}x{Height}",
                SizeId = id,
                Width = Width.ToInt32(),
                Status = StatusTypes.Active
            };

            _pictureSizeService.AddSizeDetail(size);

            Added();

            return View(new PictureSizeDetail());
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult EditSize(int id, int sizeId)
        {
            ViewBag.Id = id;

            SetPageHeader("Picture Size Detail", "Edit");

            //ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            var selectedSize = _pictureSizeService.GetItem(id);

            ViewBag.Size = selectedSize;

            var size = _pictureSizeService.GetSizeDetail(sizeId);

            return View(size);
        }

        [HttpPost]
        [AdminAuth]
        public ActionResult EditSize(int id, int sizeId, string Width, string Height)
        {
            ViewBag.Id = id;

            SetPageHeader("Picture Size Detail", "Edit");

            var selectedSize = _pictureSizeService.GetItem(id);

            ViewBag.Size = selectedSize;

            var size = _pictureSizeService.GetSizeDetail(sizeId);

            if (string.IsNullOrEmpty(Width))
                ModelState.AddModelError("Width", "Width is required.");

            if (string.IsNullOrEmpty(Width))
                ModelState.AddModelError("Height", "Height is required.");

            if (!ModelState.IsValid)
                return View(size);

            size.Height = Height.ToInt32();
            size.Name = $"_{Width}x{Height}";
            size.Width = Width.ToInt32();
            size.Status = StatusTypes.Active;

            _pictureSizeService.EditSizeDetail(sizeId, size);

            Updated();

            return Redirect($"/Admin/PictureSize/ListSize/{id}");
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult ListSize(int id)
        {
            var size = _pictureSizeService.GetItem(id);

            ViewBag.Size = size;

            var list = _pictureSizeService.ListSizeDetail(id);

            return View(list);
        }

        [HttpGet]
        [AdminAuth]
        public ActionResult DeleteSize(int id, int sizeId)
        {
            _pictureSizeService.DeleteSizeDetail(sizeId);

            Deleted();

            return RedirectToAction("ListSize", new {id});
        }
    }
}