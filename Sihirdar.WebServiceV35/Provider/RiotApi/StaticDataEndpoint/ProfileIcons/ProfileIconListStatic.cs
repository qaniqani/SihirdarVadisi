using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.ProfileIcons
{
    public class ProfileIconListStatic
    {
        [JsonProperty("data")]
        public Dictionary<string, ProfileIconStatic> ProfileIcons { get; set; }
    }
}
