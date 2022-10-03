using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.Helpers;
using Polyteks.Katman.TefrikBildirim.Models;

namespace Polyteks.Katman.Has.Controllers
{
    public class LaboratuvarController : BaseController
    {
        public ActionResult KaliteBildirimPusulasi()
        {
            TempData["Gizle"] = "gizle";
            return View(_dbPoly.Mrv_KaliteBildirimDetay.ToList());
        }
        public ActionResult _IsletmeKaliteBildirimPusulasi()
        {
            TempData["Gizle"] = "gizle";
            return PartialView(_dbPoly.Mrv_KaliteBildirimDetay.Where(a => a.Hata1YapilanMudahale == null && a.Hata2YapilanMudahale == null && a.Hata3YapilanMudahale == null).OrderByDescending(a => a.Mrv_KaliteBildirimAna.LabBildirimTarihi).ToList());
        }
        public ActionResult _LabKaliteBildirimPusulasi()
        {
            TempData["Gizle"] = "gizle";
            return PartialView(_dbPoly.Mrv_KaliteBildirimDetay.Where(a => a.Hata1YapilanMudahale != null && a.Hata2YapilanMudahale != null && a.Hata3YapilanMudahale != null).OrderByDescending(a => a.Mrv_KaliteBildirimAna.LabBildirimTarihi).ToList());
        }

        public ActionResult _IsletmeKaliteBildirimPusulasiDuzenle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mrv_KaliteBildirimDetay mrv_KaliteBildirimDetay = _dbPoly.Mrv_KaliteBildirimDetay.Find(id);
            Mrv_KaliteBildirimDetay fisDetay = _dbPoly.Mrv_KaliteBildirimDetay.FirstOrDefault(x => x.ID == id);
            if (mrv_KaliteBildirimDetay == null)
            {
                return HttpNotFound();
            }
            ViewBag.MakineAdi = new SelectList(_dbPoly.Mrv_KaliteBildirim_Makine, "ID", "MakineAdi",mrv_KaliteBildirimDetay.MakineAdi);
            ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri", mrv_KaliteBildirimDetay.Isletme);
            ViewBag.Hata1 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", mrv_KaliteBildirimDetay.Hata1);
            ViewBag.Hata2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", mrv_KaliteBildirimDetay.Hata2);
            ViewBag.Hata3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", mrv_KaliteBildirimDetay.Hata3);
            return View(fisDetay);
        }
        [HttpPost]
        public ActionResult _IsletmeKaliteBildirimPusulasiDuzenle(int id,Mrv_KaliteBildirimDetay deneme,Mrv_KaliteBildirimAna aaa)
        {
            Mrv_KaliteBildirimDetay fisDetay = _dbPoly.Mrv_KaliteBildirimDetay.FirstOrDefault(x => x.ID == id);
            if (ModelState.IsValid)
            {
              

                fisDetay.Hata1 = deneme.Hata1;
                fisDetay.Hata2= deneme.Hata2;
                fisDetay.Hata3 = deneme.Hata3;
                fisDetay.Aciklama = deneme.Aciklama;


                _dbPoly.SaveChanges();
                return RedirectToAction("KaliteBildirimPusulasi");
            }
            ViewBag.MakineAdi = new SelectList(_dbPoly.Mrv_KaliteBildirim_Makine, "ID", "MakineAdi",deneme.MakineAdi);
            ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri", deneme.Isletme);
            ViewBag.Hata1 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", deneme.Hata1);
            ViewBag.Hata2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", deneme.Hata2);
            ViewBag.Hata3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", deneme.Hata3);
            return View(deneme);
        }
        public ActionResult _IsletmeKaliteBildirimPusulasiSil(int? id)
        {

            TempData["Gizle"] = "gizle";
            if (id == null)
            {
                return RedirectToAction(nameof(KaliteBildirimPusulasi));
            }
            Mrv_KaliteBildirimDetay deneme = _dbPoly.Mrv_KaliteBildirimDetay.Find(id);

            if (deneme == null)
            {
                return HttpNotFound();
            }
            return View(deneme);
        }
    
            [HttpPost, ActionName("_IsletmeKaliteBildirimPusulasiSil")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
            Mrv_KaliteBildirimDetay deneme = _dbPoly.Mrv_KaliteBildirimDetay.First(x => x.ID == id);
                Mrv_KaliteBildirimAna fis = deneme.Mrv_KaliteBildirimAna;
                _dbPoly.Mrv_KaliteBildirimDetay.Remove(deneme);
                if (fis != null)
                    _dbPoly.Mrv_KaliteBildirimAna.Remove(fis);
                TempDataOlustur("Silme İşlemi Gerçekleştirilmiştir.", false);
                _dbPoly.SaveChanges();
                return RedirectToAction("KaliteBildirimPusulasi", "Laboratuvar");
            }

    
        public ActionResult YeniKaliteBildirimiYap()
        {
            //var model = _dbPoly.Mrv_KaliteBildirimDetay.ToList();
            //return View(model);
            ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri");
            ViewBag.Hata1 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi");
            ViewBag.Hata2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi");
            ViewBag.Hata3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi");
            ViewBag.MakineAdi = new SelectList(_dbPoly.Mrv_KaliteBildirim_Makine, "ID", "MakineAdi");

            return View();
        }
        [HttpPost]
        public ActionResult YeniKaliteBildirimiYap(Mrv_KaliteBildirimDetay fisDetay,Mrv_KaliteBildirimAna deneme)
        {

            //var model = _dbPoly.Mrv_KaliteBildirimDetay.ToList();
            //return View(model);
            if (fisDetay != null && fisDetay.Isletme != null)
            {

                Mrv_KaliteBildirimAna fis;
                fis = _dbPoly.Mrv_KaliteBildirimAna.FirstOrDefault(x => x.Personel == Kullanici.IsimSoyisim);
                if (ModelState.IsValid)
                {

                    if (fis == null)
                    {

                        fis = new Mrv_KaliteBildirimAna()
                        {
                            LabBildirimTarihi = DateTime.Now,
                            Personel = Kullanici.IsimSoyisim,
                            PersonelBolum = Kullanici.Birim,
                            Aciklama=deneme.Aciklama,
                      


                        };


                        _dbPoly.Mrv_KaliteBildirimAna.Add(fis);
                        _dbPoly.SaveChanges();
                    }
                }
                fisDetay.Mrv_KaliteBildirimAna = fis;
                fisDetay.ID = fis.ID;
                fisDetay.Durum = 0;//Laboratuvar Bildirim Yapınca

                //fisDetay.Firma = stok.Firma;
                //fisDetay.Parti = stok.IATOPartiNo;
                //fisDetay.Kalite = stok.Kalite;
                ////fisDetay.Miktar = stok.Miktar;
                //fisDetay.StokAdi = stok.StokAdi;
                //fisDetay.Birim = "KG";
                //fisDetay.Tarih = DateTime.Now;

                ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri", fisDetay.Isletme);
                ViewBag.Hata1 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", fisDetay.Hata1);
                ViewBag.Hata2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", fisDetay.Hata2);
                ViewBag.Hata3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", fisDetay.Hata3);
                ViewBag.MakineAdi = new SelectList(_dbPoly.Mrv_KaliteBildirim_Makine, "ID", "MakineAdi", fisDetay.MakineAdi);
                _dbPoly.Mrv_KaliteBildirimDetay.Add(fisDetay);
                _dbPoly.SaveChanges();
                TempDataOlustur("Talebiniz oluşturulmuştur.", true);
                return RedirectToAction(nameof(KaliteBildirimPusulasi));

            }
                return View(fisDetay);
            
        }
        //[HttpPost]
        //public ActionResult MamulAmbariTalepteBulun(string id1, string id2, string id3, Mrv_MalzemeDetay fisDetay)
        //{

        //    View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == id1 && x.Kalite == id2 && x.StokAdi == id3);
        //    ViewBag.data = stok;
        //    if (fisDetay != null && fisDetay.Miktar != null && fisDetay.Miktar <= stok.Miktar)
        //    {
        //        int sayac = 0;
        //        Mrv_MalzemeAna fis;
        //        fis = _dbPoly.Mrv_MalzemeAna.FirstOrDefault(x => x.TamamlandiMi == false && x.Personel == Kullanici.IsimSoyisim && x.Tip == (int)MalzemeAnaTipi.Mamul);
        //        if (fis == null)
        //        {
        //            fis = new Mrv_MalzemeAna()
        //            {
        //                Tarih = DateTime.Now,
        //                Personel = Kullanici.IsimSoyisim,
        //                AmbarAdi = "MAMUL AMBARI",

        //                Bolum = Kullanici.Birim,
        //                Tip = (int)MalzemeAnaTipi.Mamul,
        //                TamamlandiMi = false
        //            };
        //            sayac++;
        //            _dbPoly.Mrv_MalzemeAna.Add(fis);
        //            _dbPoly.SaveChanges();
        //        }

        //        fisDetay.Mrv_MalzemeAna = fis;
        //        fisDetay.MalzemeNo = fis.MalzemeNo;
        //        fisDetay.Firma = stok.Firma;
        //        fisDetay.Parti = stok.IATOPartiNo;
        //        fisDetay.Kalite = stok.Kalite;
        //        //fisDetay.Miktar = stok.Miktar;
        //        fisDetay.StokAdi = stok.StokAdi;
        //        fisDetay.Birim = "KG";
        //        fisDetay.Tarih = DateTime.Now;
        //        _dbPoly.Mrv_MalzemeDetay.Add(fisDetay);
        //        _dbPoly.SaveChanges();
        //        TempDataOlustur("Talebiniz oluşturulmuştur.", true);
        //        return RedirectToAction(nameof(MamulAmbariStokGoruntuleme));
        //    }
        //    ViewData["View_STOK_DURUM_PTKS_TASD"] = stok;
        //    ViewBag.Hata = "En fazla " + stok.Miktar.Value.ToString("G29") + " kadar talepte bulunabilirsiniz!";
        //    return View(fisDetay);
        //}
        //[HttpPost]
        //public ActionResult YeniKaliteBildirimiYap(int a)
        //{

        //    return View();
        //}
        //public ActionResult YeniKaliteBildirimiYap(string a1, string a2, string a3, string a4)
        //{
        //    ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri");

        //    //ViewBag.HataId = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi");
        //    //ViewBag.HataId2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi");
        //    //ViewBag.HataId3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi");
        //    //Mrv_KaliteBildirimBolumleri Isletme = _dbPoly.Mrv_KaliteBildirimBolumleri.FirstOrDefault(x => x.IsletmeBolumleri == a1);
        //    //Mrv_KaliteBildirimHata stok2 = _dbPoly.Mrv_KaliteBildirimHata.FirstOrDefault(x => x.HataTanimi == a2);
        //    //Mrv_KaliteBildirimHata stok3 = _dbPoly.Mrv_KaliteBildirimHata.FirstOrDefault(x => x.HataTanimi == a3);
        //    //Mrv_KaliteBildirimHata stok4 = _dbPoly.Mrv_KaliteBildirimHata.FirstOrDefault(x => x.HataTanimi == a4);
        //    //ViewBag.data = Isletme;
        //    //Mrv_KaliteBildirimBolumleri stok = _dbPoly.Mrv_KaliteBildirimBolumleri.FirstOrDefault(x => x.IsletmeBolumleri == a1);
        //    //ViewBag.data = stok;
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult YeniKaliteBildirimiYap(string a1,string a2,string a3,string a4,Mrv_KaliteBildirimDetay fisDetay, Mrv_KaliteBildirimAna aaa, Mrv_KaliteBildirimBolumleri bbb)
        //{

        //    //ViewBag.HataId = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", fisDetay.Mrv_KaliteBildirimAna.HataId);
        //    //ViewBag.HataId2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", fisDetay.Mrv_KaliteBildirimAna.HataId2);
        //    //ViewBag.HataId3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", fisDetay.Mrv_KaliteBildirimAna.HataId3);
        //    //Mrv_KaliteBildirimHata stok2 = _dbPoly.Mrv_KaliteBildirimHata.FirstOrDefault(x => x.HataTanimi == a2);
        //    //Mrv_KaliteBildirimHata stok3 = _dbPoly.Mrv_KaliteBildirimHata.FirstOrDefault(x => x.HataTanimi == a3);
        //    //Mrv_KaliteBildirimHata stok4 = _dbPoly.Mrv_KaliteBildirimHata.FirstOrDefault(x => x.HataTanimi == a4);
        //    //Mrv_KaliteBildirimBolumleri Isletme = _dbPoly.Mrv_KaliteBildirimBolumleri.FirstOrDefault(x => x.IsletmeBolumleri == a1);

        //    //ViewBag.data = Isletme;

        //    //ViewBag.data2 = stok2;
        //    //ViewBag.data3 = stok3;
        //    //ViewBag.data4 = stok4;
        //    //Mrv_KaliteBildirimBolumleri stok = _dbPoly.Mrv_KaliteBildirimBolumleri.FirstOrDefault(x => x.IsletmeBolumleri == a1);
        //    //ViewBag.data = stok;

        //        if (fisDetay != null)
        //        {
        //            int sayac = 0;
        //            Mrv_KaliteBildirimAna fis;



        //            fis = _dbPoly.Mrv_KaliteBildirimAna.FirstOrDefault(x => x.Personel == Kullanici.IsimSoyisim);
        //            if (fis != null && fis.hata== bbb.IsletmeBolumleri)
        //            {
        //                fis = new Mrv_KaliteBildirimAna()
        //                {
        //                    LabBildirimTarihi = DateTime.Now,
        //                    Personel = Kullanici.IsimSoyisim,
        //                    PersonelBolum = Kullanici.Birim,



        //                };
        //                sayac++;
        //                _dbPoly.Mrv_KaliteBildirimAna.Add(fis);
        //                //TempDataOlustur("Talebiniz oluşturulmuştur.", true);
        //                _dbPoly.SaveChanges();
        //            }

        //            fisDetay.Mrv_KaliteBildirimAna = fis;
        //            fisDetay.Durum = true;

        //            _dbPoly.Mrv_KaliteBildirimDetay.Add(fisDetay);
        //            _dbPoly.SaveChanges();
        //            TempDataOlustur("Talebiniz oluşturulmuştur.", true);
        //            return RedirectToAction(nameof(KaliteBildirimPusulasi));

        //        }

        //    //ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri", aaa.Isletme);
        //    //return View(aaa);
        //    ////fisDetay.Mrv_KaliteBildirimAna.hata = Isletme.IsletmeBolumleri;
        //    ////fisDetay.BildirimiYapan = Kullanici.IsimSoyisim;
        //    ////fisDetay.BildirimTarihiSaati = DateTime.Now;
        //    ////fisDetay.Mrv_KaliteBildirimAna.BildirimDurumu = stok.IATOPartiNo;
        //    ////fisDetay.SiraNo = sayac;
        //    ////fisDetay.Miktar = stok.Miktar;
        //    ////fisDetay.StokAdi = stok.StokAdi;

        //    //_dbPoly.Mrv_KaliteBildirimDetay.Add(fisDetay);
        //    //_dbPoly.SaveChanges();

        //    //TempDataOlustur("Talebiniz oluşturulmuştur.", true);
        //    //ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri", aaa.Isletme);

        //    //}
        //    return View(fis);

        //    //return View(fisDetay);
        //}
        ////Class1 cs = new Class1();


        public JsonResult hataGetir(int p)
        {
            var hatalar = (from x in _dbPoly.Mrv_KaliteBildirimHata
                           join y in _dbPoly.Mrv_KaliteBildirimBolumleri on x.Mrv_KaliteBildirimBolumleri.ID equals y.ID
                           where x.Mrv_KaliteBildirimBolumleri.ID == p
                           select new
                           {
                               Text = x.HataTanimi,
                               Value = x.ID.ToString()
                           }).ToList().OrderBy(x => x.Text);
            return Json(hatalar, JsonRequestBehavior.AllowGet);
        }
        public JsonResult makineGetir(int p)
        {
            var hatalars = (from x in _dbPoly.Mrv_KaliteBildirim_Makine
                           join y in _dbPoly.Mrv_KaliteBildirimBolumleri on x.Mrv_KaliteBildirimBolumleri.ID equals y.ID
                           where x.Mrv_KaliteBildirimBolumleri.ID == p
                           select new
                           {
                               Text = x.MakineAdi,
                               Value = x.ID.ToString()
                           }).ToList().OrderBy(x=>x.Text);
            return Json(hatalars, JsonRequestBehavior.AllowGet);
        }

        public JsonResult hataGetir1(int p)
        {
            var hatalar = (from x in _dbPoly.Mrv_KaliteBildirimHata
                           join y in _dbPoly.Mrv_KaliteBildirimBolumleri on x.Mrv_KaliteBildirimBolumleri.ID equals y.ID
                           where x.Mrv_KaliteBildirimBolumleri.ID == p
                           select new
                           {
                               Text = x.HataTanimi,
                               Value = x.ID.ToString()
                           }).ToList().OrderBy(x => x.Text);
            return Json(hatalar, JsonRequestBehavior.AllowGet);
        }
        public JsonResult makineGetir1(int p)
        {
            var hatalars = (from x in _dbPoly.Mrv_KaliteBildirim_Makine
                            join y in _dbPoly.Mrv_KaliteBildirimBolumleri on x.Mrv_KaliteBildirimBolumleri.ID equals y.ID
                            where x.Mrv_KaliteBildirimBolumleri.ID == p
                            select new
                            {
                                Text = x.MakineAdi,
                                Value = x.ID.ToString()
                            }).ToList().OrderBy(x => x.Text);
            return Json(hatalars, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult KaliteBildirimPusulasi(Mrv_KaliteBildirimDetay fisDetay)
        //{
        //    return View();
        //}
        //public ActionResult MamulAmbariTalepteBulun(string id1, string id2, string id3)
        //{
        //    TempData["Gizle"] = "gizle";
        //    View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == id1 && x.Kalite == id2 && x.StokAdi == id3);
        //    ViewBag.data = stok;
        //    return View();
        //}
        //public ActionResult _showParticalView()
        //{ 
        //    cs.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri");
        //    cs.Hatalar = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi");

        //    return PartialView("_showParticalView", cs);
        //}
        //[HttpPost]
        //public ActionResult MamulAmbariTalepteBulun(string id1, string id2, string id3, Mrv_MalzemeDetay fisDetay)
        //{

        //    View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == id1 && x.Kalite == id2 && x.StokAdi == id3);
        //    ViewBag.data = stok;
        //    if (fisDetay != null && fisDetay.Miktar != null && fisDetay.Miktar <= stok.Miktar)
        //    {
        //        int sayac = 0;
        //        Mrv_MalzemeAna fis;
        //        fis = _dbPoly.Mrv_MalzemeAna.FirstOrDefault(x => x.TamamlandiMi == false && x.Personel == Kullanici.IsimSoyisim && x.Tip == (int)MalzemeAnaTipi.Mamul);
        //        if (fis == null)
        //        {
        //            fis = new Mrv_MalzemeAna()
        //            {
        //                Tarih = DateTime.Now,
        //                Personel = Kullanici.IsimSoyisim,
        //                AmbarAdi = "MAMUL AMBARI",

        //                Bolum = Kullanici.Birim,
        //                Tip = (int)MalzemeAnaTipi.Mamul,
        //                TamamlandiMi = false
        //            };
        //            sayac++;
        //            _dbPoly.Mrv_MalzemeAna.Add(fis);
        //            _dbPoly.SaveChanges();
        //        }

        //        fisDetay.Mrv_MalzemeAna = fis;
        //        fisDetay.MalzemeNo = fis.MalzemeNo;
        //        fisDetay.Firma = stok.Firma;
        //        fisDetay.Parti = stok.IATOPartiNo;
        //        fisDetay.Kalite = stok.Kalite;
        //        //fisDetay.Miktar = stok.Miktar;
        //        fisDetay.StokAdi = stok.StokAdi;
        //        fisDetay.Birim = "KG";
        //        fisDetay.Tarih = DateTime.Now;
        //        _dbPoly.Mrv_MalzemeDetay.Add(fisDetay);
        //        _dbPoly.SaveChanges();
        //        TempDataOlustur("Talebiniz oluşturulmuştur.", true);
        //        return RedirectToAction(nameof(MamulAmbariStokGoruntuleme));
        //    }
        //    ViewData["View_STOK_DURUM_PTKS_TASD"] = stok;
        //    ViewBag.Hata = "En fazla " + stok.Miktar.Value.ToString("G29") + " kadar talepte bulunabilirsiniz!";
        //    return View(fisDetay);
        //}
        //public ActionResult Home()
        //{

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult KaliteProblemBildirimiEkle(int id)
        //{

        //    return View();
        //}
        //public ActionResult _KaliteProblemUretimBildirimiEkle()
        //{
        //    TempData["Gizle"] = "gizle";
        //    return PartialView();

        //}
        //[HttpPost]
        //public ActionResult _KaliteProblemUretimBildirimiEkle(Mrv_KaliteBildirimDetay fisDetay)
        //{
        //    TempData["Gizle"] = "gizle";
        //    if (fisDetay != null)
        //    {
        //        int sayac = 0;
        //        Mrv_KaliteBildirimAna fis;
        //        //Mrv_KaliteBildirimHata bolum;

        //        fis = _dbPoly.Mrv_KaliteBildirimAna.FirstOrDefault(x=> x.Personel== Kullanici.IsimSoyisim);
        //        if (fis == null)
        //        {
        //            fis = new Mrv_KaliteBildirimAna()
        //            {
        //                LabBildirimTarihi= DateTime.Now,

        //                Personel = Kullanici.IsimSoyisim,
        //                PersonelBolum=Kullanici.Birim,
        //                Isletme="URETIM"

        //            };
        //            sayac++;
        //            _dbPoly.Mrv_KaliteBildirimAna.Add(fis);
        //            _dbPoly.SaveChanges();
        //        }

        //        fisDetay.Mrv_KaliteBildirimAna = fis;
        //        fisDetay.Durum = false;
        //        //fisDetay.BildirimiYapan = Kullanici.IsimSoyisim;
        //        //fisDetay.BildirimTarihiSaati = DateTime.Now;
        //        //fisDetay.Mrv_KaliteBildirimAna.BildirimDurumu = stok.IATOPartiNo;
        //        //fisDetay.SiraNo = sayac;
        //        //fisDetay.Miktar = stok.Miktar;
        //        //fisDetay.StokAdi = stok.StokAdi;

        //        _dbPoly.Mrv_KaliteBildirimDetay.Add(fisDetay);
        //        _dbPoly.SaveChanges();
        //        TempDataOlustur("Talebiniz oluşturulmuştur.", true);
        //        return RedirectToAction(nameof(Home));
        //    }

        //    return View(fisDetay);


        //}

        public ActionResult LabPoyIkazTakibi(int? ara)
        {

            TempData["Gizle"] = "gizle";

            if (ara.HasValue)
            {
                return View(_dbPoly.Mrv_POY_Ikaz.Where(x => x.PartiNo.Contains(ara.ToString())).OrderByDescending(x => x.Tarih).ToList());

            }
            else
            {
                return View(_dbPoly.Mrv_POY_Ikaz.OrderByDescending(a => a.Tarih).ToList().ToPagedList(1, 999));
            }

        }
        public ActionResult LabPoyIkazGuncelle(int? id)
        {

            TempData["Gizle"] = "gizle";
            if (id == null)
            {
                return HttpNotFound();
            }
            Mrv_POY_Ikaz deneme = _dbPoly.Mrv_POY_Ikaz.Find(id);
            if (deneme == null)
            {
                return HttpNotFound();
            }
            return View(deneme);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LabPoyIkazGuncelle(Mrv_POY_Ikaz deneme)
        {
            TempData["Gizle"] = "gizle";

            MailMessage mailim = new MailMessage();
            mailim.To.Add("poyikaz@tasdelengroup.com");
            mailim.From = new MailAddress("pmailsystem1@tasdelengroup.com");
            mailim.Subject = "POY IKAZ BILDIRIMI(LABORATUVAR)HK.";
            //var body = "<p>POY IKAZ BILDIRIMI YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p>" + deneme.PartiNo;
            mailim.Body = "<b><p>POY IKAZ BILDIRIMI SONUÇLANDI.POY IKAZ ICIN LABORATUVAR SONUC BILDIRMISTIR.</p></b>"
            + deneme.PartiNo + "&nbsp Parti numarasında hata vardır.<br/>"
            + deneme.ProblemTanimi + "&nbsp Problemi bildirilmiştir.<br/>"
            + deneme.Uretim_Mak_Kan_Poz + ":&nbsp İplik Pozisyonu<br/>"
             + deneme.Teksture_Makine_No + ":&nbsp Tekstüre Makine Numarası<br/>"
             + deneme.Teksture_Pozisyon_No + ":&nbsp Tekstüre Pozisyon Numarası<br/>"
           + "Üretim " + deneme.YapilanMudahale + "&nbsp Müdahalesi Yapmıştır.<br/>"
           + "Laboratuvar Sonucu: "+ deneme.LabSonuc;
         
            mailim.IsBodyHtml = true;
            //mailim.Body = body;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("pmailsystem1@tasdelengroup.com", "");
            smtp.Host = "smtprelay.yaanimail.com";
            smtp.Port = 587;
            smtp.EnableSsl = false;
            try
            {
                smtp.Send(mailim);
                TempData["Message"] = "Mesaj Basariyla Gonderildi";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Mesaj Gönderilemedi.Hata Nedeni :" + ex.Message;
            }
            TempData["Gizle"] = "gizle";
            if (ModelState.IsValid)
            {
                _dbPoly.Entry(deneme).State = EntityState.Modified;
                _dbPoly.SaveChanges();
                TempDataOlustur("Değişiklikler kaydedilmiştir", true);
                return RedirectToAction("LabPoyIkazTakibi");
            }
            return View(deneme);
        }
        public ActionResult NumunePdfIndir(int id)
        {
            return NumuneYapilabilirlikPdfOlustur(id);
        }


        public void KayitliBilgiDegisimMailiGonder(int dosyaTipi, List<DropItem> Guncellemeler)
        {
            // dosyatipi => 1 = günlük imalat,
            if (dosyaTipi == 1)
            {
                /*
                      KayitGuncellemeleri.Add(new DropItem() { Tanim = item.AnalizDegeriString, DigerTanim = editItem.AnalizDegeriString, IdInt = editItem.LabAnalizItemAnalizCesitId });
                 */

                var Partiler = new List<string>();
                var labCesitIdler = Guncellemeler.Select(a => a.IdInt).ToList();
                var labAnalizCesitItems = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AsNoTracking().Where(a => labCesitIdler.Any(b => b == a.LabAnalizItemAnalizCesitId)).OrderBy(a => a.LabAnalizCesitAdi).ThenBy(a => a.LabAnalizItemId).ToList();
                var labItemIdler = labAnalizCesitItems.Select(a => a.LabAnalizItemId).Distinct().ToList();

                var labAnalizItems = _dbPoly.SrcnLabAnalizItems.AsNoTracking().Where(a => labItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();
                var labAnalizIdler = labAnalizItems.Select(a => a.LabAnalizId).Distinct().ToList();
                var labAnalizler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => labAnalizIdler.Any(b => b == a.LabAnalizId)).ToList();

                var mailTable = "<table border=\"1\">";

                var theadItems = new List<string>();
                var tbodyItems = new List<string>();
                theadItems.Add("Dosya No");
                theadItems.Add("Lab Analiz No");
                theadItems.Add("Parti");
                theadItems.Add("İş Emri");
                theadItems.Add("Bobin");
                theadItems.Add("Analiz");

                theadItems.Add("Eski Değer");
                theadItems.Add("Yeni Değer");

                string theadOlustur = "<thead><tr>";
                foreach (var s in theadItems)
                {
                    theadOlustur += "<th>" + s + "</th>";
                }

                theadOlustur += "</tr></thead>";
                string tbodyOlustur = "<tbody>";

                var KullanilanSatirlar = new List<int>();
                foreach (var analizCesit in labAnalizCesitItems.ToList())
                {
                    if (labAnalizItems.Any(a => a.LabAnalizItemId == analizCesit.LabAnalizItemId))
                    {
                        var LabAnalizItem = labAnalizItems.First(a => a.LabAnalizItemId == analizCesit.LabAnalizItemId);

                        if (labAnalizler.Any(a => a.LabAnalizId == LabAnalizItem.LabAnalizId))
                        {
                            var labAnaliz = labAnalizler.First(a => a.LabAnalizId == LabAnalizItem.LabAnalizId);
                            tbodyItems = new List<string>();

                            if (Partiler.Count(a => a == labAnaliz.PartiNo.Trim().ToUpper()) == 0)
                            {
                                Partiler.Add(labAnaliz.PartiNo.Trim().ToUpper());
                            }
                            tbodyOlustur += "<tr>";
                            tbodyItems.Add("#" + labAnaliz.BagliOlduguDosyaId);
                            tbodyItems.Add("#" + labAnaliz.LabAnalizId);
                            tbodyItems.Add(labAnaliz.PartiNo.Trim());
                            tbodyItems.Add(labAnaliz.RefakartKartNo.Trim());
                            tbodyItems.Add(LabAnalizItem.BobinAdi);
                            tbodyItems.Add(analizCesit.LabAnalizCesitAdi.Trim());
                            string EskiDeger = Guncellemeler
                                .First(a => a.IdInt == analizCesit.LabAnalizItemAnalizCesitId).DigerTanim;
                            tbodyItems.Add(EskiDeger);
                            tbodyItems.Add(analizCesit.AnalizDegeriString.Trim());

                            foreach (var s in tbodyItems)
                            {
                                tbodyOlustur += "<td>" + s + "</td>";
                            }

                            tbodyOlustur += "</tr>";
                        }
                    }
                }
                tbodyOlustur += "</tbody>";

                mailTable += theadOlustur;
                mailTable += tbodyOlustur;
                mailTable += "</table> </br>" + Kullanici.IsimSoyisim;
                //var labKullanicilari = _dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == 18).ToList().Select(a=>a.EmailAdres).ToList();
                var labKullanicilari = new List<string>() { "kvural@polyteks.com.tr" };
                string partiBilgiler = "";
                if (labKullanicilari.Any())
                {

                    foreach (var i in Partiler)
                    {
                        partiBilgiler += i;
                        if (i != Partiler.Last())
                        {
                            partiBilgiler += ", ";
                        }
                    }
                    GenelDurumMailGonder(null, labKullanicilari, mailTable, partiBilgiler + " Analiz Sonuç Güncellemeleri");
                }

            }
        }
        public void DosyaLabAnalizDurumGuncelle(int dosyaTipi, int id, bool DosyaDurumuDegistiMi)
        {
            // dosyatipi => 1 = günlük imalat, 2= numune, 3=müşteri şikayet, 4= deneme, 5= tecrübe

            if (dosyaTipi == 1)
            {
                var gunlukImalat = _dbPoly.SrcnGunlukImalatDosyas.Find(id);
                var labAnalizDurum = new SrcnLabAnalizDurumus();
                if (DosyaDurumuDegistiMi)
                {
                    _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                    {
                        DosyaAdi = "Günlük İmalat Analizleri",
                        DosyaHareketId = 99,
                        DosyaTipi = 1,
                        KayitTarihi = DateTime.Now,
                        HareketAdi = gunlukImalat.DosyaDurumAdi.ToUpper() + " Olarak Güncellenmiştir " + Kullanici.IsimSoyisim,
                        SikayetNumuneDosyaId = id
                    });
                    _dbPoly.SaveChanges();
                }
                if (gunlukImalat.DosyaDurumId == 11)
                {
                    // Analiz Kayıdı Oluşturuldu
                    // LabAnalizDurumu=> 1-Analiz Kayıdı Oluşturuldu
                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(1);
                }
                if (gunlukImalat.DosyaDurumId == 12)
                {
                    // Analiz Yapılmaya Başlandı
                    // LabAnalizDurumu=> 2-Analiz Yapılmaya Başlandı
                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(2);

                }
                if (gunlukImalat.DosyaDurumId == 13)
                {
                    // Analiz Tamamlandı
                    // LabAnalizDurumu=> 3-Tamamlandı
                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(3);

                }
                var labAnalizHareketler = new List<SrcnLabAnalizHarekets>();
                foreach (var item in _dbPoly.SrcnLabAnalizs.Where(a => a.BagliOlduguDosyaId == gunlukImalat.GunlukImalatDosyaId && a.ImalatAnalizYapilmaTipi == 1 && a.LabAnalizDurumu != labAnalizDurum.LabAnalizDurumu).ToList())
                {
                    item.LabAnalizDurumuAdi = labAnalizDurum.LabAnalizDurumuAdi;
                    item.LabAnalizDurumu = labAnalizDurum.LabAnalizDurumu;
                    _dbPoly.SaveChanges();
                    labAnalizHareketler.Add(new SrcnLabAnalizHarekets() { LabAnalizId = item.LabAnalizId, KayitTarihi = DateTime.Now, LabAnalizDurumu = labAnalizDurum.LabAnalizDurumu, HareketAdi = labAnalizDurum.LabAnalizDurumuAdi + " Olarak Güncellenmiştir " + Kullanici.IsimSoyisim });
                }
                _dbPoly.SrcnLabAnalizHarekets.AddRange(labAnalizHareketler);
                _dbPoly.SaveChanges();
            }

            if (dosyaTipi == 2)
            {
                var numuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
                var labAnalizDurum = new SrcnLabAnalizDurumus();
                if (DosyaDurumuDegistiMi)
                {
                    _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                    {
                        DosyaAdi = "Numune Yapilabilirlik Analizleri",
                        DosyaHareketId = 99,
                        DosyaTipi = 2,
                        KayitTarihi = DateTime.Now,
                        HareketAdi = numuneYapilabilirlik.DosyaDurumAdi.ToUpper() + " Olarak Güncellenmiştir " + Kullanici.IsimSoyisim,
                        SikayetNumuneDosyaId = id
                    });
                    _dbPoly.SaveChanges();



                }


                if (numuneYapilabilirlik.DosyaDurumId == 3)
                { //Laboratuvara Teslimi Bekleniyor

                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(4);
                    //labAnalizDurum=> Analiz Kayıdı Oluşturuldu

                }
                if (numuneYapilabilirlik.DosyaDurumId == 4)
                {
                    //Laboratuvara Girişi Yapıldı

                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(4);
                    //labAnalizDurum=> Analiz Kayıdı Oluşturuldu

                }
                if (numuneYapilabilirlik.DosyaDurumId == 5)
                {
                    //Analiz Yapılıyor

                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(4);
                    //labAnalizDurum=> Analiz Yapılmaya Başlandı

                }
                if (numuneYapilabilirlik.DosyaDurumId == 6)
                {
                    //Teknisyen Analizi Tamamlandı

                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(5);
                    //labAnalizDurum=> Analiz Tamamlandı

                }

                if (numuneYapilabilirlik.DosyaDurumId<7)
                {
                    var labAnalizHareketler = new List<SrcnLabAnalizHarekets>();
                    foreach (var item in _dbPoly.SrcnLabAnalizs.Where(a => a.BagliOlduguDosyaId == numuneYapilabilirlik.NumuneYapilabilirlikDosyaId && a.DosyaTipi == 2 && a.LabAnalizDurumu != labAnalizDurum.LabAnalizDurumu).ToList())
                    {
                        item.LabAnalizDurumuAdi = labAnalizDurum.LabAnalizDurumuAdi;
                        item.LabAnalizDurumu = labAnalizDurum.LabAnalizDurumu;
                        _dbPoly.SaveChanges();
                        labAnalizHareketler.Add(new SrcnLabAnalizHarekets() { LabAnalizId = item.LabAnalizId, KayitTarihi = DateTime.Now, LabAnalizDurumu = labAnalizDurum.LabAnalizDurumu, HareketAdi = labAnalizDurum.LabAnalizDurumuAdi + " Olarak Güncellenmiştir " + Kullanici.IsimSoyisim });
                    }
                    _dbPoly.SrcnLabAnalizHarekets.AddRange(labAnalizHareketler);
                    _dbPoly.SaveChanges();
                }
             
            }

            if (dosyaTipi == 3)
            {
                var musteriSikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
                var labAnalizDurum = new SrcnLabAnalizDurumus();
                if (DosyaDurumuDegistiMi)
                {
                    _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                    {
                        DosyaAdi = "Müşteri Şikayetleri",
                        DosyaHareketId = 99,
                        DosyaTipi = 4,
                        KayitTarihi = DateTime.Now,
                        HareketAdi = musteriSikayet.DosyaDurumAdi.ToUpper() + " Olarak Güncellenmiştir " + Kullanici.IsimSoyisim,
                        SikayetNumuneDosyaId = id
                    });
                    _dbPoly.SaveChanges();



                }


                if (musteriSikayet.DosyaDurumId == 26)
                { //Laboratuvara Teslimi Bekleniyor

                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(7);
                    //labAnalizDurum=> Analiz Kayıdı Oluşturuldu

                }
                if (musteriSikayet.DosyaDurumId == 28)
                {
                    //Laboratuvara Girişi Yapıldı

                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(8);
                    //labAnalizDurum=> Analiz Kayıdı Oluşturuldu

                }
                if (musteriSikayet.DosyaDurumId == 29)
                {
                    //Analiz Yapılıyor

                    labAnalizDurum = _dbPoly.SrcnLabAnalizDurumus.Find(9);
                    //labAnalizDurum=> Analiz Yapılmaya Başlandı

                }
               

                if (musteriSikayet.DosyaDurumId < 30 && musteriSikayet.DosyaDurumId > 25)
                {
                    var labAnalizHareketler = new List<SrcnLabAnalizHarekets>();
                    foreach (var item in _dbPoly.SrcnLabAnalizs.Where(a => a.BagliOlduguDosyaId == musteriSikayet.MusteriSikayetDosyaId && a.DosyaTipi == 4 && a.LabAnalizDurumu != labAnalizDurum.LabAnalizDurumu).ToList())
                    {
                        item.LabAnalizDurumuAdi = labAnalizDurum.LabAnalizDurumuAdi;
                        item.LabAnalizDurumu = labAnalizDurum.LabAnalizDurumu;
                        _dbPoly.SaveChanges();
                        labAnalizHareketler.Add(new SrcnLabAnalizHarekets() { LabAnalizId = item.LabAnalizId, KayitTarihi = DateTime.Now, LabAnalizDurumu = labAnalizDurum.LabAnalizDurumu, HareketAdi = labAnalizDurum.LabAnalizDurumuAdi + " Olarak Güncellenmiştir " + Kullanici.IsimSoyisim });
                    }
                    _dbPoly.SrcnLabAnalizHarekets.AddRange(labAnalizHareketler);
                    _dbPoly.SaveChanges();
                }

            }
        }
        public void BilgiMailiGonder(int DosyaTipi, int DegisiklikTipi, int id)
        {
            // dosyatipi => 1 = günlük imalat, 2= numune yapılabilirlik
            // degisiklik tipi => (günlük imalat) 1= analiz, 2= analiz sonuclandı
            // degisiklik tipi => (numune yapılabilirlik) 1= durum değişikliği


            if (DosyaTipi == 1)
            {
                // dosyatipi => 1 = günlük imalat
                var gunlukItem = _dbPoly.SrcnGunlukImalatDosyas.Find(id);
                var mailGonderilecekler = new List<string>() { "kvural@polyteks.com.tr" };
                var labEmailler = _dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == 18).ToList().Select(a => a.EmailAdres).ToList();
                var BirimBilgilendirmeleri = _dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == gunlukItem.BirimId).Select(a => a.EmailAdres).ToList();
                mailGonderilecekler.AddRange(BirimBilgilendirmeleri.ToList());
                mailGonderilecekler.AddRange(labEmailler.ToList());
                var tablolar = DosyaLabAnalizTablolariOlustur(1, id);
                string mailTablelar = "";
                var Partiler = new List<string>();
                foreach (var tablo in tablolar)
                {
                    string dosyaId = tablo.LabAnaliz.BagliOlduguDosyaId.ToString();
                    string lAbAnalizId = tablo.LabAnaliz.LabAnalizId.ToString();
                    string parti = tablo.LabAnaliz.PartiNo.Trim();
                    string isEmri = tablo.LabAnaliz.RefakartKartNo.Trim();
                    string kanalNo = tablo.LabAnaliz.KanalNo.ToString();

                    var makine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == tablo.LabAnaliz.MakineId);

                    var araSonuc = string.Format(
                        "Dosya Kodu: <b>#{0}</b> Analiz Kodu: <b>{1}</b> İş Emri: <b>{2}</b></br>" +
                        " Parti: <b>{3}</b> Kanal: <b>{4}</b> Makine: <b>{5}-{6}</b>", dosyaId, lAbAnalizId,
                        isEmri, parti, kanalNo, makine.MakineNo.Trim(), makine.MakineAdi.Trim());

                    if (Partiler.Count(a => a == tablo.LabAnaliz.PartiNo.Trim().ToUpper()) == 0)
                    {
                        Partiler.Add(tablo.LabAnaliz.PartiNo.Trim().ToUpper());
                    }


                    var mailTable = araSonuc + "<table border=\"1\">";
                    var theadItems = new List<string>();
                    string theadOlustur = "<thead><tr>";
                    string tbodyOlustur = "<tbody>";
                    theadItems.Add("#");
                    foreach (var labAnalizItem in tablo.LabAnalizItemlar)
                    {
                        theadItems.Add(labAnalizItem.BobinAdi);
                    }
                    foreach (var s in theadItems)
                    {
                        theadOlustur += "<th>" + s + "</th>";
                    }
                    theadOlustur += "</tr></thead>";
                    foreach (var satir in tablo.DosyaLabAnalizTabloSatirlar)
                    {


                        var tbodyItems = new List<string>() { satir.LabAnalizCesit.LabAnalizCesitAdi, };

                        foreach (var sutun in satir.LabAnalizItemAnalizCesitSonuclari)
                        {

                            tbodyItems.Add(sutun.AnalizDegeriString);
                        }

                        tbodyOlustur += "<tr>";
                        foreach (var s in tbodyItems)
                        {
                            tbodyOlustur += "<td>" + s + "</td>";
                        }

                        tbodyOlustur += "</tr>";
                    }

                    tbodyOlustur += "</tbody>";

                    mailTable += theadOlustur;
                    mailTable += tbodyOlustur;
                    mailTable += "</table> <hr>";
                    mailTablelar += mailTable;
                }

                if (DegisiklikTipi == 1)
                {

                    string partiBilgiler = "";
                    if (mailGonderilecekler.Any())
                    {
                        foreach (var i in Partiler)
                        {
                            partiBilgiler += i;
                            if (i != Partiler.Last())
                            {
                                partiBilgiler += ", ";
                            }
                        }

                        mailTablelar += "</br>" + Kullanici.IsimSoyisim;
                        GenelDurumMailGonder(null, mailGonderilecekler, mailTablelar, partiBilgiler + " Analiz Kayıtları Oluşturuldu");
                    }
                }

                if (DegisiklikTipi == 2)
                {

                    string partiBilgiler = "";
                    if (mailGonderilecekler.Any())
                    {
                        foreach (var i in Partiler)
                        {
                            partiBilgiler += i;
                            if (i != Partiler.Last())
                            {
                                partiBilgiler += ", ";
                            }
                        }
                        GenelDurumMailGonder(null, mailGonderilecekler, mailTablelar, partiBilgiler + " Analiz Tamamlandı");
                    }
                }
            }
            if (DosyaTipi == 2)
            {
                // dosyatipi => 2= numune yapılabilirlik
                var numuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
                string CariAdi = numuneYapilabilirlik.CariAdi.Trim();
                var mailGonderilecekler = new List<string>() { "kvural@polyteks.com.tr" };
                var labEmailler = _dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == 18).ToList().Select(a => a.EmailAdres).ToList();
                if (numuneYapilabilirlik.DosyaDurumId > 6)
                {
                    // numune analizi tamamlandıktan sonra ilgili birimlere mail atılacak
                    var BirimBilgilendirmeleri = _dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == 10 || a.MailBildirimGrupId == 21).Select(a => a.EmailAdres).ToList();
                    mailGonderilecekler.AddRange(BirimBilgilendirmeleri.ToList());
                }


                mailGonderilecekler.AddRange(labEmailler.ToList());
                var tablolar = DosyaLabAnalizTablolariOlustur(2, id);

                string mailTablelar = "";

                foreach (var tablo in tablolar)
                {
                    string dosyaId = tablo.LabAnaliz.BagliOlduguDosyaId.ToString();
                    string lAbAnalizId = tablo.LabAnaliz.LabAnalizId.ToString();

                    var araSonuc = string.Format(
                        "Dosya Kodu: <b>#{0}</b> Analiz Kodu: <b>{1}</b> Cari: <b>{2}</b>", dosyaId, lAbAnalizId, CariAdi);



                    var mailTable = araSonuc + "<table border=\"1\">";
                    var theadItems = new List<string>();
                    string theadOlustur = "<thead><tr>";
                    string tbodyOlustur = "<tbody>";
                    theadItems.Add("#");
                    foreach (var labAnalizItem in tablo.LabAnalizItemlar)
                    {
                        theadItems.Add(labAnalizItem.BobinAdi);
                    }
                    foreach (var s in theadItems)
                    {
                        theadOlustur += "<th>" + s + "</th>";
                    }
                    theadOlustur += "</tr></thead>";
                    foreach (var satir in tablo.DosyaLabAnalizTabloSatirlar)
                    {


                        var tbodyItems = new List<string>() { satir.LabAnalizCesit.LabAnalizCesitAdi, };

                        foreach (var sutun in satir.LabAnalizItemAnalizCesitSonuclari)
                        {

                            tbodyItems.Add(sutun.AnalizDegeriString);
                        }

                        tbodyOlustur += "<tr>";
                        foreach (var s in tbodyItems)
                        {
                            tbodyOlustur += "<td>" + s + "</td>";
                        }

                        tbodyOlustur += "</tr>";
                    }

                    tbodyOlustur += "</tbody>";

                    mailTable += theadOlustur;
                    mailTable += tbodyOlustur;
                    mailTable += "</table> <hr>";
                    mailTablelar += mailTable;
                }

                if (DegisiklikTipi == 1)
                {

                    if (mailGonderilecekler.Any())
                    {
                        mailTablelar = "Güncel Durum: " + numuneYapilabilirlik.DosyaDurumAdi + "</br>" + mailTablelar;
                        mailTablelar += "</br>" + Kullanici.IsimSoyisim;
                        GenelDurumMailGonder(null, mailGonderilecekler, mailTablelar, CariAdi + " Numune Yapılabilirlik Bilgi Değişikliği");
                    }
                }


            }

        }

        #region analizLog CRUD
        public void LabHareketLogGuncelle(int LabAnalizId, string YeniDeger, string EskiDeger, string KullanilanAlan)
        {
            _dbPoly = new POTA_PTKSEntities();
            var aa = new SrcnLabAnalizLogs()
            {
                LabAnalizId = LabAnalizId,
                KayitTarihi = DateTime.Now,
                YeniDeger = YeniDeger,
                EskiDeger = EskiDeger,
                Aciklama = string.Format("{0} alanı değeri {1} değerinden {2} değeri olarak olarak güncellenmiştir. - {3}", KullanilanAlan, EskiDeger, YeniDeger, Kullanici.IsimSoyisim.ToString())
            };
            try
            {
                _dbPoly.SrcnLabAnalizLogs.Add(aa);
                _dbPoly.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



        }
        #endregion
        #region Metotlar

        #region Lab Hareket CRUD
        public void LabHareketSil(int LabAnalizId, string YeniDeger, string KullanilanAlan)
        {
            _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
            {
                LabAnalizId = LabAnalizId,
                LabAnalizDurumu = 7,
                KayitTarihi = DateTime.Now,
                HareketAdi = string.Format("{0} : {1} değer  olarak silinmiştir. - {2}", KullanilanAlan, YeniDeger, Kullanici.IsimSoyisim)
            });
            _dbPoly.SaveChanges();
        }
        public void LabHareketEkle(int LabAnalizId, string YeniDeger, string KullanilanAlan)
        {
            _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
            {
                LabAnalizId = LabAnalizId,
                LabAnalizDurumu = 7,
                KayitTarihi = DateTime.Now,
                HareketAdi = string.Format("{0} : {1} değer olarak olarak eklenmiştir. - {2}", KullanilanAlan, YeniDeger, Kullanici.IsimSoyisim)
            });
            _dbPoly.SaveChanges();
        }
        public void LabHareketGuncelle(int LabAnalizId, string EskiDeger, string YeniDeger, string KullanilanAlan)
        {
            _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
            {
                LabAnalizId = LabAnalizId,
                LabAnalizDurumu = 7,
                KayitTarihi = DateTime.Now,
                HareketAdi = string.Format("{0} : {1} olan değer {0} : {2} olarak güncellenmiştir. - {3}", KullanilanAlan, EskiDeger, YeniDeger, Kullanici.IsimSoyisim)
            });
            _dbPoly.SaveChanges();
        }
        #endregion
        public LaboratuvarModel LaboratuvarModeliGetir(int LabGrupId)
        {
            var model = new LaboratuvarModel();
            var labGrup = _dbPoly.SrcnLabGrups.Find(LabGrupId);
            model.LabGrup = labGrup;
            model.LabAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 0).OrderBy(a => a.LabAnalizCesitAdi).ToList();
            model.LabGrubuAnalizleri = _dbPoly.SrcnLabGrupAnalizCesits.Where(a => a.LabGrupId == LabGrupId).ToList();
            model.UretimHatlari = new SelectList(Helpers.Helper.UretimHatlariGetir(), "Id", "Tanim");

            var makListe = new List<DropItem> { new DropItem() { Id = "0", Tanim = "Bölüm Seçiniz" } };
            model.Makineler = new SelectList(makListe, "Id", "Tanim");
            foreach (var item in model.LabAnalizCesitleri)
            {
                if (model.LabGrubuAnalizleri.Count(a => a.LabAnalizCesitId == item.LabAnalizCesitId) == 0)
                {
                    model.LabGrubuAnalizleri.Add(new SrcnLabGrupAnalizCesits()
                    {
                        LabGrupId = labGrup.LabGrupId,
                        LabGrupAdi = labGrup.LabGrupAdi,
                        LabAnalizCesitAdi = item.LabAnalizCesitAdi,
                        LabAnalizCesitId = item.LabAnalizCesitId,
                        SeciliMi = false

                    });
                }
            }

            model.LabGrubuAnalizleri = model.LabGrubuAnalizleri.OrderBy(a => a.LabAnalizCesitAdi).ToList();
            model.Kullanici = Kullanici;

            model.MalzemeLabAnalizModeller.Add(new MalzemeLabAnalizModel() { MalzemeAdi = "İPLİK", MalzemeTipi = 0, LabAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 0).OrderBy(a => a.LabAnalizCesitAdi).ToList() });
            model.MalzemeLabAnalizModeller.Add(new MalzemeLabAnalizModel() { MalzemeAdi = "KUMAŞ", MalzemeTipi = 1, LabAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 1).OrderBy(a => a.LabAnalizCesitAdi).ToList() });

            var YardimciTesisKontNoktKategoriler =
                _dbPoly.SrcnYardimciTesisKontrolNoktas.Select(a => a.Kategori).Distinct().OrderBy(a => a).ToList();

            foreach (var k in YardimciTesisKontNoktKategoriler)
            {
                model.YardimciTesisKategoriModeller.Add(new YardimciTesisKategoriModel()
                {
                    Kategori = k,
                    YardimciTesisKontrolNoktalari = _dbPoly.SrcnYardimciTesisKontrolNoktas.Where(a => a.Kategori == k).OrderBy(a => a.YardimciTesisKontrolNoktaId).ToList()

                });
            }
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            return model;
        }
        public LaboratuvarModel LaboratuvarModeliKaydet(LaboratuvarModel model)
        {
            var labAnaliz = model.LabAnaliz;
            var labGrup = _dbPoly.SrcnLabGrups.Find(model.LabGrup.LabGrupId);
            bool SorunVarmi = false;
            string mesaj = "";
            if (labAnaliz.IstenenTerminTarihi == null)
            {
                SorunVarmi = true;
                mesaj += "Termin Tarihi Belirlemediniz</br>";
            }

            if (labGrup.AnalizTipi == 0)
            {

                // imalat kontrol
                if (labAnaliz.AnalizYapilacakBobinSayisi == 0)
                {
                    SorunVarmi = true;
                    mesaj += "Analiz Yapılacak Bobin Sayısını Belirleyiniz</br>";
                }

                if (labAnaliz.ImalatAnalizYapilmaTipi == 0)
                {
                    if (labAnaliz.RefakartKartNo != null)
                    {
                        SorunVarmi = true;
                        mesaj += "Analiz Tipini 'Deneme' olarak belirlediniz fakat iş emri numarası girdiniz </br>";
                    }

                }
                else
                {
                    if (labAnaliz.RefakartKartNo == null)
                    {
                        SorunVarmi = true;
                        mesaj += "Analiz Tipini seçiminize göre iş emri numarası girmeniz gerekmetedir. </br>";
                    }
                    else
                    {
                        if (_dbPoly.RefakatKarti.Any(a => a.RefakatNo == labAnaliz.RefakartKartNo && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null))
                        {
                            SorunVarmi = true;
                            mesaj +=
                                "İş Emri numarasına uygun kayıt bulunamamıştır ya da bu iş emri işletme tarafından kapatılmıştır. Kontrol Ediniz!!! </br>";
                        }
                        else
                        {
                            var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == labAnaliz.RefakartKartNo && a.IslemSiraNo == 100 &&
                                a.IslemBitisTarihi == null);

                        }
                    }
                }

            }
            return model;
        }
        #endregion


        #region GÜNLÜK İMALAT
        public ActionResult GunlukImalatDosyaPdfIndir(int id)
        {
            return GunlukImalatDosyaPdfOlustur(id);
        }

        public ActionResult GunlukImalatDosyalar(int? id, int? id2)
        {
            TempData["Gizle"] = "gizle";
            // id=birim;
            //id2= Dosy aDurumu 
            _dbPoly = new POTA_PTKSEntities();
            var model = new LaboratuvarModel();
            int BirimId = 0;
            int DosyaDurumId = 0;

            var DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 1)
                .OrderBy(a => a.DosyaDurumId).ToList();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = Birimler;
            model.DosyaDurumlari = DosyaDurumlari;

            if (id != null)
            {
                BirimId = Convert.ToInt32(id);
                model.Birim = _dbPoly.SrcnFabrikaBirims.Find(BirimId);
            }
            if (id2 != null)
            {
                DosyaDurumId = Convert.ToInt32(id2);
                //  model.LabAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(idd2);
                model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(DosyaDurumId);
            }

            if (BirimId == 0 && DosyaDurumId == 0)
            {
                // analiz bekleyenler yapilacaklar


                // Analiz Kayıdı Oluşturuldu ve Analiz Yapılmaya Başlandı durumları
                model.GunlukImalatDosyalar = _dbPoly.SrcnGunlukImalatDosyas.Where(a => a.DosyaDurumId == 11 || a.DosyaDurumId == 12).OrderByDescending(a => a.KayitTarihi).ToList();
            }
            else
            {
                var dosyalar = new List<SrcnGunlukImalatDosyas>();

                if (BirimId > 0)
                {
                    // birime göre
                    dosyalar = _dbPoly.SrcnGunlukImalatDosyas.Where(a => a.BirimId == BirimId).OrderByDescending(a => a.KayitTarihi).ToList();
                }

                if (DosyaDurumId == 0)
                {
                    // tüm durumlar

                    if (dosyalar.Any())
                    {
                        dosyalar = dosyalar.OrderByDescending(a => a.KayitTarihi).ToList();
                    }
                    else
                    {
                        dosyalar = _dbPoly.SrcnGunlukImalatDosyas.OrderByDescending(a => a.KayitTarihi).ToList();
                    }

                }
                else
                {
                    if (dosyalar.Any())
                    {
                        dosyalar = dosyalar.Where(a => a.DosyaDurumId == DosyaDurumId).OrderByDescending(a => a.KayitTarihi).ToList();
                    }
                    else
                    {
                        dosyalar = _dbPoly.SrcnGunlukImalatDosyas.Where(a => a.DosyaDurumId == DosyaDurumId).OrderByDescending(a => a.KayitTarihi).ToList();
                    }

                }
                model.GunlukImalatDosyalar = dosyalar;

            }


            return View(model);
        }

        public ActionResult GunlukImalatMailAt(int id)
        {

            BilgiMailiGonder(1, 2, id);
            return RedirectToAction("GunlukImalatDosyalar", "Laboratuvar", new { id = 3, id2 = 13 });
        }

        public ActionResult GunlukImalatDosyaDuzenle(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 1).OrderBy(a => a.DosyaDurumId).ToList();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.AnalizDurumlari = _dbPoly.SrcnLabAnalizDurumus.Where(a => a.DosyaTipi == 1)
                .OrderBy(a => a.LabAnalizDurumuAdi).ToList();
            model.Birimler = Birimler;
            model.DosyaDurumlari = DosyaDurumlari;

            var gunlukDosya = _dbPoly.SrcnGunlukImalatDosyas.Find(id);
            model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(gunlukDosya.DosyaDurumId);
            model.Birim = _dbPoly.SrcnFabrikaBirims.Find(gunlukDosya.BirimId);
            model.GunlukImalatDosya = gunlukDosya;

            model.DosyaLabAnalizTablolar = DosyaLabAnalizTablolariOlustur(1, gunlukDosya.GunlukImalatDosyaId);
            return View(model);
        }
        [HttpPost]
        public ActionResult GunlukImalatDosyaDuzenle(LaboratuvarModel model)
        {

            bool DosyaDurumuDegistiMi = false;
            var gunlukImalat = model.GunlukImalatDosya;
            var editgunlukImalat = _dbPoly.SrcnGunlukImalatDosyas.Find(model.GunlukImalatDosya.GunlukImalatDosyaId);

            int DegisiklikVarmi = 0;
            var KayitGuncellemeleri = new List<DropItem>();
            foreach (var tablo in model.DosyaLabAnalizTablolar)
            {
                foreach (var tabloSatir in tablo.DosyaLabAnalizTabloSatirlar)
                {
                    foreach (var item in tabloSatir.LabAnalizItemAnalizCesitSonuclari.Where(a => a.AnalizDegeriString != null).ToList())
                    {
                        if (DegisiklikVarmi != 2 && DegisiklikVarmi != 1)
                        {
                            DegisiklikVarmi = 1;
                        }
                        var editItem = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Find(item.LabAnalizItemAnalizCesitId);
                        if (editItem.AnalizDegeriString != item.AnalizDegeriString && editItem.AnalizDegeriString != null)
                        {
                            DegisiklikVarmi = 2;
                            KayitGuncellemeleri.Add(new DropItem() { Tanim = item.AnalizDegeriString, DigerTanim = editItem.AnalizDegeriString, IdInt = editItem.LabAnalizItemAnalizCesitId });
                        }
                        editItem.AnalizDegeriString = item.AnalizDegeriString;
                        _dbPoly.SaveChanges();
                    }
                }
            }

            if ((DegisiklikVarmi > 0 && gunlukImalat.DosyaDurumId == 11))
            {
                // değer girildi fakat analiz yapılıyor olarak belirlenmedi
                var araDosyaDurum = _dbPoly.SrcnDosyaDurums.Find(12);
                // analiz yapılmaya başlandı olarak kaydedilecek

                editgunlukImalat.DosyaDurumAdi = araDosyaDurum.DosyaDurumAdi;
                editgunlukImalat.DosyaDurumId = araDosyaDurum.DosyaDurumId;
                _dbPoly.SaveChanges();
                BilgiMailiGonder(1, 1, editgunlukImalat.GunlukImalatDosyaId);
                DosyaDurumuDegistiMi = true;
            }
            else
            {
                if (gunlukImalat.DosyaDurumId == 12)
                {
                    if (editgunlukImalat.DosyaDurumId != 12)
                    {
                        DosyaDurumuDegistiMi = true;
                    }
                    var araDosyaDurum = _dbPoly.SrcnDosyaDurums.Find(12);
                    editgunlukImalat.DosyaDurumAdi = araDosyaDurum.DosyaDurumAdi;
                    editgunlukImalat.DosyaDurumId = araDosyaDurum.DosyaDurumId;
                    _dbPoly.SaveChanges();
                }
                else
                {
                    if (DegisiklikVarmi == 2)
                    {
                        // girilen depeğerlerin bazılarında güncellemeler yapıldı.

                        // guncellemeler

                        DosyaDurumuDegistiMi = true;
                        BilgiMailiGonder(1, 3, editgunlukImalat.GunlukImalatDosyaId);
                    }
                }
            }
            if (gunlukImalat.DosyaDurumId == 13)
            {
                //Analiz Sonuçlandı

                if (editgunlukImalat.DosyaDurumId != 13)
                {
                    DosyaDurumuDegistiMi = true;
                    var araDosyaDurum = _dbPoly.SrcnDosyaDurums.Find(13);
                    // analiz sonuclandı

                    editgunlukImalat.DosyaDurumAdi = araDosyaDurum.DosyaDurumAdi;
                    editgunlukImalat.DosyaDurumId = araDosyaDurum.DosyaDurumId;
                    _dbPoly.SaveChanges();
                    BilgiMailiGonder(1, 2, editgunlukImalat.GunlukImalatDosyaId);
                }
            }
            TempDataCRUDOlustur(2);
            DosyaLabAnalizDurumGuncelle(1, gunlukImalat.GunlukImalatDosyaId, DosyaDurumuDegistiMi);
            if (KayitGuncellemeleri.Any())
            {
                KayitliBilgiDegisimMailiGonder(1, KayitGuncellemeleri);
            }

            return RedirectToAction("GunlukImalatDosyaDuzenle", "Laboratuvar",
                new { id = model.GunlukImalatDosya.GunlukImalatDosyaId });
        }
        public ActionResult GunlukImalatDosyaSil(int id)
        {
            var gunlukDosya = _dbPoly.SrcnGunlukImalatDosyas.Find(id);
            var labAnalizler = _dbPoly.SrcnLabAnalizs
                .Where(a => a.ImalatAnalizYapilmaTipi == 1 && a.BagliOlduguDosyaId == id)
                .Select(a => a.LabAnalizId).ToList();

            _dbPoly.SrcnGunlukImalatDosyas.Remove(gunlukDosya);
            _dbPoly.SaveChanges();

            foreach (var i in labAnalizler)
            {

                LabAnalizVeritabanındanSil(i);
            }
            TempDataCRUDOlustur(3);
            return RedirectToAction("GunlukImalatDosyalar");
        }

        public ActionResult GunlukImalatLabTeknisyeniTalebi(int id)
        {
            Session["lab"] = "teknisyen";
            return RedirectToAction("PartinoyaGoreGunlukImalatTalebi", "Imalat", new { id = id });
        }

        #endregion


        #region Numune Yapılabilirlik
        public ActionResult NumuneYapilabilirlikDosyaOnaylar(int id=0)
        {
            TempData["Gizle"] = "gizle";


            var model = new LaboratuvarModel();
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => (a.DosyaTipi == 2) && (a.DosyaDurumId == 7 || a.DosyaDurumId == 8)).OrderBy(a => a.DosyaDurumId).ToList();
            if (id == null)
            {

                // TEKNİSYEN TAMAMLADI DURUMU =6 BU YÜZDEN ONAYA ONLARI GETİRYORUZ
                // analiz onay yapilacaklar
                model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == 7);
                model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == 6).OrderByDescending(a => a.KayitTarihi).ToList();
            }
            else
            {
                int idd = Convert.ToInt32(id);
                if (idd==0)
                {
                    idd = 7;
                }
                if (idd == 7)
                {
                    model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == 7);
                    idd = 6;
                }
                else
                {
                    model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == idd);
                }
       
                model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == idd).OrderByDescending(a => a.KayitTarihi).ToList();
                // analiz

            }


            return View(model);
        }
        public ActionResult NumuneYapilabilirlikDosyaOnayDuzenle(int id)
        {
            var DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => (a.DosyaTipi == 2) && (a.DosyaDurumId == 6 || a.DosyaDurumId == 7 || a.DosyaDurumId == 8)).OrderBy(a => a.DosyaDurumId).ToList();

            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var numuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            model.NumuneYapilabilirlikDosya = numuneYapilabilirlik;
            model.DosyaNumuneLabAnalizTablolar = DosyaNumuneLabAnalizTablolariOlustur(id);
            model.DosyaDurumlari = DosyaDurumlari;

            return View(model);
        }

        [HttpPost]
        public ActionResult NumuneYapilabilirlikDosyaOnayDuzenle(LaboratuvarModel model)
        {
            var numune = model.NumuneYapilabilirlikDosya;
            var editNumune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(numune.NumuneYapilabilirlikDosyaId);
            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(numune.DosyaDurumId);
            if (numune.DosyaDurumId == 7)
            {
                dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(6);
            }

            if (editNumune.DosyaDurumId != dosyaDurum.DosyaDurumId)
            {
                editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
                editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
                _dbPoly.SaveChanges();
                DosyaLabAnalizDurumGuncelle(2, editNumune.NumuneYapilabilirlikDosyaId, true);
                //BilgiMailiGonder(2, 1, numune.NumuneYapilabilirlikDosyaId);
            }

            TempDataCRUDOlustur(2);
            return RedirectToAction("NumuneYapilabilirlikDosyaOnaylar", "Laboratuvar",
                new { id = model.NumuneYapilabilirlikDosya.DosyaDurumId });
        }

        public ActionResult NumuneYapilabilirlikDosyalar(int? id)
        {
            TempData["Gizle"] = "gizle";

            var model = new LaboratuvarModel();
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 2 && a.DosyaDurumId < 7).ToList();
            if (id == null)
            {
                // analiz bekleyenler yapilacaklar
                model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId < 7).OrderByDescending(a => a.KayitTarihi).ToList();
            }
            else
            {
                int idd = Convert.ToInt32(id);
                model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == idd);
                model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == idd).OrderByDescending(a => a.KayitTarihi).ToList();
                // analiz sonuçlananlar

            }


            return View(model);
        }
        public ActionResult NumuneYapilabilirlikDosyaDuzenle(int id)
        {
            var DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 2 && a.DosyaDurumId < 7).OrderBy(a => a.DosyaDurumId).ToList();



            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var numuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            model.NumuneYapilabilirlikDosya = numuneYapilabilirlik;
            model.DosyaNumuneLabAnalizTablolar = DosyaNumuneLabAnalizTablolariOlustur(id);
            model.DosyaDurumlari = DosyaDurumlari;

            return View(model);
        }
        [HttpPost]
        public ActionResult NumuneYapilabilirlikDosyaDuzenle(LaboratuvarModel model)
        {

            var eklenecekListe = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
            var silinecekListe = new List<int>();
            var guncellenecek = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
            foreach (var tablo in model.DosyaNumuneLabAnalizTablolar)
            {
                foreach (var tabloSatir in tablo.AtkiTablo.DosyaLabAnalizTabloSatirlar)
                {

                    eklenecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId == 0 && a.AnalizDegeriString != null).ToList());
                    silinecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString == null).Select(a => a.LabAnalizItemAnalizCesitId).ToList());
                    guncellenecek.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString != null).ToList());

                }

                foreach (var tabloSatir in tablo.CozguTablo .DosyaLabAnalizTabloSatirlar)
                {

                    eklenecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId == 0 && a.AnalizDegeriString != null).ToList());
                    silinecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString == null).Select(a => a.LabAnalizItemAnalizCesitId).ToList());
                    guncellenecek.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString != null).ToList());

                }
            }

            var silListe = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                .Where(a => silinecekListe.Any(b => b == a.LabAnalizItemAnalizCesitId)).ToList();
            _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.RemoveRange(silListe);
            _dbPoly.SaveChanges();
            var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0 && a.Sira > 0).OrderBy(a => a.Sira).ToList();
            foreach (var item in eklenecekListe)
            {
                var labItem = _dbPoly.SrcnLabAnalizItems.Find(item.LabAnalizItemId);
                var analizCesit = TumIplikAnalizCesitleri.First(a => a.LabAnalizCesitId == item.LabAnalizCesitId);
                _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                {
                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                    AnalizDegeriString = item.AnalizDegeriString,
                    LabAnalizItemId = labItem.LabAnalizItemId,
                    DegerTipi = analizCesit.LabAnalizCesitId,
                    IplikAdi = labItem.BobinAdi,
                    IplikNo = labItem.IplikNo,
                    MakinePozisyonNo = labItem.MakinePozisyonNo,
                    LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi,
                    LabAnalizCesitAdiEng = analizCesit.LabAnalizCesitAdiEng,
                });
                _dbPoly.SaveChanges();
            }
            foreach (var item in guncellenecek)
            {
                if (_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Any(a => a.LabAnalizItemAnalizCesitId == item.LabAnalizItemId && a.AnalizDegeriString != item.AnalizDegeriString))
                {
                    var editItem = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Find(item.LabAnalizItemAnalizCesitId);
                    editItem.AnalizDegeriString = item.AnalizDegeriString;
                    _dbPoly.SaveChanges();
                }
            }

            var numune = model.NumuneYapilabilirlikDosya;
            var editNumune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(numune.NumuneYapilabilirlikDosyaId);

            if (numune.DosyaDurumId == 0)
            {
                numune.DosyaDurumId = 4;
            }

            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(numune.DosyaDurumId);
            editNumune.LabYorumu = model.NumuneYapilabilirlikDosya.LabYorumu;
            if (editNumune.DosyaDurumId != dosyaDurum.DosyaDurumId)
            {
                editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
                editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
                _dbPoly.SaveChanges();

                DosyaLabAnalizDurumGuncelle(2, editNumune.NumuneYapilabilirlikDosyaId, true);
                //BilgiMailiGonder(2, 1, numune.NumuneYapilabilirlikDosyaId);
            }



            TempDataCRUDOlustur(2);
            return RedirectToAction("NumuneYapilabilirlikDosyaDuzenle", "Laboratuvar",
                new { id = model.NumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId });
        }
        #endregion

        public ActionResult Index()
        {
            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();

            model.SrcnLabGruplari = _dbPoly.SrcnLabGrups.ToList();
            return View(model);
        }
        public ActionResult LabGrupLabCesitTanimlamalari(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = LaboratuvarModeliGetir(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult LabGrupLabCesitTanimlamalari(LaboratuvarModel model)
        {
            var labGrup = _dbPoly.SrcnLabGrups.Find(model.LabGrup.LabGrupId);
            var SilinecekLabGrubuAnalizleri = model.LabGrubuAnalizleri
                .Where(a => a.SeciliMi == false && a.LabGrupCesitId != 0).ToList();
            var EklenecekLabGrubuAnalizleri = model.LabGrubuAnalizleri
                .Where(a => a.SeciliMi == true && a.LabGrupCesitId == 0).ToList();

            var SilListe = new List<SrcnLabGrupAnalizCesits>();
            var EkleListe = new List<SrcnLabGrupAnalizCesits>();
            foreach (var item in SilinecekLabGrubuAnalizleri)
            {

                SilListe.Add(_dbPoly.SrcnLabGrupAnalizCesits.Find(item.LabGrupCesitId));
            }
            foreach (var item in EklenecekLabGrubuAnalizleri)
            {
                var analizCesit = _dbPoly.SrcnLabAnalizCesits.Find(item.LabAnalizCesitId);
                EkleListe.Add(new SrcnLabGrupAnalizCesits()
                {
                    LabGrupId = labGrup.LabGrupId,
                    LabGrupAdi = labGrup.LabGrupAdi,
                    SeciliMi = true,
                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                    LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi

                });
            }
            _dbPoly.SrcnLabGrupAnalizCesits.RemoveRange(SilListe.ToList());
            _dbPoly.SaveChanges();
            _dbPoly.SrcnLabGrupAnalizCesits.AddRange(EkleListe.ToList());
            _dbPoly.SaveChanges();
            TempDataOlustur("Güncelleme işlemi yapılmıştır", true);
            return RedirectToAction("LabGrupLabCesitTanimlamalari", "Laboratuvar", new { id = model.LabGrup.LabGrupId });

        }
        public ActionResult LaboratuvarGorevSecimi(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = LaboratuvarModeliGetir(id);
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(id);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            return View(model);
        }
        public ActionResult LaboratuvarAnalizIstegi(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = LaboratuvarModeliGetir(id);

            return View(model);
        }
        [HttpPost]
        public ActionResult LaboratuvarAnalizIstegi(LaboratuvarModel model)
        {
            return View(model);
        }
        public ActionResult YapilacakAnalizGrupTipineDon(int id)
        {
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);

            var anaLabGrup =
                _dbPoly.SrcnLabGrups.First(a => a.UstLabGrupId == 0 && a.AnalizTipi == labAnaliz.AnalizTipi);
            return RedirectToAction("YapilacakAnalizGrupTipi", "Laboratuvar",
                new { id = anaLabGrup.LabGrupId, id2 = labAnaliz.LabAnalizDurumu });
        }
        #region Yapılacak Analizler
        public ActionResult YapilacakAnalizler()
        {
            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var Anagruplar = _dbPoly.SrcnLabGrups.Where(a => a.UstLabGrupId == 0).OrderBy(a => a.LabGrupAdi).ToList();

            var Idler = Helpers.Helper.LaboratuvarYapilacakAnalizDurumIdler();
            var YapilacakAnalizDurumlari = new List<SrcnLabAnalizDurumus>();
            foreach (var i in Idler)
            {
                YapilacakAnalizDurumlari.Add(_dbPoly.SrcnLabAnalizDurumus.First(a => a.LabAnalizDurumu == i));
            }

            foreach (var labgrp in Anagruplar)
            {
                var araItem = new LaboratuvarIsModel();
                araItem.LabGrup = labgrp;
                foreach (var analizDurum in YapilacakAnalizDurumlari)
                {
                    araItem.LaboratuvarIsModelItemlar.Add(new LaboratuvarIsModelItem()
                    {
                        LabAnalizDurum = analizDurum,
                        Adet = _dbPoly.SrcnLabAnalizs.Count(a => a.LabAnalizDurumu == analizDurum.LabAnalizDurumu && a.AnalizTipi == labgrp.AnalizTipi)
                    });
                }

                araItem.ToplamAdet = araItem.LaboratuvarIsModelItemlar.Sum(a => a.Adet);
                model.LaboratuvarIsModeller.Add(araItem);

            }
            return View(model);
        }
        public ActionResult YapilacakAnalizGrupTipi(int id, int id2)
        {
            var labgrup = _dbPoly.SrcnLabGrups.Find(id);
            var LabAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(id2);

            var model = new LaboratuvarModel
            {
                LabAnalizleri = _dbPoly.SrcnLabAnalizs
                .Where(a => a.AnalizTipi == labgrup.AnalizTipi &&
                            a.LabAnalizDurumu == LabAnalizDurumu.LabAnalizDurumu)
                .OrderBy(a => a.KayitTarihi).ToList(),
                LabGrup = labgrup,
                LabAnalizDurumu = LabAnalizDurumu
            };
            return View(model);
        }
        #endregion
        public ActionResult YapilacakAnalizYonlendir(int id)
        {
            _dbPoly = new POTA_PTKSEntities();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            if (labAnaliz.AnalizTipi == 3)
            {
                return RedirectToAction("YapilacakAnalizNumuneDetay", "Laboratuvar", new { id = id });
            }
            if (labAnaliz.AnalizTipi == 0)
            {
                return RedirectToAction("YapilacakAnalizGunlukImalatKontrolDetay", "Laboratuvar", new { id = id });
            }
            if (labAnaliz.AnalizTipi == 4)
            {
                return RedirectToAction("YapilacakAnalizSuAnaliziKontrolDetay", "Laboratuvar", new { id = id });
            }
            if (labAnaliz.AnalizTipi == 2)
            {
                return RedirectToAction("YapilacakAnalizMusteriSikayet", "Laboratuvar", new { id = id });
            }
            TempDataOlustur("Bilinmeyen Bir Hata Oluştu Bilgi İşlem ile irtibata geçiniz", false);
            return RedirectToAction("Index", "Home");
        }

        #region  Yapilacak Analiz-Günlük İmalat
        public ActionResult YapilacakAnalizGunlukImalatKontrolDetay(int id)
        {
            _dbPoly = new POTA_PTKSEntities();
            var model = new LaboratuvarModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            model.LabAnaliz = labAnaliz;
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(labAnaliz.LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            model.LabAnalizDurumlari = _dbPoly.SrcnLabAnalizDurumus.Where(a => a.LabAnalizDurumu < 6 && a.LabAnalizDurumu >= labAnaliz.LabAnalizDurumu).ToList();
            if (model.LabAnaliz.PlanlananTerminTarihi != null)
            {
                model.PlanlananTarih = model.LabAnaliz.PlanlananTerminTarihi.Value.ToString("dd.MM.yyyy");
            }

            if (_dbPoly.SrcnLabAnalizItems.Count(a => a.LabAnalizId == id) == 0)
            {
                GunlukImalatAnalizItemOlustur(id);
            }
            return View(model);

        }
        [HttpPost]
        public ActionResult YapilacakAnalizGunlukImalatKontrolDetay(LaboratuvarModel model)
        {
            _dbPoly = new POTA_PTKSEntities();
            var labAnaliz = model.LabAnaliz;
            var editlabAnaliz = _dbPoly.SrcnLabAnalizs.Find(model.LabAnaliz.LabAnalizId);
            #region Giriş Kontrol

            bool sorunVarmi = false;
            var mesaj = "";
            if (model.PlanlananTarih == null)
            {
                mesaj = "Planlanan Termin Tarihini Girmediniz. </br>";
                sorunVarmi = true;
            }


            #endregion
            if (sorunVarmi)
            {
                TempDataOlustur(mesaj, false);
            }
            else
            {
                labAnaliz.PlanlananTerminTarihi = Convert.ToDateTime(model.PlanlananTarih);
                var labAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(labAnaliz.LabAnalizDurumu);
                if (editlabAnaliz.PlanlananTerminTarihi == null)
                {
                    LabHareketGuncelle(editlabAnaliz.LabAnalizId, "Belirlenmedi",
                        model.PlanlananTarih, "Planlanan Termin Tarihi");
                }
                else
                {
                    if (labAnaliz.PlanlananTerminTarihi != editlabAnaliz.PlanlananTerminTarihi)
                    {
                        LabHareketGuncelle(editlabAnaliz.LabAnalizId,
                            editlabAnaliz.PlanlananTerminTarihi.Value.ToShortDateString(),
                            model.PlanlananTarih, "Planlanan Termin Tarihi");
                    }
                }
                if (editlabAnaliz.LabAnalizDurumu != labAnaliz.LabAnalizDurumu)
                {
                    LabHareketGuncelle(editlabAnaliz.LabAnalizId, editlabAnaliz.LabAnalizDurumuAdi,
                        labAnalizDurumu.LabAnalizDurumuAdi, "Analiz Durumu");
                }
                editlabAnaliz.PlanlananTerminTarihi = labAnaliz.PlanlananTerminTarihi;
                editlabAnaliz.LabAnalizDurumu = labAnalizDurumu.LabAnalizDurumu;
                editlabAnaliz.LabAnalizDurumuAdi = labAnalizDurumu.LabAnalizDurumuAdi;
                _dbPoly.SaveChanges();
            }
            return RedirectToAction("YapilacakAnalizGunlukImalatKontrolDetay", "Laboratuvar", new { id = labAnaliz.LabAnalizId });
        }
        #endregion
        #region  Yapilacak Analiz-Numune Detay

        public ActionResult YapilacakAnalizNumuneDetayMatris(int id)
        {
            _dbPoly = new POTA_PTKSEntities();
            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            model.LabAnaliz = labAnaliz;
            model.LabGrup = _dbPoly.SrcnLabGrups.Find(labAnaliz.LabGrupId);
            if (model.LabAnaliz.PlanlananTerminTarihi != null)
            {
                model.PlanlananTarih = model.LabAnaliz.PlanlananTerminTarihi.Value.ToString("dd.MM.yyyy");
            }

            model.NumuneYapilabilirlikDosya = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(labAnaliz.BagliOlduguDosyaId);

            var LabAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId)
                .OrderBy(a => a.BobinAdi).ToList();

            model.LabAnalizDurumlari = _dbPoly.SrcnLabAnalizDurumus.Where(a => a.LabAnalizDurumu < 6 && a.LabAnalizDurumu >= labAnaliz.LabAnalizDurumu).ToList();

            var analizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0).OrderBy(a => a.LabAnalizCesitAdi).ToList();
            for (int i = 0; i < 5; i++)
            {
                string ipAdi = "ANA İPLİK";
                if (i > 0)
                {
                    ipAdi = "İPLİK- " + i;
                }
                model.Theadler.Add(ipAdi);
            }
            foreach (var analizCesit in analizCesitleri)
            {
                var araItem = new LabSatirItem();
                for (int i = 0; i < 5; i++)
                {
                    string ipAdi = "ANA İPLİK";
                    if (i > 0)
                    {
                        ipAdi = "İPLİK- " + i;
                    }

                    araItem.LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi;
                    araItem.LabAnalizCesitId = analizCesit.LabAnalizCesitId;
                    araItem.LabAnalizItemAnalizCesitSonuclari.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                    {
                        LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                        IplikNo = i
                    });
                }
                model.LabSatirItemlar.Add(araItem);


            }
            return View(model);
        }

        public ActionResult YapilacakAnalizNumuneDetay(int id)
        {
            _dbPoly = new POTA_PTKSEntities();
            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            model.LabAnaliz = labAnaliz;
            model.LabGrup = _dbPoly.SrcnLabGrups.Find(labAnaliz.LabGrupId);
            if (model.LabAnaliz.PlanlananTerminTarihi != null)
            {
                model.PlanlananTarih = model.LabAnaliz.PlanlananTerminTarihi.Value.ToString("dd.MM.yyyy");
            }

            model.NumuneYapilabilirlikDosya = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(labAnaliz.BagliOlduguDosyaId);
            var LabAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId)
                .OrderBy(a => a.BobinAdi).ToList();

            model.LabAnalizDurumlari = _dbPoly.SrcnLabAnalizDurumus.Where(a => a.LabAnalizDurumu < 6 && a.LabAnalizDurumu >= labAnaliz.LabAnalizDurumu).ToList();

            foreach (var item in LabAnalizItems)
            {
                model.LabAnalizItemAnalizCesitSonuclari.AddRange(_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Where(a => a.LabAnalizItemId == item.LabAnalizItemId).ToList());
            }
            model.LabAnalizItemAnalizCesitSonuclari =
                model.LabAnalizItemAnalizCesitSonuclari.OrderBy(a => a.LabAnalizCesitAdi).ToList();

            var analizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0).OrderBy(a => a.LabAnalizCesitAdi).ToList();
            string ipAdi = "ANA BOBİN";
            for (int i = 0; i < 9; i++)
            {
                var sonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                if (i != 0)
                {
                    ipAdi = "İplik - " + i.ToString();
                }
                var araitem = new BobinIplikModelItem()
                {
                    SeciliMi = false,
                    BobinAdi = ipAdi,
                    IplikNo = i,
                };
                if (LabAnalizItems.Any(a => a.IplikNo == i))
                {
                    var item = LabAnalizItems.First(a => a.IplikNo == i);
                    araitem.SeciliMi = true;
                    sonuclar = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                        .Where(a => a.LabAnalizItemId == item.LabAnalizItemId).ToList();
                }

                foreach (var cesit in analizCesitleri.AsReadOnly().ToList())
                {
                    if (sonuclar.Any(a => a.LabAnalizCesitId == cesit.LabAnalizCesitId))
                    {
                        cesit.SeciliMi = true;
                    }
                    else
                    {
                        cesit.SeciliMi = false;
                    }
                    araitem.LabAnalizCesitleri.Add(new SrcnLabAnalizCesits()
                    {
                        LabAnalizCesitId = cesit.LabAnalizCesitId,
                        LabAnalizCesitAdi = cesit.LabAnalizCesitAdi,
                        SeciliMi = cesit.SeciliMi,
                        MalzemeTipi = cesit.MalzemeTipi,
                        DegerTipi = cesit.DegerTipi,
                        MalzemeAdi = cesit.MalzemeAdi
                    });
                }
                model.BobinIplikModelItemlar.Add(araitem);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult YapilacakAnalizNumuneDetay(LaboratuvarModel model)
        {
            _dbPoly = new POTA_PTKSEntities();
            var numune =
                _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(model.NumuneYapilabilirlikDosya
                    .NumuneYapilabilirlikDosyaId);
            var labAnaliz = model.LabAnaliz;
            var editlabAnaliz = _dbPoly.SrcnLabAnalizs.Find(model.LabAnaliz.LabAnalizId);

            #region Giriş Kontrol

            bool sorunVarmi = false;
            var mesaj = "";
            if (model.PlanlananTarih == null)
            {
                mesaj = "Planlanan Termin Tarihini Girmediniz. </br>";
                sorunVarmi = true;
            }

            if (labAnaliz.LabAnalizDurumu > 3 && model.BobinIplikModelItemlar.Count(a => a.SeciliMi) == 0)
            {
                mesaj += "Analiz Yapılacak İplikleri Seçmediniz. </br>";
                sorunVarmi = true;
            }

            int say = 0;
            if (labAnaliz.LabAnalizDurumu > 3)
            {
                foreach (var item in model.BobinIplikModelItemlar.Where(a => a.SeciliMi))
                {
                    if (item.LabAnalizCesitleri.Count(a => a.SeciliMi) == 0)
                    {
                        say++;
                    }
                }
                if (say > 0)
                {
                    sorunVarmi = true;
                }
            }



            #endregion
            if (sorunVarmi)
            {
                TempDataOlustur(mesaj, false);
            }
            else
            {
                labAnaliz.PlanlananTerminTarihi = Convert.ToDateTime(model.PlanlananTarih);
                var labAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(labAnaliz.LabAnalizDurumu);
                if (editlabAnaliz.PlanlananTerminTarihi == null)
                {
                    LabHareketGuncelle(editlabAnaliz.LabAnalizId, "Belirlenmedi",
                            model.PlanlananTarih, "Planlanan Termin Tarihi");
                }
                else
                {
                    if (labAnaliz.PlanlananTerminTarihi != editlabAnaliz.PlanlananTerminTarihi)
                    {
                        LabHareketGuncelle(editlabAnaliz.LabAnalizId, editlabAnaliz.PlanlananTerminTarihi.Value.ToShortDateString(),
                            model.PlanlananTarih, "Planlanan Termin Tarihi");
                    }
                }

                if (labAnaliz.LabAnalizDurumu != 0)
                {
                    if (editlabAnaliz.LabAnalizDurumu != labAnaliz.LabAnalizDurumu)
                    {
                        LabHareketGuncelle(editlabAnaliz.LabAnalizId, editlabAnaliz.LabAnalizDurumuAdi,
                            labAnalizDurumu.LabAnalizDurumuAdi, "Analiz Durumu");
                    }
                }

                editlabAnaliz.PlanlananTerminTarihi = labAnaliz.PlanlananTerminTarihi;
                editlabAnaliz.LabAnalizDurumu = labAnalizDurumu.LabAnalizDurumu;
                editlabAnaliz.LabAnalizDurumuAdi = labAnalizDurumu.LabAnalizDurumuAdi;
                _dbPoly.SaveChanges();
                if (labAnalizDurumu.LabAnalizDurumu > 3) // analiz başladıktan sonra girilir
                {
                    var EklenecekBobinIplikModelItemlar = model.BobinIplikModelItemlar.Where(a => a.SeciliMi)
                .OrderBy(a => a.IplikNo).ToList();

                    var SilinecekBobinIplikModelItemlar = model.BobinIplikModelItemlar.Where(a => a.SeciliMi == false)
                        .OrderBy(a => a.IplikNo).ToList();

                    var LabAnalizItemlar = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId)
                        .ToList();
                    var EklenecekLabAnalizler = new List<SrcnLabAnalizItems>();
                    var SilinecekLabAnalizler = new List<SrcnLabAnalizItems>();

                    foreach (var item in SilinecekBobinIplikModelItemlar)
                    {
                        if (LabAnalizItemlar.Any(a => a.IplikNo == item.IplikNo))
                        {
                            SilinecekLabAnalizler.Add(LabAnalizItemlar.First(a => a.IplikNo == item.IplikNo));
                        }
                    }

                    var silinecekAnalizItemCesitSonuclari = new List<SrcnLabAnalizItemAnalizCesitSonucs>();

                    foreach (var item in SilinecekLabAnalizler)
                    {
                        silinecekAnalizItemCesitSonuclari.AddRange(_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Where(a => a.LabAnalizItemId == item.LabAnalizItemId).ToList());
                    }

                    foreach (var item in SilinecekLabAnalizler)
                    {
                        LabHareketSil(editlabAnaliz.LabAnalizId, item.BobinAdi, item.BobinAdi);
                    }
                    _dbPoly.SrcnLabAnalizItems.RemoveRange(SilinecekLabAnalizler.ToList());
                    _dbPoly.SaveChanges();
                    _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.RemoveRange(silinecekAnalizItemCesitSonuclari);

                    foreach (var item in silinecekAnalizItemCesitSonuclari)
                    {
                        LabHareketSil(editlabAnaliz.LabAnalizId, item.LabAnalizCesitAdi, item.IplikAdi + " Analiz Çeşidi");
                    }

                    _dbPoly.SaveChanges();
                    LabAnalizItemlar = _dbPoly.SrcnLabAnalizItems.AsNoTracking().Where(a => a.LabAnalizId == labAnaliz.LabAnalizId).ToList();
                    foreach (var item in EklenecekBobinIplikModelItemlar)
                    {
                        if (LabAnalizItemlar.Any(a => a.IplikNo == item.IplikNo))
                        {
                            var labAnalizItem = LabAnalizItemlar.First(a => a.IplikNo == item.IplikNo);

                            var itemsonuclar = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                                .Where(a => a.LabAnalizItemId == labAnalizItem.LabAnalizItemId).ToList();

                            var silinecekCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                            foreach (var cesit in item.LabAnalizCesitleri.ToList())
                            {
                                if (cesit.SeciliMi == false)
                                {
                                    if (itemsonuclar.Any(a => a.LabAnalizCesitId == cesit.LabAnalizCesitId))
                                    {
                                        var silItem = itemsonuclar.First(a => a.LabAnalizCesitId == cesit.LabAnalizCesitId);
                                        if (silinecekCesitSonuclar.Count(a => a.LabAnalizCesitId == cesit.LabAnalizCesitId) == 0)
                                        {
                                            silinecekCesitSonuclar.Add(silItem);
                                        }

                                    }
                                }
                                else
                                {
                                    var dbCesit = _dbPoly.SrcnLabAnalizCesits.Find(cesit.LabAnalizCesitId);
                                    if (itemsonuclar.Count(a => a.LabAnalizCesitId == cesit.LabAnalizCesitId) == 0)
                                    {
                                        _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Add(
                                            new SrcnLabAnalizItemAnalizCesitSonucs()
                                            {

                                                LabAnalizCesitAdi = dbCesit.LabAnalizCesitAdi,
                                                LabAnalizCesitId = dbCesit.LabAnalizCesitId,
                                                LabAnalizItemId = labAnalizItem.LabAnalizItemId,
                                                DegerTipi = dbCesit.DegerTipi,
                                                IplikAdi = "IPLIK-" + labAnalizItem.IplikNo


                                            });
                                        _dbPoly.SaveChanges();
                                    }
                                }



                            }

                            if (silinecekCesitSonuclar.Any())
                            {
                                _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.RemoveRange(silinecekCesitSonuclar);
                                _dbPoly.SaveChanges();
                            }

                        }
                        else
                        {
                            string ipAdi = "ANA İPLİK";
                            if (item.IplikNo != 0)
                            {
                                ipAdi = "IPLIK-" + item.IplikNo;
                            }
                            var yeniAnalizItem = new SrcnLabAnalizItems()
                            {
                                LabGrupId = editlabAnaliz.LabGrupId,
                                LabAnalizId = editlabAnaliz.LabAnalizId,
                                LabGrupAdi = editlabAnaliz.LabGrupAdi,
                                KayitTarihi = DateTime.Now,
                                BobinAdi = ipAdi,
                                IplikNo = item.IplikNo,
                                SonucTarihi = DateTime.Now,
                            };
                            _dbPoly.SrcnLabAnalizItems.Add(yeniAnalizItem);

                            _dbPoly.SaveChanges();


                            foreach (var cesit in item.LabAnalizCesitleri.Where(a => a.SeciliMi).ToList())
                            {
                                var dbCesit = _dbPoly.SrcnLabAnalizCesits.Find(cesit.LabAnalizCesitId);

                                _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Add(
                                    new SrcnLabAnalizItemAnalizCesitSonucs()
                                    {
                                        LabAnalizCesitAdi = dbCesit.LabAnalizCesitAdi,
                                        LabAnalizCesitId = dbCesit.LabAnalizCesitId,
                                        LabAnalizItemId = yeniAnalizItem.LabAnalizItemId,
                                        DegerTipi = dbCesit.DegerTipi,
                                        IplikAdi = "IPLIK-" + yeniAnalizItem.IplikNo
                                    });
                                _dbPoly.SaveChanges();
                            }

                        }
                    }

                    if (numune.DosyaDurumId < 3)
                    {
                        // Analiz sonucu bekleniyor
                        var editdurum = _dbPoly.SrcnDosyaDurums.Find(3);
                        numune.DosyaDurumId = editdurum.DosyaDurumId;
                        numune.DosyaDurumAdi = editdurum.DosyaDurumAdi;
                        _dbPoly.SaveChanges();
                    }


                }



                _dbPoly = new POTA_PTKSEntities();

                TempDataOlustur("Güncelleme İşlemi Yapılmıştır", true);
            }
            return RedirectToAction("YapilacakAnalizNumuneDetay", "Laboratuvar", new { id = labAnaliz.LabAnalizId });
        }
        #endregion
        #region  Yapilacak Analiz-Müşteri Şikayetleri

        public ActionResult MusteriSikayetDosyalar(int? id)
        {
            TempData["Gizle"] = "gizle";

            var model = new LaboratuvarModel();
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 4 && a.DosyaDurumId < 30 && a.DosyaDurumId > 25 && a.DosyaDurumId != 30).ToList();
            if (id == null)
            {
                // analiz bekleyenler yapilacaklar
                model.MusteriSikayetDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == 26).OrderByDescending(a => a.KayitTarihi).ToList();
            }
            else
            {
                int idd = Convert.ToInt32(id);
                model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == idd);
                model.MusteriSikayetDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == idd).OrderByDescending(a => a.KayitTarihi).ToList();
                // analiz sonuçlananlar

            }


            return View(model);
        }

        public ActionResult MusteriSikayetOnaylar(int? id)
        {
            TempData["Gizle"] = "gizle";


            var model = new LaboratuvarModel();
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => (a.DosyaTipi == 4) && (a.DosyaDurumId == 30 || a.DosyaDurumId == 31 || a.DosyaDurumId == 35 || a.DosyaDurumId == 36)).OrderBy(a => a.DosyaDurumId).ToList();
            if (id == null)
            {

                // TEKNİSYEN TAMAMLADI DURUMU =6 BU YÜZDEN ONAYA ONLARI GETİRYORUZ
                // analiz onay yapilacaklar
                model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == 30);
                model.MusteriSikayetDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == 29).OrderByDescending(a => a.KayitTarihi).ToList();
            }
            else
            {
                int idd = Convert.ToInt32(id);
                if (idd == 0)
                {
                    idd = 30;
                }
                if (idd == 30)
                {
                    model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == 30);
                    idd = 29;
                }
                if (idd == 35)
                {
                    model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == 35);
                    idd = 34;
                }
                else
                {
                    model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == idd);
                }

                model.MusteriSikayetDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == idd).OrderByDescending(a => a.KayitTarihi).ToList();
                // analiz

            }

            return View(model);
        }

        public ActionResult MusteriSikayetOnaylaDuzenle(int id)
        {
            var DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => (a.DosyaTipi == 4) && (a.DosyaDurumId == 29  || a.DosyaDurumId == 31 || a.DosyaDurumId == 36)).OrderBy(a => a.DosyaDurumId).ToList();

            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var musteriSikayetDosya = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            model.MusteriSikayetDosya = musteriSikayetDosya;
            model.DosyaMusteriLabAnalizTablo = MusteriDosyaLabAnalizTabloOlustur(id);
            model.DosyaDurumlari = DosyaDurumlari;

            return View(model);
        }

        [HttpPost]
        public ActionResult MusteriSikayetOnaylaDuzenle(LaboratuvarModel model)
        {
            var numune = model.MusteriSikayetDosya;
            var editNumune = _dbPoly.SrcnMusteriSikayetDosyas.Find(numune.MusteriSikayetDosyaId);
            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(numune.DosyaDurumId);
            if (numune.DosyaDurumId == 30)
            {
                dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(29);
            }

            if (editNumune.DosyaDurumId != dosyaDurum.DosyaDurumId)
            {
                editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
                editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
                _dbPoly.SaveChanges();
                DosyaLabAnalizDurumGuncelle(3, editNumune.MusteriSikayetDosyaId, true);
                //BilgiMailiGonder(2, 1, numune.MusteriSikayetDosyaId);
            }

            TempDataCRUDOlustur(2);
            return RedirectToAction("MusteriSikayetOnaylar", "Laboratuvar",
                new { id = model.MusteriSikayetDosya.DosyaDurumId });
        }

        public ActionResult YapilacakAnalizMusteriSikayet(int id)
        {
            var DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 4 && a.DosyaDurumId > 25 && a.DosyaDurumId < 30)
                .OrderBy(a => a.DosyaDurumId).ToList();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Where(a => a.BagliOlduguDosyaId == id && a.DosyaTipi == 3).FirstOrDefault();

            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var musteriSikayeDosya = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).ToList();
            ViewBag.Birimler = new SelectList(birimler, "BirimId", "BirimAdi");
            model.MusteriSikayetDosya = musteriSikayeDosya;
            model.DosyaMusteriLabAnalizTablo = MusteriDosyaLabAnalizTabloOlustur(id);
            model.DosyaDurumlari = DosyaDurumlari;
            model.LabAnaliz.LabAnalizId = labAnaliz.LabAnalizId;
            model.LabAnaliz.LabGrupId = labAnaliz.LabGrupId;
            model.LabAnaliz.KayitYapanKulAdi = labAnaliz.KayitYapanKulAdi;

            return View(model);

        }
        [HttpPost]
        public ActionResult YapilacakAnalizMusteriSikayet(LaboratuvarModel model)
        {
            var eklenecekListe = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
            var silinecekListe = new List<int>();
            var guncellenecek = new List<SrcnLabAnalizItemAnalizCesitSonucs>();

            foreach (var tablo in model.DosyaMusteriLabAnalizTablo)
            {
                foreach (var tabloSatir in tablo.AtkiTablo.DosyaLabAnalizTabloSatirlar)
                {

                    eklenecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId == 0 && a.AnalizDegeriString != null).ToList());
                    silinecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString == null).Select(a => a.LabAnalizItemAnalizCesitId).ToList());
                    guncellenecek.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString != null).ToList());

                }

                foreach (var tabloSatir in tablo.CozguTablo.DosyaLabAnalizTabloSatirlar)
                {

                    eklenecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId == 0 && a.AnalizDegeriString != null).ToList());
                    silinecekListe.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString == null).Select(a => a.LabAnalizItemAnalizCesitId).ToList());
                    guncellenecek.AddRange(tabloSatir.LabAnalizItemAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemAnalizCesitId != 0 && a.AnalizDegeriString != null).ToList());

                }
            }

            var silListe = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                .Where(a => silinecekListe.Any(b => b == a.LabAnalizItemAnalizCesitId)).ToList();
            _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.RemoveRange(silListe);
            _dbPoly.SaveChanges();
            var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0 && a.Sira > 0).OrderBy(a => a.Sira).ToList();
            foreach (var item in eklenecekListe)
            {
                var labItem = _dbPoly.SrcnLabAnalizItems.Find(item.LabAnalizItemId);
                var analizCesit = TumIplikAnalizCesitleri.First(a => a.LabAnalizCesitId == item.LabAnalizCesitId);
                _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                {
                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                    AnalizDegeriString = item.AnalizDegeriString,
                    LabAnalizItemId = labItem.LabAnalizItemId,
                    DegerTipi = analizCesit.LabAnalizCesitId,
                    IplikAdi = labItem.BobinAdi,
                    IplikNo = labItem.IplikNo,
                    MakinePozisyonNo = labItem.MakinePozisyonNo,
                    LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi,
                    LabAnalizCesitAdiEng = analizCesit.LabAnalizCesitAdiEng,
                });
                _dbPoly.SaveChanges();
            }
            foreach (var item in guncellenecek)
            {
                if (_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Any(a => a.LabAnalizItemAnalizCesitId == item.LabAnalizItemId && a.AnalizDegeriString != item.AnalizDegeriString))
                {
                    var editItem = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Find(item.LabAnalizItemAnalizCesitId);
                    editItem.AnalizDegeriString = item.AnalizDegeriString;
                    _dbPoly.SaveChanges();
                }
            }

            var musteriDosya = model.MusteriSikayetDosya;
            var editNumune = _dbPoly.SrcnMusteriSikayetDosyas.Find(musteriDosya.MusteriSikayetDosyaId);

            if (musteriDosya.DosyaDurumId == 0)
            {
                musteriDosya.DosyaDurumId = 27;
            }

            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(musteriDosya.DosyaDurumId);
            editNumune.BirimId = model.Birim.BirimId;
            editNumune.BirimAdi = model.Birim.BirimAdi;
            editNumune.LabAciklama = model.MusteriSikayetDosya.LabAciklama;
            if (editNumune.DosyaDurumId != dosyaDurum.DosyaDurumId)
            {
                editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
                editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
                _dbPoly.SaveChanges();

                DosyaLabAnalizDurumGuncelle(3, editNumune.MusteriSikayetDosyaId, true);
               // BilgiMailiGonder(2, 1, numune.MusteriSikayetDosyaId);
            }


            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(2);
            return RedirectToAction("YapilacakAnalizMusteriSikayet", "Laboratuvar",
                new { id = model.MusteriSikayetDosya.MusteriSikayetDosyaId });
        }

        #endregion
        public ActionResult YapilacakAnalizSuAnaliziKontrolDetay(int id)
        {
            _dbPoly = new POTA_PTKSEntities();
            var model = new LaboratuvarModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            model.LabAnaliz = labAnaliz;
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(labAnaliz.LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            model.LabAnalizDurumlari = _dbPoly.SrcnLabAnalizDurumus.Where(a => a.LabAnalizDurumu < 6 && a.LabAnalizDurumu >= labAnaliz.LabAnalizDurumu).ToList();
            if (model.LabAnaliz.PlanlananTerminTarihi != null)
            {
                model.PlanlananTarih = model.LabAnaliz.PlanlananTerminTarihi.Value.ToString("dd.MM.yyyy");
            }

            return View(model);
        }
        public ActionResult LaboratuvarAnalizHareketleri(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            model.LabAnaliz = labAnaliz;
            model.LabAnalizHareketleri = _dbPoly.SrcnLabAnalizHarekets.Where(a => a.LabAnalizId == id)
                .OrderByDescending(a => a.KayitTarihi).ToList();
            model.LabAnalizLoglari = _dbPoly.SrcnLabAnalizLogs.Where(a => a.LabAnalizId == id)
                .OrderByDescending(a => a.KayitTarihi).ToList();
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(labAnaliz.LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            return View(model);
        }
        public ActionResult LaboratuvarAnalizSonuclari(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = new LaboratuvarModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);

            var analizItemSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
            model.LabAnaliz = labAnaliz;
            model.LabAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == id)
                .OrderBy(a => a.IplikNo).ThenBy(a => a.LabAnalizItemId).ToList();
            if (labAnaliz.MakineId != 0)
            {
                model.Makine = _dbPoly.Makine.First(a => a.SayacOndalikSayisi == labAnaliz.MakineId);
            }
            var listeAnalizCesits = new List<SrcnLabAnalizItemAnalizCesitSonucs>();

            foreach (var item in model.LabAnalizItems)
            {
                var araItem = new LaboratuvarAnalizItemSonuclarModel
                {
                    LabAnalizItem = item,
                    LabAnalizItemAnalizCesitSonuclari = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                        .Where(a => a.LabAnalizItemId == item.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi)
                        .ToList()
                };
                analizItemSonuclar.AddRange(araItem.LabAnalizItemAnalizCesitSonuclari);
                foreach (var aa in araItem.LabAnalizItemAnalizCesitSonuclari)
                {
                    if (listeAnalizCesits.Count(a => a.LabAnalizCesitId == aa.LabAnalizCesitId) == 0)
                    {
                        listeAnalizCesits.Add(aa);
                    }
                }
                model.LaboratuvarAnalizItemSonuclarModeller.Add(araItem);
            }


            foreach (var bb in listeAnalizCesits.OrderBy(a => a.LabAnalizCesitAdi).ToList())
            {
                model.LabAnalizCesitleri.Add(new SrcnLabAnalizCesits()
                {
                    LabAnalizCesitId = bb.LabAnalizCesitId,
                    LabAnalizCesitAdi = bb.LabAnalizCesitAdi
                });
            }
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(labAnaliz.LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            model.LabAnalizDurumlari = _dbPoly.SrcnLabAnalizDurumus.Where(a => a.LabAnalizDurumu < 6 && a.LabAnalizDurumu >= labAnaliz.LabAnalizDurumu).ToList();
            model.LabGrup = _dbPoly.SrcnLabGrups.Find(labAnaliz.LabGrupId);
            if (labAnaliz.AnalizTipi == 3)
            {
                model.NumuneYapilabilirlikDosya =
                    _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(labAnaliz.BagliOlduguDosyaId);
            }

            if (labAnaliz.AnalizTipi == 2)
            {
                model.MusteriSikayetDosya =
                    _dbPoly.SrcnMusteriSikayetDosyas.Find(labAnaliz.BagliOlduguDosyaId);
            }
            var analizCesits = new List<SrcnLabAnalizCesits>();


            int say = 0;
            var LabSatirItemlar = new List<LabSatirItem>();
            foreach (var itt in model.LabAnalizCesitleri.OrderBy(a => a.LabAnalizCesitAdi))
            {

                var araListe = new List<SrcnLabAnalizItemAnalizCesitSonucs>();

                araListe.AddRange(analizItemSonuclar.Where(a => a.LabAnalizCesitId == itt.LabAnalizCesitId)
                    .OrderBy(a => a.IplikAdi).ToList());

                LabSatirItemlar.Add(new LabSatirItem()
                {
                    LabAnalizCesitId = itt.LabAnalizCesitId,
                    LabAnalizCesitAdi = itt.LabAnalizCesitAdi,
                    LabAnalizItemAnalizCesitSonuclari = araListe
                });
                if (say < araListe.Count)
                {
                    say = araListe.Count;
                }
            }

            var labItem = new LabSatirItem { LabAnalizCesitAdi = "Pozisyon" };
            for (int i = 0; i < say; i++)
            {
                labItem.LabAnalizItemAnalizCesitSonuclari.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                {
                    LabAnalizCesitAdi = "Pozisyon"
                });
            }
            model.LabSatirItemlar.Add(labItem);
            model.LabSatirItemlar.AddRange(LabSatirItemlar);

            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult LaboratuvarAnalizSonuclari(LaboratuvarModel model)
        {
            _dbPoly = new POTA_PTKSEntities();
            var labAnaliz = model.LabAnaliz;
            var editlabAnaliz = _dbPoly.SrcnLabAnalizs.Find(model.LabAnaliz.LabAnalizId);
            var LaboratuvarAnalizItemSonuclarModeller = model.LaboratuvarAnalizItemSonuclarModeller;

            var analizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(labAnaliz.LabAnalizDurumu);

            editlabAnaliz.LabAciklama = labAnaliz.LabAciklama;
            _dbPoly.SaveChanges();
            if (editlabAnaliz.LabAnalizDurumu != analizDurumu.LabAnalizDurumu)
            {
                string eskiDurum = editlabAnaliz.LabAnalizDurumuAdi;
                LabHareketGuncelle(editlabAnaliz.LabAnalizId, eskiDurum, analizDurumu.LabAnalizDurumuAdi,
                    "Analiz Durumu");
                editlabAnaliz.LabAnalizDurumu = analizDurumu.LabAnalizDurumu;
                editlabAnaliz.LabAnalizDurumuAdi = analizDurumu.LabAnalizDurumuAdi;
                _dbPoly.SaveChanges();

            }
            if (editlabAnaliz.AnalizTipi == 0)
            {
                // günlük imalat
                int SiraSay = 0;
                foreach (var itt in LaboratuvarAnalizItemSonuclarModeller)
                {
                    SiraSay++;
                    var labAnalizItem = _dbPoly.SrcnLabAnalizItems.Find(itt.LabAnalizItem.LabAnalizItemId);

                    if (labAnalizItem.MakinePozisyonNo != itt.LabAnalizItem.MakinePozisyonNo)
                    {
                        string eskiDeger = labAnalizItem.MakinePozisyonNo.ToString();
                        labAnalizItem.MakinePozisyonNo = itt.LabAnalizItem.MakinePozisyonNo;
                        _dbPoly.SaveChanges();
                        LabHareketLogGuncelle(editlabAnaliz.LabAnalizId, itt.LabAnalizItem.MakinePozisyonNo.ToString(),
                            eskiDeger, SiraSay.ToString() + ". Sıra Pozisyon");

                    }

                    foreach (var item in itt.LabAnalizItemAnalizCesitSonuclari)
                    {
                        var editSonucs =
                            _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Find(item.LabAnalizItemAnalizCesitId);
                        if (editSonucs.AnalizDegeriString != item.AnalizDegeriString)
                        {
                            string eskiDeger = editSonucs.AnalizDegeriString;
                            if (editSonucs.AnalizDegeriString == null)
                            {
                                eskiDeger = "Belirlenmemiş";
                            }
                            if (editSonucs.DegerTipi == 0)
                            {
                                editSonucs.AnalizDegeri = item.AnalizDegeriString.DecimaleCevir();
                            }

                            editSonucs.AnalizDegeriString = item.AnalizDegeriString;
                            _dbPoly.SaveChanges();
                            LabHareketLogGuncelle(editlabAnaliz.LabAnalizId, item.AnalizDegeriString,
                                eskiDeger, SiraSay.ToString() + ". Sıra " + editSonucs.LabAnalizCesitAdi);
                        }
                    }
                }
            }

            if (editlabAnaliz.AnalizTipi == 4)
            {
                // yardımcı tesisler su analizi

                MailMessage mailim = new MailMessage();


                mailim.To.Add("NBUTUN@polyteks.com.tr");
                mailim.To.Add("SKARASU@polyteks.com.tr");
                mailim.To.Add("muzun@polyteks.com.tr");
                mailim.To.Add("LTEKNISYENI@polyteks.com.tr");

                mailim.CC.Add("MDIKICI@polyteks.com.tr");
                mailim.From = new MailAddress("pota_bilgi@polyteks.com.tr");

                mailim.Subject = "YARDIMCI TESİSLER SU ANALİZİ HK.";
                //var body = "<p>POY IKAZ BILDIRIMI YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p>" + deneme.PartiNo;
                mailim.Body = "<p>LABORATUVAR SU ANALİZİ SONUCU GİRİLMİŞTİR.LÜTFEN SONUÇLARI KONTROL EDİNİZ.</p> ";


                mailim.IsBodyHtml = true;
                //mailim.Body = body;
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("pota_bilgi@polyteks.com.tr", "ptks1986");
                smtp.Host = "10.10.1.5";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                try
                {
                    smtp.Send(mailim);
                    TempData["Message"] = "Mesaj Basariyla Gonderildi";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Mesaj Gönderilemedi.Hata Nedeni :" + ex.Message;
                }
                int SiraSay = 0;
                foreach (var itt in LaboratuvarAnalizItemSonuclarModeller)
                {
                    SiraSay++;
                    var labAnalizItem = _dbPoly.SrcnLabAnalizItems.Find(itt.LabAnalizItem.LabAnalizItemId);


                    foreach (var item in itt.LabAnalizItemAnalizCesitSonuclari)
                    {
                        var editSonucs =
                            _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Find(item.LabAnalizItemAnalizCesitId);
                        if (editSonucs.AnalizDegeriString != item.AnalizDegeriString)
                        {
                            string eskiDeger = editSonucs.AnalizDegeriString;
                            if (editSonucs.AnalizDegeriString == null)
                            {
                                eskiDeger = "Belirlenmemiş";
                            }
                            if (editSonucs.DegerTipi == 0)
                            {
                                editSonucs.AnalizDegeri = item.AnalizDegeriString.DecimaleCevir();
                            }

                            editSonucs.AnalizDegeriString = item.AnalizDegeriString;
                            _dbPoly.SaveChanges();
                            LabHareketLogGuncelle(labAnaliz.LabAnalizId, item.AnalizDegeriString,
                                eskiDeger, SiraSay.ToString() + editSonucs.YardimciTesisKontrolNoktaAdi + " " + editSonucs.LabAnalizCesitAdi);
                        }
                    }
                }
            }
            if (editlabAnaliz.AnalizTipi == 3)
            {
                // numune yapılabilirlik
                int SiraSay = 0;
                foreach (var itt in LaboratuvarAnalizItemSonuclarModeller)
                {
                    SiraSay++;
                    var labAnalizItem = _dbPoly.SrcnLabAnalizItems.Find(itt.LabAnalizItem.LabAnalizItemId);


                    foreach (var item in itt.LabAnalizItemAnalizCesitSonuclari)
                    {
                        var editSonucs =
                            _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Find(item.LabAnalizItemAnalizCesitId);
                        if (editSonucs.AnalizDegeriString != item.AnalizDegeriString)
                        {
                            string eskiDeger = editSonucs.AnalizDegeriString;
                            if (editSonucs.AnalizDegeriString == null)
                            {
                                eskiDeger = "Belirlenmemiş";
                            }
                            if (editSonucs.DegerTipi == 0)
                            {
                                editSonucs.AnalizDegeri = item.AnalizDegeriString.DecimaleCevir();
                            }

                            editSonucs.AnalizDegeriString = item.AnalizDegeriString;
                            _dbPoly.SaveChanges();
                            LabHareketLogGuncelle(labAnaliz.LabAnalizId, item.AnalizDegeriString,
                                eskiDeger, SiraSay.ToString() + editSonucs.YardimciTesisKontrolNoktaAdi + " " + editSonucs.LabAnalizCesitAdi);
                        }
                    }
                }
            }
            TempDataOlustur("Güncelleme İşlemi Yapılmıştır", true);
            return RedirectToAction("LaboratuvarAnalizSonuclari", "Laboratuvar", new { id = labAnaliz.LabAnalizId });
        }

        public ActionResult LabPartiSonuBilgiOnayiBekleyenListe()
        {
            var model = new ImalatPartiSonuModel();
            var partiSonuIdler = _dbPoly.SrcnPartiSonuTakips
                .Where(a => (a.PartiSonuTakipHareketTipi == 2 || a.PartiSonuTakipHareketTipi == 3)).Select(a => a.PartiSonuTakipId).ToList();
            var bekleyenListe = _dbPoly.SrcnPartiSonuTakipBilgiOnays.AsNoTracking().Where(a => (a.BirimId == 18 && partiSonuIdler.Any(b => b == a.PartiSonuTakipId))).OrderBy(a => a.OnayTarihi).ToList();
            //  var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            foreach (var item in bekleyenListe)
            {
                var partiSonu = _dbPoly.SrcnPartiSonuTakips.Find(item.PartiSonuTakipId);
                if (_dbPoly.ZzzSrcnPartiSonuDurumOzet.Any(a => a.RefakatNo == partiSonu.RefakatNo))
                {
                    model.PartiSonuDurumOzetCheckItemlar.Add(new PartiSonuDurumOzetCheckItem()
                    {
                        PartiSonuDurumOzet =
                            _dbPoly.ZzzSrcnPartiSonuDurumOzet.AsNoTracking().First(a => a.RefakatNo == partiSonu.RefakatNo),
                        PartiSonuTakip = partiSonu,
                        PartiSonuTakipBilgiOnay = item
                    });
                }

            }

            return View(model);
        }

    }
}