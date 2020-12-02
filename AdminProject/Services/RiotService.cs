using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminProject.Services.Interface;
using Sihirdar.WebService.Provider.RiotApi.Interface;
using Sihirdar.WebService.Provider.RiotApi.Model.ChampionEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.SummonerEndpoint;

namespace AdminProject.Services
{
    public class RiotService : IRiotService
    {
        private readonly IRiotApi _riotApi;

        public RiotService(IRiotApi riotApi)
        {
            _riotApi = riotApi;
        }

        public List<PlayerStatsSummary> GetWhatYourK(string name, Region region)
        {
            var player = _riotApi.GetSummoner(region, name);
            var result = player.GetStatsSummaries();
            return result;
        }

        public List<PlayerStatsSummary> GetWhatYourK1(string name, Region region)
        {
            var player = _riotApi.GetSummoner(region, name);
            var result = player.GetStatsSummaries();
            return result;
        }
    }
}