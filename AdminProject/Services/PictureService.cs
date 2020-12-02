using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Helpers;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer.Infrastructure;

namespace AdminProject.Services
{
    public class PictureService : IPictureService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public PictureService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Picture instance)
        {
            var db = _dbFactory();
            db.Pictures.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, Picture newInstance)
        {
            var db = _dbFactory();
            var picture = GetItem(id);
            if (picture == null)
                return;

            picture.ContentId = newInstance.ContentId;
            picture.Height = newInstance.Height;
            picture.PicturePath = newInstance.PicturePath;
            picture.PictureType = newInstance.PictureType;
            picture.SizeId = newInstance.SizeId;
            picture.Width = newInstance.Width;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var picture = GetItem(id);
            db.Pictures.Remove(picture);
            db.SaveChanges();
        }

        public void Delete(int[] id)
        {
            var db = _dbFactory();
            var pictures = db.Pictures.Where(a => id.Contains(a.Id)).ToList();
            db.Pictures.RemoveRange(pictures);
            db.SaveChanges();
        }

        public IList<Picture> List()
        {
            var db = _dbFactory();
            var pictures = db.Pictures.OrderByDescending(a => a.Id).ToList();
            return pictures;
        }

        public IList<Picture> List(int contentId)
        {
            var db = _dbFactory();
            var pictures = db.Pictures.Where(a => a.ContentId == contentId).OrderByDescending(a => a.Id).ToList();
            return pictures;
        }

        public void DeletePictures(int contentId)
        {
            var db = _dbFactory();
            var pictures = db.Pictures.Where(a => a.ContentId == contentId).OrderByDescending(a => a.Id).ToList();
            pictures.ForEach(item =>
            {
                Utility.DeleteFile($"~/Content/{item.PicturePath}");
            });
        }

        public Picture GetItem(int instanceId)
        {
            var db = _dbFactory();
            var picture = db.Pictures.FirstOrDefault(a => a.Id == instanceId);
            return picture;
        }
    }
}