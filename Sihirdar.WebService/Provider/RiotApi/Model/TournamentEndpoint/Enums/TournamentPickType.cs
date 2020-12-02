using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.TournamentEndpoint.Enums.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.TournamentEndpoint.Enums
{
    /// <summary>
    ///     Pick type of the game (Tournament API).
    /// </summary>
    [JsonConverter(typeof(TournamentPickTypeConverter))]
    public enum TournamentPickType
    {
        /// <summary>
        /// Blind pick mode.
        /// </summary>
        BlindPick,

        /// <summary>
        /// Draft pick mode.
        /// </summary>
        DraftMode,

        /// <summary>
        /// All random mode.
        /// </summary>
        AllRandom,

        /// <summary>
        /// Tournament draft mode (adds ability to pause).
        /// </summary>
        TournamentDraft
    }
}
