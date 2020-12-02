using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.TeamEndpoint
{
    /// <summary>
    /// Roster of the team (Team API).
    /// </summary>
    public class Roster
    {
        internal Roster() { }

        /// <summary>
        /// List of the team members of the roster.
        /// </summary>
        [JsonProperty("memberList")]
        public List<TeamMemberInfo> MemberList { get; set; }

        /// <summary>
        /// Id of the owner of the team.
        /// </summary>
        [JsonProperty("ownerId")]
        public long OwnerId { get; set; }
    }
}
