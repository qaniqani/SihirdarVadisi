using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.ProfileIcons
{
    public class ProfileIconStatic
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("image")]
        public ImageStatic Image { get; set; }
    }
}
