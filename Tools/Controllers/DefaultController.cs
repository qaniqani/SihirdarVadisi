using System.Web.Mvc;

namespace Tools.Controllers
{
    public class DefaultController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("kac-saat-lol-oynadin")]
        public ActionResult LolTimePlayed()
        {
            return View();
        }

        [Route("ping")]
        public ActionResult Ping()
        {
            return View();
        }

        [Route("seninki-kac-k")]
        public ActionResult LolChampionsK()
        {
            return View();
        }

        [Route("sampiyonlar")]
        public ActionResult Champions()
        {
            return View();
        }

        [Route("aov-sampiyonlar")]
        public ActionResult AovChampions()
        {
            return View();
        }

        [Route("lol-terimler-sozlugu")]
        public ActionResult TermDictionary()
        {
            return View();
        }

        [Route("mavi-oz-hesapla")]
        public ActionResult MaviOzHesapla()
        {
            return View();
        }

        [Route("sampiyon-rehberi")]
        public ActionResult SampiyonRehberi()
        {
            return View();
        }
    }
}