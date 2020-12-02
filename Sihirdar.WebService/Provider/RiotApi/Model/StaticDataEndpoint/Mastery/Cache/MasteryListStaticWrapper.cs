
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Mastery.Cache
{
    class MasteryListStaticWrapper
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
