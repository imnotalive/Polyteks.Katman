using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Areas.Yonetim
{
    public class YonetimAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Yonetim";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Yonetim_default",
                "Yonetim/{controller}/{action}/{id}",
                new { controller = "YonetimHome", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "YonetimCalismaSekli",                                              // Route name
                "Yonetim/{controller}/{action}/{id}/{id2}",                           // URL with parameters
                new { controller = "YonetimKayitlar", action = "YonetimCalismaSeklineGoreGetir", id = UrlParameter.Optional, id2 = UrlParameter.Optional }
            );
        }
    }
}