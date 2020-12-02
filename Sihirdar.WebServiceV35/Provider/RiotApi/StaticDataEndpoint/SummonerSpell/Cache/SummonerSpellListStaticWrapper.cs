using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.SummonerSpell.Cache
{
    public class SummonerSpellListStaticWrapper
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
