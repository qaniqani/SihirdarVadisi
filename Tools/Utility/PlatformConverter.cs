using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sihirdar.WebServiceV3.Provider.RiotApi.Misc;

namespace Tools.Utility
{
    public class PlatformConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(string).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Value<string>() == null) return null;
            var str = token.Value<string>();
            switch (str)
            {
                case "NA1":
                    return Platform.NA1;
                case "BR1":
                    return Platform.BR1;
                case "LA1":
                    return Platform.LA1;
                case "LA2":
                    return Platform.LA2;
                case "OC1":
                    return Platform.OC1;
                case "EUN1":
                    return Platform.EUN1;
                case "TR1":
                    return Platform.TR1;
                case "RU":
                    return Platform.RU;
                case "EUW1":
                    return Platform.EUW1;
                case "KR":
                    return Platform.KR;
                default:
                    return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((Platform)value).ToString().ToUpper());
        }
    }

    public static class RegionToPlatformConverter
    {
        public static Platform ConvertToPlatform(this Region region)
        {
            switch (region)
            {
                case Region.na:
                    return Platform.NA1;
                case Region.br:
                    return Platform.BR1;
                case Region.lan:
                    return Platform.LA1;
                case Region.las:
                    return Platform.LA2;
                case Region.oce:
                    return Platform.OC1;
                case Region.eune:
                    return Platform.EUN1;
                case Region.tr:
                    return Platform.TR1;
                case Region.ru:
                    return Platform.RU;
                case Region.euw:
                    return Platform.EUW1;
                case Region.kr:
                    return Platform.KR;
                default:
                    return Platform.NA1;
            }
        }
    }

    public static class PlatformToRegionConverter
    {
        public static Region ConvertToRegion(this Platform platform)
        {
            switch (platform)
            {
                case Platform.NA1:
                    return Region.na;
                case Platform.BR1:
                    return Region.br;
                case Platform.LA1:
                    return Region.lan;
                case Platform.LA2:
                    return Region.las;
                case Platform.OC1:
                    return Region.oce;
                case Platform.EUN1:
                    return Region.eune;
                case Platform.TR1:
                    return Region.tr;
                case Platform.RU:
                    return Region.ru;
                case Platform.EUW1:
                    return Region.euw;
                case Platform.KR:
                    return Region.kr;
                default:
                    return Region.na;
            }
        }

    }
}