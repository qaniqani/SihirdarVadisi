using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace Tools
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
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

            var defaultCulture = CultureInfo.GetCultureInfo("tr");

            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;
        }
    }
}