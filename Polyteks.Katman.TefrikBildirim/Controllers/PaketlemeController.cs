using Polyteks.Katman.TefrikBildirim.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Data;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using PagedList;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.Has.Controllers
{
    public class PaketlemeController : BaseController
    {
        /// <summary>
        /// 0-talimat ekle, 1-talimat sil, 2-talimat detay ekle, 3detay güncelle, 4-talimat sil
        /// </summary>
        /// <param name="durum"></param>
        /// <param name="talimat"></param>
        /// <param name="talimatItems"></param>
        public void GunlukTalimatLog(int durum, SrcnPaketlemeGunlukTalimats talimat,
            List<SrcnPaketlemeGunlukTalimatItems> talimatItems)
        {
            var ccMailler = new List<string>();

            string Gonderici = "mdikici@polyteks.com.tr";
            string konu = talimat.PaketlemeGunlukTalimatTipiAdi + " ";
            string mailIcerik = "";
            var sonuc = "";
            sonuc = talimat.TalimatTarihi;

            if (Kullanici.EmailAdres != null)
            {
                if (Kullanici.EmailAdres.Contains("polyteks.com.tr"))
                {
                    Gonderici = Kullanici.EmailAdres;
                }
            }
            if (durum == 0)
            {
                // talimati Ekle
                konu = "Günlük Talimat Oluşturma";
                sonuc += " Talimat Oluşturma İşlemi Yapılmıştır";
            }
            if (durum == 1)
            {
                // talimatiSil
                konu = "Günlük Talimat Silme";
                sonuc += " Silme İşlemi Yapılmıştır";
            }
            if (durum == 2)
            {
                konu = "Günlük Talimat Detay Ekleme";
                sonuc += " Talimat Detay Ekleme İşlemi Yapılmıştır";
                // talimatiItemDuzenle
            }
            if (durum == 3)
            {
                konu = "Günlük Talimat Detay Değişikliği";
                sonuc += " Talimat Detay Değişikliği Yapılmıştır";
                // talimatiItemSil
            }
            if (durum == 4)
            {
                konu = "Günlük Talimat Detay Silinmesi";
                sonuc += " Talimat Detay Silinmesi Yapılmıştır";
                // talimatiItemSil
            }
            if (talimatItems.Any())
            {


                sonuc += "</br> <table border=\"1\">";

                var theadItems = new List<string>();
                var tbodyItems = new List<string>();
                theadItems.Add("#");
                theadItems.Add("BAŞLIK");
                theadItems.Add("İÇERİK");
                theadItems.Add("TARİH");
                theadItems.Add("KAYIT YAPAN");
                string theadOlustur = "<thead><tr>";
                foreach (var s in theadItems)
                {
                    theadOlustur += "<th>" + s + "</th>";
                }

                theadOlustur += "</tr></thead>";
                string tbodyOlustur = "<tbody>";
                int say = 0;
                foreach (var item in talimatItems)
                {
                    say++;
                    tbodyOlustur += "<tr>";
                    tbodyItems = new List<string>();

                    tbodyItems.Add(say.ToString());
                    tbodyItems.Add(item.TalimatBaslik);
                    tbodyItems.Add(item.TalimatIcerik.ToString());
                    tbodyItems.Add(item.KayitTarihi.ToString());
                    tbodyItems.Add(item.KayitYapanKulAdi);
                    foreach (var s in tbodyItems)
                    {
                        tbodyOlustur += "<td>" + s + "</td>";
                    }

                    tbodyOlustur += "</tr>";
                }

                tbodyOlustur += "</tbody>";
                sonuc += theadOlustur;
                sonuc += tbodyOlustur;
                sonuc += "</table>";
            }

            var BirimId = 8;
            if (_dbPoly.SrcnMailBildirimGrups.Any(a => a.MailBildirimGrupId == BirimId))
            {
                if (_dbPoly.SrcnMailBildirimGrupItems.Any(a => a.MailBildirimGrupId == BirimId))
                {
                    ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == BirimId).Select(a => a.EmailAdres).ToList());
                }


            }


            sonuc += "</br>" + Kullanici.IsimSoyisim;
            konu = talimat.PaketlemeGunlukTalimatTipiAdi + " " + konu.ToUpper();
            GenelDurumMailGonder(Gonderici, ccMailler, sonuc, konu);
        }
        // GET: Paketleme
        public ActionResult PaketlemePartiSonuBekleyenListe(int? id)
        {
            //PartiSonuTakipKontrolEt();

            TempData["Gizle"] = "aa";
            int idd = 3;
            if (id != null)
            {
                if (Convert.ToInt32(id) >= 3)
                {
                    idd = Convert.ToInt32(id);
                }
            }

            var model = new ImalatPartiSonuModel();

            if (idd == 6)
            {
                var TarihSiniri = DateTime.Now.Date.AddDays(-5);
                var partiSonlari = _dbPoly.SrcnPartiSonuTakips
                    .Where(a => a.PartiSonuTakipHareketTipi == idd && a.PartiKapatmaTarihi > TarihSiniri)
                    .OrderByDescending(a => a.PartiKapatmaTarihi).ToList().ToList();
                foreach (var partiSonu in partiSonlari)
                {

                    if (_dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Any(a => a.RefakatNo == partiSonu.RefakatNo))
                    {
                        model.PartiSonuDurumOzetCheckItemlar.Add(new PartiSonuDurumOzetCheckItem()
                        {
                            PartiSonuTakipIzlenebilirlik =
                                _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.AsNoTracking()
                                    .First(a => a.RefakatNo == partiSonu.RefakatNo),
                            PartiSonuTakip = partiSonu,

                        });
                    }
                }

                //model.PartiSonuDurumOzetCheckItemlar = model.PartiSonuDurumOzetCheckItemlar
                //    .OrderByDescending(a => a.PartiSonuTakip.PartiKapatmaTarihi).ToList();
            }
            else
            {
                var partiSonlari = _dbPoly.SrcnPartiSonuTakips
                    .Where(a => (a.PartiSonuTakipHareketTipi == idd)).ToList().ToList();
                foreach (var partiSonu in partiSonlari)
                {

                    if (_dbPoly.ZzzSrcnPartiSonuDurumOzet.Any(a => a.RefakatNo == partiSonu.RefakatNo))
                    {
                        model.PartiSonuDurumOzetCheckItemlar.Add(new PartiSonuDurumOzetCheckItem()
                        {
                            PartiSonuDurumOzet =
                                _dbPoly.ZzzSrcnPartiSonuDurumOzet.AsNoTracking()
                                    .First(a => a.RefakatNo == partiSonu.RefakatNo),
                            PartiSonuTakip = partiSonu,

                        });
                    }
                }


            }

            model.PartiSonuTakipHareketleri = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims
                .Where(a => a.PartiSonuTakipHareketTipi > 2 && a.PartiSonuTakipHareketTipi < 7)
                .OrderBy(a => a.PartiSonuTakipHareketTipi).ToList();
            model.Id = idd;

            return View(model);
        }

        public ActionResult PaketlemePartiSonuDuzenle(int id)
        {
            TempData["Gizle"] = "aa";
            var model = new ImalatPartiSonuModel();
            var partiSonu = _dbPoly.SrcnPartiSonuTakips.Find(id);

            if (_dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Any(a => a.RefakatNo == partiSonu.RefakatNo))
            {
                model.PartiSonuDurumOzetCheckItem = new PartiSonuDurumOzetCheckItem()
                {
                    PartiSonuTakipIzlenebilirlik =
                        _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.AsNoTracking()
                            .First(a => a.RefakatNo == partiSonu.RefakatNo),
                    PartiSonuTakip = partiSonu

                };
            }

            model.PartiSonuTakipHareketleri = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims
                .Where(a => a.PartiSonuTakipHareketTipi > 2).OrderBy(a => a.PartiSonuTakipHareketTipi).ToList();

            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            var EkIslemTanimlamalar = _dbPoly.SrcnPartiSonuEkIslemTanims.OrderBy(a => a.PartiSonuEkIslemAdi).ToList();
            var partiEkIslemleri = _dbPoly.SrcnPartiSonuEkIslems.Where(a => a.PartiSonuTakipId == id).ToList();

            foreach (var birm in birimler)
            {
                var araItem = new PartiSonuEkIslemItem
                {
                    Birim = birm
                };
                foreach (var item in EkIslemTanimlamalar)
                {
                    if (partiEkIslemleri.Any(a =>
                        a.BirimId == birm.BirimId && a.PartiSonuEkIslemTipi == item.PartiSonuEkIslemTipi))
                    {
                        araItem.PartiSonuEkIslemler.Add(partiEkIslemleri.First(a =>
                            a.BirimId == birm.BirimId && a.PartiSonuEkIslemTipi == item.PartiSonuEkIslemTipi));
                    }
                    else
                    {
                        araItem.PartiSonuEkIslemler.Add(new SrcnPartiSonuEkIslems()
                        {
                            BirimId = birm.BirimId,
                            BirimAdi = birm.BirimAdi,
                            PartiSonuEkIslemTipi = item.PartiSonuEkIslemTipi,
                            PartiSonuEkIslemAdi = item.PartiSonuEkIslemAdi,

                        });
                    }
                }

                model.PartiSonuEkIslemItemlar.Add(araItem);
                model.PartiSonuEkIslemTanimlari = EkIslemTanimlamalar;
            }

            return View(model);

        }

        [HttpPost]
        public ActionResult PaketlemePartiSonuDuzenle(ImalatPartiSonuModel model)
        {
            var partisonuTakip = model.PartiSonuDurumOzetCheckItem.PartiSonuTakip;
            var editPartisonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(partisonuTakip.PartiSonuTakipId);
            var guncellenecekHareket =
                _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(partisonuTakip.PartiSonuTakipHareketTipi);
            var SilinecekPartiSonuEkIslemler = new List<SrcnPartiSonuEkIslems>();
            var EklenecekPartiSonuEkIslemler = new List<SrcnPartiSonuEkIslems>();
            var GuncellenecekPartiSonuEkIslemler = new List<SrcnPartiSonuEkIslems>();


            foreach (var itt in model.PartiSonuEkIslemItemlar)
            {
                foreach (var item in itt.PartiSonuEkIslemler)
                {
                    if (item.GonderilecekBobinSayisi == 0)
                    {
                        // silincek
                        if (item.PartiSonuEkIslemId != 0)
                        {
                            SilinecekPartiSonuEkIslemler.Add(item);
                        }
                    }
                    else
                    {
                        if (item.PartiSonuEkIslemId != 0)
                        {
                            GuncellenecekPartiSonuEkIslemler.Add(item);
                        }
                        else
                        {
                            EklenecekPartiSonuEkIslemler.Add(item);
                        }
                    }
                }
            }

            if (SilinecekPartiSonuEkIslemler.Any())
            {
                foreach (var item in SilinecekPartiSonuEkIslemler)
                {
                    var silItem = _dbPoly.SrcnPartiSonuEkIslems.Find(item.PartiSonuEkIslemId);
                    _dbPoly.SrcnPartiSonuEkIslems.Remove(silItem);
                    _dbPoly.SaveChanges();
                }
            }

            if (GuncellenecekPartiSonuEkIslemler.Any())
            {
                foreach (var item in GuncellenecekPartiSonuEkIslemler)
                {
                    var editItem = _dbPoly.SrcnPartiSonuEkIslems.Find(item.PartiSonuEkIslemId);
                    editItem.GonderilecekBobinSayisi = item.GonderilecekBobinSayisi;
                    editItem.IslemDurumu = item.IslemDurumu;
                    if (item.IslemDurumu == 2 && editItem.IadeYapanKulId == 0)
                    {
                        editItem.IadeTarihi = DateTime.Now;
                        editItem.IadeYapanKulId = Kullanici.KullaniciId;
                        editItem.IadeYapanKulAdi = Kullanici.IsimSoyisim;
                    }

                    _dbPoly.SaveChanges();
                }
            }

            if (EklenecekPartiSonuEkIslemler.Any())
            {
                foreach (var item in EklenecekPartiSonuEkIslemler)
                {
                    var birim = _dbPoly.SrcnFabrikaBirims.Find(item.BirimId);
                    var ekIslemTipi = _dbPoly.SrcnPartiSonuEkIslemTanims.Find(item.PartiSonuEkIslemTipi);
                    var yeniKayit = new SrcnPartiSonuEkIslems()
                    {
                        BirimId = birim.BirimId,
                        BirimAdi = birim.BirimAdi,
                        KayitTarihi = DateTime.Now,
                        PartiSonuTakipId = partisonuTakip.PartiSonuTakipId,
                        PartiSonuEkIslemTipi = ekIslemTipi.PartiSonuEkIslemTipi,
                        GonderilecekBobinSayisi = item.GonderilecekBobinSayisi,
                        IslemDurumu = item.IslemDurumu,
                        PartiSonuEkIslemAdi = ekIslemTipi.PartiSonuEkIslemAdi,
                        IadeTarihi = null,
                        KayitYapanKulAdi = Kullanici.IsimSoyisim,
                        KayitYapanKulId = Kullanici.KullaniciId,
                        IadeYapanKulAdi = null,
                        IadeYapanKulId = 0
                    };
                    _dbPoly.SrcnPartiSonuEkIslems.Add(yeniKayit);
                    _dbPoly.SaveChanges();
                }
            }

            bool guncellenebilirMi = true;
            if (guncellenecekHareket.PartiSonuTakipHareketTipi == 6)
            {
                var listeee = _dbPoly.SrcnPartiSonuEkIslems.Where(a =>
                    a.PartiSonuTakipId == partisonuTakip.PartiSonuTakipId && a.IslemDurumu != 2).ToList();
                if (listeee.Count == 0)
                {
                    guncellenebilirMi = true;

                }
                else
                {
                    guncellenebilirMi = false;
                    TempDataOlustur("Ek işlem bölümünden gelmesini beklediğiniz bobinler bulunmaktadır", true);
                }
            }

            if (guncellenebilirMi)
            {
                bool mailgonderilsinMi = editPartisonuTakip.PartiSonuTakipHareketTipi !=
                                         partisonuTakip.PartiSonuTakipHareketTipi;

                TempDataCRUDOlustur(2);
                if (editPartisonuTakip.PartiSonuTakipHareketTipi == 6 &&
                    guncellenecekHareket.PartiSonuTakipHareketTipi != 6)
                {
                    if (_dbPoly.RefakatKarti.Any(a =>
                        a.RefakatNo == editPartisonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" &&
                        a.IslemNo != "900" && a.IslemBitisTarihi != null))
                    {
                        var refKart = _dbPoly.RefakatKarti.First(a =>
                            a.RefakatNo == editPartisonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" &&
                            a.IslemBitisTarihi != null && a.IslemNo != "900");
                        refKart.SabitlenenMakineNo = null;
                        refKart.IslemBitisTarihi = null;
                        _dbPoly.SaveChanges();

                        editPartisonuTakip.KapatanKulAdi = null;
                        editPartisonuTakip.KapatanKulId = null;
                        editPartisonuTakip.PartiKapatmaTarihi = null;
                        _dbPoly.SaveChanges();
                    }
                    else
                    {
                        TempDataOlustur("Bir Hata oluştu lütfen bilgi işleme bilgi veriniz", false);
                    }

                }



                editPartisonuTakip.PartiSonuTakipHareketTipi = guncellenecekHareket.PartiSonuTakipHareketTipi;
                editPartisonuTakip.PartiSonuTakipHareketAdi = guncellenecekHareket.PartiSonuTakipHareketAdi;
                _dbPoly.SaveChanges();



                if (guncellenecekHareket.PartiSonuTakipHareketTipi == 6)
                {

                    var refKart = _dbPoly.RefakatKarti.First(a =>
                        a.RefakatNo == editPartisonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" &&
                        a.IslemBitisTarihi == null && a.IslemNo != "900");
                    refKart.SabitlenenMakineNo = "1-srcn";
                    refKart.IslemBitisTarihi = DateTime.Now;
                    _dbPoly.SaveChanges();
                    editPartisonuTakip.KapatanKulAdi = Kullanici.IsimSoyisim;
                    editPartisonuTakip.KapatanKulId = Kullanici.KullaniciId;
                    editPartisonuTakip.PartiKapatmaTarihi = DateTime.Now;
                    _dbPoly.SaveChanges();

                }

                if (mailgonderilsinMi)
                {
                    partiSonuBilgiDegisiklikYap(0, partisonuTakip.PartiSonuTakipId);
                }

            }


            return RedirectToAction("PaketlemePartiSonuBekleyenListe", "Paketleme",
                new { id = partisonuTakip.PartiSonuTakipHareketTipi });
        }

        [HttpPost]
        public ActionResult PaketlemeHizliPartiSonuYap(ImalatPartiSonuModel model)
        {
            var liste = model.PartiSonuDurumOzetCheckItemlar.Where(a => a.SeciliMi)
                .Select(b => b.PartiSonuTakip.PartiSonuTakipId).ToList();
            var guncellenecekHareket = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(6);
            foreach (var i in liste)
            {
                var editPartisonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(i);
                editPartisonuTakip.PartiSonuTakipHareketTipi = guncellenecekHareket.PartiSonuTakipHareketTipi;
                editPartisonuTakip.PartiSonuTakipHareketAdi = guncellenecekHareket.PartiSonuTakipHareketAdi;
                editPartisonuTakip.KapatanKulAdi = Kullanici.IsimSoyisim;
                editPartisonuTakip.KapatanKulId = Kullanici.KullaniciId;
                editPartisonuTakip.PartiKapatmaTarihi = DateTime.Now;
                _dbPoly.SaveChanges();

                var refKart = _dbPoly.RefakatKarti.First(a =>
                    a.RefakatNo == editPartisonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" &&
                    a.IslemBitisTarihi == null && a.IslemNo != "900");
                refKart.SabitlenenMakineNo = "1-srcn";
                refKart.IslemBitisTarihi = DateTime.Now;
                _dbPoly.SaveChanges();
                partiSonuBilgiDegisiklikYap(0, editPartisonuTakip.PartiSonuTakipId);
            }

            TempDataCRUDOlustur(2);
            return RedirectToAction("PaketlemePartiSonuBekleyenListe", "Paketleme",
                new { id = 3 });
        }

        public ActionResult PaketlemeGunlukTalimatlari(int page = 1, int pageSize = 30, int id = 0)
        {
            var model = new PaketlemeGunlukTalimatModel();
            model.GunlukTalimatTipleri = _dbPoly.SrcnPaketlemeGunlukTalimatTipis.AsNoTracking()
                .OrderBy(a => a.PaketlemeGunlukTalimatTipiAdi).ToList();
            if (id != 0)
            {
                model.GunlukTalimatTipi = model.GunlukTalimatTipleri.First(a => a.PaketlemeGunlukTalimatTipi == id);
                PagedList<SrcnPaketlemeGunlukTalimats> pmodel = new PagedList<SrcnPaketlemeGunlukTalimats>(
                    _dbPoly.SrcnPaketlemeGunlukTalimats.Where(a => a.PaketlemeGunlukTalimatTipi == id).OrderByDescending(a => a.TalimatTarihiDatetime).ToList(), page,
                    pageSize);
                model.GunlukTalimatlar = pmodel;
                model.GunlukTalimat = new SrcnPaketlemeGunlukTalimats()
                { TalimatTarihi = DateTime.Now.ToShortDateString(), PaketlemeGunlukTalimatTipi = id };

            }


            return View(model);
        }

        public ActionResult PaketlemeGunlukTalimatEkle(int id)
        {
            var PartilerDrop = _dbPoly.SrcnPartiAnaKlasors.OrderBy(a => a.PartiAdi).GroupBy(a => new { a.PartiAdi, a.PartiAnaKlasorId }).Select(a => new DropItem() { Tanim = a.Key.PartiAdi, Id = a.Key.PartiAnaKlasorId.ToString() }).OrderBy(a => a.Id).ToList();


            var model = new PaketlemeGunlukTalimatModel
            {
                Partiler = new SelectList(PartilerDrop, "Id", "Tanim"),
                GunlukTalimat = new SrcnPaketlemeGunlukTalimats()
                { TalimatTarihi = DateTime.Now.ToShortDateString(), PaketlemeGunlukTalimatTipi = id },
                GunlukTalimatTipi = _dbPoly.SrcnPaketlemeGunlukTalimatTipis.Find(id)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult PaketlemeGunlukTalimatEkle(PaketlemeGunlukTalimatModel model)
        {



            var talimat = model.GunlukTalimat;
            var tarih = Convert.ToDateTime(talimat.TalimatTarihi);
            var talimatTipi = _dbPoly.SrcnPaketlemeGunlukTalimatTipis.Find(talimat.PaketlemeGunlukTalimatTipi);
            if (_dbPoly.SrcnPaketlemeGunlukTalimats.Any(a => a.TalimatTarihiDatetime == tarih && a.PaketlemeGunlukTalimatTipi == talimatTipi.PaketlemeGunlukTalimatTipi))
            {
                TempDataOlustur("Seçilen Tarihe ait, bu paketleme alanında talimat bulunmaktadır.", false);
                return RedirectToAction("PaketlemeGunlukTalimatlari", "Paketleme", new { id = talimatTipi.PaketlemeGunlukTalimatTipi });
            }
            else
            {
                var yeniKayit = new SrcnPaketlemeGunlukTalimats()
                {
                    KayitTarihi = DateTime.Now,
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulId = Kullanici.KullaniciId,
                    TalimatTarihi = tarih.ToShortDateString(),
                    TalimatTarihiDatetime = tarih,
                    PaketlemeGunlukTalimatTipi = talimatTipi.PaketlemeGunlukTalimatTipi,
                    PaketlemeGunlukTalimatTipiAdi = talimatTipi.PaketlemeGunlukTalimatTipiAdi
                };
                _dbPoly.SrcnPaketlemeGunlukTalimats.Add(yeniKayit);
                _dbPoly.SaveChanges();
                GunlukTalimatLog(0, yeniKayit, new List<SrcnPaketlemeGunlukTalimatItems>());
                TempDataOlustur("Talimat Oluşturulmuştur. Detayları düzenleyebilirsiniz", true);

                foreach (var item in _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => a.GizlemeDurumu == null).ToList())
                {
                    item.GizlemeDurumu = "0";
                    _dbPoly.SaveChanges();
                }
                return RedirectToAction("PaketlemeGunlukTalimatDuzenle", "Paketleme",
                    new { id = yeniKayit.PaketlemeGunlukTalimatId });
            }

        }

        public ActionResult PaketlemeGunlukTalimatDuzenle(int id)
        {
            var PartilerDrop = _dbPoly.SrcnPartiAnaKlasors.OrderBy(a => a.PartiAdi).GroupBy(a => new { a.PartiAdi, a.PartiAnaKlasorId }).Select(a => new DropItem() { Tanim = a.Key.PartiAdi, Id = a.Key.PartiAnaKlasorId.ToString() }).OrderBy(a => a.Id).ToList();
            var BirimlerDrop = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).GroupBy(a => new { a.BirimAdi, a.BirimId }).Select(a => new DropItem() { Tanim = a.Key.BirimAdi, Id = a.Key.BirimId.ToString() }).OrderBy(a => a.Id).ToList();
            var model = new PaketlemeGunlukTalimatModel
            {
                Partiler = new SelectList(PartilerDrop, "Id", "Tanim"),
                Birimler = new SelectList(BirimlerDrop, "Id", "Tanim"),
                GunlukTalimat = _dbPoly.SrcnPaketlemeGunlukTalimats.Find(id),
                GunlukTalimatItemlar = _dbPoly.SrcnPaketlemeGunlukTalimatItems
                    .Where(a => a.PaketlemeGunlukTalimatId == id).OrderBy(a => a.KayitTarihi).ToList(),
                GunlukTalimatItem = new SrcnPaketlemeGunlukTalimatItems() { PaketlemeGunlukTalimatId = id, GizlemeDurumu = "0" },

            };
            return View(model);
        }

        [ValidateInput(false)]

        [HttpPost]
        public ActionResult PaketlemeGunlukTalimatItemEkleDuzenle(PaketlemeGunlukTalimatModel model)
        {
            var talimat = _dbPoly.SrcnPaketlemeGunlukTalimats.Find(model.GunlukTalimat.PaketlemeGunlukTalimatId);
            var talimatItem = model.GunlukTalimatItem;
            bool SorunVarMi = false;
            if (talimatItem.PartiAnaKlasorId != null)
            {
                if (talimatItem.PartiAnaKlasorId == 0 && talimatItem.TalimatBaslik == null)
                {
                    SorunVarMi = true;
                }
            }
            else
            {
                if (talimatItem.TalimatBaslik == null)
                {
                    SorunVarMi = true;
                }
            }

            if (talimatItem.TalimatIcerik == null)
            {
                SorunVarMi = true;
            }

            if (SorunVarMi)
            {
                TempDataOlustur("Bilgileri Eksik Doldurdunuz Lütfen Kontrol Ediniz", false);
            }
            else
            {
                var yeniItem = new SrcnPaketlemeGunlukTalimatItems()
                {
                    KayitTarihi = DateTime.Now,
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulId = Kullanici.KullaniciId,
                    PaketlemeGunlukTalimatId = talimat.PaketlemeGunlukTalimatId,
                    TalimatBaslik = talimatItem.TalimatBaslik,
                    TalimatIcerik = talimatItem.TalimatIcerik,
                    GizlemeDurumu = talimatItem.GizlemeDurumu
                };
                if (talimatItem.PartiAnaKlasorId > 0)
                {
                    var anaKlasor = _dbPoly.SrcnPartiAnaKlasors.Find(talimatItem.PartiAnaKlasorId);
                    yeniItem.PartiAnaKlasorId = anaKlasor.PartiAnaKlasorId;
                    yeniItem.PartiAdi = anaKlasor.PartiAdi;
                    if (yeniItem.TalimatBaslik == null)
                    {
                        yeniItem.TalimatBaslik = anaKlasor.PartiAdi;
                    }

                }

                _dbPoly.SrcnPaketlemeGunlukTalimatItems.Add(yeniItem);
                _dbPoly.SaveChanges();
                TempDataCRUDOlustur(1);
                GunlukTalimatLog(2, _dbPoly.SrcnPaketlemeGunlukTalimats.Find(yeniItem.PaketlemeGunlukTalimatId), new List<SrcnPaketlemeGunlukTalimatItems>() { yeniItem });
            }
            /*

            if ((talimatItem.TalimatBaslik == null && talimatItem)
            {
                TempDataOlustur("Lütfen Başlık ve içerik belirtiniz", false);
            }

            if (model.PaketlemeGunlukTalimatItemId == 0)
            {
                // yeni kayıt
                var item = new SrcnPaketlemeGunlukTalimatItems
                {
                    KayitTarihi = DateTime.Now,
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulId = Kullanici.KullaniciId,
                    PaketlemeGunlukTalimatId = id,
                    TalimatBaslik = model.TalimatBaslik,
                    TalimatIcerik = model.TalimatIcerik
                };

                GunlukTalimatLog(2, _dbPoly.SrcnPaketlemeGunlukTalimats.Find(item.PaketlemeGunlukTalimatId), new List<SrcnPaketlemeGunlukTalimatItems>(){item});
                _dbPoly.SrcnPaketlemeGunlukTalimatItems.Add(item);
                _dbPoly.SaveChanges();
                TempDataCRUDOlustur(1);
            }
     */

            return RedirectToAction("PaketlemeGunlukTalimatDuzenle", "Paketleme", new { id = talimat.PaketlemeGunlukTalimatId });
        }

        public ActionResult PaketlemeGunlukTalimatSil(int id)
        {
            var talimat = _dbPoly.SrcnPaketlemeGunlukTalimats.Find(id);
            var items = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => a.PaketlemeGunlukTalimatId == id).ToList();
            GunlukTalimatLog(1, talimat, items);
            _dbPoly.SrcnPaketlemeGunlukTalimats.Remove(talimat);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnPaketlemeGunlukTalimatItems.RemoveRange(items);
            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(3);
            return RedirectToAction("PaketlemeGunlukTalimatlari");
        }

        public ActionResult PaketlemeGunlukTalimatItemSil(int id)
        {
            var item = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Find(id);
            var idd = item.PaketlemeGunlukTalimatId;
            GunlukTalimatLog(4, _dbPoly.SrcnPaketlemeGunlukTalimats.Find(item.PaketlemeGunlukTalimatId), new List<SrcnPaketlemeGunlukTalimatItems>() { item });
            _dbPoly.SrcnPaketlemeGunlukTalimatItems.Remove(item);
            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(3);
            return RedirectToAction("PaketlemeGunlukTalimatDuzenle", "Paketleme", new { id = idd });

        }

        public ActionResult PaketlemeGunlukTalimatIndir(int id)
        {

            return PaketlemeGunlukTalimatPdf(id);

        }


        public ActionResult PaketlemeGunlukTalimatAnaliz()
        //{  var baslik =_dbPoly.SrcnPaketlemeGunlukTalimatItems.ToList();
        //    SelectList list = new SelectList(baslik, "PaketlemeGunlukTalimatItemId", "TalimatBaslik");
        //    ViewBag.TalimatBaslik = list;

        // Filter down if necessary
        { 
            // var ara = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => a.TalimatBaslik.Contains(ara.ToString())).ToList();
            var PartilerDrop = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => a.PartiAnaKlasorId > 0).OrderBy(a => a.PartiAdi).GroupBy(a => new { a.PartiAdi, a.PartiAnaKlasorId }).Select(a => new DropItem() { Tanim = a.Key.PartiAdi, Id = a.Key.PartiAnaKlasorId.ToString() }).Distinct().OrderBy(a => a.Id).ToList();

                var GunlukTalimatTipiDrop = _dbPoly.SrcnPaketlemeGunlukTalimatTipis.OrderBy(a => a.PaketlemeGunlukTalimatTipiAdi).GroupBy(a => new { a.PaketlemeGunlukTalimatTipiAdi, a.PaketlemeGunlukTalimatTipi }).Select(a => new DropItem() { Tanim = a.Key.PaketlemeGunlukTalimatTipiAdi, Id = a.Key.PaketlemeGunlukTalimatTipi.ToString() }).Distinct().OrderBy(a => a.Id).ToList();
            var Basliklar = _dbPoly.SrcnPaketlemeGunlukTalimatItems.OrderBy(a => a.PaketlemeGunlukTalimatId).GroupBy(a => new { a.TalimatBaslik, a.TalimatIcerik }).Select(a => new DropItem() { Tanim = a.Key.TalimatBaslik, Id = a.Key.TalimatIcerik.ToString() }).Distinct().OrderBy(a => a.Id).ToList();

            var model = new PaketlemeGunlukTalimatModel
                {
                    Partiler = new SelectList(PartilerDrop, "Id", "Tanim"),
                    GunlukTalimatTipiDrop = new SelectList(GunlukTalimatTipiDrop, "Id", "Tanim"),
                    Baslik = new SelectList(Basliklar,"Id","Tanim")
                };
                var FiltreSecimleri = new List<DropItem>
            {
                new DropItem() {Tanim = "Paketleme Alanı Filtresi Yapılsın", IdInt = 1},
                new DropItem() {Tanim = "Tarih Alanı Filtresi Yapılsın", IdInt = 2},
                new DropItem() {Tanim = "Parti Alanı Filtresi Yapılsın", IdInt = 3},
                 new DropItem() {Tanim = "Talimat Başlığı Alanı Filtresi Yapılsın", IdInt = 4}
            };
                model.FiltreSecimleri = FiltreSecimleri;
                return View(model);
            
        }

        [HttpPost]
        public ActionResult PaketlemeGunlukTalimatAnaliz(PaketlemeGunlukTalimatModel model)
        {
            var PartilerDrop = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => a.PartiAnaKlasorId > 0).OrderBy(a => a.PartiAdi).GroupBy(a => new { a.PartiAdi, a.PartiAnaKlasorId }).Select(a => new DropItem() { Tanim = a.Key.PartiAdi, Id = a.Key.PartiAnaKlasorId.ToString() }).Distinct().OrderBy(a => a.Id).ToList();
            var GunlukTalimatTipiDrop = _dbPoly.SrcnPaketlemeGunlukTalimatTipis.OrderBy(a => a.PaketlemeGunlukTalimatTipiAdi).GroupBy(a => new { a.PaketlemeGunlukTalimatTipiAdi, a.PaketlemeGunlukTalimatTipi }).Select(a => new DropItem() { Tanim = a.Key.PaketlemeGunlukTalimatTipiAdi, Id = a.Key.PaketlemeGunlukTalimatTipi.ToString() }).Distinct().OrderBy(a => a.Id).ToList();


            var Basliklar = _dbPoly.SrcnPaketlemeGunlukTalimatItems.OrderBy(a => a.PaketlemeGunlukTalimatId).GroupBy(a => new { a.TalimatBaslik, a.TalimatIcerik }).Select(a => new DropItem() { Tanim = a.Key.TalimatBaslik, Id = a.Key.TalimatBaslik.ToString() }).Distinct().OrderBy(a => a.Id).ToList();

            var FiltreSecimleri = new List<DropItem>
            {
                new DropItem() {Tanim = "Paketleme Alanı Filtresi Yapılsın", IdInt = 1},
                new DropItem() {Tanim = "Tarih Alanı Filtresi Yapılsın", IdInt = 2},
                new DropItem() {Tanim = "Parti Alanı Filtresi Yapılsın", IdInt = 3},
                new DropItem() {Tanim = "Talimat Başlığı Alanı Filtresi Yapılsın", IdInt = 4}
            };

            var liste = new List<SrcnPaketlemeGunlukTalimatItems>();
            int PAkAlani = 0;
            int PartiAnaKlasorId = 0;
            string TalimatBaslikk ="";
            string tarih = null;
            bool filtreUygulandiMi = false;
            foreach (var item in model.FiltreSecimleri)
            {
                item.Tanim = FiltreSecimleri.First(a => a.IdInt == item.IdInt).Tanim;

                if (item.IdInt == 1 && item.SeciliMi)
                {
                    PAkAlani = model.GunlukTalimatTipiId;
                }
                if (item.IdInt == 2 && item.SeciliMi)
                {
                    tarih = Convert.ToDateTime(model.SecilenTarih).ToShortDateString();
                }
                if (item.IdInt == 3 && item.SeciliMi)
                {
                    PartiAnaKlasorId = model.PartiAnaKlasorId;
                }
                if (item.IdInt == 4 && item.SeciliMi)
                {
                   
                    TalimatBaslikk = model.TalimatBaslik;
                }

            }
            if (TalimatBaslikk !=null)
            {
                filtreUygulandiMi = true;
                liste = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a=>a.TalimatBaslik == TalimatBaslikk && a.GizlemeDurumu == "0")
                    .ToList();
            }
            if (PartiAnaKlasorId > 0)
            {
                filtreUygulandiMi = true;
                liste = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => a.PartiAnaKlasorId == PartiAnaKlasorId && a.GizlemeDurumu == "0")
                    .ToList();
            }
            if (tarih != null)
            {
                var araListe = _dbPoly.SrcnPaketlemeGunlukTalimats.Where(a => a.TalimatTarihi == tarih).Select(a => a.PaketlemeGunlukTalimatId).ToList();

                if (filtreUygulandiMi)
                {
                    liste = liste.Where(a => araListe.Any(b => b == a.PaketlemeGunlukTalimatId && a.GizlemeDurumu == "0")).ToList();

                }
                else
                {
                    filtreUygulandiMi = true;

                    liste = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => araListe.Any(b => b == a.PaketlemeGunlukTalimatId && a.GizlemeDurumu == "0")).ToList();

                }

            }

            if (PAkAlani > 0)
            {
                var araListe = _dbPoly.SrcnPaketlemeGunlukTalimats.Where(a => a.PaketlemeGunlukTalimatTipi == PAkAlani).Select(a => a.PaketlemeGunlukTalimatId).ToList();

                if (filtreUygulandiMi)
                {
                    liste = liste.Where(a => araListe.Any(b => b == a.PaketlemeGunlukTalimatId && a.GizlemeDurumu == "0")).ToList();

                }
                else
                {
                    filtreUygulandiMi = true;

                    liste = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => araListe.Any(b => b == a.PaketlemeGunlukTalimatId && a.GizlemeDurumu == "0")).ToList();

                }

            }

            model.Partiler = new SelectList(PartilerDrop, "Id", "Tanim");
            model.GunlukTalimatTipiDrop = new SelectList(GunlukTalimatTipiDrop, "Id", "Tanim");
            model.Baslik = new SelectList(Basliklar, "Id", "Tanim");
            model.GunlukTalimatItemlar = liste.OrderByDescending(a => a.KayitTarihi).ThenBy(a => a.TalimatBaslik).ToList();

            return View(model);
        }

        public ActionResult PaketlemeGunlukTalimatItemDurumDegisiklik(int id)
        {
            var item = _dbPoly.SrcnPaketlemeGunlukTalimatItems.Find(id);
            if (item.GizlemeDurumu == "1")
            {
                // gizlensin
                item.GizlemeDurumu = "0";
                _dbPoly.SaveChanges();
            }
            else
            {
                // gösterilsin
                item.GizlemeDurumu = "1";
                _dbPoly.SaveChanges();
            }
            TempDataCRUDOlustur(2);
            return RedirectToAction("PaketlemeGunlukTalimatDuzenle", "Paketleme",
                new { id = item.PaketlemeGunlukTalimatId });

        }
        [HttpPost]
        public ActionResult PartiAnaKlasorEkle(PaketlemeGunlukTalimatModel model)
        {
            int idd = model.GunlukTalimat.PaketlemeGunlukTalimatId;
            if (model.YeniPartiAdi == null || model.BirimId == 0)
            {
                TempDataOlustur("Parti Adı veya Birim Seçmediniz", false);
            }
            else
            {
                if (_dbPoly.SrcnPartiAnaKlasors.Any(a => a.PartiAdi.ToUpper() == model.YeniPartiAdi.ToUpper()))
                {
                    TempDataOlustur("Böyle Bir Parti Bulunmaktadır. " + model.YeniPartiAdi, false);
                }
                else
                {
                    var birim = _dbPoly.SrcnFabrikaBirims.Find(model.BirimId);
                    _dbPoly.SrcnPartiAnaKlasors.Add(new SrcnPartiAnaKlasors()
                    {
                        BirimId = birim.BirimId,
                        BirimAdi = birim.BirimAdi,
                        PartiAdi = model.YeniPartiAdi,
                        OlusturmaTarihi = DateTime.Now
                    });
                    _dbPoly.SaveChanges();
                    TempDataOlustur("Parti Kayıdı Yapılmıştır", true);
                }
            }

            return RedirectToAction("PaketlemeGunlukTalimatDuzenle", "Paketleme", new { id = idd });
        }

        public ActionResult PaketlemeTefrikRapor()
        {
            return View(new PaketlemeTefrikModel() { BaslangicTarihi = DateTime.Now.AddDays(-1).Date.ToString(), BitisTarihi = DateTime.Now.Date.ToString() });
        }

        public FileContentResult PaketlemeTefrikModeliOlustur(PaketlemeTefrikModel gelenModel)
        {
            var model = new PaketlemeTefrikModel()
            {
                BaslangicTarihi = gelenModel.BaslangicTarihi,
                BitisTarihi = gelenModel.BitisTarihi
            };

            model.BaslangicTarihiDateTime = Convert.ToDateTime(model.BaslangicTarihi);
            model.BitisTarihiDateTime = Convert.ToDateTime(model.BitisTarihi);

            model.TefrikRaporOzeItems = _dbPoly.ZzzSrcnTefrikRaporOzets
                .Where(a => a.Tarih >= model.BaslangicTarihiDateTime && a.Tarih <= model.BitisTarihiDateTime).OrderByDescending(a => a.Tarih).ToList();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            var headerListe = new List<string>();
            using (var excelWorkbook = new XLWorkbook())
            {
                #region Genel Bilgi Sayfa

                var wsGenelBilgi = excelWorkbook.Worksheets.Add("Genel Bilgi");

                headerListe.Add("Birim");
                headerListe.Add("Toplam Bobin");
                headerListe.Add("Kısa Bobin");
                headerListe.Add("Hatalı Bobin");
                headerListe.Add("HATA ORANI");
                int satir = 1;
                int sutun = 0;

                foreach (var j in headerListe)
                {
                    sutun++;
                    wsGenelBilgi.Cell(satir, sutun).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues
                        .Center);
                    wsGenelBilgi.Cell(satir, sutun).Value = j.ToUpper();
                }

                foreach (var xlColumn in wsGenelBilgi.Columns())
                {
                    xlColumn.Width = 20;
                }

                foreach (var birim in Birimler)
                {
                    satir++;
                    sutun = 0;
                    var birimListe = model.TefrikRaporOzeItems.Where(a => a.BirimId == birim.BirimId).ToList();
                    var TefrikGenelOzetListe = _dbPoly.ZzzSrcnTefrikGenelOzet.Where(a =>
                            a.BirimId == birim.BirimId && a.Tarih >= model.BaslangicTarihiDateTime &&
                            a.Tarih <= model.BitisTarihiDateTime)
                        .ToList();

                    int TopBobin = Convert.ToInt32(TefrikGenelOzetListe.Sum(a => a.ToplamBobin));
                    int KisaBobin = Convert.ToInt32(TefrikGenelOzetListe.Sum(a => a.KısaBobin));
                    int hataliBobin = birimListe.GroupBy(a => new { a.Pozisyon, a.RefakatKartNo, a.UretimSiraNo })
                        .Count();
                   
                      decimal HataOrani = (Convert.ToDecimal(hataliBobin) / Convert.ToDecimal(TopBobin)) * 100;
                   

                    var SatirListe = new List<string>();

                    SatirListe.Add(birim.BirimAdi);
                    SatirListe.Add(TopBobin.ToString());
                    SatirListe.Add(KisaBobin.ToString());
                    SatirListe.Add(hataliBobin.ToString());

                    SatirListe.Add(HataOrani.ToString("F"));
                    foreach (var j in SatirListe)
                    {
                        sutun++;
                        wsGenelBilgi.Cell(satir, sutun).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues
                            .Center);
                        wsGenelBilgi.Cell(satir, sutun).Value = j;
                    }

                }
                #endregion


                foreach (var birim in Birimler)
                {
                    satir = 1;
                    sutun = 0;
                    var wsBirimBilgi = excelWorkbook.Worksheets.Add(birim.BirimAdi.Replace("/", "-").ToUpper());

                    headerListe = new List<string>();
                    headerListe.Add("MAKİNE");
                    headerListe.Add("PARTİ");
                    headerListe.Add("STOK ADI");
                    headerListe.Add("Toplam Bobin");
                    headerListe.Add("Kısa Bobin");
                    headerListe.Add("Hatalı Bobin");
                    headerListe.Add("HATA ORANI");



                    foreach (var j in headerListe)
                    {
                        sutun++;
                        wsBirimBilgi.Cell(satir, sutun).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues
                            .Center);
                        wsBirimBilgi.Cell(satir, sutun).Value = j.ToUpper();
                    }




                    foreach (var xlColumn in wsBirimBilgi.Columns())
                    {
                        xlColumn.Width = 20;
                    }

                    var birimListe = model.TefrikRaporOzeItems.Where(a => a.BirimId == birim.BirimId).GroupBy(a => new { a.MakineNo, a.PartiNo, a.StokAdi, a.BirimId }).ToList();
                    var SatirListe = new List<string>();
                    foreach (var exCellSatir in birimListe)
                    {
                        satir++;
                        sutun = 0;
                        var TefrikGenelOzetListe = _dbPoly.ZzzSrcnTefrikGenelOzet.Where(a =>
                                a.MakineNo == exCellSatir.Key.MakineNo &&
                                a.PartiNo == exCellSatir.Key.PartiNo &&
                                a.StokAdi == exCellSatir.Key.StokAdi &&

                                a.BirimId == birim.BirimId &&
                                a.Tarih >= model.BaslangicTarihiDateTime &&
                                a.Tarih <= model.BitisTarihiDateTime)
                            .ToList();

                        int TopBobin = Convert.ToInt32(TefrikGenelOzetListe.Sum(a => a.ToplamBobin));
                        int KisaBobin = Convert.ToInt32(TefrikGenelOzetListe.Sum(a => a.KısaBobin));
                        int hataliBobin =
                            model.TefrikRaporOzeItems.Where(a => a.BirimId == exCellSatir.Key.BirimId &&
                                                                 a.PartiNo == exCellSatir.Key.PartiNo && 
                                                                 a.StokAdi == exCellSatir.Key.StokAdi &&
                                                                 a.MakineNo == exCellSatir.Key.MakineNo
                                                                 ).ToList().GroupBy(a => new { a.Pozisyon, a.RefakatKartNo, a.UretimSiraNo }).Count();

                        decimal HataOrani = (Convert.ToDecimal(hataliBobin) / Convert.ToDecimal(TopBobin)) * 100;


                        SatirListe = new List<string>();
                        SatirListe.Add(exCellSatir.Key.MakineNo);
                        SatirListe.Add(exCellSatir.Key.PartiNo);
                        SatirListe.Add(exCellSatir.Key.StokAdi.Trim());
                        SatirListe.Add(TopBobin.ToString());
                        SatirListe.Add(KisaBobin.ToString());
                        SatirListe.Add(hataliBobin.ToString());
                        SatirListe.Add(HataOrani.ToString("F1").Replace(",","."));
                        foreach (var j in SatirListe)
                        {
                            sutun++;
                            wsBirimBilgi.Cell(satir, sutun).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            wsBirimBilgi.Cell(satir, sutun).Value = j;
                        }

                    }




                }
                //  MakineNo, RefakatKartNo, PartiNo, StokAdi,BirimId





                //var UniqueStokAdi = birimListe.Select(a => a.StokAdi).Distinct().ToList();
                //var UniqueHatalar = birimListe.Select(a => a.HataNedeni).Distinct().ToList();


                //excelWorkbook.Worksheets.Add(wsGenelBilgi);
                using (MemoryStream stream = new MemoryStream())
                {

                    excelWorkbook.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + ".xlsx");

                }
            }
        }
        [HttpPost]
        public ActionResult PaketlemeTefrikRapor(PaketlemeTefrikModel model)
        {


            return PaketlemeTefrikModeliOlustur(model);
        }

    }
}