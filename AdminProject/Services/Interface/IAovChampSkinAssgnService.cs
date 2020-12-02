using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IAovChampSkinAssgnService
    {
        void Add(AovChampSkinAssng skin);
        void Add(List<AovChampSkinAssng> skin);
        AovChampSkinAssng Get(int id);
        void Delete(int id);
        void AllDeleteChampSkin(int champId);
        List<AovChampSkinAssng> List(int champId);
    }
}