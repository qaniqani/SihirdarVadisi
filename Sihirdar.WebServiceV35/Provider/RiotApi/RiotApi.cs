using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sihirdar.WebServiceV3.Model;
using Sihirdar.WebServiceV3.Provider.RiotApi.ChampionEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.ChampionMasteryEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.CurrentGameEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.FeaturedGamesEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.GameEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.Http;
using Sihirdar.WebServiceV3.Provider.RiotApi.Http.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.LeagueEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint.Enums;
using Sihirdar.WebServiceV3.Provider.RiotApi.MatchListEndPoint;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc.Converters;
using Sihirdar.WebServiceV3.Provider.RiotApi.SummonerEndpoint;

namespace Sihirdar.WebServiceV3.Provider.RiotApi
{
    /// <summary>
    /// Implementation of IRiotApi
    /// </summary>
    public class RiotApi : IRiotApi
    {
        #region Private Fields
        private const string SummonerRootUrl = "/lol/summoner/v3/summoners";
        private const string SummonerByAccountIdUrl = "/by-account/{0}";
        private const string SummonerByNameUrl = "/by-name/{0}";
        private const string SummonerBySummonerIdUrl = "/{0}";

        private const string NamesUrl = "/{0}/name";
        private const string MasteriesUrl = "/lol/platform/v3/masteries/by-summoner/{0}";

        private const string RunesRootUrl = "/api/lol/{0}/v1.4/summoner";
        private const string RunesUrl = "/{0}/runes";

        private const string ChampionsUrl = "/lol/platform/v3/champions";

        private const string GameRootUrl = "/api/lol/{0}/v1.3/game";
        private const string RecentGamesUrl = "/by-summoner/{0}/recent";

        private const string LeagueRootUrl = "/api/lol/{0}/v2.5/league";
        private const string LeagueChallengerUrl = "/challenger";
        private const string LeagueMasterUrl = "/master";

        private const string LeagueBySummonerUrl = "/by-summoner/{0}";
        private const string LeagueEntryUrl = "/entry";

        private const string MatchRootUrl = "/api/lol/{0}/v2.2/match";
        private const string MatchListRootUrl = "/api/lol/{0}/v2.2/matchlist/by-summoner";

        private const string CurrentGameRootUrl = "/observer-mode/rest/consumer/getSpectatorGameInfo/{0}";

        private const string FeaturedGamesRootUrl = "/observer-mode/rest/featured";

        private const string IdUrl = "/{0}";

        private const string ChampionMasteryRootUrl = "/lol/champion-mastery/v3";
        private const string ChampionMasteriesBySummonerUrl = "/champion-masteries/by-summoner/{0}";
        private const string ChampionMasteryBySummonerUrl = "/champion-masteries/by-summoner/{0}/by-champion/{1}";
        private const string ChampionMasteryTotalScoreBySummonerUrl = "/scores/by-summoner/{0}";

        // Used in call which have a maximum number of items you can retrieve in a single call
        private const int MaxNrSummoners = 40;
        private const int MaxNrMasteryPages = 40;
        private const int MaxNrRunePages = 40;
        private const int MaxNrLeagues = 10;
        private const int MaxNrEntireLeagues = 10;

        private readonly IRateLimitedRequester _requester;

        public List<PlayedTimeMultipliers> GetPlayedTimeList;
        #endregion

        public RiotApi(RiotApiConfig config)
        {
            var asd = new Dictionary<TimeSpan, int>
            {
                [TimeSpan.FromMinutes(10)] = config.RateLimitPer10M,
                [TimeSpan.FromSeconds(10)] = config.RateLimitPer10S
            };

            Requesters.RiotApiRequester = new RateLimitedRequester(config.ApiKey, asd);
            _requester = Requesters.RiotApiRequester;

            //GetPlayedTimeList = new List<PlayedTimeMultipliers>
            //{
            //    new PlayedTimeMultipliers { Name = "Suikast", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.Assassinate, Time = 0 },
            //    new PlayedTimeMultipliers { Name = "Eşli Dereceli 3v3", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedPremade3x3, Time = 0 },
            //    new PlayedTimeMultipliers { Name = "Eşli Dereceli 5v5", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedPremade5x5, Time = 0 },
            //    new PlayedTimeMultipliers { Name = "ARAM", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.AramUnranked5x5, Time = 25 },
            //    new PlayedTimeMultipliers { Name = "Yükseliş Modu", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Ascension, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Black Market Brawlers games", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Bilgewater, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Takım Kurucu", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.CAP5x5, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Sihirdar Vadisi Bot", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.CoopVsAI, Time = 25 },
            //    new PlayedTimeMultipliers { Name = "Uğursuz Koruluk Bot", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.CoopVsAI3x3, Time = 25 },
            //    new PlayedTimeMultipliers { Name = "Dost Kazığı", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.CounterPick, Time = 30 },
            //    new PlayedTimeMultipliers { Name = "Kartopu Savaşı 1x1", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.FirstBlood1x1, Time = 25 },
            //    new PlayedTimeMultipliers { Name = "Kartopu Savaşı 2x2", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.FirstBlood2x2, Time = 25 },
            //    new PlayedTimeMultipliers { Name = "Altıda Altı Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Hexakill, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Poro Kralı", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.KingPoro, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Kıyamet Botları", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.NightmareBot, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Kristal Kayalık", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.OdinUnranked, Time = 20 },
            //    new PlayedTimeMultipliers { Name = "Birimiz Hepimiz İçin", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.OneForAll5x5, Time = 25 },
            //    new PlayedTimeMultipliers { Name = "Dereceli Sihirdar Vadisi Solo", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedSolo5x5, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Dereceli Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedTeam3x3, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Dereceli Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedTeam5x5, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Esnek Dereceli SR", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedFlexSR, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Esnek Dereceli TT", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedFlexTT, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Merkez Kuşatması", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Siege, Time = 20 },
            //    new PlayedTimeMultipliers { Name = "Altıda Altı Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.SummonersRift6x6, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Unranked, Time = 35 },
            //    new PlayedTimeMultipliers { Name = "Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Unranked3x3, Time = 30 },
            //    new PlayedTimeMultipliers { Name = "URF", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.URF, Time = 30 },
            //    new PlayedTimeMultipliers { Name = "Botlara Karşı URF", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.URFBots, Time = 30 },
            //    new PlayedTimeMultipliers { Name = "Tüm Rastgele Haberci Savaşı", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.Arsr, Time = 30 }
            //};
        }

#pragma warning disable CS1591
        #region Summoner
        public Summoner GetSummonerByAccountId(Region region, long accountId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(SummonerRootUrl + SummonerByAccountIdUrl, accountId), region);
            var obj = JsonConvert.DeserializeObject<Summoner>(json);
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }

        public async Task<Summoner> GetSummonerByAccountIdAsync(Region region, long accountId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(SummonerRootUrl + SummonerByAccountIdUrl, accountId), region);
            var obj = (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Summoner>(json)));
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }

        public Summoner GetSummonerBySummonerId(Region region, long summonerId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(SummonerRootUrl + SummonerBySummonerIdUrl, summonerId), region);
            var obj = JsonConvert.DeserializeObject<Summoner>(json);
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }

        public async Task<Summoner> GetSummonerBySummonerIdAsync(Region region, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(SummonerRootUrl + SummonerBySummonerIdUrl, summonerId), region);
            var obj = (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Summoner>(json)));
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }
       
        public Summoner GetSummonerByName(Region region, string summonerName)
        {
            var json = _requester.CreateGetRequest(
                string.Format(SummonerRootUrl + SummonerByNameUrl, summonerName), region);
            var obj = JsonConvert.DeserializeObject<Summoner>(json);
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }
      
        public async Task<Summoner> GetSummonerByNameAsync(Region region, string summonerName)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(SummonerRootUrl + SummonerByNameUrl, summonerName), region);
            var obj = (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Summoner>(json)));
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }
        #endregion

        #region Champion
        public List<Champion> GetChampions(Region region, bool freeToPlay = false)
        {
            var json = _requester.CreateGetRequest(ChampionsUrl, region,
                new List<string> { string.Format("freeToPlay={0}", freeToPlay ? "true" : "false") });
            return JsonConvert.DeserializeObject<ChampionList>(json).Champions;
        }
       
        public async Task<List<Champion>> GetChampionsAsync(Region region, bool freeToPlay = false)
        {
            var json = await _requester.CreateGetRequestAsync(ChampionsUrl, region,
                new List<string> { string.Format("freeToPlay={0}", freeToPlay ? "true" : "false") });
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<ChampionList>(json))).Champions;
        }
       
        public Champion GetChampion(Region region, int championId)
        {
            var json = _requester.CreateGetRequest(
                ChampionsUrl + string.Format(IdUrl, championId), region);
            return JsonConvert.DeserializeObject<Champion>(json);
        }
 
        public async Task<Champion> GetChampionAsync(Region region, int championId)
        {
            var json = await _requester.CreateGetRequestAsync(ChampionsUrl + string.Format(IdUrl, championId), region);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Champion>(json));
        }
        #endregion

        #region Masteries
        public List<MasteryPage> GetMasteryPages(Region region, long summonerId)
        {
            var json = _requester.CreateGetRequest(string.Format(MasteriesUrl, summonerId), region);

            var masteries = JsonConvert.DeserializeObject<MasteryPages>(json);
            return masteries.Pages;
        }
 
        public async Task<List<MasteryPage>> GetMasteryPagesAsync(Region region, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(string.Format(MasteriesUrl, summonerId), region);

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<MasteryPages>(json).Pages);
        }
        #endregion

        #region Runes
        public Dictionary<long, List<RunePage>> GetRunePages(Region region, List<long> summonerIds)
        {
            var dict = new Dictionary<long, List<RunePage>>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrRunePages))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(RunesRootUrl,
                        region.ToString()) + string.Format(RunesUrl, Util.BuildIdsString(grp)),
                    region);
                var subDict = ConstructRuneDict(JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json));
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
   
        public async Task<Dictionary<long, List<RunePage>>> GetRunePagesAsync(Region region, List<long> summonerIds)
        {
            var tasks = MakeGroups(summonerIds, MaxNrRunePages).Select(
                grp => _requester.CreateGetRequestAsync(
                    string.Format(RunesRootUrl, region.ToString()) +
                    string.Format(RunesUrl, Util.BuildIdsString(grp)), region
                    ).ContinueWith(
                        json => ConstructRuneDict(
                            JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json.Result))
                    )
                ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        #endregion

        #region League
        public Dictionary<long, List<League>> GetLeagues(Region region, List<long> summonerIds)
        {
            var dict = new Dictionary<long, List<League>>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrLeagues))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(LeagueRootUrl, region.ToString()) +
                        string.Format(LeagueBySummonerUrl, Util.BuildIdsString(grp)) + LeagueEntryUrl,
                    region);
                var subDict = JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json);
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
 
        public async Task<Dictionary<long, List<League>>> GetLeaguesAsync(Region region, List<long> summonerIds)
        {
            var tasks = MakeGroups(summonerIds, MaxNrLeagues).Select(
                grp => _requester.CreateGetRequestAsync(
                    string.Format(LeagueRootUrl, region.ToString()) +
                    string.Format(LeagueBySummonerUrl, Util.BuildIdsString(grp)) + LeagueEntryUrl, region
                    ).ContinueWith(
                        json => JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json.Result)
                    )
                ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    
        public Dictionary<long, List<League>> GetEntireLeagues(Region region, List<long> summonerIds)
        {
            var dict = new Dictionary<long, List<League>>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrEntireLeagues))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(LeagueRootUrl,
                        region.ToString()) + string.Format(LeagueBySummonerUrl, Util.BuildIdsString(grp)),
                    region);
                var subDict = JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json);
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
    
        public async Task<Dictionary<long, List<League>>> GetEntireLeaguesAsync(Region region,
            List<long> summonerIds)
        {
            var tasks = MakeGroups(summonerIds, MaxNrEntireLeagues).Select(
                   grp => _requester.CreateGetRequestAsync(
                       string.Format(LeagueRootUrl, region.ToString()) +
                       string.Format(LeagueBySummonerUrl, Util.BuildIdsString(grp)), region
                       ).ContinueWith(
                           json => JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json.Result)
                       )
                   ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        
        public League GetChallengerLeague(Region region, string queue)
        {
            var json = _requester.CreateGetRequest(
                string.Format(LeagueRootUrl, region.ToString()) + LeagueChallengerUrl,
                region,
                new List<string> { string.Format("type={0}", queue) });
            return JsonConvert.DeserializeObject<League>(json);
        }
      
        public async Task<League> GetChallengerLeagueAsync(Region region, string queue)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(LeagueRootUrl, region.ToString()) + LeagueChallengerUrl,
                region,
                new List<string> { string.Format("type={0}", queue) });
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<League>(json));
        }
  
        public League GetMasterLeague(Region region, string queue)
        {
            var json = _requester.CreateGetRequest(
                string.Format(LeagueRootUrl, region.ToString()) + LeagueMasterUrl,
                region,
                new List<string> { string.Format("type={0}", queue) });
            return JsonConvert.DeserializeObject<League>(json);
        }
    
        public async Task<League> GetMasterLeagueAsync(Region region, string queue)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(LeagueRootUrl, region.ToString()) + LeagueMasterUrl,
                region,
                new List<string> { string.Format("type={0}", queue) });
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<League>(json));
        }
        
        public MatchDetail GetMatch(Region region, long matchId, bool includeTimeline = false)
        {
            var json = _requester.CreateGetRequest(
                string.Format(MatchRootUrl, region.ToString()) + string.Format(IdUrl, matchId),
                region,
                includeTimeline
                    ? new List<string> { string.Format("includeTimeline={0}", includeTimeline.ToString().ToLower() ) }
                    : null);
            return JsonConvert.DeserializeObject<MatchDetail>(json);
        }
        #endregion

        #region Match
        public async Task<MatchDetail> GetMatchAsync(Region region, long matchId, bool includeTimeline = false)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(MatchRootUrl, region.ToString()) + string.Format(IdUrl, matchId),
                region,
                includeTimeline
                    ? new List<string> { string.Format("includeTimeline={0}", includeTimeline) }
                    : null);
            return await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<MatchDetail>(json));
        }
    
        public MatchList GetMatchList(Region region, long summonerId,
            List<long> championIds = null, List<string> rankedQueues = null,
            List<Season> seasons = null, DateTime? beginTime = null, DateTime? endTime = null,
            int? beginIndex = null, int? endIndex = null)
        {
            var addedArguments = new List<string> {
                    string.Format("beginIndex={0}", beginIndex),
                    string.Format("endIndex={0}", endIndex),
            };
            if (beginTime != null)
            {
                addedArguments.Add(string.Format("beginTime={0}", beginTime.Value.ToLong()));
            }
            if (endTime != null)
            {
                addedArguments.Add(string.Format("endTime={0}", endTime.Value.ToLong()));
            }
            if (championIds != null)
            {
                addedArguments.Add(string.Format("championIds={0}", Util.BuildIdsString(championIds)));
            }
            if (rankedQueues != null)
            {
                addedArguments.Add(string.Format("rankedQueues={0}", Util.BuildQueuesString(rankedQueues)));
            }
            if (seasons != null)
            {
                addedArguments.Add(string.Format("seasons={0}", Util.BuildSeasonString(seasons)));
            }

            var json = _requester.CreateGetRequest(
                string.Format(MatchListRootUrl, region.ToString()) + string.Format(IdUrl, summonerId),
                region,
                addedArguments);
            return JsonConvert.DeserializeObject<MatchList>(json);
        }
      
        public async Task<MatchList> GetMatchListAsync(Region region, long summonerId,
            List<long> championIds = null, List<string> rankedQueues = null,
            List<Season> seasons = null, DateTime? beginTime = null,
            DateTime? endTime = null, int? beginIndex = null, int? endIndex = null)
        {
            var addedArguments = new List<string> {
                    string.Format("beginIndex={0}", beginIndex),
                    string.Format("endIndex={0}", endIndex),
            };
            if (beginTime != null)
            {
                addedArguments.Add(string.Format("beginTime={0}", beginTime.Value.ToLong()));
            }
            if (endTime != null)
            {
                addedArguments.Add(string.Format("endTime={0}", endTime.Value.ToLong()));
            }
            if (championIds != null)
            {
                addedArguments.Add(string.Format("championIds={0}", Util.BuildIdsString(championIds)));
            }
            if (rankedQueues != null)
            {
                addedArguments.Add(string.Format("rankedQueues={0}", Util.BuildQueuesString(rankedQueues)));
            }
            if (seasons != null)
            {
                addedArguments.Add(string.Format("seasons={0}", Util.BuildSeasonString(seasons)));
            }

            var json = await _requester.CreateGetRequestAsync(
                string.Format(MatchListRootUrl, region.ToString()) + string.Format(IdUrl, summonerId),
                region,
                addedArguments);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<MatchList>(json));
        }

        public List<Game> GetRecentGames(Region region, long summonerId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(GameRootUrl, region) + string.Format(RecentGamesUrl, summonerId),
                region);
            return JsonConvert.DeserializeObject<RecentGames>(json).Games;
        }
     
        public async Task<List<Game>> GetRecentGamesAsync(Region region, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(GameRootUrl, region) + string.Format(RecentGamesUrl, summonerId),
                region);
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RecentGames>(json))).Games;
        }
        #endregion    

        #region Spectator
        public CurrentGame GetCurrentGame(Platform platform, long summonerId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(CurrentGameRootUrl, platform.ToString()) + string.Format(IdUrl, summonerId),
                platform.ConvertToRegion());
            return JsonConvert.DeserializeObject<CurrentGame>(json);
        }
     
        public async Task<CurrentGame> GetCurrentGameAsync(Platform platform, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(CurrentGameRootUrl, platform.ToString()) + string.Format(IdUrl, summonerId),
                platform.ConvertToRegion());
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CurrentGame>(json)));
        }
      
        public FeaturedGames GetFeaturedGames(Region region)
        {
            var json = _requester.CreateGetRequest(
                FeaturedGamesRootUrl,
                region);
            return JsonConvert.DeserializeObject<FeaturedGames>(json);
        }
      
        public async Task<FeaturedGames> GetFeaturedGamesAsync(Region region)
        {
            var json = await _requester.CreateGetRequestAsync(
                FeaturedGamesRootUrl,
                region);
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<FeaturedGames>(json)));
        }
        #endregion

        #region Champion Mastery
        public ChampionMastery GetChampionMastery(Region region, long summonerId, long championId)
        {
            var requestUrl = string.Format(ChampionMasteryBySummonerUrl, summonerId, championId);

            var json = _requester.CreateGetRequest(ChampionMasteryRootUrl + requestUrl, region);
            return JsonConvert.DeserializeObject<ChampionMastery>(json);
        }

        public async Task<ChampionMastery> GetChampionMasteryAsync(Region region, long summonerId, long championId)
        {
            var requestUrl = string.Format(ChampionMasteryBySummonerUrl, summonerId, championId);

            var json = await _requester.CreateGetRequestAsync(ChampionMasteryRootUrl + requestUrl, region);
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ChampionMastery>(json)));
        }

        public List<ChampionMastery> GetChampionMasteries(Region region, long summonerId)
        {
            var requestUrl = string.Format(ChampionMasteriesBySummonerUrl, summonerId);

            var json = _requester.CreateGetRequest(ChampionMasteryRootUrl + requestUrl, region);
            return JsonConvert.DeserializeObject<List<ChampionMastery>>(json);
        }
       
        public async Task<List<ChampionMastery>> GetChampionMasteriesAsync(Region region, long summonerId)
        {
            var requestUrl = string.Format(ChampionMasteriesBySummonerUrl, summonerId);

            var json = await _requester.CreateGetRequestAsync(ChampionMasteryRootUrl + requestUrl, region);
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<ChampionMastery>>(json)));
        }

        public int GetTotalChampionMasteryScore(Region region, long summonerId)
        {
            var requestUrl = string.Format(ChampionMasteryTotalScoreBySummonerUrl, summonerId);

            var json = _requester.CreateGetRequest(ChampionMasteryRootUrl + requestUrl, region);
            return JsonConvert.DeserializeObject<int>(json);
        }

        public async Task<int> GetTotalChampionMasteryScoreAsync(Region region, long summonerId)
        {
            var requestUrl = string.Format(ChampionMasteryTotalScoreBySummonerUrl, summonerId);

            var json = _requester.CreateGetRequest(ChampionMasteryRootUrl + requestUrl, region);
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<int>(json)));
        }
        #endregion

        #region Helpers
        private Dictionary<long, List<MasteryPage>> ConstructMasteryDict(Dictionary<string, MasteryPages> dict)
        {
            var returnDict = new Dictionary<long, List<MasteryPage>>();
            foreach (var masteryPage in dict.Values)
            {
                returnDict.Add(masteryPage.SummonerId, masteryPage.Pages);
            }
            return returnDict;
        }

        private Dictionary<long, List<RunePage>> ConstructRuneDict(Dictionary<string, RunePages> dict)
        {
            var returnDict = new Dictionary<long, List<RunePage>>();
            foreach (var runePage in dict.Values)
            {
                returnDict.Add(runePage.SummonerId, runePage.Pages);
            }
            return returnDict;
        }

        private List<List<T>> MakeGroups<T>(List<T> toSplit, int chunkSize)
        {
            return toSplit
                .Distinct()
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
        #endregion
#pragma warning restore
    }
}
