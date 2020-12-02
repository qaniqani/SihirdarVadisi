using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint
{
    /// <summary>
    /// Details about a match (Match API).
    /// </summary>
    public class MatchDetail : MatchSummary
    {
        internal MatchDetail() { }

        /// <summary>
        /// Team information.
        /// </summary>
        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        /// <summary>
        /// Match timeline data. Not included by default.
        /// </summary>
        [JsonProperty("timeline")]
        public Timeline Timeline { get; set; }
    }
}
