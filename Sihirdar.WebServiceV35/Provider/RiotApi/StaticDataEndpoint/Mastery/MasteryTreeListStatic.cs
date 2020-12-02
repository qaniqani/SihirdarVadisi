using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Mastery
{
    /// <summary>
    /// Class representing a list of mastery trees (Static API).
    /// </summary>
    public class MasteryTreeListStatic
    {
        internal MasteryTreeListStatic() { }

        /// <summary>
        /// List of mastery tree items.
        /// </summary>
        [JsonProperty("masteryTreeItems")]
        public List<MasteryTreeItemStatic> MasteryTreeItems { get; set; }
    }
}
