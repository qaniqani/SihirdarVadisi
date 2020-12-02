using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sihirdar.WebServiceV3.Provider.RiotApi.LeagueEndpoint.Enums.Converters
{
    public class CharArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(char[]).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.ToString().ToCharArray();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
