using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Interface;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;
using Sihirdar.WebService.Provider.RiotApi.Model.StatusEndpoint;

namespace Sihirdar.WebService.Provider.RiotApi
{
    public class StatusRiotApi : IStatusRiotApi
    {
        #region Private Fields
        private const string StatusRootUrl = "/shards";

        private const string RegionUrl = "/{0}";

        private const string RootDomain = "status.leagueoflegends.com";

        private readonly Requester _requester;
        #endregion

        private StatusRiotApi()
        {
            Requesters.StatusApiRequester = new Requester();
            _requester = Requesters.StatusApiRequester;
        }
       
        public List<Shard> GetShards()
        {
            var json = _requester.CreateGetRequest(StatusRootUrl, RootDomain, null, false);
            return JsonConvert.DeserializeObject<List<Shard>>(json);
        }
      
        public async Task<List<Shard>> GetShardsAsync()
        {
            var json = await _requester.CreateGetRequestAsync(StatusRootUrl, RootDomain, null, false);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Shard>>(json));
        }
      
        public ShardStatus GetShardStatus(Region region)
        {
            var json = _requester.CreateGetRequest(StatusRootUrl + string.Format(RegionUrl, region.ToString()),
                RootDomain, null, false);
            return JsonConvert.DeserializeObject<ShardStatus>(json);
        }

        public async Task<ShardStatus> GetShardStatusAsync(Region region)
        {
            var json = await _requester.CreateGetRequestAsync(StatusRootUrl + string.Format(RegionUrl, region.ToString()),
                RootDomain, null, false);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ShardStatus>(json));
        }
    }
}
