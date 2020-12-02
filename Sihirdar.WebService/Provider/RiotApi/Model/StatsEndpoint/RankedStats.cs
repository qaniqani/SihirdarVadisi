using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint
{
    class RankedStats
    {
        [JsonProperty("champions")]
        public List<ChampionStats> ChampionStats { get; set; }

        [JsonProperty("modifyDate")]
        [JsonConverter(typeof(DateTimeConverterFromLong))]
        public DateTime ModifyDate { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }
    }
}
