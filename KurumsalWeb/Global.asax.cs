using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KurumsalWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private void RouteYapilandir(RouteCollection route)
        {
            //route.MapRoute(
            //    name: "Anasayfa",
            //    url: "Anasayfa",
            //    defaults: new { controller = "_Home", action = "Index", id = UrlParameter.Optional }
            //);

        }
        protected void Application_Start()
        {
            //RouteYapilandir(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
