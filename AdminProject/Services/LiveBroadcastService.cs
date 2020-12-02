using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services
{
    public class LiveBroadcastService : ILiveBroadcastService
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public LiveBroadcastService(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public void Add(LiveBroadcast instance)
        {
            var db = _dbFactory();
            db.LiveBroadcasts.Add(instance);
            db.SaveChanges();
        }

        public string UrlCheck(string url)
        {
            var db = _dbFactory();

            var count = db.LiveBroadcasts.Count(a => a.Url == url);
            if (count > 0)
                url = $"{url}-{count}";

            return url;
        }

        public void Edit(int id, LiveBroadcast newInstance)
        {
            var db = _dbFactory();
            var live = db.LiveBroadcasts.FirstOrDefault(a => a.Id == id);
            if (live == null)
                return;

            live.ChatAddress = newInstance.ChatAddress;
            live.Description = newInstance.Description;
            live.GameType = newInstance.GameType;
            live.Language = newInstance.Language;
            live.LanguageId = newInstance.LanguageId;
            live.Live = newInstance.Live;
            live.Name = newInstance.Name;
            live.PublishAddress = newInstance.PublishAddress;
            live.Status = newInstance.Status;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var live = db.LiveBroadcasts.FirstOrDefault(a => a.Id == id);
            db.LiveBroadcasts.Remove(live);
            db.SaveChanges();
        }

        public IList<LiveBroadcast> List()
        {
            var db = _dbFactory();
            var language = _setting.Language;
            var lives = db.LiveBroadcasts.Where(a => a.Language == language).OrderBy(a => a.SequenceNumber).ToList();
            return lives;
        }

        public IList<LiveBroadcast> ActiveList()
        {
            var db = _dbFactory();
            var lives = db.LiveBroadcasts.OrderBy(a => a.SequenceNumber).Where(a => a.Status == StatusTypes.Active).ToList();
            return lives;
        }

        public LiveBroadcast GetItem(int instanceId)
        {
            var db = _dbFactory();
            var item = db.LiveBroadcasts.FirstOrDefault(a => a.Id == instanceId);
            return item;
        }

        public LiveBroadcast GetItem(string url)
        {
            var db = _dbFactory();
            var item = db.LiveBroadcasts.FirstOrDefault(a => a.Url == url && a.Status == StatusTypes.Active && a.Live);
            return item;
        }

        //Site
        public LiveBroadcastListDto GetActiveStream()
        {
            var list = new LiveBroadcastListDto {Live = false};

            var db = _dbFactory();
            var lives =
                db.LiveBroadcasts.OrderBy(a => a.SequenceNumber)
                    .Where(a => a.Status == StatusTypes.Active)
                    .ToList()
                    .Select(a =>
                    {
                        var item = new LiveBroadcastItemDto
                        {
                            GameType = a.GameType,
                            GameTypeText = Tool.GetGameTypeText[a.GameType],
                            Live = a.Live,
                            Name = a.Name,
                            Url = a.Url
                        };

                        if (item.Live)
                            list.Live = true;

                        return item;
                    }).ToList();

            list.Items = lives;

            return list;
        }
    }
}