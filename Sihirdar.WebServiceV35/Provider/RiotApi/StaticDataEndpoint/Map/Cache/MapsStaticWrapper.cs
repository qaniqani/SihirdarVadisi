using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Map.Cache
{
    public class MapsStaticWrapper
    {
        public MapsStatic MapsStatic { get; private set; }
        public Language Language { get; private set; }
        public string Version { get; private set; }

        public MapsStaticWrapper(MapsStatic mapsStatic, Language language, string version)
        {
            MapsStatic = mapsStatic;
            Language = language;
            Version = version;
        }
    }
}
