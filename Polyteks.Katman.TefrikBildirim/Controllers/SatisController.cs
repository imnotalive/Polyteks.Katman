using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.Has.Controllers;
using Polyteks.Katman.TefrikBildirim.Models;
using System.IO;
using Polyteks.Katman.Helpers;


namespace Polyteks.Katman.Has.Controllers
{
    public class SatisController : BaseController
    {
        public void KayitBilgiMailiGonder(int dosyaTipi, int id)
        {
            // dosya tipi => 2= numune, 3=müşteri
            // kayit tipi=> (numune) 1= müşteri referans ürünlü, 2= teknik spektli

            if (dosyaTipi == 2)
            {
                var numune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);

                string konu = numune.CariAdi.Trim() + " Adına -Numune Yapılabilirlik- talebi oluşturulmuştur ";
                string icerik = "";
                if (numune.NumuneDosyaTipi == 1)
                {

                    // müşteri referanslı
                    string araicerik = "";
                    if (numune.AnalizYapilacakBobinSayisi > 0)
                    {
                        araicerik += numune.AnalizYapilacakBobinSayisi.ToString() + " Adet İplik ";
                    }
                    if (numune.AnalizYapilacakKumasSayisi > 0)
                    {
                        araicerik += numune.AnalizYapilacakKumasSayisi.ToString() + " Adet Kumaş ";
                    }
                    icerik += string.Format(
                        "Analiz Numarası:<b> #{0}</b> </br> Firma: <b>{1}</b> {2} olarak göndermiştir. </br><b>{3}</b>",
                        numune.DosyaDurumId, numune.CariAdi.Trim(), araicerik, numune.Aciklama);
                }
                else
                {
                    //teknik spektli
                    konu += " Müşteri Teknik Spektler ile -Numune Yapılabilirlik- talebi oluşturulmuştur";
                    icerik += string.Format(
                        "<b>Analiz Numarası: #{0}  </br> Firma: {1}  </br> olarak belirtilmiştir.</b></br>",
                        numune.DosyaDurumId, numune.CariAdi);
                }
                icerik += "<br/>Analiz durumu ve sonuçları için hazırlanan ek-ERP sistemini kullanınız...</p>";
                string ccMail = "MDIKICI@polyteks.com.tr";

                List<string> Alicilar = _dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == 10 || a.MailBildirimGrupId == 18 || a.MailBildirimGrupId == 21).Select(a => a.EmailAdres).ToList();

                //List<string> Alicilar = new List<string>();
                //Alicilar.Add(ccMail);
                if (Kullanici.EmailAdres != null)
                {
                    if (Kullanici.EmailAdres.Contains("@polyteks"))
                    {
                        ccMail = Kullanici.EmailAdres;
                    }
                }


                GenelDurumMailGonder(ccMail, Alicilar, icerik, konu);
            }
        }
        public void YeniLabAnalizKaydet(int DosyaTipi, int DosyaId, Cari Cari, int AnalizYapilacakBobinSayisi, int AnalizYapilacakKumasSayisi, int AnaliziYapilanBilesenSayisi)
        {
            var labAnalizDurumu = new SrcnLabAnalizDurumus();

            string DosyaTipiAdi = "NUMUNE YAPILABİLİRLİK";
            if (DosyaTipi == 3)
            {
                DosyaTipiAdi = "MÜŞTERİ ŞİKAYETLERİ";
                labAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(7);

                var yeniAnaliz = new SrcnLabAnalizs()
                {
                    KayitTarihi = DateTime.Now,
                    LabAnalizDurumu = labAnalizDurumu.LabAnalizDurumu,
                    LabAnalizDurumuAdi = labAnalizDurumu.LabAnalizDurumuAdi,
                    IstenenTerminTarihi = DateTime.Now.AddDays(1),
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                    GerceklesenTerminTarihi = null,
                    ImalatAnalizYapilmaTipi = 0,
                    IsEmriNumarasiDurumu = 0,
                    LabAciklama = null,
                    MakineId = 0,
                    PartiNo = null,
                    PlanlananTerminTarihi = null,
                    RefakartKartNo = null,
                    TakimSaati = null,
                    BagliOlduguDosyaId = DosyaId,
                    AnalizAdi = DosyaTipiAdi,
                    CariNo = Cari.CariNo,
                    CariAdi = Cari.CariAdi,
                    AnaliziYapilanBilesenSayisi = AnaliziYapilanBilesenSayisi,
                    AnalizYapilacakBobinSayisi = AnalizYapilacakBobinSayisi,
                    AnalizYapilacakKumasSayisi = AnalizYapilacakKumasSayisi,
                    DosyaTipi = DosyaTipi,
                    DosyaTipiAdi = DosyaTipiAdi,
                    LabGrupAdi = "Müşteri Şikayeti",
                    LabGrupId = 5,
                    AnalizTipi = 2,
                };
                try
                {
                    _dbPoly.SrcnLabAnalizs.Add(yeniAnaliz);
                    _dbPoly.SaveChanges();
                }
                catch (Exception e)
                {

                }
                _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
                {
                    LabAnalizId = yeniAnaliz.LabAnalizId,
                    KayitTarihi = DateTime.Now,
                    HareketAdi = labAnalizDurumu.LabAnalizDurumuAdi + " - " + Kullanici.IsimSoyisim
                });
                _dbPoly.SaveChanges();
                //Müşteri Kayıdı Oluşturuldu
            }
            else
            {
                labAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(4);
                //Analiz Kayıdı Oluşturuldu numune yapılabilirlik
                var yeniAnaliz = new SrcnLabAnalizs()
                {
                    KayitTarihi = DateTime.Now,
                    LabAnalizDurumu = labAnalizDurumu.LabAnalizDurumu,
                    LabAnalizDurumuAdi = labAnalizDurumu.LabAnalizDurumuAdi,
                    IstenenTerminTarihi = DateTime.Now.AddDays(1),
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                    GerceklesenTerminTarihi = null,
                    ImalatAnalizYapilmaTipi = 0,
                    IsEmriNumarasiDurumu = 0,
                    LabAciklama = null,
                    MakineId = 0,
                    PartiNo = null,
                    PlanlananTerminTarihi = null,
                    RefakartKartNo = null,
                    TakimSaati = null,
                    BagliOlduguDosyaId = DosyaId,
                    AnalizAdi = DosyaTipiAdi,
                    CariNo = Cari.CariNo,
                    CariAdi = Cari.CariAdi,
                    AnaliziYapilanBilesenSayisi = AnaliziYapilanBilesenSayisi,
                    AnalizYapilacakBobinSayisi = AnalizYapilacakBobinSayisi,
                    AnalizYapilacakKumasSayisi = AnalizYapilacakKumasSayisi,
                 
                    DosyaTipi = DosyaTipi,
                    DosyaTipiAdi = DosyaTipiAdi,
                    LabGrupAdi = "Numune Yapilabilirlik"
                };
                try
                {
                    _dbPoly.SrcnLabAnalizs.Add(yeniAnaliz);
                    _dbPoly.SaveChanges();
                }
                catch (Exception e)
                {

                }
                _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
                {
                    LabAnalizId = yeniAnaliz.LabAnalizId,
                    KayitTarihi = DateTime.Now,
                    HareketAdi = labAnalizDurumu.LabAnalizDurumuAdi + " - " + Kullanici.IsimSoyisim
                });
                _dbPoly.SaveChanges();
            }


        }
        #region Tanımlamalar
        private static POTA_TASDEntities _appContextTasd;
        private static POTA_PSETEntities _appContextPset;
        private static POTA_TASDEntities _dbTasdCreate()
        {
            if (_appContextTasd == null)
            {
                _appContextTasd = new POTA_TASDEntities() { Configuration = { LazyLoadingEnabled = false, ProxyCreationEnabled = false } };

                return _appContextTasd;
            }
            else
            {
                return _appContextTasd;
            }
        }
        private static POTA_PSETEntities _dbPsetCreate()
        {
            if (_appContextPset == null)
            {
                _appContextPset = new POTA_PSETEntities() { Configuration = { LazyLoadingEnabled = false, ProxyCreationEnabled = false } };

                return _appContextPset;
            }
            else
            {
                return _appContextPset;
            }
        }
        public POTA_TASDEntities _dbTasd = _dbTasdCreate();
        public POTA_PSETEntities _dbPset = _dbPsetCreate();
        #endregion
        public Cari CariGetir(string CariKoduPoly, string CariKoduTasd, string CariKoduPset, int FirmaTipi)
        {
            var Cari = new Cari();
            if (FirmaTipi == 1)
            {
                Cari = _dbPoly.Cari.First(a => a.CariNo == CariKoduPoly);
            }
            if (FirmaTipi == 2)
            {
                Cari = _dbTasd.Cari.First(a => a.CariNo == CariKoduTasd);
            }
            if (FirmaTipi == 3)
            {
                Cari = _dbPset.Cari.First(a => a.CariNo == CariKoduPset);
            }

            return Cari;
        }
        public void NumuneYapilabilirlikDosyaHareketOzet(int id)
        {
            var numuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            var yeniDosyaHareketTanim = _dbPoly.SrcnDosyaDurums.Find(5);
            var Hareket = "<b>Bilgi Değişikleri</b> </br> Cari: " + numuneYapilabilirlik.CariAdi + "</br> ";

            var liste = new List<DropItem>
            {
                new DropItem()
                {
                    Id = "Analiz Yapılacak İplik Sayısı",
                    Tanim = numuneYapilabilirlik.AnalizYapilacakBobinSayisi.ToString()
                },
                new DropItem()
                {
                    Id = "Analiz Yapılacak İplik Sayısı",
                    Tanim = numuneYapilabilirlik.AnalizYapilacakKumasSayisi.ToString()
                },
                                new DropItem()
                {
                    Id = "Analiz Yapılacak Renk Çalışması",
                    Tanim = numuneYapilabilirlik.AnalizYapilacakRenkCalismasi.ToString()
                },
                new DropItem() {Id = "Yapılabilirlik Durumu", Tanim = numuneYapilabilirlik.YapilabilirlikDurumuAdi},
                new DropItem() {Id = "Deneme Yapılma Durumu", Tanim = numuneYapilabilirlik.DenemeYapilmaDurumuAdi},
                new DropItem() {Id = "Siparişe Dönme Durumu", Tanim = numuneYapilabilirlik.SipariseDonmeDurumuAdi},
                new DropItem() {Id = "Deneme Kodu", Tanim = numuneYapilabilirlik.DenemeKodu},
                new DropItem() {Id = "Sipariş No", Tanim = numuneYapilabilirlik.SiparisNo},
                new DropItem() {Id = "Açıklama", Tanim = numuneYapilabilirlik.Aciklama}
            };

            foreach (var item in liste)
            {
                string DurumAdi = item.Tanim;
                if (DurumAdi == null)
                {
                    DurumAdi = "Belirlenmedi";
                }
                Hareket += string.Format("{0} : {1} </br> ", item.Id, DurumAdi);
            }
            _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
            {
                KayitTarihi = DateTime.Now,
                DosyaHareketTanimId = yeniDosyaHareketTanim.DosyaDurumId,
                SikayetNumuneDosyaId = id,
                DosyaAdi = "Numune Yapılabilirlik",
                DosyaTipi = 3,
                HareketAdi = Hareket + Kullanici.IsimSoyisim
            });
            _dbPoly.SaveChanges();
        }
        public void MusteriSikayetDosyaHareketOzetEkle(int id)
        {
            var sikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            var sikayetItems = _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.AsNoTracking().Where(a => a.MusteriSikayetDosyaId == id).Select(a => a.MusteriSikayetGrupId).ToList();
            var SikayetGruplar = _dbPoly.SrcnMusteriSikayetGrups
                .Where(a => sikayetItems.Any(b => b == a.MusteriSikayetGrupId)).AsNoTracking().ToList();

            var AltItemler = new List<SrcnMusteriSikayetGrups>();
            var AnaItemler = new List<SrcnMusteriSikayetGrups>();
            foreach (var item in SikayetGruplar)
            {

                if (AltItemler.Count(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId) == 0)
                {
                    AltItemler.Add(item);
                    if (AnaItemler.Count(a => a.ItemTipi == item.UstItemTipi) == 0)
                    {
                        AnaItemler.Add(new SrcnMusteriSikayetGrups
                        {
                            ItemTipi = item.UstItemTipi,
                            MusteriSikayetGrupAdi = item.UstMusteriSikayetGrupAdi
                        });
                    }
                }
            }
            var Hareket = "Cari: " + sikayet.CariAdi + "</br>";
            AnaItemler = AnaItemler.OrderBy(a => a.ItemTipi).ToList();
            foreach (var anaItem in AnaItemler)
            {
                Hareket += anaItem.MusteriSikayetGrupAdi + ": ";
                foreach (var altItem in AltItemler.Where(a => a.UstItemTipi == anaItem.ItemTipi).OrderBy(a => a.MusteriSikayetGrupAdi).ToList())
                {
                    Hareket += altItem.MusteriSikayetGrupAdi + ", ";
                }

                Hareket += "</br>";
            }
            var yeniDosyaHareketTanim = _dbPoly.SrcnDosyaDurums.Find(5);
            _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
            {
                KayitTarihi = DateTime.Now,
                DosyaHareketTanimId = yeniDosyaHareketTanim.DosyaDurumId,
                SikayetNumuneDosyaId = id,
                DosyaAdi = "Müşteri Şikayetleri",
                DosyaTipi = 3,
                HareketAdi = Hareket + Kullanici.IsimSoyisim
            });
            _dbPoly.SaveChanges();
        }

        #region Metotlar
        public SatisModel SatisModelDosyalariListele(int DosyaTipi, int KategoriDosyaTipi, int DosyaDurumuId)
        {
            var model = new SatisModel();

            if (DosyaTipi == 2)
            {
                model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 2).OrderBy(a => a.DosyaDurumId)
                    .ToList();

                //numune
                model.NumuneDosyaTipleri = _dbPoly.SrcnNumuneDosyaTipis.Where(a => a.NumuneDosyaTipiKategoriTipi == 1)
                    .OrderBy(a => a.NumuneDosyaTipiKategoriTipi).ToList();

                if (KategoriDosyaTipi > 0)
                {

                    model.NumuneDosyaTipi = _dbPoly.SrcnNumuneDosyaTipis.Find(KategoriDosyaTipi);

                    if (DosyaDurumuId > 0)
                    {
                        model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(DosyaDurumuId);
                        model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.NumuneDosyaTipi == KategoriDosyaTipi && a.DosyaDurumId == DosyaDurumuId)
                            .OrderByDescending(a => a.NumuneYapilabilirlikDosyaId).ToList();
                    }
                    //else
                    //{
                    //    model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.NumuneDosyaTipi == KategoriDosyaTipi)
                    //        .OrderByDescending(a => a.NumuneYapilabilirlikDosyaId).ToList();
                    //}

                }

            }
            else
            {
                //şikayet
                model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaTipi == 4).OrderBy(a => a.DosyaDurumId)
                    .ToList();

                if (DosyaDurumuId > 0)
                {
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(DosyaDurumuId);

                    model.MusteriSikayetDosyalar = _dbPoly.SrcnMusteriSikayetDosyas
                        .Where(a => a.DosyaDurumId == DosyaDurumuId).OrderByDescending(a => a.MusteriSikayetDosyaId)
                        .ToList();
                }
            }

            return model;
        }
        public SatisModel SatisModelDosyayiGetir(int dosyaTipi, int id)
        {
            var model = new SatisModel { };

            if (Session["CarilerPoly"] == null)
            {
                model.CarilerPoly = new SelectList(_dbPoly.Cari.AsNoTracking().OrderBy(a => a.CariAdi).ToList(), "CariNo", "CariAdi");
                Session["CarilerPoly"] = model.CarilerPoly;
            }
            else
            {
                model.CarilerPoly = (SelectList)Session["CarilerPoly"];
            }
            if (Session["CarilerTasd"] == null)
            {
                model.CarilerTasd = new SelectList(_dbTasd.Cari.AsNoTracking().OrderBy(a => a.CariAdi).ToList(), "CariNo", "CariAdi");
                Session["CarilerTasd"] = model.CarilerPoly;
            }
            else
            {
                model.CarilerTasd = (SelectList)Session["CarilerTasd"];
            }
            if (Session["CarilerPset"] == null)
            {
                model.CarilerPset = new SelectList(_dbPset.Cari.AsNoTracking().OrderBy(a => a.CariAdi).ToList(), "CariNo", "CariAdi");
            }
            else
            {
                model.CarilerPset = (SelectList)Session["CarilerPset"];
            }


            if (dosyaTipi == 2)
            {
                model.NumuneDosyaTipleri = _dbPoly.SrcnNumuneDosyaTipis.Where(a => a.NumuneDosyaTipiKategoriTipi == 1)
                    .OrderBy(a => a.NumuneDosyaTipiKategoriTipi).ToList();

                model.NumuneYapilabilirlikDosya = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
                // numune
                model.LabAnalizleri = _dbPoly.SrcnLabAnalizs
                    .Where(a => a.DosyaTipi == 2 && a.BagliOlduguDosyaId == id).OrderByDescending(a => a.KayitTarihi).ToList();
                model.DosyaResimleri = _dbPoly.SrcnResims
                    .Where(a => a.ResimKategoriId == 2 && a.BagliOlduguDosyaId == id)
                    .OrderByDescending(a => a.ResimId).ToList();
                model.DosyaHareketleri = _dbPoly.SrcnDosyaHarekets
                    .Where(a => a.DosyaTipi == 2 && a.SikayetNumuneDosyaId == id)
                    .OrderByDescending(a => a.KayitTarihi).ToList();
            }
            if (dosyaTipi == 3)
            {
                
                var dosyaItems = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.MusteriSikayetDosyaId == id)
                    .FirstOrDefault();
                model.MusteriSikayetDosya = dosyaItems;
                model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 37).ToList();
                model.LabAnalizleri = _dbPoly.SrcnLabAnalizs
                    .Where(a => a.DosyaTipi == 3 && a.BagliOlduguDosyaId == id).OrderByDescending(a => a.KayitTarihi).ToList();
                model.DosyaResimleri = _dbPoly.SrcnResims
                    .Where(a => a.ResimKategoriId == 3 && a.BagliOlduguDosyaId == id)
                    .OrderByDescending(a => a.ResimId).ToList();
                model.DosyaHareketleri = _dbPoly.SrcnDosyaHarekets
                    .Where(a => a.DosyaTipi == 4 && a.SikayetNumuneDosyaId == id)
                    .OrderByDescending(a => a.KayitTarihi).ToList();
                //şikayet
                var SikayetItems = _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.AsNoTracking()
                    .Where(a => a.MusteriSikayetDosyaId == id).ToList();
                foreach (var item in _dbPoly.SrcnMusteriSikayetGrups.Where(a => a.UstItemTipi == 0 && a.ItemTipi > 0 && a.ItemTipi < 6).OrderBy(a => a.ItemTipi).ToList())
                {
                    var araItem = new MusteriSikayetGrupItemModel()
                    {
                        AnaGrup = item,
                        AltItemGruplar = _dbPoly.SrcnMusteriSikayetGrups.AsNoTracking().Where(a => a.UstItemTipi == item.ItemTipi).OrderBy(a => a.MusteriSikayetGrupAdi).ToList()
                    };




                    foreach (var altItem in araItem.AltItemGruplar)
                    {
                        if (SikayetItems.Any(a => a.MusteriSikayetGrupId == altItem.MusteriSikayetGrupId))
                        {
                            if (araItem.AnaGrup.RadioSecimMi == "1")
                            {
                                araItem.RadioId = altItem.MusteriSikayetGrupId;
                            }
                            else
                            {
                                altItem.SeciliMi = true;
                            }


                        }
                    }
                    model.MusteriSikayetGrupItemModeller.Add(araItem);
                }
            }
            return model;
        }
        // GET: Satis
        public SatisModel LabAnalizeGoreSatisModeliGetir(int LabAnalizId)
        {
            var model = new SatisModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(LabAnalizId);
            var labgrup = _dbPoly.SrcnLabGrups.Find(labAnaliz.LabGrupId);
            model.LabAnaliz = labAnaliz;
            model.LabGrup = labgrup;
            LabGruplari = new List<SrcnLabGrups>();
            var aralabgrup = _dbPoly.SrcnLabGrups.First(a => a.AnalizTipi == labgrup.AnalizTipi && a.UstLabGrupId == 0);
            RecursiveLabGrups(aralabgrup.LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();

            if (labAnaliz.AnalizTipi == 2)
            {
                //   Müşteri şikayeti
                var sikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(labAnaliz.BagliOlduguDosyaId);
                model.MusteriSikayetDosya = sikayet;
            }
            else
            {
                // numune yapılabilirlik
                var yapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(labAnaliz.BagliOlduguDosyaId);
                model.NumuneYapilabilirlikDosya = yapilabilirlik;
            }
            var labAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == LabAnalizId).OrderBy(a => a.YardimciTesisKontrolNoktaAdi).ToList();
            var SecilenLabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
            foreach (var analizItem in labAnalizItems)
            {
                var item = new LabAnalizDetayModel()
                {
                    LabAnalizItem = analizItem,
                    LabAnalizItemAnalizCesitSonuclari = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Where(a => a.LabAnalizItemId == analizItem.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi).ToList()
                };
                foreach (var aa in item.LabAnalizItemAnalizCesitSonuclari)
                {
                    if (SecilenLabAnalizCesitleri.Count(a => a.LabAnalizCesitId == aa.LabAnalizCesitId && a.LabAnalizCesitAdi == aa.LabAnalizCesitAdi) == 0)
                    {
                        SecilenLabAnalizCesitleri.Add(new SrcnLabAnalizCesits()
                        {
                            LabAnalizCesitAdi = aa.LabAnalizCesitAdi,
                            LabAnalizCesitId = aa.LabAnalizCesitId
                        });
                    }
                }
                model.LabAnalizDetayModeller.Add(item);

            }
            model.LabAnalizCesitleri = SecilenLabAnalizCesitleri.OrderBy(a => a.LabAnalizCesitAdi).ToList();
            model.LabAnalizLoglari = _dbPoly.SrcnLabAnalizLogs.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId)
                .OrderByDescending(a => a.KayitTarihi).ToList();
            model.AnalizHareketleri = _dbPoly.SrcnLabAnalizHarekets.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId)
                .OrderByDescending(a => a.KayitTarihi).ToList();
            return model;
        }
        public SatisModel SatisModeliBaseOlustur(int DosyaTipi, int KategoriDosyaTipi, int DosyaId)
        {
            // int DosyaTipi => 2= numune, 3=müşteri şikateri
            // KategoriDosyaTipi =>1 => (numune)NumuneDosyaTipiKategoriTipi

            var model = new SatisModel();
            model.Kullanici = Kullanici;
            if (DosyaId == 0)
            {

                if (Session["CarilerPoly"] == null)
                {
                    model.CarilerPoly = new SelectList(_dbPoly.Cari.AsNoTracking().OrderBy(a => a.CariAdi).ToList(), "CariNo", "CariAdi");
                    Session["CarilerPoly"] = model.CarilerPoly;
                }
                else
                {
                    model.CarilerPoly = (SelectList)Session["CarilerPoly"];
                }
                if (Session["CarilerTasd"] == null)
                {
                    model.CarilerTasd = new SelectList(_dbTasd.Cari.AsNoTracking().OrderBy(a => a.CariAdi).ToList(), "CariNo", "CariAdi");
                    Session["CarilerTasd"] = model.CarilerPoly;
                }
                else
                {
                    model.CarilerTasd = (SelectList)Session["CarilerTasd"];
                }
                if (Session["CarilerPset"] == null)
                {
                    model.CarilerPset = new SelectList(_dbPset.Cari.AsNoTracking().OrderBy(a => a.CariAdi).ToList(), "CariNo", "CariAdi");
                }
                else
                {
                    model.CarilerPset = (SelectList)Session["CarilerPset"];
                }

            }
            if (DosyaTipi == 2)
            {
                //numune
                model.NumuneDosyaTipleri = _dbPoly.SrcnNumuneDosyaTipis.Where(a => a.NumuneDosyaTipiKategoriTipi == 1)
                    .OrderBy(a => a.NumuneDosyaTipiKategoriTipi).ToList();
                if (DosyaId != 0)
                {
                    var argeyorumuSonrasiDurumlarDrop = _dbPoly.SrcnDosyaDurums
                        .Where(a => a.DosyaDurumId == 10 || (a.DosyaDurumId > 14 && a.DosyaDurumId <= 20))
                        .Select(a => new DropItem() { Tanim = a.DosyaDurumAdi, IdInt = a.DosyaDurumId }).ToList();

                    var numune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(DosyaId);
                    model.GenelSelectList = new SelectList(argeyorumuSonrasiDurumlarDrop.OrderBy(a => a.IdInt), "IdInt", "Tanim");
                    model.NumuneYapilabilirlikDosya = numune;
                    model.NumuneDosyaTipi = _dbPoly.SrcnNumuneDosyaTipis.First(a => a.NumuneDosyaTipi == numune.NumuneDosyaTipi);
                    if (model.NumuneYapilabilirlikDosya.FirmaTipi == 1)
                    {
                        model.CariKoduPoly = model.NumuneYapilabilirlikDosya.CariKodu;
                    }
                    if (model.NumuneYapilabilirlikDosya.FirmaTipi == 2)
                    {
                        model.CariKoduTasd = model.NumuneYapilabilirlikDosya.CariKodu;
                    }
                    if (model.NumuneYapilabilirlikDosya.FirmaTipi == 3)
                    {
                        model.CariKoduPset = model.NumuneYapilabilirlikDosya.CariKodu;
                    }
                }
                else
                {
                    if (KategoriDosyaTipi > 0)
                    {
                        model.NumuneDosyaTipi = _dbPoly.SrcnNumuneDosyaTipis.Find(KategoriDosyaTipi);
                    }
                }
            }

            if (DosyaTipi == 3)
            {
                // müşteri şikayeti
                foreach (var item in _dbPoly.SrcnMusteriSikayetGrups.Where(a => a.UstItemTipi == 0 && a.ItemTipi > 0 && a.ItemTipi < 6).OrderBy(a => a.ItemTipi).ToList())
                {
                    model.MusteriSikayetGrupItemModeller.Add(new MusteriSikayetGrupItemModel()
                    {
                        AnaGrup = item,
                        AltItemGruplar = _dbPoly.SrcnMusteriSikayetGrups.Where(a => a.UstItemTipi == item.ItemTipi).OrderBy(a => a.MusteriSikayetGrupAdi).ToList()
                    });
                }
            }

            return model;

        }
        public SatisModel IsletmeYorumBase(int id)
        {
            // id = numune dosyaId
            //tip => 1= Ürge için deniz hanım, 2= mesut bülent aydın için
            var model = new SatisModel();
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = birimler;
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 32 || a.DosyaDurumId == 33 || a.DosyaDurumId == 34).ToList();


            if (id > 0)
            {
                model.MusteriSikayetDosya = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
                model.DosyaMusteriLabAnalizTablolar = MusteriDosyaLabAnalizTabloOlustur(id);
            }
            return model;
        }

        public SatisModel IsletmeOnayBase(int id)
        {
            // id = numune dosyaId
            //tip => 1= Ürge için deniz hanım, 2= mesut bülent aydın için
            var model = new SatisModel();
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = birimler;
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 32 || a.DosyaDurumId == 33 || a.DosyaDurumId == 34).ToList();


            if (id > 0)
            {
                model.MusteriSikayetDosya = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
                model.DosyaMusteriLabAnalizTablolar = MusteriDosyaLabAnalizTabloOlustur(id);
            }
            return model;
        }
        public ActionResult MusteriSikayetleriIsletmeYorumlari(int? id)
        {
            var model = IsletmeYorumBase(0);
            if (id == null)
            {
                var musteriDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == 31 || a.DosyaDurumId == 32)
                    .OrderBy(a => a.KayitTarihi).ToList();
                model.MusteriSikayetDosyalar = musteriDosyalar;
                model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(31);
            }
            else
            {
                int idd = Convert.ToInt32(id);
                if (idd == 32 || idd == 0)
                {
                    idd = 31;
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(31);
                }
                else
                {
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(idd);
                }

                model.MusteriSikayetDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == idd)
                    .OrderBy(a => a.KayitTarihi).ToList();
            }



            return View(model);
        }
        public ActionResult MusteriSikayetleriIsletmeYorum(int id)
        {
            var model = new SatisModel();
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = birimler;
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 31 ||  a.DosyaDurumId == 33).ToList();


            if (id > 0)
            {
                model.MusteriSikayetDosya = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
                model.DosyaMusteriLabAnalizTablolar = MusteriDosyaLabAnalizTabloOlustur(id);
            }
            return View(model);
 
        }
        [HttpPost]
        public ActionResult MusteriSikayetleriIsletmeYorum(SatisModel model)
        {
            var musteriDosya = model.MusteriSikayetDosya;

            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(model.MusteriSikayetDosya.DosyaDurumId);
            var editNumune = _dbPoly.SrcnMusteriSikayetDosyas.Find(model.MusteriSikayetDosya.MusteriSikayetDosyaId);
            editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
            editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
            editNumune.IsletmeAciklama = musteriDosya.IsletmeAciklama;
            _dbPoly.SaveChanges();
            _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
            {
                KayitTarihi = DateTime.Now,
                DosyaHareketTanimId = editNumune.DosyaDurumId,
                SikayetNumuneDosyaId = editNumune.MusteriSikayetDosyaId,
                DosyaAdi = "Müşteri Şikayetleri",
                DosyaTipi = 4,
                HareketAdi = editNumune.DosyaDurumAdi + " - " + Kullanici.IsimSoyisim
            });

            // yönetici yorumunda

            TempDataCRUDOlustur(2);

            return RedirectToAction("MusteriSikayetleriIsletmeYorumlari", "Satis",
                new { id = model.MusteriSikayetDosya.DosyaDurumId });

        }

        public ActionResult MusteriSikayetOnaylar(int? id)
        {
            TempData["Gizle"] = "gizle";


            var model = new SatisModel();
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => (a.DosyaTipi == 4) && (a.DosyaDurumId == 31 || a.DosyaDurumId == 32 || a.DosyaDurumId == 33 || a.DosyaDurumId == 34 || a.DosyaDurumId == 36)).OrderBy(a => a.DosyaDurumId).ToList();
            if (id == null)
            {

                // TEKNİSYEN TAMAMLADI DURUMU =6 BU YÜZDEN ONAYA ONLARI GETİRYORUZ
                // analiz onay yapilacaklar
                model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == 32);
                model.MusteriSikayetDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == 31).OrderByDescending(a => a.KayitTarihi).ToList();
            }
            else
            {
                int idd = Convert.ToInt32(id);
                if (idd == 0)
                {
                    idd = 32;
                }
                if (idd == 31)
                {
                    model.DosyaDurumu = model.DosyaDurumlari.First(a => a.DosyaDurumId == 31);
                    idd = 32;
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
            var model = IsletmeOnayBase(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult MusteriSikayetOnaylaDuzenle(SatisModel model)
        {
            var musteriDosya = model.MusteriSikayetDosya;

            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(model.MusteriSikayetDosya.DosyaDurumId);
            var editNumune = _dbPoly.SrcnMusteriSikayetDosyas.Find(model.MusteriSikayetDosya.MusteriSikayetDosyaId);
            editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
            editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
 
            _dbPoly.SaveChanges();
            _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
            {
                KayitTarihi = DateTime.Now,
                DosyaHareketTanimId = editNumune.DosyaDurumId,
                SikayetNumuneDosyaId = editNumune.MusteriSikayetDosyaId,
                DosyaAdi = "Müşteri Şikayetleri",
                DosyaTipi = 4,
                HareketAdi = editNumune.DosyaDurumAdi + " - " + Kullanici.IsimSoyisim
            });

            // yönetici yorumunda

            TempDataCRUDOlustur(2);
            return RedirectToAction("MusteriSikayetOnaylar", "Satis",
                new { id = model.MusteriSikayetDosya.DosyaDurumId });
        }

        #endregion
        #region Numune Yapılabilirlik CRUD
        public ActionResult NumuneYapilabilirlikDosyaTipleri(int? id, int? id2)
        {
            //id => numune yapılabilirlik kategori tipi=> 1= referans numuneli, 2= tekn. spektli
            // id2 = >
            Session["CarilerPoly"] = null;
            Session["CarilerTasd"] = null;
            Session["CarilerTasd"] = null;
            var numYapilabilirlikKategoriTipi = 0;
            int dosyaDurumuId = 10;
            if (id != null)
            {
                numYapilabilirlikKategoriTipi = Convert.ToInt32(id);
            }
            if (id2 != null)
            {
                dosyaDurumuId = Convert.ToInt32(id2);
            }
            var model = SatisModelDosyalariListele(2, numYapilabilirlikKategoriTipi, dosyaDurumuId);

            return View(model);
        }
        public ActionResult NumuneYapilabilirlikDosyalari()
        {
            TempData["Gizle"] = "gizle";
            return View(SatisModelDosyalariListele(3, 0, 0));
        }
        public ActionResult NumuneYapilabilirlikDosyaEkle(int id)
        {
            TempData["Gizle"] = "gizle";
            return View(SatisModeliBaseOlustur(2, id, 0));
        }
        [HttpPost]
        public ActionResult NumuneYapilabilirlikDosyaEkle(SatisModel model)
        {

            var numDosyaTipi = _dbPoly.SrcnNumuneDosyaTipis.Find(model.NumuneDosyaTipi.NumuneDosyaTipi);
            var numuneYapilabilirlik = model.NumuneYapilabilirlikDosya;
            int YonlendirmeId = 0;
            bool girisSorunuVarmi = model.CariKoduPoly == null && model.CariKoduTasd == null && model.CariKoduPset == null;
            if (!girisSorunuVarmi)
            {
                var Cari = CariGetir(model.CariKoduPoly, model.CariKoduTasd, model.CariKoduPset, numuneYapilabilirlik.FirmaTipi);

                if (numDosyaTipi.NumuneDosyaTipi == 1)
                {
                    // müşteri referans ürünlü
                    girisSorunuVarmi = numuneYapilabilirlik.AnalizYapilacakBobinSayisi == 0 && numuneYapilabilirlik.AnalizYapilacakKumasSayisi == 0 &&numuneYapilabilirlik.AnalizYapilacakRenkCalismasi == 0;
                    if (!girisSorunuVarmi)
                    {
                        var yeniDosyaHareketTanim = _dbPoly.SrcnDosyaDurums.Find(3);
                        var yeniNumuneYapilabilirlikDosya = new SrcnNumuneYapilabilirlikDosyas()
                        {
                            FirmaTipi = numuneYapilabilirlik.FirmaTipi,
                            KayitTarihi = DateTime.Now,
                            CariAdi = Cari.CariAdi,
                            CariKodu = Cari.CariNo,
                            DosyaDurumId = yeniDosyaHareketTanim.DosyaDurumId,
                            DosyaDurumAdi = yeniDosyaHareketTanim.DosyaDurumAdi,
                            AnalizYapilacakBobinSayisi = numuneYapilabilirlik.AnalizYapilacakBobinSayisi,
                            AnalizYapilacakKumasSayisi = numuneYapilabilirlik.AnalizYapilacakKumasSayisi,
                            AnalizYapilacakRenkCalismasi = numuneYapilabilirlik.AnalizYapilacakRenkCalismasi,
                            Aciklama = numuneYapilabilirlik.Aciklama,
                            KayitYapanKulAdi = Kullanici.IsimSoyisim,
                            KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                            NumuneDosyaTipi = numDosyaTipi.NumuneDosyaTipi,
                            NumuneDosyaTipiAdi = numDosyaTipi.NumuneDosyaTipiAdi,
                            NumuneDosyaTipiKategoriTipi = numDosyaTipi.NumuneDosyaTipiKategoriTipi,
                            NumuneDosyaTipiKategoriTipiAdi = numDosyaTipi.NumuneDosyaTipiKategoriTipiAdi
                        };
                        _dbPoly.SrcnNumuneYapilabilirlikDosyas.Add(yeniNumuneYapilabilirlikDosya);
                        _dbPoly.SaveChanges();
                        _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                        {
                            KayitTarihi = DateTime.Now,
                            DosyaHareketTanimId = yeniDosyaHareketTanim.DosyaDurumId,
                            SikayetNumuneDosyaId = yeniNumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId,
                            DosyaAdi = "Numune Yapılabilirlik",
                            DosyaTipi = 2,
                            HareketAdi = yeniDosyaHareketTanim.DosyaDurumAdi + " - " + Kullanici.IsimSoyisim
                        });
                        _dbPoly.SaveChanges();
                        if (yeniNumuneYapilabilirlikDosya.AnalizYapilacakBobinSayisi > 0)
                        {
                            for (int i = 0; i < yeniNumuneYapilabilirlikDosya.AnalizYapilacakBobinSayisi; i++)
                            {
                                YeniLabAnalizKaydet(2, yeniNumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId, Cari,
                                    yeniNumuneYapilabilirlikDosya.AnalizYapilacakBobinSayisi, 0, 1);
                            }

                        }

                        if (yeniNumuneYapilabilirlikDosya.AnalizYapilacakKumasSayisi > 0)
                        {
                            for (int i = 0; i < yeniNumuneYapilabilirlikDosya.AnalizYapilacakKumasSayisi; i++)
                            {
                                YeniLabAnalizKaydet(2, yeniNumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId, Cari, 0,
                                    yeniNumuneYapilabilirlikDosya.AnalizYapilacakKumasSayisi, 2);
                            }


                        }

                        YonlendirmeId = yeniNumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId;

                    }

                }
                else
                {
                    // müşteri teknik spektli
                    if (numuneYapilabilirlik.Aciklama == null)
                    {
                        girisSorunuVarmi = true;
                    }
                    else
                    {
                        var yeniDosyaHareketTanim = _dbPoly.SrcnDosyaDurums.Find(9);
                        var yeniNumuneYapilabilirlikDosya = new SrcnNumuneYapilabilirlikDosyas()
                        {
                            FirmaTipi = numuneYapilabilirlik.FirmaTipi,
                            KayitTarihi = DateTime.Now,
                            CariAdi = Cari.CariAdi,
                            CariKodu = Cari.CariNo,
                            DosyaDurumId = yeniDosyaHareketTanim.DosyaDurumId,
                            DosyaDurumAdi = yeniDosyaHareketTanim.DosyaDurumAdi,
                            Aciklama = numuneYapilabilirlik.Aciklama,
                            KayitYapanKulAdi = Kullanici.IsimSoyisim,
                            KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                            NumuneDosyaTipi = numDosyaTipi.NumuneDosyaTipi,
                            NumuneDosyaTipiAdi = numDosyaTipi.NumuneDosyaTipiAdi,
                            NumuneDosyaTipiKategoriTipi = numDosyaTipi.NumuneDosyaTipiKategoriTipi,
                            NumuneDosyaTipiKategoriTipiAdi = numDosyaTipi.NumuneDosyaTipiKategoriTipiAdi,
                            LabAnalizDurumu = 0,
                            LabAnalizDurumuAdi = "Analiz Yok",
                            DenemeYapilmaDurumu = 0,
                            DenemeYapilmaDurumuAdi = "Seçim Yapılmadı"

                        };
                        _dbPoly.SrcnNumuneYapilabilirlikDosyas.Add(yeniNumuneYapilabilirlikDosya);
                        YonlendirmeId = yeniNumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId;
                        _dbPoly.SaveChanges();
                        _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                        {
                            KayitTarihi = DateTime.Now,
                            DosyaHareketTanimId = yeniDosyaHareketTanim.DosyaDurumId,
                            SikayetNumuneDosyaId = yeniNumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId,
                            DosyaAdi = "Numune Yapılabilirlik",
                            DosyaTipi = 2,
                            HareketAdi = yeniDosyaHareketTanim.DosyaDurumAdi + "  " + Kullanici.IsimSoyisim,
                        });
                        _dbPoly.SaveChanges();


                    }

                }

                //KayitBilgiMailiGonder(2, YonlendirmeId);

            }


            if (girisSorunuVarmi)
            {
                TempDataOlustur("Lütfen Bilgileri Eksiksiz Doldurarak Tekrar Formu Kaydediniz", false);
                return RedirectToAction("NumuneYapilabilirlikDosyaEkle", "Satis", new { id = numDosyaTipi.NumuneDosyaTipi });
            }

            TempDataOlustur("Talebiniz Oluşturulmuştur. Düzenleme yapabilirsiniz", true);
            return RedirectToAction("NumuneYapilabilirlikDosyaBilgileri", "Satis", new { id = YonlendirmeId });


        }
        public ActionResult NumuneYapilabilirlikDosyaBilgileri(int id)
        {
            try
            {
                var NumuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("NumuneYapilabilirlikDosyaTipleri");
            }


            TempData["Gizle"] = "gizle";
            var model = SatisModeliBaseOlustur(2, 0, id);
            return View(model);
        }


        [HttpPost]
        public ActionResult NumuneYapilabilirlikDosyaBilgileri(SatisModel model)
        {
            var numuneYapilabilirlik = model.NumuneYapilabilirlikDosya;
            var editNumuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(numuneYapilabilirlik.NumuneYapilabilirlikDosyaId);

            bool degisiklikVarMi = numuneYapilabilirlik.FirmaTipi != editNumuneYapilabilirlik.FirmaTipi;



            if (numuneYapilabilirlik.YapilabilirlikDurumu != editNumuneYapilabilirlik.YapilabilirlikDurumu)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.DenemeYapilmaDurumu != editNumuneYapilabilirlik.DenemeYapilmaDurumu)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.SipariseDonmeDurumu != editNumuneYapilabilirlik.SipariseDonmeDurumu)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.SipariseDonmeDurumuAdi != editNumuneYapilabilirlik.SipariseDonmeDurumuAdi)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.Aciklama != editNumuneYapilabilirlik.Aciklama)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.SiparisNo != editNumuneYapilabilirlik.SiparisNo)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.DenemeKodu != editNumuneYapilabilirlik.DenemeKodu)
            {
                degisiklikVarMi = true;
            }

            if (numuneYapilabilirlik.DosyaDurumId > 9)
            {
                if (editNumuneYapilabilirlik.DosyaDurumId != numuneYapilabilirlik.DosyaDurumId)
                {
                    var yeniDurum = _dbPoly.SrcnDosyaDurums.Find(numuneYapilabilirlik.DosyaDurumId);
                    editNumuneYapilabilirlik.DosyaDurumId = yeniDurum.DosyaDurumId;
                    editNumuneYapilabilirlik.DosyaDurumAdi = yeniDurum.DosyaDurumAdi;
                }


            }

            editNumuneYapilabilirlik.FirmaTipi = numuneYapilabilirlik.FirmaTipi;

            editNumuneYapilabilirlik.SatisYorumu = numuneYapilabilirlik.SatisYorumu;

            editNumuneYapilabilirlik.SipariseDonmeDurumu = numuneYapilabilirlik.SipariseDonmeDurumu;

            editNumuneYapilabilirlik.SiparisNo = numuneYapilabilirlik.SiparisNo;
            _dbPoly.SaveChanges();

            TempDataCRUDOlustur(2);

            return RedirectToAction("NumuneYapilabilirlikDosyaBilgileri", "Satis",
                new { id = editNumuneYapilabilirlik.NumuneYapilabilirlikDosyaId });
        }

        public ActionResult NumuneYapilabilirlikDosyaSil(int id)
        {
            NumuneYapilabilirlikDosyaSil(id);
            TempDataCRUDOlustur(3);
            return RedirectToAction("NumuneYapilabilirlikDosyaTipleri");
        }

        public ActionResult NumuneYapilabilirlikDosyaAnalizleri(int id)
        {
            try
            {
                var NumuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("NumuneYapilabilirlikDosyalari");
            }


            var model = SatisModelDosyayiGetir(2, id);
            return View(model);
        }
        public ActionResult NumuneYapilabilirlikDosyaHareketleri(int id)
        {
            try
            {
                var NumuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("NumuneYapilabilirlikDosyalari");
            }


            var model = SatisModelDosyayiGetir(2, id);
            return View(model);
        }
        [HttpPost]
        public ActionResult NumuneYapilabilirlikDosyaHareketleri(SatisModel model)
        {

            var numuneYapilabilirlik =
                _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(model.NumuneYapilabilirlikDosya
                    .NumuneYapilabilirlikDosyaId);
            bool SorunVarmi = model.DosyaHareket.HareketAdi == null;

            if (SorunVarmi == false)
            {
                var dosyaHareketTanim = _dbPoly.SrcnDosyaDurums.First(a => a.DosyaDurumId == 99);
                _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                {
                    DosyaAdi = "Numune Yapılabilirlik",
                    DosyaHareketTanimId = dosyaHareketTanim.DosyaDurumId,
                    DosyaTipi = 2,
                    KayitTarihi = DateTime.Now,
                    HareketAdi = model.DosyaHareket.HareketAdi + " - " + Kullanici.IsimSoyisim,
                    SikayetNumuneDosyaId = numuneYapilabilirlik.NumuneYapilabilirlikDosyaId
                });
                _dbPoly.SaveChanges();
                TempDataOlustur("Hareket Kayıt işlemi yapılmıştır", true);
            }

            return RedirectToAction("NumuneYapilabilirlikDosyaHareketleri", "Satis",
                new { id = numuneYapilabilirlik.NumuneYapilabilirlikDosyaId });
        }
        public ActionResult NumuneYapilabilirlikAnalizResimler(int id)
        {
            try
            {
                var NumuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("NumuneYapilabilirlikDosyalari");
            }
            TempData["Gizle"] = "gizle";
            var model = SatisModelDosyayiGetir(2, id);
            return View(model);
        }
        [HttpPost]
        public ActionResult NumuneYapilabilirlikAnalizResimler(HttpPostedFileBase files, SatisModel model)
        {
            var numuneYapilabilirlik =
                _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(model.NumuneYapilabilirlikDosya
                    .NumuneYapilabilirlikDosyaId);
            bool SorunVarmi = false;
            if (model.Resim.Aciklama == null || files == null || files.ContentLength == 0)
            {
                TempData["Msg"] = "Lütfen Bilgileri Eksiksiz Giriniz ve Fotoğraf Seçimi Yapınız.";
                TempData["class"] = "danger";
                SorunVarmi = true;

            }
            else
            {
                //  var path = "~/Upload/" + Guid.NewGuid() + Path.GetExtension(files.FileName);
                // files.SaveAs(Server.MapPath(path));

                using (var inputStream = files.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }

                    var data = memoryStream.ToArray();


                    var resKategori = _dbPoly.SrcnResimKategoris.First(a => a.ResimKategoriId == 2);
                    var resim = new SrcnResims()
                    {
                        KayitTarihi = DateTime.Now,
                        Aciklama = model.Resim.Aciklama,
                        BagliOlduguDosyaId = numuneYapilabilirlik.NumuneYapilabilirlikDosyaId,
                        //  FileName = path,
                        YukleyenKulAdi = Kullanici.IsimSoyisim,
                        YukleyenKulKodu = Kullanici.KullaniciId.ToString(),
                        ResimKategoriId = resKategori.ResimKategoriId,
                        ResimKategoriAdi = resKategori.ResimKategoriAdi,
                        Resim = data

                    };

                    _dbPoly.SrcnResims.Add(resim);
                    _dbPoly.SaveChanges();
                }
                TempDataOlustur("Fotoğraf Yükleme İşlemi Yapılmıştır", true);
            }


            return Json(new
            {

                redirectUrl = Url.Action("NumuneYapilabilirlikAnalizResimler", "Satis", new { id = numuneYapilabilirlik.NumuneYapilabilirlikDosyaId }),
                isRedirect = true
            });
        }

        public ActionResult NumuneYapilabilirlikAnalizDetay(int id)
        {

            var model = LabAnalizeGoreSatisModeliGetir(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult NumuneYapilabilirlikAnalizDetay(SatisModel model)
        {
            var labAnaliz = model.LabAnaliz;
            var EditLAbAnaliz = _dbPoly.SrcnLabAnalizs.Find(labAnaliz.LabAnalizId);
            if (labAnaliz.Aciklama != EditLAbAnaliz.Aciklama)
            {
                if (EditLAbAnaliz.Aciklama != null)
                {
                    _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
                    {
                        LabAnalizId = labAnaliz.LabAnalizId,
                        KayitTarihi = DateTime.Now,
                        HareketAdi = string.Format("{0} olan açıklama {1} olarak güncellenmiştir - {2}", EditLAbAnaliz.Aciklama, labAnaliz.Aciklama, Kullanici.IsimSoyisim)
                    });
                    _dbPoly.SaveChanges();
                }
                EditLAbAnaliz.Aciklama = labAnaliz.Aciklama;
                _dbPoly.SaveChanges();
            }
            TempDataCRUDOlustur(2);
            return View(LabAnalizeGoreSatisModeliGetir(model.LabAnaliz.LabAnalizId));
        }

        #endregion
        #region Müşteri Şikayet CRUD

        public ActionResult MusteriSikayetiDosyalari(int? id2)
        {

            TempData["Gizle"] = "gizle";
            int dosyaDurumuId = 26;
            if (id2 != null)
            {
                dosyaDurumuId = Convert.ToInt32(id2);
            }

            return View(SatisModelDosyalariListele(3, 0, dosyaDurumuId));
        }
        public ActionResult MusteriSikayetiDosyaInteraktifEkle()
        {

            // yetkisiz bölümünde aramalar
            return View(new SatisModel());
        }


        public ActionResult MusteriSikayetiDosyaEkle()
        {
            var sikayetLab = _dbPoly.SrcnLabGrups.First(a => a.AnalizTipi == 2 && a.UstLabGrupId == 0);

            return View(SatisModeliBaseOlustur(3, 0, 0));
        }
        [HttpPost]
        public ActionResult MusteriSikayetiDosyaEkle(SatisModel model)
        {

            var musteriSikayet = model.MusteriSikayetDosya;
            var cariGetirdi = CariGetir(model.CariKoduPoly, model.CariKoduTasd, model.CariKoduPset,
                model.MusteriSikayetDosya.FirmaTipi);
            #region Kayıt Kontrolü

            bool girisSorunuVarmi = musteriSikayet.FirmaTipi == 0 || model.LabAnalizeGondermeDurumu == 0;

            if (model.LabAnalizeGondermeDurumu == 1)
            {
                girisSorunuVarmi = musteriSikayet.AnalizYapilacakBobinSayisi == 0 && musteriSikayet.AnalizYapilacakKumasSayisi == 0;
            }
            //foreach (var araItem in model.MusteriSikayetGrupItemModeller)
            //{
            //    var anaGrup = _dbPoly.SrcnMusteriSikayetGrups.Find(araItem.AnaGrup.MusteriSikayetGrupId);
            //    if (anaGrup.RadioSecimMi == "1")
            //    {
            //        if (araItem.RadioId == 0)
            //        {
            //            girisSorunuVarmi = true;
            //        }
            //    }
            //    else
            //    {
            //        int SecimSay = araItem.AltItemGruplar.Count(a => a.SeciliMi);

            //        if (SecimSay == 0)
            //        {
            //            girisSorunuVarmi = true;
            //        }
            //    }
            //}

            if (girisSorunuVarmi)
            {
                TempDataOlustur("Lütfen Bilgileri Eksiksiz Doldurarak Tekrar Formu Kaydediniz", false);
                return RedirectToAction("MusteriSikayetiDosyaEkle");
            }
            #endregion
            var Cari = CariGetir(model.CariKoduPoly, model.CariKoduTasd, model.CariKoduPset, musteriSikayet.FirmaTipi);

            var yeniDosyaHareketTanim = _dbPoly.SrcnDosyaDurums.Find(26);
            var yeniMusteriSikayetDosya = new SrcnMusteriSikayetDosyas()
            {
                PartiNo = musteriSikayet.PartiNo,
                FirmaTipi = musteriSikayet.FirmaTipi,
                KayitTarihi = DateTime.Now,
                CariAdi = cariGetirdi.CariAdi,
                CariKodu = cariGetirdi.CariNo,
                DosyaDurumId = yeniDosyaHareketTanim.DosyaDurumId,
                DosyaDurumAdi = yeniDosyaHareketTanim.DosyaDurumAdi,
                AnalizYapilacakBobinSayisi = musteriSikayet.AnalizYapilacakBobinSayisi,
                AnalizYapilacakKumasSayisi = musteriSikayet.AnalizYapilacakKumasSayisi,
                Aciklama = musteriSikayet.Aciklama,
                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                SevkMiktari = musteriSikayet.SevkMiktari,
                BobinIciLotNo = musteriSikayet.BobinIciLotNo,
                SevkTarihi = musteriSikayet.SevkTarihi,
                LabAnalizDurumu = 0,
                LabAnalizDurumuAdi = ""
            };
            //try
            //{
            _dbPoly.SrcnMusteriSikayetDosyas.Add(yeniMusteriSikayetDosya);
            _dbPoly.SaveChanges();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);

            //}

            foreach (var value in model.MusteriSikayetGrupItemModeller)
            {
                if (value.RadioId == 11)
                {
                    _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Add(new SrcnMusteriSikayetDosyaSikayetGrups()
                    {
                        MusteriSikayetDosyaId = yeniMusteriSikayetDosya.MusteriSikayetDosyaId,
                        MusteriSikayetGrupId = 11,
                        MusteriSikayetGrupAdi = "YAPILDI",
                    });

                    _dbPoly.SaveChanges();
                }

                if (value.RadioId == 12)
                {
                    _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Add(new SrcnMusteriSikayetDosyaSikayetGrups()
                    {
                        MusteriSikayetDosyaId = yeniMusteriSikayetDosya.MusteriSikayetDosyaId,
                        MusteriSikayetGrupId = 12,
                        MusteriSikayetGrupAdi = "YAPILMADI",
                    });

                    _dbPoly.SaveChanges();
                }

                foreach (var altgrup in value.AltItemGruplar)
                {


                    if (altgrup.SeciliMi == true)
                    {
                        _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Add(new SrcnMusteriSikayetDosyaSikayetGrups()
                        {
                            MusteriSikayetDosyaId = yeniMusteriSikayetDosya.MusteriSikayetDosyaId,
                            MusteriSikayetGrupId = altgrup.MusteriSikayetGrupId,
                            MusteriSikayetGrupAdi = altgrup.MusteriSikayetGrupAdi.Trim()
                        });

                        _dbPoly.SaveChanges();
                    }

                }

            }


            _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
            {
                KayitTarihi = DateTime.Now,
                DosyaHareketTanimId = yeniDosyaHareketTanim.DosyaDurumId,
                SikayetNumuneDosyaId = yeniMusteriSikayetDosya.MusteriSikayetDosyaId,
                DosyaAdi = yeniDosyaHareketTanim.DosyaDurumAdi,
                DosyaTipi = 4,
                HareketAdi = yeniDosyaHareketTanim.DosyaDurumAdi + " - " + Kullanici.IsimSoyisim
            });
            _dbPoly.SaveChanges();

            if (yeniMusteriSikayetDosya.AnalizYapilacakBobinSayisi > 0)
            {
                for (int i = 0; i < yeniMusteriSikayetDosya.AnalizYapilacakBobinSayisi; i++)
                {
                    YeniLabAnalizKaydet(3, yeniMusteriSikayetDosya.MusteriSikayetDosyaId, Cari,
                        yeniMusteriSikayetDosya.AnalizYapilacakBobinSayisi, 0, 1);
                }

            }

            if (yeniMusteriSikayetDosya.AnalizYapilacakKumasSayisi > 0)
            {
                for (int i = 0; i < yeniMusteriSikayetDosya.AnalizYapilacakKumasSayisi; i++)
                {
                    YeniLabAnalizKaydet(3, yeniMusteriSikayetDosya.MusteriSikayetDosyaId, Cari, 0,
                        yeniMusteriSikayetDosya.AnalizYapilacakKumasSayisi, 2);
                }


            }

            TempDataCRUDOlustur(1);
            return RedirectToAction("MusteriSikayetiDosyalari");
            // return View(SatisModeliBaseOlustur(sikayetLab.LabGrupId));
        }
        public ActionResult MusteriSikayetAnalizleri()
        {
            TempData["Gizle"] = "gizle";
            return View(SatisModelDosyalariListele(3, 0, 0));
        }
        public ActionResult MusteriSikayetDosyaBilgileri(int id)
        {
            try
            {
                var sikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("MusteriSikayetiDosyalari");
            }


            TempData["Gizle"] = "gizle";
            var model = SatisModelDosyayiGetir(3, id);
            //   MusteriDosyaHareketOzetEkle(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult MusteriSikayetDosyaBilgileri(SatisModel model)
        {
            var labgrup = _dbPoly.SrcnLabGrups.Find(model.LabGrup.LabGrupId);
            var istekTarihi = Convert.ToDateTime(model.IstekTarihi);
            var musteriSikayet = model.MusteriSikayetDosya;
            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(musteriSikayet.DosyaDurumId);
            #region Kayıt Kontrolü

            bool girisSorunuVarmi = musteriSikayet.FirmaTipi == 0 || model.LabAnalizeGondermeDurumu == 0;

            var sikayetLab = _dbPoly.SrcnLabGrups.First(a => a.AnalizTipi == 2 && a.UstLabGrupId == 0);

            if (model.LabAnalizeGondermeDurumu == 1)
            {
                girisSorunuVarmi = musteriSikayet.AnalizYapilacakBobinSayisi == 0 && musteriSikayet.AnalizYapilacakKumasSayisi == 0;
            }
            foreach (var araItem in model.MusteriSikayetGrupItemModeller)
            {
                var anaGrup = _dbPoly.SrcnMusteriSikayetGrups.Find(araItem.AnaGrup.MusteriSikayetGrupId);
                if (anaGrup.RadioSecimMi == "1")
                {
                    if (araItem.RadioId == 0)
                    {
                        girisSorunuVarmi = true;
                    }
                }
                else
                {
                    int SecimSay = araItem.AltItemGruplar.Count(a => a.SeciliMi);

                    if (SecimSay == 0)
                    {
                        girisSorunuVarmi = true;
                    }
                }
            }
            #endregion
            if (girisSorunuVarmi)
            {
                TempDataOlustur("Lütfen Bilgileri Eksiksiz Doldurarak Tekrar Formu Kaydediniz", false);

            }
            else
            {


                var editSikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(musteriSikayet.MusteriSikayetDosyaId);
                //var Cari = CariGetir(model.CariKoduPoly, model.CariKoduTasd, model.CariKoduPset, musteriSikayet.FirmaTipi);

                editSikayet.FirmaTipi = musteriSikayet.FirmaTipi;
                editSikayet.KayitTarihi = DateTime.Now;
                editSikayet.SevkTarihi = musteriSikayet.PartiNo;
                editSikayet.AnalizYapilacakBobinSayisi = musteriSikayet.AnalizYapilacakBobinSayisi;
                editSikayet.AnalizYapilacakKumasSayisi = musteriSikayet.AnalizYapilacakKumasSayisi;
                editSikayet.Aciklama = musteriSikayet.Aciklama;
                editSikayet.KayitYapanKulAdi = Kullanici.IsimSoyisim;
                editSikayet.KayitYapanKulKodu = Kullanici.KullaniciId.ToString();
                editSikayet.SevkMiktari = musteriSikayet.SevkMiktari;
                editSikayet.BobinIciLotNo = musteriSikayet.BobinIciLotNo;
                editSikayet.SevkTarihi = musteriSikayet.SevkTarihi;
                editSikayet.DosyaDurumId = dosyaDurum.DosyaDurumId;
                editSikayet.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
                _dbPoly.SaveChanges();

                var sikayetItems = _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups
                    .Where(a => a.MusteriSikayetDosyaId == editSikayet.MusteriSikayetDosyaId).ToList();

                var eklenecekSikayetGrup = new List<SrcnMusteriSikayetGrups>();
                var SilinecekSikayetDosyaGrup = new List<SrcnMusteriSikayetDosyaSikayetGrups>();

                foreach (var itt in model.MusteriSikayetGrupItemModeller)
                {
                    var anaGrup = _dbPoly.SrcnMusteriSikayetGrups.Find(itt.AnaGrup.MusteriSikayetGrupId);
                    if (anaGrup.RadioSecimMi == "1")
                    {
                        foreach (var item in itt.AltItemGruplar)
                        {
                            if (item.MusteriSikayetGrupId == itt.RadioId)
                            {
                                // eklenecek
                                if (sikayetItems.Count(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId) == 0)
                                {
                                    if (eklenecekSikayetGrup.Count(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId) == 0)
                                    {
                                        eklenecekSikayetGrup.Add(new SrcnMusteriSikayetGrups()
                                        {
                                            MusteriSikayetGrupId = itt.RadioId
                                        });
                                    }
                                }

                            }
                            else
                            {
                                if (sikayetItems.Count(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId) != 0)
                                {
                                    var aa = sikayetItems.First(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId);

                                    if (SilinecekSikayetDosyaGrup.Count(a => a.MusteriSikayetGrupId == aa.MusteriSikayetGrupId) == 0)
                                    {
                                        SilinecekSikayetDosyaGrup.Add(aa);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in itt.AltItemGruplar)
                        {
                            if (item.SeciliMi)
                            {
                                //eklenecek
                                if (sikayetItems.Count(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId) == 0)
                                {
                                    if (eklenecekSikayetGrup.Count(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId) == 0)
                                    {
                                        eklenecekSikayetGrup.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                //silinecek
                                if (sikayetItems.Count(a => a.MusteriSikayetGrupId == item.MusteriSikayetGrupId) != 0)
                                {
                                    var aa = sikayetItems.First(a => a.MusteriSikayetGrupId == itt.RadioId);

                                    if (SilinecekSikayetDosyaGrup.Count(a => a.MusteriSikayetGrupId == aa.MusteriSikayetGrupId) == 0)
                                    {
                                        SilinecekSikayetDosyaGrup.Add(aa);
                                    }
                                }

                            }
                        }
                    }

                }


                if (eklenecekSikayetGrup.Any() || SilinecekSikayetDosyaGrup.Any())
                {
                    foreach (var silItem in SilinecekSikayetDosyaGrup)
                    {
                        var sikayetItem = _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Find(silItem.MusteriSikayetDosyaSikayetGrupId);
                        _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Remove(sikayetItem);


                        _dbPoly.SaveChanges();
                    }
                    foreach (var ekleItem in eklenecekSikayetGrup)
                    {
                        var sikayetItem = _dbPoly.SrcnMusteriSikayetGrups.Find(ekleItem.MusteriSikayetGrupId);
                        _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Add(new SrcnMusteriSikayetDosyaSikayetGrups()
                        {
                            MusteriSikayetGrupAdi = sikayetItem.MusteriSikayetGrupAdi,
                            MusteriSikayetGrupId = sikayetItem.MusteriSikayetGrupId,
                            MusteriSikayetDosyaId = editSikayet.MusteriSikayetDosyaId
                        });
                        _dbPoly.SaveChanges();
                    }

                    MusteriSikayetDosyaHareketOzetEkle(editSikayet.MusteriSikayetDosyaId);
                }
                TempDataOlustur("Güncelleme İşlemi Yapılmıştır", true);
            }
            return RedirectToAction("MusteriSikayetDosyaBilgileri", "Satis", new { id = musteriSikayet.MusteriSikayetDosyaId });

        }
        public ActionResult MusteriSikayetDosyaHareketleri(int id)
        {
            try
            {
                var SrcnMusteriSikayetDosyas = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("MusteriSikayetiDosyalari");
            }


            TempData["Gizle"] = "gizle";
            var model = SatisModelDosyayiGetir(3, id);
            return View(model);
        }
        [HttpPost]
        public ActionResult MusteriSikayetDosyaHareketleri(SatisModel model)
        {

            var musteriSikayet =
                _dbPoly.SrcnMusteriSikayetDosyas.Find(model.MusteriSikayetDosya
                    .MusteriSikayetDosyaId);
            bool SorunVarmi = model.DosyaHareket.HareketAdi == null;

            if (SorunVarmi == false)
            {
                var dosyaHareketTanim = _dbPoly.SrcnDosyaDurums.First(a => a.DosyaDurumId == 26);
                _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                {
                    DosyaAdi = "Müşteri Şikayetleri",
                    DosyaHareketTanimId = dosyaHareketTanim.DosyaDurumId,
                    DosyaTipi = 4,
                    KayitTarihi = DateTime.Now,
                    HareketAdi = model.DosyaHareket.HareketAdi + " - " + Kullanici.IsimSoyisim,
                    SikayetNumuneDosyaId = musteriSikayet.MusteriSikayetDosyaId
                });
                _dbPoly.SaveChanges();
                TempDataOlustur("Hareket Kayıt işlemi yapılmıştır", true);
            }

            return RedirectToAction("MusteriSikayetDosyaHareketleri", "Satis",
                new { id = musteriSikayet.MusteriSikayetDosyaId });
        }
        public ActionResult MusteriSikayetDosyaResimleri(int id)
        {
            try
            {
                var musterisikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("MusteriSikayetiDosyalari");
            }
            TempData["Gizle"] = "gizle";
            var model = SatisModelDosyayiGetir(3, id);
            return View(model);
        }
        [HttpPost]
        public ActionResult MusteriSikayetDosyaResimleri(HttpPostedFileBase files, SatisModel model)
        {
            var musteriSikayet =
                _dbPoly.SrcnMusteriSikayetDosyas.Find(model.MusteriSikayetDosya
                    .MusteriSikayetDosyaId);
            bool SorunVarmi = false;
            if (model.Resim.Aciklama == null || files == null || files.ContentLength == 0)
            {
                TempData["Msg"] = "Lütfen Bilgileri Eksiksiz Giriniz ve Fotoğraf Seçimi Yapınız.";
                TempData["class"] = "danger";
                SorunVarmi = true;

            }
            else
            {
                //var path = "~/Upload/" + Guid.NewGuid() + Path.GetExtension(files.FileName);
                //files.SaveAs(Server.MapPath(path));
                using (var inputStream = files.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }

                    var data = memoryStream.ToArray();

                    var resKategori = _dbPoly.SrcnResimKategoris.First(a => a.ResimKategoriId == 3);
                    var resim = new SrcnResims()
                    {
                        KayitTarihi = DateTime.Now,
                        Aciklama = model.Resim.Aciklama,
                        BagliOlduguDosyaId = musteriSikayet.MusteriSikayetDosyaId,
                        //       FileName = path,
                        YukleyenKulAdi = Kullanici.IsimSoyisim,
                        YukleyenKulKodu = Kullanici.KullaniciId.ToString(),
                        ResimKategoriId = resKategori.ResimKategoriId,
                        ResimKategoriAdi = resKategori.ResimKategoriAdi,
                        Resim = data

                    };
                    _dbPoly.SrcnResims.Add(resim);
                    _dbPoly.SaveChanges();
                }

                TempDataOlustur("Fotoğraf Yükleme İşlemi Yapılmıştır", true);
            }


            return Json(new
            {

                redirectUrl = Url.Action("MusteriSikayetDosyaResimleri", "Satis", new { id = musteriSikayet.MusteriSikayetDosyaId }),
                isRedirect = true
            });
        }

        public ActionResult MusteriSikayetiDosyaAnalizleri(int id)
        {
            try
            {
                var musteriSikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            }
            catch (Exception)
            {
                return RedirectToAction("MusteriSikayetiDosyalari");
            }


            TempData["Gizle"] = "gizle";
            var model = SatisModelDosyayiGetir(3, id);
            return View(model);
        }
        public ActionResult MusteriSikayetiDosyaAnalizDetay(int id)
        {

            var model = LabAnalizeGoreSatisModeliGetir(id);
            return View(model);
        }
        #endregion


        public ActionResult ResimIndir(int id)
        {
            var imageData = _dbPoly.SrcnResims.Find(id).Resim;
            return File(imageData, "image/png");
        }
        public ActionResult LabFormIndir(int id)
        {
            return MusteriSikayetAnaliziPDFOlustur(id);
        }
        public ActionResult MusteriFormIndir(int id)
        {
            return PdfDetayliOlustur(id, 2, 1);
        }
        public ActionResult MusteriFormIndirEng(int id)
        {
            return PdfDetayliOlustur(id, 2, 2);
        }
        public ActionResult AnalizSil(int id)
        {
            var analiz = _dbPoly.SrcnLabAnalizs.Find(id);

            var analizTipi = analiz.AnalizTipi;
            var dosyaId = analiz.BagliOlduguDosyaId;


            LabAnalizVeritabanındanSil(id);

            TempDataCRUDOlustur(2);
            if (analizTipi == 3)
            {
                return RedirectToAction("NumuneYapilabilirlikDosyaAnalizleri", "Satis", new { id = dosyaId });
            }
            else
            {
                return RedirectToAction("MusteriSikayetiDosyaAnalizleri", "Satis", new { id = dosyaId });
            }


        }
        public ActionResult DosyaSil(int id, int id2)
        {
            TempDataCRUDOlustur(3);
            var analizIdler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.BagliOlduguDosyaId == id && a.AnalizTipi == id2)
                .Select(a => a.LabAnalizId).ToList();
            var fotolar = _dbPoly.SrcnResims.Where(a => a.BagliOlduguDosyaId == id && a.ResimKategoriId == id2).ToList();
            var dosyaHareketler = _dbPoly.SrcnDosyaHarekets.Where(a => a.DosyaTipi == id2 && a.SikayetNumuneDosyaId == id).ToList();


            foreach (var i in analizIdler)
            {
                LabAnalizVeritabanındanSil(i);
            }

            _dbPoly.SrcnResims.RemoveRange(fotolar);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnDosyaHarekets.RemoveRange(dosyaHareketler);
            _dbPoly.SaveChanges();

            if (id2 == 2)
            {
                // numune

                var numune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
                int yonlendirmeId = 1;
                int dosyaDurumId = 4;

                if (numune.NumuneDosyaTipi != null)
                {
                    yonlendirmeId = Convert.ToInt32(numune.NumuneDosyaTipi);
                }

                if (numune.DosyaDurumId != 0)
                {
                    dosyaDurumId = numune.DosyaDurumId;
                }
                var labAnalizIdler = _dbPoly.SrcnLabAnalizs.Where(a => a.DosyaTipi == 2 && a.BagliOlduguDosyaId == id)
                    .Select(a => a.LabAnalizId).ToList();


                _dbPoly.SrcnNumuneYapilabilirlikDosyas.Remove(numune);
                _dbPoly.SaveChanges();
                foreach (var i in labAnalizIdler)
                {
                    LabAnalizVeritabanındanSil(i);
                }
                TempDataCRUDOlustur(3);
                return RedirectToAction("NumuneYapilabilirlikDosyaTipleri", "Satis", new { id = yonlendirmeId, id2 = dosyaDurumId });
            }
            else
            {
                // müşterişikayet
                var sikayet = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
                _dbPoly.SrcnMusteriSikayetDosyas.Remove(sikayet);
                _dbPoly.SaveChanges();

                var dosyaHareket = _dbPoly.SrcnDosyaHarekets.Where(a => a.SikayetNumuneDosyaId == id && a.DosyaTipi == 4).FirstOrDefault();
                _dbPoly.SrcnDosyaHarekets.Remove(dosyaHareket);
                _dbPoly.SaveChanges();

                var dosyaSikayetGrup = _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Where(a => a.MusteriSikayetDosyaId == id).FirstOrDefault();
                _dbPoly.SrcnMusteriSikayetDosyaSikayetGrups.Remove(dosyaSikayetGrup);
                _dbPoly.SaveChanges();
                return RedirectToAction("MusteriSikayetiDosyalari");
            }
        }


        #region Eski Kodlar
        /*
        [HttpPost]
        public ActionResult NumuneYapilabilirlikDosyaBilgileri(SatisModel model)
        {
            var numuneYapilabilirlik = model.NumuneYapilabilirlikDosya;
            var editNumuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(numuneYapilabilirlik.NumuneYapilabilirlikDosyaId);

            bool degisiklikVarMi = numuneYapilabilirlik.FirmaTipi != editNumuneYapilabilirlik.FirmaTipi;

            if (!(model.CariKoduPoly == editNumuneYapilabilirlik.CariKodu || model.CariKoduTasd == editNumuneYapilabilirlik.CariKodu || model.CariKoduPset == editNumuneYapilabilirlik.CariKodu))
            {
                degisiklikVarMi = true;
            }

            if (numuneYapilabilirlik.YapilabilirlikDurumu != editNumuneYapilabilirlik.YapilabilirlikDurumu)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.DenemeYapilmaDurumu != editNumuneYapilabilirlik.DenemeYapilmaDurumu)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.SipariseDonmeDurumu != editNumuneYapilabilirlik.SipariseDonmeDurumu)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.SipariseDonmeDurumuAdi != editNumuneYapilabilirlik.SipariseDonmeDurumuAdi)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.Aciklama != editNumuneYapilabilirlik.Aciklama)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.SiparisNo != editNumuneYapilabilirlik.SiparisNo)
            {
                degisiklikVarMi = true;
            }
            if (numuneYapilabilirlik.DenemeKodu != editNumuneYapilabilirlik.DenemeKodu)
            {
                degisiklikVarMi = true;
            }

            var DenemeDurumlar = Helper.NumuneYapilabilirlikDenemeDurumlariGetir();
            var SiparisDurumlar = Helper.NumuneYapilabilirlikSipariseDonmeDurumlariGetir();
            var yapilabilirlikDurumlar = Helper.NumuneYapilabilirlikBaseYapilabilirlikDurumlariGetir();
            var Cari = CariGetir(model.CariKoduPoly, model.CariKoduTasd, model.CariKoduPset, numuneYapilabilirlik.FirmaTipi);
            editNumuneYapilabilirlik.FirmaTipi = numuneYapilabilirlik.FirmaTipi;
            editNumuneYapilabilirlik.CariKodu = Cari.CariNo;
            editNumuneYapilabilirlik.CariAdi = Cari.CariAdi;
            editNumuneYapilabilirlik.AnalizYapilacakBobinSayisi = numuneYapilabilirlik.AnalizYapilacakBobinSayisi;
            editNumuneYapilabilirlik.AnalizYapilacakKumasSayisi = numuneYapilabilirlik.AnalizYapilacakKumasSayisi;
            editNumuneYapilabilirlik.YapilabilirlikDurumu = numuneYapilabilirlik.YapilabilirlikDurumu;
            editNumuneYapilabilirlik.DenemeYapilmaDurumu = numuneYapilabilirlik.DenemeYapilmaDurumu;
            editNumuneYapilabilirlik.SipariseDonmeDurumu = numuneYapilabilirlik.SipariseDonmeDurumu;
            editNumuneYapilabilirlik.SipariseDonmeDurumuAdi = SiparisDurumlar.First(a => a.Id == numuneYapilabilirlik.SipariseDonmeDurumu.ToString()).Tanim;
            editNumuneYapilabilirlik.DenemeYapilmaDurumuAdi = DenemeDurumlar
                .First(a => a.Id == numuneYapilabilirlik.DenemeYapilmaDurumu.ToString()).Tanim;
            editNumuneYapilabilirlik.YapilabilirlikDurumuAdi = DenemeDurumlar
                .First(a => a.Id == numuneYapilabilirlik.YapilabilirlikDurumu.ToString()).Tanim;
            editNumuneYapilabilirlik.DenemeKodu = numuneYapilabilirlik.DenemeKodu;
            editNumuneYapilabilirlik.SiparisNo = numuneYapilabilirlik.SiparisNo;
            _dbPoly.SaveChanges();
            if (degisiklikVarMi)
            {
                NumuneYapilabilirlikDosyaHareketOzet(editNumuneYapilabilirlik.NumuneYapilabilirlikDosyaId);
            }
            TempDataOlustur("Güncelleme İşlemi Yapılmıştır", true);

            return RedirectToAction("NumuneYapilabilirlikDosyaBilgileri", "Satis",
                new { id = editNumuneYapilabilirlik.NumuneYapilabilirlikDosyaId });
        }
        */

        #endregion
    }
}