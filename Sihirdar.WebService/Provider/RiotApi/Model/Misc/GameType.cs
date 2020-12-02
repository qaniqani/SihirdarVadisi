using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.Misc
{
    /// <summary>
    /// Type of the game.
    /// </summary>
    [JsonConverter(typeof(GameTypeConverter))]
    public enum GameType
    {
        /// <summary>
        /// Custom games.
        /// </summary>
        CustomGame,

        /// <summary>
        /// Neither tutorial nor custom games
        /// </summary>
        MatchedGame,

        /// <summary>
        /// Tutorial games.
        /// </summary>
        TutorialGame
    }
}
