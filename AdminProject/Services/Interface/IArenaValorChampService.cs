using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using System.Collections.Generic;

namespace AdminProject.Services.Interface
{
    public interface IArenaValorChampService
    {
        void Add(ArenaValorChamp item);
        void Edit(int id, ArenaValorChamp item);
        ArenaValorChamp GetItem(int id);
        void Delete(int id);
        List<ArenaValorChamp> List();
        ArenaValorChamp GetItem(string url);
        List<ArenaValorChampDto> GetChamps();
    }
}