using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.TefrikBildirim.Models;


namespace Polyteks.Katman.Has.Controllers
{
    public class YardimciTesislerController : BaseController
    {
        // GET: YardimciTesisler
        #region Metotlar
        public YardimciTesisModel YardimciTesisLabAnalizSonuciGetir(int LabAnalizId)
        {
            var model = new YardimciTesisModel();
            model.Kullanici = Kullanici;
            model.LabAnaliz = _dbPoly.SrcnLabAnalizs.Find(LabAnalizId);
            model.AnalizHareketleri = _dbPoly.SrcnLabAnalizHarekets.Where(a => a.LabAnalizId == LabAnalizId)
                .OrderByDescending(a => a.KayitTarihi).ToList();
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
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(model.LabAnaliz.LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            return model;
        }

        public YardimciTesisModel YardimciTesisModeliGetir(int LabGrupId)
        {
            var model = new YardimciTesisModel();
            model.Kullanici = Kullanici;
            var labGrup = _dbPoly.SrcnLabGrups.Find(LabGrupId);
            model.LabGrup = labGrup;
            model.LabAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.OrderBy(a => a.LabAnalizCesitAdi).ToList();
            model.LabGrubuAnalizleri = _dbPoly.SrcnLabGrupAnalizCesits.Where(a => a.LabGrupId == LabGrupId).ToList();

            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(LabGrupId);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            var YardimciTesisKontNoktKategoriler = _dbPoly.SrcnYardimciTesisKontrolNoktas.Select(a => a.Kategori).Distinct().OrderBy(a => a).ToList();
            foreach (var k in YardimciTesisKontNoktKategoriler)
            {
                model.YardimciTesisKategoriModeller.Add(new YardimciTesisKategoriModel()
                {
                    Kategori = k,
                    YardimciTesisKontrolNoktalari = _dbPoly.SrcnYardimciTesisKontrolNoktas.Where(a => a.Kategori == k).OrderBy(a => a.YardimciTesisKontrolNoktaId).ToList()

                });
            }

            return model;
        }
        #endregion
        public ActionResult YardimciTesisLabAnalizleri(int? id)
        {
            TempData["Gizle"] = "aa";
            var model = new YardimciTesisModel();
            var anaGrup = _dbPoly.SrcnLabGrups.First(a => a.UstLabGrupId == 0 && a.AnalizTipi == 4);
            model.SrcnLabGruplari = _dbPoly.SrcnLabGrups.Where(a => a.UstLabGrupId == anaGrup.LabGrupId)
                .OrderBy(a => a.LabGrupAdi).ToList();
            if (id != null && id != 0)
            {
                model.LabAnalizleri =
                    _dbPoly.SrcnLabAnalizs.Where(a => a.LabGrupId == id).OrderByDescending(a => a.KayitTarihi).ToList();
                model.LabGrup = _dbPoly.SrcnLabGrups.Find(id);
            }

            return View(model);
        }

        public ActionResult YardimciTesisAnalizIstegi(int id)
        {

            bool sorunVarMi = false;

            if (_dbPoly.SrcnLabGrups.Any(a => a.LabGrupId == id))
            {
                var labgrup = _dbPoly.SrcnLabGrups.First(a => a.LabGrupId == id);
                if (labgrup.AnalizTipi != 4)
                {
                    sorunVarMi = true;
                }
            }
            else
            {
                sorunVarMi = true;
            }

            if (sorunVarMi)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var model = YardimciTesisModeliGetir(id);
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult YardimciTesisAnalizIstegi(YardimciTesisModel model)
        {
            TempData["Gizle"] = "gizle";

            MailMessage mailim = new MailMessage();
            //mailim.To.Add("NBUTUN@polyteks.com.tr");
            //mailim.To.Add("SKARASU@polyteks.com.tr");
            //mailim.To.Add("muzun@polyteks.com.tr");
            //mailim.To.Add("LTEKNISYENI@polyteks.com.tr");
            mailim.To.Add("oogan@tasdelengroup.com");
            mailim.To.Add("skarasu@tasdelengroup.com");
            mailim.To.Add("ytbteknisyeni@tasdelengroup.com");
            mailim.To.Add("labteknisyeni@tasdelengroup.com");
            mailim.CC.Add("mdikici@tasdelengroup.com");

            mailim.From = new MailAddress("pmailsystem1@tasdelengroup.com");
            //mailim.Subject = "deneme hehehe" + employee.Makine_No;
            mailim.Subject = "YARDIMCI TESİSLER SU ANALİZİ HK.";
            //var body = "<p>POY IKAZ BILDIRIMI YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p>" + deneme.PartiNo;
            //mailim.Body = body;
            mailim.Body = "<b><p>YARDIMCI TESİSLER SU ANALİZİ İÇİN BEKLEMEKTEDİR.LÜTFEN SU ANALİZİ SONUÇLARINI GİRİNİZ.</p></b>";

            mailim.IsBodyHtml = true;
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

  
            var labgrup = _dbPoly.SrcnLabGrups.Find(model.LabGrup.LabGrupId);
            var istekTarihi = Convert.ToDateTime(model.IstekTarihi);
            var labAnaliz = model.LabAnaliz;
           
            if (model.YardimciTesisKategoriModeller.Count(a => a.YardimciTesisKontrolNoktalari.Any(b => b.SeciliMi)) == 0)
            {
                TempDataOlustur("Analiz Yapılacak Kontrol Noktalarını Seçmediniz", false);
                return RedirectToAction("YardimciTesisAnalizIstegi", "YardimciTesisler", new { id = labgrup.LabGrupId });
            }
            else
            {
                var YardimciTesisSecilenKontrolNoktalari = new List<SrcnYardimciTesisKontrolNoktas>();
                foreach (var itt in model.YardimciTesisKategoriModeller.ToList())
                {
                    foreach (var item in itt.YardimciTesisKontrolNoktalari.Where(a => a.SeciliMi).ToList())
                    {
                        var kontrolNoktasi = _dbPoly.SrcnYardimciTesisKontrolNoktas.Find(item.YardimciTesisKontrolNoktaId);
                        YardimciTesisSecilenKontrolNoktalari.Add(kontrolNoktasi);
                    }
                }
                var LabGrubuAnalizleri = _dbPoly.SrcnLabGrupAnalizCesits.Where(a => a.LabGrupId == labgrup.LabGrupId).ToList();
                var labAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(1);
                var yeniAnaliz = new SrcnLabAnalizs()
                {
                    KayitTarihi = DateTime.Now,
                    Aciklama = labAnaliz.Aciklama,
                    AnalizTipi = labgrup.AnalizTipi,
                    LabGrupAdi = labgrup.LabGrupAdi,
                    LabGrupId = labgrup.LabGrupId,
                    LabAnalizDurumu = labAnalizDurumu.LabAnalizDurumu,
                    LabAnalizDurumuAdi = labAnalizDurumu.LabAnalizDurumuAdi,
                    IstenenTerminTarihi = Convert.ToDateTime(model.IstekTarihi),
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),

                    AnaliziYapilanBilesenSayisi = YardimciTesisSecilenKontrolNoktalari.Count,
                    AnalizYapilacakBobinSayisi = 0,
                    AnalizYapilacakKumasSayisi = 0,

                    CariAdi = "",
                    CariNo = "",
                    FirmaTipi = 0,
                    GerceklesenTerminTarihi = null,
                    ImalatAnalizYapilmaTipi = 0,
                    IsEmriNumarasiDurumu = 0,
                    LabAciklama = null,
                    MakineId = 0,
                    PartiNo = null,
                    PlanlananTerminTarihi = null,
                    RefakartKartNo = null,
                    TakimSaati = null
                };
                _dbPoly.SrcnLabAnalizs.Add(yeniAnaliz);
                _dbPoly.SaveChanges();
                _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
                {
                    KayitTarihi = DateTime.Now,
                    HareketAdi = string.Format("{0} - {1}", labAnalizDurumu.LabAnalizDurumuAdi, Kullanici.IsimSoyisim),
                    LabAnalizId = yeniAnaliz.LabAnalizId
                });
                _dbPoly.SaveChanges();
                foreach (var item in YardimciTesisSecilenKontrolNoktalari.OrderBy(a => a.YardimciTesisKontrolNoktaAdi))
                {
                    var analizItemKayit = new SrcnLabAnalizItems()
                    {
                        KayitTarihi = DateTime.Now,
                        LabGrupAdi = labgrup.LabGrupAdi,
                        LabGrupId = labgrup.LabGrupId,
                        LabAnalizId = yeniAnaliz.LabAnalizId,
                        MakinePozisyonNo = 0,
                        SonucTarihi = null,
                        YardimciTesisKontrolNoktaAdi = item.YardimciTesisKontrolNoktaAdi,
                        YardimciTesisKontrolNoktaId = item.YardimciTesisKontrolNoktaId
                    };
                    _dbPoly.SrcnLabAnalizItems.Add(analizItemKayit);
                    _dbPoly.SaveChanges();

                    foreach (var labgrupAnalizCesit in LabGrubuAnalizleri)
                    {
                        var analizItemCesit = new SrcnLabAnalizItemAnalizCesitSonucs()
                        {
                            YardimciTesisKontrolNoktaAdi = item.YardimciTesisKontrolNoktaAdi,
                            YardimciTesisKontrolNoktaId = item.YardimciTesisKontrolNoktaId,
                            MakinePozisyonNo = 0,
                            AnalizDegeri = 0,
                            LabAnalizCesitAdi = labgrupAnalizCesit.LabAnalizCesitAdi,
                            LabAnalizCesitId = labgrupAnalizCesit.LabGrupCesitId,
                            LabAnalizItemId = analizItemKayit.LabAnalizItemId
                        };
                        _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Add(analizItemCesit);
                        _dbPoly.SaveChanges();
                    }


                }
                TempDataOlustur("Analiz Talebiniz Oluşturulmuştur", true);
            }

            return RedirectToAction("YardimciTesisLabAnalizleri", "YardimciTesisler", new { id = labgrup.LabGrupId });
        }

        public ActionResult YardimciTesisAnalizSonucDetay(int id)
        {
            var model = YardimciTesisLabAnalizSonuciGetir(id);
            return View(model);
        }
        public ActionResult YardimciTesisAnalizTalebiKaldir(int id)
        {
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            var labGrupId = labAnaliz.LabGrupId;
            if (labAnaliz.LabAnalizDurumu == 1 && labAnaliz.KayitYapanKulKodu == Kullanici.KullaniciId.ToString())
            {
                LabAnalizVeritabanındanSil(id);
                TempDataOlustur("Silme İşlemi Yapılmıştır", true);
            }
            else
            {
                TempDataOlustur("Silme İşlemi Yapılamaz", false);
            }

            return RedirectToAction("YardimciTesisLabAnalizleri", "YardimciTesisler", new { id = labGrupId });
        }
    }
}