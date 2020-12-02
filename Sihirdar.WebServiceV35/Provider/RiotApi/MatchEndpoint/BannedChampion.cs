using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint
{
    /// <summary>
    /// Class representing a banned champion (Game API).
    /// </summary>
    public class BannedChampion
    {
        internal BannedChampion() { }

        /// <summary>
        /// Banned champion ID.
        /// </summary>
        [JsonProperty("championId")]
        public int ChampionId { get; set; }

        /// <summary>
        /// Turn during which the champion was banned.
        /// </summary>
        [JsonProperty("pickTurn")]
        public int PickTurn { get; set; }
    }
}
