using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.Has.Controllers;
using Polyteks.Katman.TefrikBildirim.Models;

namespace Polyteks.Katman.TefrikBildirim.Controllers
{
    public class TanimlamalarController : BaseController
    {
        // GET: Tanimlamalar
        public ActionResult Liste()
        {
            var model = _dbPoly.Personel.OrderBy(a => a.PersonelAdi).ToList();
            return View(model);
           
        }
        public ActionResult Duzenle(string id)
        {
            var personel = _dbPoly.Personel.First(a => a.PersonelNo == id);

            return View();
        }

        public ActionResult SrcnKullaniciListe()
        {
            var model = new SrcnKullaniciModel();
            model.Kullanicilar = _dbPoly.SrcnKullanicis.Where(a => a.Sifre != null && a.Sifre.Length>0).OrderBy(a => a.IsimSoyisim).ToList();

            model.UretimHatlari = new SelectList(Helpers.Helper.UretimHatlariGetir(), "Id", "Tanim");
            model.UretimHatBolumleri = new SelectList(Helpers.Helper.UretimHatBirimlerilariGetir(), "Id", "Tanim");
            return View(model);

        }
        [HttpPost]
        public ActionResult SrcnKullaniciListe(SrcnKullaniciModel model)
        {
            var uretimHatlar = Helpers.Helper.UretimHatlariGetir();
            var bolumler = Helpers.Helper.UretimHatBirimlerilariGetir();

            var secilenUretimHat = uretimHatlar.First(a => a.Id == model.BirimId);
            var secilenUretimHatBolum = bolumler.First(a => a.Id == model.BolumId);

            foreach (var item in model.Kullanicilar.Where(a=>a.SeciliMi==true).ToList())
            {
                var editItem = _dbPoly.SrcnKullanicis.Find(item.KullaniciId);
                editItem.BirimId = Convert.ToInt32(secilenUretimHat.Id);
                editItem.Birim = secilenUretimHat.Tanim;
                editItem.Bolum = secilenUretimHatBolum.Tanim;
                editItem.BolumId = Convert.ToInt32(secilenUretimHatBolum.Id);
                editItem.GrupId = model.GrupId;
                editItem.GrupAdi = model.GrupId + ".GRUP";
                _dbPoly.SaveChanges();
            }
            TempDataOlustur("Ok",true);
            return RedirectToAction("SrcnKullaniciListe");

        }
    
    }
}