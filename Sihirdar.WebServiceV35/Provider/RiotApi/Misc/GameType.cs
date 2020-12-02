using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc.Converters;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.Misc
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
