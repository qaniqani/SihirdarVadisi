using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Rune.Cache
{
    public class RuneListStaticWrapper
    {
        public RuneListStatic RuneListStatic { get; private set; }
        public Language Language { get; private set; }
        public RuneData RuneData { get; private set; }

        public RuneListStaticWrapper(RuneListStatic runes, Language language, RuneData runeData)
        {
            RuneListStatic = runes;
            Language = language;
            RuneData = runeData;
        }
    }
}
