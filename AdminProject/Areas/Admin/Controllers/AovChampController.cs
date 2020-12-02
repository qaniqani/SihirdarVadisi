using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using System.Linq;

namespace AdminProject.Areas.Admin.Controllers
{
    public class AovChampController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly IAovSkinService _skinService;
        private readonly IAovSpellService _spellService;
        private readonly IAovChampService _champService;
        private readonly IAovChampSkinAssgnService _champSkinService;
        private readonly IAovChampSpellAssgnService _champSpellService;

        public AovChampController(IAovSkinService skinService, RuntimeSettings setting, IAovSpellService spellService, IAovChampService champService, IAovChampSkinAssgnService champSkinService, IAovChampSpellAssgnService champSpellService) : base(setting)
        {
            _setting = setting;
            _skinService = skinService;
            _spellService = spellService;
            _champService = champService;
            _champSkinService = champSkinService;
            _champSpellService = champSpellService;
        }

        public ActionResult Add()
        {
            SetPageHeader("AOV Champ", "Add New Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.Spells = _spellService.List(StatusTypes.Active);
            ViewBag.Skins = _skinService.List(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, string Title, string Key, HttpPostedFileBase Image, string Role, string Partype, string Lore, string Quote, StatusTypes Status, string[] spellId, string[] skinId,
            int Gold = 0, int Ticket = 0, int InfoDifficulty = 0, int InfoAttack = 0, int InfoConst = 0, int InfoMagic = 0, int Ad = 0, int Adperlevel = 0, int Ap = 0, int Apperlevel = 0, int Hp = 0, int Hpperlevel = 0, int Hpregen = 0, int Hpregenperlevel = 0, int Manaregen = 0, int Manaregenperlevel = 0, int Armor = 0, int ArmorPerLevel = 0, int Mr = 0, int MrPerLevel = 0, int As = 0, int AsPerLevel = 0, int Cd = 0, int CdPerLevel = 0, int Critic = 0, int CriticPerLevel = 0, int Movement = 0, int MovementPerLevel = 0)
        {
            SetPageHeader("AOV Champ", "Add New Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.Spells = _spellService.List(StatusTypes.Active);
            ViewBag.Skins = _skinService.List(StatusTypes.Active);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Title))
                ModelState.AddModelError("Title", "Title is required.");

            if (string.IsNullOrEmpty(Key))
                ModelState.AddModelError("Key", "Key is required.");

            if (string.IsNullOrEmpty(Role))
                ModelState.AddModelError("Role", "Role is required.");

            if (string.IsNullOrEmpty(Partype))
                ModelState.AddModelError("Partype", "Partype is required.");

            if (string.IsNullOrEmpty(Lore))
                ModelState.AddModelError("Lore", "Lore is required.");

            if (string.IsNullOrEmpty(Quote))
                ModelState.AddModelError("Quote", "Quote is required.");

            if (Image == null)
                ModelState.AddModelError("Image", "Image is required.");

            if (!ModelState.IsValid)
                return View();

            var champ = new AovChamp
            {
                Ad = Ad,
                Adperlevel = Adperlevel,
                Ap = Ap,
                Apperlevel = Apperlevel,
                Armor = Armor,
                ArmorPerLevel = ArmorPerLevel,
                As = As,
                AsPerLevel = AsPerLevel,
                Cd = Cd,
                CdPerLevel = CdPerLevel,
                Critic = Critic,
                CriticPerLevel = CriticPerLevel,
                Gold = Gold,
                Hp = Hp,
                Hpperlevel = Hpperlevel,
                Hpregen = Hpregen,
                Hpregenperlevel = Hpregenperlevel,
                InfoAttack = InfoAttack,
                InfoConst = InfoConst,
                InfoDifficulty = InfoDifficulty,
                InfoMagic = InfoMagic,
                Key = Key,
                Lore = Lore,
                Manaregen = Manaregen,
                Manaregenperlevel = Manaregenperlevel,
                Movement = Movement,
                MovementPerLevel=MovementPerLevel,
                Mr = Mr,
                MrPerLevel = MrPerLevel,
                Name = Name,
                Partype = Partype,
                Quote = Quote,
                Role = Role,
                Status = Status,
                Ticket = Ticket,
                Title = Title
            };

            var fileName = Image.FileName;
            var extension = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(extension))
                ModelState.AddModelError("Extension", "Image extension not found.");

            if (!_setting.PictureMimeType.Contains(Image.ContentType))
                ModelState.AddModelError("MimeType",
                    $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

            if (!_setting.PictureExtensionTypes.Contains(extension))
                ModelState.AddModelError("Extension",
                    $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

            if (!ModelState.IsValid)
                return View();

            var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
            var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
            try
            {
                Utility.FileUpload(Image, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                champ.Image = pictureName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("PictureUploadError", ex.Message);
                return View();
            }

            var result = _champService.Add(champ);

            var spell = spellId.Where(a => !string.IsNullOrEmpty(a)).ToArray().Select(a => new AovChampSpellAssng
            {
                ChampId = result.Id,
                SpellId = Convert.ToInt32(a)
            }).ToList();
            var skin = skinId.Where(a => !string.IsNullOrEmpty(a)).ToArray().Select(a => new AovChampSkinAssng
            {
                ChampId = result.Id,
                SkillId = Convert.ToInt32(a)
            }).ToList();

            _champSkinService.Add(skin);
            _champSpellService.Add(spell);

            Added();

            return RedirectToAction("Add");
        }

        public ActionResult Edit(int id)
        {
            var champ = _champService.Get(id);
            if (champ == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            SetPageHeader("AOV Champ", "Edit Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(champ.Status);
            ViewBag.Spells = _spellService.List(StatusTypes.Active);
            ViewBag.Skins = _skinService.List(StatusTypes.Active);

            var champSkins = _champSkinService.List(id).Select(a => a.SkillId).ToArray();
            var champSpell = _champSpellService.List(id).Select(a => a.SpellId).ToArray();

            ViewBag.ChampSkins = string.Join(",", champSkins);
            ViewBag.ChampSpells = string.Join(",", champSpell);

            return View(champ);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, string Title, string Key, HttpPostedFileBase Image, string Role, string Partype, string Lore, string Quote, StatusTypes Status, string[] spellId, string[] skinId,
            int Gold = 0, int Ticket = 0, int InfoDifficulty = 0, int InfoAttack = 0, int InfoConst = 0, int InfoMagic = 0, int Ad = 0, int Adperlevel = 0, int Ap = 0, int Apperlevel = 0, int Hp = 0, int Hpperlevel = 0, int Hpregen = 0, int Hpregenperlevel = 0, int Manaregen = 0, int Manaregenperlevel = 0, int Armor = 0, int ArmorPerLevel = 0, int Mr = 0, int MrPerLevel = 0, int As = 0, int AsPerLevel = 0, int Cd = 0, int CdPerLevel = 0, int Critic = 0, int CriticPerLevel = 0, int Movement = 0, int MovementPerLevel = 0)
        {
            var champ = _champService.Get(id);
            if (champ == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            SetPageHeader("AOV Champ", "Edit Champ");

            ViewBag.StatusList = DropdownTypes.GetStatus(champ.Status);
            ViewBag.Spells = _spellService.List(StatusTypes.Active);
            ViewBag.Skins = _skinService.List(StatusTypes.Active);

            var champSkins = _champSkinService.List(id).Select(a => a.SkillId).ToArray();
            var champSpell = _champSpellService.List(id).Select(a => a.SpellId).ToArray();

            ViewBag.ChampSkins = string.Join(",", champSkins);
            ViewBag.ChampSpells = string.Join(",", champSpell);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Title))
                ModelState.AddModelError("Title", "Title is required.");

            if (string.IsNullOrEmpty(Key))
                ModelState.AddModelError("Key", "Key is required.");

            if (string.IsNullOrEmpty(Role))
                ModelState.AddModelError("Role", "Role is required.");

            if (string.IsNullOrEmpty(Partype))
                ModelState.AddModelError("Partype", "Partype is required.");

            if (string.IsNullOrEmpty(Lore))
                ModelState.AddModelError("Lore", "Lore is required.");

            if (string.IsNullOrEmpty(Quote))
                ModelState.AddModelError("Quote", "Quote is required.");

            if (!ModelState.IsValid)
                return View(champ);

            champ.Ad = Ad;
            champ.Adperlevel = Adperlevel;
            champ.Ap = Ap;
            champ.Apperlevel = Apperlevel;
            champ.Armor = Armor;
            champ.ArmorPerLevel = ArmorPerLevel;
            champ.As = As;
            champ.AsPerLevel = AsPerLevel;
            champ.Cd = Cd;
            champ.CdPerLevel = CdPerLevel;
            champ.Critic = Critic;
            champ.CriticPerLevel = CriticPerLevel;
            champ.Gold = Gold;
            champ.Hp = Hp;
            champ.Hpperlevel = Hpperlevel;
            champ.Hpregen = Hpregen;
            champ.Hpregenperlevel = Hpregenperlevel;
            champ.InfoAttack = InfoAttack;
            champ.InfoConst = InfoConst;
            champ.InfoDifficulty = InfoDifficulty;
            champ.InfoMagic = InfoMagic;
            champ.Key = Key;
            champ.Lore = Lore;
            champ.Manaregen = Manaregen;
            champ.Manaregenperlevel = Manaregenperlevel;
            champ.Movement = Movement;
            champ.MovementPerLevel = MovementPerLevel;
            champ.Mr = Mr;
            champ.MrPerLevel = MrPerLevel;
            champ.Name = Name;
            champ.Partype = Partype;
            champ.Quote = Quote;
            champ.Role = Role;
            champ.Status = Status;
            champ.Ticket = Ticket;
            champ.Title = Title;

            if (Image != null)
            {
                var fileName = Image.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "Image extension not found.");

                if (!_setting.PictureMimeType.Contains(Image.ContentType))
                    ModelState.AddModelError("MimeType",
                        $"Only {string.Join(", ", _setting.PictureMimeType)} mime type upload.");

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension",
                        $"Only {string.Join(", ", _setting.PictureExtensionTypes)} upload.");

                if (!ModelState.IsValid)
                    return View(champ);

                var pictureName = Utility.UrlSeo($"{Name}-{fileName}-{DateTime.Now}");
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + ".jpg");
                try
                {
                    Utility.FileUpload(Image, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    champ.Image = pictureName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(champ);
                }
            }

            _champService.Edit(id, champ);

            _champSkinService.AllDeleteChampSkin(id);
            _champSpellService.AllDeleteChampSpell(id);

            var spell = spellId.Where(a => !string.IsNullOrEmpty(a)).ToArray().Select(a => new AovChampSpellAssng
            {
                ChampId = id,
                SpellId = Convert.ToInt32(a)
            }).ToList();
            var skin = skinId.Where(a => !string.IsNullOrEmpty(a)).ToArray().Select(a => new AovChampSkinAssng
            {
                ChampId = id,
                SkillId = Convert.ToInt32(a)
            }).ToList();

            _champSkinService.Add(skin);
            _champSpellService.Add(spell);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("AOV Champ", "Champ List");

            var champs = _champService.List();

            return View(champs);
        }

        public ActionResult Delete(int id)
        {
            var champ = _champService.Get(id);
            if (champ == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            _champService.Delete(id);
            _champSkinService.AllDeleteChampSkin(id);
            _champSpellService.AllDeleteChampSpell(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}