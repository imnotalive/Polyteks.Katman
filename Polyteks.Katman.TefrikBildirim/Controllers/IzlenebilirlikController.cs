using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.TefrikBildirim.Models;

namespace Polyteks.Katman.Has.Controllers
{
    public class IzlenebilirlikController : BaseController
    {
        public IzlenebilirlikPartiSonuModel IzlenebilirlikPartiSonuModeliGetir(IzlenebilirlikPartiSonuModel model)
        {
            var planlamaBaslangicTarihi = Convert.ToDateTime(model.PlanlamaBaslangicTarihi).Date;
            var planlamaBitisTarihi = Convert.ToDateTime(model.PlanlamaBitisTarihi).Date.AddDays(1);
            var PartiSonuTakipIzlenebilirlikler = new List<ZzzSrcnPartiSonuTakipIzlenebilirlik>();
            bool SessionVarmi = false;
            if (Session["Izlenebilirlik"] != null)
            {
                var araModel = (SessionIzlenebilirlikPartiSonuDroplar)Session["Izlenebilirlik"];
                if (araModel.Tarih > DateTime.Now.AddMinutes(-25))
                {
                    SessionVarmi = true;
                }
            }

            if (SessionVarmi)
            {
                var araModel = (SessionIzlenebilirlikPartiSonuDroplar)Session["Izlenebilirlik"];

                model.Partiler = araModel.Partiler;
                model.Cariler = araModel.Cariler;
                model.PartisonuTakipDurumlar = araModel.PartisonuTakipDurumlar;
                model.SiparisNolar =  araModel.SiparisNolar;
                model.Stoklar = araModel.Stoklar;
                model.RefakatNolar = araModel.RefakatNolar;
            }
            else
            {
                var tumListe = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.AsNoTracking().Select(a => new { a.PartiNo,a.CariAdiSecili,a.CariKoduSecili, a.PartiSonuTakipHareketAdi, a.PartiSonuTakipHareketTipi, a.SiparisNo, a.StokAdi, a.StokKodu, a.RefakatNo, a.PartiSonuTakipId }).ToList();

                var partilerDrop = tumListe.OrderBy(a => a.PartiSonuTakipId).Select(a => a.PartiNo).Distinct().ToList().Select(a => new DropItem() { Tanim = a, Id = a }).OrderBy(a => a.Tanim).ToList();
                var carilerDrop = tumListe.GroupBy(a => new { a.CariAdiSecili, a.CariKoduSecili }).Select(a => new DropItem() { Tanim = a.Key.CariAdiSecili, Id = a.Key.CariKoduSecili }).OrderBy(a => a.Tanim).ToList();
                var PartiSonuDrop = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.OrderBy(a => a.PartiSonuTakipHareketTipi).GroupBy(a => new { a.PartiSonuTakipHareketAdi, a.PartiSonuTakipHareketTipi }).Select(a => new DropItem() { Tanim = a.Key.PartiSonuTakipHareketAdi, Id = a.Key.PartiSonuTakipHareketTipi.ToString() }).OrderBy(a => a.Id).ToList();

                var SiparisNoDrop = tumListe.OrderBy(a => a.SiparisNo).Select(a => a.SiparisNo).Distinct().ToList().Select(a => new DropItem() { Tanim = a, Id = a }).OrderBy(a => a.Tanim).ToList();
                var StoklarDrop = tumListe.OrderBy(a => a.StokAdi).GroupBy(a => new { a.StokAdi, a.StokKodu }).Select(a => new DropItem() { Tanim = a.Key.StokAdi, Id = a.Key.StokKodu }).OrderBy(a => a.Tanim).ToList();

                var Refnolar = tumListe.OrderBy(a => a.RefakatNo).GroupBy(a => new { a.RefakatNo }).Select(a => new DropItem() { Tanim = a.Key.RefakatNo, Id = a.Key.RefakatNo.Trim() }).OrderBy(a => a.Tanim).ToList();

                /*
                var partilerDrop = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.OrderBy(a => a.PartiSonuTakipId).Select(a => a.PartiNo).Distinct().ToList().Select(a => new DropItem() { Tanim = a, Id = a }).OrderBy(a => a.Tanim).ToList();
                var carilerDrop = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.GroupBy(a => new { a.CariAdiSecili, a.CariKoduSecili }).Select(a => new DropItem() { Tanim = a.Key.CariAdiSecili, Id = a.Key.CariKoduSecili }).OrderBy(a => a.Tanim).ToList();
                var PartiSonuDrop = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.OrderBy(a => a.PartiSonuTakipHareketTipi).GroupBy(a => new { a.PartiSonuTakipHareketAdi, a.PartiSonuTakipHareketTipi }).Select(a => new DropItem() { Tanim = a.Key.PartiSonuTakipHareketAdi, Id = a.Key.PartiSonuTakipHareketTipi.ToString() }).OrderBy(a => a.Id).ToList();

                var SiparisNoDrop = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.OrderBy(a => a.SiparisNo).Select(a => a.SiparisNo).Distinct().ToList().Select(a => new DropItem() { Tanim = a, Id = a }).OrderBy(a => a.Tanim).ToList();
                var StoklarDrop = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.OrderBy(a => a.StokAdi).GroupBy(a => new { a.StokAdi, a.StokKodu }).Select(a => new DropItem() { Tanim = a.Key.StokAdi, Id = a.Key.StokKodu }).OrderBy(a => a.Tanim).ToList();

                var Refnolar = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.OrderBy(a => a.RefakatNo).GroupBy(a => new { a.RefakatNo }).Select(a => new DropItem() { Tanim = a.Key.RefakatNo, Id = a.Key.RefakatNo.Trim() }).OrderBy(a => a.Tanim).ToList();
                */

                model.Partiler = new SelectList(partilerDrop, "Id", "Tanim");
                model.Cariler = new SelectList(carilerDrop, "Id", "Tanim");
                model.PartisonuTakipDurumlar = new SelectList(PartiSonuDrop, "Id", "Tanim");
                model.SiparisNolar = new SelectList(SiparisNoDrop, "Id", "Tanim");
                model.Stoklar = new SelectList(StoklarDrop, "Id", "Tanim");
                model.RefakatNolar = new SelectList(Refnolar, "Id", "Tanim");
                var sessionModel = new SessionIzlenebilirlikPartiSonuDroplar
                {
                    Tarih = DateTime.Now,
                    Partiler = model.Partiler,
                    PartisonuTakipDurumlar = model.PartisonuTakipDurumlar,
                    SiparisNolar = model.SiparisNolar,
                    Stoklar = model.Stoklar,
                    Cariler = model.Cariler,
                    RefakatNolar = model.RefakatNolar
                };
                Session["Izlenebilirlik"] = null;
                Session["Izlenebilirlik"] = sessionModel;

            }

            #region Filtreleme

            bool HerhangiFiltrelemeUygulandiMi = false;
            if (model.PlanlamaTarihFiltresiYapilsinMi)
            {
                if (HerhangiFiltrelemeUygulandiMi==false)
                {
                    PartiSonuTakipIzlenebilirlikler = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                        .Where(a => a.PlanlamaTermini >= planlamaBaslangicTarihi &&
                                    a.PlanlamaTermini < planlamaBitisTarihi).OrderBy(a => a.PartiNo).ToList();
                    HerhangiFiltrelemeUygulandiMi = true;
                }
                else
                {
                    PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler
                        .Where(a => a.PlanlamaTermini > planlamaBaslangicTarihi &&
                                    a.PlanlamaTermini < planlamaBitisTarihi).OrderBy(a => a.PartiNo).ToList();
                }

            }
            if (model.SecilenParti!=null)
            {
                if (HerhangiFiltrelemeUygulandiMi == false)
                {
                    PartiSonuTakipIzlenebilirlikler = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                        .Where(a => a.PartiNo == model.SecilenParti).OrderBy(a => a.PartiNo).ToList();
                    HerhangiFiltrelemeUygulandiMi = true;
                }
                else
                {
                    PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler
                        .Where(a => a.PartiNo == model.SecilenParti).OrderBy(a => a.PartiNo).ToList();
                }
            }

            if (model.SecilenCariKod != null)
            {
                if (HerhangiFiltrelemeUygulandiMi == false)
                {
                    PartiSonuTakipIzlenebilirlikler = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                        .Where(a => a.CariKoduSecili == model.SecilenCariKod).OrderBy(a => a.PartiNo).ToList();
                    HerhangiFiltrelemeUygulandiMi = true;
                }
                else
                {
                    PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler
                        .Where(a => a.CariKoduSecili == model.SecilenCariKod).OrderBy(a => a.PartiNo).ToList();
                }
            }

            if (model.PartiSonuTakipDurumId!=0)
            {
                if (HerhangiFiltrelemeUygulandiMi == false)
                {
                    PartiSonuTakipIzlenebilirlikler = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                        .Where(a => a.PartiSonuTakipHareketTipi == model.PartiSonuTakipDurumId).OrderBy(a => a.PartiNo).ToList();
                    HerhangiFiltrelemeUygulandiMi = true;
                }
                else
                {
                    PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler
                        .Where(a => a.PartiSonuTakipHareketTipi == model.PartiSonuTakipDurumId).OrderBy(a => a.PartiNo).ToList();
                }
            }

            if (model.SiparisNo != null)
            {
                if (HerhangiFiltrelemeUygulandiMi == false)
                {
                    PartiSonuTakipIzlenebilirlikler = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                        .Where(a => a.SiparisNo == model.SiparisNo).OrderBy(a => a.PartiNo).ToList();
                    HerhangiFiltrelemeUygulandiMi = true;
                }
                else
                {
                    PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler
                        .Where(a => a.SiparisNo == model.SiparisNo).OrderBy(a => a.PartiNo).ToList();
                }
            }
            if (model.SecilenStokKodu != null)
            {
                if (HerhangiFiltrelemeUygulandiMi == false)
                {
                    PartiSonuTakipIzlenebilirlikler = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                        .Where(a => a.StokKodu == model.SecilenStokKodu).OrderBy(a => a.PartiNo).ToList();
                    HerhangiFiltrelemeUygulandiMi = true;
                }
                else
                {
                    PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler
                        .Where(a => a.StokKodu == model.SecilenStokKodu).OrderBy(a => a.PartiNo).ToList();
                }
            }
            if (model.SecilenRefNo != null)
            {
                if (HerhangiFiltrelemeUygulandiMi == false)
                {
                    PartiSonuTakipIzlenebilirlikler = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                        .Where(a => a.RefakatNo.Trim() == model.SecilenRefNo).OrderBy(a => a.PartiNo).ToList();
                    HerhangiFiltrelemeUygulandiMi = true;
                }
                else
                {
                    PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler
                        .Where(a => a.RefakatNo.Trim() == model.SecilenRefNo).OrderBy(a => a.PartiNo).ToList();
                }
            }

            #endregion
            model.PartiSonuTakipIzlenebilirlikler = PartiSonuTakipIzlenebilirlikler;
            return model;
        }
        // GET: Izlenebilirlik
        public ActionResult PartiSonuDurum()
        {
            TempData["Gizle"] = "gizle";
           // Session["Izlenebilirlik"] = null;
            var model = new IzlenebilirlikPartiSonuModel();

            return View(IzlenebilirlikPartiSonuModeliGetir(model));
        }
        [HttpPost]
        public ActionResult PartiSonuDurum(IzlenebilirlikPartiSonuModel model)
        {
            TempData["Gizle"] = "gizle";
           

            return View(IzlenebilirlikPartiSonuModeliGetir(model));
        }
    }
}