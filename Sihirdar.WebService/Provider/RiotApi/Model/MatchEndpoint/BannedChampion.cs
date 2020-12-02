using Newtonsoft.Json;

namespace Sihirdar.WebService.Provider.RiotApi.Model.MatchEndpoint
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
