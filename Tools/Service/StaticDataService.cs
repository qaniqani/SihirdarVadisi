using System.Threading.Tasks;
using Sihirdar.WebServiceV3.Provider.RiotApi;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Champion;
using Tools.Service.Interface;

namespace Tools.Service
{
    public class StaticDataService : IStaticDataService
    {
        private readonly RiotApiConfig _config;

        public StaticDataService(RiotApiConfig config)
        {
            _config = config;
        }

        public async Task<ChampionStatic> GetChampDetail(Region region, int champId)
        {
            var staticApi = StaticRiotApi.GetInstance(_config.ApiKey);

            var champData = await staticApi.GetChampionAsync(region, champId, ChampionData.all, Language.tr_TR);

            return champData;
        }

        public async Task<ChampionListStatic> GetAllChampList(Region region, ChampionData championData, Language language)
        {
            var staticApi = StaticRiotApi.GetInstance(_config.ApiKey);

            var champs = await staticApi.GetChampionsAsync(region, championData, language);
            return champs;
        }
    }
}