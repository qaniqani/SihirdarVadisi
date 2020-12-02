using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.SummonerEndpoint
{
    public class MasteryPages
    {
        /// <summary>
        /// List of MasteryPages.
        /// </summary>
        [JsonProperty("pages")]
        public List<MasteryPage> Pages { get; set; }

        /// <summary>
        /// Summoner ID to wich the pages belong.
        /// </summary>
        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }
    }
}