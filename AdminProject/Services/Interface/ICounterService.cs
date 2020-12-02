using System.Collections.Generic;
using AdminProject.Models;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public interface ICounterService : IBaseInterface<Counter>
    {
        void Delete(int userId, int id);
        IList<Counter> List(int userId);
        List<UserLikeItemDto> CounterList(int userId);
        IList<ContentCounterViewModel> UserCounterList(int userId);
        void Delete(int userId, int contentId, ContentTypes contentType);
        void Delete(int userId, string contentUrl, ContentTypes contentType);
        bool CheckUserLiked(int userId, string contentUrl, ContentTypes contentType);
        Counter UserContentLike(int userId, string contentUrl, ContentTypes contentType);
        int CounterCount(int userId);
    }
}