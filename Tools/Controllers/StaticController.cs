using System.Threading.Tasks;
using System.Web.Http;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Tools.Service.Interface;

namespace Tools.Controllers
{
    [RoutePrefix("api")]
    public class StaticController : ApiController
    {
        private readonly IStaticDataService _staticService;

        public StaticController(IStaticDataService staticService)
        {
            _staticService = staticService;
        }

        [HttpGet]
        [Route("champion/{id}")]
        public async Task<IHttpActionResult> GetChampDetail(int id, Region region = Region.tr)
        {
            var champDetail = await _staticService.GetChampDetail(region, id);

            return Ok(champDetail);
        }
    }
}