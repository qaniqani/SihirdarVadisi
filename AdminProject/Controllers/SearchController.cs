using System.Web.Mvc;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly IContentService _contentService;
        private readonly int _take;

        public SearchController(IContentService contentService)
        {
            _take = 6;
            _contentService = contentService;
        }

        [Route("arama")]
        public ActionResult Search(string kelimeler, int page = 1)
        {
            if (string.IsNullOrEmpty(kelimeler))
                return Redirect("/");

            ViewBag.Key = kelimeler;
            TempData["CategoryUrl"] = "";

            var result = _contentService.GetCategoryContentSearch(kelimeler, CategoryTypes.Story, page, _take);

            return View(result);
        }
    }
}