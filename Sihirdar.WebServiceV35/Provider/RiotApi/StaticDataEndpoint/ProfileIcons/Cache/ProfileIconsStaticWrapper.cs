using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.ProfileIcons.Cache
{
    public class ProfileIconsStaticWrapper
    {
        public ProfileIconListStatic ProfileIconListStatic { get; private set; }
        public Language Language { get; private set; }

        public ProfileIconsStaticWrapper(ProfileIconListStatic profileIconListStatic, Language language)
        {
            ProfileIconListStatic = profileIconListStatic;
            Language = language;
        }
    }
}
