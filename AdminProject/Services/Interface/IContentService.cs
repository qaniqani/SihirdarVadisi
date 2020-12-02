using System.Collections.Generic;
using AdminProject.Areas.Admin.Models;
using AdminProject.Models;
using AdminProject.Services.Models;
using PagedList;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public interface IContentService : IBaseInterface<Content>
    {
        IList<Content> OrderList();
        string UrlCheck(string url);
        List<RssViewModel> GetRssContent(int take);
        CategoryVideoItemViewModel GetDayOfVideo();
        IList<ContentListItemDto> ListCategories();
        IList<Content> SetOrderList(string[] order);
        IList<WidgetContentViewModel> GetTopContent(string categoryUrl);
        ContentNavigateViewModel GetLastAndNextVideo();
        GalleryNavigateViewModel GetLastAndNextGallery();
        List<GameTypeContentViewModel> GetFourSubContent();
        List<CategoryVideoItemViewModel> GetLastedVideos();
        PagerList<Content> AllContentList(int skip, int take);
        IList<WidgetContentViewModel> GetTopContent(int take);
        List<CategoryVideoItemViewModel> GetHomeStaticVideos();
        ContentNavigateViewModel GetLastAndNextVideo(string url);
        GalleryNavigateViewModel GetLastAndNextGallery(string url);
        ContentDetailViewModel GetContentDetail(string contentUrl);
        PagerList<ContentListItemDto> ListCategories(ContentSearchRequestDto request);
        PagerList<Content> AllContentList(int skip, int take, StatusTypes status);
        StaticPagedList<CategoryContentItemViewModel> GetTagSearch(string tag, int skip, int take);
        StaticPagedList<CategoryGalleryViewModel> GetCategoryGallery(string categoryUrl, CategoryTypes categoryType, int skip, int take);
        StaticPagedList<CategoryVideoItemViewModel> GetCategoryVideo(string categoryUrl, CategoryTypes categoryType, int skip, int take);
        StaticPagedList<CategoryContentItemViewModel> GetCategoryContentSearch(string keys, CategoryTypes categoryType, int skip, int take);
        StaticPagedList<CategoryContentItemViewModel> GetCategoryContent(string categoryUrl, CategoryTypes categoryType, int skip, int take);
        StaticPagedList<CategoryGalleryViewModel> GetCategoryGallery(string categoryUrl, CategoryTypes categoryType, GameTypes gameType, int skip, int take);
        StaticPagedList<CategoryVideoItemViewModel> GetCategoryVideo(string categoryUrl, CategoryTypes categoryType, GameTypes gameType, ContentFilterTypes filterType, int skip, int take);
    }
}