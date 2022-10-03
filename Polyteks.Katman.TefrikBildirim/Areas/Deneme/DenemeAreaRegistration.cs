using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Areas.Deneme
{
    public class DenemeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Deneme";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Deneme_default",
                "Deneme/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}