using System.Collections.Generic;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace Tools.Service.Interface
{
    public interface IAovChampService
    {
        AovChamp Add(AovChamp champ);
        AovChamp Get(int id);
        void Edit(int id, AovChamp echamp);
        void Delete(int id);
        List<AovChamp> List();
        List<AovChamp> List(StatusTypes status);
    }
}