using System;
using System.Linq;
using Sihirdar.DataAccessLayer;
using System.Collections.Generic;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services
{
    public class AovSkinService : IAovSkinService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AovSkinService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(AovSkin skin)
        {
            var db = _dbFactory();
            db.AovSkins.Add(skin);
            db.SaveChanges();
        }

        public AovSkin Get(int id)
        {
            var db = _dbFactory();
            var skin = db.AovSkins.FirstOrDefault(a => a.Id == id);
            return skin;
        }

        public void Edit(int id, AovSkin eskin)
        {
            var db = _dbFactory();
            var skin = db.AovSkins.FirstOrDefault(a => a.Id == id);
            if (skin == null)
                return;

            skin.Picture = eskin.Picture;
            skin.Name = eskin.Name;
            skin.Num = eskin.Num;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var skin = db.AovSkins.FirstOrDefault(a => a.Id == id);
            if (skin == null)
                return;

            db.AovSkins.Remove(skin);
            db.SaveChanges();
        }

        public List<AovSkin> GetChampSkins(int[] ids)
        {
            var db = _dbFactory();
            var skin = db.AovSkins.Where(a => ids.Contains(a.Id) && a.Status == StatusTypes.Active).OrderBy(a => a.Name)
                .ToList();
            return skin;
        }

        public List<AovSkin> List()
        {
            var db = _dbFactory();
            var skin = db.AovSkins.OrderBy(a => a.Name).ToList();
            return skin;
        }

        public List<AovSkin> List(StatusTypes status)
        {
            var db = _dbFactory();
            var skin = db.AovSkins.Where(a => a.Status == status).OrderBy(a => a.Name).ToList();
            return skin;
        }
    }
}