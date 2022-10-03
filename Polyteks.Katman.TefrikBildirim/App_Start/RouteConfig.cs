using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Polyteks.Katman.Has
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CalismaSekli",
                url: "{controller}/{action}/{id}/{id2}",
                defaults: new { controller = "Laboratuvar", action = "YapilacakAnalizGrupTipi", id = UrlParameter.Optional, id2 = UrlParameter.Optional },
                namespaces: new[] { "Polyteks.Katman.Has.Controllers" }
            );
        }
    }
}
