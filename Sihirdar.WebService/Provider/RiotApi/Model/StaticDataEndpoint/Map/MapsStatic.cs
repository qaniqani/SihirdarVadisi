using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Map
{
    class MapsStatic
    {
        /// <summary>
        /// Map of id to map.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<int, MapStatic> Data { get; set; }
    }
}
