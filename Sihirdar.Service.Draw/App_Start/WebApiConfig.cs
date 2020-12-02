using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Sihirdar.Service.Draw
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            //Json formatter
            var jsonFormatter = new JsonMediaTypeFormatter();

            var jsonSerializerSettings = jsonFormatter.SerializerSettings;
            jsonSerializerSettings.Formatting = Formatting.Indented;
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);
        }
    }
}
