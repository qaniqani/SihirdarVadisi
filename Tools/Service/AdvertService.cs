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
    public class AdvertService : IAdvertService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AdvertService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

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
    }
}