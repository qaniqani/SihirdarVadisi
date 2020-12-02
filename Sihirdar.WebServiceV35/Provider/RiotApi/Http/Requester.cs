using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sihirdar.WebServiceV3.Provider.RiotApi.Http.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.Http
{
    /// <summary>
    /// A requester without a rate limiter.
    /// </summary>
    public class Requester : RequesterBase, IRequester
    {
        public Requester(string apiKey) : base(apiKey)
        {
        }

        #region Public Methods        
        public string CreateGetRequest(string relativeUrl, Region region, List<string> addedArguments = null,
            bool useHttps = true)
        {
            rootDomain = GetPlatformDomain(region);
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Get);
            var response = Get(request);
            return GetResponseContent(response);
        }

        public async Task<string> CreateGetRequestAsync(string relativeUrl, Region region,
            List<string> addedArguments = null, bool useHttps = true)
        {
            rootDomain = GetPlatformDomain(region);
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Get);
            var response = await GetAsync(request);
            return await GetResponseContentAsync(response);
        }
        #endregion
    }
}
