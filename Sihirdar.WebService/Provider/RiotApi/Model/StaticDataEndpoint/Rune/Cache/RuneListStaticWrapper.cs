
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Rune.Cache
{
    class RuneListStaticWrapper
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
