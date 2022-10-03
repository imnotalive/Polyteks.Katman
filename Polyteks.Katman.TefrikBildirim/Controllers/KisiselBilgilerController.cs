using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Has.Controllers;

namespace Polyteks.Katman.TefrikBildirim.Controllers
{
    public class KisiselBilgilerController : BaseController
    {
        // GET: KisiselBilgiler
        public ActionResult Duzenle()
        {
            return View(Kullanici);
        }
        [HttpPost]
        public ActionResult Duzenle(SrcnKullanicis model)
        {
            var editItem = _dbPoly.SrcnKullanicis.Find(Kullanici.KullaniciId);
            editItem.IsimSoyisim = model.IsimSoyisim;
            editItem.EmailAdres = model.EmailAdres;
            _dbPoly.SaveChanges();
            TempDataOlustur("Güncelleme İşlemi Yapılmıştır",true);
            Session["kull"] = null;
            return RedirectToAction("Duzenle");
        }
    }
}