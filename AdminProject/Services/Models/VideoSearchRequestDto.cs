using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Models
{
    public class VideoSearchRequestDto
    {
        public string Subject { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public StatusTypes Status { get; set; } = StatusTypes.Active;
        public int Skip { get; set; } = 1;
        public int Take { get; set; } = 20;
    }
}