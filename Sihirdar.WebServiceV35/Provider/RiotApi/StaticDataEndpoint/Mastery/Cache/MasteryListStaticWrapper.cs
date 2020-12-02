using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Mastery.Cache
{
    public class MasteryListStaticWrapper
    {
        public MasteryListStatic MasteryListStatic { get; private set; }
        public Language Language { get; private set; }
        public MasteryData MasteryData { get; private set; }

        public MasteryListStaticWrapper(MasteryListStatic masteries, Language language, MasteryData masteryData)
        {
            MasteryListStatic = masteries;
            Language = language;
            MasteryData = masteryData;
        }
    }
}
