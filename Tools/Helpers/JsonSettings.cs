using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Tools.Helpers
{
    public class JsonSettings
    {
        public static readonly Lazy<JsonSerializerSettings> SerializerSettings =
            new Lazy<JsonSerializerSettings>(CreateSerializer);

        private static JsonSerializerSettings CreateSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return settings;
        }

        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, SerializerSettings.Value);
        }

        public static T FromJson<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject, SerializerSettings.Value);
        }
    }
}