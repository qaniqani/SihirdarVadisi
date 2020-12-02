using System.Linq;
using System.Web.Http;
using Sihirdar.DataAccessLayer;
using Tools.Service.Interface;

namespace Tools.Controllers
{
    [RoutePrefix("api")]
    public class ContentController : ApiController
    {
        private readonly IContentService _contentService;
        private readonly IAdvertService _advertService;

        public ContentController(IAdvertService advertService, IContentService contentService)
        {
            _advertService = advertService;
            _contentService = contentService;
        }

        [HttpGet]
        [Route("advert/right")]
        public IHttpActionResult GetCategoryAdverts()
        {
            const string url = "araclar";

            var adverts = _advertService.GetCategoryAdverts(url, "tr");

            var rightBlockAdverts = adverts.Where(a => a.Location == AdvertLocationTypes.RightBlock).ToList();
            return Ok(rightBlockAdverts);
        }

        [HttpGet]
        [Route("top-content")]
        public IHttpActionResult GetTopContent()
        {
            var contents = _contentService.GetTopContent();
            return Ok(contents);
        }
    }
}