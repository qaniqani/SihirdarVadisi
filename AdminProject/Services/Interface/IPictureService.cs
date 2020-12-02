using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IPictureService : IBaseInterface<Picture>
    {
        IList<Picture> List(int contentId);
        void Delete(int[] id);
        void DeletePictures(int contentId);
    }
}