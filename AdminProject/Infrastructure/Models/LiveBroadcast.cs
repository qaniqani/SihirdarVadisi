using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class LiveBroadcast
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public GameTypes GameType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PublishAddress { get; set; }
        public string ChatAddress { get; set; }
        public string Url { get; set; }
        public int SequenceNumber { get; set; }
        public bool Live { get; set; }
        public StatusTypes Status { get; set; }
    }
}