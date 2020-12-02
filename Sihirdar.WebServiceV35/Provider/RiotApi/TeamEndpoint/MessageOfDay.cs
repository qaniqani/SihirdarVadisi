﻿using System;
using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc.Converters;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.TeamEndpoint
{
    /// <summary>
    /// Message of the day of the team (Team API).
    /// </summary>
    public class MessageOfDay
    {
        internal MessageOfDay() { }

        /// <summary>
        /// Date of the message creation.
        /// </summary>
        [JsonProperty("createDate")]
        [JsonConverter(typeof(DateTimeConverterFromLong))]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Version of the message.
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }
    }
}
