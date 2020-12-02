using System;
using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc.Converters;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint.Enums;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint
{
    /// <summary>
    /// Stats summary of a player (Stats API).
    /// </summary>
    public class PlayerStatsSummary
    {
        public PlayerStatsSummary() { }

        /// <summary>
        /// Aggregated stats.
        /// </summary>
        [JsonProperty("aggregatedStats")]
        public AggregatedStat AggregatedStats { get; set; }

        /// <summary>
        /// Number of losses for this queue type. Returned for ranked queue types only.
        /// </summary>
        [JsonProperty("losses")]
        public int Losses { get; set; }

        /// <summary>
        /// Date stats were last modified specified as epoch milliseconds.
        /// </summary>
        [JsonProperty("modifyDate")]
        [JsonConverter(typeof(DateTimeConverterFromLong))]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Player stats summary type.
        /// </summary>
        [JsonProperty("playerStatSummaryType")]
        public PlayerStatsSummaryType PlayerStatSummaryType { get; set; }

        /// <summary>
        /// Number of wins for this queue type.
        /// </summary>
        [JsonProperty("wins")]
        public int Wins { get; set; }
    }
}
