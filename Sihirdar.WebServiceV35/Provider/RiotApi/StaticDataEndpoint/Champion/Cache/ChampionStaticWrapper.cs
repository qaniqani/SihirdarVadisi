using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Champion.Cache
{
    public class ChampionStaticWrapper
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
