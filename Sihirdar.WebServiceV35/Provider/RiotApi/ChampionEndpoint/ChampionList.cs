using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.ChampionEndpoint
{
    public class ChampionList
    {
        /// <summary>
        /// List of Champions.
        /// </summary>
        [JsonProperty("champions")]
        public List<Champion> Champions { get; set; }
    }
}
