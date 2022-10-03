using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.Toplanti.Controllers
{
    public class YetkisizController : Controller
    {
        // GET: Yetkisiz
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public ActionResult KullaniciListeGetir(string id)
        {
            var liste = _dbPoly.SrcnKullanicis.Where(a => a.IsimSoyisim.ToUpper().Contains(id) && a.GizlemeDurumu == 0)
                .OrderBy(a => a.IsimSoyisim).ToList();
            return PartialView("~/Views/Yetkisiz/_KullaniciListeGetir.cshtml", liste);
        }
        public ActionResult KullaniciGetir(int id)
        {
            var liste = _dbPoly.SrcnKullanicis.Find(id);
            return PartialView("~/Views/Yetkisiz/_KullaniciGiris.cshtml", liste);
        }
    }
}