using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Has.Models;
using Polyteks.Katman.TefrikBildirim.Areas.Yonetim.Models;
using Filter = Polyteks.Katman.Has.Filters.Filter;

namespace Polyteks.Katman.TefrikBildirim.Areas.Yonetim.Controllers
{
    [Filter.CookieLoginRequired]
    public class YonetimBaseController : Controller
    {
        public void TempDataCRUDOlustur(int Durum)
        {
            string Mesaj = "Kayıt İşlemi Yapılmıştır";
            TempData["class"] = "success";
            bool OlumLuMu = true;

            if (Durum == 2)
            {
                // güncelleme
                Mesaj = "Güncelleme İşlemi Yapılmıştır";
            }
            if (Durum == 3)
            {
                // sİLME
                Mesaj = "Silme İşlemi Yapılmıştır";
                OlumLuMu = false;
            }
            TempData["Msg"] = Mesaj;

            if (OlumLuMu == false)
            {
                TempData["class"] = "danger";
            }
        }
        #region Global degiskenler
        private YonetimLayoutModel BildirimKontrol(YonetimLayoutModel model)
        {

            return model;
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
      
        public SrcnKullanicis Kullanici { get; set; }

        private static POTA_PTKSEntities _appContext;

        public static POTA_PTKSEntities _dbPolyCreate(bool yenilensinMi)
        {
            if (_appContext == null)
            {

                _appContext = new POTA_PTKSEntities() { Configuration = { LazyLoadingEnabled = false, ProxyCreationEnabled = false } };


            }
            return _appContext;
        }
        #endregion
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
       
            var httpContext = requestContext.HttpContext;
            var YonetimLayoutModel = new YonetimLayoutModel();
           bool SorunVarmi = true;
            if (httpContext.Session["kull"] != null)
            {
                Kullanici = (SrcnKullanicis)httpContext.Session["kull"];
                SorunVarmi = false;
            }
            else
            {
                var cookieBase = Helpers.Helper.CookieBaseBilgiGetir(1);
                var reqCookie = requestContext.HttpContext.Request.Cookies[cookieBase.CookieName];
                if (reqCookie != null)
                {
                    int KullaniciId = Convert.ToInt32(reqCookie.Value);
                    Kullanici = _dbPoly.SrcnKullanicis.Find(KullaniciId);
                    httpContext.Session["kull"] = Kullanici;
                    SorunVarmi = false;
                }
            }
            if (SorunVarmi == false)
            {
                YonetimLayoutModel.Kullanici = Kullanici;
                YonetimLayoutModel = BildirimKontrol(YonetimLayoutModel);
            }

        

            string controllerName = requestContext.RouteData.Values["controller"].ToString();
            string actionName = requestContext.RouteData.Values["action"].ToString();

            //  httpContext.Session["CurrentURL"] = controllerName + " / " + actionName;
            ViewBag.YonetimLayoutModel = YonetimLayoutModel;
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}