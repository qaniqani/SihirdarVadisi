using System.Collections.Generic;
using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.CurrentGameEndpoint;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.FeaturedGamesEndpoint
{
    /// <summary>
    /// Class representing Featured Games in the API.
    /// </summary>
    public class FeaturedGames
    {
        /// <summary>
        /// The suggested interval to wait before requesting FeaturedGames again
        /// </summary>
        [JsonProperty("clientRefreshInterval")]
        public long ClientRefreshInterval { get; set; }

        /// <summary>
        /// The list of featured games
        /// </summary>
        [JsonProperty("gameList")]
        public List<CurrentGame> GameList { get; set; }
    }
}
