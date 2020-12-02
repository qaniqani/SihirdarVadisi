using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Sihirdar.WebServiceV3.Provider.RiotApi;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Tools.Service.Interface;
using Tools.Utility;

namespace Tools.Controllers
{
    [System.Web.Http.RoutePrefix("api")]
    public class GeneralController : ApiController
    {
        private readonly IPlayerService _playerService;

        public GeneralController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("hour-play-lol")]
        //[OutputCache(Duration = 1800, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "username")]
        public async Task<IHttpActionResult> HowManyHourPlay(string username, Region region = Region.tr)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest("Kullanıcı adı zorunludur.");

            username = Common.UsernameConvert(username);

            if (string.IsNullOrEmpty(username))
                return BadRequest("Kullanıcı adı zorunludur.");

            try
            {
                var result = await _playerService.GetHowToPlayedTime(region, username);
                return Ok(result);
            }
            catch (RiotSharpException rsEx)
            {
                return BadRequest(ErrorCheck.GetErrorMessage(rsEx.HttpStatusCode, rsEx.Message));
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu. Hata detayı: {ex.Message}");
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("how-much-level")]
        [OutputCache(Duration = 1800, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "username")]
        public async Task<IHttpActionResult> HowMuchK(string username, Platform platform = Platform.TR1, Region region = Region.tr)
        {
            //return Ok();
            if (string.IsNullOrEmpty(username))
                return BadRequest("Kullanıcı adı zorunludur.");

            username = Common.UsernameConvert(username);

            if (string.IsNullOrEmpty(username))
                return BadRequest("Kullanıcı adı zorunludur.");

            try
            {
                var result = await _playerService.GetHowMuckYourK(username, platform, region);

                return Ok(result);
            }
            catch (RiotSharpException rsEx)
            {
                return BadRequest(ErrorCheck.GetErrorMessage(rsEx.HttpStatusCode, rsEx.Message));
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu. Hata detayı: {ex.Message}");
            }
        }
    }
}