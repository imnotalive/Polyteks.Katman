using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.TefrikBildirim.Models;

namespace Polyteks.Katman.TefrikBildirim.Controllers
{
    public class YetkisizController : Controller
    {
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();

        private static POTA_TASDEntities _appContextTasd;
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

        public POTA_TASDEntities _dbTasd = _dbTasdCreate();

        // GET: Yetkisiz
        public ActionResult Index()
        {
            return View();
        }

        #region Müşteri Şikayet Özellik Bölümü

        public ActionResult MusteriSikayetiDosyaDbSecimi(int id)
        {
            var dropListe = new List<DropItem>();
            string dbSecim = "POLYTEKS CARİ SEÇİMİ";
            if (id == 1)
            {
                dropListe = _dbPoly.ZzzSrcnMusteriSikayetOlusturmaTabloPTKS.GroupBy(a => new { a.CariAdi, a.CariNoCizgili }).Select(a => new DropItem() { Tanim = a.Key.CariAdi, Id = a.Key.CariNoCizgili }).OrderBy(a => a.Tanim).ToList();
            }
            if (id == 2)
            {
                dbSecim = "TAŞDELEN CARİ SEÇİMİ";
                dropListe = _dbTasd.ZzzSrcnMusteriSikayetTabloOlusturmaTabloTASD.GroupBy(a => new { a.CariAdi, a.CariNoCizgili }).Select(a => new DropItem() { Tanim = a.Key.CariAdi, Id = a.Key.CariNoCizgili }).OrderBy(a => a.Tanim).ToList();
            }
            var model = new SatisModel();
            model.CarilerPoly = new SelectList(dropListe.OrderBy(a => a.Tanim).ToList(), "Id", "Tanim");
            model.CariKoduTasd = dbSecim;
            return PartialView("~/Views/Yetkisiz/_MusteriSikayetiDosyaDbSecimi.cshtml", model);
        }
        public ActionResult MusteriSikayetiDosyaDbCariSecimi(int id, string id2)
        {
            // id= DB Seçimi
            //id2 = cariNoCizgili

            var model = new SatisModel();
            var dropListe = new List<DropItem>();

            if (id == 1)
            {
                dropListe = _dbPoly.ZzzSrcnMusteriSikayetOlusturmaTabloPTKS.Where(a => a.CariNoCizgili.Trim() == id2.Trim()).GroupBy(a => new { a.StokAdi, a.StokKoduCizgili }).Select(a => new DropItem() { Tanim = a.Key.StokAdi, Id = a.Key.StokKoduCizgili }).OrderBy(a => a.Tanim).ToList();
            }
            else
            {
                dropListe = _dbTasd.ZzzSrcnMusteriSikayetTabloOlusturmaTabloTASD.Where(a => a.CariNoCizgili.Trim() == id2.Trim()).GroupBy(a => new { a.StokAdi, a.StokKoduCizgili }).Select(a => new DropItem() { Tanim = a.Key.StokAdi, Id = a.Key.StokKoduCizgili }).OrderBy(a => a.Tanim).ToList();
            }




            model.CarilerTasd = new SelectList(dropListe.OrderBy(a => a.Tanim).ToList(), "Id", "Tanim");
            return PartialView("~/Views/Yetkisiz/_MusteriSikayetiDosyaDbCariSecimi.cshtml", model);
        }

        public ActionResult MusteriSikayetiDosyaStokKoduSecimi(string id)
        {
            // id= DB Seçimi
            //id2 = cariNoCizgili

            var kodlar = id.Split(new string[] { "--" }, StringSplitOptions.None).ToList();
            var DbTipi = kodlar[0];
            var CariNoCizgili = kodlar[1];
            var StokKoduCizgili = kodlar[2];

            var model = new SatisModel();
            var dropListe = new List<DropItem>();
            var araModel = new List<MusteriSikayetOlusturmaModel>();

            if (DbTipi == "1")
            {
                araModel = _dbPoly.ZzzSrcnMusteriSikayetOlusturmaTabloPTKS.Where(a => a.CariNoCizgili.Trim() == CariNoCizgili && a.StokKoduCizgili == StokKoduCizgili).Select(a => new MusteriSikayetOlusturmaModel()
                {
                    SevkTarihi = a.SevkTarihi,
                    StokAdi = a.StokAdi,
                    RefKartNo = a.RefKartNo,
                    PartiNo = a.PartiNo,
                    CariAdi = a.CariAdi,
                    CariNo = a.CariNo,
                    SeciliMi = false,
                    StokKodu = a.StokKodu,
                    SevkMiktari = a.SevkMiktari,
                    IrsalıyeNo = a.IrsalıyeNo,
                    Sira = 0
                }).OrderByDescending(a => a.SevkTarihi).ToList();
            }
            else
            {
                araModel = _dbTasd.ZzzSrcnMusteriSikayetTabloOlusturmaTabloTASD.Where(a => a.CariNoCizgili.Trim() == CariNoCizgili && a.StokKoduCizgili == StokKoduCizgili).Select(a => new MusteriSikayetOlusturmaModel()
                {
                    SevkTarihi = a.SevkTarihi,
                    StokAdi = a.StokAdi,
                    RefKartNo = a.RefKartNo,
                    PartiNo = a.PartiNo,
                    CariAdi = a.CariAdi,
                    CariNo = a.CariNo,
                    SeciliMi = false,
                    StokKodu = a.StokKodu,
                    SevkMiktari = a.SevkMiktari,
                    IrsalıyeNo = a.IrsalıyeNo,
                    Sira = 0
                }).OrderByDescending(a => a.SevkTarihi).ToList();
            }


            model.StokKodu = StokKoduCizgili;
            model.CariKodu = CariNoCizgili;
            model.DbTipi = DbTipi;
            int ii = 0;
            foreach (var item in araModel)
            {
                ii++;
                item.Sira = ii;
                model.MusteriSikayetOlusturmaModeller.Add(item);

            }
            model.CarilerTasd = new SelectList(dropListe.OrderBy(a => a.Tanim).ToList(), "Id", "Tanim");
            return PartialView("~/Views/Yetkisiz/_MusteriSikayetiDosyaStokKoduSecimi.cshtml", model);
        }
        #endregion
        public ActionResult UretimHattinaGoreMakineGetir(int? id)
        {
            var model = new LaboratuvarModel();
            model.Makineler = new SelectList(_dbPoly.Makine.Where(a => a.SBSonrasiCalismaSaati == id).OrderBy(a => a.MakineNo).ToList(),
                "SayacOndalikSayisi", "MakineNo");
            return PartialView("~/Views/Yetkisiz/_UretimHattiMakineleri.cshtml", model);
        }
        public ActionResult UretimHattinaGoreMakineGetirImalat(int? id)
        {
            var model = new ImalatModel();
            model.Makineler = new SelectList(_dbPoly.Makine.Where(a => a.SBSonrasiCalismaSaati == id).OrderBy(a => a.MakineNo).ToList(),
                "SayacOndalikSayisi", "MakineNo");


            return PartialView("~/Views/Yetkisiz/_UretimHattiMakineleriImalat.cshtml", model);
        }
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
        public ActionResult IkPersonelKullaniciGetir(string id)
        {

            var sql = string.Format(
             "SELECT * FROM [POTA_PTKS].[dbo].[SrcnKullanicis] (NOLOCK) WHERE [IsimSoyisim] LIKE  ('%{0}%') ", id);


            var liste = _dbPoly.Set<SrcnKullanicis>().SqlQuery(sql).ToList();

            //  var liste = _dbPoly.SrcnKullanicis.Where(a => a.IsimSoyisim.ToUpper().Contains(id) && a.GizlemeDurumu == 0).OrderBy(a => a.IsimSoyisim).ToList();
            return PartialView("~/Views/Yetkisiz/_IkPersonelKullaniciListeGetir.cshtml", liste);
        }
        public ActionResult ImalatSecilenPartiNoOzetGetir(int id, string id2)
        {
            // id birim
            // id2 =refakatno
            var model = new ImalatPartiSonuModel();
            var liste = new List<ZzzSrcnPartiSonuDurumOzet>();
            if (id2 == null)
            {
                liste = _dbPoly.ZzzSrcnPartiSonuDurumOzet.Where(a => a.SabitlenenMakineNo != "1-srcn" && a.BirimId == id).OrderBy(a => a.PartiNo).ThenBy(a => a.IslemBaslamaTarihi).ToList();
            }
            else
            {
                var refItem = _dbPoly.ZzzSrcnPartiSonuDurumOzet.First(a => a.RefakatNo == id2);
                string Tanim = refItem.PartiNo;
                liste = _dbPoly.ZzzSrcnPartiSonuDurumOzet.Where(a => a.SabitlenenMakineNo != "1-srcn").Where(a => a.PartiNo == Tanim).OrderBy(a => a.IslemBaslamaTarihi).ToList();

            }


            foreach (var item in liste.ToList())
            {
                model.PartiSonuDurumOzetCheckItemlar.Add(new PartiSonuDurumOzetCheckItem() { PartiSonuDurumOzet = item });
            }
            model.OnayAlinacakBirimler = _dbPoly.SrcnFabrikaBirims.Where(a => (a.BirimId < 5 || (a.BirimId == 18)) && (a.BirimId != id))
                .OrderBy(a => a.BirimAdi).ToList();
            model.Id = id;
            return PartialView("~/Views/Imalat/_ImalatPartiSonuListe.cshtml", model);
        }
        public ActionResult ImalatSecilenPartiNoGIATO(int id, string id2)
        {
            var model = new ImalatModel { Birim = _dbPoly.SrcnFabrikaBirims.Find(id) };
            var liste = new List<ZzzSrcnLottanOzelAlanaGecisler>();
            var araModel = new List<GunlukImalatMatrisHucreSatir>();
            if (id2 == null)
            {
                liste = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.Where(a => a.BirimId == id).OrderBy(a => a.PartiNo).ToList();
            }
            else
            {
                var refItem = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.First(a => a.RefakatNo == id2);
                string Tanim = refItem.PartiNo;
                liste = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.Where(a => a.PartiNo == Tanim).OrderBy(a => a.MakineNo).ToList();
            }
            if (Session["GIATO"] != null)
            {
                araModel = (List<GunlukImalatMatrisHucreSatir>)Session["GIATO"];
            }

            if (model.Birim.BirimId == 3)
            {
                // poy/fdy
                var Kanallar = new List<DropItem>();
                for (int i = 1; i <= 8; i++)
                {
                    Kanallar.Add(new DropItem() { IdInt = i, Tanim = string.Format("{0}. Kanal", i) });
                }

                model.Kanallar = Kanallar;

                foreach (var item in liste)
                {
                    if (araModel.Count(a => a.LottanOzelAlanaGecis.RefakatNo == item.RefakatNo) == 0)
                    {
                        var sayi = 0;
                        if (item.KanalSayisi.Replace(" ", "").ToCharArray().Length > 0)
                        {
                            sayi = Convert.ToInt32(item.KanalSayisi);
                        }
                        var araItem = new GunlukImalatMatrisHucreSatir()
                        {
                            LottanOzelAlanaGecis = item
                        };

                        foreach (var dropItem in Kanallar)
                        {
                            //  bool MevcutMu = false || sayi <= dropItem.IdInt;

                            araItem.GunlukImalatMatrisHucreler.Add(new GunlukImalatMatrisHucre() { LottanOzelAlanaGecis = item, KanalAdi = dropItem.Tanim, KanalNo = dropItem.IdInt, MevcutMu = false || sayi >= dropItem.IdInt });
                        }
                        araModel.Add(araItem);
                    }
                }
            }
            else
            {
                foreach (var item in liste)
                {
                    if (araModel.Count(a => a.LottanOzelAlanaGecis.RefakatNo == item.RefakatNo) == 0)
                    {
                        var araItem = new GunlukImalatMatrisHucreSatir()
                        {
                            LottanOzelAlanaGecis = item
                        };
                        araModel.Add(araItem);
                    }
                }
            }



            Session["GIATO"] = araModel;
            model.GunlukImalatMatrisHucreSatirlar = araModel.OrderBy(a => a.LottanOzelAlanaGecis.MakineNo).ToList();


            return PartialView("~/Views/Imalat/_ImalatGIATO.cshtml", model);
        }
        public ActionResult ImalatSecilenPartiNoKaldirGIATO(int id, string id2)
        {
            var model = new ImalatModel { Birim = _dbPoly.SrcnFabrikaBirims.Find(id) };
            var liste = new List<ZzzSrcnLottanOzelAlanaGecisler>();
            var araModel = new List<GunlukImalatMatrisHucreSatir>();

            if (Session["GIATO"] != null)
            {
                araModel = (List<GunlukImalatMatrisHucreSatir>)Session["GIATO"];
            }

            araModel = araModel.Where(a => a.LottanOzelAlanaGecis.RefakatNo.Trim() != id2)
                .OrderBy(a => a.LottanOzelAlanaGecis.MakineNo).ToList();
            Session["GIATO"] = araModel;
            model.GunlukImalatMatrisHucreSatirlar = araModel;


            return PartialView("~/Views/Imalat/_ImalatKIATO.cshtml", model);
        }

        public ActionResult ImalatSecilenPartiNoKIATO(int id, string id2)
        {
            var model = new ImalatModel { Birim = _dbPoly.SrcnFabrikaBirims.Find(id) };
            var liste = new List<ZzzSrcnLottanOzelAlanaGecisler>();
            var araModel = new List<GunlukImalatMatrisHucreSatir>();
            if (id2 == null)
            {
                liste = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.Where(a => a.BirimId == id).OrderBy(a => a.PartiNo).ToList();
            }
            else
            {
                var refItem = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.First(a => a.RefakatNo == id2);
                string Tanim = refItem.PartiNo;
                liste = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.Where(a => a.PartiNo == Tanim).OrderBy(a => a.MakineNo).ToList();
            }
            if (Session["KIATO"] != null)
            {
                araModel = (List<GunlukImalatMatrisHucreSatir>)Session["KIATO"];
            }

            if (model.Birim.BirimId == 3)
            {
                // poy/fdy
                var Kanallar = new List<DropItem>();
                for (int i = 1; i <= 8; i++)
                {
                    Kanallar.Add(new DropItem() { IdInt = i, Tanim = string.Format("{0}. Kanal", i) });
                }

                model.Kanallar = Kanallar;

                foreach (var item in liste)
                {
                    if (araModel.Count(a => a.LottanOzelAlanaGecis.RefakatNo == item.RefakatNo) == 0)
                    {
                        var sayi = 0;
                        if (item.KanalSayisi.Replace(" ", "").ToCharArray().Length > 0)
                        {
                            sayi = Convert.ToInt32(item.KanalSayisi);
                        }
                        var araItem = new GunlukImalatMatrisHucreSatir()
                        {
                            LottanOzelAlanaGecis = item
                        };

                        foreach (var dropItem in Kanallar)
                        {
                            //  bool MevcutMu = false || sayi <= dropItem.IdInt;

                            araItem.GunlukImalatMatrisHucreler.Add(new GunlukImalatMatrisHucre() { LottanOzelAlanaGecis = item, KanalAdi = dropItem.Tanim, KanalNo = dropItem.IdInt, MevcutMu = false || sayi >= dropItem.IdInt });
                        }
                        araModel.Add(araItem);
                    }
                }
            }
            else
            {
                foreach (var item in liste)
                {
                    if (araModel.Count(a => a.LottanOzelAlanaGecis.RefakatNo == item.RefakatNo) == 0)
                    {
                        var araItem = new GunlukImalatMatrisHucreSatir()
                        {
                            LottanOzelAlanaGecis = item
                        };
                        araModel.Add(araItem);
                    }
                }
            }



            Session["KIATO"] = araModel;
            model.GunlukImalatMatrisHucreSatirlar = araModel.OrderBy(a => a.LottanOzelAlanaGecis.MakineNo).ToList();

            var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0 && a.Sira > 0).OrderBy(a => a.Sira).ToList();

            model.LabAnalizCesitleri = TumIplikAnalizCesitleri;

            return PartialView("~/Views/Imalat/_ImalatKIATO.cshtml", model);
        }
        public ActionResult ImalatSecilenPartiNoKaldirKIATO(int id, string id2)
        {
            var model = new ImalatModel { Birim = _dbPoly.SrcnFabrikaBirims.Find(id) };
            var liste = new List<ZzzSrcnLottanOzelAlanaGecisler>();
            var araModel = new List<GunlukImalatMatrisHucreSatir>();

            if (Session["KIATO"] != null)
            {
                araModel = (List<GunlukImalatMatrisHucreSatir>)Session["KIATO"];
            }

            araModel = araModel.Where(a => a.LottanOzelAlanaGecis.RefakatNo.Trim() != id2)
                .OrderBy(a => a.LottanOzelAlanaGecis.MakineNo).ToList();
            Session["KIATO"] = araModel;
            model.GunlukImalatMatrisHucreSatirlar = araModel;

            var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0 && a.Sira > 0).OrderBy(a => a.Sira).ToList();

            model.LabAnalizCesitleri = TumIplikAnalizCesitleri;

            return PartialView("~/Views/Imalat/_ImalatGIATO.cshtml", model);
        }



        public ActionResult IzlenebilirlikRefakatNoDetay(string id)
        {
            var izlenebilirlik = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.First(a => a.RefakatNo == id);
            var partiSonuTakip = new SrcnPartiSonuTakips();
            var bilgiOnaylar = new List<SrcnPartiSonuTakipBilgiOnays>();
            if (izlenebilirlik.PartiSonuTakipId != null)
            {
                partiSonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(izlenebilirlik.PartiSonuTakipId);
                bilgiOnaylar = _dbPoly.SrcnPartiSonuTakipBilgiOnays
                    .Where(a => a.PartiSonuTakipId == partiSonuTakip.PartiSonuTakipId)
                    .OrderByDescending(a => a.OnayTarihi).ToList();
            }

            var model = new IzlenebilirlikPartiSonuModel()
            {
                PartiSonuTakipIzlenebilirlik = izlenebilirlik,
                PartiSonuTakip = partiSonuTakip,
                PartiSonuTakipBilgiOnaylari = bilgiOnaylar,
                Siparis = _dbPoly.Siparis.First(a => a.SiparisNo == izlenebilirlik.SiparisNo && a.SiraNo == izlenebilirlik.SiparisSiraNo),
                SiparisAna = _dbPoly.SiparisAna.First(a => a.SiparisNo == izlenebilirlik.SiparisNo)
            };


            return PartialView("~/Views/Izlenebilirlik/_IzlenebilirlikRefkartNoDetay.cshtml", model);
        }
        public ActionResult BirimMakineyeGoreParametreleriGetir(int id)
        {
            var model = new ImalatDenemelerModel();
            var Hamliste = _dbPoly.ZzzSrcnIslemMakineParametre.AsNoTracking().Where(a => a.MakineId == id).OrderBy(a => a.ParametreAdi).ToList();

            var DistParametreNolar = Hamliste.Select(a => a.ParametreNo).Distinct();

            foreach (var i in DistParametreNolar)
            {
                model.IslemMakineParametreler.Add(Hamliste.First(a => a.ParametreNo == i));
            }


            return PartialView("~/Views/Yetkisiz/_BirimMakineyeGoreParametreleriGetir.cshtml", model);
        }

        public ActionResult ImalatDenemeDosyaDetayMakineItemGetir(int id, string id2)
        {
            // id= denemedosyaId
            // id2=makineId

            var birlesikId = id2.Split('-');
            int makineId = Convert.ToInt32(birlesikId[0]);
            string IslemNo = birlesikId[1].ToString();
            var islem = _dbPoly.Islem.First(a => a.IslemNo.Replace(" ", "") == IslemNo);

            var model = new ImalatNumuneModel();
            model.DenemeDosya = _dbPoly.SrcnDenemeDosya.Find(id);
            var ParametreNolar = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.MakineId == makineId && a.IslemNo == islem.IslemNo).OrderBy(a => a.ParametreAdi).ToList();

            model.Makine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == makineId);
            model.Islem = islem;


            foreach (var item in ParametreNolar)
            {
                //  var item = _dbPoly.ZzzSrcnIslemMakineParametre.First(a => a.MakineId == makineId && a.ParametreNo == i);
                model.DosyaItemMakineParametreler.Add(new SrcnDosyaItemMakineParametres()
                {
                    ParametreAdi = item.ParametreAdi.Trim(),
                    ParametreNo = item.ParametreNo,
                    OlcuBirimi = item.OlcuBirimi,
                });
            }

            model.DosyaItemMakineParametreler = model.DosyaItemMakineParametreler.OrderBy(a => a.ParametreAdi).ToList();

            model.LabAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 0 && a.Sira > 0).OrderBy(a => a.Sira).ToList();

            return PartialView("~/Views/Imalat/_ImalatNumuneDosyaItemEkle.cshtml", model);
        }

        public ActionResult ImalatPartiAltKlasorMakineItemGetir(int id, string id2)
        {
            // id= PartiAltKlasorTipi
            // id2=makineId

            var birlesikId = id2.Split('-');
            int makineId = Convert.ToInt32(birlesikId[0]);
            string IslemNo = birlesikId[1].ToString();
            var islem = _dbPoly.Islem.First(a => a.IslemNo.Replace(" ", "") == IslemNo);
            var model = new ImalatPartiAltKlasorModel();
            model.PartiAltKlasorTipi = _dbPoly.SrcnPartiAltKlasorTipis.Find(id);
            var ParametreNolar = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.MakineId == makineId && a.IslemNo == islem.IslemNo).OrderBy(a => a.ParametreAdi).ToList();
            model.Makine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == makineId);
            model.Islem = islem;


            foreach (var item in ParametreNolar)
            {
                //  var item = _dbPoly.ZzzSrcnIslemMakineParametre.First(a => a.MakineId == makineId && a.ParametreNo == i);
                model.PartiAltKlasorDosyaItemlar.Add(new SrcnPartiAltKlasorDosyaItems()
                {
                    ParametreAdi = item.ParametreAdi.Trim(),
                    ParametreNo = item.ParametreNo,
                    OlcuBirimi = item.OlcuBirimi,
                });
            }

            model.PartiAltKlasorDosyaItemlar = model.PartiAltKlasorDosyaItemlar.OrderBy(a => a.ParametreAdi).ToList();
            return PartialView("~/Views/Imalat/_ImalatPartiAltKlasorMakineItem.cshtml", model);
        }


        public ActionResult PaketlemePArtiNoyaGoreGunlukItemlariGetir(int id)
        {
            var model = new PaketlemeGunlukTalimatModel();
            var Hamliste = _dbPoly.SrcnPaketlemeGunlukTalimatItems.AsNoTracking().Where(a => a.PartiAnaKlasorId == id).OrderByDescending(a => a.KayitTarihi).ToList();

            model.GunlukTalimatItemlar = Hamliste;



            return PartialView("~/Views/Yetkisiz/_PaketlemePArtiNoyaGoreGunlukItemlariGetir.cshtml", model);
        }

        public ActionResult OneriGetir(string id)
        {


            var AraListe = _dbPoly.SrcnPartiAnaKlasors.Where(n => n.PartiAdi.ToUpper().Contains(id.ToUpper())).ToList();
            if (AraListe.Any())
            {
                var namelist = AraListe.Select(a => new { label = a.PartiAdi, PartiAnaKlasorId = a.PartiAnaKlasorId.ToString() }).ToList();
                return Json(namelist, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult PartiAdiGetir(string id)
        {


            var AraListe = _dbPoly.SrcnPartiAnaKlasors.Where(n => n.PartiAdi.ToUpper().Contains(id.ToUpper())).ToList();
            if (AraListe.Any())
            {
                var namelist = AraListe.Select(a => new { label = a.PartiAdi, PartiAnaKlasorId = a.PartiAnaKlasorId.ToString() }).ToList();
                return Json(namelist, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult DenemeAdiGetir(string id)
        {


            var AraListe = _dbPoly.SrcnDenemeDosya.Where(n => n.DenemeAdi.ToUpper().Contains(id.ToUpper())).ToList();
            if (AraListe.Any())
            {
                var namelist = AraListe.Select(a => new { label = a.DenemeAdi, DenemeDosyaId = a.DenemeDosyaId.ToString() }).ToList();
                return Json(namelist, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult StokAdiGetir(string id)
        {


            var AraListe = _dbPoly.ZzzSrcnDenemeNumuneStoks.Where(n => n.LotNo.ToUpper().Contains(id.ToUpper())).ToList();
            if (AraListe.Any())
            {
                var namelist = AraListe.Select(a => new { label = a.LotNo.Trim(), StokKodu = a.StokKoduCizgili.Trim().ToString() }).ToList();
                return Json(namelist, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult NumuneDenemeHamMaddeListeyeEkle(int id, string id2)
        {
            //id=> 1=deneme, 2= parti no, 3= stok


            var model = new List<DropItem>();

            if (Session["NumDenModel"] != null)
            {
                model = (List<DropItem>)Session["NumDenModel"];
            }

            var dropItem = new DropItem();

            {
                if (id == 1)
                {
                    var deneme = _dbPoly.SrcnDenemeDosya.Find(Convert.ToInt32(id2));

                    dropItem = new DropItem()
                    {
                        Tanim = deneme.DenemeAdi,
                        DigerTanim = "Deneme & Numune",
                        Id = id2,
                        IdInt = id
                    };

                }
                if (id == 2)
                {
                    var partiAnaKlasor = _dbPoly.SrcnPartiAnaKlasors.Find(Convert.ToInt32(id2));
                    dropItem = new DropItem()
                    {
                        Tanim = partiAnaKlasor.PartiAdi,
                        DigerTanim = "POTA-PARTİ",
                        Id = id2,
                        IdInt = id
                    };
                }
                if (id == 3)
                {

                    var stokTanim = _dbPoly.ZzzSrcnDenemeNumuneStoks.First(a => a.StokKoduCizgili == id2);
                    dropItem = new DropItem()
                    {
                        Tanim = stokTanim.LotNo,
                        DigerTanim = "HAM MADDE",
                        Id = id2,
                        IdInt = id
                    };
                }

                if (model.Count(a => a.Id == id2 && a.IdInt == id) == 0)
                {
                    model.Add(dropItem);
                }
                int say = 0;

                model = model.OrderBy(a => a.Tanim).ToList();
                foreach (var item in model)
                {
                    say++;
                    item.Sira = say;
                }

                Session["NumDenModel"] = model;
                return PartialView("~/Views/Yetkisiz/_NumuneDenemeHamMaddeListeyeEkle.cshtml", model);
            }

        }


        public ActionResult NumuneDenemeHamMaddeListedenSil(int id)
        {



            var model = new List<DropItem>();

            if (Session["NumDenModel"] != null)
            {
                model = (List<DropItem>)Session["NumDenModel"];
            }

            var dropItem = new DropItem();

            {
                

              

                model = model.Where(a => a.Sira!= id).ToList();
             

                Session["NumDenModel"] = model;
                return PartialView("~/Views/Yetkisiz/_NumuneDenemeHamMaddeListeyeEkle.cshtml", model);
            }

        }
    }
}