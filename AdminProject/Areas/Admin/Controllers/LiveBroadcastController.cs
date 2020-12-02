using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class LiveBroadcastController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly ILiveBroadcastService _liveService;

        public LiveBroadcastController(ILiveBroadcastService liveService, RuntimeSettings setting) : base(setting)
        {
            _liveService = liveService;
            _setting = setting;
        }

        public ActionResult Add()
        {
            SetPageHeader("Live Broadcase", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameTypes.All);

            return View();
        }

        [HttpPost]
        public ActionResult Add(GameTypes GameType, string Name, string PublishAddress, string ChatAddress, string Description, string Url, bool Live, string SequenceNumber, StatusTypes Status)
        {
            SetPageHeader("Live Broadcase", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(PublishAddress))
                ModelState.AddModelError("PublishAddress", "Publish Address is required.");

            if (string.IsNullOrEmpty(ChatAddress))
                ModelState.AddModelError("ChatAddress", "Chat Address is required.");

            if (!ModelState.IsValid)
                return View();

            var url = string.IsNullOrEmpty(Url.Trim()) ? Utility.UrlSeo(Name.Trim()) : Utility.UrlSeo(Url.Trim());
            url = _liveService.UrlCheck(url);

            var live = new LiveBroadcast
            {
                ChatAddress = ChatAddress,
                Description = Description,
                GameType = GameType,
                Language = _setting.Language,
                LanguageId = _setting.LanguageId,
                Live = Live,
                Name = Name,
                PublishAddress = PublishAddress,
                Url = url,
                SequenceNumber = !string.IsNullOrEmpty(SequenceNumber) ? SequenceNumber.ToInt32() : 9999,
                Status = Status
            };

            _liveService.Add(live);

            Added();

            return View();
        }

        public ActionResult Edit(int id)
        {
            SetPageHeader("Live Broadcase", "Edit");

            var live = _liveService.GetItem(id);
            if (live == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(live.Status);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(live.GameType);

            return View(live);
        }

        [HttpPost]
        public ActionResult Edit(int id, GameTypes GameType, string Name, string PublishAddress, string ChatAddress, string Description, string Url, bool Live, string SequenceNumber, StatusTypes Status)
        {
            SetPageHeader("Live Broadcase", "Edit");

            var live = _liveService.GetItem(id);
            if (live == null)
            {
                Warning();
                RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.GameTypeList = DropdownTypes.GetGameType(GameType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(PublishAddress))
                ModelState.AddModelError("PublishAddress", "Publish Address is required.");

            if (string.IsNullOrEmpty(ChatAddress))
                ModelState.AddModelError("ChatAddress", "Chat Address is required.");

            if (string.IsNullOrEmpty(Url))
                ModelState.AddModelError("Url", "Url is required.");

            if (!ModelState.IsValid)
                return View(live);

            live.Url = Utility.UrlSeo(Url.Trim());
            live.ChatAddress = ChatAddress;
            live.Description = Description;
            live.GameType = GameType;
            live.Language = _setting.Language;
            live.LanguageId = _setting.LanguageId;
            live.Live = Live;
            live.Name = Name;
            live.PublishAddress = PublishAddress;
            live.SequenceNumber = !string.IsNullOrEmpty(SequenceNumber) ? SequenceNumber.ToInt32() : 9999;
            live.Status = Status;

            _liveService.Edit(id, live);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("Live Broadcase", "List");

            var lives = _liveService.List();

            return View(lives);
        }

        public ActionResult Delete(int id)
        {
            var live = _liveService.GetItem(id);
            if (live == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            _liveService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}