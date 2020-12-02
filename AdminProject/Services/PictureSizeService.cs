using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services
{
    public class PictureSizeService : IPictureSizeService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public PictureSizeService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(PictureSize instance)
        {
            var db = _dbFactory();
            db.PictureSizes.Add(instance);
            db.SaveChanges();
        }

        public void Edit(int id, PictureSize newInstance)
        {
            var db = _dbFactory();
            var size = db.PictureSizes.FirstOrDefault(a => a.Id == id);
            if (size == null)
                return;

            size.Name = newInstance.Name;
            size.PictureType = newInstance.PictureType;
            size.Status = newInstance.Status;
            db.SaveChanges();
        }

        public PictureSize GetItem(int instanceId)
        {
            var db = _dbFactory();
            var size = db.PictureSizes.FirstOrDefault(a => a.Id == instanceId);
            return size;
        }

        public IList<PictureSize> List()
        {
            var db = _dbFactory();
            var sizes = db.PictureSizes.OrderBy(a => a.Name).ToList();
            return sizes;
        }

        public List<PictureSize> List(StatusTypes status)
        {
            var db = _dbFactory();
            var sizes = db.PictureSizes.Where(a => a.Status == status).OrderBy(a => a.Name).ToList();
            return sizes;
        }

        public List<PictureSize> List(ContentTypes type, StatusTypes status)
        {
            var db = _dbFactory();
            var sizes =
                db.PictureSizes.Where(a => a.Status == status && a.PictureType == type).OrderByDescending(a => a.Id).ToList();
            return sizes;
        }

        public void AddSizeDetail(PictureSizeDetail detail)
        {
            var db = _dbFactory();
            db.PictureSizeDetails.Add(detail);
            db.SaveChanges();
        }

        public void EditSizeDetail(int id, PictureSizeDetail detail)
        {
            var db = _dbFactory();
            var size = db.PictureSizeDetails.FirstOrDefault(a => a.Id == id);
            if (size == null)
                return;

            size.Width = detail.Width;
            size.Height = detail.Height;
            
            db.SaveChanges();
        }

        public List<PictureSizeDetail> ListSizeDetail(int id)
        {
            var db = _dbFactory();
            var list = db.PictureSizeDetails.Where(a => a.SizeId == id).ToList();
            return list;
        }

        public List<PictureSizeDetail> ActiveListSizeDetail(int id)
        {
            var db = _dbFactory();
            var list = db.PictureSizeDetails.Where(a => a.SizeId == id && a.Status == StatusTypes.Active).OrderBy(a => a.Width).ToList();
            return list;
        }

        public PictureSizeDetail GetSizeDetail(int id)
        {
            var db = _dbFactory();
            var size = db.PictureSizeDetails.FirstOrDefault(a => a.Id == id);
            return size;
        }

        public void DeleteSizeDetail(int id)
        {
            var db = _dbFactory();
            var size = db.PictureSizeDetails.FirstOrDefault(a => a.Id == id);
            db.PictureSizeDetails.Remove(size);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();

            var size = db.PictureSizes.FirstOrDefault(a => a.Id == id);
            if (size == null)
                return;

            db.PictureSizes.Remove(size);
            db.SaveChanges();
        }
    }
}