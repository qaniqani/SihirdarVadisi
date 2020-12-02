using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Web.Optimization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Web.Routing;
using AdminProject.App_Start;

namespace AdminProject
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //Install-Package Ninject.Web.WebApi 
            //install-package Ninject.Web.WebApi.WebHost
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();

                //accept-header bilgisini alir
                config.MessageHandlers.Add(new LanguageHandler());

                var jsonFormatter = new JsonMediaTypeFormatter();

                var jsonSerializerSettings = jsonFormatter.SerializerSettings;
                jsonSerializerSettings.Formatting = Formatting.Indented;
                jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                config.Formatters.Clear();
                config.Formatters.Add(jsonFormatter);
            });

            AreaRegistration.RegisterAllAreas();

            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //var cookie = HttpContext.Current.Request.Cookies["language"];
            //if (cookie?.Value != null)
            //{
            //    var selectedCulture = CultureInfo.GetCultureInfo(cookie.Value);
            //    Thread.CurrentThread.CurrentCulture = selectedCulture;
            //    Thread.CurrentThread.CurrentUICulture = selectedCulture;
            //    return;
            //}

            var currentUrl = HttpContext.Current.Request.RawUrl;
            foreach (var t in GetRedirectUrls)
            {
                if (t.Key != currentUrl)
                    continue;

                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", t.Value);
                Response.End();
            }

            var defaultCulture = CultureInfo.GetCultureInfo("tr");

            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;
        }

        public static readonly Dictionary<string, string> GetRedirectUrls = new Dictionary<string, string>
        {
            {"/haber/strike-of-kings-haberleri","/haber/arena-of-valor-haberleri"}
        };
    }
}
