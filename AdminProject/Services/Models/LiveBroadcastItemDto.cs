using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Models
{
    public class LiveBroadcastItemDto
    {
        public GameTypes GameType { get; set; }
        public string GameTypeText { get; set; }
        public string Name { get; set; }
        public bool Live { get; set; }
        public string Url { get; set; }
    }
}