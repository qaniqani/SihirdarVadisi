using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace Tools.Service.Interface
{
    public interface IAovChampSpellAssgnService
    {
        void Add(AovChampSpellAssng spell);
        void Add(List<AovChampSpellAssng> spell);
        AovChampSpellAssng Get(int id);
        List<AovChampSpellAssng> GetChampSpell(int champId);
        void Delete(int id);
        void AllDeleteChampSpell(int champId);
        List<AovChampSpellAssng> List(int champId);
    }
}