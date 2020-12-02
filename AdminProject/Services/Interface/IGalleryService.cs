using System.Collections.Generic;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface IGalleryService
    {
        string UrlCheck(string url);
        List<GalleryDetailItemDto> GetGalleryDetails(int galleryId);
        GalleryDetailItemDto GetGalleryShowcasePicture(int galleryId);
    }
}