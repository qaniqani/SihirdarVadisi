using Sihirdar.DataAccessLayer;

namespace AdminProject.Models
{
    public class WidgetContentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public GameTypes GameTypes { get; set; }
        public string GameType { get; set; }
        public string CategoryUrl { get; set; }
        public string ContentUrl { get; set; }
        public string Picture { get; set; }
    }
}