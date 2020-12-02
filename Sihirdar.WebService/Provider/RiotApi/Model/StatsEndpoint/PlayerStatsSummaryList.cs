using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint
{
    class PlayerStatsSummaryList
    {
        [JsonProperty("playerStatSummaries")]
        public List<PlayerStatsSummary> PlayerStatSummaries { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }
    }
}
