using System.Collections.Generic;
using System.ServiceModel.Syndication;
using AdminProject.Models;

namespace AdminProject.Services.Interface
{
    public interface IRssService
    {
        SyndicationFeed GetFeedList(List<RssViewModel> contents, string domain);
    }
}