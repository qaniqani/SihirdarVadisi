using System.Web.Mvc;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    public class TagController : BaseController
    {
        private readonly IContentService _contentService;
        private readonly int _take;

        public TagController(IContentService contentService)
        {
            _contentService = contentService;
            _take = 6;
        }

        [Route("etiket/{tag}")]
        public ActionResult TagSearch(string tag, int page = 1)
        {
            if (string.IsNullOrEmpty(tag))
                return Redirect("/");

            try
            {
                ViewBag.Take = _take;
                var result = _contentService.GetTagSearch(tag, page, _take);

                var title = string.Format(Resources.Lang.tagSearchPageTitle, tag);
                SetTitle(title);
                TempData["CategoryUrl"] = "";
                ViewBag.Tag = tag;
                ViewBag.PageTitle = title;
                ViewBag.Page = page;

                return View(result);
            }
            catch (CustomException cEx)
            {
                SetErrorMessage(cEx.Message);
                return Redirect("/");
            }
        }
    }
}