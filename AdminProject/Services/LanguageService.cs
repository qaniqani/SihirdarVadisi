using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public LanguageService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Language instance)
        {
            var db = _dbFactory();
            db.Languages.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Language newInstance)
        {
            var db = _dbFactory();
            var lang = db.Languages.FirstOrDefault(a => a.Id == id);
            lang.Name = newInstance.Name;
            lang.Status = newInstance.Status;
            lang.UrlTag = newInstance.UrlTag;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.Id == id);
            db.Languages.Remove(lang);
            db.SaveChanges();
        }

        public IList<Language> List()
        {
            var db = _dbFactory();

            var languages = db.Languages.ToList();
            return languages;
        }

        public Language GetItem(int instanceId)
        {
            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.Id == instanceId);
            return lang;
        }

        public Language GetItem(string url)
        {
            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.UrlTag == url);
            return lang;
        }

        public Language GetItem(int id, string url)
        {
            var db = _dbFactory();

            var lang = db.Languages.FirstOrDefault(a => a.Id != id && a.UrlTag == url);
            return lang;
        }

        public IList<Language> ActiveList()
        {
            var db = _dbFactory();

            var languages = db.Languages.Where(a => a.Status == StatusTypes.Active).ToList();
            return languages;
        }

        public SelectList ActiveList(int selectedLanguage)
        {
            var db = _dbFactory();
            var languages = db.Languages.Where(a => a.Status == StatusTypes.Active).ToList();
            return new SelectList(languages, "Id", "Name", selectedLanguage.ToString());
        }
    }
}