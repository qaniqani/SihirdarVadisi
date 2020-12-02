
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Rune.Cache
{
    class RuneStaticWrapper
    {
        public RuneStatic RuneStatic { get; private set; }
        public Language Language { get; private set; }
        public RuneData RuneData { get; private set; }

        public RuneStaticWrapper(RuneStatic rune, Language language, RuneData runeData)
        {
            RuneStatic = rune;
            Language = language;
            RuneData = runeData;
        }
    }
}
