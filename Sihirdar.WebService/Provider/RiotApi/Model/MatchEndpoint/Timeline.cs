using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc.Converters;

namespace Sihirdar.WebService.Provider.RiotApi.Model.MatchEndpoint
{
    /// <summary>
    /// Class representing a match's timeline (Match API).
    /// </summary>
    public class Timeline
    {
        internal Timeline() { }

        /// <summary>
        /// Time between each returned frame.
        /// </summary>
        [JsonProperty("frameInterval")]
        [JsonConverter(typeof(TimeSpanConverterFromMilliseconds))]
        public TimeSpan FrameInterval { get; set; }

        /// <summary>
        /// List of timeline frames for the game.
        /// </summary>
        [JsonProperty("frames")]
        public List<Frame> Frames { get; set; }
    }
}
