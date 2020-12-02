using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services
{
    public class CounterService: ICounterService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public CounterService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Counter instance)
        {
            var db = _dbFactory();
            db.Counters.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Counter newInstance)
        {
            var db = _dbFactory();
            var counter = GetItem(id);
            if (counter == null)
                return;

            counter.ContentId = newInstance.ContentId;
            counter.ContentType = newInstance.ContentType;
            counter.ContentUrl = newInstance.ContentUrl;
            counter.DateTime = newInstance.DateTime;
            counter.UserId = newInstance.UserId;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var counter = GetItem(id);
            if (counter == null)
                return;

            var db = _dbFactory();
            db.Counters.Remove(counter);
            db.SaveChanges();
        }

        public void Delete(int userId, int id)
        {
            var counter = GetItem(id);
            if (counter?.UserId != userId)
                return;

            var db = _dbFactory();
            db.Counters.Remove(counter);
            db.SaveChanges();
        }

        public IList<Counter> List()
        {
            var db = _dbFactory();
            var counters = db.Counters.OrderByDescending(a => a.DateTime).ToList();
            return counters;
        }

        public IList<Counter> List(int userId)
        {
            var db = _dbFactory();
            var counters = db.Counters.OrderByDescending(a => a.DateTime).Where(a => a.UserId == userId).ToList();
            return counters;
        }

        public IList<ContentCounterViewModel> UserCounterList(int userId)
        {
            var db = _dbFactory();
            var counters = (from counter in db.Counters
                join content in db.Contents
                on counter.ContentId equals content.Id
                where counter.UserId == userId
                select new ContentCounterViewModel
                {
                    ContentId = content.Id,
                    ContentStatus = content.Status == StatusTypes.Active,
                    ContentSubject = content.Name,
                    ContentType = content.ContentType,
                    ContentUrl = content.Url,
                    DateTime = counter.DateTime,
                    Id = counter.Id
                }).ToList();

            return counters;
        }

        public List<UserLikeItemDto> CounterList(int userId)
        {
            var db = _dbFactory();
            var likedContents = (from content in db.Contents
                                 join category in db.Categories
                                 on content.CategoryId equals category.Id
                                 join counter in db.Counters
                                 on content.Id equals counter.ContentId into contentCounter
                                 from cc in contentCounter.DefaultIfEmpty()
                                 orderby cc.DateTime descending
                                 where content.Status == StatusTypes.Active
                                 && cc.UserId == userId
                                 select new
                                 {
                                     cc.Id,
                                     CategoryUrl = category.Url,
                                     content.Url,
                                     content.Name,
                                     content.GameType,
                                     cc.DateTime,
                                     cc.ContentType
                                 }).ToList();

            var contentResult = likedContents.Select(a => new UserLikeItemDto
            {
                Id = a.Id,
                DateTime = a.DateTime,
                GameType = a.GameType,
                GameTypeText = Tool.GetGameTypeText[a.GameType],
                Name = a.Name,
                Url = a.Url,
                Picture = a.Url,
                ContentType = a.ContentType
            }).ToList();

            var likedGallery = (from gallery in db.Galleries
                join category in db.Categories
                on gallery.CategoryId equals category.Id
                join counter in db.Counters
                on gallery.Id equals counter.ContentId into contentCounter
                from cc in contentCounter.DefaultIfEmpty()
                orderby cc.DateTime descending
                where gallery.Status == StatusTypes.Active
                      && cc.UserId == userId
                select new
                {
                    cc.Id,
                    CategoryUrl = category.Url,
                    gallery.Url,
                    gallery.Name,
                    gallery.GameType,
                    cc.DateTime,
                    cc.ContentType
                }).ToList();

            var galleryResult = likedGallery.Select(a => new UserLikeItemDto
            {
                Id = a.Id,
                DateTime = a.DateTime,
                GameType = a.GameType,
                GameTypeText = Tool.GetGameTypeText[a.GameType],
                Name = a.Name,
                Url = $"{a.CategoryUrl}/{a.Url}",
                Picture = a.Url,
                ContentType = a.ContentType
            }).ToList();

            var result = contentResult.Concat(galleryResult).OrderByDescending(a => a.Id).ToList();

            return result;
        }

        public int CounterCount(int userId)
        {
            var db = _dbFactory();
            var contentCount = (from content in db.Contents
                                 join category in db.Categories
                                 on content.CategoryId equals category.Id
                                 join counter in db.Counters
                                 on content.Id equals counter.ContentId into contentCounter
                                 from cc in contentCounter.DefaultIfEmpty()
                                 orderby cc.DateTime descending
                                 where content.Status == StatusTypes.Active
                                 && cc.UserId == userId
                                 select content).Count();

            var galleryCount = (from gallery in db.Galleries
                                join category in db.Categories
                                on gallery.CategoryId equals category.Id
                                join counter in db.Counters
                                on gallery.Id equals counter.ContentId into contentCounter
                                from cc in contentCounter.DefaultIfEmpty()
                                orderby cc.DateTime descending
                                where gallery.Status == StatusTypes.Active
                                      && cc.UserId == userId
                                select gallery).Count();

            return contentCount + galleryCount;
        }

        public bool CheckUserLiked(int userId, string contentUrl, ContentTypes contentType)
        {
            var db = _dbFactory();
            var counter = db.Counters.FirstOrDefault(a => a.ContentUrl == contentUrl && a.ContentType == contentType && a.UserId == userId);
            return counter != null;
        }

        public Counter UserContentLike(int userId, string contentUrl, ContentTypes contentType)
        {
            var db = _dbFactory();
            var counter = db.Counters.FirstOrDefault(a => a.ContentUrl == contentUrl && a.ContentType == contentType && a.UserId == userId);
            return counter;
        }

        public void Delete(int userId, int contentId, ContentTypes contentType)
        {
            var db = _dbFactory();
            var counter = db.Counters.FirstOrDefault(a => a.ContentId == contentId && a.ContentType == contentType && a.UserId == userId);
            db.Counters.Remove(counter);
            db.SaveChanges();
        }

        public void Delete(int userId, string contentUrl, ContentTypes contentType)
        {
            var db = _dbFactory();
            var counter = db.Counters.FirstOrDefault(a => a.ContentUrl == contentUrl && a.ContentType == contentType && a.UserId == userId);
            db.Counters.Remove(counter);
            db.SaveChanges();
        }

        public Counter GetItem(int instanceId)
        {
            var db = _dbFactory();
            var counter = db.Counters.FirstOrDefault(a => a.Id == instanceId);
            return counter;
        }
    }
}