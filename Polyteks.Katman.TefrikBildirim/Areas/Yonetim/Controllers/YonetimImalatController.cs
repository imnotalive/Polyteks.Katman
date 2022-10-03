using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Areas.Yonetim.Controllers
{
    public class YonetimImalatController : YonetimBaseController
    {
        // GET: Yonetim/YonetimImalat
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartiSonuTakipGeriAlListe()
        {
            return View();
        }

        public JsonResult PartiSonuListeGetir(string id, string id2)
        {
            var partiSonuListe = _dbPoly.SrcnPartiSonuTakips.Where(a => a.PartiNo == id || a.RefakatNo == id2 && a.PartiKapatmaTarihi != null).ToList();

            return Json(partiSonuListe, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartiSonuTakipSil(int id)
        {
            int say = 0;
            var partiSonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(id);
            var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == partiSonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemNo != "900");
            refKart.IslemBitisTarihi = null;
            refKart.SabitlenenMakineNo = null;
            _dbPoly.SaveChanges();

            var SrcnPartiSonuTakipBilgiOnays = _dbPoly.SrcnPartiSonuTakipBilgiOnays.Where(a => a.PartiSonuTakipId == partiSonuTakip.PartiSonuTakipId).ToList();
            var SrcnPartiSonuEkIslems = _dbPoly.SrcnPartiSonuEkIslems.Where(a => a.PartiSonuTakipId == partiSonuTakip.PartiSonuTakipId).ToList();
            var SrcnPartiSonuHarekets = _dbPoly.SrcnPartiSonuHarekets.Where(a => a.PartiSonuTakipId == partiSonuTakip.PartiSonuTakipId).ToList();

            _dbPoly.SrcnPartiSonuTakipBilgiOnays.RemoveRange(SrcnPartiSonuTakipBilgiOnays);
            _dbPoly.SaveChanges();

            _dbPoly.SrcnPartiSonuEkIslems.RemoveRange(SrcnPartiSonuEkIslems);
            _dbPoly.SaveChanges();

            _dbPoly.SrcnPartiSonuHarekets.RemoveRange(SrcnPartiSonuHarekets);
            _dbPoly.SaveChanges();


            _dbPoly.SrcnPartiSonuTakips.Remove(partiSonuTakip);
            _dbPoly.SaveChanges();
            
            return RedirectToAction("PartiSonuTakipGeriAlListe");
        }
    }
}