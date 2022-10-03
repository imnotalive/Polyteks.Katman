using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using PagedList;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.Has.Controllers;
using Polyteks.Katman.TefrikBildirim.Models;


namespace Polyteks.Katman.TefrikBildirim.Controllers
{

    public class ImalatController : BaseController
    {


        #region KALİTEKONTROLBİLDİRİMİ
        public ActionResult KaliteBildirimPusulasiImalat(Mrv_KaliteBildirimDetay mrv_KaliteBildirimDetay)
        {
          
           
                var aaa = _dbPoly.Mrv_KaliteBildirimDetay.Include(m => m.Mrv_KaliteBildirimAna).Include(m => m.Mrv_KaliteBildirimBolumleri).Include(m => m.Mrv_KaliteBildirimHata).Include(m => m.Mrv_KaliteBildirimHata1).Include(m => m.Mrv_KaliteBildirimHata2);

            var b = _dbPoly.Mrv_KaliteBildirimDetay.Where(a=>a.Mrv_KaliteBildirimBolumleri.IsletmeBolumleri==Kullanici.Birim);
            if (b != null)
            {
                return View(b.ToList());
            }
            return HttpNotFound();
      

        }

        public ActionResult KaliteBildirimPusulasiMudahale(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mrv_KaliteBildirimDetay mrv_KaliteBildirimDetay = _dbPoly.Mrv_KaliteBildirimDetay.Find(id);
            Mrv_KaliteBildirimDetay deneme = _dbPoly.Mrv_KaliteBildirimDetay.Find(id);

            if (mrv_KaliteBildirimDetay == null)
            {
                return HttpNotFound();
            }

            //ViewBag.BildirimId = new SelectList(_dbPoly.Mrv_KaliteBildirimAna, "ID", "Personel", mrv_KaliteBildirimDetay.BildirimId);
            //ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri");
            //ViewBag.Hata1 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "Bolum");
            //ViewBag.Hata2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "Bolum");
            //ViewBag.Hata3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "Bolum");
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //ViewBag.Hata1YapilanMudahale = new SelectList(_dbPoly.Mrv_KaliteBildirimHataMudahale, "ID", "YapilanMudahale", mrv_KaliteBildirimDetay.Hata1YapilanMudahale);
            //ViewBag.Hata2YapilanMudahale = new SelectList(_dbPoly.Mrv_KaliteBildirimHataMudahale, "ID", "YapilanMudahale", mrv_KaliteBildirimDetay.Hata2YapilanMudahale);
            //ViewBag.Hata3YapilanMudahale = new SelectList(_dbPoly.Mrv_KaliteBildirimHataMudahale, "ID", "YapilanMudahale", mrv_KaliteBildirimDetay.Hata3YapilanMudahale);


            return View(deneme);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KaliteBildirimPusulasiMudahale(Mrv_KaliteBildirimDetay deneme, Mrv_KaliteBildirimAna denemess, int? i, int? id1, int? id2, int? id3)
        {
            Mrv_KaliteBildirimDetay stok = _dbPoly.Mrv_KaliteBildirimDetay.FirstOrDefault(x => x.Hata1 == id1 && x.Hata2 == id2 && x.Hata3 == id3);
            ViewBag.data = stok;

            if (ModelState.IsValid && denemess.ID.ToString() == deneme.ID.ToString())
            {


                deneme = new Mrv_KaliteBildirimDetay()
                {
                    MudahaleTarihSaat = DateTime.Now,
                    MudahaleKullanici = Kullanici.IsimSoyisim,
                    //PartiNo = deneme.PartiNo,
                    //SiraNo=deneme.SiraNo,
                    //MakineAdi=deneme.MakineAdi,
                    //PozisyonNo = deneme.PozisyonNo,
                    //KanalNo=deneme.KanalNo,
                    IsletmeAciklama = deneme.IsletmeAciklama,
                    Aciklama = deneme.Aciklama,
                    Hata1 = deneme.Hata1,


                    //Mrv_KaliteBildirimHata2 = deneme.Mrv_KaliteBildirimHata2,
                    //Mrv_KaliteBildirimHata1 = deneme.Mrv_KaliteBildirimHata1,

                    MudahaleKullaniciBolum = Kullanici.Birim,
                    Hata1YapilanMudahale = deneme.Hata1YapilanMudahale,
                    Hata2YapilanMudahale = deneme.Hata2YapilanMudahale,
                    Hata3YapilanMudahale = deneme.Hata3YapilanMudahale,

                    Durum = 1,


                };

                _dbPoly.SaveChanges();

            }
            //deneme.BildirimId = denemess.ID;
            //deneme.Hata1 = deneme.Hata1;
            //deneme.Hata2 = deneme.Hata2;
            //deneme.Hata3 = deneme.Hata3;
            //ViewBag.BildirimId = new SelectList(_dbPoly.Mrv_KaliteBildirimAna, "ID", "Personel", deneme.BildirimId);
            //ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri", deneme.Isletme);
            //ViewBag.Isletme = new SelectList(_dbPoly.Mrv_KaliteBildirimBolumleri, "ID", "IsletmeBolumleri", deneme.);
            //ViewBag.Hata1 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", deneme.Hata1);
            //ViewBag.Hata2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", deneme.Hata2);
            //ViewBag.Hata3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHata, "ID", "HataTanimi", deneme.Hata3);
            //ViewBag.MakineAdi = new SelectList(_dbPoly.Mrv_KaliteBildirim_Makine, "ID", "MakineAdi", deneme.MakineAdi);
            ViewData["Mrv_KaliteBildirimDetay"] = stok;

            _dbPoly.SaveChanges();
            TempDataOlustur("Talebiniz güncellenmiştir.", true);
            return RedirectToAction("KaliteBildirimPusulasiImalat");

        }
        public ActionResult States(int id)
        {
            var states = _dbPoly.Mrv_KaliteBildirimHataMudahale.Select(s => new { ID = s.ID, YapilanMudahale = s.YapilanMudahale }).ToList();
            return Json(states);
        }
        //public JsonResult cozumGetir(int p)
        //{
        //    var hatalar = (from x in _dbPoly.Mrv_KaliteBildirimHataMudahale
        //                   join y in _dbPoly.Mrv_KaliteBildirimHata on x.Mrv_KaliteBildirimHata.ID equals y.ID
        //                   where x.Mrv_KaliteBildirimHata.ID == p
        //                   select new
        //                   {
        //                       Text = x.HataTanimi,
        //                       Value = x.ID.ToString()
        //                   }).ToList().OrderBy(x => x.Text);
        //    return Json(hatalar, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult hataGetir(int p)
        //{
        //    var hatalar = (from x in _dbPoly.Mrv_KaliteBildirimHata
        //                   join y in _dbPoly.Mrv_KaliteBildirimBolumleri on x.Mrv_KaliteBildirimBolumleri.ID equals y.ID
        //                   where x.Mrv_KaliteBildirimBolumleri.ID == p
        //                   select new
        //                   {
        //                       Text = x.HataTanimi,
        //                       Value = x.ID.ToString()
        //                   }).ToList().OrderBy(x => x.Text);
        //    return Json(hatalar, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult KaliteBildirimPusulasiMudahale(int? id )
        //{
        //    TempData["Gizle"] = "gizle";

        //    //ViewBag.Hata1 = new SelectList(_dbPoly.Mrv_KaliteBildirimHataMudahale, "ID", "YapilanMudahale");
        //    //ViewBag.Hata2 = new SelectList(_dbPoly.Mrv_KaliteBildirimHataMudahale, "ID", "YapilanMudahale");
        //    //ViewBag.Hata3 = new SelectList(_dbPoly.Mrv_KaliteBildirimHataMudahale, "ID", "YapilanMudahale");

        //    if (id == null)
        //        return HttpNotFound();
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult KaliteBildirimPusulasiMudahale(int? id, Mrv_KaliteBildirimDetay deneme)
        //{
        //    TempData["Gizle"] = "gizle";
        //    Mrv_KaliteBildirimDetay fisDetay = _dbPoly.Mrv_KaliteBildirimDetay.FirstOrDefault(x => x.ID == id);
        //    if (ModelState.IsValid)
        //    {
        //        fisDetay.MudahaleTarihSaat = deneme.MudahaleTarihSaat;
        //        fisDetay.MudahaleKullanici = deneme.MudahaleKullanici;
        //        fisDetay.MudahaleKullaniciBolum = deneme.MudahaleKullaniciBolum;
        //        fisDetay.Hata1YapilanMudahale = deneme.Hata1YapilanMudahale;
        //        fisDetay.Hata2YapilanMudahale = deneme.Hata2YapilanMudahale;
        //        fisDetay.Hata3YapilanMudahale = deneme.Hata3YapilanMudahale;
        //        fisDetay.Aciklama = deneme.Aciklama;

        //        int result = _dbPoly.SaveChanges();
        //        return RedirectToAction("Sepet");
        //    }
        //    return View(fisDetay);
        //}
        //public ActionResult MamulAmbariDuzenle(int? id, string a, string b, string c)
        //{
        //    TempData["Gizle"] = "gizle";
        //    if (id == null)
        //        return RedirectToAction(nameof(Sepet));
        //    Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);

        //    if (fisDetay == null)
        //        return HttpNotFound();

        //    View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == a && x.Kalite == b && x.StokAdi == c);
        //    ViewData["View_STOK_DURUM_PTKS_TASD"] = stok;

        //    return View("MamulAmbariTalepteBulun", fisDetay);
        //}
        //[HttpPost]
        //public ActionResult MamulAmbariDuzenle(int id, string a, string b, string c, Mrv_MalzemeDetay deneme)
        //{
        //    TempData["Gizle"] = "gizle";
        //    Mrv_MalzemeDetay fisDetay = _dbPoly.Mrv_MalzemeDetay.FirstOrDefault(x => x.ID == id);
        //    if (ModelState.IsValid)
        //    {
        //        fisDetay.Miktar = deneme.Miktar;
        //        fisDetay.Aciklama = deneme.Aciklama;

        //        int result = _dbPoly.SaveChanges();
        //        return RedirectToAction("Sepet");
        //    }
        //    View_STOK_DURUM_PTKS_TASD stok = _dbPoly.View_STOK_DURUM_PTKS_TASD.FirstOrDefault(x => x.IATOPartiNo == a && x.Kalite == b && x.StokAdi == c);
        //    ViewData["View_STOK_DURUM_PTKS_TASD"] = stok;
        //    return View("MamulAmbariTalepteBulun", deneme);
        //}
        #endregion
        #region POYIKAZ


        public ActionResult PoyIkazTakibi(/*int? ara*/)
        {

            TempData["Gizle"] = "gizle";

            //if (ara.HasValue)
            //{
            //    return View(_dbPoly.Mrv_POY_Ikaz.Where(x => x.PartiNo.Contains(ara.ToString())).OrderByDescending(x => x.Tarih).ToList());

            //}
            //else
            //{
                return View(_dbPoly.Mrv_POY_Ikaz.OrderByDescending(a => a.Tarih).ToList().ToPagedList(1, 999));
            //}
            //var result = (from c in _dbPoly.Mrv_POY_Ikaz
            //              select new GridTables
            //              {
            //                  Tarih = c.Tarih,
            //                  PartiNo = c.PartiNo,
            //                  Denye_Flaman = c.Denye_Flaman,
            //                  Uretim_Mak_Kan_Poz = c.Uretim_Mak_Kan_Poz,
            //                  ProblemTanimi = c.ProblemTanimi,
            //                  YapilanMudahale = c.YapilanMudahale,
            //                  Teksture_Makine_No = c.Teksture_Makine_No,
            //                  Teksture_Pozisyon_No = c.Teksture_Pozisyon_No,
            //                  LabIncelemeTarihi = c.LabIncelemeTarihi,
            //                  LabSonuc = c.LabSonuc,
            //                  TekstureSonuc = c.TekstureSonuc

            //              }).ToList();

        }

        public ActionResult Report(string id)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "Report1.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<Mrv_POY_Ikaz> cm = new List<Mrv_POY_Ikaz>();
            using (POTA_PTKSEntities dc = new POTA_PTKSEntities())
            {
                cm = dc.Mrv_POY_Ikaz.ToList();
            }
            ReportDataSource rd = new ReportDataSource("DataSet1", cm);
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

        [HttpGet]

        public ActionResult PoyIkazTakibiYeni()
        {

            TempData["Gizle"] = "gizle";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PoyIkazTakibiYeni(Mrv_POY_Ikaz deneme)
        {
            TempData["Gizle"] = "gizle";
           
            MailMessage mailim = new MailMessage();

            mailim.To.Add("poyikaz@tasdelengroup.com");


            mailim.From = new MailAddress("pmailsystem1@tasdelengroup.com");
            //mailim.Subject = "deneme hehehe" + employee.Makine_No;
            mailim.Subject = "POY IKAZ BILDIRIMI(TEKSTURE) HK.";
            //var body = "<p>POY IKAZ BILDIRIMI YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p>" + deneme.PartiNo;
            //mailim.Body = body;
            mailim.Body = "<b><p>TEKSTURE BOLUMU YENI POY IKAZ BILDIRIMI YAPTI.URETIM BOLUMUNUN MUDAHALE ETMESİ BEKLENMEKTEDİR.SMART POTAYI KONTROL EDİNİZ.</p></b>"
                + deneme.PartiNo + ":&nbsp &nbsp Parti numarasında hata vardır.<br/>"
                + deneme.ProblemTanimi + ":&nbsp &nbsp Problemi bildirilmiştir.<br/>"
             + deneme.Uretim_Mak_Kan_Poz + ":&nbsp &nbsp İplik Pozisyonu<br/>"
             + deneme.Teksture_Makine_No + ":&nbsp &nbsp Tekstüre Makine Numarası<br/>"
             + deneme.Teksture_Pozisyon_No + ":&nbsp &nbsp Tekstüre Pozisyon Numarası<br/>"
             +Kullanici.IsimSoyisim + ":&nbsp &nbsp Kullanıcısı Bildirdi<hr/>"
           ;
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
            //if (ModelState.IsValid)
            //{
            //    //else if (yeni == null)
            //    //{
            //    //    yeni = new Mrv_POY_Ikaz()
            //    //    {
            //    //    }
            //    //}
            //    _dbPoly.Mrv_POY_Ikaz.Add(deneme);
            //    _dbPoly.SaveChanges();
            //    return RedirectToAction("PoyIkazTakibi");
            //}


            //return View(deneme);
      
          
                if (ModelState.IsValid)
                {
                deneme = new Mrv_POY_Ikaz()
                {
                    Tarih = DateTime.Now,
                    TekstureKullanici = Kullanici.IsimSoyisim,
                    PartiNo = deneme.PartiNo,
                    ProblemTanimi = deneme.ProblemTanimi,
                    Denye_Flaman = deneme.Denye_Flaman,
                    SiraliYuklemeIstekTalebi = deneme.SiraliYuklemeIstekTalebi,
                    Teksture_Makine_No=deneme.Teksture_Makine_No,
                    Teksture_Pozisyon_No=deneme.Teksture_Pozisyon_No,
                    Uretim_Mak_Kan_Poz=deneme.Uretim_Mak_Kan_Poz,

                   
                    };
             
                 
                }

            // Mrv_POY_Ikaz aaa = fis;
            //fis.PartiNo= aaa.PartiNo  ;
            //aaa.SiraliYuklemeIstekTalebi = fis.SiraliYuklemeIstekTalebi;
            //aaa.Parti = stok.IATOPartiNo;
            //aaa.Kalite = stok.Kalite;
            ////fisDetay.Miktar = stok.Miktar;
            //aaa.StokAdi = stok.StokAdi;
            _dbPoly.Mrv_POY_Ikaz.Add(deneme);
            _dbPoly.SaveChanges();
            TempDataOlustur("Talebiniz oluşturulmuştur.", true);
            return RedirectToAction("PoyIkazTakibi");
         
            
     
        }

        public ActionResult PoyIkazGuncelle(int? id)
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
        public ActionResult PoyIkazGuncelle(/*[Bind(Include = "Id,İkaz_tarihi,Parti_no,Problem_Tanımı,Yapılan_Mudahele,LabSonuc,UretımId,TekstureId,Makine_Kanal_No,Makine_Pozisyon_No,Tekstüre_Kanal_No,Tekstüre_Pozisyon_No")]*/ Mrv_POY_Ikaz deneme)
        {
            TempData["Gizle"] = "gizle";

            MailMessage mailim = new MailMessage();
       
            mailim.To.Add("poyikaz@tasdelengroup.com");
            mailim.From = new MailAddress("pmailsystem1@tasdelengroup.com");

            mailim.Subject = "POY IKAZ BILDIRIMI(URETIM) HK.";
            //var body = "<p>POY IKAZ BILDIRIMI YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p>" + deneme.PartiNo;
            mailim.Body = "<b><p>POY IKAZ BILDIRIMINDE URETIM MUDAHALE YAPMISTIR.İLGİLİ POYLAR LABORATUVARA GÖNDERİLMİŞTİR.</p></b>"
            + deneme.PartiNo + "&nbsp Parti numarasında hata vardır.<br/>"
            + deneme.ProblemTanimi + "&nbsp Problemi bildirilmiştir.<br/>"
            + deneme.Uretim_Mak_Kan_Poz + ":&nbsp İplik Pozisyonu<br/>"
             + deneme.Teksture_Makine_No + ":&nbsp Tekstüre Makine Numarası<br/>"
             + deneme.Teksture_Pozisyon_No + ":&nbsp Tekstüre Pozisyon Numarası<br/>"
           + "Üretim " + deneme.YapilanMudahale + "&nbsp Müdahalesi Yapmıştır.";

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
                TempDataOlustur("Talebiniz güncellenmiştir.", true);
                return RedirectToAction("PoyIkazTakibi");
            }
            return View(deneme);
        }
        public ActionResult PoyIkazTekstureSonuc(int? id)
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
        public ActionResult PoyIkazTekstureSonuc(/*[Bind(Include = "Id,İkaz_tarihi,Parti_no,Problem_Tanımı,Yapılan_Mudahele,LabSonuc,UretımId,TekstureId,Makine_Kanal_No,Makine_Pozisyon_No,Tekstüre_Kanal_No,Tekstüre_Pozisyon_No")]*/ Mrv_POY_Ikaz deneme)
        {
            TempData["Gizle"] = "gizle";

            MailMessage mailim = new MailMessage();

            mailim.To.Add("poyikaz@tasdelengroup.com");

            mailim.From = new MailAddress("pmailsystem1@tasdelengroup.com");

            mailim.Subject = "POY IKAZ BILDIRIMI(TEKSTURE SONUC) HK.";
            //var body = "<p>POY IKAZ BILDIRIMI YAPILDI.SMART POTAYI KONTROL EDİNİZ.</p>" + deneme.PartiNo;
            mailim.Body = "<p>POY İKAZ İÇİN TEKSTÜRE SONUCU GİRİLMİŞTİR.POY İKAZ SONLANMIŞTIR .</p>"
            + deneme.PartiNo + "&nbsp Parti numarasında hata vardır.<br/>"
            + deneme.ProblemTanimi + "&nbsp Problemi bildirilmiştir.<br/>"
            + deneme.Uretim_Mak_Kan_Poz + ":&nbsp İplik Pozisyonu<br/>"
             + deneme.Teksture_Makine_No + ":&nbsp Tekstüre Makine Numarası<br/>"
             + deneme.Teksture_Pozisyon_No + ":&nbsp Tekstüre Pozisyon Numarası<br/>"
           + "Üretim " + deneme.YapilanMudahale + "&nbsp Müdahalesi Yapmıştır.<br/>"
           + "POY İKAZDA &nbsp  " + deneme.TekstureSonuc;



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
                TempDataOlustur("Talebiniz sonuçlandırılmıştır.", true);
                return RedirectToAction("PoyIkazTakibi");
            }
            return View(deneme);
        }

        public ActionResult PoyIkazSil(int? id)
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


        [HttpPost, ActionName("PoyIkazSil")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mrv_POY_Ikaz deneme = _dbPoly.Mrv_POY_Ikaz.Find(id);
            _dbPoly.Mrv_POY_Ikaz.Remove(deneme);
            _dbPoly.SaveChanges();
            TempDataOlustur("Talebiniz silinmiştir.", false);
            return RedirectToAction("PoyIkazTakibi");
        }

        #endregion POYIKAZ


        public ImalatNumuneModel ImalatNumuneModeliBaseOlustur(int DurumId, int id)
        {
            var model = new ImalatNumuneModel();
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = birimler;
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 18 || a.DosyaDurumId == 17).ToList();

            if (id == 0)
            {
                var numDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == DurumId)
                    .OrderBy(a => a.KayitTarihi).ToList();
                model.NumuneYapilabilirlikDosyalar = numDosyalar;
                model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(DurumId);
            }
            else
            {

                model.NumuneYapilabilirlikDosya = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
                model.DosyaNumuneLabAnalizTablolar = DosyaNumuneLabAnalizTablolariOlustur(id);
     
                var numDenemeDosyalari = _dbPoly.SrcnDenemeDosya
                    .Where(a => a.BagliOlduguDosyaTipi == 2 && a.BagliOlduguDosyaId == id).ToList();
                foreach (var birim in birimler)
                {
                    var araItem = new BirimDenemeAraModel();
                    araItem.Birim = birim;

                    if (numDenemeDosyalari.Any(a => a.BirimId == birim.BirimId))
                    {
                        araItem.DenemeDosyalar.AddRange(numDenemeDosyalari.Where(a => a.BirimId == birim.BirimId));
                    }
                    model.BirimDenemeAraModeller.Add(araItem);

                }
            }


            return model;
        }

        public ImalatNumuneModel ImalatDenemeDosyaModeliBaseOlustur(int BirimId, int DenemeId)
        {
            var model = new ImalatNumuneModel();
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = birimler;
            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 18 || a.DosyaDurumId == 17).ToList();
            if (BirimId != 0)
            {
                model.Birim = _dbPoly.SrcnFabrikaBirims.Find(BirimId);
                model.DenemeDosyalar = _dbPoly.SrcnDenemeDosya.Where(a => a.BirimId == BirimId)
                    .OrderByDescending(a => a.OlusturmaTarihi).ToList();
            }

            if (DenemeId != 0)
            {
                var denemeDosya = _dbPoly.SrcnDenemeDosya.Find(DenemeId);
                model.DenemeDosya = denemeDosya;
                model.Birim = _dbPoly.SrcnFabrikaBirims.Find(denemeDosya.BirimId);
                model.NumuneYapilabilirlikDosya = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(denemeDosya.BagliOlduguDosyaId);
                model.DosyaNumuneLabAnalizTablolar = DosyaNumuneLabAnalizTablolariOlustur(model.NumuneYapilabilirlikDosya.NumuneYapilabilirlikDosyaId);
               
                var BirimMakineleri = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.BirimId == denemeDosya.BirimId).OrderBy(a => a.MakineNo).GroupBy(a => new { a.MakineId, a.MakineNo, a.IslemTuru, a.IslemNo }).Select(a => new DropItem() { Tanim = a.Key.MakineNo + " - " + a.Key.IslemTuru.Trim(), Id = a.Key.MakineId.ToString() + "-" + a.Key.IslemNo.Replace(" ", "") }).OrderBy(a => a.Id).ToList();
                model.DropBirimMakineleri = new SelectList(BirimMakineleri, "Id", "Tanim");

                var denemeDosyaItemlar = _dbPoly.SrcnDenemeDosyaItems.AsNoTracking().Where(a => a.DenemeDosyaId == DenemeId).OrderByDescending(a => a.OlusturmaTarihi).ToList();

                foreach (var dosyaItem in denemeDosyaItemlar)
                {
                  model.BirimDenemeAraModeller.Add(new BirimDenemeAraModel(){
                      DenemeDosyaItem = dosyaItem,
                      DosyaItemMakineParametreler = _dbPoly.SrcnDosyaItemMakineParametres.Where(a=>a.BagliOlduguDosyaItemId==dosyaItem.DenemeDosyaItemId).OrderBy(a=>a.ParametreAdi).ToList(),
                      DosyaLabAnalizTablolar = DosyaLabAnalizTablolariOlustur(3, dosyaItem.DenemeDosyaItemId)
                  });  
                }
            }
            return model;

        }

        public ActionResult NumuneHavuzuListesi(int? id)
        {
            int idd = 17;
            if (id != null)
            {
                idd = Convert.ToInt32(id);
                if (idd == 0)
                {
                    idd = 17;
                }
            }

            return View(ImalatNumuneModeliBaseOlustur(idd, 0));
        }

        public ActionResult NumuneImalatNumuneDosyaDetay(int id)
        {
            return View(ImalatNumuneModeliBaseOlustur(0, id));
        }
        public ActionResult NumuneImalatBirimIcinDosyaOlustur(int id, int id2)
        {
            // id=birim
            // id2 = numune
            var birim = _dbPoly.SrcnFabrikaBirims.Find(id);
            var editNumune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id2);
            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(21);
            var birimDenemeDosya = new SrcnDenemeDosya()
            {
                OlusturmaTarihi = DateTime.Now,
                BirimAdi = birim.BirimAdi,
                BagliOlduguDosyaId = editNumune.NumuneYapilabilirlikDosyaId,
                BirimId = birim.BirimId,
                SeciliMi = true,
                BagliOlduguDosyaTipi = 2,
                BagliOlduguDosyaTipiAdi = "Numune Yapılabilirlik",
                DenemeAdi = null,
                PartiAdi = "",
                PartiAnaKlasorId = 0,
                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                KayitYapanKulId = Kullanici.KullaniciId,
                CariAdi = editNumune.CariAdi,
                CariNo = editNumune.CariKodu,
                FirmaTipi = editNumune.FirmaTipi,
                DosyaDurumId = dosyaDurum.DosyaDurumId,
                DosyaDurumAdi = dosyaDurum.DosyaDurumAdi
            };
            _dbPoly.SrcnDenemeDosya.Add(birimDenemeDosya);
            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(1);
            return RedirectToAction("BirimDenemeDosyaDetay", "Imalat",
                new { id = birimDenemeDosya.DenemeDosyaId });
        }
        public ActionResult BirimDenemeDosyaDetay(int id)
        {
            Session["NumDenModel"] = null;
            TempData["Gizle"] = "gizle";
            return View(ImalatDenemeDosyaModeliBaseOlustur(0, id));
        }
        [HttpPost]
        public ActionResult DenemeAdiDegistir(ImalatNumuneModel model)
        {
            var deneme = model.DenemeDosya;
            var editDeneme = _dbPoly.SrcnDenemeDosya.Find(deneme.DenemeDosyaId);

            if (deneme.DenemeAdi == null)
            {
                TempDataOlustur("Lütfen Tanım Kodu Giriniz", false);
            }
            else
            {
                if (editDeneme.DenemeAdi == null)
                {
                    // ilk kayıtta hatayı engellemek için
                    editDeneme.DenemeAdi = "a";
                }
                if (editDeneme.DenemeAdi.ToUpper() == deneme.DenemeAdi.ToUpper())
                {
                    TempDataOlustur("Tanım Kodunu Değiştirmediniz", false);
                }
                else
                {
                    if (_dbPoly.SrcnDenemeDosya.Count(a => a.DenemeDosyaId != editDeneme.DenemeDosyaId && a.DenemeAdi == deneme.DenemeAdi.ToUpper()) > 0)
                    {
                        TempDataOlustur("Bu Tanım Kodu Daha Önce Kullanıldı", false);
                    }
                    else
                    {
                        bool harekeketlensinMi = false || editDeneme.BagliOlduguDosyaTipi == 2 && editDeneme.DenemeAdi.ToCharArray().Length == 0;
                        editDeneme.DenemeAdi = deneme.DenemeAdi.ToUpper();
                        _dbPoly.SaveChanges();
                        if (harekeketlensinMi)
                        {
                            // numuneYapilabilir
                            _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                            {
                                KayitTarihi = DateTime.Now,
                                DosyaHareketTanimId = 99,
                                SikayetNumuneDosyaId = editDeneme.BagliOlduguDosyaId,
                                DosyaAdi = "Numune Yapılabilirlik",
                                DosyaTipi = 2,
                                HareketAdi = "Deneme Kodu: " + deneme.DenemeAdi.ToUpper() + " olarak belirlenmiştir - " + Kullanici.IsimSoyisim
                            });
                            _dbPoly.SaveChanges();
                        }
                        TempDataCRUDOlustur(2);
                    }
                }

            }


            return RedirectToAction("BirimDenemeDosyaDetay", "Imalat", new { id = deneme.DenemeDosyaId });
        }

        [HttpPost]
        public ActionResult ImalatNumuneDosyaItemEkle(ImalatNumuneModel model)
        {
            var deneme = model.DenemeDosya;
            if (model.LabAnalizCesitleri.Any(a => a.SeciliMi))
            {

                var denemeDosyaItem = model.DenemeDosyaItem;
                var dosyaTipi = _dbPoly.SrcnDosyaTipis.Find(3);
                var labAnalizDurumu = _dbPoly.SrcnLabAnalizDurumus.Find(10);
                // lab sonucu bekleniyor
                var denemeDosyaDurum = _dbPoly.SrcnDosyaDurums.Find(23);

                var editDeneme = _dbPoly.SrcnDenemeDosya.Find(deneme.DenemeDosyaId);
                editDeneme.DosyaDurumId = denemeDosyaDurum.DosyaDurumId;
                editDeneme.DosyaDurumAdi = denemeDosyaDurum.DosyaDurumAdi;
                _dbPoly.SaveChanges();

                var makine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == model.Makine.MakineId);
                var islem = _dbPoly.Islem.First(a => a.IslemNo == model.Islem.IslemNo);

                var yenidenemeDosyaItem = new SrcnDenemeDosyaItems()
                {
                    Aciklama = denemeDosyaItem.Aciklama,
                    MakineId = makine.MakineId,
                    MakineNo = makine.MakineNo,
                    IslemNo = islem.IslemNo,
                    MakineAdi = makine.MakineAdi,
                    DenemeDosyaId = deneme.DenemeDosyaId,
                    OlusturmaTarihi = DateTime.Now,
                    DenemeAdi = editDeneme.DenemeAdi,
                    IslemTuru = islem.IslemTuru
                };
                _dbPoly.SrcnDenemeDosyaItems.Add(yenidenemeDosyaItem);
                _dbPoly.SaveChanges();
                int SiraSay = 0;

                // yeniden sıralandı
                foreach (var item in _dbPoly.SrcnDenemeDosyaItems.Where(a => a.DenemeDosyaId == deneme.DenemeDosyaId).OrderBy(a => a.OlusturmaTarihi).ToList())
                {
                    SiraSay++;
                    item.Sira = SiraSay;
                    _dbPoly.SaveChanges();
                }
                var seciliParaMetreler = model.DosyaItemMakineParametreler.Where(a => a.ParametreDegeri != null)
                    .Select(a => new { a.ParametreNo, a.ParametreDegeri }).ToList();
                var IdlerSadece = seciliParaMetreler.Select(a => a.ParametreNo).ToList();
                var ParametreNolar = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.MakineId== makine.MakineId && IdlerSadece.Any(b => b== a.ParametreNo)).OrderBy(a => a.ParametreAdi).ToList();

                var DosyaItemMakineParametreler = new List<SrcnDosyaItemMakineParametres>();
                foreach (var item in ParametreNolar)
                {
                    var yeniItem = new SrcnDosyaItemMakineParametres()
                    {
                        ParametreNo = item.ParametreNo,
                        ParametreAdi = item.ParametreAdi,
                        ParametreDegeri = "",
                        OlcuBirimi = item.OlcuBirimi,
                        BagliOlduguDosyaItemId = yenidenemeDosyaItem.DenemeDosyaItemId,
                        BagliOlduguDosyaTipi = 3,
                        BagliOlduguDosyaTipiAdi = dosyaTipi.DosyaTipi,
                    };
                    yeniItem.ParametreDegeri = seciliParaMetreler.First(a => a.ParametreNo == item.ParametreNo).ParametreDegeri;
                    DosyaItemMakineParametreler.Add(yeniItem);

                }
                _dbPoly.SrcnDosyaItemMakineParametres.AddRange(DosyaItemMakineParametreler);
                _dbPoly.SaveChanges();
                var yeniLabAnaliz = new SrcnLabAnalizs()
                {
                    DosyaTipi = dosyaTipi.DosyaId,
                    AnalizAdi = "Deneme Dosyası",
                    AnalizTipi = 3,
                    AnalizYapilacakBobinSayisi = model.AnalizYapilacakBobinSayisi,
                    CariAdi = editDeneme.CariAdi,
                    CariNo = editDeneme.CariNo,
                    BagliOlduguDosyaId = yenidenemeDosyaItem.DenemeDosyaItemId,
                    KayitTarihi = DateTime.Now,
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                    LabAnalizDurumu = labAnalizDurumu.LabAnalizDurumu,
                    LabAnalizDurumuAdi = labAnalizDurumu.LabAnalizDurumuAdi,
                    IstenenTerminTarihi = DateTime.Now.Date,
                    ImalatAnalizYapilmaTipi = 3,
                    MakineId = Convert.ToInt32(makine.MakineId)
                };
                _dbPoly.SrcnLabAnalizs.Add(yeniLabAnaliz);
                _dbPoly.SaveChanges();

                var secilenLabAnalizItemIdler = model.LabAnalizCesitleri.Where(a => a.SeciliMi).Select(a => a.LabAnalizCesitId).ToList();
                var secilenLabAnalizCesitler = _dbPoly.SrcnLabAnalizCesits.Where(a => secilenLabAnalizItemIdler.Any(b => b == a.LabAnalizCesitId)).OrderBy(a => a.Sira).ToList();

                for (int i = 1; i <= model.AnalizYapilacakBobinSayisi; i++)
                {
                    var labItem = new SrcnLabAnalizItems()
                    {
                        DosyaTipi = dosyaTipi.DosyaId,
                        LabAnalizId = yeniLabAnaliz.LabAnalizId,
                        LabGrupId = 3,
                        BagliOlduguDosyaId = deneme.DenemeDosyaId,
                        KayitTarihi = DateTime.Now,
                        IplikNo = i,
                        BobinAdi = "Bobin " + i.ToString()
                    };
                    _dbPoly.SrcnLabAnalizItems.Add(labItem);
                    _dbPoly.SaveChanges();
                    foreach (var analizCesit in secilenLabAnalizCesitler)
                    {
                        _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                        {
                            LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                            LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi,
                            LabAnalizItemId = labItem.LabAnalizItemId,
                            DegerTipi = analizCesit.DegerTipi,
                            IplikNo = i,
                            AnalizDegeriString = null,
                            MakinePozisyonNo = 0,
                            AnalizDegeri = 0,
                            IplikAdi = "Bobin " + i.ToString(),
                            LabAnalizCesitAdiEng = analizCesit.LabAnalizCesitAdiEng,
                        });
                        _dbPoly.SaveChanges();
                    }
                }

              




                TempDataOlustur("Kayıt Oluşturulmuştur. Yapılan denemeler bölümünden takip edebilirsiniz", true);

            }
            else
            {
                TempDataOlustur("Lütfen Yapılacak Analizleri Belirleyerek Tekrar Giriniz", false);
            }

            return RedirectToAction("BirimDenemeDosyaDetay", "Imalat", new { id = deneme.DenemeDosyaId });
        }

        #region Günlük İMalat ekle
        [HttpPost]
        public ActionResult GunlukImalatDosyaTalebi(ImalatModel model)
        {

            var GunlukImalatMatrisHucreSatirlar = model.GunlukImalatMatrisHucreSatirlar;

            var KayitOlacakListe = new List<DropItem>();
            bool sorunVarmi = false;
            if (model.Birim.BirimId == 3)
            {
                //fdy-poy
                foreach (var itt in GunlukImalatMatrisHucreSatirlar)
                {
                    if (sorunVarmi == false)
                    {
                        foreach (var item in itt.GunlukImalatMatrisHucreler)
                        {
                            if (sorunVarmi == false)
                            {
                                var takimSaati = item.TakimSaati;
                                var bobinSayisi = item.BobinSayisi;
                                var refakatNo = item.LottanOzelAlanaGecis.RefakatNo;
                                var kanalNo = item.KanalNo;
                                if (takimSaati != null && bobinSayisi > 0)
                                {
                                    // sorunyok
                                }
                                else
                                {

                                    if (takimSaati == null && bobinSayisi > 0)
                                    {
                                        TempDataOlustur(
                                            "Bobin Sayısı Girdiniz ama takım saati belirlemediniz. Yeniden Belirleyiniz",
                                            false);

                                        sorunVarmi = true;
                                    }
                                    if (takimSaati != null && bobinSayisi == 0)
                                    {
                                        TempDataOlustur(
                                            "Takım Saati Girdiniz ama bobin Saati belirlemediniz. Yeniden Belirleyiniz",
                                            false);
                                        sorunVarmi = true;
                                    }
                                }
                                if (sorunVarmi == false && takimSaati != null && bobinSayisi > 0)
                                {
                                    KayitOlacakListe.Add(new DropItem()
                                    {
                                        Tanim = refakatNo,
                                        Id = takimSaati,
                                        IdInt = bobinSayisi,
                                        DigerTanim = kanalNo.ToString()
                                    });
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                // diğer
                foreach (var item in GunlukImalatMatrisHucreSatirlar)
                {
                    if (sorunVarmi == false)
                    {
                        var takimSaati = item.TakimSaati;
                        var bobinSayisi = item.BobinSayisi;
                        var refakatNo = item.LottanOzelAlanaGecis.RefakatNo;
                        if (takimSaati != null && bobinSayisi > 0)
                        {
                            // sorunyok
                        }
                        else
                        {
                            if (takimSaati != null && bobinSayisi == 0)
                            {
                                TempDataOlustur(
                                    "Takım Saati Girdiniz ama bobin Saati belirlemediniz. Yeniden Belirleyiniz",
                                    false);
                                sorunVarmi = true;
                            }
                            else
                            {
                                if (takimSaati == null && bobinSayisi > 0)
                                {
                                    TempDataOlustur(
                                        "Bobin Sayısı Girdiniz ama takım saati belirlemediniz. Yeniden Belirleyiniz",
                                        false);

                                    sorunVarmi = true;
                                }
                            }
                        }
                        if (sorunVarmi == false)
                        {
                            KayitOlacakListe.Add(new DropItem()
                            {
                                Tanim = refakatNo,
                                Id = takimSaati,
                                IdInt = bobinSayisi
                            });
                        }
                    }
                }
            }
            if (KayitOlacakListe.Count == 0)
            {
                sorunVarmi = true;
                TempDataOlustur(
                    "Parti Seçimi Yapmadınız",
                    false);

            }
            if (sorunVarmi)
            {
                return RedirectToAction("PartinoyaGoreGunlukImalatTalebi", "Imalat", new { id = model.Birim.BirimId });
            }

            var birim = _dbPoly.SrcnFabrikaBirims.Find(model.Birim.BirimId);
            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(11);
            var labDurum = _dbPoly.SrcnLabAnalizDurumus.First(a => a.LabAnalizDurumu == 1);


            var yeniGunlukImalatDosya = new SrcnGunlukImalatDosyas()
            {
                BirimId = birim.BirimId,
                Aciklama = "",
                BirimAdi = birim.BirimAdi,
                KayitTarihi = DateTime.Now,

                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                DosyaDurumAdi = dosyaDurum.DosyaDurumAdi,
                DosyaDurumId = dosyaDurum.DosyaDurumId
            };
            _dbPoly.SrcnGunlukImalatDosyas.Add(yeniGunlukImalatDosya);
            _dbPoly.SaveChanges();


            foreach (var item in KayitOlacakListe)
            {
                var lottanGecis = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.First(a => a.RefakatNo == item.Tanim);
                var labGrup = _dbPoly.SrcnLabGrups.First(a =>
                    a.IplikUretimTeknikId == lottanGecis.IplikUretimTeknikId);
                var KanalNo = 0;
                var AnalizYapilacakBobinSayisi = item.IdInt;
                var TakimSaati = item.Id;

                var partiAnaKlasor = new SrcnPartiAnaKlasors();

                if (_dbPoly.SrcnPartiAnaKlasors.Any(a => a.PartiAdi == lottanGecis.PartiNo.Trim().ToUpper()))
                {
                    partiAnaKlasor =
                        _dbPoly.SrcnPartiAnaKlasors.First(a => a.PartiAdi == lottanGecis.PartiNo.Trim().ToUpper());
                }
                else
                {
                    partiAnaKlasor.PartiAdi = lottanGecis.PartiNo.Trim().ToUpper();
                    partiAnaKlasor.BirimId = birim.BirimId;
                    partiAnaKlasor.BirimAdi = birim.BirimAdi;
                    partiAnaKlasor.OlusturmaTarihi = DateTime.Now;
                    _dbPoly.SrcnPartiAnaKlasors.Add(partiAnaKlasor);
                    _dbPoly.SaveChanges();
                }
                if (item.DigerTanim != null)
                {
                    KanalNo = Convert.ToInt32(item.DigerTanim);
                }
                var yeniLAbAnaliz = new SrcnLabAnalizs()
                {
                    LabGrupId = labGrup.LabGrupId,
                    LabGrupAdi = labGrup.LabGrupAdi,
                    Aciklama = "",
                    AnalizAdi = "Günlük İmalat Kontrol",
                    AnalizTipi = labGrup.AnalizTipi,
                    AnalizYapilacakBobinSayisi = AnalizYapilacakBobinSayisi,
                    BagliOlduguDosyaId = yeniGunlukImalatDosya.GunlukImalatDosyaId,
                    AnalizYapilacakKumasSayisi = 0,
                    AnaliziYapilanBilesenSayisi = 0,
                    BobinIciLotNo = lottanGecis.PartiNo,
                    RefakartKartNo = lottanGecis.RefakatNo,
                    CariAdi = "",
                    CariNo = "",
                    FirmaTipi = 0,
                    GerceklesenTerminTarihi = null,
                    ImalatAnalizYapilmaTipi = 1, // günlük imalat
                    IsEmriNumarasiDurumu = 1,
                    IstenenTerminTarihi = DateTime.Now.Date,
                    KayitTarihi = DateTime.Now,
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                    KutuNo = "",
                    LabAciklama = "",
                    TakimSaati = TakimSaati,
                    LabAnalizDurumu = labDurum.LabAnalizDurumu,
                    LabAnalizDurumuAdi = labDurum.LabAnalizDurumuAdi,
                    PlanlananTerminTarihi = null,
                    MakineId = Convert.ToInt32(lottanGecis.MakineId),
                    PartiNo = lottanGecis.PartiNo,
                    SevkMiktari = null,
                    SevkTarihi = null,
                    KanalNo = KanalNo,
                    PartiAnaKlasorId = partiAnaKlasor.PartiAnaKlasorId,
                    DosyaTipi = 1,
                    DosyaTipiAdi = "Günlük İmalat Analizi"
                };

                _dbPoly.SrcnLabAnalizs.Add(yeniLAbAnaliz);
                _dbPoly.SaveChanges();

                GunlukImalatAnalizItemOlustur(yeniLAbAnaliz.LabAnalizId);
            }
            TempDataCRUDOlustur(1);
            if (Session["lab"] != null)
            {
                Session["lab"] = null;
                return RedirectToAction("GunlukImalatDosyalar", "Laboratuvar", new { id = yeniGunlukImalatDosya.BirimId, id2 = yeniGunlukImalatDosya.DosyaDurumId });
            }
            else
            {
                return RedirectToAction("PartinoyaGoreGunlukImalatTalebi", "Imalat", new { id = model.Birim.BirimId });
            }


        }
        #endregion
        /*
         burdan altı deneme
         */
        #region Kısmi Analiz Ekle
        public ActionResult PartiNoyaGoreKismiAnalizTalebi(int? id)
        {
            TempData["Gizle"] = "gizle";
            Session["KIATO"] = null;
            // Kısmi Analiz Talebi Olustur;
            int idd = 0;
            var Birimler = _dbPoly.SrcnFabrikaBirims.AsNoTracking().Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            var model = new ImalatModel
            {
                Birimler = Birimler
            };
            if (id == null || id == 0)
            {

                idd = 0;
            }
            else
            {
                idd = Convert.ToInt32(id);
            }
            if (idd > 0)
            {
                model.Birim = _dbPoly.SrcnFabrikaBirims.Find(idd);
                var araListe = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.Where(a => a.BirimId == idd)
                    .OrderBy(a => a.PartiNo).ToList();
                var DropItemlar = new List<DropItem>();

                foreach (var item in araListe)
                {
                    string Tanim = item.PartiNo;
                    Tanim = Tanim.ToUpper();
                    if (DropItemlar.Count(a => a.Tanim == Tanim) == 0)
                    {
                        DropItemlar.Add(new DropItem() { Tanim = Tanim, Id = item.RefakatNo });
                    }
                }

                model.Makineler = new SelectList(DropItemlar.OrderBy(a => a.Tanim).ToList(), "Id", "Tanim");




            }
            return View(model);
        }


        #endregion

        //#region Denemeler

        //public ActionResult DenemeDosyalari(int? id)
        //{
        //    int idd = 0;
        //    if (id != null)
        //    {
        //        idd = Convert.ToInt32(id);
        //    }

        //    return View(ImalatDenemeModeliBaseOlustur(idd, 0));
        //}

        //public ActionResult DenemeDosyaDetay(int id)
        //{
        //    return View(ImalatDenemeModeliBaseOlustur(0, id));
        //}
        //[HttpPost]
        //public ActionResult DenemeAdiDegistir(ImalatDenemeModel model)
        //{
        //    var deneme = model.DenemeDosya;
        //    var editDeneme = _dbPoly.SrcnDenemeDosya.Find(deneme.DenemeDosyaId);

        //    if (deneme.DenemeAdi == null)
        //    {
        //        TempDataOlustur("Lütfen Tanım Kodu Giriniz", false);
        //    }
        //    else
        //    {
        //        if (editDeneme.DenemeAdi.ToUpper() == deneme.DenemeAdi.ToUpper())
        //        {
        //            TempDataOlustur("Tanım Kodunu Değiştirmediniz", false);
        //        }
        //        else
        //        {
        //            if (_dbPoly.SrcnDenemeDosya.Count(a => a.DenemeDosyaId != editDeneme.DenemeDosyaId && a.DenemeAdi == deneme.DenemeAdi.ToUpper()) > 0)
        //            {
        //                TempDataOlustur("Bu Tanım Kodu Daha Önce Kullanıldı", false);
        //            }
        //            else
        //            {
        //                bool harekeketlensinMi = false || editDeneme.BagliOlduguDosyaTipi == 2 && editDeneme.DenemeAdi.ToCharArray().Length == 0;
        //                editDeneme.DenemeAdi = deneme.DenemeAdi.ToUpper();
        //                _dbPoly.SaveChanges();
        //                if (harekeketlensinMi)
        //                {
        //                    // numuneYapilabilir
        //                    _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
        //                    {
        //                        KayitTarihi = DateTime.Now,
        //                        DosyaHareketTanimId = 99,
        //                        SikayetNumuneDosyaId = editDeneme.BagliOlduguDosyaId,
        //                        DosyaAdi = "Numune Yapılabilirlik",
        //                        DosyaTipi = 2,
        //                        HareketAdi = "Deneme Kodu: " + deneme.DenemeAdi.ToUpper() + " olarak belirlenmiştir - " + Kullanici.IsimSoyisim
        //                    });
        //                    _dbPoly.SaveChanges();
        //                }
        //                TempDataCRUDOlustur(2);
        //            }
        //        }

        //    }


        //    return RedirectToAction("DenemeDosyaDetay", "Imalat", new { id = deneme.DenemeDosyaId });
        //}


        //public ActionResult DenemeDosyaDetayEkle(int id)
        //{
        //    var denemeDosya = _dbPoly.SrcnDenemeDosya.Find(id);
        //    if (denemeDosya.DenemeAdi.ToCharArray().Length == 0)
        //    {
        //        TempDataOlustur("Lütfen Tanım Kodu Belirtiniz", false);
        //        return RedirectToAction("DenemeDosyaDetay", "Imalat", new { id = id });
        //    }

        //    var model = ImalatDenemeModeliBaseOlustur(0, id);


        //    var BirimMakineleri = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.BirimId == denemeDosya.BirimId).OrderBy(a => a.MakineNo).GroupBy(a => new { a.MakineId, a.MakineNo, a.IslemTuru, a.IslemNo }).Select(a => new DropItem() { Tanim = a.Key.MakineNo + " - " + a.Key.IslemTuru.Trim(), Id = a.Key.MakineId.ToString() + "-" + a.Key.IslemNo.Replace(" ", "") }).OrderBy(a => a.Id).ToList();
        //    model.DropBirimMakineleri = new SelectList(BirimMakineleri, "Id", "Tanim");
        //    return View(model);
        //}
        //#endregion
        public void LabAnalizTalepHareketKayit(int LabAnalizId)
        {
            _dbPoly.SrcnLabAnalizHarekets.Add(new SrcnLabAnalizHarekets()
            {
                LabAnalizId = LabAnalizId,
                LabAnalizDurumu = 1,
                KayitTarihi = DateTime.Now,
                HareketAdi = string.Format("Laboratuvar Analiz Talebi Oluşturulmuştur. - {0}", Kullanici.IsimSoyisim)
            });
            _dbPoly.SaveChanges();
        }
        public ActionResult PartiNoyaGoreGunlukImalatTalebi(int? id)
        {


            TempData["Gizle"] = "gizle";
            Session["GIATO"] = null;
            int idd = 0;
            var Birimler = _dbPoly.SrcnFabrikaBirims.AsNoTracking().Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            var model = new ImalatModel
            {
                Birimler = Birimler
            };
            if (id == null || id == 0)
            {

                idd = 0;
            }
            else
            {
                idd = Convert.ToInt32(id);
            }
            if (idd > 0)
            {
                model.Birim = _dbPoly.SrcnFabrikaBirims.Find(idd);
                var araListe = _dbPoly.ZzzSrcnLottanOzelAlanaGecisler.Where(a => a.BirimId == idd)
                    .OrderBy(a => a.PartiNo).ToList();
                var DropItemlar = new List<DropItem>();

                foreach (var item in araListe)
                {
                    string Tanim = item.PartiNo;
                    Tanim = Tanim.ToUpper();
                    if (DropItemlar.Count(a => a.Tanim == Tanim) == 0)
                    {
                        DropItemlar.Add(new DropItem() { Tanim = Tanim, Id = item.RefakatNo });
                    }
                }

                model.Makineler = new SelectList(DropItemlar.OrderBy(a => a.Tanim).ToList(), "Id", "Tanim");




            }
            return View(model);
        }
        public ImalatModel ImalatAnalizTalebiOlustur(ImalatModel model)
        {
            var labAnaliz = model.LabAnaliz;
            var labGrup = _dbPoly.SrcnLabGrups.Find(model.LabGrup.LabGrupId);
            bool SorunVarmi = false;
            string mesaj = "";
            if (model.IstekTarihi == null)
            {
                SorunVarmi = true;
                mesaj += "Termin Tarihi Belirlemediniz</br>";
            }

            if (labGrup.AnalizTipi == 0)
            {

                // imalat kontrol

                if (labAnaliz.AnalizYapilacakBobinSayisi == 0 && model.IsEmriNumarasiHazirlikDurumu == 1)
                {
                    SorunVarmi = true;
                    mesaj += "Analiz Yapılacak Bobin Sayısını Belirleyiniz</br>";
                }
                if (model.PartiNoBobinSayisi == 0 && model.IsEmriNumarasiHazirlikDurumu == 2)
                {
                    SorunVarmi = true;
                    mesaj += "Analiz Yapılacak Bobin Sayısını Belirleyiniz</br>";
                }
                if (labAnaliz.ImalatAnalizYapilmaTipi == 0)
                {
                    SorunVarmi = true;
                    mesaj += "Analiz Tipini Belirleyiniz </br>";

                }
                if (model.IsEmriNumarasiHazirlikDurumu == 0)
                {
                    SorunVarmi = true;
                    mesaj += "İş Emri Oluşturma Durumunu Belirleyiniz </br>";

                }

                if (SorunVarmi == false)
                {
                    if (model.IsEmriNumarasiHazirlikDurumu == 1)
                    {
                        // iş emri var
                        if (labAnaliz.RefakartKartNo == null)
                        {
                            SorunVarmi = true;
                            mesaj += "Analiz Tipini seçiminize göre iş emri numarası girmeniz gerekmetedir. </br>";
                        }
                        else
                        {
                            if (!(_dbPoly.RefakatKarti.Any(a => a.RefakatNo == labAnaliz.RefakartKartNo && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null)))
                            {
                                SorunVarmi = true;
                                mesaj += "İş Emri numarasına uygun kayıt bulunamamıştır ya da bu iş emri işletme tarafından kapatılmıştır. Kontrol Ediniz!!! </br>";
                            }
                            else
                            {
                                var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == labAnaliz.RefakartKartNo && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null);
                                var makine = _dbPoly.Makine.First(a => a.MakineNo == refKart.MakineNo);
                                var zzRefListe =
                                    _dbPoly.ZzzSrcnTefrikRefakatFisListe.First(a => a.RefakatNo == refKart.RefakatNo);
                                var labDurum =
                                    _dbPoly.SrcnLabAnalizDurumus.First(a => a.LabAnalizDurumu == 1);
                                var yeniLAbAnaliz = new SrcnLabAnalizs()
                                {
                                    LabGrupId = labGrup.LabGrupId,
                                    LabGrupAdi = labGrup.LabGrupAdi,
                                    Aciklama = labAnaliz.Aciklama,
                                    AnalizAdi = "Günlük İmalat Kontrol",
                                    AnalizTipi = labGrup.AnalizTipi,
                                    AnalizYapilacakBobinSayisi = labAnaliz.AnalizYapilacakBobinSayisi,
                                    BagliOlduguDosyaId = 0,
                                    AnalizYapilacakKumasSayisi = 0,
                                    AnaliziYapilanBilesenSayisi = 0,
                                    BobinIciLotNo = zzRefListe.PartiNo,
                                    RefakartKartNo = labAnaliz.RefakartKartNo,
                                    CariAdi = "",
                                    CariNo = "",
                                    FirmaTipi = 0,
                                    GerceklesenTerminTarihi = null,
                                    ImalatAnalizYapilmaTipi = labAnaliz.ImalatAnalizYapilmaTipi,
                                    IsEmriNumarasiDurumu = 1,
                                    IstenenTerminTarihi = Convert.ToDateTime(model.IstekTarihi),
                                    KayitTarihi = DateTime.Now,
                                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                                    KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                                    KutuNo = "",
                                    LabAciklama = "",
                                    TakimSaati = labAnaliz.TakimSaati,
                                    LabAnalizDurumu = labDurum.LabAnalizDurumu,
                                    LabAnalizDurumuAdi = labDurum.LabAnalizDurumuAdi,
                                    PlanlananTerminTarihi = null,
                                    MakineId = Convert.ToInt32(makine.SBSonrasiCalismaSaati),
                                    PartiNo = zzRefListe.PartiNo,
                                    SevkMiktari = null,
                                    SevkTarihi = null
                                };
                                _dbPoly.SrcnLabAnalizs.Add(yeniLAbAnaliz);
                                _dbPoly.SaveChanges();
                                LabAnalizTalepHareketKayit(yeniLAbAnaliz.LabAnalizId);

                                SorunVarmi = false;
                                mesaj = "KAYIT İŞLEMİ YAPILMIŞTIR";

                            }
                        }
                    }
                    else
                    {
                        // iş emri yok

                        var SayacOndalikSayisi = Convert.ToInt32(model.MakineId);
                        var makine = _dbPoly.Makine.First(a => a.SayacOndalikSayisi == SayacOndalikSayisi);
                        var labDurum = _dbPoly.SrcnLabAnalizDurumus.First(a => a.LabAnalizDurumu == 1);
                        var yeniLAbAnaliz = new SrcnLabAnalizs()
                        {
                            LabGrupId = labGrup.LabGrupId,
                            LabGrupAdi = labGrup.LabGrupAdi,
                            Aciklama = labAnaliz.Aciklama,
                            AnalizAdi = "Günlük İmalat Kontrol",
                            AnalizTipi = labGrup.AnalizTipi,
                            AnalizYapilacakBobinSayisi = model.PartiNoBobinSayisi,
                            BagliOlduguDosyaId = 0,
                            AnalizYapilacakKumasSayisi = 0,
                            AnaliziYapilanBilesenSayisi = 0,
                            BobinIciLotNo = "",
                            RefakartKartNo = labAnaliz.RefakartKartNo,
                            CariAdi = "",
                            CariNo = "",
                            FirmaTipi = 0,
                            GerceklesenTerminTarihi = null,
                            ImalatAnalizYapilmaTipi = labAnaliz.ImalatAnalizYapilmaTipi,
                            IsEmriNumarasiDurumu = 2,
                            IstenenTerminTarihi = Convert.ToDateTime(model.IstekTarihi),
                            KayitTarihi = DateTime.Now,
                            KayitYapanKulAdi = Kullanici.IsimSoyisim,
                            KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                            KutuNo = "",
                            LabAciklama = "",
                            TakimSaati = model.PartiNoTakimSaati,
                            LabAnalizDurumu = labDurum.LabAnalizDurumu,
                            LabAnalizDurumuAdi = labDurum.LabAnalizDurumuAdi,
                            PlanlananTerminTarihi = null,
                            MakineId = Convert.ToInt32(makine.SBSonrasiCalismaSaati),
                            PartiNo = labAnaliz.PartiNo,
                            SevkMiktari = null,
                            SevkTarihi = null
                        };
                        _dbPoly.SrcnLabAnalizs.Add(yeniLAbAnaliz);
                        _dbPoly.SaveChanges();
                        LabAnalizTalepHareketKayit(yeniLAbAnaliz.LabAnalizId);
                        SorunVarmi = false;
                        mesaj = "KAYIT İŞLEMİ YAPILMIŞTIR";
                    }
                }


            }
            TempDataOlustur(mesaj, !SorunVarmi);
            model.PosttaSorunVarMi = SorunVarmi;
            return model;
        }
        // GET: Imalat
        public ActionResult Index()
        {
            var deger = Environment.MachineName;
            TempData["Gizle"] = "gizle";
            var model = new ImalatModel
            {
                SrcnLabGruplari = _dbPoly.SrcnLabGrups.Where(a => a.AnalizTipi == 0 && a.UstLabGrupId != 0)
                    .OrderBy(a => a.LabGrupAdi).ToList(),
                LabGrup = _dbPoly.SrcnLabGrups.First(a => a.AnalizTipi == 0 && a.UstLabGrupId == 0)
            };
            return View(model);
        }
        public ActionResult ImalatKategoriAnalizleri(int id)
        {
            var model = new ImalatModel();
            var labgrup = _dbPoly.SrcnLabGrups.Find(id);
            model.LabAnalizleri = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.LabGrupId == id)
                .OrderByDescending(a => a.KayitTarihi).ToList();
            model.LabGrup = labgrup;
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(id);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            return View(model);
        }
        public ActionResult ImalatKategoriAnalizTalebi(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = new ImalatModel();
            var labgrup = _dbPoly.SrcnLabGrups.Find(id);
            model.LabGrup = labgrup;
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(id);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            model.UretimHatlari = new SelectList(Helpers.Helper.UretimHatlariGetir(), "Id", "Tanim");
            model.LabGrubuAnalizleri = _dbPoly.SrcnLabGrupAnalizCesits.Where(a => a.LabGrupId == id).ToList();
            var lst = new List<DropItem> { new DropItem() { Id = "0", Tanim = "Birim Seçiniz" } };
            model.Makineler = new SelectList(lst, "Id", "Tanim");
            model.Kullanici = Kullanici;
            return View(model);
        }
        [HttpPost]
        public ActionResult ImalatKategoriAnalizTalebi(ImalatModel model)
        {
            model = ImalatAnalizTalebiOlustur(model);

            if (model.PosttaSorunVarMi)
            {
                return RedirectToAction("ImalatKategoriAnalizTalebi", "Imalat", new { id = model.LabGrup.LabGrupId });
            }
            else
            {
                return RedirectToAction("ImalatKategoriAnalizleri", "Imalat", new { id = model.LabGrup.LabGrupId });
            }

        }
        public ActionResult ImalatKategoriAnalizSonucu(int id)
        {
            TempData["Gizle"] = "gizle";
            var model = new ImalatModel();
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            var labgrup = _dbPoly.SrcnLabGrups.Find(id);
            model.LabGrup = labgrup;
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(id);
            model.SrcnLabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            model.UretimHatlari = new SelectList(Helpers.Helper.UretimHatlariGetir(), "Id", "Tanim");
            model.LabGrubuAnalizleri = _dbPoly.SrcnLabGrupAnalizCesits.Where(a => a.LabGrupId == id).ToList();
            var lst = new List<DropItem> { new DropItem() { Id = "0", Tanim = "Birim Seçiniz" } };
            model.Makineler = new SelectList(lst, "Id", "Tanim");
            model.Kullanici = Kullanici;

            return View(model);
        }
        #region  İmalat Parti Sonu
        public ActionResult ImalatPartiSonuBirimeGoreListe(int? id)
        {
            //PartiSonuTakipKontrolEt();
            TempData["Gizle"] = "gizle";
            var model = new ImalatPartiSonuModel();
            int idd = 0;

            if (id != null)
            {
                idd = Convert.ToInt32(id);
            }

            var Birim = new SrcnFabrikaBirims();
            var DropItemlar = new List<DropItem>();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            if (idd > 0)
            {

                var araListe = _dbPoly.ZzzSrcnPartiSonuDurumOzet.Where(a => a.SabitlenenMakineNo != "1-srcn" && a.PartiNo != null && a.BirimId == idd).OrderBy(a => a.PartiNo).ThenBy(a => a.PartiNo).ThenBy(a => a.IslemBaslamaTarihi).ThenBy(a => a.SiparisSiraNo).ToList();
                //foreach (var item in araListe)
                //{
                //    model.PartiSonuDurumOzetCheckItemlar.Add(new PartiSonuDurumOzetCheckItem() { PartiSonuDurumOzet = item });
                //}
                Birim = _dbPoly.SrcnFabrikaBirims.Find(idd); ;

                foreach (var item in araListe)
                {
                    string Tanim = item.PartiNo;
                    Tanim = Tanim.ToUpper();
                    if (DropItemlar.Count(a => a.Tanim == Tanim) == 0)
                    {
                        DropItemlar.Add(new DropItem() { Tanim = Tanim, Id = item.RefakatNo });
                    }
                }


                model.DropPartiNolar = new SelectList(DropItemlar.OrderBy(a => a.Tanim).ToList(), "Id", "Tanim");
                model.OnayAlinacakBirimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5 || a.BirimId == 18)
                    .OrderBy(a => a.BirimAdi).ToList();
            }
            model.Birim = Birim;
            model.Birimler = Birimler;
            model.Id = idd;
            model.IkinciButonGrup = 1;


            return View(model);

        }
        [HttpPost]
        public ActionResult ImalatPartiSonuTalebi(ImalatPartiSonuModel model)
        {
            var id = model.Id;
            var birim = _dbPoly.SrcnFabrikaBirims.Find(id);
            var liste = model.PartiSonuDurumOzetCheckItemlar.Where(a => a.SeciliMi).ToList();
            var onayAlinacakBirimler = model.OnayAlinacakBirimler.Where(a => a.SeciliMi).ToList();

            if (liste.Any())
            {
                bool IsEmriKayidiVarMi = false;

                foreach (var ii in liste.Where(a => a.PartiSonuDurumOzet.RefakatNo != null))
                {
                    if (_dbPoly.SrcnPartiSonuTakips.Any(a => a.RefakatNo.Trim() == ii.PartiSonuDurumOzet.RefakatNo.Trim()))
                    {
                        IsEmriKayidiVarMi = true;
                    }
                }

                if (IsEmriKayidiVarMi)
                {
                    TempDataOlustur("Seçilen İş Emirlerinden biri veya birkaçı için daha önce kayıt oluşturulmuştur", false);
                    return RedirectToAction("ImalatPartiSonuBirimeGoreListe", "Imalat", new { id = birim.BirimId });
                }
                else
                {



                    var refListe = new List<RefakatKarti>();
                    string mailKonu = "";
                    string mailIcerik = "";
                    var ccMailler = new List<string> { "muygur@polyteks.com.tr", "sayhan@polyteks.com.tr" };
                    var ccOnayMailler = new List<string>();
                    string Gonderici = "MDIKICI@polyteks.com.tr";


                    foreach (var item in liste)
                    {

                        var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == item.PartiSonuDurumOzet.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemBitisTarihi == null && a.IslemNo != "900");
                        refKart.SabitlenenMakineNo = "1-srcn";
                        refListe.Add(refKart);
                        _dbPoly.SaveChanges();
                    }

                    if (model.YapilacakPartiSonuIslemTipi == 1)
                    {
                        // partiyi direk kapat

                        mailKonu = "PARTİ SONU YAPILMIŞ ÜRETİMLER";

                        foreach (var item in liste)
                        {
                            string KulKodu = "SUPERVISOR";
                            if (Kullanici.KullaniciKodu != null)
                            {
                                KulKodu = Kullanici.KullaniciKodu;
                            }
                            var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == item.PartiSonuDurumOzet.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemBitisTarihi == null && a.IslemNo != "900");
                            refKart.IslemBitisTarihi = DateTime.Now;
                            refKart.SabitlenenMakineNo = "1-srcn";
                            _dbPoly.SaveChanges();
                        }
                        var aradurum = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(1);

                        foreach (var refakatKarti in refListe)
                        {
                            var itemzZz = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.AsNoTracking().First(a => a.RefakatNo == refakatKarti.RefakatNo);

                            var partiSonuTakip = new SrcnPartiSonuTakips()
                            {
                                RefakatNo = refakatKarti.RefakatNo,
                                PartiNo = itemzZz.PartiNo,
                                KayitTarihi = DateTime.Now,
                                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                                KayitYapanKulId = Kullanici.KullaniciId,
                                YapilacakPartiSonuIslemTipi = model.YapilacakPartiSonuIslemTipi,
                                YapilacakPartiSonuIslemTipiAdi = "İmalat Kontrollü Parti Kapama",
                                BirimId = birim.BirimId,
                                BirimAdi = birim.BirimAdi,
                                KapatanKulAdi = Kullanici.IsimSoyisim,
                                KapatanKulId = Kullanici.KullaniciId,
                                PartiKapatmaTarihi = DateTime.Now

                            };
                            partiSonuTakip.PartiSonuTakipHareketTipi = aradurum.PartiSonuTakipHareketTipi;
                            partiSonuTakip.PartiSonuTakipHareketAdi = aradurum.PartiSonuTakipHareketAdi;

                            _dbPoly.SrcnPartiSonuTakips.Add(partiSonuTakip);
                            _dbPoly.SaveChanges();

                        }
                    }
                    if (model.YapilacakPartiSonuIslemTipi == 2)
                    {
                        // paketlemeye gönder
                        mailKonu = "PARTİ SONU YAPILMA TALEBİ";
                        var partiListe = new List<SrcnPartiSonuTakips>();
                        foreach (var refakatKarti in refListe)
                        {
                            var itemzZz = _dbPoly.ZzzSrcnPartiSonuDurumOzet.AsNoTracking().First(a => a.RefakatNo == refakatKarti.RefakatNo);
                            var partiSonuTakip = new SrcnPartiSonuTakips()
                            {
                                RefakatNo = refakatKarti.RefakatNo,
                                PartiNo = itemzZz.PartiNo,
                                KayitTarihi = DateTime.Now,
                                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                                KayitYapanKulId = Kullanici.KullaniciId,
                                YapilacakPartiSonuIslemTipi = model.YapilacakPartiSonuIslemTipi,
                                YapilacakPartiSonuIslemTipiAdi = "Paketleme Kontrollü",
                                BirimId = birim.BirimId,
                                BirimAdi = birim.BirimAdi
                            };
                            if (onayAlinacakBirimler.Any())
                            {
                                var aradurum = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(2);
                                partiSonuTakip.PartiSonuTakipHareketTipi = aradurum.PartiSonuTakipHareketTipi;
                                partiSonuTakip.PartiSonuTakipHareketAdi = aradurum.PartiSonuTakipHareketAdi;
                            }
                            else
                            {
                                var aradurum = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(3);
                                partiSonuTakip.PartiSonuTakipHareketTipi = aradurum.PartiSonuTakipHareketTipi;
                                partiSonuTakip.PartiSonuTakipHareketAdi = aradurum.PartiSonuTakipHareketAdi;
                            }
                            _dbPoly.SrcnPartiSonuTakips.Add(partiSonuTakip);
                            _dbPoly.SaveChanges();
                            partiListe.Add(partiSonuTakip);
                        }
                        foreach (var partiSonu in partiListe)
                        {
                            foreach (var item in onayAlinacakBirimler)
                            {
                                var onayBirim = _dbPoly.SrcnFabrikaBirims.AsNoTracking().First(a => a.BirimId == item.BirimId);
                                ccOnayMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == onayBirim.BirimId).Select(a => a.EmailAdres).ToList());
                                _dbPoly.SrcnPartiSonuTakipBilgiOnays.Add(new SrcnPartiSonuTakipBilgiOnays()
                                {
                                    BirimId = onayBirim.BirimId,
                                    BirimAdi = onayBirim.BirimAdi,
                                    KayitTarihi = DateTime.Now,
                                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                                    KayitYapanKulId = Kullanici.KullaniciId,
                                    PartiSonuTakipId = partiSonu.PartiSonuTakipId,

                                });
                                _dbPoly.SaveChanges();

                            }
                        }

                    }

                    if (model.YapilacakPartiSonuIslemTipi == 3)
                    {
                        // sadece iş emrini 48 saat sonra kapat
                        ccMailler = new List<string>();
                        mailKonu = "İŞ EMRİ KAPATMA TALEBİ";
                        foreach (var item in liste)
                        {
                            string KulKodu = "SUPERVISOR";
                            if (Kullanici.KullaniciKodu != null)
                            {
                                KulKodu = Kullanici.KullaniciKodu;
                            }
                            var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == item.PartiSonuDurumOzet.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemBitisTarihi == null && a.IslemNo != "900");
                            refKart.IslemBitisTarihi = null;
                            refKart.SabitlenenMakineNo = "1-srcn";
                            _dbPoly.SaveChanges();
                        }

                        var aradurum = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(7);

                        foreach (var refakatKarti in refListe)
                        {
                            var itemzZz = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.AsNoTracking().First(a => a.RefakatNo == refakatKarti.RefakatNo);

                            var partiSonuTakip = new SrcnPartiSonuTakips()
                            {
                                RefakatNo = refakatKarti.RefakatNo,
                                PartiNo = itemzZz.PartiNo,
                                KayitTarihi = DateTime.Now,
                                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                                KayitYapanKulId = Kullanici.KullaniciId,
                                YapilacakPartiSonuIslemTipi = model.YapilacakPartiSonuIslemTipi,
                                YapilacakPartiSonuIslemTipiAdi = "Otomatik Kapatma-İmalat Talepli",
                                BirimId = birim.BirimId,
                                BirimAdi = birim.BirimAdi,
                                KapatanKulAdi = Kullanici.IsimSoyisim,
                                KapatanKulId = Kullanici.KullaniciId,
                                PartiKapatmaTarihi = DateTime.Now

                            };
                            partiSonuTakip.PartiSonuTakipHareketTipi = aradurum.PartiSonuTakipHareketTipi;
                            partiSonuTakip.PartiSonuTakipHareketAdi = aradurum.PartiSonuTakipHareketAdi;
                            _dbPoly.SrcnPartiSonuTakips.Add(partiSonuTakip);
                            _dbPoly.SaveChanges();

                        }
                    }

                    bool MailGondermeSorunVarmi = false;
                    if (Kullanici.BirimId != null)
                    {
                        var BirimId = Convert.ToInt32(Kullanici.BirimId);
                        if (_dbPoly.SrcnMailBildirimGrups.Any(a => a.MailBildirimGrupId == BirimId))
                        {
                            if (_dbPoly.SrcnMailBildirimGrupItems.Any(a => a.MailBildirimGrupId == BirimId))
                            {
                                ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == BirimId).Select(a => a.EmailAdres).ToList());
                            }
                            else
                            {
                                MailGondermeSorunVarmi = true;
                            }

                        }
                        else
                        {
                            MailGondermeSorunVarmi = true;
                        }

                    }
                    else
                    {
                        MailGondermeSorunVarmi = true;
                    }
                    mailIcerik = PartiSonuOzetTabloOlustur(refListe);

                    if (MailGondermeSorunVarmi)
                    {
                        ccMailler.Add("MDIKICI@polyteks.com.tr");
                    }


                    if (Kullanici.EmailAdres != null)
                    {
                        if (Kullanici.EmailAdres.Contains("@polyteks.com.tr"))
                        {
                            Gonderici = Kullanici.EmailAdres;
                        }
                    }

                    if (ccOnayMailler.Any())
                    {
                        ccMailler.AddRange(ccOnayMailler);
                    }

                    mailIcerik += "</br> " + Kullanici.IsimSoyisim;
                    GenelDurumMailGonder(Gonderici, ccMailler, mailIcerik, mailKonu);

                    TempDataOlustur("Güncelleme işlemi yapılmıştır", true);
                    return RedirectToAction("ImalatPartiSonuBekleyenListe", "Imalat", new { id = birim.BirimId });
                }
            }
            else
            {
                TempDataOlustur("Lütfen Parti Seçimi Yapın", false);
                return RedirectToAction("ImalatPartiSonuBirimeGoreListe", "Imalat", new { id = birim.BirimId });
            }
        }
        public ActionResult ImalatPartiSonuBekleyenListe(int? id)
        {
            TempData["Gizle"] = "gizle";
            var model = new ImalatPartiSonuModel();
            int idd = 0;

            if (id != null)
            {
                idd = Convert.ToInt32(id);
            }

            var Birim = _dbPoly.SrcnFabrikaBirims.Find(idd);
            var DropItemlar = new List<DropItem>();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();

            model.Birim = Birim;
            model.Birimler = Birimler;
            model.Id = idd;
            model.IkinciButonGrup = 3;

            var bekleyenListe = _dbPoly.SrcnPartiSonuTakips.AsNoTracking().Where(a => a.BirimId == idd && a.PartiSonuTakipHareketTipi < 3).OrderBy(a => a.PartiNo).ToList();



            foreach (var item in bekleyenListe)
            {

                if (_dbPoly.ZzzSrcnPartiSonuDurumOzet.Any(a => a.RefakatNo == item.RefakatNo))
                {
                    model.PartiSonuDurumOzetCheckItemlar.Add(new PartiSonuDurumOzetCheckItem() { PartiSonuDurumOzet = _dbPoly.ZzzSrcnPartiSonuDurumOzet.AsNoTracking().First(a => a.RefakatNo == item.RefakatNo), PartiSonuTakip = item, PartiSonuTakipBilgiOnaylar = _dbPoly.SrcnPartiSonuTakipBilgiOnays.Where(a => a.PartiSonuTakipId == item.PartiSonuTakipId).OrderBy(a => a.BirimAdi).ToList() });
                }

            }

            return View(model);

        }
        public ActionResult ImalatPartiSonuBilgiOnayiBekleyenListe(int? id)
        {
            TempData["Gizle"] = "gizle";
            var model = new ImalatPartiSonuModel();
            int idd = 0;

            if (id != null)
            {
                idd = Convert.ToInt32(id);
            }

            var Birim = _dbPoly.SrcnFabrikaBirims.Find(idd);
            var DropItemlar = new List<DropItem>();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();

            model.Birim = Birim;
            model.Birimler = Birimler;
            model.Id = idd;
            model.IkinciButonGrup = 2;
            var partiSonuIdler = _dbPoly.SrcnPartiSonuTakips
                .Where(a => (a.YapilacakPartiSonuIslemTipi == 2 || a.YapilacakPartiSonuIslemTipi == 3)).Select(a => a.PartiSonuTakipId).ToList();
            try
            {
                if (partiSonuIdler != null)
                {
                    var bekleyenListe = _dbPoly.SrcnPartiSonuTakipBilgiOnays.AsNoTracking().Where(a => a.BirimId == idd).OrderByDescending(a => a.KayitTarihi).ToList();
                    foreach (var item in bekleyenListe)
                    {
                        var partiSonu = _dbPoly.SrcnPartiSonuTakips.Where(a => a.PartiSonuTakipId == item.PartiSonuTakipId).FirstOrDefault();
                        var partisonudurumOzet = _dbPoly.ZzzSrcnPartiSonuDurumOzet.Where(a => a.RefakatNo == partiSonu.RefakatNo).ToList();
                        if (partisonudurumOzet.Count() > 0)
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
                }        
           
            }
            catch (Exception e)
            {

                var message = e.Message;
            }
          
           

            return View(model);

        }
        public ActionResult ImalatPartiSonuBilgiOnayiDegisiklik(int id)
        {
            var partiSonuTakipBilgiOnay = _dbPoly.SrcnPartiSonuTakipBilgiOnays.Find(id);
            var partisonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(partiSonuTakipBilgiOnay.PartiSonuTakipId);
            if (partisonuTakip.YapilacakPartiSonuIslemTipi >= 4)
            {
                TempDataOlustur("Seçilen Partinin kontrolü paketleme olduğu için işlem yapılamaz", false);
            }
            else
            {
                if (partiSonuTakipBilgiOnay.OnayapanKulId == 0)
                {
                    // onay veriyor
                    partiSonuTakipBilgiOnay.OnayTarihi = DateTime.Now;
                    partiSonuTakipBilgiOnay.OnayapanKulAdi = Kullanici.IsimSoyisim;
                    partiSonuTakipBilgiOnay.OnayapanKulId = Kullanici.KullaniciId;
                    _dbPoly.SaveChanges();
                }
                else
                {
                    // onayı kaldıryor
                    partiSonuTakipBilgiOnay.OnayTarihi = null;
                    partiSonuTakipBilgiOnay.OnayapanKulAdi = null;
                    partiSonuTakipBilgiOnay.OnayapanKulId = 0;
                    _dbPoly.SaveChanges();

                }
                TempDataCRUDOlustur(2);

                if (_dbPoly.SrcnPartiSonuTakipBilgiOnays.Count(a => a.PartiSonuTakipId == partisonuTakip.PartiSonuTakipId && a.OnayapanKulId == 0) == 0)
                {
                    var islemTipi = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(3);
                    partisonuTakip.PartiSonuTakipHareketTipi = islemTipi.PartiSonuTakipHareketTipi;
                    partisonuTakip.PartiSonuTakipHareketAdi = islemTipi.PartiSonuTakipHareketAdi;
                    _dbPoly.SaveChanges();
                }
                else
                {
                    var islemTipi = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(2);
                    partisonuTakip.PartiSonuTakipHareketTipi = islemTipi.PartiSonuTakipHareketTipi;
                    partisonuTakip.PartiSonuTakipHareketAdi = islemTipi.PartiSonuTakipHareketAdi;
                    _dbPoly.SaveChanges();
                }

            }

            partiSonuBilgiDegisiklikYap(id, 0);
            if (partiSonuTakipBilgiOnay.BirimId == 18)
            {
                return RedirectToAction("LabPartiSonuBilgiOnayiBekleyenListe", "Laboratuvar");
            }
            else
            {
                return RedirectToAction("ImalatPartiSonuBilgiOnayiBekleyenListe", "Imalat",
                new { id = partiSonuTakipBilgiOnay.BirimId });
            }


        }

        public SatisModel IsletmeYorumBase(int id)
        {
            // id = numune dosyaId
            //tip => 1= Ürge için deniz hanım, 2= mesut bülent aydın için
            var model = new SatisModel();
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = birimler;

            model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 29 || a.DosyaDurumId == 30).ToList();


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
                var musteriDosyalar = _dbPoly.SrcnMusteriSikayetDosyas.Where(a => a.DosyaDurumId == 29 || a.DosyaDurumId == 30)
                    .OrderBy(a => a.KayitTarihi).ToList();
                model.MusteriSikayetDosyalar = musteriDosyalar;
                model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(30);
            }
            else
            {
                int idd = Convert.ToInt32(id);
                if (idd == 30 || idd == 0)
                {
                    idd = 29;
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(30);
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
            var model = IsletmeYorumBase(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult MusteriSikayetleriIsletmeYorum(SatisModel model)
        {
            var musteriDosya = model.MusteriSikayetDosya;

            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(31);
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

            return RedirectToAction("MusteriSikayetleriIsletmeYorumlari");

        }
        public ActionResult ImalatPartiSonuBekleyenListeKaldir(int id)
        {
            var partisonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(id);
            int birimId = partisonuTakip.BirimId;
            var bilgionaylar = _dbPoly.SrcnPartiSonuTakipBilgiOnays.Where(a => a.PartiSonuTakipId == id).ToList();

            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId == partisonuTakip.BirimId).ToList();

            foreach (var itt in bilgionaylar)
            {
                if (birimler.Count(a => a.BirimId == itt.BirimId) == 0)
                {
                    birimler.AddRange(_dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId == itt.BirimId));
                }
            }

            var liste = new List<RefakatKarti>();

            var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == partisonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemBitisTarihi == null && a.IslemNo != "900");
            refKart.SabitlenenMakineNo = null;
            refKart.IslemBitisTarihi = null;
            _dbPoly.SaveChanges();

            liste.Add(refKart);
            string ccKullanici = "MDIKICI@polyteks.com.tr";
            if (Kullanici.EmailAdres != null)
            {
                if (Kullanici.EmailAdres.Contains("@polyteks.com.tr"))
                {
                    ccKullanici = Kullanici.EmailAdres;
                }
            }
            string mailIcerik = string.Format("{0} Partisi parti sonu talebi iptal edilmiştir. </br> {1} </br> {2}",
                partisonuTakip.PartiNo.Trim(), PartiSonuOzetTabloOlustur(liste), Kullanici.IsimSoyisim);
            string konu = partisonuTakip.PartiNo.Trim() + " PARTİ SONU TALEBİ İPTALİ";

            var Gonderilecekler = new List<string>();
            var birimIdler = birimler.Select(a => a.BirimId).Distinct().ToList();
            Gonderilecekler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => birimIdler.Any(b => b == a.MailBildirimGrupId)).Select(a => a.EmailAdres).Distinct().ToList());
            GenelDurumMailGonder(ccKullanici, Gonderilecekler, mailIcerik, konu);

            _dbPoly.SrcnPartiSonuTakips.Remove(partisonuTakip);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnPartiSonuTakipBilgiOnays.RemoveRange(bilgionaylar);
            _dbPoly.SaveChanges();
            TempDataCRUDOlustur(3);
            return RedirectToAction("ImalatPartiSonuBekleyenListe", "Imalat", new { id = birimId });
        }

        #endregion
        public ActionResult ImalatPartiSonuOzetListe(int? id)
        {
            TempData["Gizle"] = "gizle";
            int idd = 1;
            if (id == null || id == 0 || id == 1)
            {
                // parti Sonu yapılmamış
                //sati 
                idd = 1;
            }
            else
            {
                idd = 2;
            }

            var DropItemlar = new List<DropItem>();
            var araListe = new List<ZzzSrcnPartiSonuDurumOzet>();
            if (idd == 1)
            {
                // partiSonu Yapilmamis
                araListe = _dbPoly.ZzzSrcnPartiSonuDurumOzet.Where(a => a.SabitlenenMakineNo != "1-srcn" && a.PartiNo != null).ToList();
            }
            else
            {
                // parti sonu yapılmış
                araListe = _dbPoly.ZzzSrcnPartiSonuDurumOzet.AsNoTracking().Where(a => a.SabitlenenMakineNo == "1-srcn" && a.PartiNo != null).ToList();
            }

            foreach (var item in araListe)
            {
                string Tanim = item.PartiNo;
                Tanim = Tanim.ToUpper();
                if (DropItemlar.Count(a => a.Tanim == Tanim) == 0)
                {
                    DropItemlar.Add(new DropItem() { Tanim = Tanim, Id = item.RefakatNo });
                }

            }

#pragma warning disable IDE0017 // Simplify object initialization
            var model = new ImalatPartiSonuModel();
#pragma warning restore IDE0017 // Simplify object initialization
            model.Id = idd;
            model.DropPartiNolar = new SelectList(DropItemlar.OrderBy(a => a.Tanim).ToList(), "Id", "Tanim");

            return View(model);
        }
        public ActionResult ImalatPartiSonuGeriAl(int id)
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

            return RedirectToAction("Index");
        }
        public ActionResult ImalatDenemeDosyalari(int? id)
        {
            TempData["Gizle"] = "gizle";
            var model = new ImalatDenemelerModel();
            int idd = 0;
            if (id != null)
            {
                idd = Convert.ToInt32(id);
            }

            var Birim = new SrcnFabrikaBirims();
            var DropItemlar = new List<DropItem>();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            if (idd > 0)
            {
                var BirimMakineleri = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.BirimId == idd).OrderBy(a => a.MakineNo).GroupBy(a => new { a.MakineId, a.MakineNo }).Select(a => new DropItem() { Tanim = a.Key.MakineNo, Id = a.Key.MakineId.ToString() }).OrderBy(a => a.Id).ToList();
                Birim = _dbPoly.SrcnFabrikaBirims.First(a => a.BirimId == idd);
                model.DropBirimMakineleri = new SelectList(BirimMakineleri, "Id", "Tanim");
            }


            model.Birim = Birim;
            model.Birimler = Birimler;
            model.Id = idd;
            model.IkinciButonGrup = 1;


            return View(model);

        }
        #region İmalat Parti Klasörleri Toplu
        public ActionResult ImalatPartiAnaKlasorleri(int id = 0)
        {
            TempData["Gizle"] = "gizle";
            var model = new ImalatPartiAnaKlasorModel();
            var Birim = new SrcnFabrikaBirims();
            var DropItemlar = new List<DropItem>();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();

            if (id == 0)
            {
                if (_dbPoly.ZzzSrcnYeniPartiKlasorListes.Any())
                {
                    var liste = _dbPoly.ZzzSrcnYeniPartiKlasorListes.AsNoTracking().ToList();
                    foreach (var item in liste)
                    {
                        if (_dbPoly.SrcnPartiAnaKlasors.Any(a => a.PartiAdi == item.PartiNo.Trim()))
                        {
                            _dbPoly.SrcnPartiAnaKlasors.Add(new SrcnPartiAnaKlasors()
                            {
                                BirimId = item.BirimId,
                                BirimAdi = item.BirimAdi,
                                OlusturmaTarihi = DateTime.Now,
                                PartiAdi = item.PartiNo.Trim()
                            });
                            _dbPoly.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                var PartiAnaKlasorler = _dbPoly.SrcnPartiAnaKlasors.Where(a => a.BirimId == id).OrderBy(a => a.PartiAdi).GroupBy(a => new { a.PartiAnaKlasorId, a.PartiAdi }).Select(a => new DropItem() { Tanim = a.Key.PartiAdi, Id = a.Key.PartiAnaKlasorId.ToString() }).OrderBy(a => a.Id).ToList();
                Birim = _dbPoly.SrcnFabrikaBirims.First(a => a.BirimId == id);
                model.DropPartiler = new SelectList(PartiAnaKlasorler, "Id", "Tanim");
            }

            model.Birim = Birim;
            model.Birimler = Birimler;
            model.Id = id;
            model.IkinciButonGrup = 1;
            return View(model);

        }
        public ActionResult ImalatPartiAltKlasorleri(int id = 0, int id2 = 0)
        {
            // id= PartiAltKlasorTipi
            // id2=birim

            TempData["Gizle"] = "gizle";
            var model = new ImalatPartiAltKlasorModel();

            var Birim = new SrcnFabrikaBirims();
            var DropItemlar = new List<DropItem>();
            var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            var PartiAltKlasorTipi = _dbPoly.SrcnPartiAltKlasorTipis.Find(id);
            if (id2 > 0)
            {
                //var PartiAnaKlasorler = _dbPoly.SrcnPartiAnaKlasors.Where(a => a.BirimId == id).OrderBy(a => a.PartiAdi).GroupBy(a => new { a.PartiAnaKlasorId, a.PartiAdi }).Select(a => new DropItem() { Tanim = a.Key.PartiAdi, Id = a.Key.PartiAnaKlasorId.ToString() }).OrderBy(a => a.Id).ToList();
                //Birim = _dbPoly.SrcnFabrikaBirims.First(a => a.BirimId == id);
                //model.DropPartiler = new SelectList(PartiAnaKlasorler, "Id", "Tanim");
                Birim = _dbPoly.SrcnFabrikaBirims.Find(id2);
                var PartiAltKlasorleri = _dbPoly.SrcnPartiAltKlasors
                    .Where(a => a.BirimId == id2 && a.PartiAltKlasorTipiId == id)
                    .OrderByDescending(a => a.OlusturmaTarihi).ToList();
                model.PartiAltKlasorleri = PartiAltKlasorleri;
            }
            model.Birim = Birim;
            model.Birimler = Birimler;
            model.Id = id2;
            model.IkinciButonGrup = 1;
            model.PartiAltKlasorTipi = PartiAltKlasorTipi;


            return View(model);

        }
        public ActionResult ImalatPartiAltKlasorEkle(int id = 0, int id2 = 0)
        {
            // id= PartiAltKlasorTipi
            // id2=birim
            TempData["Gizle"] = "gizle";
            var model = new ImalatPartiAltKlasorModel();

            if (id != 0 && id2 != 0)
            {
                var DropItemlar = new List<DropItem>();
                var Birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
                var PartiAltKlasorTipi = _dbPoly.SrcnPartiAltKlasorTipis.Find(id);


                var BirimMakineleri = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.BirimId == id2).OrderBy(a => a.MakineNo).GroupBy(a => new { a.MakineId, a.MakineNo, a.IslemTuru, a.IslemNo }).Select(a => new DropItem() { Tanim = a.Key.MakineNo + " - " + a.Key.IslemTuru.Trim(), Id = a.Key.MakineId.ToString() + "-" + a.Key.IslemNo.Replace(" ", "") }).OrderBy(a => a.Id).ToList();

                model.Birim = _dbPoly.SrcnFabrikaBirims.Find(id2); ;
                model.Birimler = Birimler;
                model.Id = id2;
                model.IkinciButonGrup = 1;
                model.PartiAltKlasorTipi = PartiAltKlasorTipi;
                model.DropBirimMakineleri = new SelectList(BirimMakineleri, "Id", "Tanim");

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }



        }
        [HttpPost]
        public ActionResult ImalatPartiAltKlasorEkle(ImalatPartiAltKlasorModel model)
        {
            var doluParametreler = model.PartiAltKlasorDosyaItemlar.Where(a => a.ParametreDegeri != null).ToList();
            var makine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == model.Makine.MakineId);
            var PartiAltKlasorTipi = _dbPoly.SrcnPartiAltKlasorTipis.Find(model.PartiAltKlasorTipi.PartiAltKlasorTipiId);
            var islem = _dbPoly.Islem.First(a => a.IslemNo == model.Islem.IslemNo);
            var partiAltKlasor = model.PartiAltKlasor;
            bool sorunvarMi = false;
            if (partiAltKlasor.PartiAltKlasorKodAdi != null && doluParametreler.Any())
            {
                if (_dbPoly.SrcnPartiAltKlasors.Any(a => a.BirimId == makine.BirimId && a.PartiAltKlasorKodAdi == partiAltKlasor.PartiAltKlasorKodAdi.ToUpper()))
                {
                    sorunvarMi = true;
                    TempDataOlustur("Tanımlanan Kod Daha Önce Kullanılmıştır", false);
                }
                else
                {
                    var yeniPartiAltKlasor = new SrcnPartiAltKlasors()
                    {
                        BirimId = makine.BirimId,
                        BirimAdi = makine.BirimAdi,
                        PartiAltKlasorTipiId = PartiAltKlasorTipi.PartiAltKlasorTipiId,
                        PartiAltKlasorTipAdi = PartiAltKlasorTipi.PartiAltKlasorTipAdi,
                        KayitYapanKulAdi = Kullanici.IsimSoyisim,
                        KayitYapanKulId = Kullanici.KullaniciId,
                        OlusturmaTarihi = DateTime.Now,
                        PartiAltKlasorKodAdi = partiAltKlasor.PartiAltKlasorKodAdi
                    };
                    _dbPoly.SrcnPartiAltKlasors.Add(yeniPartiAltKlasor);
                    _dbPoly.SaveChanges();
                    var yeniPartiAltKlasorDosya = new SrcnPartiAltKlasorDosyas()
                    {
                        MakineNo = makine.MakineNo,
                        MakineId = makine.MakineId,
                        KayitYapanKulAdi = Kullanici.IsimSoyisim,
                        KayitYapanKulId = Kullanici.KullaniciId,
                        IslemNo = islem.IslemNo.Trim(),
                        OlusturmaTarihi = DateTime.Now,
                        IslemTuru = islem.IslemTuru.TrimEnd(),
                        PartiAltKlasorId = yeniPartiAltKlasor.PartiAltKlasorId,
                        PartiAltKlasorKodAdi = yeniPartiAltKlasor.PartiAltKlasorKodAdi.ToUpper(),
                        SiraNo = 1

                    };

                    _dbPoly.SrcnPartiAltKlasorDosyas.Add(yeniPartiAltKlasorDosya);
                    _dbPoly.SaveChanges();
                    var hareketTanim = _dbPoly.SrcnPartiAltKlasorDosyaHareketTanims.Find(1);
                    foreach (var i in doluParametreler)
                    {
                        var item = _dbPoly.ZzzSrcnIslemMakineParametre.First(a =>
                            a.MakineId == makine.MakineId && a.IslemNo == islem.IslemNo &&
                            a.ParametreNo == i.ParametreNo);
                        _dbPoly.SrcnPartiAltKlasorDosyaItems.Add(new SrcnPartiAltKlasorDosyaItems()
                        {
                            ParametreAdi = item.ParametreAdi.Trim(),
                            ParametreNo = item.ParametreNo,
                            ParametreDegeri = i.ParametreDegeri,
                            OlcuBirimi = item.OlcuBirimi,
                            PartiAltKlasorDosyaId = yeniPartiAltKlasorDosya.PartiAltKlasorDosyaId
                        });
                        _dbPoly.SaveChanges();


                        var hareketKayit = new SrcnPartiAltKlasorDosyaHarekets()
                        {
                            HareketTarihi = DateTime.Now,
                            PartiAltKlasorDosyaHareketTanimId = 1,
                            PartiAltKlasorDosyaHareketTanimi = hareketTanim.PartiAltKlasorDosyaHareketTanim,
                            PartiAltKlasorDosyaId = yeniPartiAltKlasorDosya.PartiAltKlasorDosyaId,
                            PartiAltKlasorDosyaHareketi = string.Format("{0} değeri {1} olarak eklenmiştir. {2}", item.ParametreAdi.Trim(), i.ParametreDegeri, Kullanici.IsimSoyisim)
                        };
                        _dbPoly.SrcnPartiAltKlasorDosyaHarekets.Add(hareketKayit);
                        _dbPoly.SaveChanges();



                    }

                    TempDataCRUDOlustur(1);
                }
            }
            else
            {
                sorunvarMi = true;
                TempDataOlustur("Lütfen Bilgileri Eksiksikz Giriniz", false);

            }
            if (sorunvarMi)
            {
                return RedirectToAction("ImalatPartiAltKlasorEkle", "Imalat", new { id = PartiAltKlasorTipi.PartiAltKlasorTipiId, id2 = makine.BirimId });
            }
            else
            {
                return RedirectToAction("ImalatPartiAltKlasorleri", "Imalat", new { id = PartiAltKlasorTipi.PartiAltKlasorTipiId, id2 = makine.BirimId });
            }

        }
        public ActionResult ImalatPartiAltKlasorDuzenle(int id)
        {
            var model = new ImalatPartiAltKlasorModel();
            var PartiAltKlasor = _dbPoly.SrcnPartiAltKlasors.Find(id);
            var PartiAltKlasorDosyalar = _dbPoly.SrcnPartiAltKlasorDosyas.Where(a => a.PartiAltKlasorId == id)
                .OrderByDescending(a => a.OlusturmaTarihi).ToList();
            model.PartiAltKlasor = PartiAltKlasor;
            model.PartiAltKlasorDosyalar = PartiAltKlasorDosyalar;
            return View(model);
        }
        public ActionResult ImalatPartiAltKlasorDosyaEkle(int id)
        {
            var model = new ImalatPartiAltKlasorModel();

            var PartiAltKlasor = _dbPoly.SrcnPartiAltKlasors.Find(id);
            var RefartiAltKlasorDosya = _dbPoly.SrcnPartiAltKlasorDosyas.OrderByDescending(a => a.SiraNo).First(a => a.PartiAltKlasorId == id);
            model.PartiAltKlasor = PartiAltKlasor;



            int makineId = Convert.ToInt32(RefartiAltKlasorDosya.MakineId);
            string IslemNo = RefartiAltKlasorDosya.IslemNo;
            var islem = _dbPoly.Islem.First(a => a.IslemNo.Replace(" ", "") == IslemNo);
            var ParametreNolar = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.MakineId == makineId && a.IslemNo == islem.IslemNo).OrderBy(a => a.ParametreAdi).ToList();
            model.Makine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == makineId);
            model.Islem = islem;
            var refparametreler = _dbPoly.SrcnPartiAltKlasorDosyaItems
                .Where(a => a.PartiAltKlasorDosyaId == RefartiAltKlasorDosya.PartiAltKlasorDosyaId).ToList();
            foreach (var item in ParametreNolar)
            {
                if (refparametreler.Any(a => a.ParametreNo == item.ParametreNo))
                {
                    model.PartiAltKlasorDosyaItemlar.Add(refparametreler.First(a => a.ParametreNo == item.ParametreNo));
                }
                else
                {
                    model.PartiAltKlasorDosyaItemlar.Add(new SrcnPartiAltKlasorDosyaItems()
                    {
                        ParametreAdi = item.ParametreAdi.Trim(),
                        ParametreNo = item.ParametreNo,
                        OlcuBirimi = item.OlcuBirimi,
                    });
                }

            }


            return View(model);
        }
        [HttpPost]
        public ActionResult ImalatPartiAltKlasorDosyaEkle(ImalatPartiAltKlasorModel model)
        {
            var doluParametreler = model.PartiAltKlasorDosyaItemlar.Where(a => a.ParametreDegeri != null).ToList();
            var partiAltKlasor = _dbPoly.SrcnPartiAltKlasors.Find(model.PartiAltKlasor.PartiAltKlasorId);
            var RefartiAltKlasorDosya = _dbPoly.SrcnPartiAltKlasorDosyas.OrderByDescending(a => a.SiraNo).First(a => a.PartiAltKlasorId == partiAltKlasor.PartiAltKlasorId);

            bool sorunvarMi = false;
            if (doluParametreler.Any())
            {
                var yeniPartiAltKlasorDosya = new SrcnPartiAltKlasorDosyas()
                {
                    MakineNo = RefartiAltKlasorDosya.MakineNo,
                    MakineId = RefartiAltKlasorDosya.MakineId,
                    KayitYapanKulAdi = Kullanici.IsimSoyisim,
                    KayitYapanKulId = Kullanici.KullaniciId,
                    IslemNo = RefartiAltKlasorDosya.IslemNo.Trim(),
                    OlusturmaTarihi = DateTime.Now,
                    IslemTuru = RefartiAltKlasorDosya.IslemTuru.TrimEnd(),
                    PartiAltKlasorId = partiAltKlasor.PartiAltKlasorId,
                    PartiAltKlasorKodAdi = partiAltKlasor.PartiAltKlasorKodAdi.ToUpper(),
                    SiraNo = RefartiAltKlasorDosya.SiraNo + 1

                };

                _dbPoly.SrcnPartiAltKlasorDosyas.Add(yeniPartiAltKlasorDosya);
                _dbPoly.SaveChanges();
                var hareketTanim = _dbPoly.SrcnPartiAltKlasorDosyaHareketTanims.Find(1);
                foreach (var i in doluParametreler)
                {
                    var item = _dbPoly.ZzzSrcnIslemMakineParametre.First(a =>
                        a.MakineId == RefartiAltKlasorDosya.MakineId && a.IslemNo == RefartiAltKlasorDosya.IslemNo &&
                        a.ParametreNo == i.ParametreNo);
                    _dbPoly.SrcnPartiAltKlasorDosyaItems.Add(new SrcnPartiAltKlasorDosyaItems()
                    {
                        ParametreAdi = item.ParametreAdi.Trim(),
                        ParametreNo = item.ParametreNo,
                        ParametreDegeri = i.ParametreDegeri,
                        OlcuBirimi = item.OlcuBirimi,
                        PartiAltKlasorDosyaId = yeniPartiAltKlasorDosya.PartiAltKlasorDosyaId
                    });
                    _dbPoly.SaveChanges();


                    var hareketKayit = new SrcnPartiAltKlasorDosyaHarekets()
                    {
                        HareketTarihi = DateTime.Now,
                        PartiAltKlasorDosyaHareketTanimId = 1,
                        PartiAltKlasorDosyaHareketTanimi = hareketTanim.PartiAltKlasorDosyaHareketTanim,
                        PartiAltKlasorDosyaId = yeniPartiAltKlasorDosya.PartiAltKlasorDosyaId,
                        PartiAltKlasorDosyaHareketi = string.Format("{0} değeri {1} olarak eklenmiştir. {2}", item.ParametreAdi.Trim(), i.ParametreDegeri, Kullanici.IsimSoyisim)
                    };
                    _dbPoly.SrcnPartiAltKlasorDosyaHarekets.Add(hareketKayit);
                    _dbPoly.SaveChanges();



                }

                TempDataCRUDOlustur(1);
            }
            else
            {
                sorunvarMi = true;
                TempDataOlustur("Lütfen Bilgileri Eksiksiz Giriniz", false);
            }

            if (sorunvarMi)
            {
                return RedirectToAction("ImalatPartiAltKlasorDosyaEkle", "Imalat", new { id = partiAltKlasor.PartiAltKlasorTipiId, id2 = partiAltKlasor.BirimId });
            }
            else
            {
                return RedirectToAction("ImalatPartiAltKlasorDuzenle", "Imalat", new { id = partiAltKlasor.PartiAltKlasorId });
            }

        }
        public ActionResult ImalatPartiAltKlasorDosyaDuzenle(int id)
        {
            var model = new ImalatPartiAltKlasorModel();
            var PartiAltKlasorDosya = _dbPoly.SrcnPartiAltKlasorDosyas.Find(id);
            var PartiAltKlasor = _dbPoly.SrcnPartiAltKlasors.Find(PartiAltKlasorDosya.PartiAltKlasorId);
            model.PartiAltKlasor = PartiAltKlasor;



            int makineId = Convert.ToInt32(PartiAltKlasorDosya.MakineId);
            string IslemNo = PartiAltKlasorDosya.IslemNo;
            var islem = _dbPoly.Islem.First(a => a.IslemNo.Replace(" ", "") == IslemNo);
            var ParametreNolar = _dbPoly.ZzzSrcnIslemMakineParametre.Where(a => a.MakineId == makineId && a.IslemNo == islem.IslemNo).OrderBy(a => a.ParametreAdi).ToList();
            model.Makine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == makineId);
            model.Islem = islem;
            var kayitliParametreler = _dbPoly.SrcnPartiAltKlasorDosyaItems
                .Where(a => a.PartiAltKlasorDosyaId == PartiAltKlasorDosya.PartiAltKlasorDosyaId).ToList();
            foreach (var item in ParametreNolar)
            {
                if (kayitliParametreler.Any(a => a.ParametreNo == item.ParametreNo))
                {
                    model.PartiAltKlasorDosyaItemlar.Add(kayitliParametreler.First(a => a.ParametreNo == item.ParametreNo));
                }
                else
                {
                    model.PartiAltKlasorDosyaItemlar.Add(new SrcnPartiAltKlasorDosyaItems()
                    {
                        ParametreAdi = item.ParametreAdi.Trim(),
                        ParametreNo = item.ParametreNo,
                        OlcuBirimi = item.OlcuBirimi,
                    });
                }

            }


            return View(model);
        }
        [HttpPost]
        public ActionResult ImalatPartiAltKlasorDosyaDuzenle(ImalatPartiAltKlasorModel model)
        {
            var doluParametreler = model.PartiAltKlasorDosyaItemlar.Where(a => a.ParametreDegeri != null).ToList();

            var silinecekParametreler = model.PartiAltKlasorDosyaItemlar.Where(a => a.ParametreDegeri == null && a.PartiAltKlasorDosyaItemId != 0);
            var eklenecekParametreler = model.PartiAltKlasorDosyaItemlar.Where(a => a.ParametreDegeri != null && a.PartiAltKlasorDosyaItemId == 0);
            var guncellenecekParametreler = model.PartiAltKlasorDosyaItemlar.Where(a => a.ParametreDegeri != null && a.PartiAltKlasorDosyaItemId != 0);

            var partiAltKlasorDosya = model.PartiAltKlasorDosya;
            var editpartiAltKlasorDosya = _dbPoly.SrcnPartiAltKlasorDosyas.Find(model.PartiAltKlasorDosya.PartiAltKlasorDosyaId);
            //  var kayitliParametreler = _dbPoly.SrcnPartiAltKlasorDosyaItems.Where(a => a.PartiAltKlasorDosyaId == editpartiAltKlasorDosya.PartiAltKlasorDosyaId).ToList();


            bool sorunvarMi = false;
            if (doluParametreler.Any())
            {
                if (silinecekParametreler.Any())
                {
                    var liste = new List<SrcnPartiAltKlasorDosyaItems>();
                    foreach (var item in silinecekParametreler)
                    {
                        var araItem = _dbPoly.SrcnPartiAltKlasorDosyaItems.Find(item.PartiAltKlasorDosyaItemId);
                        liste.Add(araItem);
                        var hareketTanim = _dbPoly.SrcnPartiAltKlasorDosyaHareketTanims.Find(3);
                        var hareketKayit = new SrcnPartiAltKlasorDosyaHarekets()
                        {
                            HareketTarihi = DateTime.Now,
                            PartiAltKlasorDosyaHareketTanimId = hareketTanim.PartiAltKlasorDosyaHareketTanimId,
                            PartiAltKlasorDosyaHareketTanimi = hareketTanim.PartiAltKlasorDosyaHareketTanim,
                            PartiAltKlasorDosyaId = editpartiAltKlasorDosya.PartiAltKlasorDosyaId,
                            PartiAltKlasorDosyaHareketi = string.Format("{0} değeri {1} silinmiştir. {2}", item.ParametreAdi.Trim(), araItem.ParametreDegeri, Kullanici.IsimSoyisim)
                        };
                        _dbPoly.SrcnPartiAltKlasorDosyaHarekets.Add(hareketKayit);
                        _dbPoly.SaveChanges();
                    }

                    _dbPoly.SrcnPartiAltKlasorDosyaItems.RemoveRange(liste);
                    _dbPoly.SaveChanges();
                }
                if (eklenecekParametreler.Any())
                {

                    foreach (var i in eklenecekParametreler)
                    {
                        var item = _dbPoly.ZzzSrcnIslemMakineParametre.First(a =>
                            a.MakineId == editpartiAltKlasorDosya.MakineId && a.IslemNo == editpartiAltKlasorDosya.IslemNo &&
                            a.ParametreNo == i.ParametreNo);
                        _dbPoly.SrcnPartiAltKlasorDosyaItems.Add(new SrcnPartiAltKlasorDosyaItems()
                        {
                            ParametreAdi = item.ParametreAdi.Trim(),
                            ParametreNo = item.ParametreNo,
                            ParametreDegeri = i.ParametreDegeri,
                            OlcuBirimi = item.OlcuBirimi,
                            PartiAltKlasorDosyaId = editpartiAltKlasorDosya.PartiAltKlasorDosyaId
                        });
                        _dbPoly.SaveChanges();

                        var hareketTanim = _dbPoly.SrcnPartiAltKlasorDosyaHareketTanims.Find(1);
                        var hareketKayit = new SrcnPartiAltKlasorDosyaHarekets()
                        {
                            HareketTarihi = DateTime.Now,
                            PartiAltKlasorDosyaHareketTanimId = 1,
                            PartiAltKlasorDosyaHareketTanimi = hareketTanim.PartiAltKlasorDosyaHareketTanim,
                            PartiAltKlasorDosyaId = editpartiAltKlasorDosya.PartiAltKlasorDosyaId,
                            PartiAltKlasorDosyaHareketi = string.Format("{0} değeri {1} olarak eklenmiştir. {2}", item.ParametreAdi.Trim(), i.ParametreDegeri, Kullanici.IsimSoyisim)
                        };
                        _dbPoly.SrcnPartiAltKlasorDosyaHarekets.Add(hareketKayit);
                        _dbPoly.SaveChanges();
                    }
                }
                if (guncellenecekParametreler.Any())
                {
                    foreach (var item in guncellenecekParametreler)
                    {
                        var editItem = _dbPoly.SrcnPartiAltKlasorDosyaItems.Find(item.PartiAltKlasorDosyaItemId);
                        if (editItem.ParametreDegeri != item.ParametreDegeri)
                        {
                            string eskiDeger = editItem.ParametreDegeri;
                            string YeniDeger = item.ParametreDegeri;
                            editItem.ParametreDegeri = item.ParametreDegeri;
                            _dbPoly.SaveChanges();

                            var hareketTanim = _dbPoly.SrcnPartiAltKlasorDosyaHareketTanims.Find(3);
                            var hareketKayit = new SrcnPartiAltKlasorDosyaHarekets()
                            {
                                HareketTarihi = DateTime.Now,
                                PartiAltKlasorDosyaHareketTanimId = hareketTanim.PartiAltKlasorDosyaHareketTanimId,
                                PartiAltKlasorDosyaHareketTanimi = hareketTanim.PartiAltKlasorDosyaHareketTanim,
                                PartiAltKlasorDosyaId = editpartiAltKlasorDosya.PartiAltKlasorDosyaId,
                                PartiAltKlasorDosyaHareketi = string.Format("{0} değeri {1} olan değer {2} olarak güncellenmiştir. {3}", editItem.ParametreAdi.Trim(), eskiDeger, YeniDeger, Kullanici.IsimSoyisim)
                            };
                            _dbPoly.SrcnPartiAltKlasorDosyaHarekets.Add(hareketKayit);
                            _dbPoly.SaveChanges();

                        }
                    }

                }
                TempDataCRUDOlustur(2);
            }
            else
            {
                sorunvarMi = true;
                TempDataOlustur("Lütfen Bilgileri Eksiksiz Giriniz", false);
            }

            if (sorunvarMi)
            {
                return RedirectToAction("ImalatPartiAltKlasorDosyaDuzenle", "Imalat", new { id = editpartiAltKlasorDosya.PartiAltKlasorDosyaId });
            }
            else
            {
                return RedirectToAction("ImalatPartiAltKlasorDuzenle", "Imalat", new { id = editpartiAltKlasorDosya.PartiAltKlasorId });
            }

        }
        #endregion
        public ActionResult PartiNoTanimlama()
        {


            var model = new ImalatModel
            {
                Makineler = new SelectList(_dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList(), "BirimId", "BirimAdi")
            };
            return View(model);
        }
        public ActionResult AyniAndaCalisanPartiler()
        {
            var liste = _dbPoly.ZzzSrcnPartiSonuDurumOzet.AsNoTracking().GroupBy(a => new { a.PartiNo, a.MakineAdi, a.RefakatNo })
                .Where(a => a.Key.PartiNo != null).Select(a => a.Key.RefakatNo).ToList();



            var model = new ImalatPartiSonuModel();

            foreach (var i in liste)
            {
                if (!(model.PartiSonuDurumOzetCheckItemlar.Any(a => a.PartiSonuDurumOzet.RefakatNo == i)))
                {
                    var item = _dbPoly.ZzzSrcnPartiSonuDurumOzet.First(a => a.RefakatNo == i);

                    model.PartiSonuDurumOzetCheckItemlar.Add(new PartiSonuDurumOzetCheckItem() { PartiSonuDurumOzet = item });
                }


            }


            var ozetListe = new List<PartiSonuDurumOzetCheckItem>();

            foreach (var item in model.PartiSonuDurumOzetCheckItemlar)
            {
                if (model.PartiSonuDurumOzetCheckItemlar.Count(a => a.PartiSonuDurumOzet.PartiNo == item.PartiSonuDurumOzet.PartiNo && a.PartiSonuDurumOzet.MakineAdi == item.PartiSonuDurumOzet.MakineAdi &&
                                                                  a.PartiSonuDurumOzet.MakineNo == item.PartiSonuDurumOzet.MakineNo &&
                                                                  a.PartiSonuDurumOzet.MakineAdi == item.PartiSonuDurumOzet.MakineAdi) > 1)
                {
                    ozetListe.Add(item);
                }
            }

            model.PartiSonuDurumOzetCheckItemlar = ozetListe.OrderBy(a => a.PartiSonuDurumOzet.PartiNo).ToList();
            return View(model);
        }
    }
}