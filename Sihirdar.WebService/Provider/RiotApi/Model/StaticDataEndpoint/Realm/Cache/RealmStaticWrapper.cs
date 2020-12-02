namespace Sihirdar.WebService.Provider.RiotApi.Model.StaticDataEndpoint.Realm.Cache
{
    class RealmStaticWrapper
    {
        public RealmStatic RealmStatic { get; private set; }

        public RealmStaticWrapper(RealmStatic realm)
        {
            RealmStatic = realm;
        }
    }
}
