﻿using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.MatchEndpoint.Enums.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.MatchEndpoint.Enums
{
    /// <summary>
    /// Ascended type.
    /// </summary>
    [JsonConverter(typeof(AscendedTypeConverter))]
    public enum AscendedType
    {
        /// <summary>
        /// Champion ascended.
        /// </summary>
        ChampionAscended,

        /// <summary>
        /// Clear ascended.
        /// </summary>
        ClearAscended,

        /// <summary>
        /// Minion ascended.
        /// </summary>
        MinionAscended
    }

    static class AscendedTypeExtension
    {
        public static string ToCustomString(this AscendedType ascendedType)
        {
            switch (ascendedType)
            {
                case AscendedType.ChampionAscended:
                    return "CHAMPION_ASCENDED";
                case AscendedType.ClearAscended:
                    return "CLEAR_ASCENDED";
                case AscendedType.MinionAscended:
                    return "MINION_ASCENDED";
                default:
                    return string.Empty;
            }
        }
    }
}
