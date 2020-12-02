using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services
{
    public class AovChampService : IAovChampService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public AovChampService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public AovChamp Add(AovChamp champ)
        {
            var db = _dbFactory();
            db.AovChamps.Add(champ);
            db.SaveChanges();
            return champ;
        }

        public AovChamp Get(int id)
        {
            var db = _dbFactory();
            var champ = db.AovChamps.FirstOrDefault(a => a.Id == id);

            return champ;
        }

        public void Edit(int id, AovChamp echamp)
        {
            var db = _dbFactory();
            var champ = db.AovChamps.FirstOrDefault(a => a.Id == id);
            if (champ == null)
                return;

            champ.Ad = echamp.Ad;
            champ.Adperlevel = echamp.Adperlevel;
            champ.Ap = echamp.Ap;
            champ.Apperlevel = echamp.Apperlevel;
            champ.Armor = echamp.Armor;
            champ.ArmorPerLevel = echamp.ArmorPerLevel;
            champ.As = echamp.As;
            champ.AsPerLevel = echamp.AsPerLevel;
            champ.Cd = echamp.Cd;
            champ.CdPerLevel = echamp.CdPerLevel;
            champ.Critic = echamp.Critic;
            champ.CriticPerLevel = echamp.CriticPerLevel;
            champ.Gold = echamp.Gold;
            champ.Hp = echamp.Hp;
            champ.Hpperlevel = echamp.Hpperlevel;
            champ.Hpregen = echamp.Hpregen;
            champ.Hpregenperlevel = echamp.Hpregenperlevel;
            champ.Image = echamp.Image;
            champ.InfoAttack = echamp.InfoAttack;
            champ.InfoConst = echamp.InfoConst;
            champ.InfoDifficulty = echamp.InfoDifficulty;
            champ.InfoMagic = echamp.InfoMagic;
            champ.Key = echamp.Key;
            champ.Lore = echamp.Lore;
            champ.Manaregen = echamp.Manaregen;
            champ.Manaregenperlevel = echamp.Manaregenperlevel;
            champ.Movement = echamp.Movement;
            champ.MovementPerLevel = echamp.MovementPerLevel;
            champ.Mr = echamp.Mr;
            champ.MrPerLevel = echamp.MrPerLevel;
            champ.Name = echamp.Name;
            champ.Partype = echamp.Partype;
            champ.Quote = echamp.Quote;
            champ.Role = echamp.Role;
            champ.Status = echamp.Status;
            champ.Ticket = echamp.Ticket;
            champ.Title = echamp.Title;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var champ = db.AovChamps.FirstOrDefault(a => a.Id == id);
            if (champ == null)
                return;

            db.AovChamps.Remove(champ);
            db.SaveChanges();
        }

        public List<AovChamp> List(StatusTypes status)
        {
            var db = _dbFactory();
            var champs = db.AovChamps.Where(a => a.Status == status).OrderBy(a => a.Name).ToList();
            return champs;
        }

        public List<AovChamp> List()
        {
            var db = _dbFactory();
            var champs = db.AovChamps.OrderBy(a => a.Name).ToList();
            return champs;
        }
    }
}