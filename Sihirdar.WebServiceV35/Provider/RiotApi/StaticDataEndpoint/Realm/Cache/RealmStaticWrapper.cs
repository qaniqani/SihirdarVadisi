namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Realm.Cache
{
    public class RealmStaticWrapper
    {
        public RealmStatic RealmStatic { get; private set; }

        public RealmStaticWrapper(RealmStatic realm)
        {
            RealmStatic = realm;
        }
    }
}
