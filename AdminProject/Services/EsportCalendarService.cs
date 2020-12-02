using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Services
{
    public class EsportCalendarService : IEsportCalendarService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public EsportCalendarService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(EsportCalendar instance)
        {
            var db = _dbFactory();
            db.EsportCalendars.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, EsportCalendar newInstance)
        {
            var db = _dbFactory();
            var esport = db.EsportCalendars.FirstOrDefault(a => a.Id == id);
            if (esport == null)
                return;

            esport.Color = newInstance.Color;
            esport.Description = newInstance.Description;
            esport.Language = newInstance.Language;
            esport.LanguageId = newInstance.LanguageId;
            esport.Name = newInstance.Name;
            esport.Picture = newInstance.Picture;
            esport.StartDateTime = newInstance.StartDateTime;
            esport.Status = newInstance.Status;
            esport.UpdatedDate = DateTime.Now;
            esport.UpdatedEditorId = newInstance.UpdatedEditorId;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var esport = GetItem(id);
            if (esport == null)
                return;

            var db = _dbFactory();
            db.EsportCalendars.Remove(esport);
            db.SaveChanges();
        }

        public IList<EsportCalendar> List()
        {
            var db = _dbFactory();
            var esports = db.EsportCalendars.OrderByDescending(a => a.StartDateTime).ToList();
            return esports;
        }

        public IList<EsportCalendar> List(StatusTypes status)
        {
            var db = _dbFactory();
            var esports = db.EsportCalendars.OrderByDescending(a => a.StartDateTime).Where(a => a.Status == status).ToList();
            return esports;
        }

        public IList<EsportCalendarDto> List(DateTime startDate)
        {
            var db = _dbFactory();
            var esports = db.EsportCalendars.OrderByDescending(a => a.StartDateTime).Where(a => a.Status == StatusTypes.Active && a.StartDateTime >= startDate).ToList();
            var result = esports.Select(a => new EsportCalendarDto
            {
                color = a.Color,
                date = a.StartDateTime.Day.ToString(),
                description = a.Description,
                image = $"/Content/{a.Picture}.jpg",
                month = a.StartDateTime.Month.ToString(),
                name = a.Name,
                start_time = a.StartDateTime.ToString("HH:mm"),
                year = a.StartDateTime.Year.ToString()
            }).ToList();
            return result;
        }

        public EsportCalendar GetItem(int instanceId)
        {
            var db = _dbFactory();
            var esportDate = db.EsportCalendars.FirstOrDefault(a => a.Id == instanceId);
            return esportDate;
        }
    }
}