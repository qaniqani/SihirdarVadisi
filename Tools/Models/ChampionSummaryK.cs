using System;

namespace Tools.Models
{
    public class ChampionSummaryK
    {
        public string Picture { get; set; }
        public string ChampionName { get; set; }
        public long ChampionId { get; set; }
        public DateTime LastPlayTime { get; set; }
        public decimal LevelPercent { get; set; }
        public int ChampionPoint { get; set; }
        public int ChampionLevel { get; set; }
        public bool ChestGranted { get; set; }
    }
}