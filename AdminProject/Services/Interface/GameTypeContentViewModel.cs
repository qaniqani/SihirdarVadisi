using AdminProject.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public class GameTypeContentViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string GameTypeText { get; set; }
        public GameTypes GameType { get; set; }
    }
}