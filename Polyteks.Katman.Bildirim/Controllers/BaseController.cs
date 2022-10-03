using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Polyteks.Katman.Bildirim.Models;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.Bildirim.Controllers
{
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

        private LayoutModelBildirim BildirimKontrol(LayoutModelBildirim model)
        {

            return model;
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public POTA_TASDEntities _dbTasd = new POTA_TASDEntities();
        // GET: Base
        public Kullanici Kullanici { get; set; }
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var httpContext = requestContext.HttpContext;
            var LayoutModelBildirim = new LayoutModelBildirim();
            LayoutModelBildirim.Kullanici = new Kullanici();
            /*bool SorunVarmi = true;

            if (httpContext.Session["kull"] != null)
            {
                Kullanici = (Kullanici)httpContext.Session["kull"];
                SorunVarmi = false;
            }
            else
            {
                var cookieBase = Helpers.Helper.CookieBaseBilgiGetir(1);
                var reqCookie = requestContext.HttpContext.Request.Cookies[cookieBase.CookieName];
                if (reqCookie != null)
                {
                    int KullaniciId = Convert.ToInt32(reqCookie.Value);
                    Kullanici = _dbPoly.Kullanici.First(a => a.KullaniciId == KullaniciId);
                    httpContext.Session["kull"] = Kullanici;
                    SorunVarmi = false;
                }
            }
            if (SorunVarmi == false)
            {
                LayoutModelBildirim.Kullanici = Kullanici;
                LayoutModelBildirim = BildirimKontrol(LayoutModelBildirim);
            }


          
            string controllerName = requestContext.RouteData.Values["controller"].ToString();
            */

            //string menuAktif = Helper.AktifMenuAyariGetir(controllerName);
            //CalisanLayoutModel.MenuAktif = menuAktif;

            LayoutModelBildirim.Kullanici = Kullanici;
            //LayoutModelBildirim = BildirimKontrol(LayoutModelBildirim);
            ViewBag.LayoutModelBildirim = LayoutModelBildirim;
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}