using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sihirdar.WebService.Provider.RiotApi.Interface;
using Sihirdar.WebService.Provider.RiotApi.Model;
using Sihirdar.WebService.Provider.RiotApi.Model.ChampionEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.ChampionMasteryEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.CurrentGameEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.FeaturedGamesEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.GameEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.LeagueEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.MatchEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.MatchListEndPoint;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc.Converters;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint.Enums;
using Sihirdar.WebService.Provider.RiotApi.Model.SummonerEndpoint;

namespace Sihirdar.WebService.Provider.RiotApi
{
    public class RiotApi : IRiotApi
    {
        #region Private Fields
     
        private const string SummonerRootUrl = "/api/lol/{0}/v1.4/summoner";
        private const string ByNameUrl = "/by-name/{0}";
        private const string NamesUrl = "/{0}/name";
        private const string MasteriesUrl = "/{0}/masteries";
        private const string RunesUrl = "/{0}/runes";

        private const string ChampionRootUrl = "/api/lol/{0}/v1.2/champion";

        private const string GameRootUrl = "/api/lol/{0}/v1.3/game";
        private const string RecentGamesUrl = "/by-summoner/{0}/recent";

        private const string LeagueRootUrl = "/api/lol/{0}/v2.5/league";
        private const string LeagueChallengerUrl = "/challenger";
        private const string LeagueMasterUrl = "/master";

        private const string LeagueByTeamUrl = "/by-team/{0}";
        private const string LeagueBySummonerUrl = "/by-summoner/{0}";
        private const string LeagueEntryUrl = "/entry";

        private const string TeamRootUrl = "/api/lol/{0}/v2.4/team";
        private const string TeamBySummonerUrl = "/by-summoner/{0}";

        private const string StatsRootUrl = "/api/lol/{0}/v1.3/stats";
        private const string StatsSummaryUrl = "/by-summoner/{0}/summary";
        private const string StatsRankedUrl = "/by-summoner/{0}/ranked";

        private const string MatchRootUrl = "/api/lol/{0}/v2.2/match";
        private const string MatchListRootUrl = "/api/lol/{0}/v2.2/matchlist/by-summoner";

        private const string CurrentGameRootUrl = "/observer-mode/rest/consumer/getSpectatorGameInfo/{0}";

        private const string FeaturedGamesRootUrl = "/observer-mode/rest/featured";

        private const string IdUrl = "/{0}";

        private const string ChampionMasteryRootUrl = "/championmastery/location/{0}/player/{1}";
        private const string ChampionMasteryByChampionId = "/champion/{0}";
        private const string ChampionMasteryAllChampions = "/champions";
        private const string ChampionMasteryTotalScore = "/score";
        private const string ChampionMasteryTopChampions = "/topchampions";

        // Used in call which have a maximum number of items you can retrieve in a single call
        private const int MaxNrSummoners = 40;
        private const int MaxNrMasteryPages = 40;
        private const int MaxNrRunePages = 40;
        private const int MaxNrLeagues = 10;
        private const int MaxNrEntireLeagues = 10;
        private const int MaxNrTeams = 10;

        private readonly RateLimitedRequester _requester;

        public List<PlayedTimeMultipliers> GetPlayedTimeList;

        #endregion

        /// <summary>
        /// Get the instance of RiotApi.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>The instance of RiotApi.</returns>
        public RiotApi(RiotApiConfig config)
        {
            Requesters.RiotApiRequester = new RateLimitedRequester(config.ApiKey, config.RateLimitPer10S, config.RateLimitPer10M);
            _requester = Requesters.RiotApiRequester;

            GetPlayedTimeList = new List<PlayedTimeMultipliers>
            {
                new PlayedTimeMultipliers { Name = "Suikast", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.Assassinate, Time = 0 },
                new PlayedTimeMultipliers { Name = "Eşli Dereceli 3v3", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedPremade3x3, Time = 0 },
                new PlayedTimeMultipliers { Name = "Eşli Dereceli 5v5", Odd = 0, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedPremade5x5, Time = 0 },
                new PlayedTimeMultipliers { Name = "ARAM", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.AramUnranked5x5, Time = 25 },
                new PlayedTimeMultipliers { Name = "Yükseliş Modu", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Ascension, Time = 35 },
                new PlayedTimeMultipliers { Name = "Black Market Brawlers games", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Bilgewater, Time = 35 },
                new PlayedTimeMultipliers { Name = "Takım Kurucu", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.CAP5x5, Time = 35 },
                new PlayedTimeMultipliers { Name = "Sihirdar Vadisi Bot", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.CoopVsAI, Time = 25 },
                new PlayedTimeMultipliers { Name = "Uğursuz Koruluk Bot", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.CoopVsAI3x3, Time = 25 },
                new PlayedTimeMultipliers { Name = "Dost Kazığı", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.CounterPick, Time = 30 },
                new PlayedTimeMultipliers { Name = "Kartopu Savaşı 1x1", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.FirstBlood1x1, Time = 25 },
                new PlayedTimeMultipliers { Name = "Kartopu Savaşı 2x2", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.FirstBlood2x2, Time = 25 },
                new PlayedTimeMultipliers { Name = "Altıda Altı Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Hexakill, Time = 35 },
                new PlayedTimeMultipliers { Name = "Poro Kralı", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.KingPoro, Time = 35 },
                new PlayedTimeMultipliers { Name = "Kıyamet Botları", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.NightmareBot, Time = 35 },
                new PlayedTimeMultipliers { Name = "Kristal Kayalık", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.OdinUnranked, Time = 20 },
                new PlayedTimeMultipliers { Name = "Birimiz Hepimiz İçin", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.OneForAll5x5, Time = 25 },
                new PlayedTimeMultipliers { Name = "Dereceli Sihirdar Vadisi Solo", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedSolo5x5, Time = 35 },
                new PlayedTimeMultipliers { Name = "Dereceli Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedTeam3x3, Time = 35 },
                new PlayedTimeMultipliers { Name = "Dereceli Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedTeam5x5, Time = 35 },
                new PlayedTimeMultipliers { Name = "Esnek Dereceli SR", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedFlexSR, Time = 35 },
                new PlayedTimeMultipliers { Name = "Esnek Dereceli TT", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.RankedFlexTT, Time = 35 },
                new PlayedTimeMultipliers { Name = "Merkez Kuşatması", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Siege, Time = 20 },
                new PlayedTimeMultipliers { Name = "Altıda Altı Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.SummonersRift6x6, Time = 35 },
                new PlayedTimeMultipliers { Name = "Sihirdar Vadisi", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Unranked, Time = 35 },
                new PlayedTimeMultipliers { Name = "Uğursuz Koruluk", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.Unranked3x3, Time = 30 },
                new PlayedTimeMultipliers { Name = "URF", Odd = 2, PlayerStatsSummaryType = PlayerStatsSummaryType.URF, Time = 30 },
                new PlayedTimeMultipliers { Name = "Botlara Karşı URF", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.URFBots, Time = 30 },
                new PlayedTimeMultipliers { Name = "Tüm Rastgele Haberci Savaşı", Odd = 1.2m, PlayerStatsSummaryType = PlayerStatsSummaryType.Arsr, Time = 30 }
            };
        }

        public List<PlayedTimeMultipliers> GetPlayedList()
        {
            return GetPlayedTimeList;
        }

        public Summoner GetSummoner(Region region, long summonerId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(SummonerRootUrl, region) + string.Format(IdUrl, summonerId), region);
            var obj = JsonConvert.DeserializeObject<Dictionary<long, Summoner>>(json).Values.FirstOrDefault();
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }

        public async Task<Summoner> GetSummonerAsync(Region region, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(SummonerRootUrl, region.ToString()) + string.Format(IdUrl, summonerId), region);
            var obj = (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<Dictionary<long, Summoner>>(json))).Values.FirstOrDefault();
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }
       
        public List<Summoner> GetSummoners(Region region, List<long> summonerIds)
        {
            var list = new List<Summoner>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrSummoners))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(SummonerRootUrl,
                        region.ToString()) + string.Format(IdUrl, Util.BuildIdsString(grp)),
                    region);
                var subList = JsonConvert.DeserializeObject<Dictionary<long, Summoner>>(json).Values.ToList();
                list.AddRange(subList);
            }
            foreach (var summ in list)
            {
                summ.Region = region;
            }
            return list;
        }
      
        public async Task<List<Summoner>> GetSummonersAsync(Region region, List<long> summonerIds)
        {
            var tasks = MakeGroups(summonerIds, MaxNrSummoners).Select(
                grp => _requester.CreateGetRequestAsync(
                    string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(IdUrl, Util.BuildIdsString(grp)), region
                    ).ContinueWith(
                        json => JsonConvert.DeserializeObject<Dictionary<long, Summoner>>(json.Result)
                    )
                ).ToList();

            await Task.WhenAll(tasks);
            var list = tasks.SelectMany(task => task.Result.Values).ToList();

            foreach (var summ in list)
            {
                summ.Region = region;
            }
            return list;
        }
       
        public Summoner GetSummoner(Region region, string summonerName)
        {
            var json = _requester.CreateGetRequest(
                string.Format(SummonerRootUrl, region) +
                    string.Format(ByNameUrl, Uri.EscapeDataString(summonerName)),
                region);
            var obj = JsonConvert.DeserializeObject<Dictionary<string, Summoner>>(json).Values.FirstOrDefault();
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }
      
        public async Task<Summoner> GetSummonerAsync(Region region, string summonerName)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(ByNameUrl, Uri.EscapeDataString(summonerName)),
                region);
            var obj = (await Task.Factory.StartNew(() =>
                    JsonConvert.DeserializeObject<Dictionary<string, Summoner>>(json))).Values.FirstOrDefault();
            if (obj != null)
            {
                obj.Region = region;
            }
            return obj;
        }
       
        public List<Summoner> GetSummoners(Region region, List<string> summonerNames)
        {
            var list = new List<Summoner>();
            foreach (var grp in MakeGroups(summonerNames, MaxNrSummoners))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(SummonerRootUrl,
                        region.ToString()) + string.Format(ByNameUrl, Util.BuildNamesString(grp)),
                    region);
                var subList = JsonConvert.DeserializeObject<Dictionary<string, Summoner>>(json).Values.ToList();
                list.AddRange(subList);
            }
            foreach (var summ in list)
            {
                summ.Region = region;
            }
            return list;
        }
       
        public async Task<List<Summoner>> GetSummonersAsync(Region region, List<string> summonerNames)
        {
            var tasks = MakeGroups(summonerNames, MaxNrSummoners).Select(
                grp => _requester.CreateGetRequestAsync(
                    string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(ByNameUrl, Util.BuildNamesString(grp)), region
                    ).ContinueWith(
                        json => JsonConvert.DeserializeObject<Dictionary<string, Summoner>>(json.Result)
                    )
                ).ToList();

            await Task.WhenAll(tasks);
            var list = tasks.SelectMany(task => task.Result.Values).ToList();

            foreach (var summ in list)
            {
                summ.Region = region;
            }
            return list;
        }
       
        public SummonerBase GetSummonerName(Region region, long summonerId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(SummonerRootUrl, region.ToString()) + string.Format(NamesUrl, summonerId), region);
            var child = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return new SummonerBase(child.Keys.FirstOrDefault(), child.Values.FirstOrDefault(), _requester, region);
        }
       
        public async Task<SummonerBase> GetSummonerNameAsync(Region region, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(SummonerRootUrl, region.ToString()) + string.Format(NamesUrl, summonerId), region);
            var child = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return new SummonerBase(child.Keys.FirstOrDefault(), child.Values.FirstOrDefault(), _requester, region);
        }
       
        public List<SummonerBase> GetSummonerNames(Region region, List<long> summonerIds)
        {
            var list = new List<SummonerBase>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrSummoners))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(SummonerRootUrl,
                        region.ToString()) + string.Format(NamesUrl, Util.BuildIdsString(grp)),
                    region);
                var children = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                foreach (var child in children)
                {
                    list.Add(new SummonerBase(child.Key, child.Value, _requester, region));
                }

            }
            return list;
        }
      
        public async Task<List<SummonerBase>> GetSummonerNamesAsync(Region region, List<long> summonerIds)
        {
            var tasks = MakeGroups(summonerIds, MaxNrSummoners).Select(
                grp => _requester.CreateGetRequestAsync(
                    string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(NamesUrl, Util.BuildIdsString(grp)), region
                    ).ContinueWith(
                        json => JsonConvert.DeserializeObject<Dictionary<string, string>>(json.Result)
                    )
                ).ToList();

            await Task.WhenAll(tasks);

            var list = new List<SummonerBase>();
            foreach (var child in tasks.SelectMany(task => task.Result))
            {
                list.Add(new SummonerBase(child.Key, child.Value, _requester, region));
            }
            return list;
        }
     
        public List<Champion> GetChampions(Region region, bool freeToPlay = false)
        {
            var json = _requester.CreateGetRequest(string.Format(ChampionRootUrl, region), region,
                new List<string> {$"freeToPlay={(freeToPlay ? "true" : "false")}"});
            return JsonConvert.DeserializeObject<ChampionList>(json).Champions;
        }
       
        public async Task<List<Champion>> GetChampionsAsync(Region region, bool freeToPlay = false)
        {
            var json = await _requester.CreateGetRequestAsync(string.Format(ChampionRootUrl, region.ToString()), region,
                new List<string> { string.Format("freeToPlay={0}", freeToPlay ? "true" : "false") });
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<ChampionList>(json))).Champions;
        }
       
        public Champion GetChampion(Region region, int championId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(ChampionRootUrl, region) + string.Format(IdUrl, championId), region);
            return JsonConvert.DeserializeObject<Champion>(json);
        }
 
        public async Task<Champion> GetChampionAsync(Region region, int championId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(ChampionRootUrl, region.ToString()) + string.Format(IdUrl, championId),
                region);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Champion>(json));
        }
 
        public Dictionary<long, List<MasteryPage>> GetMasteryPages(Region region, List<long> summonerIds)
        {
            var dict = new Dictionary<long, List<MasteryPage>>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrMasteryPages))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(SummonerRootUrl,
                        region.ToString()) + string.Format(MasteriesUrl, Util.BuildIdsString(grp)),
                    region);
                var subDict =
                    ConstructMasteryDict(JsonConvert.DeserializeObject<Dictionary<string, MasteryPages>>(json));
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
 
        public async Task<Dictionary<long, List<MasteryPage>>> GetMasteryPagesAsync(Region region,
            List<long> summonerIds)
        {
            var tasks = MakeGroups(summonerIds, MaxNrMasteryPages).Select(
                grp => _requester.CreateGetRequestAsync(
                    string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(MasteriesUrl, Util.BuildIdsString(grp)), region
                    ).ContinueWith(
                        json => ConstructMasteryDict(
                            JsonConvert.DeserializeObject<Dictionary<string, MasteryPages>>(json.Result))
                    )
                ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
  
        public Dictionary<long, List<RunePage>> GetRunePages(Region region, List<long> summonerIds)
        {
            var dict = new Dictionary<long, List<RunePage>>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrRunePages))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(SummonerRootUrl,
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
                    string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(RunesUrl, Util.BuildIdsString(grp)), region
                    ).ContinueWith(
                        json => ConstructRuneDict(
                            JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json.Result))
                    )
                ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
 
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
    
        public Dictionary<string, List<League>> GetLeagues(Region region, List<string> teamIds)
        {
            var dict = new Dictionary<string, List<League>>();
            foreach (var grp in MakeGroups(teamIds, MaxNrLeagues))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(LeagueRootUrl, region.ToString()) +
                        string.Format(LeagueByTeamUrl, Util.BuildNamesString(grp)) + LeagueEntryUrl,
                    region);
                var subDict = JsonConvert.DeserializeObject<Dictionary<string, List<League>>>(json);
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
    
        public async Task<Dictionary<string, List<League>>> GetLeaguesAsync(Region region, List<string> teamIds)
        {
            var tasks = MakeGroups(teamIds, MaxNrLeagues).Select(
                   grp => _requester.CreateGetRequestAsync(
                       string.Format(LeagueRootUrl, region.ToString()) +
                       string.Format(LeagueByTeamUrl, Util.BuildNamesString(grp)) + LeagueEntryUrl, region
                       ).ContinueWith(
                           json => JsonConvert.DeserializeObject<Dictionary<string, List<League>>>(json.Result)
                       )
                   ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
     
        public Dictionary<string, List<League>> GetEntireLeagues(Region region, List<string> teamIds)
        {
            var dict = new Dictionary<string, List<League>>();
            foreach (var grp in MakeGroups(teamIds, MaxNrEntireLeagues))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(LeagueRootUrl,
                        region.ToString()) + string.Format(LeagueByTeamUrl, Util.BuildNamesString(grp)),
                    region);
                var subDict = JsonConvert.DeserializeObject<Dictionary<string, List<League>>>(json);
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
  
        public async Task<Dictionary<string, List<League>>> GetEntireLeaguesAsync(Region region,
            List<string> teamIds)
        {
            var tasks = MakeGroups(teamIds, MaxNrEntireLeagues).Select(
                   grp => _requester.CreateGetRequestAsync(
                       string.Format(LeagueRootUrl, region.ToString()) +
                       string.Format(LeagueByTeamUrl, Util.BuildNamesString(grp)), region
                       ).ContinueWith(
                           json => JsonConvert.DeserializeObject<Dictionary<string, List<League>>>(json.Result)
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
 
        public Dictionary<long, List<Team>> GetTeams(Region region, List<long> summonerIds)
        {
            var dict = new Dictionary<long, List<Team>>();
            foreach (var grp in MakeGroups(summonerIds, MaxNrTeams))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(TeamRootUrl,
                        region.ToString()) + string.Format(TeamBySummonerUrl, Util.BuildIdsString(grp)),
                    region);
                var subDict = JsonConvert.DeserializeObject<Dictionary<long, List<Team>>>(json);
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
  
        public async Task<Dictionary<long, List<Team>>> GetTeamsAsync(Region region,
            List<long> summonerIds)
        {
            var tasks = MakeGroups(summonerIds, MaxNrTeams).Select(
                   grp => _requester.CreateGetRequestAsync(
                       string.Format(TeamRootUrl, region.ToString()) +
                       string.Format(TeamBySummonerUrl, Util.BuildIdsString(grp)), region
                       ).ContinueWith(
                           json => JsonConvert.DeserializeObject<Dictionary<long, List<Team>>>(json.Result)
                       )
                   ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
      
        public Dictionary<string, Team> GetTeams(Region region, List<string> teamIds)
        {
            var dict = new Dictionary<string, Team>();
            foreach (var grp in MakeGroups(teamIds, MaxNrTeams))
            {
                var json = _requester.CreateGetRequest(
                    string.Format(TeamRootUrl, region.ToString()) + string.Format(IdUrl, Util.BuildNamesString(grp)),
                    region);
                var subDict = JsonConvert.DeserializeObject<Dictionary<string, Team>>(json);
                foreach (var child in subDict)
                {
                    dict.Add(child.Key, child.Value);
                }
            }
            return dict;
        }
     
        public async Task<Dictionary<string, Team>> GetTeamsAsync(Region region, List<string> teamIds)
        {
            var tasks = MakeGroups(teamIds, MaxNrTeams).Select(
                   grp => _requester.CreateGetRequestAsync(
                       string.Format(TeamRootUrl, region.ToString()) +
                       string.Format(IdUrl, Util.BuildNamesString(grp)), region
                       ).ContinueWith(
                           json => JsonConvert.DeserializeObject<Dictionary<string, Team>>(json.Result)
                       )
                   ).ToList();

            await Task.WhenAll(tasks);
            return tasks.SelectMany(task => task.Result).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
  
        public MatchDetail GetMatch(Region region, long matchId, bool includeTimeline = false)
        {
            var json = _requester.CreateGetRequest(
                string.Format(MatchRootUrl, region) + string.Format(IdUrl, matchId),
                region,
                new List<string> {$"includeTimeline={includeTimeline}"});
            return JsonConvert.DeserializeObject<MatchDetail>(json);
        }
     
        public async Task<MatchDetail> GetMatchAsync(Region region, long matchId, bool includeTimeline = false)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(MatchRootUrl, region.ToString()) + string.Format(IdUrl, matchId),
                region,
                new List<string> {$"includeTimeline={includeTimeline}"});
            return await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<MatchDetail>(json));
        }
    
        public MatchList GetMatchList(Region region, long summonerId,
            List<long> championIds = null, List<string> rankedQueues = null,
            List<Model.MatchEndpoint.Enums.Season> seasons = null, DateTime? beginTime = null, DateTime? endTime = null,
            int? beginIndex = null, int? endIndex = null)
        {
            var addedArguments = new List<string> {
                $"beginIndex={beginIndex}",
                $"endIndex={endIndex}",
            };
            if (beginTime != null)
            {
                addedArguments.Add($"beginTime={beginTime.Value.ToLong()}");
            }
            if (endTime != null)
            {
                addedArguments.Add($"endTime={endTime.Value.ToLong()}");
            }
            if (championIds != null)
            {
                addedArguments.Add($"championIds={Util.BuildIdsString(championIds)}");
            }
            if (rankedQueues != null)
            {
                addedArguments.Add($"rankedQueues={Util.BuildQueuesString(rankedQueues)}");
            }
            if (seasons != null)
            {
                addedArguments.Add($"seasons={Util.BuildSeasonString(seasons)}");
            }

            var json = _requester.CreateGetRequest(
                string.Format(MatchListRootUrl, region.ToString()) + string.Format(IdUrl, summonerId),
                region,
                addedArguments);
            return JsonConvert.DeserializeObject<MatchList>(json);
        }
      
        public async Task<MatchList> GetMatchListAsync(Region region, long summonerId,
            List<long> championIds = null, List<string> rankedQueues = null,
            List<Model.MatchEndpoint.Enums.Season> seasons = null, DateTime? beginTime = null,
            DateTime? endTime = null, int? beginIndex = null, int? endIndex = null)
        {
            var addedArguments = new List<string> {
                $"beginIndex={beginIndex}",
                $"endIndex={endIndex}",
            };
            if (beginTime != null)
            {
                addedArguments.Add($"beginTime={beginTime.Value.ToLong()}");
            }
            if (endTime != null)
            {
                addedArguments.Add($"endTime={endTime.Value.ToLong()}");
            }
            if (championIds != null)
            {
                addedArguments.Add($"championIds={Util.BuildIdsString(championIds)}");
            }
            if (rankedQueues != null)
            {
                addedArguments.Add($"rankedQueues={Util.BuildQueuesString(rankedQueues)}");
            }
            if (seasons != null)
            {
                addedArguments.Add($"seasons={Util.BuildSeasonString(seasons)}");
            }

            var json = await _requester.CreateGetRequestAsync(
                string.Format(MatchListRootUrl, region) + string.Format(IdUrl, summonerId),
                region,
                addedArguments);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<MatchList>(json));
        }
    
        public List<PlayerStatsSummary> GetStatsSummaries(Region region, long summonerId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(StatsRootUrl, region) + string.Format(StatsSummaryUrl, summonerId),
                region);
            return JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json).PlayerStatSummaries;
        }
     
        public async Task<List<PlayerStatsSummary>> GetStatsSummariesAsync(Region region, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(StatsRootUrl, region) + string.Format(StatsSummaryUrl, summonerId),
                region);
            var result = (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json))).PlayerStatSummaries;
            return result;
        }
     
        public List<PlayerStatsSummary> GetStatsSummaries(Region region, long summonerId, Season season)
        {
            var json = _requester.CreateGetRequest(
                string.Format(StatsRootUrl, region) + string.Format(StatsSummaryUrl, summonerId),
                region,
                new List<string> { string.Format("season={0}", season.ToString().ToUpper()) });
            return JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json).PlayerStatSummaries;
        }
     
        public async Task<List<PlayerStatsSummary>> GetStatsSummariesAsync(Region region, long summonerId,
            Season season)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(StatsRootUrl, region) + string.Format(StatsSummaryUrl, summonerId),
                region,
                new List<string> { string.Format("season={0}", season.ToString().ToUpper()) });
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json))).PlayerStatSummaries;
        }
    
        public List<ChampionStats> GetStatsRanked(Region region, long summonerId)
        {
            var json = _requester.CreateGetRequest(
                string.Format(StatsRootUrl, region) + string.Format(StatsRankedUrl, summonerId),
                region);
            return JsonConvert.DeserializeObject<RankedStats>(json).ChampionStats;
        }
      
        public async Task<List<ChampionStats>> GetStatsRankedAsync(Region region, long summonerId)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(StatsRootUrl, region) + string.Format(StatsRankedUrl, summonerId),
                region);
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RankedStats>(json))).ChampionStats;
        }
       
        public List<ChampionStats> GetStatsRanked(Region region, long summonerId, Season season)
        {
            var json = _requester.CreateGetRequest(
                string.Format(StatsRootUrl, region) + string.Format(StatsRankedUrl, summonerId),
                region,
                new List<string> { string.Format("season={0}", season.ToString().ToUpper()) });
            return JsonConvert.DeserializeObject<RankedStats>(json).ChampionStats;
        }
      
        public async Task<List<ChampionStats>> GetStatsRankedAsync(Region region, long summonerId,
            Season season)
        {
            var json = await _requester.CreateGetRequestAsync(
                string.Format(StatsRootUrl, region) + string.Format(StatsRankedUrl, summonerId),
                region,
                new List<string> { string.Format("season={0}", season.ToString().ToUpper()) });
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RankedStats>(json))).ChampionStats;
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
        
        public ChampionMastery GetChampionMastery(Platform platform, long summonerId, int championId)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);
            var additionalUrl = string.Format(ChampionMasteryByChampionId, championId);

            var json = _requester.CreateGetRequest(rootUrl + additionalUrl, platform.ConvertToRegion());
            return JsonConvert.DeserializeObject<ChampionMastery>(json);
        }
       
        public async Task<ChampionMastery> GetChampionMasteryAsync(Platform platform,
            long summonerId, int championId)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);
            var additionalUrl = string.Format(ChampionMasteryByChampionId, championId);

            var json = await _requester.CreateGetRequestAsync(rootUrl + additionalUrl, platform.ConvertToRegion());
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ChampionMastery>(json)));
        }
       
        public List<ChampionMastery> GetChampionMasteries(Platform platform, long summonerId)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);

            var json = _requester.CreateGetRequest(rootUrl + ChampionMasteryAllChampions,
                platform.ConvertToRegion());
            return JsonConvert.DeserializeObject<List<ChampionMastery>>(json);
        }
      
        public async Task<List<ChampionMastery>> GetChampionMasteriesAsync(Platform platform, long summonerId)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);

            var json = await _requester.CreateGetRequestAsync(rootUrl + ChampionMasteryAllChampions,
                platform.ConvertToRegion());
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<ChampionMastery>>(json)));
        }
    
        public int GetTotalChampionMasteryScore(Platform platform, long summonerId)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);

            var json = _requester.CreateGetRequest(rootUrl + ChampionMasteryTotalScore,
                platform.ConvertToRegion());
            return JsonConvert.DeserializeObject<int>(json);
        }
      
        public async Task<int> GetTotalChampionMasteryScoreAsync(Platform platform, long summonerId)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);

            var json = await _requester.CreateGetRequestAsync(rootUrl + ChampionMasteryTotalScore,
                platform.ConvertToRegion());
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<int>(json)));
        }
   
        public List<ChampionMastery> GetTopChampionsMasteries(Platform platform, long summonerId,
            int count = 3)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);

            var json = _requester.CreateGetRequest(rootUrl + ChampionMasteryTopChampions,
                platform.ConvertToRegion(), new List<string> {$"count={count}"});
            return JsonConvert.DeserializeObject<List<ChampionMastery>>(json);
        }
     
        public async Task<List<ChampionMastery>> GetTopChampionsMasteriesAsync(Platform platform,
            long summonerId, int count = 3)
        {
            var rootUrl = string.Format(ChampionMasteryRootUrl, platform, summonerId);

            var json = await _requester.CreateGetRequestAsync(rootUrl + ChampionMasteryTopChampions,
                platform.ConvertToRegion(), new List<string> { string.Format("count={0}", count) });
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<ChampionMastery>>(json)));
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
    }
}
