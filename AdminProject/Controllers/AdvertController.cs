using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    public class AdvertController : Controller
    {
        private readonly IAdvertService _advertService;

        public AdvertController(IAdvertService advertService)
        {
            _advertService = advertService;
        }

        [Route("advert/redirect")]
        public ActionResult RedirectLink(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return RedirectPermanent("/");

            var advertDetail = _advertService.GetAdvert(guid);
            if (advertDetail == null)
                return RedirectPermanent("/");

            var redirectLink = advertDetail.AdLink;
            return RedirectLink(redirectLink);
        }
    }
}