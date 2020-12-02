using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services
{
    public class SliderService : ISliderService
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public SliderService(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public void Add(Slider instance)
        {
            var db = _dbFactory();
            db.Sliders.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Slider newInstance)
        {
            var db = _dbFactory();
            var slider = db.Sliders.FirstOrDefault(a => a.Id == id);
            if (slider == null)
                return;

            slider.CreateDate = newInstance.CreateDate;
            slider.Detail1 = newInstance.Detail1;
            slider.Detail2 = newInstance.Detail2;
            slider.Detail3 = newInstance.Detail3;
            slider.IsVideoLink = newInstance.IsVideoLink;
            slider.LanguageId = newInstance.LanguageId;
            slider.Name = newInstance.Name;
            slider.Picture1 = newInstance.Picture1;
            slider.Picture10 = newInstance.Picture10;
            slider.Picture2 = newInstance.Picture2;
            slider.Picture3 = newInstance.Picture3;
            slider.Picture4 = newInstance.Picture4;
            slider.Picture5 = newInstance.Picture5;
            slider.Picture6 = newInstance.Picture6;
            slider.Picture7 = newInstance.Picture7;
            slider.Picture8 = newInstance.Picture8;
            slider.Picture9 = newInstance.Picture9;
            slider.PictureType = newInstance.PictureType;
            slider.SequenceNumber = newInstance.SequenceNumber;
            slider.Status = newInstance.Status;
            slider.VideoEmbedCode = newInstance.VideoEmbedCode;
            slider.VideoUrl = newInstance.VideoUrl;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var slider = db.Sliders.FirstOrDefault(a => a.Id == id);
            if (slider == null)
                return;

            db.Sliders.Remove(slider);
            db.SaveChanges();
        }

        public IList<Slider> List()
        {
            var db = _dbFactory();
            var languageId = _setting.LanguageId;
            var sliders = db.Sliders.OrderBy(a => a.SequenceNumber).Where(a => a.LanguageId == languageId).ToList();
            return sliders;
        }

        public IList<Slider> ActiveList()
        {
            var db = _dbFactory();
            var languageId = _setting.LanguageId;
            var sliders = db.Sliders.OrderBy(a => a.SequenceNumber).Where(a => a.LanguageId == languageId && a.Status == StatusTypes.Active).ToList();
            return sliders;
        }

        public IList<Slider> ActiveTop4List()
        {
            var db = _dbFactory();
            var languageId = _setting.LanguageId;
            var sliders = db.Sliders.OrderBy(a => a.SequenceNumber).Where(a => a.LanguageId == languageId && a.Status == StatusTypes.Active).Take(4).ToList();
            return sliders;
        }

        public IList<Slider> SliderOrder()
        {
            var db = _dbFactory();
            var languageId = _setting.LanguageId;
            var sliders = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink == VideoTypes.IsNotVideo).OrderByDescending(a => a.Id).Take(30).OrderBy(a => a.SequenceNumber).ToList();
            return sliders;
        }

        public void SliderOrder(string[] order)
        {
            var db = _dbFactory();

            var list = order.Select(a =>
            {
                var id = Convert.ToInt32(a.Split('|')[0]);
                var sequenceNumber = Convert.ToInt32(a.Split('|')[1]);

                return new { Id = id, Number = sequenceNumber };
            });

            var languageId = _setting.LanguageId;
            var slider = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink == VideoTypes.IsNotVideo).ToList();

            slider.ForEach(g =>
            {
                var item = list.FirstOrDefault(a => a.Id == g.Id);
                if (item != null)
                    g.SequenceNumber = item.Number;
            });
            db.SaveChanges();
        }

        public IList<Slider> VideoOrder()
        {
            var db = _dbFactory();
            var languageId = _setting.LanguageId;
            var sliders = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink != VideoTypes.IsNotVideo).OrderBy(a => a.SequenceNumber).ToList();
            return sliders;
        }

        public void VideoOrder(string[] order)
        {
            var db = _dbFactory();

            var list = order.Select(a =>
            {
                var id = Convert.ToInt32(a.Split('|')[0]);
                var sequenceNumber = Convert.ToInt32(a.Split('|')[1]);

                return new { Id = id, Number = sequenceNumber };
            });

            var languageId = _setting.LanguageId;
            var slider = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink != VideoTypes.IsNotVideo).ToList();

            slider.ForEach(g =>
            {
                var item = list.FirstOrDefault(a => a.Id == g.Id);
                if (item != null)
                    g.SequenceNumber = item.Number;
            });
            db.SaveChanges();
        }

        public Slider GetItem(int instanceId)
        {
            var db = _dbFactory();
            var slider = db.Sliders.FirstOrDefault(a => a.Id == instanceId);
            return slider;
        }

        public List<SliderViewModel> SecondSlider()
        {
            var db = _dbFactory();
            var contents = (from content in db.Contents
                        join category in db.Categories
                        on content.CategoryId equals category.Id
                        where content.Status == StatusTypes.Active
                        && category.Status == StatusTypes.Active
                        && content.GameType == GameTypes.LOL
                        && category.CategoryType == CategoryTypes.Story
                        orderby content.Id descending
                        select new
                        {
                            content.Description,
                            content.GameType,
                            content.Id,
                            content.Url,
                            content.Name,
                            CategoryUrl = category.Url
                        })
                        .Take(4)
                        .ToList();

            var result = contents.Select(a => new SliderViewModel
            {
                Description = a.Description,
                GameType = a.GameType,
                GameTypeText = Tool.GetGameTypeText[a.GameType],
                Id = a.Id,
                Picture = a.Url,
                Subject = a.Name,
                Url = $"{a.CategoryUrl}/{a.Url}",
            }).ToList();

            return result;
        }
    }
}