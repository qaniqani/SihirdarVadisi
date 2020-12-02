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
    public class AdvertService : IAdvertService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AdvertService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Advert instance)
        {
            var db = _dbFactory();
            db.Adverts.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Advert newInstance)
        {
            var advert = GetItem(id);
            if (advert == null)
                return;

            var db = _dbFactory();
            advert.AdCode = newInstance.AdCode;
            advert.AdFilePath = newInstance.AdFilePath;
            advert.AdFileType = newInstance.AdFileType;
            advert.AdGuid = newInstance.AdGuid;
            advert.AdLink = newInstance.AdLink;
            advert.AdLocation = newInstance.AdLocation;
            advert.AdType = newInstance.AdType;
            advert.ClickHit = newInstance.ClickHit;
            advert.EndDate = newInstance.EndDate;
            advert.Language = newInstance.Language;
            advert.LanguageId = newInstance.LanguageId;
            advert.Name = newInstance.Name;
            advert.SequenceNr = newInstance.SequenceNr;
            advert.StartDate = newInstance.StartDate;
            advert.Status = newInstance.Status;
            advert.UpdatedDate = DateTime.Now;
            advert.UpdateEditorId = newInstance.UpdateEditorId;
            advert.ViewHit = newInstance.ViewHit;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var advert = GetItem(id);
            if (advert == null)
                return;

            var db = _dbFactory();
            db.Adverts.Remove(advert);
            db.SaveChanges();
        }

        public IList<Advert> List()
        {
            var db = _dbFactory();
            var adverts = db.Adverts.OrderBy(a => a.SequenceNr).ToList();
            return adverts;
        }

        public Advert GetItem(int instanceId)
        {
            var db = _dbFactory();
            var advert = db.Adverts.FirstOrDefault(a => a.Id == instanceId);
            return advert;
        }

        //front
        public List<CategoryAdvertDto> GetCategoryAdverts(string categoryUrl, string language)
        {
            var nowDate = DateTime.Now;
            var db = _dbFactory();
            var categoryAdvert = (from a in db.Adverts
                                  join c in db.Categories
                                  on a.CategoryId equals c.Id
                                  orderby a.SequenceNr
                                  where c.Url == categoryUrl
                                        && a.Status == StatusTypes.Active
                                        && a.StartDate <= nowDate
                                        && a.EndDate >= nowDate
                                        && c.Status == StatusTypes.Active
                                        && c.LanguageTag == language
                                        && c.CategoryTagType != CategoryTagTypes.Coming
                                        && (a.AdType == AdvertTypes.Image || a.AdType == AdvertTypes.Adsense || a.AdType == AdvertTypes.Embedded)
                                  select new
                                  {
                                      a.AdCode,
                                      a.AdFilePath,
                                      a.AdFileType,
                                      a.AdGuid,
                                      a.AdLink,
                                      a.AdLocation,
                                      a.AdType,
                                      a.SequenceNr
                                  }).ToList();

            var result = categoryAdvert.Select(a => new CategoryAdvertDto
            {
                AdType = a.AdType,
                Code = a.AdCode,
                FilePath = a.AdFilePath,
                FileType = a.AdFileType,
                Guid = a.AdGuid,
                Link = a.AdLink,
                Location = a.AdLocation,
                SequenceNr = a.SequenceNr
            }).ToList();

            return result;
        }

        public List<CategoryAdvertDto> GetCategoryAdverts(string language, AdvertLocationTypes location)
        {
            var nowDate = DateTime.Now;
            var db = _dbFactory();
            var categoryAdvert = (from a in db.Adverts
                                  orderby a.SequenceNr
                                  where a.Status == StatusTypes.Active
                                        && a.AdLocation == location
                                        && a.StartDate <= nowDate
                                        && a.EndDate >= nowDate
                                        && (a.AdType == AdvertTypes.Image || a.AdType == AdvertTypes.Adsense || a.AdType == AdvertTypes.Embedded)
                                  select new
                                  {
                                      a.AdCode,
                                      a.AdFilePath,
                                      a.AdFileType,
                                      a.AdGuid,
                                      a.AdLink,
                                      a.AdLocation,
                                      a.AdType,
                                      a.SequenceNr
                                  }).ToList();

            var result = categoryAdvert.Select(a => new CategoryAdvertDto
            {
                AdType = a.AdType,
                Code = a.AdCode,
                FilePath = a.AdFilePath,
                FileType = a.AdFileType,
                Guid = a.AdGuid,
                Link = a.AdLink,
                Location = a.AdLocation,
                SequenceNr = a.SequenceNr
            }).ToList();

            return result;
        }

        public Advert GetAdvert(string guid)
        {
            var db = _dbFactory();
            var advert = db.Adverts.FirstOrDefault(a => a.AdGuid == guid);
            if (advert == null)
                return null;

            advert.ClickHit++;
            db.SaveChanges();
            return advert;
        }

        public List<CategoryAdvertDto> GetLocationAdverts(string categoryUrl, string language, AdvertLocationTypes location)
        {
            var nowDate = DateTime.Now;
            var db = _dbFactory();
            var categoryAdvert = (from a in db.Adverts
                                  join c in db.Categories
                                  on a.CategoryId equals c.Id
                                  orderby a.SequenceNr
                                  where c.Url == categoryUrl
                                        && a.AdLocation == location
                                        && a.Status == StatusTypes.Active
                                        && a.StartDate <= nowDate
                                        && a.EndDate >= nowDate
                                        && c.Status == StatusTypes.Active
                                        && c.LanguageTag == language
                                        && c.CategoryTagType != CategoryTagTypes.Coming
                                        && (a.AdType == AdvertTypes.Image || a.AdType == AdvertTypes.Adsense || a.AdType == AdvertTypes.Embedded)
                                  select new
                                  {
                                      a.AdCode,
                                      a.AdFilePath,
                                      a.AdFileType,
                                      a.AdGuid,
                                      a.AdLink,
                                      a.AdLocation,
                                      a.AdType,
                                      a.SequenceNr
                                  }).ToList();

            var result = categoryAdvert.Select(a => new CategoryAdvertDto
            {
                AdType = a.AdType,
                Code = a.AdCode,
                FilePath = a.AdFilePath,
                FileType = a.AdFileType,
                Guid = a.AdGuid,
                Link = a.AdLink,
                Location = a.AdLocation,
                SequenceNr = a.SequenceNr
            }).ToList();

            return result;
        }
    }
}