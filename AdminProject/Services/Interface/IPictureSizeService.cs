using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Interface
{
    public interface IPictureSizeService : IBaseInterface<PictureSize>
    {
        List<PictureSize> List(StatusTypes status);
        List<PictureSize> List(ContentTypes type, StatusTypes status);

        //Size Detail
        void AddSizeDetail(PictureSizeDetail detail);
        void EditSizeDetail(int id, PictureSizeDetail detail);
        List<PictureSizeDetail> ListSizeDetail(int id);
        List<PictureSizeDetail> ActiveListSizeDetail(int id);
        PictureSizeDetail GetSizeDetail(int id);
        void DeleteSizeDetail(int id);
    }
}