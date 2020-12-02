﻿using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint.Enums.Converters;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint.Enums
{
    /// <summary>
    /// Building type (Match API).
    /// </summary>
    [JsonConverter(typeof(BuildingTypeConverter))]
    public enum BuildingType
    {
        /// <summary>
        /// Inhibitors.
        /// </summary>
        InhibitorBuilding,

        /// <summary>
        /// Towers.
        /// </summary>
        TowerBuilding
    }

    static class BuildingTypeExtension
    {
        public static string ToCustomString(this BuildingType buildingType)
        {
            switch (buildingType)
            {
                case BuildingType.InhibitorBuilding:
                    return "INHIBITOR_BUILDING";
                case BuildingType.TowerBuilding:
                    return "TOWER_BUILDING";
                default:
                    return string.Empty;
            }
        }
    }
}
