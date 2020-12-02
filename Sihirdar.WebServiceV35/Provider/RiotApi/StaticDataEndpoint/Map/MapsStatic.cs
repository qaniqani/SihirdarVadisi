using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Map
{
    public class MapsStatic
    {
        /// <summary>
        /// Map of id to map.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<int, MapStatic> Data { get; set; }
    }
}
