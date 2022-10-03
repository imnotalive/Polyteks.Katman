using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using PagedList;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.TefrikBildirim.Models;

namespace Polyteks.Katman.Has.Controllers
{
    public class InsanKaynaklariController : BaseController
    {
        // GET: InsanKaynaklari

        public string OturumSureAciklama(DateTime baslangic, string basSaat, DateTime bitis, string bitSaat)
        {

            if (baslangic == bitis)
            {
                // saat farkı gerekiyor
                var splitBaslangic = basSaat.Split(':');
                var splitBitis = bitSaat.Split(':');

                int SaatBasl = Convert.ToInt32(splitBaslangic[0]);
                int SaatBits = Convert.ToInt32(splitBitis[0]);
                int DakBasl = Convert.ToInt32(splitBaslangic[0]);
                int DakBits = Convert.ToInt32(splitBitis[0]);

                var farkSaat = Math.Abs(SaatBits - SaatBasl);

                if (Math.Abs(DakBits - DakBasl) > 0)
                {
                    farkSaat++;
                }

                return farkSaat + " Saat";

            }
            else
            {
                // gün farkı gerekiyor
                var farkGun = Convert.ToInt32((bitis - baslangic).TotalDays);

                return farkGun + " Gün";
            }

        }
        public IkModel IkModeliBaseOlustur()
        {
            var model = new IkModel();
            model.IkEgitimTipleri = _dbPoly.SrcnIkEgitimTipis.OrderBy(a => a.IkEgitimTipiAdi).ToList();
            ViewBag.IkEgitimTipleri = model.IkEgitimTipleri;
            return model;
        }

        public ActionResult PersonelSicilIslemler()
        {
            TempData["Gizle"] = "gizle";

            return View(IkModeliBaseOlustur());
        }

        //public ActionResult PersonelOzlukBilgiler(int? id)
        //{
        //    TempData["Gizle"] = "gizle";
        //    var model = new PersonelOzlukBilgiModel();


        //    int idd = Convert.ToInt32(id);
        //    var liste = _dbPoly.ZzzSrcnKullaniciCalismaPozisyonOzet.AsNoTracking().Where(a => a.BirimId == id).OrderBy(a => a.IsimSoyisim).ToList();
        //    model.SrcnKullaniciCalismaPozisyonOzetler = liste;
        //    var fabrikaBirimleri = _dbPoly.SrcnFabrikaBirims.AsNoTracking().OrderBy(a => a.BirimAdi).ToList();

        //    var dropListe = new List<DropItem>()
        //    {
        //        new DropItem(){Tanim = "Tanımlı Olmayan Kişiler", Id = null}
        //    };

        //    dropListe.AddRange(fabrikaBirimleri.Select(a => new DropItem() { Tanim = a.BirimAdi.ToUpper(), Id = a.BirimId.ToString() }).ToList());
        //    model.OzelDropMenuler = dropListe;


        //    return View(model);
        //}

        public ActionResult PersonelOzlukBilgiler()
        {
            TempData["Gizle"] = "gizle";

            var liste = _dbPoly.ZzzSrcnKullaniciCalismaPozisyonOzet.AsNoTracking().ToList();

            return View(liste);
        }

        public ActionResult PersonelOzlukBilgisiEkle()
        {
            var model = new PersonelOzlukBilgiModel()
            {
                GrupId = 0,
                BirimId = 0,
                CalismaPozisyonu = 0,
                Vardiya = 0,
                SicilNo = "",
                NufusaKayitliOlduguIl = "1",
                IsyeriKodu = "0"
            };

            var liste = _dbPoly.ZzzSrcnKullaniciCalismaPozisyonOzet.OrderBy(a => a.IsimSoyisim).ToList();

            var GrupIdler = liste.Select(a => a.GrupId).Distinct().ToList();

            var vardiyalar = new List<DropItem>();
            var GruplarListe = new List<DropItem>();
            var calismaPozisyonlari = new List<DropItem>();


            var fabrikaBirimleri = _dbPoly.SrcnFabrikaBirims.AsNoTracking().OrderBy(a => a.BirimAdi).ToList();
            var fabrikaBolumleri = _dbPoly.SrcnFabrikaBolums.AsNoTracking().OrderBy(a => a.BolumAdi).ToList();

            for (int i = 1; i <= 4; i++)
            {
                vardiyalar.Add(new DropItem()
                {
                    Tanim = i.ToString(),
                    IdInt = i
                });
            }
            foreach (var i in GrupIdler)
            {
                if (i != null)
                {
                    GruplarListe.Add(new DropItem()
                    {
                        Tanim = liste.First(a => a.GrupId == i).GrupAdi,
                        IdInt = Convert.ToInt32(i)
                    });
                }
            }

            foreach (var item in _dbPoly.SrcnIkCalismaPozisyons.AsNoTracking().OrderBy(a => a.YakaTipiAdi).ThenBy(a => a.CalismaPozisyonAdi).ToList())
            {
                calismaPozisyonlari.Add(new DropItem()
                {
                    Tanim = string.Format("{0} - {1}", item.YakaTipiAdi, item.CalismaPozisyonAdi),
                    IdInt = item.IkCalismaPozisyonId
                });
            }
            model.DropVardiyalar = new SelectList(vardiyalar.ToList(), "IdInt", "Tanim");

            model.DropGruplar = new SelectList(GruplarListe.OrderBy(a => a.Tanim).ToList(), "IdInt", "Tanim");

            model.DropCalismaPozisyonlari = new SelectList(calismaPozisyonlari.ToList(), "IdInt", "Tanim");

            model.DropFabBirimleri = new SelectList(fabrikaBirimleri.ToList(), "BirimId", "BirimAdi");
            model.DropFabrikaBolumleri = new SelectList(fabrikaBolumleri.ToList(), "BolumId", "BolumAdi");



            return View(model);
        }
        [HttpPost]
        public ActionResult PersonelOzlukBilgisiEkle(PersonelOzlukBilgiModel model)
        {

            string birim = "";
            string bolum = "";
            string grup = "";

            if (model.GrupId != null && model.GrupId != 0)
            {
                grup = _dbPoly.SrcnKullanicis.First(a => a.GrupId == model.GrupId).GrupAdi;
            }
            if (model.BirimId != null && model.BirimId != 0)
            {
                birim = _dbPoly.SrcnFabrikaBirims.First(a => a.BirimId == model.BirimId).BirimAdi;
            }
            if (model.BolumId != null && model.BolumId != 0)
            {
                bolum = _dbPoly.SrcnFabrikaBolums.First(a => a.BolumId == model.BolumId).BolumAdi;
            }


            string sicilNo = model.SicilNo.Substring(2, 4);
            //[Display(Name = "Bu alan boş bırakılamaz!")];

            string topPersonelKodu = _dbPoly.SrcnKullanicis.OrderByDescending(a => a.KullaniciId)
                .Select(a => a.PersonelKodu).FirstOrDefault();

            string[] ayristirmis = topPersonelKodu.Split('P');
            int convertTopPersonelKodu = Convert.ToInt32(ayristirmis[1]);

            convertTopPersonelKodu++;

            string personelKodu = "P" + convertTopPersonelKodu.ToString();

            try
            {
                //if (!String.IsNullOrEmpty(model.SicilNo))
                //{
                //    var yeniKayit = new Personel()
                //    {
                //        ElektronikPosta = model.SrcnKullanici.EmailAdres,
                //        PersonelNo = personelKodu,
                //        NufusaKayitliOlduguIl = "1",
                //        IsyeriKodu = "0",
                //        EvTelefonu = sicilNo,
                //        PersonelAdi = model.SrcnKullanici.IsimSoyisim
                //    };
                //    _dbPoly.Personel.Add(yeniKayit);
                //    _dbPoly.SaveChanges();
                //    TempDataOlustur("Kayıt işlemi yapılmıştır", true);
                //}
                //else
                //{
                //    TempDataOlustur("Kayıt sırasında hata oluştu. Lütfen bilgileri kontrol edin.", false);
                //}



                if (!String.IsNullOrEmpty(model.SicilNo))
                {


                    var yeniKayitKullanici = new SrcnKullanicis()
                    {
                        EmailAdres = model.SrcnKullanici.EmailAdres,
                        PersonelKodu = personelKodu,
                        BirimId = model.BirimId,
                        GrupAdi = grup,
                        GrupId = model.GrupId,
                        Birim = birim,
                        Vardiya = model.Vardiya,
                        Bolum = bolum,
                        BolumId = model.BolumId,
                        KullaniciKodu = null,
                        Sifre = sicilNo,
                        IsimSoyisim = model.SrcnKullanici.IsimSoyisim,
                        GizlemeDurumu = 0,
                        SeciliMi = false,
                        IkCalismaPozisyonId = model.CalismaPozisyonu,
                        GirisSifre = model.SicilNo
                    };
                    _dbPoly.SrcnKullanicis.Add(yeniKayitKullanici);
                    _dbPoly.SaveChanges();
                }
                else
                {
                    TempDataOlustur("Kayıt sırasında hata oluştu. Lütfen bilgileri kontrol edin.", false);
                }
            }
            catch (Exception e)
            {
                TempDataOlustur("Bir hata oluştu lütfen IT birimine bildiriniz.", false);
            }

            return RedirectToAction("PersonelOzlukBilgiler", "InsanKaynaklari");
        }
        public ActionResult PersonelOzlukBilgiDuzenle(int id, string personelKodu)
        {
            TempData["Gizle"] = "gizle";
            var kul = _dbPoly.SrcnKullanicis.Find(id);
            var model = new PersonelOzlukBilgiModel()
            {
                GrupId = 0,
                BirimId = 0,
                CalismaPozisyonu = 0,
                Vardiya = 0,
                SicilNo = "",
                GercekSicilNo = "",
            };
            model.SrcnKullanici = kul;
            if (kul.GrupId != null)
            {
                model.GrupId = Convert.ToInt32(kul.GrupId);
            }
            if (kul.GirisSifre != null)
            {
                model.GercekSicilNo = kul.GirisSifre;
            }
            if (kul.BirimId != null)
            {
                model.BirimId = Convert.ToInt32(kul.BirimId);
            }
            if (kul.IkCalismaPozisyonId != null)
            {
                model.CalismaPozisyonu = Convert.ToInt32(kul.IkCalismaPozisyonId);
            }
            if (kul.Vardiya != null)
            {
                model.Vardiya = Convert.ToInt32(kul.Vardiya);
            }
            if (model.SrcnKullanici.PersonelKodu == personelKodu)
            {
                model.SicilNo = _dbPoly.Personel.Where(a => a.PersonelNo == personelKodu).Select(a => a.EvTelefonu).FirstOrDefault();
            }
            var liste = _dbPoly.ZzzSrcnKullaniciCalismaPozisyonOzet.OrderBy(a => a.IsimSoyisim).ToList();

            var GrupIdler = liste.Select(a => a.GrupId).Distinct().ToList();

            var vardiyalar = new List<DropItem>();
            var GruplarListe = new List<DropItem>();
            var calismaPozisyonlari = new List<DropItem>();


            var fabrikaBirimleri = _dbPoly.SrcnFabrikaBirims.AsNoTracking().OrderBy(a => a.BirimAdi).ToList();
            var fabrikaBolumleri = _dbPoly.SrcnFabrikaBolums.AsNoTracking().OrderBy(a => a.BolumAdi).ToList();

            for (int i = 1; i <= 4; i++)
            {
                vardiyalar.Add(new DropItem()
                {
                    Tanim = i.ToString(),
                    IdInt = i
                });
            }
            foreach (var i in GrupIdler)
            {
                if (i != null)
                {
                    GruplarListe.Add(new DropItem()
                    {
                        Tanim = liste.First(a => a.GrupId == i).GrupAdi,
                        IdInt = Convert.ToInt32(i)
                    });
                }
            }

            foreach (var item in _dbPoly.SrcnIkCalismaPozisyons.AsNoTracking().OrderBy(a => a.YakaTipiAdi).ThenBy(a => a.CalismaPozisyonAdi).ToList())
            {
                calismaPozisyonlari.Add(new DropItem()
                {
                    Tanim = string.Format("{0} - {1}", item.YakaTipiAdi, item.CalismaPozisyonAdi),
                    IdInt = item.IkCalismaPozisyonId
                });
            }
            model.DropVardiyalar = new SelectList(vardiyalar.ToList(), "IdInt", "Tanim");

            model.DropGruplar = new SelectList(GruplarListe.OrderBy(a => a.Tanim).ToList(), "IdInt", "Tanim");

            model.DropCalismaPozisyonlari = new SelectList(calismaPozisyonlari.ToList(), "IdInt", "Tanim");

            model.DropFabBirimleri = new SelectList(fabrikaBirimleri.ToList(), "BirimId", "BirimAdi");
            model.DropFabrikaBolumleri = new SelectList(fabrikaBolumleri.ToList(), "BolumId", "BolumAdi");

            return View(model);
        }

        [HttpPost]
        public ActionResult PersonelOzlukBilgiDuzenle(PersonelOzlukBilgiModel model)
        {
            var personel = model.SrcnKullanici;

            var editPersonel = _dbPoly.SrcnKullanicis.Find(personel.KullaniciId);
            editPersonel.EmailAdres = personel.EmailAdres;
            var personelKodu = model.SrcnKullanici.PersonelKodu;
            var sifreEdit = _dbPoly.Personel.Where(a => a.PersonelNo == personelKodu).FirstOrDefault();
            if (sifreEdit != null)
            {
                sifreEdit.EvTelefonu = model.SicilNo;
                editPersonel.Sifre = model.SicilNo;
            }

            if (model.GrupId != 0)
            {
                var grupAdi = _dbPoly.SrcnKullanicis.First(a => a.GrupId == model.GrupId).GrupAdi;
                editPersonel.GrupId = model.GrupId;
                editPersonel.GrupAdi = grupAdi;
            }
            if (model.BirimId != 0)
            {
                var birim = _dbPoly.SrcnFabrikaBirims.Find(model.BirimId);
                editPersonel.Birim = birim.BirimAdi;
                editPersonel.BirimId = birim.BirimId;

            }
            if (model.CalismaPozisyonu != 0)
            {
                var calismaPozisyon = _dbPoly.SrcnIkCalismaPozisyons.Find(model.CalismaPozisyonu);
                editPersonel.IkCalismaPozisyonId = calismaPozisyon.IkCalismaPozisyonId;
                editPersonel.YakaTipi = editPersonel.YakaTipi;

            }
            if (model.Vardiya != 0)
            {
                editPersonel.Vardiya = model.Vardiya;
            }
            if (model.BolumId != 0)
            {
                var bolum = _dbPoly.SrcnFabrikaBolums.Find(model.BolumId);
                editPersonel.BolumId = bolum.BolumId;
                editPersonel.Bolum = bolum.BolumAdi;

            }
            editPersonel.GizlemeDurumu = personel.GizlemeDurumu;
            editPersonel.GirisSifre = model.GercekSicilNo;
            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(2);
            return RedirectToAction("PersonelOzlukBilgiler", "InsanKaynaklari", new { id = editPersonel.BirimId });
        }

        #region Egitim Crud

        public ActionResult EgitimTanimListesi()
        {
            var model = IkModeliBaseOlustur();
            model.IkEgitimler = _dbPoly.SrcnIkEgitims.OrderBy(a => a.IkEgitimTipiAdi).ThenBy(a => a.IkEgitimAdi)
                .ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult EgitimEkleDuzenle(SrcnIkEgitims egitim)
        {

            var egitimTipi = _dbPoly.SrcnIkEgitimTipis.Find(egitim.IkEgitimTipi);
            if (egitim.IkEgitimId == 0)
            {
                if (egitimTipi.IkEgitimTipiAdi == null || egitim.IkEgitimTipi == 0)
                {
                    TempDataOlustur("Eğitim Adı veya Eğitim Tipi Belirlemediniz", false);
                }
                else
                {
                    var yeniKayit = new SrcnIkEgitims
                    {
                        IkEgitimTipi = egitimTipi.IkEgitimTipi,
                        IkEgitimAdi = egitim.IkEgitimAdi,
                        IkEgitimTipiAdi = egitimTipi.IkEgitimTipiAdi
                    };
                    _dbPoly.SrcnIkEgitims.Add(yeniKayit);
                    _dbPoly.SaveChanges();
                    TempDataOlustur("Kayıt işlemi yapılmıştır", true);
                }

            }
            else
            {
                var editEgitim = _dbPoly.SrcnIkEgitims.Find(egitim.IkEgitimId);
                editEgitim.IkEgitimTipi = egitimTipi.IkEgitimTipi;
                editEgitim.IkEgitimAdi = egitim.IkEgitimAdi;
                editEgitim.IkEgitimTipiAdi = egitimTipi.IkEgitimTipiAdi;
                _dbPoly.SaveChanges();
                TempDataOlustur("Güncelleme işlemi yapılmıştır", false);

            }

            return RedirectToAction("EgitimTanimListesi");
        }

        public ActionResult EgitimSil(int id)
        {
            if (_dbPoly.SrcnIkEgitimFirmas.Any(a => a.IkEgitimId == id))
            {
                TempDataOlustur(
                    "Bu eğitimi almış personel ya da eğitimi verebilecek firma bağlantıları bulunduğu için silme işlemi yapılamaz",
                    false);
            }
            else
            {
                var item = _dbPoly.SrcnIkEgitims.Find(id);
                _dbPoly.SrcnIkEgitims.Remove(item);
                _dbPoly.SaveChanges();
                TempDataOlustur("Silme İşlemi Yapılmıştır", true);
            }
            return RedirectToAction("EgitimTanimListesi");
        }

        #endregion

        #region Firma Crud
        public ActionResult FirmaTanimListesi()
        {
            var model = IkModeliBaseOlustur();
            model.IkFirmalar = _dbPoly.SrcnIkFirmas.OrderBy(a => a.IkFirmaAdi).ToList();
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult FirmaEkleDuzenle(SrcnIkFirmas firma)
        {

            if (firma.IkFirmaId == 0)
            {
                if (firma.IkFirmaAdi == null)
                {
                    TempDataOlustur("Eğitim Adı Belirlemediniz", false);
                }
                else
                {
                    var yeniKayit = new SrcnIkFirmas()
                    {
                        IkFirmaAdi = firma.IkFirmaAdi,
                        Aciklama = firma.Aciklama
                    };
                    _dbPoly.SrcnIkFirmas.Add(yeniKayit);
                    _dbPoly.SaveChanges();
                    TempDataOlustur("Kayıt işlemi yapılmıştır", false);
                }

            }
            else
            {
                var editItem = _dbPoly.SrcnIkFirmas.Find(firma.IkFirmaId);
                editItem.IkFirmaAdi = firma.IkFirmaAdi;
                editItem.Aciklama = firma.Aciklama;
                _dbPoly.SaveChanges();
                TempDataOlustur("Güncelleme işlemi yapılmıştır", false);

            }

            return RedirectToAction("FirmaTanimListesi");
        }
        public ActionResult FirmaSil(int id)
        {
            if (_dbPoly.SrcnIkEgitimFirmas.Any(a => a.IkFirmaId == id))
            {
                TempDataOlustur(
                    "Bu eğitimi almış personel ya da eğitimi verebilecek firma bağlantıları bulunduğu için silme işlemi yapılamaz",
                    false);
            }
            else
            {
                var item = _dbPoly.SrcnIkFirmas.Find(id);
                _dbPoly.SrcnIkFirmas.Remove(item);
                _dbPoly.SaveChanges();
                TempDataOlustur("Silme İşlemi Yapılmıştır", true);
            }

            return RedirectToAction("FirmaTanimListesi");
        }

        #endregion

        #region Egitim-Firma Crud

        public ActionResult EgitimFirmaTanimListesi()
        {
            var model = IkModeliBaseOlustur();
            model.IkEgitimFirmalar = _dbPoly.SrcnIkEgitimFirmas.OrderBy(a => a.IkEgitimTipiAdi)
                .ThenBy(a => a.IkEgitimAdi).ThenBy(a => a.IkFirmaAdi).ToList();
            return View(model);
        }

        public ActionResult EgitimFirmaTanimEkle()
        {
            var model = IkModeliBaseOlustur();
            var egitimler = _dbPoly.SrcnIkEgitims.AsNoTracking().OrderBy(a => a.IkEgitimTipiAdi)
                .ThenBy(a => a.IkEgitimAdi).ToList();
            var firmalar = _dbPoly.SrcnIkFirmas.AsNoTracking().OrderBy(a => a.IkFirmaAdi).ToList();
            foreach (var egitim in egitimler)
            {
                model.OzelDropMenuler.Add(new DropItem()
                {
                    Tanim = string.Format("{0} - {1}", egitim.IkEgitimTipiAdi, egitim.IkEgitimAdi),
                    IdInt = egitim.IkEgitimId
                });
            }

            model.IkEgitimFirmalar = _dbPoly.SrcnIkEgitimFirmas.OrderBy(a => a.IkEgitimTipiAdi)
                .ThenBy(a => a.IkEgitimAdi).ThenBy(a => a.IkFirmaAdi).ToList();
            model.DropEgitimler = new SelectList(model.OzelDropMenuler, "IdInt", "Tanim");
            model.DropFirmalar = new SelectList(firmalar, "IkFirmaId", "IkFirmaAdi");
            return View(model);
        }

        [HttpPost]
        public ActionResult EgitimFirmaTanimEkle(IkModel model)
        {
            var egitimFirma = model.IkEgitimFirma;
            bool SorunVarMi = false;
            if (egitimFirma.IkEgitimId == 0 || egitimFirma.IkFirmaId == 0)
            {
                TempDataOlustur("Lütfen eğitim ve firma eşleşmesi yapınız", false);
                SorunVarMi = true;

            }
            else
            {
                var egitim = _dbPoly.SrcnIkEgitims.Find(egitimFirma.IkEgitimId);
                var firma = _dbPoly.SrcnIkFirmas.Find(egitimFirma.IkFirmaId);
                if (_dbPoly.SrcnIkEgitimFirmas.Count(a => a.IkEgitimId == egitimFirma.IkEgitimId && a.IkFirmaId == egitimFirma.IkFirmaId) == 0)
                {
                    var yeniKayit = new SrcnIkEgitimFirmas()
                    {
                        IkFirmaAdi = firma.IkFirmaAdi,
                        IkFirmaId = firma.IkFirmaId,
                        IkEgitimId = egitim.IkEgitimId,
                        Aciklama = egitimFirma.Aciklama,
                        IkEgitimTipiAdi = egitim.IkEgitimTipiAdi,
                        IkEgitimAdi = egitim.IkEgitimAdi,
                        IkEgitimTipi = egitim.IkEgitimTipi
                    };
                    _dbPoly.SrcnIkEgitimFirmas.Add(yeniKayit);
                    _dbPoly.SaveChanges();
                    TempDataCRUDOlustur(1);
                }
                else
                {
                    TempDataOlustur(string.Format("Seçtiğiniz eğitm-firma eşleşmesi mevcuttur. {0} - {1}", egitim.IkEgitimAdi, firma.IkFirmaAdi), false);
                    SorunVarMi = true;
                }
            }

            if (SorunVarMi)
            {
                return RedirectToAction("EgitimFirmaTanimEkle");
            }
            else
            {
                return RedirectToAction("EgitimFirmaTanimListesi");
            }


        }

        public ActionResult EgitimFirmaTanimDuzenle(int id)
        {
            var model = IkModeliBaseOlustur();
            model.IkEgitimFirma = _dbPoly.SrcnIkEgitimFirmas.Find(id);
            var egitimler = _dbPoly.SrcnIkEgitims.AsNoTracking().OrderBy(a => a.IkEgitimTipiAdi)
                .ThenBy(a => a.IkEgitimAdi).ToList();
            var firmalar = _dbPoly.SrcnIkFirmas.AsNoTracking().OrderBy(a => a.IkFirmaAdi).ToList();
            foreach (var egitim in egitimler)
            {
                model.OzelDropMenuler.Add(new DropItem()
                {
                    Tanim = string.Format("{0} - {1}", egitim.IkEgitimTipiAdi, egitim.IkEgitimAdi),
                    IdInt = egitim.IkEgitimId
                });
            }

            model.IkEgitimFirmalar = _dbPoly.SrcnIkEgitimFirmas.OrderBy(a => a.IkEgitimTipiAdi)
                .ThenBy(a => a.IkEgitimAdi).ThenBy(a => a.IkFirmaAdi).ToList();
            model.DropEgitimler = new SelectList(model.OzelDropMenuler, "IdInt", "Tanim");
            model.DropFirmalar = new SelectList(firmalar, "IkFirmaId", "IkFirmaAdi");
            return View(model);
        }

        [HttpPost]
        public ActionResult EgitimFirmaTanimDuzenle(IkModel model)
        {
            var egitimFirma = model.IkEgitimFirma;
            bool SorunVarMi = false;
            if (egitimFirma.IkEgitimId == 0 || egitimFirma.IkFirmaId == 0)
            {
                TempDataOlustur("Lütfen eğitim ve firma eşleşmesini yeniden yapınız", false);
                SorunVarMi = true;
            }
            else
            {
                var egitim = _dbPoly.SrcnIkEgitims.Find(egitimFirma.IkEgitimId);
                var firma = _dbPoly.SrcnIkFirmas.Find(egitimFirma.IkFirmaId);
                if (_dbPoly.SrcnIkEgitimFirmas.Count(a =>
                        a.IkEgitimId == egitimFirma.IkEgitimId && a.IkFirmaId == egitimFirma.IkFirmaId && a.IkEgitimFirmaId != egitimFirma.IkEgitimFirmaId) == 0)
                {
                    var editKayit = _dbPoly.SrcnIkEgitimFirmas.Find(egitimFirma.IkEgitimFirmaId);
                    editKayit.IkFirmaAdi = firma.IkFirmaAdi;
                    editKayit.IkFirmaId = firma.IkFirmaId;
                    editKayit.IkEgitimId = egitim.IkEgitimId;
                    editKayit.Aciklama = egitimFirma.Aciklama;
                    editKayit.IkEgitimTipiAdi = egitim.IkEgitimTipiAdi;
                    editKayit.IkEgitimAdi = egitim.IkEgitimAdi;
                    editKayit.IkEgitimTipi = egitim.IkEgitimTipi;
                    _dbPoly.SaveChanges();
                    TempDataCRUDOlustur(2);
                }
                else
                {
                    TempDataOlustur(string.Format("Seçtiğiniz eğitm-firma eşleşmesi mevcuttur. {0} - {1}", egitim.IkEgitimAdi, firma.IkFirmaAdi), false);
                    SorunVarMi = true;
                }
            }
            if (SorunVarMi)
            {
                return RedirectToAction("EgitimFirmaTanimDuzenle", "InsanKaynaklari", new { id = egitimFirma.IkEgitimFirmaId });
            }
            else
            {
                return RedirectToAction("EgitimFirmaTanimListesi");
            }


        }
        public ActionResult EgitimFirmaSil(int id)
        {

            if (_dbPoly.SrcnIkEgitimOturums.Any(a => a.IkEgitimFirmaId == id))
            {
                TempDataOlustur(
                    "Bu eğitimi almış personel ya da oturumlar  bulunduğu için silme işlemi yapılamaz",
                    false);
            }
            else
            {
                var item = _dbPoly.SrcnIkEgitimOturums.Find(id);
                _dbPoly.SrcnIkEgitimOturums.Remove(item);
                _dbPoly.SaveChanges();
                TempDataOlustur("Silme İşlemi Yapılmıştır", true);
            }
            return RedirectToAction("EgitimFirmaTanimListesi");
        }
        #endregion

        #region Eğitim Tipi Liste
        public ActionResult EgitimleriTipineGoreGetir(int id)
        {
            var model = IkModeliBaseOlustur();
            var egitimTipi = _dbPoly.SrcnIkEgitimTipis.Find(id);

            var EgitimIdler = _dbPoly.SrcnIkEgitimFirmas.Where(a => a.IkEgitimTipi == egitimTipi.IkEgitimTipi)
                .Select(a => a.IkEgitimId).Distinct().ToList();
            model.IkEgitimler = _dbPoly.SrcnIkEgitims.AsNoTracking().Where(a => EgitimIdler.Any(b => b == a.IkEgitimId))
                .OrderBy(a => a.IkEgitimAdi).ToList();
            model.EgitimTipi = egitimTipi;
            return View(model);
        }
        #endregion
        #region Eğitim oturum crud
        public ActionResult EgitimOturumlari(int id)
        {
            var model = IkModeliBaseOlustur();
            model.IkEgitim = _dbPoly.SrcnIkEgitims.Find(id);
            model.EgitimOturumlari = _dbPoly.SrcnIkEgitimOturums.AsNoTracking().Where(a => a.IkEgitimId == id)
                .OrderByDescending(a => a.OturumTarihiDateTime).ToList();
            var Idler = model.EgitimOturumlari.Select(a => a.EgitimOturumId).ToList();
            model.EgitimOturumSrcnKullanicilar = _dbPoly.SrcnIkEgitimOturumSrcnKullanicis
                .Where(a => Idler.Any(b => b == a.EgitimOturumId))
                .OrderBy(a => a.IsimSoyisim).ToList();
            return View(model);

        }
        public ActionResult EgitimOturumEkle(int id)
        {
            var model = IkModeliBaseOlustur();
            model.IkEgitim = _dbPoly.SrcnIkEgitims.Find(id);
            var egitimFirmalar = _dbPoly.SrcnIkEgitimFirmas.AsNoTracking().Where(a => a.IkEgitimId == id)
                .OrderBy(a => a.IkEgitimTipiAdi).ThenBy(a => a.IkFirmaAdi).ToList();

            foreach (var item in egitimFirmalar)
            {
                model.OzelDropMenuler.Add(new DropItem() { Tanim = string.Format("{0} - {1}", item.IkEgitimTipiAdi, item.IkFirmaAdi), IdInt = item.IkEgitimFirmaId });
            }

            var isimler = _dbPoly.SrcnKullanicis.AsNoTracking().Where(a => a.GizlemeDurumu == 0)
                .Select(a => a.IsimSoyisim).ToList();
            var Idler = _dbPoly.SrcnKullanicis.AsNoTracking().Where(a => a.GizlemeDurumu == 0)
                .Select(a => a.KullaniciId).ToList();
            var liste = new List<DropItem>();
            for (int i = 0; i < isimler.Count; i++)
            {
                liste.Add(new DropItem() { Tanim = isimler[i], IdInt = Idler[i] });
            }

            model.DropKullanicilar = new SelectList(liste.OrderBy(a => a.Tanim).ToList(), "IdInt", "Tanim");
            model.DropFirmalar = new SelectList(model.OzelDropMenuler.OrderBy(a => a.Tanim).ToList(), "IdInt", "Tanim");

            if (Session["EgtPersonel"] != null)
            {
                model.EgitimOturumSrcnKullanicilar = (List<SrcnIkEgitimOturumSrcnKullanicis>)Session["EgtPersonel"];
            }

            return View(model);


        }



        [HttpPost]
        public ActionResult EgitimOturumEkle(IkModel model)
        {
            var egitimOturum = model.EgitimOturum;
            var egitimPersoneller = model.EgitimOturumSrcnKullanicilar;
            if (egitimOturum.OturumTarihi == null || egitimOturum.OturumTarihiSaati == null || egitimOturum.OturumTarihiBitisi == null || egitimOturum.OturumTarihiBitisSaati == null || egitimOturum.IkEgitimFirmaId == 0 || egitimPersoneller.Count == 0)
            {
                TempDataOlustur("Lütfen tarih, firma ve personel seçimlerini yapınız", false);
                return RedirectToAction("EgitimOturumEkle", "InsanKaynaklari", new { id = model.IkEgitim.IkEgitimId });
            }

            var egitimFirma = _dbPoly.SrcnIkEgitimFirmas.Find(egitimOturum.IkEgitimFirmaId);
            var Idler = egitimPersoneller.Select(a => a.SrcnKullaniciId).ToList();
            var SecilenPersoneller = _dbPoly.SrcnKullanicis.Where(x => Idler.Any(b => b == x.KullaniciId))
                .Select(x => new
                {
                    x.KullaniciId,
                    x.IsimSoyisim
                }).ToList();
            var anaoturumDurum = _dbPoly.SrcnIkEgitimKatilimTanim.Find(1);
            var personelOturumDurum = _dbPoly.SrcnIkEgitimKatilimTanim.Find(4);
            var tarih = Convert.ToDateTime(egitimOturum.OturumTarihi);
            var bitistarih = Convert.ToDateTime(egitimOturum.OturumTarihiBitisi);
            var yeniOturum = new SrcnIkEgitimOturums()
            {
                Aciklama = egitimOturum.Aciklama,
                IkEgitimFirmaId = egitimFirma.IkEgitimFirmaId,
                IkEgitimId = egitimFirma.IkEgitimId,
                IkEgitimAdi = egitimFirma.IkEgitimAdi,
                IkFirmaAdi = egitimFirma.IkFirmaAdi,
                KayitTarihi = DateTime.Now,
                IkFirmaId = egitimFirma.IkFirmaId,
                OturumTarihi = tarih.ToShortDateString(),
                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                KatilimDurumAdi = anaoturumDurum.KatilimDurumAdi,
                OturumTarihiDateTime = tarih,
                EgitimKatilimTanimId = anaoturumDurum.EgitimKatilimTanimId,
                EgitimOturumAdi = egitimOturum.OturumTarihi,
                KayitYapanKulId = Kullanici.KullaniciId,
                OturumTarihiBitisi = bitistarih.ToShortDateString(),
                OturumTarihiBitisSaati = egitimOturum.OturumTarihiBitisSaati,
                OturumTarihiSaati = egitimOturum.OturumTarihiSaati,
                OturumTarihiBitisDateTime = bitistarih
            };
            yeniOturum.OturumSureAciklama = OturumSureAciklama(tarih, yeniOturum.OturumTarihiSaati, bitistarih,
                yeniOturum.OturumTarihiBitisSaati);

            _dbPoly.SrcnIkEgitimOturums.Add(yeniOturum);
            _dbPoly.SaveChanges();

            foreach (var item in SecilenPersoneller)
            {
                _dbPoly.SrcnIkEgitimOturumSrcnKullanicis.Add(new SrcnIkEgitimOturumSrcnKullanicis()
                {
                    IsimSoyisim = item.IsimSoyisim,
                    SrcnKullaniciId = item.KullaniciId,
                    EgitimKatilimTanimId = personelOturumDurum.EgitimKatilimTanimId,
                    EgitimOturumId = yeniOturum.EgitimOturumId,
                    KatilimDurumAdi = personelOturumDurum.KatilimDurumAdi,
                    EgitimOturumAdi = yeniOturum.EgitimOturumAdi

                });
                _dbPoly.SaveChanges();
            }

            Session["EgtPersonel"] = null;
            TempDataCRUDOlustur(1);
            return RedirectToAction("EgitimOturumlari", "InsanKaynaklari", new { id = yeniOturum.IkEgitimId });


        }

        public ActionResult EgitimOturumDuzenle(int id)
        {
            var model = IkModeliBaseOlustur();

            var oturum = _dbPoly.SrcnIkEgitimOturums.Find(id);
            var egitimPersoneller = _dbPoly.SrcnIkEgitimOturumSrcnKullanicis.Where(a => a.EgitimOturumId == id)
                .OrderBy(a => a.IsimSoyisim).ToList();

            var egitimFirmalar = _dbPoly.SrcnIkEgitimFirmas.AsNoTracking().Where(a => a.IkEgitimId == oturum.IkEgitimId)
                .OrderBy(a => a.IkEgitimTipiAdi).ThenBy(a => a.IkFirmaAdi).ToList();
            //var KatilimDurumlar = _dbPoly.SrcnIkEgitimKatilimTanim.OrderBy(a => a.KatilimDurumAdi).ToList();

            foreach (var item in egitimFirmalar)
            {
                model.OzelDropMenuler.Add(new DropItem() { Tanim = string.Format("{0} - {1}", item.IkEgitimTipiAdi, item.IkFirmaAdi), IdInt = item.IkEgitimFirmaId });
            }

            var isimler = _dbPoly.SrcnKullanicis.AsNoTracking().Where(a => a.GizlemeDurumu == 0)
                .Select(a => a.IsimSoyisim).ToList();
            var Idler = _dbPoly.SrcnKullanicis.AsNoTracking().Where(a => a.GizlemeDurumu == 0)
                .Select(a => a.KullaniciId).ToList();
            var liste = new List<DropItem>();
            for (int i = 0; i < isimler.Count; i++)
            {
                liste.Add(new DropItem() { Tanim = isimler[i], IdInt = Idler[i] });
            }

            model.DropKullanicilar = new SelectList(liste.OrderBy(a => a.Tanim).ToList(), "IdInt", "Tanim");
            model.DropFirmalar = new SelectList(model.OzelDropMenuler.OrderBy(a => a.Tanim).ToList(), "IdInt", "Tanim");
            model.EgitimOturum = oturum;
            model.EgitimOturumSrcnKullanicilar = egitimPersoneller;
            //model.EgitimKatilimlari = KatilimDurumlar;
            return View(model);
        }
        [HttpPost]
        public ActionResult EgitimOturumDuzenle(IkModel model)
        {
            var egitimOturum = model.EgitimOturum;
            var egitimPersoneller = model.EgitimOturumSrcnKullanicilar;
            if (egitimOturum.OturumTarihi == null || egitimOturum.OturumTarihiSaati == null || egitimOturum.OturumTarihiBitisi == null || egitimOturum.OturumTarihiBitisSaati == null || egitimOturum.IkEgitimFirmaId == 0 || egitimPersoneller.Count == 0)
            {
                TempDataOlustur("Lütfen tarih, firma ve personel seçimlerini yapınız", false);
            }
            else
            {
                var egitimFirma = _dbPoly.SrcnIkEgitimFirmas.Find(egitimOturum.IkEgitimFirmaId);
                var bitistarih = Convert.ToDateTime(egitimOturum.OturumTarihiBitisi);
                //var anaoturumDurum = _dbPoly.SrcnIkEgitimKatilimTanim.Find(egitimOturum.EgitimKatilimTanimId);

                var tarih = Convert.ToDateTime(egitimOturum.OturumTarihi);
                var editOturum = _dbPoly.SrcnIkEgitimOturums.Find(egitimOturum.EgitimOturumId);
                editOturum.Aciklama = egitimOturum.Aciklama;
                editOturum.IkEgitimFirmaId = egitimFirma.IkEgitimFirmaId;
                editOturum.IkEgitimId = egitimFirma.IkEgitimId;
                editOturum.IkEgitimAdi = egitimFirma.IkEgitimAdi;
                editOturum.IkFirmaAdi = egitimFirma.IkFirmaAdi;

                editOturum.IkFirmaId = egitimFirma.IkFirmaId;
                editOturum.OturumTarihi = tarih.ToShortDateString();

                //editOturum.KatilimDurumAdi = anaoturumDurum.KatilimDurumAdi;
                editOturum.OturumTarihiDateTime = tarih;
                //editOturum.EgitimKatilimTanimId = anaoturumDurum.EgitimKatilimTanimId;
                editOturum.EgitimOturumAdi = egitimOturum.OturumTarihi;
                editOturum.OturumTarihiBitisi = bitistarih.ToShortDateString();
                editOturum.OturumTarihiBitisSaati = egitimOturum.OturumTarihiBitisSaati;
                editOturum.OturumTarihiSaati = egitimOturum.OturumTarihiSaati;
                editOturum.OturumTarihiBitisDateTime = bitistarih;

                editOturum.OturumSureAciklama = OturumSureAciklama(tarih, editOturum.OturumTarihiSaati, bitistarih,
                    editOturum.OturumTarihiBitisSaati);

                _dbPoly.SaveChanges();
                //foreach (var egtPers in egitimPersoneller)
                //{
                //    var item = _dbPoly.SrcnIkEgitimOturumSrcnKullanicis.Find(egtPers.EgitimOturumSrcnKullaniciId);
                //    if (item.EgitimKatilimTanimId != egtPers.EgitimKatilimTanimId)
                //    {
                //        var personelOturumDurum = _dbPoly.SrcnIkEgitimKatilimTanim.Find(egtPers.EgitimKatilimTanimId);
                //        item.EgitimKatilimTanimId = personelOturumDurum.EgitimKatilimTanimId;
                //        item.KatilimDurumAdi = personelOturumDurum.KatilimDurumAdi;
                //        _dbPoly.SaveChanges();
                //    }
                //}

                TempDataCRUDOlustur(2);

            }


            return RedirectToAction("EgitimOturumDuzenle", "InsanKaynaklari", new { id = egitimOturum.EgitimOturumId });


        }
        public ActionResult EgitimOturumaPersonelEkle(int id, int id2)
        {
            var personel = _dbPoly.SrcnKullanicis.Find(id2);
            var oturum = _dbPoly.SrcnIkEgitimOturums.Find(id);
            //var durum = _dbPoly.SrcnIkEgitimKatilimTanim.Find(4);
            _dbPoly.SrcnIkEgitimOturumSrcnKullanicis.Add(new SrcnIkEgitimOturumSrcnKullanicis()
            {
                IsimSoyisim = personel.IsimSoyisim,
                SrcnKullaniciId = personel.KullaniciId,
                //EgitimKatilimTanimId = durum.EgitimKatilimTanimId,
                EgitimOturumId = oturum.EgitimOturumId,
                //KatilimDurumAdi = durum.KatilimDurumAdi,
                EgitimOturumAdi = oturum.EgitimOturumAdi

            });
            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(1);
            return RedirectToAction("EgitimOturumDuzenle", "InsanKaynaklari", new { id = id });
        }

        public ActionResult EgitimOturumdanPersonelSil(int id)
        {
            var item = _dbPoly.SrcnIkEgitimOturumSrcnKullanicis.Find(id);
            int idd = item.EgitimOturumId;
            _dbPoly.SrcnIkEgitimOturumSrcnKullanicis.Remove(item);
            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(3);
            return RedirectToAction("EgitimOturumDuzenle", "InsanKaynaklari", new { id = idd });
        }
        public ActionResult PersoneliSessionaEkle(int id)
        {
            var model = new IkModel();
            var EgitimPersoneller = new List<SrcnIkEgitimOturumSrcnKullanicis>();

            if (Session["EgtPersonel"] != null)
            {
                EgitimPersoneller = (List<SrcnIkEgitimOturumSrcnKullanicis>)Session["EgtPersonel"];
            }

            if (EgitimPersoneller.Count(a => a.SrcnKullaniciId == id) == 0)
            {
                var kul = _dbPoly.SrcnKullanicis.Find(id);
                EgitimPersoneller.Add(new SrcnIkEgitimOturumSrcnKullanicis()
                {
                    IsimSoyisim = kul.IsimSoyisim,
                    SrcnKullaniciId = kul.KullaniciId
                });
            }

            Session["EgtPersonel"] = EgitimPersoneller;
            model.EgitimOturumSrcnKullanicilar = EgitimPersoneller.OrderBy(a => a.IsimSoyisim).ToList();
            return PartialView("~/Views/InsanKaynaklari/_EgitimOturumKatilimciListe.cshtml", model);

        }
        public ActionResult PersoneliSessiondanKaldir(int id)
        {
            var model = new IkModel();
            var EgitimPersoneller = new List<SrcnIkEgitimOturumSrcnKullanicis>();

            if (Session["EgtPersonel"] != null)
            {
                EgitimPersoneller = (List<SrcnIkEgitimOturumSrcnKullanicis>)Session["EgtPersonel"];
            }

            EgitimPersoneller = EgitimPersoneller.Where(a => a.SrcnKullaniciId != id).ToList();

            Session["EgtPersonel"] = EgitimPersoneller;
            model.EgitimOturumSrcnKullanicilar = EgitimPersoneller.OrderBy(a => a.IsimSoyisim).ToList();
            return PartialView("~/Views/InsanKaynaklari/_EgitimOturumKatilimciListe.cshtml", model);

        }




        #endregion

        #region Personel Sicil
        public ActionResult PersonelSicilAra()
        {
            var model = IkModeliBaseOlustur();

            return View(model);
        }


        public ActionResult PersonelSicilOzeti(int id)
        {
            var model = IkModeliBaseOlustur();

            model.SrcnKullanici = _dbPoly.SrcnKullanicis.Find(id);
            model.PersonelSicilOzetItemlar = PersonelSicilOzetItemlarOlustur(id);
            return View(model);
        }
        public ActionResult PdfPersonelSicilOzeti(int id)
        {

            return PdfPersonelSicilOlustur(id);
        }

        #endregion
        #region Eğitim Genelleme-Filtreleme
        public ActionResult PersonelEgitimGenel()
        {
            TempData["Gizle"] = "gizle";




            var result = (from c in _dbPoly.SrcnIkEgitimOturums
                          select new GridTablesIK
                          {

                              IkFirmaAdi = c.IkFirmaAdi,
                              OturumTarihi = c.OturumTarihi,
                              OturumSureAciklama = c.OturumSureAciklama,
                              IkEgitimAdi = c.IkEgitimAdi,
                              KatilimDurumAdi = c.KatilimDurumAdi,


                          }).ToList();

            return View(_dbPoly.SrcnIkEgitimOturums.OrderBy(a => a.EgitimOturumId).ToList().ToPagedList(1, 999));
        }
        public ActionResult PersonelEgitimGenelDetay(int id)
        {
            SrcnIkEgitimOturums aaa = _dbPoly.SrcnIkEgitimOturums.Find(id);
            return View(aaa);
        }
        public ActionResult Report(string id)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "Report2.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<SrcnIkEgitimOturums> cm = new List<SrcnIkEgitimOturums>();
            using (POTA_PTKSEntities dc = new POTA_PTKSEntities())
            {
                cm = dc.SrcnIkEgitimOturums.ToList();
            }
            ReportDataSource rd = new ReportDataSource("IKDataSet", cm);
            lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>20in</PageWidth>" +
            "  <PageHeight>25in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        #endregion

    }
}
