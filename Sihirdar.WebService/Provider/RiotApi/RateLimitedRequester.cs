using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace Sihirdar.WebService.Provider.RiotApi
{
    internal class RateLimitedRequester : Requester
    {
        public int RateLimitPer10S { get; set; }
        public int RateLimitPer10M { get; set; }

        internal RateLimitedRequester(string apiKey, int rateLimitPer10S, int rateLimitPer10M)
        {
            ApiKey = apiKey;
            RateLimitPer10S = rateLimitPer10S;
            RateLimitPer10M = rateLimitPer10M;
        }

        private readonly Dictionary<Region, RateLimiter> _rateLimiters = new Dictionary<Region, RateLimiter>();

        public string CreateGetRequest(string relativeUrl, Region region, List<string> addedArguments = null,
            bool useHttps = true)
        {
            RootDomain = region + ".api.pvp.net";
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Get);
            
            //GetRateLimiter(region).HandleRateLimit();

            return GetResult(request);
        }


        public async Task<string> CreateGetRequestAsync(string relativeUrl, Region region,
            List<string> addedArguments = null, bool useHttps = true)
        {
            RootDomain = region + ".api.pvp.net";
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Get);
            
            //await GetRateLimiter(region).HandleRateLimitAsync().ConfigureAwait(false);

            return await GetResultAsync(request).ConfigureAwait(false);
        }

        public string CreatePostRequest(string relativeUrl, Region region, string body,
            List<string> addedArguments = null, bool useHttps = true)
        {
            RootDomain = region + ".api.pvp.net";
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Post);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            //GetRateLimiter(region).HandleRateLimit();

            return Post(request);
        }

        public async Task<string> CreatePostRequestAsync(string relativeUrl, Region region, string body,
            List<string> addedArguments = null, bool useHttps = true)
        {
            RootDomain = region + ".api.pvp.net";
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Post);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            //await GetRateLimiter(region).HandleRateLimitAsync().ConfigureAwait(false);

            return await PostAsync(request).ConfigureAwait(false);
        }

        public bool CreatePutRequest(string relativeUrl, Region region, string body, List<string> addedArguments = null,
            bool useHttps = true)
        {
            RootDomain = region + ".api.pvp.net";
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Put);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            //GetRateLimiter(region).HandleRateLimit();

            var response = Put(request);
            return (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;
        }

        public async Task<bool> CreatePutRequestAsync(string relativeUrl, Region region, string body,
            List<string> addedArguments = null, bool useHttps = true)
        {
            RootDomain = region + ".api.pvp.net";
            var request = PrepareRequest(relativeUrl, addedArguments, useHttps, HttpMethod.Put);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            //await GetRateLimiter(region).HandleRateLimitAsync().ConfigureAwait(false);

            var response = await PutAsync(request).ConfigureAwait(false);
            return (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;
        }

        /// <summary>
        /// Returns the respective region's RateLimiter, creating it if needed.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private RateLimiter GetRateLimiter(Region region)
        {
            if (!_rateLimiters.ContainsKey(region))
                _rateLimiters[region] = new RateLimiter(RateLimitPer10S, RateLimitPer10M);
            return _rateLimiters[region]; 
        }
    }
}
