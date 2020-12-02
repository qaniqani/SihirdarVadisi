using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebService.Provider.RiotApi.Model.SummonerEndpoint
{
    class SummonerBaseList
    {
        [JsonProperty("summoners")]
        public List<SummonerBase> Summoners { get; set; }
    }
}
