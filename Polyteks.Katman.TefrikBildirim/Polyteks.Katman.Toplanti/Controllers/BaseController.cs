
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Polyteks.Katman.Toplanti.Models;
using Filter = Polyteks.Katman.Toplanti.Filters.Filter;

namespace Polyteks.Katman.Toplanti.Controllers
{
    [Filter.CookieLoginRequiredAttribute]
    public class BaseController : Controller
    {
        public void TempDataOlustur(string Mesaj, bool OlumLuMu)
        {
            TempData["Msg"] = Mesaj;
            TempData["class"] = "danger";
            if (OlumLuMu)
            {
                TempData["class"] = "success";
            }
        }
        public SrcnKullanicis Kullanici { get; set; }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var httpContext = requestContext.HttpContext;
            var LayoutModelToplanti = new LayoutModelToplanti();


            bool SorunVarmi = true;
            if (httpContext.Session["kull"] != null)
            {
                Kullanici = (SrcnKullanicis)httpContext.Session["kull"];
                SorunVarmi = false;
            }
            else
            {
                var cookieBase = Helpers.Helper.CookieBaseBilgiGetir(2);
                var reqCookie = requestContext.HttpContext.Request.Cookies[cookieBase.CookieName];
                if (reqCookie != null)
                {
                    int KullaniciId = Convert.ToInt32(reqCookie.Value);
                    Kullanici = _dbPoly.SrcnKullanicis.First(a => a.KullaniciId == KullaniciId);
                    httpContext.Session["kull"] = Kullanici;
                    SorunVarmi = false;
                }
            }
            if (SorunVarmi == false)
            {
                LayoutModelToplanti.Kullanici = Kullanici;
               //LayoutModelToplanti = BildirimKontrol(LayoutModelTefrik);
            }

            string controllerName = requestContext.RouteData.Values["controller"].ToString();

            //string menuAktif = Helper.AktifMenuAyariGetir(controllerName);
         

            ViewBag.LayoutModelToplanti = LayoutModelToplanti;
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}