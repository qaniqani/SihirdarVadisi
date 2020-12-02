using System;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint.Enums;

namespace Sihirdar.WebService.Model
{
    public class PlayerStatsSummaryConverter
    {
        public string SeasonName { get; set; }
        public AggregatedStat AggregatedStats { get; set; }

        public int Losses { get; set; }

        public DateTime ModifyDate { get; set; }

        public PlayerStatsSummaryType PlayerStatSummaryType { get; set; }
        public int Wins { get; set; }
    }
}
