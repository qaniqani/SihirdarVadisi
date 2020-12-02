using System.Collections.Generic;

namespace Tools.Models
{
    public class PlayerStat
    {
        public List<PlayerStatDetailModel> PlayerStatDetail { get; set; } = new List<PlayerStatDetailModel>();
        public decimal GameTime { get; set; }
        public int PlayedGameTime { get; set; }
        public int PlayedGameDay { get; set; }
    }

    public class PlayerStatDetailModel
    {
        public string Name { get; set; }
        public int Time { get; set; }
        public decimal Win { get; set; }
        public int Average { get; set; }
    }
}