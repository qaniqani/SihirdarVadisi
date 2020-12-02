using System.Linq;
using Tools.Models;
using System.Web.Http;
using Tools.Service.Interface;

namespace Tools.Controllers
{
    [RoutePrefix("api")]
    public class AovChampApiController : ApiController
    {
        private readonly IAovChampService _champService;
        private readonly IAovSkinService _skinService;
        private readonly IAovSpellService _spellService;
        private readonly IAovChampSkinAssgnService _champSkinService;
        private readonly IAovChampSpellAssgnService _champSpellService;

        public AovChampApiController(IAovChampService champService, IAovSkinService skinService, IAovSpellService spellService, IAovChampSkinAssgnService champSkinService, IAovChampSpellAssgnService champSpellService)
        {
            _champService = champService;
            _skinService = skinService;
            _spellService = spellService;
            _champSkinService = champSkinService;
            _champSpellService = champSpellService;
        }

        [HttpGet]
        [Route("aov/champ/{id}")]
        public IHttpActionResult ChampDetail(int id)
        {
            var champ = _champService.Get(id);
            if (champ == null)
                return BadRequest();

            var skinIds = _champSkinService.List(id).Select(a => a.SkillId).ToArray();
            var spellIds = _champSpellService.List(id).Select(a => a.SpellId).ToArray(); ;

            var skins = _skinService.GetChampSkins(skinIds);
            var spells = _spellService.GetChampSpells(spellIds);

            var result = new AovChampDetailDto
            {
                Gold = champ.Gold,
                Id = champ.Id,
                Image = $"/Content/{champ.Image}.jpg",
                InfoAttack = champ.InfoAttack,
                InfoConst = champ.InfoConst,
                InfoDifficulty = champ.InfoDifficulty,
                InfoMagic = champ.InfoMagic,
                Key = champ.Key,
                Lore = champ.Lore,
                Name = champ.Name,
                Partype = champ.Partype,
                Quote = champ.Quote,
                Role = champ.Role,
                Ticket = champ.Ticket,
                Title = champ.Title,
                Stats = new StatsDto
                {
                    Ad = champ.Ad,
                    Adperlevel = champ.Adperlevel,
                    Ap = champ.Ap,
                    Apperlevel = champ.Apperlevel,
                    Armor = champ.Armor,
                    ArmorPerLevel = champ.ArmorPerLevel,
                    As = champ.As,
                    AsPerLevel = champ.AsPerLevel,
                    Cd = champ.Cd,
                    CdPerLevel = champ.CdPerLevel,
                    Critic = champ.Critic,
                    CriticPerLevel = champ.CriticPerLevel,
                    Hp = champ.Hp,
                    Hpperlevel = champ.Hpperlevel,
                    Hpregen = champ.Hpregen,
                    Hpregenperlevel = champ.Hpregenperlevel,
                    Manaregen = champ.Manaregen,
                    Manaregenperlevel = champ.Manaregenperlevel,
                    Movement = champ.Movement,
                    MovementPerLevel = champ.MovementPerLevel,
                    Mr = champ.Mr,
                    MrPerLevel = champ.MrPerLevel
                },
                Skins = skins.Select(a => new SkinDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Num = a.Num
                }).ToList(),
                Spells = spells.Select(a => new SpellDto
                {
                    Description = a.Description,
                    Id = a.Id,
                    Num = a.Num,
                    Name = a.Name,
                    Image = $"/Content/{a.Image}.jpg"
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("aov/champs")]
        public IHttpActionResult Champs()
        {
            var champs = _champService
                .List(Sihirdar.DataAccessLayer.StatusTypes.Active)
                .Select(a => new AovChampDto
                {
                    Id = a.Id,
                    Image = $"/Content/{a.Image}.jpg",
                    Name = a.Name
                }).ToList();

            return Ok(champs);
        }
    }
}
