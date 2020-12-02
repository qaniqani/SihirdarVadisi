using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.TournamentEndpoint.Enums.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.TournamentEndpoint.Enums
{
    /// <summary>
    ///     Spectator type of the game (Tournament API).
    /// </summary>
    [JsonConverter(typeof(TournamentSpectatorTypeConverter))]
    public enum TournamentSpectatorType
    {
        /// <summary>
        /// No spectators allowed.
        /// </summary>
        None,

        /// <summary>
        /// Spectators only allowed in the lobby.
        /// </summary>
        LobbyOnly,

        /// <summary>
        /// Spectators allowed in the lobby and the game itself.
        /// </summary>
        All
    }
}
