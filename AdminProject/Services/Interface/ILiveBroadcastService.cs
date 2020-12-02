using System.Collections.Generic;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface ILiveBroadcastService : IBaseInterface<LiveBroadcast>
    {
        string UrlCheck(string url);
        IList<LiveBroadcast> ActiveList();
        LiveBroadcastListDto GetActiveStream();
        LiveBroadcast GetItem(string url);
    }
}