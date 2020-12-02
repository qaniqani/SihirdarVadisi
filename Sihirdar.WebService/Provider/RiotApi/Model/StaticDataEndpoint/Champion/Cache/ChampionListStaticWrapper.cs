
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Champion.Cache
{
    class ChampionListStaticWrapper
    {
        public ChampionListStatic ChampionListStatic { get; private set; }
        public Language Language { get; private set; }
        public ChampionData ChampionData { get; private set; }

        public ChampionListStaticWrapper(ChampionListStatic champions, Language language, ChampionData championData)
        {
            ChampionListStatic = champions;
            Language = language;
            ChampionData = championData;
        }
    }
}
