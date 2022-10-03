using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.Helpers;

namespace Polyteks.Katman.Has.Filters
{
    public class Filter
    {
        public class CookieLoginRequiredAttribute : ActionFilterAttribute
        {

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                bool SessionVarmi = filterContext.HttpContext.Session["kull"] != null;
                if (SessionVarmi == false)
                {
                    var cookieBase = Helper.CookieBaseBilgiGetir(1);
                    if (filterContext.HttpContext.Request.Cookies[cookieBase.CookieName] == null)
                    {
                        filterContext.HttpContext.Session["uyari"] = "Lütfen Giriş Yapın";
                        filterContext.Result = new RedirectResult("/Kullanici/Giris");
                    }
                }


                base.OnActionExecuting(filterContext);
            }
        }



        
    }
}