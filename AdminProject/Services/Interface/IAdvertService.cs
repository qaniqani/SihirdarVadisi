using System.Collections.Generic;
using AdminProject.Models;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public interface IAdvertService : IBaseInterface<Advert>
    {
        //front
        Advert GetAdvert(string guid);
        List<CategoryAdvertDto> GetCategoryAdverts(string categoryUrl, string language);
        List<CategoryAdvertDto> GetCategoryAdverts(string language, AdvertLocationTypes location);
        List<CategoryAdvertDto> GetLocationAdverts(string categoryUrl, string language, AdvertLocationTypes location);
    }
}