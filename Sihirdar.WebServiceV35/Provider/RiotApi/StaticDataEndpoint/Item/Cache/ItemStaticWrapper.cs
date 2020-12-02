using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.StaticDataEndpoint.Item.Cache
{
    public class ItemStaticWrapper
    {
        public ItemStatic ItemStatic { get; private set; }
        public Language Language { get; private set; }
        public ItemData ItemData { get; private set; }

        public ItemStaticWrapper(ItemStatic item, Language language, ItemData itemData)
        {
            ItemStatic = item;
            Language = language;
            ItemData = itemData;
        }
    }
}
