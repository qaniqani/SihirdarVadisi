using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Areas.Admin.Models;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Resources;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using PagedList;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Services
{
    public class ContentService : IContentService
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly IPictureService _pictureService;
        private readonly ICategoryService _categoryService;
        private readonly IGalleryService _galleryService;
        private readonly IAdvertService _advertService;

        public ContentService(RuntimeSettings setting, Func<AdminDbContext> dbFactory, IPictureService pictureService, ICategoryService categoryService, IGalleryService galleryService, IAdvertService advertService)
        {
            _setting = setting;
            _dbFactory = dbFactory;
            _pictureService = pictureService;
            _categoryService = categoryService;
            _galleryService = galleryService;
            _advertService = advertService;
        }

        public void Add(Content instance)
        {
            var db = _dbFactory();
            db.Contents.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Content newInstance)
        {
            var db = _dbFactory();
            var content = GetItem(id);
            content.GameType = newInstance.GameType;
            content.VideoUrl = newInstance.VideoUrl;
            content.CategoryId = newInstance.CategoryId;
            content.FilterType = newInstance.FilterType;
            content.ContentType = newInstance.ContentType;
            content.Description = newInstance.Description;
            content.Detail = newInstance.Detail;
            content.IsShowcase = newInstance.IsShowcase;
            content.Keyword = newInstance.Keyword;
            content.LanguageId = newInstance.LanguageId;
            content.LanguageTag = newInstance.LanguageTag;
            content.ModifiedDate = newInstance.ModifiedDate;
            content.ModifiedUser = newInstance.ModifiedUser;
            content.Name = newInstance.Name;
            content.Picture = newInstance.Picture;
            content.SequenceNumber = newInstance.SequenceNumber;
            content.Status = newInstance.Status;
            content.Tags = newInstance.Tags;
            content.Title = newInstance.Title;
            content.Url = newInstance.Url;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();

            var content = db.Contents.FirstOrDefault(a => a.Id == id);
            if (content == null)
                return;

            var pictures = _pictureService.List(content.Id).Select(a => a.Id).ToArray();
            _pictureService.Delete(pictures);

            db.Contents.Remove(content);
            db.SaveChanges();
        }

        public string UrlCheck(string url)
        {
            var db = _dbFactory();

            var count = db.Contents.Count(a => a.Url == url);
            if (count > 0)
            {
                url = $"{url}-{count}";
                UrlCheck(url);
            }

            return url;
        }

        public IList<Content> List()
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var contents = db.Contents.Where(a => a.LanguageId == languageId).ToList().Select(a =>
            {
                a.Detail = Utility.StripHtml(a.Detail);
                return a;
            }).ToList();

            return contents;
        }

        public IList<ContentListItemDto> ListCategories()
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var query = from content in db.Contents
                        join editor in db.Admins
                        on content.CreateUser equals editor.Id
                        join updateEditor in db.Admins
                        on content.ModifiedUser equals updateEditor.Id into ps
                        from edit in ps.DefaultIfEmpty()
                        where content.LanguageId == languageId
                        select new
                        {
                            content.CategoryId,
                            content.CreateDate,
                            content.PublishDate,
                            content.ModifiedDate,
                            content.Name,
                            content.Id,
                            content.Status,
                            content.Url,
                            ModifiedUserId = content.ModifiedUser,
                            CreateUser = editor.Name,
                            CreateUsername = editor.Username,
                            ModifiedUser = edit.Name,
                            ModifiedUsername = edit.Username
                        };

            var contents = query.ToList().Select(a =>
            {
                var categoryId = a.CategoryId;
                var content = new ContentListItemDto
                {
                    CreateDate = a.CreateDate,
                    PublishDate = a.PublishDate,
                    Categories = _categoryService.GetChainCategoryLink(categoryId, " > ").Name,
                    Name = a.Name,
                    Id = a.Id,
                    Status = a.Status,
                    Url = a.Url,
                    ModifiedDate = a.ModifiedDate,
                    CreateEditor = $"{a.CreateUser}/ {a.CreateUsername}",
                    UpdateEditor = $"{a.ModifiedUser}/ {a.ModifiedUsername}",
                    UpdateEditorId = a.ModifiedUserId
                };
                return content;
            }).ToList();

            return contents;
        }

        public PagerList<ContentListItemDto> ListCategories(ContentSearchRequestDto request)
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var query = from content in db.Contents
                        join editor in db.Admins
                        on content.CreateUser equals editor.Id
                        join updateEditor in db.Admins
                        on content.ModifiedUser equals updateEditor.Id into ps
                        from edit in ps.DefaultIfEmpty()
                        orderby content.Id descending
                        where content.LanguageId == languageId
                        && content.ContentType == request.ContentType
                        && content.Status == request.Status
                        select new
                        {
                            content.IsShowcase,
                            content.GameType,
                            content.CategoryId,
                            content.CreateDate,
                            content.ModifiedDate,
                            content.Name,
                            content.Id,
                            content.Status,
                            content.Url,
                            content.PublishDate,
                            ModifiedUserId = content.ModifiedUser,
                            CreateUser = editor.Name,
                            CreateUsername = editor.Username,
                            ModifiedUser = edit.Name,
                            ModifiedUsername = edit.Username
                        };

            if (request.CreateDate != null)
            {
                var date = (DateTime)request.CreateDate;
                query = query.Where(a => a.CreateDate >= date);
            }

            if (request.GameType != GameTypes.All)
                query = query.Where(a => a.GameType == request.GameType);

            if (request.IsShowcase)
                query = query.Where(a => a.IsShowcase);

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(a => a.Name.Contains(request.Name));

            if (!string.IsNullOrEmpty(request.Url))
                query = query.Where(a => a.Url.Contains(request.Url));

            var contentCount = query.Count();
            var contentResult = query.Skip(request.Skip * request.Take).Take(request.Take).ToList();

            var contents = contentResult.ToList().Select(a =>
            {
                var categoryId = a.CategoryId;
                var content = new ContentListItemDto
                {
                    CreateDate = a.CreateDate,
                    Categories = _categoryService.GetChainCategoryLink(categoryId, " > ").Name,
                    Name = a.Name,
                    Id = a.Id,
                    Status = a.Status,
                    Url = a.Url,
                    PublishDate = a.PublishDate,
                    ModifiedDate = a.ModifiedDate,
                    CreateEditor = a.CreateUsername,
                    UpdateEditor = a.ModifiedUsername,
                    UpdateEditorId = a.ModifiedUserId
                };
                return content;
            }).ToList();

            var result = new PagerList<ContentListItemDto>
            {
                TotalCount = contentCount,
                List = contents
            };

            return result;
        }

        public IList<Content> OrderList()
        {
            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var contents = db.Contents.Where(a => a.LanguageId == languageId).OrderBy(a => a.SequenceNumber).ToList();

            return contents;
        }

        public IList<Content> SetOrderList(string[] order)
        {
            var db = _dbFactory();

            var list = order.Select(a =>
            {
                var id = Convert.ToInt32(a.Split('|')[0]);
                var sequenceNumber = Convert.ToInt32(a.Split('|')[1]);

                return new { Id = id, Number = sequenceNumber };
            }).ToList();

            var languageId = _setting.LanguageId;
            var contents = db.Contents.Where(a => a.LanguageId == languageId).OrderByDescending(a => a.Id).ToList();

            contents.ForEach(g =>
            {
                var item = list.FirstOrDefault(a => a.Id == g.Id);
                if (item != null)
                    g.SequenceNumber = item.Number;
            });
            db.SaveChanges();

            var newList = db.Contents.Where(a => a.LanguageId == languageId).OrderBy(a => a.SequenceNumber).ToList();

            return newList;
        }

        public PagerList<Content> AllContentList(int skip, int take)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var contents = db.Contents.OrderByDescending(a => a.Id).Skip(skip).Take(take).ToList();
            var contentCount = db.Contents.Count();

            var result = new PagerList<Content>
            {
                TotalCount = contentCount,
                List = contents
            };

            return result;
        }

        public PagerList<Content> AllContentList(int skip, int take, StatusTypes status)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var contents = db.Contents.Where(a => a.Status == status).OrderByDescending(a => a.Id).Skip(skip).Take(take).ToList();
            var contentCount = db.Contents.Count(a => a.Status == status);

            var result = new PagerList<Content>
            {
                TotalCount = contentCount,
                List = contents
            };

            return result;
        }

        public Content GetItem(int instanceId)
        {
            var db = _dbFactory();
            var content = db.Contents.FirstOrDefault(a => a.Id == instanceId);
            return content;
        }

        //Site
        public StaticPagedList<CategoryContentItemViewModel> GetCategoryContent(string categoryUrl, CategoryTypes categoryType, int skip, int take)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Url == categoryUrl && a.Status == StatusTypes.Active);
            if (category == null)
                throw new CustomException(Lang.categoryNotFound);

            var dateNow = DateTime.Now;

            var categoryId = category.Id;
            var contents = (from content in db.Contents
                            join cat in db.Categories
                            on content.CategoryId equals cat.Id
                            join editor in db.Admins
                            on content.CreateUser equals editor.Id
                            orderby content.Id descending
                            where cat.Id == categoryId
                                  && content.Status == StatusTypes.Active
                                  && cat.Status == StatusTypes.Active
                                  && cat.CategoryType == categoryType
                                  && content.PublishDate < dateNow
                            select new
                            {
                                content.Id,
                                content.CreateDate,
                                Subject = content.Name,
                                content.GameType,
                                EditorName = editor.Name,
                                Description = content.Detail,
                                Description2 = content.Description,
                                content.Url,
                                content.Picture
                            });

            var records = contents.ToPagedList(skip, take);
            var contentItems = records.Select(a =>
            {
                //var detail = Utility.StripHtml(a.Description);
                //detail = detail.Length > 160 ? detail.Substring(0, 160) + "..." : detail;

                //detail = detail.Length < 10 ? a.Description2 : detail;
                var item = new CategoryContentItemViewModel
                {
                    Id = a.Id,
                    Url = a.Url,
                    Subject = a.Subject,
                    Description = a.Description2,
                    PictureName = a.Picture,
                    CreateDate = a.CreateDate,
                    GameTypes = a.GameType,
                    GameType = Tool.GetGameTypeText[a.GameType],
                    EditorName = a.EditorName
                };
                return item;
            }).ToList();

            var result = new StaticPagedList<CategoryContentItemViewModel>(contentItems, records);

            return result;
        }

        public StaticPagedList<CategoryContentItemViewModel> GetCategoryContentSearch(string keys, CategoryTypes categoryType, int skip, int take)
        {
            var db = _dbFactory();

            var key = keys.Split(' ').ToList();

            var contents = (from content in db.Contents
                            join cat in db.Categories
                            on content.CategoryId equals cat.Id
                            join editor in db.Admins
                            on content.CreateUser equals editor.Id
                            orderby content.Id descending
                            where content.Status == StatusTypes.Active
                                  && cat.Status == StatusTypes.Active
                                  && content.ContentType == ContentTypes.Content
                                  && cat.CategoryType == categoryType
                            select new
                            {
                                content.Id,
                                content.CreateDate,
                                Subject = content.Name,
                                content.GameType,
                                EditorName = editor.Name,
                                Description = content.Detail,
                                content.Url,
                                content.Picture,
                                CategoryUrl = cat.Url,
                                content.Keyword,
                                content.Tags,
                                content.Title
                            });

            key.ForEach(item =>
            {
                contents = contents.Where(a => a.Subject.Contains(item) || a.Url.Contains(item) || a.Description.Contains(item) || a.Tags.Contains(item) || a.Title.Contains(item));
            });

            var records = contents.ToPagedList(skip, take);
            var contentItems = records.Select(a =>
            {
                var detail = Utility.StripHtml(a.Description);
                detail = detail.Length > 160 ? detail.Substring(0, 160) + "..." : detail;
                var item = new CategoryContentItemViewModel
                {
                    Id = a.Id,
                    Url = $"{a.CategoryUrl}/{a.Url}",
                    Subject = a.Subject,
                    Description = detail,
                    PictureName = a.Url,
                    CreateDate = a.CreateDate,
                    GameTypes = a.GameType,
                    GameType = Tool.GetGameTypeText[a.GameType],
                    EditorName = a.EditorName
                };
                return item;
            }).ToList();

            var result = new StaticPagedList<CategoryContentItemViewModel>(contentItems, records);

            return result;
        }

        public List<GameTypeContentViewModel> GetFourSubContent()
        {
            var dateNow = DateTime.Now;
            var db = _dbFactory();
            var contents = (from dotaContent in db.Contents
                            join category in db.Categories
                            on dotaContent.CategoryId equals category.Id
                            orderby dotaContent.Id descending
                            where dotaContent.GameType == GameTypes.Dota2 
                            && dotaContent.Status == StatusTypes.Active
                            && dotaContent.PublishDate < dateNow
                            && category.CategoryType == CategoryTypes.Story
                            select new
                            {
                                dotaContent.Name,
                                dotaContent.Url,
                                dotaContent.GameType,
                                dotaContent.Description,
                                CategoryUrl = category.Url
                            }).Take(4)
                .Union(
                    (from csContent in db.Contents
                     join category in db.Categories
                        on csContent.CategoryId equals category.Id
                     orderby csContent.Id descending
                     where csContent.GameType == GameTypes.CsGo &&
                           csContent.Status == StatusTypes.Active
                           && csContent.PublishDate < dateNow
                           && category.CategoryType == CategoryTypes.Story
                     select new
                     {
                         csContent.Name,
                         csContent.Url,
                         csContent.GameType,
                         csContent.Description,
                         CategoryUrl = category.Url
                     }).Take(4)
                ).Union(
                    (from heartstoneContent in db.Contents
                     join category in db.Categories
                        on heartstoneContent.CategoryId equals category.Id
                     orderby heartstoneContent.Id descending
                     where heartstoneContent.GameType == GameTypes.Heartstone &&
                           heartstoneContent.Status == StatusTypes.Active
                           && heartstoneContent.PublishDate < dateNow
                           && category.CategoryType == CategoryTypes.Story
                     select new
                     {
                         heartstoneContent.Name,
                         heartstoneContent.Url,
                         heartstoneContent.GameType,
                         heartstoneContent.Description,
                         CategoryUrl = category.Url
                     }).Take(4)
                ).Union(
                    (from arenaOfValorContent in db.Contents
                     join category in db.Categories
                        on arenaOfValorContent.CategoryId equals category.Id
                     orderby arenaOfValorContent.Id descending
                     where arenaOfValorContent.GameType == GameTypes.StrikeOfKings &&
                           arenaOfValorContent.Status == StatusTypes.Active
                           && arenaOfValorContent.PublishDate < dateNow
                           && category.CategoryType == CategoryTypes.Story
                     select new
                     {
                         arenaOfValorContent.Name,
                         arenaOfValorContent.Url,
                         arenaOfValorContent.GameType,
                         arenaOfValorContent.Description,
                         CategoryUrl = category.Url
                     }).Take(4)
                ).ToList();

            var result = contents.Select(a => new GameTypeContentViewModel
            {
                Name = a.Name,
                Url = $"{a.CategoryUrl}/{a.Url}",
                Picture = a.Url,
                GameType = a.GameType,
                GameTypeText = Tool.GetGameTypeText[a.GameType],
                Description = a.Description
            }).ToList();

            return result;
        }

        //ping content & rss content
        public IList<WidgetContentViewModel> GetTopContent(int take)
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

            var result = contents.Take(take).ToList().Select(a =>
            {
                var description = Utility.StripHtml(a.Detail);
                description = description.Length > 100 ? description.Substring(0, 100) + "..." : description;
                var item = new WidgetContentViewModel
                {
                    Description = description,
                    GameTypes = a.GameType,
                    GameType = Tool.GetGameTypeText[a.GameType],
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

        public List<RssViewModel> GetRssContent(int take)
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
                    content.Id,
                    content.Name,
                    content.Picture,
                    ContentUrl = content.Url,
                    CategoryUrl = category.Url,
                    content.CreateDate,
                    content.ModifiedDate
                });

            var result = contents.Take(take).ToList().Select(a =>
            {
                var description = Utility.StripHtml(a.Detail);
                description = description.Length > 100 ? description.Substring(0, 100) + "..." : description;
                var item = new RssViewModel
                {
                    Description = description,
                    Id = a.Id,
                    Name = a.Name,
                    Picture = a.ContentUrl,
                    CategoryUrl = a.CategoryUrl,
                    ContentUrl = a.ContentUrl,
                    CreateDate = a.CreateDate,
                    ModifiedDate = a.ModifiedDate
                };

                return item;
            }).ToList();

            return result;
        }

        //video
        public StaticPagedList<CategoryVideoItemViewModel> GetCategoryVideo(string categoryUrl, CategoryTypes categoryType, int skip, int take)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Url == categoryUrl && a.Status == StatusTypes.Active);
            if (category == null)
                throw new CustomException(Lang.categoryNotFound);

            var dateNow = DateTime.Now;

            var categoryId = category.Id;
            var contents = (from content in db.Contents
                            join cat in db.Categories
                            on content.CategoryId equals cat.Id
                            orderby content.Id descending
                            where cat.Id == categoryId
                                  && content.Status == StatusTypes.Active
                                  && cat.Status == StatusTypes.Active
                                  && cat.CategoryType == categoryType
                                  && content.PublishDate < dateNow
                            select new
                            {
                                content.Id,
                                content.CreateDate,
                                Subject = content.Name,
                                content.GameType,
                                Description = content.Detail,
                                content.Url,
                                content.VideoUrl,
                                content.FilterType,
                                content.CategoryId
                            });

            var records = contents.ToPagedList(skip, take);
            var contentItems = records.Select(a =>
            {
                var detail = Utility.StripHtml(a.Description);
                detail = detail.Length > 160 ? detail.Substring(0, 160) + "..." : detail;
                var item = new CategoryVideoItemViewModel
                {
                    Id = a.Id,
                    Url = a.Url,
                    Subject = a.Subject,
                    Description = detail,
                    Picture = a.Url,
                    VideoUrl = a.VideoUrl,
                    GameType = Tool.GetGameTypeText[a.GameType],
                    CategoryId = a.CategoryId,
                    Detail = a.Description,
                    FilterTypes = a.FilterType,
                    GameTypes = a.GameType
                };
                return item;
            }).ToList();

            var result = new StaticPagedList<CategoryVideoItemViewModel>(contentItems, records);

            return result;
        }

        public StaticPagedList<CategoryVideoItemViewModel> GetCategoryVideo(string categoryUrl, CategoryTypes categoryType, GameTypes gameType, ContentFilterTypes filterType, int skip, int take)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Url == categoryUrl && a.Status == StatusTypes.Active);
            if (category == null)
                throw new CustomException(Lang.categoryNotFound);

            var categoryId = category.Id;
            var contents = (from content in db.Contents
                            join cat in db.Categories
                            on content.CategoryId equals cat.Id
                            orderby content.Id descending
                            where cat.Id == categoryId
                                  && content.Status == StatusTypes.Active
                                  && cat.Status == StatusTypes.Active
                                  && cat.CategoryType == categoryType
                            select new
                            {
                                content.Id,
                                content.CreateDate,
                                Subject = content.Name,
                                content.GameType,
                                Description = content.Detail,
                                content.Url,
                                content.VideoUrl,
                                content.FilterType,
                                content.CategoryId,
                                content.Detail
                            });

            if (gameType != GameTypes.All)
                contents = contents.Where(a => a.GameType == gameType);

            if (filterType != ContentFilterTypes.All)
                contents = contents.Where(a => a.FilterType == filterType);

            var records = contents.ToPagedList(skip, take);
            var contentItems = records.Select(a =>
            {
                var detail = Utility.StripHtml(a.Description);
                detail = detail.Length > 160 ? detail.Substring(0, 160) + "..." : detail;
                var item = new CategoryVideoItemViewModel
                {
                    Id = a.Id,
                    Url = a.Url,
                    Subject = a.Subject,
                    Description = detail,
                    Picture = a.Url,
                    VideoUrl = a.VideoUrl,
                    GameTypes = a.GameType,
                    GameType = Tool.GetGameTypeText[a.GameType],
                    CategoryId = a.CategoryId,
                    Detail = a.Detail,
                    FilterTypes = a.FilterType,
                };
                return item;
            }).ToList();

            var result = new StaticPagedList<CategoryVideoItemViewModel>(contentItems, records);

            return result;
        }

        public ContentNavigateViewModel GetLastAndNextVideo()
        {
            var db = _dbFactory();

            var dateNow = DateTime.Now;

            var showcaseVideo = db.Contents.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ContentType == ContentTypes.Video && a.Status == StatusTypes.Active && a.IsShowcase && a.PublishDate < dateNow);
            if (showcaseVideo == null)
                return null;

            var videoId = showcaseVideo.Id;
            var videoUrl = showcaseVideo.Url;

            var previousContent = db.Contents.FirstOrDefault(a => a.ContentType == ContentTypes.Video && a.Status == StatusTypes.Active && a.Id < videoId && a.PublishDate < dateNow);
            var nextContent = db.Contents.FirstOrDefault(a => a.ContentType == ContentTypes.Video && a.Status == StatusTypes.Active && a.Id > videoId && a.PublishDate < dateNow);

            var likedCount = db.Counters.Count(a => a.ContentUrl == videoUrl && a.ContentType == ContentTypes.Video);

            var returnModel = new ContentNavigateViewModel
            {
                FirstVideo = new CategoryVideoItemViewModel
                {
                    Description = showcaseVideo.Description,
                    Detail = showcaseVideo.Detail,
                    GameType = Tool.GetGameTypeText[showcaseVideo.GameType],
                    Id = showcaseVideo.Id,
                    Picture = showcaseVideo.Url,
                    Subject = showcaseVideo.Name,
                    Url = showcaseVideo.Url,
                    VideoUrl = showcaseVideo.VideoUrl,
                    GameTypes = showcaseVideo.GameType,
                    ContentType = ContentTypes.Video,
                    CategoryId = showcaseVideo.CategoryId,
                    FilterTypes = showcaseVideo.FilterType,
                    LikedCount = likedCount
                }
            };

            if (nextContent != null)
                returnModel.NextVideo = new NavigateVideoLink
                {
                    Name = nextContent.Name,
                    Url = nextContent.Url
                };

            if (previousContent != null)
                returnModel.PrevVideo = new NavigateVideoLink
                {
                    Name = previousContent.Name,
                    Url = previousContent.Url
                };

            return returnModel;
        }

        public ContentNavigateViewModel GetLastAndNextVideo(string url)
        {
            var db = _dbFactory();

            var showcaseVideo = db.Contents.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ContentType == ContentTypes.Video && a.Status == StatusTypes.Active && a.Url == url);
            if (showcaseVideo == null)
                return null;

            var likedCount = db.Counters.Count(a => a.ContentUrl == url && a.ContentType == ContentTypes.Video);

            var returnModel = new ContentNavigateViewModel
            {
                FirstVideo = new CategoryVideoItemViewModel
                {
                    Description = showcaseVideo.Description,
                    Detail = showcaseVideo.Detail,
                    GameType = Tool.GetGameTypeText[showcaseVideo.GameType],
                    Id = showcaseVideo.Id,
                    Picture = showcaseVideo.Url,
                    Subject = showcaseVideo.Name,
                    Url = showcaseVideo.Url,
                    VideoUrl = showcaseVideo.VideoUrl,
                    FilterTypes = showcaseVideo.FilterType,
                    GameTypes = showcaseVideo.GameType,
                    CategoryId = showcaseVideo.CategoryId,
                    ContentType = ContentTypes.Video,
                    LikedCount = likedCount
                }
            };

            var videoId = showcaseVideo.Id;
            var previousContent = db.Contents.FirstOrDefault(a => a.ContentType == ContentTypes.Video && a.Status == StatusTypes.Active && a.Id < videoId);
            var nextContent = db.Contents.FirstOrDefault(a => a.ContentType == ContentTypes.Video && a.Status == StatusTypes.Active && a.Id > videoId);

            if (nextContent != null)
                returnModel.NextVideo = new NavigateVideoLink
                {
                    Name = nextContent.Name,
                    Url = nextContent.Url,
                    Picture = nextContent.Url
                };

            if (previousContent != null)
                returnModel.PrevVideo = new NavigateVideoLink
                {
                    Name = previousContent.Name,
                    Url = previousContent.Url,
                    Picture = previousContent.Url
                };

            return returnModel;
        }

        public CategoryVideoItemViewModel GetDayOfVideo()
        {
            var dateNow = DateTime.Now;

            var db = _dbFactory();
            var video =
                db.Contents.OrderByDescending(a => a.Id)
                    .FirstOrDefault(a => a.ContentType == ContentTypes.Video && a.Status == StatusTypes.Active && a.PublishDate < dateNow);

            if (video == null)
                return null;

            var result = new CategoryVideoItemViewModel
            {
                CategoryId = video.CategoryId,
                Description = video.Description,
                Detail = video.Detail,
                FilterTypes = video.FilterType,
                GameType = Tool.GetGameTypeText[video.GameType],
                GameTypes = video.GameType,
                Id = video.Id,
                Picture = video.Url,
                Subject = video.Name,
                Url = video.Url,
                VideoUrl = video.VideoUrl
            };

            return result;
        }

        public List<CategoryVideoItemViewModel> GetLastedVideos()
        {
            var db = _dbFactory();

            var dateNow = DateTime.Now;

            var videos = (from content in db.Contents
                          join category in db.Categories
                          on content.CategoryId equals category.Id
                          orderby content.Id descending
                          where content.Status == StatusTypes.Active
                                && category.CategoryType == CategoryTypes.Video
                                && category.Status == StatusTypes.Active
                                && content.PublishDate < dateNow
                                && content.FilterType != ContentFilterTypes.HomepageStatic
                          select new
                          {
                              content.Id,
                              content.CategoryId,
                              content.ContentType,
                              content.FilterType,
                              content.VideoUrl,
                              content.Name,
                              content.Url,
                              content.GameType,
                              content.Description,
                              content.Detail
                          }).Take(6).ToList();

            var result = videos.Select(a => new CategoryVideoItemViewModel
            {
                CategoryId = a.CategoryId,
                Description = a.Description,
                Detail = a.Detail,
                FilterTypes = a.FilterType,
                GameType = Tool.GetGameTypeText[a.GameType],
                GameTypes = a.GameType,
                Id = a.Id,
                Picture = a.Url,
                Subject = a.Name,
                Url = a.Url,
                VideoUrl = a.VideoUrl
            }).ToList();

            return result;
        }

        public List<CategoryVideoItemViewModel> GetHomeStaticVideos()
        {
            var db = _dbFactory();
            var dateNow = DateTime.Now;
            var videos = (from content in db.Contents
                join category in db.Categories
                on content.CategoryId equals category.Id
                orderby content.Id descending
                where content.Status == StatusTypes.Active
                      && category.CategoryType == CategoryTypes.Video
                      && category.Status == StatusTypes.Active
                      && content.PublishDate < dateNow
                      && content.FilterType == ContentFilterTypes.HomepageStatic
                select new
                {
                    content.Id,
                    content.CategoryId,
                    content.ContentType,
                    content.FilterType,
                    content.VideoUrl,
                    content.Name,
                    content.Url,
                    content.GameType,
                    content.Description,
                    content.Detail
                }).Take(2).ToList();

            var result = videos.Select(a => new CategoryVideoItemViewModel
            {
                CategoryId = a.CategoryId,
                Description = a.Description,
                Detail = a.Detail,
                FilterTypes = a.FilterType,
                GameType = Tool.GetGameTypeText[a.GameType],
                GameTypes = a.GameType,
                Id = a.Id,
                Picture = a.Url,
                Subject = a.Name,
                Url = a.Url,
                VideoUrl = a.VideoUrl
            }).ToList();

            return result;
        }

        //gallery
        public StaticPagedList<CategoryGalleryViewModel> GetCategoryGallery(string categoryUrl, CategoryTypes categoryType, int skip, int take)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Url == categoryUrl && a.Status == StatusTypes.Active);
            if (category == null)
                throw new CustomException(Lang.categoryNotFound);

            var categoryId = category.Id;
            var galleries = (from gallery in db.Galleries
                             join cat in db.Categories
                             on gallery.CategoryId equals cat.Id
                             orderby gallery.Id descending
                             where cat.Id == categoryId
                             && gallery.Status == StatusTypes.Active
                             && cat.Status == StatusTypes.Active
                             && cat.CategoryType == categoryType
                             select new
                             {
                                 gallery.Id,
                                 gallery.CreateDate,
                                 Subject = gallery.Name,
                                 gallery.GameType,
                                 gallery.Description,
                                 gallery.Url,
                                 gallery.CategoryId
                             });

            var records = galleries.ToPagedList(skip, take);

            var galleryItems = records.Select(a =>
            {
                var picture = string.Empty;
                var showcasePicture = _galleryService.GetGalleryShowcasePicture(a.Id);
                if (showcasePicture != null)
                    picture = showcasePicture.Picture;

                var item = new CategoryGalleryViewModel
                {
                    CategoryId = a.CategoryId,
                    CreateDate = a.CreateDate,
                    Description = a.Description,
                    GameType = a.GameType,
                    ContentType = ContentTypes.Gallery,
                    GameTypeText = Tool.GetGameTypeText[a.GameType],
                    Id = a.Id,
                    Subject = a.Subject,
                    Url = a.Url,
                    Picture = picture
                };

                return item;
            });

            var result = new StaticPagedList<CategoryGalleryViewModel>(galleryItems, records);
            return result;
        }

        public StaticPagedList<CategoryGalleryViewModel> GetCategoryGallery(string categoryUrl, CategoryTypes categoryType, GameTypes gameType, int skip, int take)
        {
            var db = _dbFactory();
            var category = db.Categories.FirstOrDefault(a => a.Url == categoryUrl && a.Status == StatusTypes.Active);
            if (category == null)
                throw new CustomException(Lang.categoryNotFound);

            var categoryId = category.Id;
            var galleries = (from gallery in db.Galleries
                             join cat in db.Categories
                             on gallery.CategoryId equals cat.Id
                             orderby gallery.Id descending
                             where cat.Id == categoryId
                             && gallery.Status == StatusTypes.Active
                             && cat.Status == StatusTypes.Active
                             && cat.CategoryType == categoryType
                             select new
                             {
                                 gallery.Id,
                                 gallery.CreateDate,
                                 Subject = gallery.Name,
                                 gallery.GameType,
                                 gallery.Description,
                                 gallery.Url,
                                 gallery.CategoryId
                             });

            if (gameType != GameTypes.All)
                galleries = galleries.Where(a => a.GameType == gameType);

            var records = galleries.ToPagedList(skip, take);

            var galleryItems = records.Select(a =>
            {
                var picture = string.Empty;
                var showcasePicture = _galleryService.GetGalleryShowcasePicture(a.Id);
                if (showcasePicture != null)
                    picture = showcasePicture.Picture;

                var item = new CategoryGalleryViewModel
                {
                    CategoryId = a.CategoryId,
                    CreateDate = a.CreateDate,
                    Description = a.Description,
                    GameType = a.GameType,
                    ContentType = ContentTypes.Gallery,
                    Id = a.Id,
                    Subject = a.Subject,
                    Url = a.Url,
                    GameTypeText = Tool.GetGameTypeText[a.GameType],
                    Picture = picture
                };

                return item;
            });

            var result = new StaticPagedList<CategoryGalleryViewModel>(galleryItems, records);
            return result;
        }

        public GalleryNavigateViewModel GetLastAndNextGallery()
        {
            var db = _dbFactory();

            var selectedGallery = db.Galleries.OrderByDescending(a => a.Id).Skip(1).FirstOrDefault(a => a.Status == StatusTypes.Active);
            if (selectedGallery == null)
                return null;

            var galleryUrl = selectedGallery.Url;
            var galleryId = selectedGallery.Id;

            var previousGallery = db.Galleries.FirstOrDefault(a => a.Status == StatusTypes.Active && a.Id < galleryId);
            var nextGallery = db.Galleries.FirstOrDefault(a => a.Status == StatusTypes.Active && a.Id > galleryId);

            var likedCount = db.Counters.Count(a => a.ContentUrl == galleryUrl && a.ContentType == ContentTypes.Gallery);

            var returnModel = new GalleryNavigateViewModel
            {
                FirstGallery = new CategoryGalleryItemViewModel
                {
                    Description = selectedGallery.Description,
                    GameType = Tool.GetGameTypeText[selectedGallery.GameType],
                    Id = selectedGallery.Id,
                    Subject = selectedGallery.Name,
                    Url = selectedGallery.Url,
                    CategoryId = selectedGallery.CategoryId,
                    GameTypes = selectedGallery.GameType,
                    Pictures = _galleryService.GetGalleryDetails(selectedGallery.Id),
                    ContentType = ContentTypes.Gallery,
                    LikedCount = likedCount
                }
            };

            if (nextGallery != null)
            {
                var nextId = nextGallery.Id;
                var picture = "";
                var nextGalleryShowcasePic = db.GalleryDetails.OrderBy(a => a.Id).FirstOrDefault(a => a.GalleryId == nextId);
                if (nextGalleryShowcasePic != null)
                    picture = nextGalleryShowcasePic.PictureUrl;

                returnModel.NextGallery = new NavigateGalleryLink
                {
                    Name = nextGallery.Name,
                    Url = nextGallery.Url,
                    Picture = picture
                };
            }

            if (previousGallery != null)
            {
                var prevId = previousGallery.Id;
                var picture = "";
                var prevGalleryShowcasePic = db.GalleryDetails.OrderBy(a => a.Id).FirstOrDefault(a => a.GalleryId == prevId);
                if (prevGalleryShowcasePic != null)
                    picture = prevGalleryShowcasePic.PictureUrl;

                returnModel.PrevGallery = new NavigateGalleryLink
                {
                    Name = previousGallery.Name,
                    Url = previousGallery.Url,
                    Picture = picture
                };
            }

            return returnModel;
        }

        public GalleryNavigateViewModel GetLastAndNextGallery(string url)
        {
            var db = _dbFactory();

            var selectedGallery = db.Galleries.FirstOrDefault(a => a.Url == url && a.Status == StatusTypes.Active);
            if (selectedGallery == null)
                return null;

            var galleryId = selectedGallery.Id;

            var previousGallery = db.Galleries.FirstOrDefault(a => a.Status == StatusTypes.Active && a.Id < galleryId);
            var nextGallery = db.Galleries.FirstOrDefault(a => a.Status == StatusTypes.Active && a.Id > galleryId);

            var likedCount = db.Counters.Count(a => a.ContentUrl == url && a.ContentType == ContentTypes.Gallery);

            var returnModel = new GalleryNavigateViewModel
            {
                FirstGallery = new CategoryGalleryItemViewModel
                {
                    Description = selectedGallery.Description,
                    GameType = Tool.GetGameTypeText[selectedGallery.GameType],
                    Id = selectedGallery.Id,
                    Subject = selectedGallery.Name,
                    Url = selectedGallery.Url,
                    CategoryId = selectedGallery.CategoryId,
                    GameTypes = selectedGallery.GameType,
                    Pictures = _galleryService.GetGalleryDetails(selectedGallery.Id),
                    ContentType = ContentTypes.Gallery,
                    LikedCount = likedCount
                }
            };

            if (nextGallery != null)
                returnModel.NextGallery = new NavigateGalleryLink
                {
                    Name = nextGallery.Name,
                    Url = nextGallery.Url
                };

            if (previousGallery != null)
                returnModel.PrevGallery = new NavigateGalleryLink
                {
                    Name = previousGallery.Name,
                    Url = previousGallery.Url
                };

            return returnModel;
        }

        public ContentDetailViewModel GetContentDetail(string contentUrl)
        {
            var dateNow = DateTime.Now;

            var db = _dbFactory();
            var content = db.Contents.FirstOrDefault(a => a.Status == StatusTypes.Active && a.Url == contentUrl && a.PublishDate < dateNow);
            if (content == null)
                throw new CustomException(Lang.contentNotFound);

            const string advertCode = "[ADVERTCODE]";
            var categoryId = content.CategoryId;

            #region advert code checking
            if (content.Detail.Contains(advertCode))
            {
                var category = db.Categories.FirstOrDefault(a => a.Id == categoryId);
                if (category != null)
                {
                    var categoryUrl = category.Url;
                    var language = content.LanguageTag;
                    var advert = _advertService.GetLocationAdverts(categoryUrl, language, AdvertLocationTypes.Content).FirstOrDefault();
                    if (advert != null)
                    {
                        var advertHtmlCode = advert.AdType == AdvertTypes.Image ? $"<a href=\"/advert/redirect?guid={advert.Guid}\"><img src=\"/Content/File/{advert.FilePath}\" alt=\"\"/></a>" : advert.Code;

                        content.Detail = content.Detail.Replace(advertCode, advertHtmlCode);
                    }
                    else
                        content.Detail = content.Detail.Replace(advertCode, "");
                }
            }
            #endregion

            var returnModel = new ContentDetailViewModel
            {
                Content = new ContentViewModel
                {
                    CreateDate = content.CreateDate,
                    CreateUser = content.CreateUser,
                    Description = content.Description,
                    Detail = content.Detail,
                    GameTypes = content.GameType,
                    GameType = Tool.GetGameTypeText[content.GameType],
                    Hit = content.Hit,
                    IsShowcase = content.IsShowcase,
                    Keyword = content.Keyword,
                    ModifiedDate = content.ModifiedDate,
                    ModifiedUser = content.ModifiedUser,
                    Name = content.Name,
                    Picture = content.Picture,
                    Tags = !string.IsNullOrEmpty(content.Tags) ? content.Tags.Split(',') : new string[0],
                    Id = content.Id,
                    Title = content.Title,
                    Url = content.Url
                }
            };

            var contentId = content.Id;
            var userId = content.CreateUser;

            var editor = db.Admins.FirstOrDefault(a => a.Id == userId);

            var previousContent = db.Contents.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ContentType == ContentTypes.Content && a.Status == StatusTypes.Active && a.CategoryId == categoryId && a.Id < contentId);
            var nextContent = db.Contents.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ContentType == ContentTypes.Content && a.Status == StatusTypes.Active && a.CategoryId == categoryId && a.Id > contentId);

            if (editor != null)
                returnModel.Editor = new EditorUser
                {
                    Username = editor.Username,
                    Motto = editor.Motto,
                    NameSurname = editor.Name,
                    Picture = editor.Picture,
                    Title = Tool.GetEditorTypeText[editor.AdminType]
                };

            if (nextContent != null)
                returnModel.NextContent = new NavigateLink
                {
                    Name = nextContent.Name,
                    Url = nextContent.Url
                };

            if (previousContent != null)
                returnModel.PrevContent = new NavigateLink
                {
                    Name = previousContent.Name,
                    Url = previousContent.Url
                };

            return returnModel;
        }

        public IList<WidgetContentViewModel> GetTopContent(string categoryUrl)
        {
            var language = _setting.Language;

            var db = _dbFactory();
            var dateNow = DateTime.Now;
            var contents = (from content in db.Contents
                            join category in db.Categories
                            on content.CategoryId equals category.Id
                            orderby content.Id descending
                            where content.ContentType == ContentTypes.Content &&
                                  content.FilterType == ContentFilterTypes.TopRated &&
                                  content.Status == StatusTypes.Active &&
                                  category.Status == StatusTypes.Active &&
                                  content.PublishDate < dateNow &&
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
                var description = Utility.StripHtml(a.Detail);
                description = description.Length > 100 ? description.Substring(0, 100) + "..." : description;
                var item = new WidgetContentViewModel
                {
                    Description = description,
                    GameTypes = a.GameType,
                    GameType = Tool.GetGameTypeText[a.GameType],
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

        public StaticPagedList<CategoryContentItemViewModel> GetTagSearch(string tag, int skip, int take)
        {
            var combineTag = $"{tag},";

            var db = _dbFactory();
            var contents = (from content in db.Contents
                            join cat in db.Categories
                            on content.CategoryId equals cat.Id
                            join editor in db.Admins
                            on content.CreateUser equals editor.Id
                            orderby content.Id descending
                            where cat.Id == content.CategoryId
                                  && content.Status == StatusTypes.Active
                                  && cat.Status == StatusTypes.Active
                                  && content.ContentType == ContentTypes.Content
                                  && (content.Tags.Contains(combineTag) || content.Tags.Contains(tag))
                            select new
                            {
                                content.Id,
                                content.CreateDate,
                                Subject = content.Name,
                                content.GameType,
                                EditorName = editor.Name,
                                Description = content.Detail,
                                content.Url,
                                content.Picture,
                                CategoryUrl = cat.Url
                            });

            var records = contents.ToPagedList(skip, take);
            var contentItems = records.Select(a =>
            {
                var detail = Utility.StripHtml(a.Description);
                detail = detail.Length > 160 ? detail.Substring(0, 160) + "..." : detail;
                var item = new CategoryContentItemViewModel
                {
                    Id = a.Id,
                    Url = $"{a.CategoryUrl}/{a.Url}",
                    Subject = a.Subject,
                    Description = detail,
                    PictureName = a.Url,
                    CreateDate = a.CreateDate,
                    GameTypes = a.GameType,
                    GameType = Tool.GetGameTypeText[a.GameType],
                    EditorName = $"{a.EditorName}"
                };
                return item;
            }).ToList();

            var result = new StaticPagedList<CategoryContentItemViewModel>(contentItems, records);

            return result;
        }
    }
}