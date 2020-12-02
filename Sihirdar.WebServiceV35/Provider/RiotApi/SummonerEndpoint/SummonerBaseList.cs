using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.SummonerEndpoint
{
    public class SummonerBaseList
    {
        [JsonProperty("summoners")]
        public List<SummonerBase> Summoners { get; set; }
    }
}
