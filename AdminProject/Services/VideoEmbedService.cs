using System.Net;
using System.Web;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using HtmlAgilityPack;

namespace AdminProject.Services
{
    public class VideoEmbedService : IVideoEmbedService
    {
        private readonly WebClient _client;

        public VideoEmbedService()
        {
            _client = new WebClient();
            _client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        }

        public VideoEmbedResult GetVideoDetail(string videoUrl, string pictureSaveName)
        {
            if (string.IsNullOrEmpty(videoUrl))
                return null;

            var pageHtml = _client.DownloadString(videoUrl);
            var document = new HtmlDocument();
            document.LoadHtml(pageHtml);

            var collection = document.DocumentNode.SelectNodes("//meta");

            var imageUrl = string.Empty;
            var embedUrl = string.Empty;

            var izleseneCheck = videoUrl.Contains("izlesene.com");
            var vimeoCheck = videoUrl.Contains("vimeo.com");

            foreach (var link in collection)
            {
                if (izleseneCheck)
                {
                    if (link.Attributes["property"] != null && link.Attributes["property"].Value == "og:image")
                        imageUrl = link.Attributes["content"].Value;
                    else if (link.Attributes["name"] != null && link.Attributes["name"].Value == "twitter:player" && string.IsNullOrEmpty(embedUrl))
                        embedUrl = link.Attributes["content"].Value;
                }
                else if (vimeoCheck)
                {
                    if (link.Attributes["name"] != null)
                    {
                        var target = link.Attributes["name"].Value;
                        if (target == "og:video:url" && string.IsNullOrEmpty(embedUrl))
                            embedUrl = link.Attributes["content"].Value;
                        else if (target == "twitter:player" && string.IsNullOrEmpty(embedUrl))
                            embedUrl = link.Attributes["content"].Value;
                    }
                    else if (link.Attributes["property"] != null)
                    {
                        var target = link.Attributes["property"].Value;
                        if (target == "og:image")
                            imageUrl = link.Attributes["content"].Value;
                    }
                }
                else
                {
                    if (link.Attributes["property"] == null)
                        continue;

                    var target = link.Attributes["property"].Value;
                    if (target == "og:image")
                        imageUrl = link.Attributes["content"].Value;
                    else if (target == "og:video:secure_url" && string.IsNullOrEmpty(embedUrl))
                        embedUrl = link.Attributes["content"].Value;
                }
            }

            var savePath = $"~/Content/{pictureSaveName}.jpg";
            _client.DownloadFile(imageUrl, HttpContext.Current.Server.MapPath(savePath));

            if (videoUrl.Contains("izlesene.com"))
                embedUrl = embedUrl.Replace("?playername=izlesene_twitter", "");
            else if(videoUrl.Contains("twitch.tv"))
                embedUrl = embedUrl.Replace("player=facebook&amp;", "");

            var result = new VideoEmbedResult
            {
                SaveName = $"{pictureSaveName}",
                VideoEmbedUrl = embedUrl
            };

            return result;
        }
    }
}