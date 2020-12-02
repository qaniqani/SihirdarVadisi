using System.Collections.Generic;
using Sihirdar.DataAccessLayer;
using Tools.Service.Model;

namespace Tools.Service.Interface
{
    public interface IAdvertService
    {
        List<CategoryAdvertDto> GetCategoryAdverts(string categoryUrl, string language);
        List<CategoryAdvertDto> GetCategoryAdverts(string language, AdvertLocationTypes location);
    }
}