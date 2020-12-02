using System;
//using RiotSharp.StatsEndpoint;
//using RiotSharp.StatsEndpoint.Enums;

namespace Sihirdar.WebServiceV3.Model
{
    public class PlayerStatsSummaryConverter
    {
        public string SeasonName { get; set; }
        //public AggregatedStat AggregatedStats { get; set; }

        public int Losses { get; set; }

        public DateTime ModifyDate { get; set; }

        //public PlayerStatsSummaryType PlayerStatSummaryType { get; set; }
        public int Wins { get; set; }
    }
}
