using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TesoreriaVS12
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Selectordesistemas", action = "Index", id = UrlParameter.Optional },
                defaults: new { controller = "Account", action = "LogOn", id = UrlParameter.Optional },
                namespaces: new[] { "TesoreriaVS12.Controllers" }
            );
        }
    }
}