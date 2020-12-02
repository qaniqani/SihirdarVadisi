﻿using Newtonsoft.Json;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint
{
    /// <summary>
    /// Stats for all champions (Stats API).
    /// </summary>
    public class ChampionStats
    {
        internal ChampionStats() { }

        /// <summary>
        /// Champion ID. Note that champion ID 0 represents the combined stats for all champions.
        /// </summary>
        [JsonProperty("id")]
        public int ChampionId { get; set; }

        /// <summary>
        /// Champion stats associated with the champion.
        /// </summary>
        [JsonProperty("stats")]
        public ChampionStat Stats { get; set; }
    }
}
