using System.Web.Mvc;

namespace AdminProject.Controllers
{
    public class ToolsController : Controller
    {
        // GET: Tools
        [Route("ping")]
        public ActionResult Ping()
        {
            return RedirectPermanent("http://araclar.gamerpeople.com/ping");
            //TempData["CategoryUrl"] = "vadiden-haberler";
            //return View();
        }

        [Route("sampiyonlar")]
        public ActionResult Sampiyonlar()
        {
            return View();
        }

        [Route("seninki-kac-k")]
        public ActionResult WhatYourK()
        {
            return RedirectPermanent("http://araclar.gamerpeople.com/seninki-kac-k");
        }

        [Route("kac-saat-lol-oynadim")]
        public ActionResult HourToPlay()
        {
            return RedirectPermanent("http://araclar.gamerpeople.com/kac-saat-lol-oynadin");
        }
    }
}