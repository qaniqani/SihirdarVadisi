using System;
using System.Collections.Generic;
using System.Linq;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Tools.Service.Interface;

namespace Tools.Service
{
    public class AovChampSkinAssgnService : IAovChampSkinAssgnService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AovChampSkinAssgnService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(AovChampSkinAssng skin)
        {
            var db = _dbFactory();
            db.AovChampSkinAssngs.Add(skin);
            db.SaveChanges();
        }

        public void Add(List<AovChampSkinAssng> skin)
        {
            var db = _dbFactory();
            db.AovChampSkinAssngs.AddRange(skin);
            db.SaveChanges();
        }

        public AovChampSkinAssng Get(int id)
        {
            var db = _dbFactory();
            var skin = db.AovChampSkinAssngs.FirstOrDefault(a => a.Id == id);
            return skin;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var skin = db.AovChampSkinAssngs.FirstOrDefault(a => a.Id == id);
            if (skin == null)
                return;

            db.AovChampSkinAssngs.Remove(skin);
            db.SaveChanges();
        }

        public void AllDeleteChampSkin(int champId)
        {
            var db = _dbFactory();
            var skin = db.AovChampSkinAssngs.Where(a => a.ChampId == champId);
            db.AovChampSkinAssngs.RemoveRange(skin);
            db.SaveChanges();
        }

        public List<AovChampSkinAssng> List(int champId)
        {
            var db = _dbFactory();
            var skins = db.AovChampSkinAssngs.Where(a => a.ChampId == champId).ToList();
            return skins;
        }
    }
}