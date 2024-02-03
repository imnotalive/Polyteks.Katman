using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Has.Controllers;
using Polyteks.Katman.TefrikBildirim.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Controllers
{
    public class SevkiyatController : BaseController
    {

        // GET: Sevkiyat
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
        #region SEPET İŞLEMLERİ
        public ActionResult TotalCount(int? id) // id: MalzemeAnaTipi
        { 
            try
            {
                int count = 0;
                Mrv_MalzemeAna fis = _dbPoly.Mrv_MalzemeAna.FirstOrDefault(x => x.TamamlandiMi == false && x.Tip == id && x.Personel == Kullanici.IsimSoyisim);
                if(fis != null)
                    count = fis.Mrv_MalzemeDetay.Count;
                ViewBag.Count = count;
            }
            catch (Exception)
            {
                ViewBag.Count = 0;
            }
            return PartialView();
        }
        public ActionResult Sepet(int? id) // id: MalzemeAnaTipi
        {

            var kullaniciadi = Kullanici.IsimSoyisim;
            var model = _dbPoly.SrcnKullanicis.FirstOrDefault(x => x.IsimSoyisim == kullaniciadi);
            ViewData["SrcnKullanicis"] = model;
            Mrv_MalzemeAna fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.TamamlandiMi == false && x.Tip == id && x.Personel == Kullanici.IsimSoyisim).OrderByDescending(x => x.Tarih).FirstOrDefault();
            if (fisAna == null)
            {
                fisAna = new Mrv_MalzemeAna();
                //fisAna.Mrv_MalzemeDetay = new List<Mrv_MalzemeDetay>();
            }
            return View(fisAna);
        }
        [HttpPost]
        public ActionResult SepetOnayi(int? id) // id: MalzemeAnaTipi
        {
            var kullaniciadi = Kullanici.IsimSoyisim;
            var model = _dbPoly.SrcnKullanicis.FirstOrDefault(x => x.IsimSoyisim == kullaniciadi);
            ViewData["SrcnKullanicis"] = model;
            Mrv_MalzemeAna fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.TamamlandiMi == false && x.Tip == id && x.Personel == Kullanici.IsimSoyisim).FirstOrDefault();
            if (fisAna == null)
                return HttpNotFound();
            fisAna.TamamlandiMi = true;
            MailMessage mailim = new MailMessage();
            if (fisAna.Tip == 1)//mamül
            {
                if (fisAna.Bolum == "FANTAZİ/BÜKÜM")
                {
                    mailim.To.Add("sevkiyat@polyteks.com.tr");
                    mailim.To.Add("pazarlama@polyteks.com.tr");
                    mailim.To.Add("Bilgislem@polyteks.com.tr");
                    mailim.To.Add("FTeknisyeni@polyteks.com.tr");
                    mailim.To.Add("YTOPHISAR@polyteks.com.tr");
                    mailim.To.Add("ACORAPCI@polyteks.com.tr");
                    mailim.CC.Add("scelik@polyteks.com.tr");
                    mailim.CC.Add("denetim@polyteks.com.tr");
                }
                if (fisAna.Bolum == "TEKSTÜRE")
                {
                    mailim.To.Add("sevkiyat@polyteks.com.tr");
                    mailim.To.Add("pazarlama@polyteks.com.tr");
                    mailim.To.Add("Bilgislem@polyteks.com.tr");
                    mailim.To.Add("teksturebolumu@polyteks.com.tr");
                  
                    mailim.To.Add("ACORAPCI@polyteks.com.tr");

                    mailim.CC.Add("scelik@polyteks.com.tr");
                    mailim.CC.Add("denetim@polyteks.com.tr");
                }
                if (fisAna.Bolum == "TEKNİK TEKSTİL")
                {
                    mailim.To.Add("sevkiyat@polyteks.com.tr");
                    mailim.To.Add("pazarlama@polyteks.com.tr");
                    mailim.To.Add("Bilgislem@polyteks.com.tr");
                 
                    mailim.To.Add("MTUTUK@polyteks.com.tr");

                    mailim.CC.Add("scelik@polyteks.com.tr");
                  
                }


                mailim.From = new MailAddress("pota_bilgi@polyteks.com.tr");
                mailim.Subject = "MALZEME TALEBİ HK.";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<b><p>MAMÜL AMBARINDAN MALZEME TALEBİ YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p></b>");
                sb.AppendLine(fisAna.MalzemeNo + ":&nbsp &nbsp Talep Numarasıdır.<br/>");
                sb.AppendLine(fisAna.Tarih + ":&nbsp &nbsp Tarihinde Talep Edilmiştir.<br/>");
                sb.AppendLine(fisAna.Bolum + ":&nbsp &nbsp Bölümü Talep Etmiştir.<br/><br/>");

             
                sb.AppendLine("<u><b>TALEP EDİLEN ÜRÜNLER:</b></u><br/><br/><br/><table style='border: 1px solid black'>");
                sb.AppendLine("<thead style='border: 1px solid black' style='background - color: gray'><tr style='background - color: gray'style='border: 1px solid black'> <br/> &nbsp; <th style='border: 1px solid black'>STOK ADI</th>  &nbsp; <th style='border: 1px solid black'>PARTİ NUMARASI</th> &nbsp; <th style='border: 1px solid black'>KALİTE</th> &nbsp; <th style='border: 1px solid black'>FİRMA</th> &nbsp; <th style='border: 1px solid black'>MİKTAR</th> &nbsp; <th style='border: 1px solid black'>AÇIKLAMA</th> </tr></thead>");
                foreach (var item in fisAna.Mrv_MalzemeDetay)
                {
                    //sb.AppendLine($"<li>{item.StokKodu} - {item.StokAdi} - {item.Parti} - {item.Miktar}</li>");
                    sb.AppendLine($"<tbody style='border: 1px solid black'><tr style='border: 1px solid black'> <td style='border: 1px solid black'> {item.StokAdi}</td> <td style='border: 1px solid black'><center>{item.Parti}</center></td> <td style='border: 1px solid black'><center>{item.Kalite}</center></td> <td style='border: 1px solid black'><center>{item.Firma}</center></td> <td style='border: 1px solid black'> {item.Miktar}</td> <td style='border: 1px solid black'> {item.Aciklama}</td> </tr></tbody>");

                }
                sb.AppendLine("</table>");
                mailim.Body = sb.ToString();
                //foreach (var item in fisAna.Mrv_MalzemeDetay)
                //{
                //    sb.AppendLine($"<li>{item.StokAdi} - {item.Parti} - {item.Miktar}</li>");
                //}
                //sb.AppendLine("</ul>");
                //mailim.Body = sb.ToString();
                //+fis.StokAdi + ": &nbsp &nbsp Stok Adı <br/>"
                //  + fis.StokKodu + ":&nbsp &nbsp Stok Kodu <br/>"
                //    + fis.Parti + ":&nbsp &nbsp Parti Numarası <br/>"

                //  + fis.Miktar + ":&nbsp &nbsp Miktarda talepte bulunulmuştur.<br/>";
            }
            else if (fisAna.Tip == 2)//hammadde
            {
                mailim.To.Add("EDOGAN@polyteks.com.tr");
                mailim.To.Add("FYUKSEL@polyteks.com.tr");
                mailim.To.Add("MDIKICI@polyteks.com.tr");
         
                mailim.CC.Add("scelik@polyteks.com.tr");
                mailim.CC.Add("denetim@polyteks.com.tr");
                mailim.From = new MailAddress("pota_bilgi@polyteks.com.tr");
                mailim.Subject = "MALZEME TALEBİ HK.";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<b><p>HAMMADDE AMBARINDAN MALZEME TALEBİ YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p></b>");
                sb.AppendLine(fisAna.MalzemeNo + ":&nbsp &nbsp Talep Numarasıdır.<br/>");
                sb.AppendLine(fisAna.Tarih + ":&nbsp &nbsp Tarihinde Talep Edilmiştir.<br/>");
                sb.AppendLine(fisAna.AmbarAdi + ":&nbsp &nbsp Ambarından İstekte Bulunulmuştur.<br/>");
                sb.AppendLine(fisAna.Bolum + ":&nbsp &nbsp Bölümü Talep Etmiştir.<br/><br/>");

                sb.AppendLine("<u><b>TALEP EDİLEN ÜRÜNLER:</b></u><br/><br/><br/><table style='border: 1px solid black'>");
                sb.AppendLine("<thead style='border: 1px solid black' style='background - color: gray'><tr style='background - color: gray'style='border: 1px solid black'> <br/> &nbsp; <th style='border: 1px solid black'>STOK KODU</th>  &nbsp; <th style='border: 1px solid black'>STOK ADI</th> &nbsp; <th style='border: 1px solid black'>PARTİ NUMARASI</th>  &nbsp; <th style='border: 1px solid black'>MİKTAR</th> &nbsp; <th style='border: 1px solid black'>CHİPS SİLOSU</th> &nbsp; <th style='border: 1px solid black'>AÇIKLAMA</th> </tr></thead>");
                foreach (var item in fisAna.Mrv_MalzemeDetay)
                {
                    //sb.AppendLine($"<li>{item.StokKodu} - {item.StokAdi} - {item.Parti} - {item.Miktar}</li>");
                    sb.AppendLine($"<tbody><tr style='border: 1px solid black'>  <td style='border: 1px solid black'> {item.StokKodu} </td> <td style='border: 1px solid black'> {item.StokAdi}</td> <td style='border: 1px solid black'><center>{item.Parti}</center></td> <td style='border: 1px solid black'> {item.Miktar}</td>  <td style='border: 1px solid black'> {item.ChipsSilosu}</td><td style='border: 1px solid black'> {item.Aciklama}</td> </tr></tbody>");

                }
                sb.AppendLine("</table>");
                mailim.Body = sb.ToString();
                //foreach (var item in fisAna.Mrv_MalzemeDetay)
                //{
                //    sb.AppendLine($"<li>{item.StokKodu} - {item.StokAdi} - {item.Parti} - {item.Miktar}</li>");
                //}
                //sb.AppendLine("</ul>");
                //mailim.Body = sb.ToString();
            }
            else if (fisAna.Tip == 3)//yedekparça
            {   mailim.To.Add("Bilgislem@polyteks.com.tr");
                mailim.To.Add("yedekparca@polyteks.com.tr");
                //mailim.To.Add("BUNER@polyteks.com.tr");
                mailim.CC.Add("scelik@polyteks.com.tr");
                mailim.CC.Add("denetim@polyteks.com.tr");
                //mailim.From = new MailAddress("pota_bilgi@polyteks.com.tr");
                mailim.Subject = "MALZEME TALEBİ HK.";
                //var body = "<p>POY IKAZ BILDIRIMI YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p>" + deneme.PartiNo;
                //mailim.Body = body;
                //mailim.Body = "<b><p>YEDEK PARÇA AMBARINDAN MALZEME TALEBİ YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p></b>"
                //    + fisAna.Tarih + ":&nbsp &nbsp Tarihinde talep edilmiştir.<br/>"
                //    + fisAna.AmbarAdi + ":&nbsp &nbsp Ambarından istekte bulunulmuştur.<br/>"
                //    + fisAna.Bolum + ":&nbsp &nbsp Bölümü Talep Etmiştir.<br/>";

                mailim.From = new MailAddress("pota_bilgi@polyteks.com.tr");
                mailim.Subject = "MALZEME TALEBİ HK.";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<b><p>YEDEK PARÇA AMBARINDAN MALZEME TALEBİ YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p></b>");
                sb.AppendLine(fisAna.MalzemeNo + ":&nbsp &nbsp Talep Numarasıdır.<br/>");
                sb.AppendLine(fisAna.Tarih + ":&nbsp &nbsp Tarihinde Talep Edilmiştir.<br/>");
                sb.AppendLine(fisAna.AmbarAdi + ":&nbsp &nbsp Ambarından İstekte Bulunulmuştur.<br/>");
                sb.AppendLine(fisAna.Bolum + ":&nbsp &nbsp Bölümü Talep Etmiştir.<br/><br/>");

                //sb.AppendLine(fisAna.MalzemeNo + "<th style='border: 1px solid black'>TALEP NUMARASI</th> ");
                //sb.AppendLine(fisAna.Tarih + "<th style='border: 1px solid black'>TALEP TARİHİ</th> ");

                //sb.AppendLine(fisAna.AmbarAdi + "<th style='border: 1px solid black'>TALEP EDİLEN AMBAR</th>");
                //sb.AppendLine(fisAna.Bolum + "<th style='border: 1px solid black'>TALEP EDEN BÖLÜM</th> </tr></thead>");


                sb.AppendLine("<u><b>TALEP EDİLEN ÜRÜNLER:</b></u><br/><br/><br/><table style='border: 1px solid black'>");
                sb.AppendLine("<thead style='border: 1px solid black' style='background - color: gray'><tr style='background - color: gray'style='border: 1px solid black'> <br/> &nbsp; <th style='border: 1px solid black'>STOK KODU</th>  &nbsp; <th style='border: 1px solid black'>STOK ADI</th>  &nbsp; <th style='border: 1px solid black'>MİKTAR</th>   &nbsp; <th style='border: 1px solid black'>AÇIKLAMA</th> </tr></thead>");
                foreach (var item in fisAna.Mrv_MalzemeDetay)
                {
                    //sb.AppendLine($"<li> {item.} {item.StokKodu} - {item.StokAdi} - {item.Parti} - {item.Miktar}</li>");
                    sb.AppendLine($"<tbody style='border: 1px solid black'><tr style='border: 1px solid black'> <td style='border: 1px solid black'> {item.StokKodu} </td> <td style='border: 1px solid black'> {item.StokAdi}</td> <td style='border: 1px solid black'> {item.Miktar}</td> <td style='border: 1px solid black'> {item.Aciklama}</td> </tr></tbody>");

                }
                sb.AppendLine("</table>");
                mailim.Body = sb.ToString();
            }


            mailim.IsBodyHtml = true;
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

            _dbPoly.SaveChanges();

            return RedirectToAction(nameof(MalzemeAnaListe));
        }
        public ActionResult YedekParcaAmbarDuzenle(int? id, string a, decimal b)
        {
            TempData["Gizle"] = "gizle";
            if (id == null)
                return RedirectToAction(nameof(Sepet));
            Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);

            if (fisDetay == null)
                return HttpNotFound();

            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu == a && x.Miktar1 == b);
            ViewData["ZzzMrvStokDetayTakip"] = stok;

            return View("YedekParcaTalepteBulun", fisDetay);
        }
        [HttpPost]
        public ActionResult YedekParcaAmbarDuzenle(int id, string a, decimal b, Mrv_MalzemeDetay deneme)
        {
            TempData["Gizle"] = "gizle";
            Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);
            if (ModelState.IsValid)
            {
                fisDetay.Miktar = deneme.Miktar;
                fisDetay.Aciklama = deneme.Aciklama;

                int result = _dbPoly.SaveChanges();
                return RedirectToAction("Sepet");
            }
            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu == a && x.Miktar1 == b);
            ViewData["ZzzMrvStokDetayTakip"] = stok;
            return View("YedekParcaTalepteBulun", deneme);
        }
        #endregion
        public ActionResult Ambarlar()
        {
            return View();
        }
        public ActionResult HamMaddeAmbarDuzenle(int? id, string a,decimal b)
        {
            TempData["Gizle"] = "gizle";
            if (id == null)
                return RedirectToAction(nameof(Sepet));
            Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);

            if (fisDetay == null)
                return HttpNotFound();

            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu== a && x.Miktar1 == b );
            ViewData["ZzzMrvStokDetayTakip"] = stok;
        
            return View("HamMaddeTalepteBulun", fisDetay);
        }
        [HttpPost]
        public ActionResult HamMaddeAmbarDuzenle(int id, string a, decimal b, Mrv_MalzemeDetay deneme)
        {
            TempData["Gizle"] = "gizle";
            Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);
            if (ModelState.IsValid)
            {
                fisDetay.Miktar = deneme.Miktar;
                fisDetay.Aciklama = deneme.Aciklama;

                int result = _dbPoly.SaveChanges();
                return RedirectToAction("Sepet");
            }
            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu == a && x.Miktar1 == b);
            ViewData["ZzzMrvStokDetayTakip"] = stok;
            return View("HamMaddeTalepteBulun", deneme);
        }



        #region MAMÜL AMBARI
        public ActionResult MamulAmbariStokGoruntuleme()
        {
            ViewBag.Tip = (int)MalzemeAnaTipi.Mamul;
            return View(_dbPoly.View_STOK_DURUM_PTKS_TASD.AsNoTracking());

        }
        public ActionResult MamulAmbariDuzenle(int? id, string a, string b,string c)
        {
            TempData["Gizle"] = "gizle";
            if (id == null)
                return RedirectToAction(nameof(Sepet));
            Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);

            if (fisDetay == null)
                return HttpNotFound();

            View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == a && x.Kalite == b && x.StokAdi==c);
            ViewData["View_STOK_DURUM_PTKS_TASD"] = stok;

            return View("MamulAmbariTalepteBulun", fisDetay);
        }
        [HttpPost]
        public ActionResult MamulAmbariDuzenle(int id, string a, string b,string c, Mrv_MalzemeDetay deneme)
        {
            TempData["Gizle"] = "gizle";
            Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);
            if (ModelState.IsValid)
            {
                fisDetay.Miktar = deneme.Miktar;
                fisDetay.Aciklama = deneme.Aciklama;

                int result = _dbPoly.SaveChanges();
                return RedirectToAction("Sepet");
            }
            View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == a && x.Kalite == b&&x.StokAdi==c);
            ViewData["View_STOK_DURUM_PTKS_TASD"] = stok;
            return View("MamulAmbariTalepteBulun", deneme);
        }
        public ActionResult MamulAmbarSil(int? id)
        {
            TempData["Gizle"] = "gizle";
            if (id == null)
            {
                return RedirectToAction(nameof(Sepet));
            }
            Mrv_MalzemeDetay deneme = _dbPoly.Mrv_MalzemeDetay.Find(id);

            if (deneme == null)
            {
                return HttpNotFound();
            }
            return View(deneme);
        }
        [HttpPost, ActionName("MamulAmbarSil")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mrv_MalzemeDetay deneme = _dbPoly.Mrv_MalzemeDetay.First(x => x.ID == id);
            Mrv_MalzemeAna fis = deneme.Mrv_MalzemeAna;
            _dbPoly.Mrv_MalzemeDetay.Remove(deneme);
            if (fis.Mrv_MalzemeDetay.Count == 0)
                _dbPoly.Mrv_MalzemeAna.Remove(fis);
            TempDataOlustur("Silme İşlemi Gerçekleştirilmiştir.", false);
            _dbPoly.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        #endregion



        #region HAMMADDEAMBARI
        public ActionResult HamMaddeAmbariStokGoruntuleme()
        {
            ViewBag.Tip = (int)MalzemeAnaTipi.Hammadde;
           
            var model =_dbPoly.ZzzMrvStokDetayTakip.Where(x => x.AmbarNo=="003" || x.AmbarNo=="050").AsNoTracking();
      
            return View(model);
        }
        public ActionResult HamMaddeTalepteBulun(string id1, decimal? id2,string id3,string id4)
        {
            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu == id1 && x.StokAdi == id3 && x.LotNo==id4);
           ViewBag.data= stok;
            return View();
        }



        [HttpPost]
        public ActionResult HamMaddeTalepteBulun(string id1, /*decimal? id2,*/string id3 ,Mrv_MalzemeDetay fisDetay)
        {
            TempData["Gizle"] = "gizle";
            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu == id1 /*&& x.Miktar1 == id2 */&& x.StokAdi == id3);
            //ViewData["ZzzMrvStokDetayTakip"] = stok;
            ViewBag.data = stok;
            if (fisDetay != null && fisDetay.Miktar != null && fisDetay.Miktar <= stok.Miktar1)
            {
                int sayac = 0;
                Mrv_MalzemeAna fis;
                fis = _dbPoly.Mrv_MalzemeAna.FirstOrDefault(x => x.TamamlandiMi == false && x.Personel == Kullanici.IsimSoyisim && x.Tip.Value == (int)MalzemeAnaTipi.Hammadde);
                if (fis == null)
                {
                    fis = new Mrv_MalzemeAna()
                    {
                        Tarih = DateTime.Now,
                        Personel = Kullanici.IsimSoyisim,
                        Bolum=Kullanici.Birim,
                        AmbarAdi=stok.AmbarAdi,
                        AmbarNo=stok.AmbarNo,
                        Tip = (int)MalzemeAnaTipi.Hammadde,
                        TamamlandiMi = false
                    };
                    sayac++;
                    _dbPoly.Mrv_MalzemeAna.Add(fis);
                    _dbPoly.SaveChanges();

                }

                fisDetay.Mrv_MalzemeAna = fis;
                fisDetay.MalzemeNo = fis.MalzemeNo;
                fisDetay.Parti = stok.LotNo;
                fisDetay.Birim = stok.OlcuBirimi1;
                
                fisDetay.StokAdi = stok.StokAdi;
                fisDetay.StokKodu = stok.StokKodu;
                fisDetay.Tarih = DateTime.Now;
                _dbPoly.Mrv_MalzemeDetay.Add(fisDetay);
                _dbPoly.SaveChanges();
                TempDataOlustur("Talebiniz oluşturulmuştur.", true);

                return RedirectToAction(nameof(HamMaddeAmbariStokGoruntuleme));
            }
            ViewData["ZzzMrvStokDetayTakip"] = stok;
            ViewBag.Hata = "En fazla " + stok.Miktar1.Value.ToString("G29") + " kadar talepte bulunabilirsiniz!";
            return View(fisDetay);
        }
 
 
        #endregion
        #region YEDEKPARÇAAMBARI
        public ActionResult YedekParcaAmbariStokGoruntuleme()
        {

            var getlist = new List<StokAmbar>();
            ViewBag.Tip = (int)MalzemeAnaTipi.YedekParca;
          var model=_dbPoly.ZzzMrvStokDetayTakip.Where(x => x.AmbarNo == "012" || x.AmbarNo == "115").ToList();
            return View(model);
        }
        public ActionResult YedekParcaTalepteBulun(string id1, decimal? id2)
        {
            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu == id1 && x.Miktar1 == id2);
            ViewBag.data = stok;
            return View();
        }



        [HttpPost]
        public ActionResult YedekParcaTalepteBulun(string id1, decimal? id2, Mrv_MalzemeDetay fisDetay)
        {
            TempData["Gizle"] = "gizle";

            ZzzMrvStokDetayTakip stok = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.StokKodu == id1 && x.Miktar1 == id2);
            ViewBag.data = stok;

            if (fisDetay != null && fisDetay.Miktar != null && fisDetay.Miktar <= stok.Miktar1)
            {
                int sayac = 0;
                Mrv_MalzemeAna fis;
                int result = 0;
                fis = _dbPoly.Mrv_MalzemeAna.FirstOrDefault(x => x.TamamlandiMi == false && x.Personel == Kullanici.IsimSoyisim && x.Tip.Value == (int)MalzemeAnaTipi.YedekParca);
               
                    if (fis == null)
                    {
                        fis = new Mrv_MalzemeAna()
                        {
                            Tarih = DateTime.Now,
                            Personel = Kullanici.IsimSoyisim,
                            Bolum = Kullanici.Birim,
                            AmbarAdi=stok.AmbarAdi,
                            AmbarNo=stok.AmbarNo,
                            Tip = (int)MalzemeAnaTipi.YedekParca,
                            TamamlandiMi = false

                        };
                        sayac++;
                        _dbPoly.Mrv_MalzemeAna.Add(fis);
                        result =_dbPoly.SaveChanges();

                    }

                fisDetay.Mrv_MalzemeAna = fis;
                fisDetay.MalzemeNo = fis.MalzemeNo;
                //fisDetay.Parti = stok.IATOPartiNo;
               
                fisDetay.Parti = stok.LotNo;
                fisDetay.Birim = stok.OlcuBirimi1;
                fisDetay.StokAdi = stok.StokAdi;
                fisDetay.StokKodu = stok.StokKodu;
                fisDetay.Tarih = DateTime.Now;
              
                    _dbPoly.Mrv_MalzemeDetay.Add(fisDetay);
                    result = _dbPoly.SaveChanges();
                    TempDataOlustur("Talebiniz oluşturulmuştur.", true);
                
           
                return RedirectToAction(nameof(YedekParcaAmbariStokGoruntuleme));
            }
            ViewBag.Hata = "En fazla " + stok.Miktar1.Value.ToString("G29") + " kadar talepte bulunabilirsiniz!";

            return View(fisDetay);
        }
        public ActionResult YedekParcaSepeti()
        {

            var kullaniciadi = Kullanici.IsimSoyisim;
            var model = _dbPoly.SrcnKullanicis.FirstOrDefault(x => x.IsimSoyisim == kullaniciadi);
            ViewData["SrcnKullanicis"] = model;
            Mrv_MalzemeAna fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.TamamlandiMi == false && x.Personel == Kullanici.IsimSoyisim).OrderByDescending(x => x.Tarih).FirstOrDefault();
            if (fisAna == null)
            {
                fisAna = new Mrv_MalzemeAna();
                //fisAna.Mrv_MalzemeDetay = new List<Mrv_MalzemeDetay>();
            }
            return View(fisAna);
        }

        #endregion
        #region TALEPLER

 
        public ActionResult MamulAmbariTalepteBulun(string id1,string id2,string id3)
        {
            TempData["Gizle"] = "gizle";
            View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == id1 && x.Kalite==id2 && x.StokAdi==id3);
            ViewBag.data = stok;
            return View();
        }
        [HttpPost]
        public ActionResult MamulAmbariTalepteBulun(string id1,string id2,string id3,Mrv_MalzemeDetay fisDetay)
        {

            View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == id1 && x.Kalite == id2 &&x.StokAdi==id3);
            ViewBag.data = stok;
            if (fisDetay != null && fisDetay.Miktar != null && fisDetay.Miktar <= stok.Miktar)
            {
                int sayac = 0;
                Mrv_MalzemeAna fis;
                fis = _dbPoly.Mrv_MalzemeAna.FirstOrDefault(x => x.TamamlandiMi == false && x.Personel == Kullanici.IsimSoyisim && x.Tip==(int)MalzemeAnaTipi.Mamul);
                if (fis == null)
                {
                    fis = new Mrv_MalzemeAna()
                    {
                        Tarih = DateTime.Now,
                        Personel = Kullanici.IsimSoyisim,
                        AmbarAdi="MAMUL AMBARI",
                       
                        Bolum = Kullanici.Birim,
                        Tip = (int)MalzemeAnaTipi.Mamul,
                        TamamlandiMi = false
                    };
                    sayac++;
                    _dbPoly.Mrv_MalzemeAna.Add(fis);
                    _dbPoly.SaveChanges();
                }

                fisDetay.Mrv_MalzemeAna = fis;
                fisDetay.MalzemeNo = fis.MalzemeNo;
                fisDetay.Firma = stok.Firma;
                fisDetay.Parti = stok.IATOPartiNo;
                fisDetay.Kalite = stok.Kalite;
                //fisDetay.Miktar = stok.Miktar;
                fisDetay.StokAdi = stok.StokAdi;
                fisDetay.Birim = "KG";
                fisDetay.Tarih = DateTime.Now;
                _dbPoly.Mrv_MalzemeDetay.Add(fisDetay);
                _dbPoly.SaveChanges();
                TempDataOlustur("Talebiniz oluşturulmuştur.", true);
                return RedirectToAction(nameof(MamulAmbariStokGoruntuleme));
            }
            ViewData["View_STOK_DURUM_PTKS_TASD"] = stok;
             ViewBag.Hata = "En fazla " + stok.Miktar.Value.ToString("G29") + " kadar talepte bulunabilirsiniz!";
            return View(fisDetay);
        }
       // [HttpPost]
        //public ActionResult FisAnaOnayla(int id)
        //{
        //    Mrv_MalzemeAna fisAna = _dbPoly.Mrv_MalzemeAna.FirstOrDefault(x => x.MalzemeNo == id);
        //    fisAna.TamamlandiMi = true;
        //    fisAna.Personel = Kullanici.IsimSoyisim;
        //    fisAna.Tarih = DateTime.Now;

        //    int count = 1;
        //    foreach (var item in fisAna.Mrv_MalzemeDetay)
        //    {
        //        item.SiraNo = count++;

        //    }
        //    _dbPoly.SaveChanges();
        //    return RedirectToAction(nameof(MalzemeAnaListe));
        //}
        #region GENEL
        public ActionResult MalzemeAnaListe()
        {
            //List<Mrv_MalzemeAna> fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.Personel == Kullanici.IsimSoyisim).ToList();

            //Mrv_MalzemeAna fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.Personel == Kullanici.IsimSoyisim).OrderByDescending(x => x.Tarih).FirstOrDefault();
            TempData["Gizle"] = "gizle";
            List<Mrv_MalzemeAna> fisAna = _dbPoly.Mrv_MalzemeAna.Where(x=>x.TamamlandiMi==true).OrderByDescending(x => x.Tarih).ToList();
            return View(fisAna);
        }
        public ActionResult MamulTalep()
        {
            List<Mrv_MalzemeAna> fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.TamamlandiMi == true && x.Tip == 1).OrderByDescending(x => x.Tarih).ToList();
            return PartialView(fisAna);
        }
        public ActionResult HamMaddeTalep()
        {
            List<Mrv_MalzemeAna> fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.TamamlandiMi == true && x.Tip == 2).OrderByDescending(x => x.Tarih).ToList();
            return PartialView(fisAna);
        }
     
        public ActionResult YedekParcaTalep()
        {
            List<Mrv_MalzemeAna> fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.TamamlandiMi == true && x.Tip == 3).OrderByDescending(x => x.Tarih).ToList();
            return PartialView(fisAna);
        }
        #endregion













        #endregion




    }
}

#region deneme
//public ActionResult Talep()
//{
//    var model = new MalzemeTalepViewModel();
//    return View(model);

//}
//[HttpPost]
//public ActionResult Talep(MalzemeTalepViewModel model)
//{
//    {
//        MalzemeTalepViewModel aaa = new MalzemeTalepViewModel();

//        aaa.Mrv_MalzemeTalepAna = model.Mrv_MalzemeTalepAna;
//        if (ModelState.IsValid)
//        {

//        }

//        return View(model);

//    }
//public JsonResult StokAmbar(string State)
//{
//    IEnumerable<SelectListItem> Counties = _dbPoly.ZzzMrvStokDetayTakip.Select(c => new SelectListItem
//    {
//        Value = c.AmbarNo,
//        Text = c.AmbarAdi
//    }).ToList();
//    return Json(Counties, JsonRequestBehavior.AllowGet);
//}

//public ActionResult FillCountyList(string State)
//{
//    Models.CountyList mod = new CountyList();
//    mod.TheState = State;
//    List<ZzzMrvStokDetayTakip> Counties = mod.ListCounties();
//    return Json(Counties, JsonRequestBehavior.AllowGet);
//}
//public JsonResult FillCountyList(string State)
//{
//    IEnumerable<SelectListItem> Counties = db.ListCountiesByState(State).Select(c => new SelectListItem
//    {
//        Value = c.County,
//        Text = c.County
//    }).ToList();
//    return Json(Counties, JsonRequestBehavior.AllowGet);
//}
#endregion
#region MİKTAR GÜNCELLEME
//public ActionResult MiktarGüncelle(string id1, string id2, string id3)
//{
//    ZzzMrvStokDetayTakip zzzMrvStokDetayTakip = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.AmbarNo == id1 && x.StokKodu == id2 && x.LotNo == id3);
//    return View(zzzMrvStokDetayTakip);

//}

//[HttpPost]
//public ActionResult MiktarGüncelle(ZzzMrvStokDetayTakip ZzzMrvStokDetayTakip)
//{
//    var model = _dbPoly.ZzzMrvStokDetayTakip.Find(ZzzMrvStokDetayTakip.AmbarNo, ZzzMrvStokDetayTakip.StokKodu, ZzzMrvStokDetayTakip.LotNo);
//    TempData["Gizle"] = "gizle";
//    if (ZzzMrvStokDetayTakip.Miktar1 <= model.Miktar1 )
//    {

//          model.Miktar1 = model.Miktar1 - ZzzMrvStokDetayTakip.Miktar1;

//    }
//    _dbPoly.SaveChanges();
//    return RedirectToAction("StokAmbar");

//}
#endregion

#region STOKDETAY
//public ActionResult PolyStokGoruntuleme()
//{

//    return View(_dbPoly.View_Stok_Durum_PTKS.AsNoTracking());

//}


//public POTA_TASDEntities _dbTasd = _dbTasdCreate();
//public ActionResult TasdStokGoruntuleme(int? ara)
//{
//    if (ara.HasValue)
//    {
//        return View(_dbTasd.View_Stok_Durum_TASD.ToList());

//    }
//    else
//    {
//        return View(_dbTasd.View_Stok_Durum_TASD.OrderByDescending(a => a.Miktar).ToList().ToPagedList(1, 9999));
//    }
//}
#endregion



#region sacmalardansecmeler
//public ActionResult HamMaddeSepeti()
//{

//    var kullaniciadi = Kullanici.IsimSoyisim;
//    var model = _dbPoly.SrcnKullanicis.FirstOrDefault(x => x.IsimSoyisim == kullaniciadi);
//    ViewData["SrcnKullanicis"] = model;
//    Mrv_MalzemeAna fisAna = _dbPoly.Mrv_MalzemeAna.Where(x => x.TamamlandiMi == false && x.Personel == Kullanici.IsimSoyisim).OrderByDescending(x => x.Tarih).FirstOrDefault();
//    if (fisAna == null)
//    {
//        fisAna = new Mrv_MalzemeAna();
//        //fisAna.Mrv_MalzemeDetay = new List<Mrv_MalzemeDetay>();
//    }
//    return View(fisAna);
//}
//public ActionResult StokAmbarDetay()
//{
//    //return View(_dbPoly.StokAmbar.OrderBy(a => a.AmbarNo).ToPagedList(1, 99));

//    var getlist = new List<StokAmbar>();
//    ViewBag.aaa = new SelectList(_dbPoly.ZzzMrvStokDetayTakip.Select(m => m.AmbarNo).Distinct(), "AmbarNo");
//    return View();


//}
//public ActionResult StokAmbarDetayTakip(string id)
//{

//    var model = _dbPoly.ZzzMrvStokDetayTakip.Where(x => x.AmbarNo == id).OrderByDescending(x => x.Miktar1).Distinct().ToList();

//    return View("StokAmbarDetayTakip", model);

//}
//public ActionResult StokTalebiDetay(string id1,string id2)
//{
//    View_STOK_DURUM_PTKS_TASD View_STOK_DURUM_PTKS_TASD = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == id1 && x.Kalite==id2);
//    if (View_STOK_DURUM_PTKS_TASD == null)
//    {
//        return HttpNotFound();
//    }
//    return View(View_STOK_DURUM_PTKS_TASD);
//}
//public ActionResult Create()
//{
//    List<SelectListItem> liste = new List<SelectListItem>();
//    liste.Add(new SelectListItem() { Value = "-1", Text = "Lütfen Bir Stok Seçiniz" });
//    foreach (var items in _dbPoly.View_STOK_DURUM_PTKS_TASD.ToList())
//    {
//        SelectListItem sli = new SelectListItem()
//        {
//            Value = items.IATOPartiNo.ToString(),
//            Text = items.StokAdi
//        };
//        liste.Add(sli);
//    }
//    SelectList selectLists = new SelectList("Value", "Text");
//    ViewData["IATOPartiNo"] = selectLists;
//    return View();
//}

//[HttpPost]
//public ActionResult Create(MalzemeTalepViewModel fisvm, int[] sira)
//{
//    Mrv_MalzemeAna fisAna = new Mrv_MalzemeAna();
//    if (fisvm != null)
//    {
//        foreach (var item in _dbPoly.Mrv_MalzemeDetay.ToList())
//        {
//            int id = item.ID;
//            int count = _dbPoly.Mrv_MalzemeAna.Where(x => x.MalzemeNo == id).Count();
//            item.SiraNo = count;
//            fisvm.Mrv_MalzemeDetay.SiraNo = item.SiraNo;
//        }
//        _dbPoly.Mrv_MalzemeAna.Add(fisvm.Mrv_MalzemeAna);
//        fisvm.Mrv_MalzemeDetay.MalzemeNo = fisvm.Mrv_MalzemeAna.MalzemeNo;
//        //fisvm.Mrv_MalzemeDetay.ID = (Convert.ToInt32(fisvm.View_STOK_DURUM_PTKS_TASD.IATOPartiNo));
//        //fisvm.FisDetay.SiraNo = 1;
//        _dbPoly.Mrv_MalzemeDetay.Add(fisvm.Mrv_MalzemeDetay);

//        _dbPoly.SaveChanges();
//    }

//    return RedirectToAction(nameof(MamulAmbariStokGoruntuleme));
//}




//public ActionResult AmbarStokTalep(string id1, string id2, string id3)
//{

//    ZzzMrvStokDetayTakip zzzMrvStokDetayTakip = _dbPoly.ZzzMrvStokDetayTakip.FirstOrDefault(x => x.AmbarNo == id1 && x.StokKodu == id2 && x.LotNo == id3);
//    if (zzzMrvStokDetayTakip == null)
//    {
//        return HttpNotFound();
//    }
//    return View(zzzMrvStokDetayTakip);
//}
//[HttpPost]
//public ActionResult AmbarStokTalep(ZzzMrvStokDetayTakip zzzMrvStokDetayTakip)
//{
//    TempData["Gizle"] = "gizle";
//    if (ModelState.IsValid)
//    {
//        _dbPoly.Entry(zzzMrvStokDetayTakip).State = EntityState.Modified;
//        _dbPoly.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    return View(zzzMrvStokDetayTakip);
//}
#endregion