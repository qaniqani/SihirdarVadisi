using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminProject.Services
{
    public class ArenaValorChampService : IArenaValorChampService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public ArenaValorChampService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(ArenaValorChamp item)
        {
            var db = _dbFactory();
            db.ArenaValorChamps.Add(item);
            db.SaveChanges();
        }

        public void Edit(int id, ArenaValorChamp item)
        {
            var db = _dbFactory();
            var arena = db.ArenaValorChamps.FirstOrDefault(a => a.Id == item.Id);
            arena.Detail = item.Detail;
            arena.Name = item.Name;
            arena.Url = item.Url;
            arena.Status = item.Status;

            db.SaveChanges();
        }

        public ArenaValorChamp GetItem(int id)
        {
            var db = _dbFactory();
            var arena = db.ArenaValorChamps.FirstOrDefault(a => a.Id == id);
            return arena;
        }

        public ArenaValorChamp GetItem(string url)
        {
            var db = _dbFactory();
            var arena = db.ArenaValorChamps.FirstOrDefault(a => a.Url == url && a.Status == StatusTypes.Active);
            return arena;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var arena = db.ArenaValorChamps.FirstOrDefault(a => a.Id == id);
            db.ArenaValorChamps.Remove(arena);
            db.SaveChanges();
        }

        public List<ArenaValorChamp> List()
        {
            var db = _dbFactory();
            var list = db.ArenaValorChamps.OrderBy(a => a.Name).ToList();
            return list;
        }

        public List<ArenaValorChampDto> GetChamps()
        {
            var db = _dbFactory();
            var list = db.ArenaValorChamps.OrderBy(a => a.Name).Where(a => a.Status == StatusTypes.Active).Select(a => new ArenaValorChampDto {
                Id = a.Id,
                Name = a.Name,
                Picture = a.Picture,
                Url = a.Url
            }).ToList();
            return list;
        }
    }
}