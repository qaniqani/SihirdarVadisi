using System.Collections.Generic;

namespace AdminProject.Services.Models
{
    public class LiveBroadcastListDto
    {
        public bool Live { get; set; }
        public List<LiveBroadcastItemDto> Items { get; set; }
    }
}