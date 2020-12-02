using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.TournamentEndpoint.Enums.Converters;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.TournamentEndpoint.Enums
{
    /// <summary>
    ///     Mode of the game (Tournament API).
    /// </summary>
    [JsonConverter(typeof(TournamentMapTypeConverter))]
    public enum TournamentMapType
    {
        /// <summary>
        /// Summoner's Rift map.
        /// </summary>
        SummonersRift,

        /// <summary>
        /// Twisted Treeline map.
        /// </summary>
        TwistedTreeline,

        /// <summary>
        /// Crystal Scar (Dominion) map.
        /// </summary>
        CrystalScar,

        /// <summary>
        /// Howling Abyss (ARAM) map.
        /// </summary>
        HowlingAbyss
    }
}
