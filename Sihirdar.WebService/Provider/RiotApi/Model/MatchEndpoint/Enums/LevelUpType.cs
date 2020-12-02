﻿using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.MatchEndpoint.Enums.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.MatchEndpoint.Enums
{
    /// <summary>
    /// Type of level up (Match API).
    /// </summary>
    [JsonConverter(typeof(LevelUpTypeConverter))]
    public enum LevelUpType
    {
        /// <summary>
        /// When leveling up involves evolving (notably Kha'zix).
        /// </summary>
        Evolve,

        /// <summary>
        /// Normal leveling up.
        /// </summary>
        Normal
    }

    static class LevelUpTypeExtension
    {
        public static string ToCustomString(this LevelUpType levelUpType)
        {
            switch (levelUpType)
            {
                case LevelUpType.Evolve:
                    return "EVOLVE";
                case LevelUpType.Normal:
                    return "NORMAL";
                default:
                    return string.Empty;
            }
        }
    }
}
