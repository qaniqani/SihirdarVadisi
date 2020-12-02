using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.Http;
using Sihirdar.WebServiceV3.Provider.RiotApi.Http.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Sihirdar.WebServiceV3.Provider.RiotApi.StatusEndpoint;

namespace Sihirdar.WebServiceV3.Provider.RiotApi
{
    public class StatusRiotApi : IStatusRiotApi
    {
        #region Private Fields
        
        private const string StatusRootUrl = "/lol/status/v3/shard-data";

        private const string PlatformSubdomain = "{0}.";

        private const string RootDomain = "api.riotgames.com";

        private IRequester requester;

        #endregion

        /// <summary>
        /// Get the instance of StatusRiotApi.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>The instance of StatusRiotApi.</returns>
        private StatusRiotApi(RiotApiConfig config)
        {
            Requesters.StaticApiRequester = new Requester(config.ApiKey);
            requester = Requesters.StatusApiRequester;
        }

        #region Public Methods

        public ShardStatus GetShardStatus(Region region)
        {
            var json = requester.CreateGetRequest(StatusRootUrl, region, null, true);

            return JsonConvert.DeserializeObject<ShardStatus>(json);
        }

        public async Task<ShardStatus> GetShardStatusAsync(Region region)
        {
            var json = await requester.CreateGetRequestAsync(StatusRootUrl, region, null, true);

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ShardStatus>(json));
        }

        #endregion
    }
}
