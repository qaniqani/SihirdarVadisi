using System.Collections.Generic;
using System.Threading.Tasks;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Tools.Models;

namespace Tools.Service.Interface
{
    public interface IPlayerService
    {
        Task<PlayerStat> GetHowToPlayedTime(Region region, string username);

        Task<List<ChampionSummaryK>> GetHowMuckYourK(string username, Platform platform = Platform.TR1,
            Region region = Region.tr);
    }
}