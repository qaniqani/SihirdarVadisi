using System.Collections.Generic;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace Tools.Service.Interface
{
    public interface IAovSkinService
    {
        void Add(AovSkin skin);
        AovSkin Get(int id);
        void Delete(int id);
        List<AovSkin> List();
        void Edit(int id, AovSkin eskin);
        List<AovSkin> List(StatusTypes status);
        List<AovSkin> GetChampSkins(int[] ids);
    }
}