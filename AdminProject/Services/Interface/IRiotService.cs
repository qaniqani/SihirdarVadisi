using System.Collections.Generic;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.SummonerEndpoint;

namespace AdminProject.Services.Interface
{
    public interface IRiotService
    {
        List<PlayerStatsSummary> GetWhatYourK(string name, Region region);
        List<PlayerStatsSummary> GetWhatYourK1(string name, Region region);
    }
}