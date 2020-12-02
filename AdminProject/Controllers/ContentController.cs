using System.Web.Mvc;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    public class ContentController : BaseController
    {
        private readonly IContentService _contentService;
        private readonly ICategoryService _categoryService;

        public ContentController(IContentService contentService, ICategoryService categoryService)
        {
            _contentService = contentService;
            _categoryService = categoryService;
        }

        [Route("haber/{categoryUrl}/{contentUrl}")]
        public ActionResult ContentDetail(string categoryUrl, string contentUrl)
        {
            if (string.IsNullOrEmpty(contentUrl))
                return RedirectPermanent("/");

            try
            {
                var category = _categoryService.GetCategory(categoryUrl);
                if(category == null)
                    return Redirect("/");

                var content = _contentService.GetContentDetail(contentUrl);

                SetTitle(content.Content.Title);

                TempData["CategoryUrl"] = categoryUrl;
                ViewBag.Category = category;

                return View(content);
            }
            catch (CustomException cEx)
            {
                return Redirect("/");
            }
        }
    }
}