using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using reCAPTCHA.MVC;

namespace AdminProject.Controllers
{
    public class StaticPagesController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IThirtPartService _thirtPartService;
        private readonly IContentService _contentService;

        public StaticPagesController(IEmailService emailService, IThirtPartService thirtPartService, IContentService contentService)
        {
            _emailService = emailService;
            _thirtPartService = thirtPartService;
            _contentService = contentService;
        }

        [HttpGet]
        [Route("iletisim/iletisim-bilgileri")]
        public ActionResult Iletisim()
        {
            ViewBag.SubjectType = DropdownTypes.GetSubjectType(SubjectTypes.NotSelected);
            return View(new ContactModelDto {Subject = SubjectTypes.NotSelected});
        }

        [HttpPost]
        [CaptchaValidator(
        ErrorMessage = "Doğrulama kodu hatalı.",
        RequiredMessage = "Lütfen doğrulama kodunu giriniz.")]
        [Route("iletisim/iletisim-bilgileri")]
        public ActionResult Iletisim(ContactModelDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
                ModelState.AddModelError("Email", "Email adresi zorunludur.");

            if (string.IsNullOrEmpty(request.Message))
                ModelState.AddModelError("Message", "Mesaj zorunludur.");

            if (string.IsNullOrEmpty(request.NameSurname))
                ModelState.AddModelError("NameSurname", "İsim ve soyisim zorunludur.");

            if (request.Subject == SubjectTypes.NotSelected)
                ModelState.AddModelError("Subject", "Lütfen konuyu belirtiniz.");

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "asd";
                ViewBag.SubjectType = DropdownTypes.GetSubjectType(request.Subject);
                return View(request);
            }

            ViewBag.Ok = "asd";
            _emailService.SendContactMail("1 New Contact Form", request);

            ViewBag.SubjectType = DropdownTypes.GetSubjectType(SubjectTypes.NotSelected);

            return View(new ContactModelDto { Subject = SubjectTypes.NotSelected });
        }

        [HttpGet]
        [Route("turnuva-kayit")]
        public ActionResult Tournament()
        {
            if (Tool.UserCheck() != null)
            {
                var userId = Tool.UserCheck().Id;
                var tournament = _thirtPartService.GetTournament(userId);
                return View(tournament);
            }

            return View(new TournamentSaveModelDto());
        }

        [HttpPost]
        [CaptchaValidator(
            ErrorMessage = "Doğrulama kodu hatalı.",
            RequiredMessage = "Lütfen doğrulama kodunu giriniz.")]
        [Route("turnuva-kayit")]
        public ActionResult Tournament(TournamentSaveModelDto request)
        {
            if (Tool.UserCheck() == null)
            {
                ModelState.AddModelError("UserLogin", "Lütfen önce üyelik girişi yapınız.");

                ViewBag.Error = "asd";
                return View(request);
            }

            if (string.IsNullOrEmpty(request.TeamName))
                ModelState.AddModelError("TeamName", "Takım adı zorunludur.");

            if (string.IsNullOrEmpty(request.GameName))
                ModelState.AddModelError("GameName", "Oyun adı zorunludur.");

            if (string.IsNullOrEmpty(request.Phone))
                ModelState.AddModelError("Phone", "Telefon zorunludur.");

            if (string.IsNullOrEmpty(request.Username1))
                ModelState.AddModelError("Username1", "1. Kullancı adı zorunludur.");

            if (string.IsNullOrEmpty(request.Username2))
                ModelState.AddModelError("Username2", "2. Kullancı adı zorunludur.");

            if (string.IsNullOrEmpty(request.Username3))
                ModelState.AddModelError("Username3", "3. Kullancı adı zorunludur.");

            if (string.IsNullOrEmpty(request.Username4))
                ModelState.AddModelError("Username4", "4. Kullancı adı zorunludur.");

            if (string.IsNullOrEmpty(request.Username5))
                ModelState.AddModelError("Username5", "5. Kullancı adı zorunludur.");

            if (string.IsNullOrEmpty(request.UserNick1))
                ModelState.AddModelError("UserNick1", "1. Kullancı takma adı zorunludur.");

            if (string.IsNullOrEmpty(request.UserNick2))
                ModelState.AddModelError("UserNick2", "2. Kullancı takma adı zorunludur.");

            if (string.IsNullOrEmpty(request.UserNick3))
                ModelState.AddModelError("UserNick3", "3. Kullancı takma adı zorunludur.");

            if (string.IsNullOrEmpty(request.UserNick4))
                ModelState.AddModelError("UserNick4", "4. Kullancı takma adı zorunludur.");

            if (string.IsNullOrEmpty(request.UserNick5))
                ModelState.AddModelError("UserNick5", "5. Kullancı takma adı zorunludur.");

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "asd";
                return View(request);
            }

            var user = Tool.UserCheck();
            request.UserId = user.Id;

            if(request.Id == 0)
                _thirtPartService.AddTournament(request);
            else
                _thirtPartService.EditTournament(request, user.Id);


            ViewBag.Ok = "asd";

            return View(new TournamentSaveModelDto());
        }


        [Route("api/aov/champ/{id}")]
        public JsonResult ChampDetail(int id)
        {


            return Json("", JsonRequestBehavior.AllowGet);
        }

        [Route("araclar/testler")]
        public ActionResult Test()
        {
            TempData["CategoryUrl"] = "vadiden-haberler";
            return View();
        }

        [Route("araclar/testler/lolde-hangi-nisancisin")]
        public ActionResult Test1()
        {
            TempData["CategoryUrl"] = "vadiden-haberler";
            return View();
        }

        [Route("araclar/testler/hangi-mid-sampiyonusun")]
        public ActionResult Test2()
        {
            TempData["CategoryUrl"] = "takim-haberleri";
            return View();
        }

        [Route("araclar/testler/aslinda-hangi-kumedesin")]
        public ActionResult TestLol3Kume()
        {
            TempData["CategoryUrl"] = "takim-haberleri";
            return View();
        }

        [Route("araclar/testler/lol-bilginizi-olcuyoruz")]
        public ActionResult TestLol4()
        {
            TempData["CategoryUrl"] = "takim-haberleri";
            return View();
        }

        [Route("page/content")]
        public ActionResult PingContentPage(int take = 10)
        {
            take = take < 1 ? 10 : take;

            take = take > 20 ? 20 : take;

            var contents = _contentService.GetTopContent(take);

            return View(contents);
        }
    }
}