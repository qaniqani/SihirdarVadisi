using System.Collections.Generic;
using Sihirdar.WebService.Provider.RiotApi.Model.StatsEndpoint.Enums;

namespace Sihirdar.WebService.Provider.RiotApi.Model
{
    public class PlayedTimeMultipliers
    {
        public PlayerStatsSummaryType PlayerStatsSummaryType { get; set; }
        public decimal Odd { get; set; }
        public int Time { get; set; }
        public string Name { get; set; }
    }
}
