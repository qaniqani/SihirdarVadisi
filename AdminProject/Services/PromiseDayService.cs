using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services
{
    public class PromiseDayService : IPromiseDayService
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public PromiseDayService(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public void Add(PromiseDay promise)
        {
            var db = _dbFactory();
            db.PromiseDays.Add(promise);
            db.SaveChanges();
        }

        public void Edit(int id, PromiseDay promise)
        {
            var db = _dbFactory();
            var p = db.PromiseDays.FirstOrDefault(a => a.Id == id);
            if (p == null)
                return;

            p.Language = promise.Language;
            p.LanguageId = promise.LanguageId;
            p.Promise = promise.Promise;
            p.PublishDate = promise.PublishDate;
            p.Status = promise.Status;
            p.Teller = promise.Teller;
            p.UpdateDate = promise.UpdateDate;
            p.UpdateEditorId = promise.UpdateEditorId;

            db.SaveChanges();
        }

        public PromiseDay GetItem(int id)
        {
            var db = _dbFactory();
            var promise = db.PromiseDays.FirstOrDefault(a => a.Id == id);
            return promise;
        }

        public IList<PromiseDay> List()
        {
            var db = _dbFactory();
            var promises = db.PromiseDays.OrderByDescending(a => a.PublishDate).ToList();
            return promises;
        }

        public PromiseDay GetDayPromise()
        {
            var db = _dbFactory();

            var toDay = DateTime.Now.Date.AddSeconds(-1);

            var language = _setting.Language;

            var promise =
                db.PromiseDays.FirstOrDefault(a => a.Status == StatusTypes.Active && a.PublishDate >= toDay && a.Language == language);
            if (promise == null)
                return db.PromiseDays.OrderBy(a => Guid.NewGuid()).FirstOrDefault(a => a.Status == StatusTypes.Active && a.Language == language);

            return promise;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var promise = db.PromiseDays.FirstOrDefault(a => a.Id == id);
            db.PromiseDays.Remove(promise);
            db.SaveChanges();
        }
    }
}