using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Polyteks.Katman.Toplanti.Controllers
{
    public class KullaniciController : Controller
    {
        POTA_PTKSEntities _dbpoly = new POTA_PTKSEntities();
        public void CookieKaydet(SrcnKullanicis Kullanici)
        {
            var cookieBase = Helper.CookieBaseBilgiGetir(2);
            var cookie = new HttpCookie(cookieBase.CookieName, Kullanici.KullaniciId.ToString());
            cookie.Expires = DateTime.Now.AddDays(10); // Süre(9 saat)
            HttpContext.Response.Cookies.Add(cookie);
        }
        public void ClearApplicationCache()
        {
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                HttpContext.Cache.Remove(keys[i]);
            }
        }
      
        public ActionResult Giris()
        {

            ClearApplicationCache();

            return View(new Kullanici());
        }
        [HttpPost]
        public ActionResult Giris(SrcnKullanicis model)
        {
            //string inputFile = @"..\Concrete\\EntityFramework\\PolyModel.edmx";
            if (model.GirisSifre == null)
            {
                TempData["Msg"] = "Şifrenizi Giriniz";
                TempData["class"] = "danger";
                return RedirectToAction("Giris");
            }
            else
            {
                var uye = _dbpoly.SrcnKullanicis.Find(model.KullaniciId);
                #region giriş alanı

                if (uye.Sifre.ToLower() == model.GirisSifre.ToLower())
                {
                    CookieKaydet(uye);
                    Session["kull"] = uye;
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["Msg"] = "Kullanıcı Kodu veya Şifre Hatalı";
                    TempData["class"] = "danger";
                    return RedirectToAction("Giris");
                }
                #endregion
            }

        }

        public string CookieGetir(string Id)
        {
            HttpCookie cookie = this.HttpContext.Request.Cookies[Id];
            if (cookie != null)
            {

                return cookie.Value;
            }
            else
            {
                return null;
            }


        }
        public ActionResult Cikis()
        {
            try
            {
                Session["kullanici"] = null;
                var cookieBase = Helpers.Helper.CookieBaseBilgiGetir(1);
                HttpCookie cookie = this.HttpContext.Request.Cookies[cookieBase.CookieName];

                cookie.Expires = DateTime.Now.AddHours(-10); // Süre(9 saat)
                HttpContext.Response.Cookies.Add(cookie);
            }
            catch (Exception e)
            {
                string AA = e.Message;
            }

            return RedirectToAction("Giris");
        }
    }
}