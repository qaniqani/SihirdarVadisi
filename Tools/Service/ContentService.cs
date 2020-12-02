using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;
using Tools.Service.Interface;
using Tools.Service.Model;

namespace Tools.Service
{
    public class ContentService : IContentService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public ContentService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IList<WidgetContentViewModel> GetTopContent()
        {
            var language = "tr";

            var db = _dbFactory();

            var contents = (from content in db.Contents
                            join category in db.Categories
                            on content.CategoryId equals category.Id
                            orderby content.Id descending
                            where content.ContentType == ContentTypes.Content &&
                                  content.FilterType == ContentFilterTypes.TopRated &&
                                  content.Status == StatusTypes.Active &&
                                  category.Status == StatusTypes.Active &&
                                  content.LanguageTag == language &&
                                  category.LanguageTag == language
                            select new
                            {
                                content.Detail,
                                content.GameType,
                                content.Id,
                                content.Name,
                                content.Picture,
                                ContentUrl = content.Url,
                                CategoryUrl = category.Url
                            });

            var result = contents.Take(5).ToList().Select(a =>
            {
                var description = Common.StripHtml(a.Detail);
                description = description.Length > 100 ? description.Substring(0, 100) + "..." : description;
                var item = new WidgetContentViewModel
                {
                    Description = description,
                    GameTypes = a.GameType,
                    GameType = Common.GetGameTypeText[a.GameType],
                    Id = a.Id,
                    Name = a.Name,
                    Picture = a.ContentUrl,
                    CategoryUrl = a.CategoryUrl,
                    ContentUrl = a.ContentUrl
                };

                return item;
            }).ToList();

            return result;
        }

        public IList<WidgetContentViewModel> GetTopContent(string categoryUrl)
        {
            var language = "tr";

            var db = _dbFactory();

            var contents = (from content in db.Contents
                            join category in db.Categories
                            on content.CategoryId equals category.Id
                            orderby content.Id descending
                            where content.ContentType == ContentTypes.Content &&
                                  content.FilterType == ContentFilterTypes.TopRated &&
                                  content.Status == StatusTypes.Active &&
                                  category.Status == StatusTypes.Active &&
                                  content.LanguageTag == language &&
                                  category.LanguageTag == language
                            select new
                            {
                                content.Detail,
                                content.GameType,
                                content.Id,
                                content.Name,
                                content.Picture,
                                ContentUrl = content.Url,
                                CategoryUrl = category.Url
                            });

            if (!string.IsNullOrEmpty(categoryUrl))
                contents = contents.Where(a => a.CategoryUrl == categoryUrl);

            var result = contents.Take(5).ToList().Select(a =>
            {
                var description = Common.StripHtml(a.Detail);
                description = description.Length > 100 ? description.Substring(0, 100) + "..." : description;
                var item = new WidgetContentViewModel
                {
                    Description = description,
                    GameTypes = a.GameType,
                    GameType = Common.GetGameTypeText[a.GameType],
                    Id = a.Id,
                    Name = a.Name,
                    Picture = a.ContentUrl,
                    CategoryUrl = a.CategoryUrl,
                    ContentUrl = a.ContentUrl
                };

                return item;
            }).ToList();

            return result;
        }
    }
}