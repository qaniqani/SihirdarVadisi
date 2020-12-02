using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Provider.RiotApi.Http;
using Sihirdar.WebServiceV3.Provider.RiotApi.Http.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Sihirdar.WebServiceV3.Provider.RiotApi.TournamentEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.TournamentEndpoint.Enums;

namespace Sihirdar.WebServiceV3.Provider.RiotApi
{
    public class TournamentRiotApi : ITournamentRiotApi
    {
        #region Private Fields

        private const string TournamentStubUrl = "/lol/tournament-stub/v3";
        private const string TournamentUrl = "/lol/tournament/v3";
        private const string CreateCodesUrl = "/codes";
        private const string GetCodesUrl = "/codes/{0}";
        private const string PutCodeUrl = "/codes/{0}";
        private const string LobbyEventUrl = "/lol/tournament/v3/lobby-events/by-code/{0}";
        private const string CreateProviderUrl = "/providers";
        private const string CreateTournamentUrl = "/tournaments";

        private const string MatchRootUrl = "/api/lol/{0}/v2.2/match";
        private const string GetMatchIdUrl = "/by-tournament/{0}/ids";
        private const string GetMatchDetailUrl = "/for-tournament/{0}";

        private readonly IRateLimitedRequester _requester;
        private string _tournamentRootUrl;

        #endregion

        private TournamentRiotApi(RiotApiConfig config)
        {
            var asd = new Dictionary<TimeSpan, int>
            {
                [TimeSpan.FromMinutes(10)] = config.RateLimitPer10M,
                [TimeSpan.FromSeconds(10)] = config.RateLimitPer10S
            };

            Requesters.RiotApiRequester = new RateLimitedRequester(config.ApiKey, asd);
            _requester = Requesters.TournamentApiRequester;
            SetTournamentRootUrl(false);
        }

        #region Public Methods
      
        public int CreateProvider(Region region, string url)
        {
            var body = new Dictionary<string, object>
            {
                { "url", url },
                { "region", region.ToString().ToUpper() }
            };
            var json = _requester.CreatePostRequest(_tournamentRootUrl + CreateProviderUrl, Region.global,
                JsonConvert.SerializeObject(body));

            return int.Parse(json);
        }

        public async Task<int> CreateProviderAsync(Region region, string url)
        {
            var body = new Dictionary<string, object>
            {
                { "url", url },
                { "region", region.ToString().ToUpper() }
            };
            var json =
                await
                    _requester.CreatePostRequestAsync(_tournamentRootUrl + CreateProviderUrl, Region.global,
                        JsonConvert.SerializeObject(body));

            return int.Parse(json);
        }  

        public int CreateTournament(int providerId, string name)
        {
            var body = new Dictionary<string, object>
            {
                { "name", name },
                { "providerId", providerId }
            };
            var json = _requester.CreatePostRequest(_tournamentRootUrl + CreateTournamentUrl, Region.global,
                JsonConvert.SerializeObject(body));

            return int.Parse(json);
        }

        public async Task<int> CreateTournamentAsync(int providerId, string name)
        {
            var body = new Dictionary<string, object> {
                { "name", name },
                { "providerId", providerId }
            };
            var json =
                await
                    _requester.CreatePostRequestAsync(_tournamentRootUrl + CreateTournamentUrl, Region.global,
                        JsonConvert.SerializeObject(body));

            return int.Parse(json);
        }

        public List<string> CreateTournamentCodes(int tournamentId, int count, int teamSize, TournamentSpectatorType spectatorType, 
            TournamentPickType pickType, TournamentMapType mapType, List<long> allowedParticipantIds = null,
            string metadata = "")
        {
            ValidateTeamSize(teamSize);
            ValidateTournamentCodeCount(count);
            var body = new Dictionary<string, object>
            {
                { "teamSize", teamSize },
                { "allowedSummonerIds", allowedParticipantIds },
                { "spectatorType", spectatorType },
                { "pickType", pickType },
                { "mapType", mapType },
                { "metadata", metadata }
            };
            var json = _requester.CreatePostRequest(_tournamentRootUrl + CreateCodesUrl, Region.global,
                JsonConvert.SerializeObject(body, null, 
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
                    new List<string>
                    {
                        string.Format("tournamentId={0}", tournamentId),
                        string.Format("count={0}", count)
                    });

            var tournamentCodes = JsonConvert.DeserializeObject<List<string>>(json);

            return tournamentCodes;
        }
   
        public async Task<List<string>> CreateTournamentCodesAsync(int tournamentId, int count, int teamSize,
            TournamentSpectatorType spectatorType, TournamentPickType pickType, 
            TournamentMapType mapType, List<long> allowedParticipantIds = null,  string metadata = null)
        {
            ValidateTeamSize(teamSize);
            ValidateTournamentCodeCount(count);
            var body = new Dictionary<string, object>
            {
                { "teamSize", teamSize },
                { "allowedSummonerIds", allowedParticipantIds },
                { "spectatorType", spectatorType },
                { "pickType", pickType },
                { "mapType", mapType },
                { "metadata", metadata }
            };
            var json = await _requester.CreatePostRequestAsync(_tournamentRootUrl + CreateCodesUrl, Region.global,
                JsonConvert.SerializeObject(body, null, 
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
                    new List<string>
                    {
                        string.Format("tournamentId={0}", tournamentId),
                        string.Format("count={0}", count)
                    });

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<string>>(json));
        }    

        public TournamentCodeDetail GetTournamentCodeDetails(string tournamentCode)
        {
            var json = _requester.CreateGetRequest(_tournamentRootUrl + string.Format(GetCodesUrl, tournamentCode),
                Region.global);
            var tournamentCodeDetails = JsonConvert.DeserializeObject<TournamentCodeDetail>(json);

            return tournamentCodeDetails;
        }

        public async Task<TournamentCodeDetail> GetTournamentCodeDetailsAsync(string tournamentCode)
        {
            var json =
                await
                    _requester.CreateGetRequestAsync(_tournamentRootUrl + string.Format(GetCodesUrl, tournamentCode),
                        Region.global);

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<TournamentCodeDetail>(json));
        }
        
        public List<TournamentLobbyEvent> GetTournamentLobbyEvents(string tournamentCode)
        {
            var json = _requester.CreateGetRequest(_tournamentRootUrl + string.Format(LobbyEventUrl, tournamentCode),
                Region.global);
            var lobbyEventsDTO = JsonConvert.DeserializeObject<Dictionary<string, List<TournamentLobbyEvent>>>(json);

            return lobbyEventsDTO["eventList"];
        }

        public async Task<List<TournamentLobbyEvent>> GetTournamentLobbyEventsAsync(string tournamentCode)
        {
            var json =
                await
                    _requester.CreateGetRequestAsync(_tournamentRootUrl + string.Format(LobbyEventUrl, tournamentCode),
                        Region.global);

            return
                await
                    Task.Factory.StartNew(() =>
                        JsonConvert.DeserializeObject<Dictionary<string, List<TournamentLobbyEvent>>>(json)["eventList"]
                    );
        }
      
        public bool UpdateTournamentCode(string tournamentCode, List<long> allowedParticipantIds = null,
            TournamentSpectatorType? spectatorType = null, TournamentPickType? pickType = null, TournamentMapType? mapType= null)
        {
            var body = BuildTournamentUpdateBody(allowedParticipantIds, spectatorType, pickType, mapType);

            return _requester.CreatePutRequest(_tournamentRootUrl + string.Format(PutCodeUrl, tournamentCode), Region.global,
                JsonConvert.SerializeObject(body));
        }

        public async Task<bool> UpdateTournamentCodeAsync(string tournamentCode, List<long> allowedParticipantIds = null,
            TournamentSpectatorType? spectatorType = null, TournamentPickType? pickType = null, TournamentMapType? mapType = null)
        {
            var body = BuildTournamentUpdateBody(allowedParticipantIds, spectatorType, pickType, mapType);

            return await _requester.CreatePutRequestAsync(_tournamentRootUrl + string.Format(PutCodeUrl, tournamentCode),
                Region.global, JsonConvert.SerializeObject(body));
        }

        #endregion

        #region Get Tournament Matches (based on Match endpoint)

        public MatchDetail GetTournamentMatch(Region region, long matchId, string tournamentCode, bool includeTimeline)
        {
            var json =
                _requester.CreateGetRequest(
                    string.Format(MatchRootUrl, region) + string.Format(GetMatchDetailUrl, matchId), region,
                    new List<string>
                    {
                        string.Format("tournamentCode={0}", tournamentCode),
                        string.Format("includeTimeline={0}", includeTimeline)
                    });

            var matchDetail = JsonConvert.DeserializeObject<MatchDetail>(json);

            return matchDetail;
        }

        public async Task<MatchDetail> GetTournamentMatchAsync(Region region, long matchId, string tournamentCode,
            bool includeTimeline)
        {
            var json =
                await
                    _requester.CreateGetRequestAsync(
                        string.Format(MatchRootUrl, region) + string.Format(GetMatchDetailUrl, matchId), region,
                        new List<string>
                        {
                            string.Format("tournamentCode={0}", tournamentCode),
                            string.Format("includeTimeline={0}", includeTimeline)
                        });

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<MatchDetail>(json));
        }

        public long GetTournamentMatchId(Region region, string tournamentCode)
        {
            var json =
                _requester.CreateGetRequest(
                    string.Format(MatchRootUrl, region) + string.Format(GetMatchIdUrl, tournamentCode), region);

            var matchIds = JsonConvert.DeserializeObject<List<long>>(json);

            return matchIds.FirstOrDefault();
        }

        public async Task<long> GetTournamentMatchIdAsync(Region region, string tournamentCode)
        {
            var json =
                await
                    _requester.CreateGetRequestAsync(
                        string.Format(MatchRootUrl, region) + string.Format(GetMatchIdUrl, tournamentCode), region);

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<long>>(json).FirstOrDefault());
        }

        #endregion

        #region Private Helpers
        private void SetTournamentRootUrl(bool useStub)
        {
            if (useStub)
                _tournamentRootUrl = TournamentStubUrl;
            else
                _tournamentRootUrl = TournamentUrl;
        }

        private Dictionary<string, object> BuildTournamentUpdateBody(List<long> allowedSummonerIds,
            TournamentSpectatorType? spectatorType, TournamentPickType? pickType, TournamentMapType? mapType)
        {
            var body = new Dictionary<string, object>();
            if (allowedSummonerIds != null)
                body.Add("allowedParticipants", string.Join(",", allowedSummonerIds));
            if (spectatorType != null)
                body.Add("spectatorType", spectatorType);
            if (pickType != null)
                body.Add("pickType", pickType);
            if (mapType != null)
                body.Add("mapType", mapType);

            return body;
        }

        private void ValidateTeamSize(int teamSize)
        {
            if (teamSize < 1 || teamSize > 5)
                throw new ArgumentException("Invalid team size. Valid values are 1-5.", nameof(teamSize));
        }

        private void ValidateTournamentCodeCount(int count)
        {
            if (count > 1000)
                throw new ArgumentException("Invalid count. You cannot create more than 1000 codes at once.", nameof(count));
            if (count < 1)
                throw new ArgumentException("Invalid count. The value of count must be greater than 0.", nameof(count));
        }

        #endregion

    }
}
