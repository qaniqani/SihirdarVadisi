using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.Misc
{
    /// <summary>
    /// Region for the API.
    /// </summary>
    [JsonConverter(typeof(RegionConverter))]
    public enum Region
    {
        /// <summary>
        /// Brasil.
        /// </summary>
        br,

        /// <summary>
        /// North-eastern europe.
        /// </summary>
        eune,

        /// <summary>
        /// Western europe.
        /// </summary>
        euw,

        /// <summary>
        /// North america.
        /// </summary>
        na,

        /// <summary>
        /// South korea.
        /// </summary>
        kr,

        /// <summary>
        /// Latin America North.
        /// </summary>
        lan,

        /// <summary>
        /// Latin America South.
        /// </summary>
        las,

        /// <summary>
        /// Oceania.
        /// </summary>
        oce,

        /// <summary>
        /// Russia.
        /// </summary>
        ru,

        /// <summary>
        /// Turkey.
        /// </summary>
        tr,
        
        /// <summary>
        /// Japan.
        /// </summary>
        jp,

        /// <summary>
        /// Global.
        /// </summary>
        global
    }
}
