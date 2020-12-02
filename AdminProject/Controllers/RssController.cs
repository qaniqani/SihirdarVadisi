using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using AdminProject.Helpers;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    public class RssController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IRssService _rssService;

        public RssController(IContentService contentService, IRssService rssService)
        {
            _contentService = contentService;
            _rssService = rssService;
        }

        [HttpGet]
        [Route("feed/last-content/atom")]
        [OutputCache(Duration = 3600)]
        public RssResult Atom()
        {
            var helper = new UrlHelper(Request.RequestContext);
            var url = helper.Action("Index", "Default", new { }, Request.IsSecureConnection ? "https" : "http");

            var contents = _contentService.GetRssContent(20);
            var feed = _rssService.GetFeedList(contents, url);

            return new RssResult(feed);
        }

        [HttpGet]
        [Route("feed/last-content/rss")]
        [OutputCache(Duration = 3600)]
        public ContentResult Rss()
        {
            var helper = new UrlHelper(Request.RequestContext);
            var url = helper.Action("Index", "Default", new { }, Request.IsSecureConnection ? "https" : "http");

            var contents = _contentService.GetRssContent(20);
            var rss = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("rss",
                    new XAttribute("version", "2.0"),
                    new XElement("channel",
                        new XElement("title", "Türkiye’nin Espor Haber Platformu | Gamer People"),
                        new XElement("link", url),
                        new XElement("description", "League of Legends Haberleri, CS:GO Haberleri, DOTA2 Haberleri, Overwatch Haberleri, Heartstone Haberleri, Haftalık Turnuvalar, Şampiyon Bilgileri ve çok daha fazlası"),
                        new XElement("copyright", $"© {DateTime.Now.Year} gamerpeople.com"),
                        from item in contents
                        select
                        new XElement("item",
                            new XElement("title", item.Name),
                            new XElement("description", item.Description),
                            new XElement("image",
                                new XElement("link", $"{url}haber/{item.CategoryUrl}/{item.ContentUrl}"),
                                new XElement("title", item.Name),
                                new XElement("height", 315),
                                new XElement("width", 198),
                                new XElement("url", $"{url}Content/" + item.ContentUrl + "_315x198.jpg")
                            ),
                            new XElement("link", $"{url}haber/{item.CategoryUrl}/{item.ContentUrl}"),
                            new XElement("pubDate", item.CreateDate.ToString("R"))
                        )
                    )
                )
            );

            return Content(rss.ToString(), "text/xml");
        }
    }
}