using System;
using System.Linq;
using Tools.Service.Interface;
using Sihirdar.DataAccessLayer;
using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace Tools.Service
{
    public class AovSpellService : IAovSpellService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AovSpellService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(AovSpell spell)
        {
            var db = _dbFactory();
            db.AovSpells.Add(spell);
            db.SaveChanges();
        }

        public AovSpell Get(int id)
        {
            var db = _dbFactory();
            var spell = db.AovSpells.FirstOrDefault(a => a.Id == id);
            return spell;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var spell = db.AovSpells.FirstOrDefault(a => a.Id == id);
            if (spell == null)
                return;

            db.AovSpells.Remove(spell);
            db.SaveChanges();
        }

        public void Edit(int id, AovSpell espell)
        {
            var db = _dbFactory();
            var spell = db.AovSpells.FirstOrDefault(a => a.Id == id);
            if (spell == null)
                return;

            spell.Description = espell.Description;
            spell.Image = espell.Image;
            spell.Name = espell.Name;
            spell.Num = espell.Num;
            spell.Status = espell.Status;

            db.SaveChanges();
        }

        public List<AovSpell> GetChampSpells(int[] ids)
        {
            var db = _dbFactory();
            var spells = db.AovSpells.Where(a => ids.Contains(a.Id) && a.Status == StatusTypes.Active).OrderBy(a => a.Name)
                .ToList();
            return spells;
        }

        public List<AovSpell> List()
        {
            var db = _dbFactory();
            var list = db.AovSpells.OrderBy(a => a.Name).ToList();
            return list;
        }

        public List<AovSpell> List(StatusTypes status)
        {
            var db = _dbFactory();
            var list = db.AovSpells.Where(a => a.Status == status).OrderBy(a => a.Name).ToList();
            return list;
        }
    }
}