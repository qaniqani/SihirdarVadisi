using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public GalleryService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<GalleryDetailItemDto> GetGalleryDetails(int galleryId)
        {
            var db = _dbFactory();
            var pictures =
                db.GalleryDetails.OrderBy(a => a.SequenceNumber)
                    .Where(a => a.GalleryId == galleryId && a.Status == StatusTypes.Active)
                    .ToList()
                    .Select(a => new GalleryDetailItemDto
                    {
                        Description = a.Description,
                        Id = a.Id,
                        Name = a.Name,
                        Picture = a.PictureUrl
                    }).ToList();

            return pictures;
        }

        public GalleryDetailItemDto GetGalleryShowcasePicture(int galleryId)
        {
            var db = _dbFactory();
            var picture =
                db.GalleryDetails.OrderBy(a => a.SequenceNumber)
                    .FirstOrDefault(a => a.GalleryId == galleryId && a.Status == StatusTypes.Active);

            if (picture == null)
                return null;

            var result = new GalleryDetailItemDto
            {
                Description = picture.Description,
                Id = picture.Id,
                Name = picture.Name,
                Picture = picture.PictureUrl
            };

            return result;
        }

        public string UrlCheck(string url)
        {
            var db = _dbFactory();

            var count = db.Galleries.Count(a => a.Url == url);
            if (count > 0)
                url = $"{url}-{count}";

            return url;
        }
    }
}