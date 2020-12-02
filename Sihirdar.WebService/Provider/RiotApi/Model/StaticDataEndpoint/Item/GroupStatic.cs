using Newtonsoft.Json;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Item
{
    /// <summary>
    /// Class representing an item's group (Static API).
    /// </summary>
    public class GroupStatic
    {
        internal GroupStatic() { }

        /// <summary>
        /// Max group ownable.
        /// </summary>
        [JsonProperty("MaxGroupOwnable")]
        public string MaxGroupOwnable { get; set; }

        /// <summary>
        /// Key of the group.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
