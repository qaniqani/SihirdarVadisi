using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class RssService : IRssService
    {
        public SyndicationFeed GetFeedList(List<RssViewModel> contents, string domain)
        {
            var feed = new SyndicationFeed
            {
                Id = "gamerpeoplecom",
                Title =
                    SyndicationContent.CreatePlaintextContent(
                        "Türkiye’nin Espor Haber Platformu | Gamer People"),
                Description =
                    SyndicationContent.CreatePlaintextContent(
                        "League of Legends Haberleri, CS:GO Haberleri, DOTA2 Haberleri, PubG Haberleri, Overwatch Haberleri, Heartstone Haberleri, Haftalık Turnuvalar, Şampiyon Bilgileri ve çok daha fazlası"),
                LastUpdatedTime = DateTime.Now,
                ImageUrl = new Uri("https://gamerpeople.com/html/assets/img/logo.png"),
                Copyright = SyndicationContent.CreatePlaintextContent($"© {DateTime.Now.Year} GamerPeople.com"),
                Items = contents.Select(a =>
                {
                    var item = new SyndicationItem
                    {
                        Title = new TextSyndicationContent(a.Name, TextSyndicationContentKind.Plaintext),
                        BaseUri = new Uri(domain),
                        Id = a.Id.ToString(),
                        PublishDate = a.CreateDate,
                        LastUpdatedTime = a.ModifiedDate > new DateTime(2017, 1, 1) ? a.ModifiedDate : a.CreateDate,
                        Summary = new TextSyndicationContent(a.Description)
                    };

                    item.Links.Add(new SyndicationLink { BaseUri = new Uri(domain), Uri = new Uri($"{domain}haber/{a.CategoryUrl}/{a.ContentUrl}") });
                    return item;
                })
            };

            return feed;
        }
    }
}