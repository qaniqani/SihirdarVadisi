
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.SummonerSpell.Cache
{
    class SummonerSpellListStaticWrapper
    {
        public SummonerSpellListStatic SummonerSpellListStatic { get; private set; }
        public Language Language { get; private set; }
        public SummonerSpellData SummonerSpellData { get; private set; }

        public SummonerSpellListStaticWrapper(SummonerSpellListStatic spells, Language language
            , SummonerSpellData summonerSpellData)
        {
            SummonerSpellListStatic = spells;
            Language = language;
            SummonerSpellData = summonerSpellData;
        }
    }
}
