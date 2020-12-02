using System.Linq;
using System.Web.Mvc;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Controllers
{
    public class LiveStreamController : BaseController
    {
        private readonly IAdvertService _advertService;
        private readonly ILiveBroadcastService _streamService;

        public LiveStreamController(ILiveBroadcastService streamService, IAdvertService advertService)
        {
            _streamService = streamService;
            _advertService = advertService;
        }

        [Route("canli-yayin")]
        public ActionResult LiveBroadcast()
        {
            GetCategoryAdverts();
            return View();
        }

        [Route("canli-yayin/{url}")]
        public ActionResult LiveBroadcast(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Redirect("/");

            var liveStream = _streamService.GetItem(url);
            if(liveStream == null)
                return Redirect("/");

            GetCategoryAdverts();

            return View(liveStream);
        }

        private void GetCategoryAdverts()
        {
            var advert = _advertService.GetCategoryAdverts("tr", AdvertLocationTypes.Live728X90).FirstOrDefault();

            if (advert != null)
                TypeAdvert(advert);
        }
    }
}