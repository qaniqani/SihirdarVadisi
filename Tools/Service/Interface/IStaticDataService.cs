using System.Threading.Tasks;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Champion;

namespace Tools.Service.Interface
{
    public interface IStaticDataService
    {
        Task<ChampionStatic> GetChampDetail(Region region, int champId);
        Task<ChampionListStatic> GetAllChampList(Region region, ChampionData championData, Language language);
    }
}