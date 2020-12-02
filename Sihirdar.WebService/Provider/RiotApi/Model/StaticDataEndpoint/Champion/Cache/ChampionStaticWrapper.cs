
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Champion.Cache
{
    class ChampionStaticWrapper
    {
        public ChampionStatic ChampionStatic { get; private set; }
        public Language Language { get; private set; }
        public ChampionData ChampionData { get; private set; }

        public ChampionStaticWrapper(ChampionStatic champion, Language language, ChampionData championData)
        {
            ChampionStatic = champion;
            Language = language;
            ChampionData = championData;
        }
    }
}
