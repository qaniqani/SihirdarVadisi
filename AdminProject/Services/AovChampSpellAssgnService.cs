using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services
{
    public class AovChampSpellAssgnService : IAovChampSpellAssgnService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AovChampSpellAssgnService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(AovChampSpellAssng spell)
        {
            var db = _dbFactory();
            db.AovChampSpellAssng.Add(spell);
            db.SaveChanges();
        }

        public void Add(List<AovChampSpellAssng> spell)
        {
            var db = _dbFactory();
            db.AovChampSpellAssng.AddRange(spell);
            db.SaveChanges();
        }

        public AovChampSpellAssng Get(int id)
        {
            var db = _dbFactory();
            var spell = db.AovChampSpellAssng.FirstOrDefault(a => a.Id == id);
            return spell;
        }

        public List<AovChampSpellAssng> GetChampSpell(int champId)
        {
            var db = _dbFactory();
            var spell = db.AovChampSpellAssng.Where(a => a.ChampId == champId).ToList();
            return spell;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var spell = db.AovChampSpellAssng.FirstOrDefault(a => a.Id == id);
            if (spell == null)
                return;

            db.AovChampSpellAssng.Remove(spell);
            db.SaveChanges();
        }

        public void AllDeleteChampSpell(int champId)
        {
            var db = _dbFactory();
            var spell = db.AovChampSpellAssng.Where(a => a.ChampId == champId);

            db.AovChampSpellAssng.RemoveRange(spell);
            db.SaveChanges();
        }

        public List<AovChampSpellAssng> List(int champId)
        {
            var db = _dbFactory();
            var list = db.AovChampSpellAssng.Where(a => a.ChampId == champId).ToList();
            return list;
        }
    }
}