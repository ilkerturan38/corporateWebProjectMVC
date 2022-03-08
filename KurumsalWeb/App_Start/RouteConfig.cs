using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KurumsalWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            // Otomatik olarak Her sayfa için SeoURL tanımlamasının Yapımı ;

            routes.MapMvcAttributeRoutes(); // Controller içerisinde oluşturulan,ActionResult üzerinde SeoURL işlemleri yapılabilicek


            // Manuel olarak Her sayfa için ayrı ayrı SeoURL tanımlamasının Yapımı ;

            routes.MapRoute(
               name: "test", // RouteConfig Sayfasında Aynı name isimleri tanımlanmamalı.
               url: "denemeRouteSayfam/{id}", /* sayfayı hangi URL ismi çağırılsın -- localhost:44350/denemeRouteSayfam */
               defaults: new { controller = "_Home", action = "denemeRoute", id = UrlParameter.Optional } // id değeri tanımlanabilir yada tanımlanma'yadabilir ; sayfa sadece isimle çalışabilir.
               );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "_Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
