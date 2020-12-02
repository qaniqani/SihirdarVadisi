using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sihirdar.WebServiceV3.Model;
using Sihirdar.WebServiceV3.Provider.RiotApi;
using Sihirdar.WebServiceV3.Provider.RiotApi.Interfaces;
using Sihirdar.WebServiceV3.Provider.RiotApi.MatchEndpoint.Enums;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;
using Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint;
using Tools.Models;
using Tools.Service.Interface;

namespace Tools.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IRiotApi _riotApi;
        private readonly RiotApiConfig _config;

        public PlayerService(IRiotApi riotApi, RiotApiConfig config)
        {
            _riotApi = riotApi;
            _config = config;
        }

        public async Task<PlayerStat> GetHowToPlayedTime(Region region, string username)
        {
            try
            {
                var player = await _riotApi.GetSummonerByNameAsync(region, username).ConfigureAwait(false);
                var playerId = player.Id;



                //var stats = await _riotApi.GetStatsSummariesAsync(region, playerId).ConfigureAwait(false);
                //foreach (var season in Seasons)
                //{
                //    var result = await _riotApi.GetStatsSummariesAsync(region, playerId, season.Key).ConfigureAwait(false);
                //    var converter = result.Select(a => new PlayerStatsSummaryConverter
                //    {
                //        AggregatedStats = a.AggregatedStats,
                //        Losses = a.Losses,
                //        ModifyDate = a.ModifyDate,
                //        PlayerStatSummaryType = a.PlayerStatSummaryType,
                //        SeasonName = Seasons[season.Key],
                //        Wins = a.Wins
                //    });
                //    stats.AddRange(converter);
                //}

                ////var playedTimeList = Common.GetPlayedTimeList;

                //var list = new List<PlayerStatConverter>();
                //stats.ForEach(a =>
                //{
                //    //if (a.Wins > 0)
                //    //{
                //    var item = new PlayerStatConverter
                //    {
                //        Detail = playedTimeList.FirstOrDefault(d => d.PlayerStatsSummaryType == a.PlayerStatSummaryType),
                //        PlayerStatSummaryType = a.PlayerStatSummaryType,
                //        Win = a.Wins
                //    };
                //    list.Add(item);
                //    //}
                //});

                //var sum = 0m;
                //var time = 0m;
                //var resultModel = new PlayerStat();
                //list.ForEach(a =>
                //{
                //    a.Detail = a.Detail ?? new PlayedTimeMultipliers();
                //    a.Detail.Name = a.Detail.Name;// string.IsNullOrEmpty(a.Detail.Name) ? a.PlayerStatSummaryType.ToString() : a.Detail.Name;

                //    sum += a.Win * a.Detail.Odd;
                //    time += a.Detail.Time * a.Win * a.Detail.Odd / 60;

                //    resultModel.PlayerStatDetail.Add(new PlayerStatDetailModel
                //    {
                //        Name = a.Detail.Name,
                //        Time = Convert.ToInt32(a.Detail.Time * a.Win * a.Detail.Odd / 60),
                //        Win = a.Win * a.Detail.Odd,
                //        Average = Convert.ToInt32(a.Detail.Time)
                //    });
                //});
                //resultModel.GameTime = sum;
                //resultModel.PlayedGameTime = Convert.ToInt32(time);
                //resultModel.PlayedGameDay = Convert.ToInt32(time / 24);
                //return resultModel;

                return new PlayerStat();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<ChampionSummaryK>> GetHowMuckYourK(string username, Platform platform = Platform.TR1, Region region = Region.tr)
        {
            //return null;
            var player = await _riotApi.GetSummonerByNameAsync(region, username).ConfigureAwait(false);
            var championSummary = new List<ChampionSummaryK>();

            if (player == null)
                return championSummary;

            var playerId = player.Id;

            var staticApi = StaticRiotApi.GetInstance(_config.ApiKey);
            var champs = await staticApi.GetChampionsAsync(region, ChampionData.image , Language.tr_TR);

            var summary = await _riotApi.GetChampionMasteriesAsync(region, playerId).ConfigureAwait(false);

            summary.ForEach(a =>
            {
                var percentLevel = 100m;
                if (a.ChampionPointsUntilNextLevel > 0)
                {
                    var level = (a.ChampionPointsSinceLastLevel + a.ChampionPointsUntilNextLevel) / 100;
                    percentLevel = Convert.ToDecimal(a.ChampionPointsSinceLastLevel / level);
                }

                var champion = champs.Champions.FirstOrDefault(d => d.Value.Id == a.ChampionId).Value;
                var champName = champion.Name;
                var pictureName = champion.Image.Full;
                var champData = new ChampionSummaryK
                {
                    Picture = pictureName,
                    ChampionName = champName,
                    ChampionId = a.ChampionId,
                    ChampionPoint = a.ChampionPoints,
                    LastPlayTime = a.LastPlayTime,
                    LevelPercent = percentLevel,
                    ChampionLevel = a.ChampionLevel,
                    ChestGranted = a.ChestGranted
                };
                championSummary.Add(champData);
            });

            return championSummary.OrderByDescending(a => a.ChampionLevel).ToList();
        }

        private static readonly Dictionary<Season, string> Seasons = new Dictionary<Season, string>
        {
            {Season.Season2017, "2017. Sezon"},
            {Season.Season2016, "2016. Sezon"},
            {Season.Season2015, "2015. Sezon"},
            {Season.Season2014, "2014. Sezon"},
            {Season.Season3, "2013. Sezon"}
        };
    }
}