using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Mastery.Cache
{
    public class MasteryStaticWrapper
    {
        public MasteryStatic MasteryStatic { get; private set; }
        public Language Language { get; private set; }
        public MasteryData MasteryData { get; private set; }

        public MasteryStaticWrapper(MasteryStatic mastery, Language language, MasteryData masteryData)
        {
            MasteryStatic = mastery;
            Language = language;
            MasteryData = masteryData;
        }
    }
}
