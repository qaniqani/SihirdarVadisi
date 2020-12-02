using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.LeagueEndpoint.Enums.Converters;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.LeagueEndpoint.Enums
{
    /// <summary>
    /// Tier of the league (League API).
    /// </summary>
    [JsonConverter(typeof(TierConverter))]
    public enum Tier
    {
        /// <summary>
        /// Master tier.
        /// </summary>
        Master,

        /// <summary>
        /// Challenger tier.
        /// </summary>
        Challenger,

        /// <summary>
        /// Diamon tier.
        /// </summary>
        Diamond,

        /// <summary>
        /// Platinum tier.
        /// </summary>
        Platinum,

        /// <summary>
        /// Gold tier.
        /// </summary>
        Gold,

        /// <summary>
        /// Silver tier.
        /// </summary>
        Silver,

        /// <summary>
        /// Bronze tier.
        /// </summary>
        Bronze,
        
        /// <summary>
        /// Unranked.
        /// </summary>
        Unranked
    }
}
