using Polyteks.Katman.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Polyteks.Katman.Helpers
{
    public static class Helper
    {
        public static decimal DecimaleCevir(this string deger)
        {
            decimal sonuc = 0;

            if (deger.Contains(','))
            {
                var liste = deger.Split(',');

                sonuc = Convert.ToDecimal(liste[0]);
                var kusurat = liste[1];

                if (kusurat.ToCharArray().Length==1)
                {
                    var kusuratDeger = Convert.ToDecimal(kusurat) / 10;
                    sonuc += kusuratDeger;
                }
                else
                {
                    var kusuratDeger = Convert.ToDecimal(kusurat) / 100;
                    sonuc += kusuratDeger;
                }
            }
            else
            {
                sonuc = Convert.ToDecimal(deger);
            }
            return sonuc;
        }
        public static List<int> LaboratuvarYapilacakAnalizDurumIdler()
        {
            var liste = new List<int>();
            liste.Add(1);
            liste.Add(3);
            liste.Add(4);
            return liste;

        }
        public static List<DropItem> NumuneYapilabilirLikDurumlariGetir()
        {
            var liste = new List<DropItem>();
            liste.Add(new DropItem() { Id = "0", Tanim = "BELİRLENMEDİ" });
            liste.Add(new DropItem() { Id = "1", Tanim = "YAPILABİLİR" });
            liste.Add(new DropItem() { Id = "2", Tanim = "YAPILAMAZ" });
            liste.Add(new DropItem() { Id = "3", Tanim = "POY/FDY" });
            liste.Add(new DropItem() { Id = "4", Tanim = "TEKNİK TEKSTİL" });

            return liste;
        }
        public static List<DropItem> NumuneDenemeleriYapilmaDurumlariGetir()
        {
            var liste = new List<DropItem>();
            liste.Add(new DropItem() { Id = "0", Tanim = "BELİRLENMEDİ" });
            liste.Add(new DropItem() { Id = "1", Tanim = "YAPILMASI BEKLENİYOR" });
            liste.Add(new DropItem() { Id = "2", Tanim = "YAPILDI" });


            return liste;
        }
        public static List<DropItem> NumuneSipariseDonmeDurumlariGetir()
        {
            var liste = new List<DropItem>();
            liste.Add(new DropItem() { Id = "0", Tanim = "BELİRLENMEDİ" });
            liste.Add(new DropItem() { Id = "1", Tanim = "SİPARİŞE DÖNDÜ" });
            liste.Add(new DropItem() { Id = "2", Tanim = "SİPARİŞE DÖNMEDİ" });


            return liste;
        }
        public static string YetkiDurumuOncekiGetir(string Durum)
        {
            string sonuc = "0";

            Durum = Durum.Replace(" ", "");
            if (Durum == "ILKONAY")
            {
                sonuc = null;
            }

            if (Durum == "TEKNIKONAY")
            {
                sonuc = "ILKONAY";
            }

            if (Durum == "SONONAY")
            {
                sonuc = "TEKNIKONAY";
            }

            return sonuc;


        }
        public static int YetkiDurumuGuncelGetir(string Durum)
        {
            int sonuc = 0;

            Durum = Durum.Replace(" ", "");
            if (Durum == "ILKONAY")
            {
                sonuc = 0;
            }

            if (Durum == "TEKNIKONAY")
            {
                sonuc = 1;
            }

            if (Durum == "SONONAY")
            {
                sonuc = 2;
            }

            return sonuc;


        }
        public static string YetkiDurumuSonrakiGetir(string Durum)
        {
            string sonuc = null;

            Durum = Durum.Replace(" ", "");
            if (Durum == "ILKONAY")
            {
                sonuc = "TEKNIK ONAY";
            }

            if (Durum == "TEKNIKONAY")
            {
                sonuc = "SON ONAY";
            }



            return sonuc;


        }
        public static CookieBaseBilgi CookieBaseBilgiGetir(int SiteTipi)
        {

            var model = new CookieBaseBilgi();
            model.SiteTipi = SiteTipi;
            if (SiteTipi == 1)
            {

                model.CookieName = "tefrik";

            }
            if (SiteTipi == 2)
            {

                model.CookieName = "toplanti";

            }
            if (SiteTipi == 3)
            {

                model.CookieName = "msynPanel";

            }
            if (SiteTipi == 4)
            {

                model.CookieName = "msynSube";

            }

            return model;
        }
        public static int YetkiDurumuGerekenSeviyeGetir(int? OnaySiraNo)
        {
            if (OnaySiraNo == 0 || OnaySiraNo == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(OnaySiraNo - 1);
            }
        }
        public static string OnayGerekenStringDurum(int? OnaySiraNo)
        {
            string Durum = "Onayınız Yeterli";
            if (OnaySiraNo != null)
            {
                if (OnaySiraNo == 1)
                {
                    Durum = "İLK ONAY GEREKLİ";
                }
                if (OnaySiraNo == 2)
                {
                    Durum = "İLK ONAY VE TEKNİK ONAY GEREKLİ";
                }
            }

            return Durum;

        }
        public static List<DropItem> UretimHatlariGetir()
        {
            var liste = new List<DropItem>();

            liste.Add(new DropItem() { Id = "1", Tanim = "TEKSTÜRE" });
            liste.Add(new DropItem() { Id = "2", Tanim = "FANTAZİ/BÜKÜM" });
            liste.Add(new DropItem() { Id = "3", Tanim = "POY/FDY" });
            liste.Add(new DropItem() { Id = "4", Tanim = "TEKNİK TEKSTİL" });

            return liste;
        }
        public static List<DropItem> UretimHatBirimlerilariGetir()
        {
            var liste = new List<DropItem>();

            liste.Add(new DropItem() { Id = "1", Tanim = "Alt Salon" });
            liste.Add(new DropItem() { Id = "2", Tanim = "AS Salonu" });
      

            return liste;
        }
        public static bool MailGonder()
        {
            string GonderenMail = "sdoyuranli@polyteks.com.tr";
            string MailSifre = "12345678";
            // string GonderenMail = mailKullanici.EmailAdresi;
            // string MailSifre = mailKullanici.EmailSifre;
            var body = "<p>DENEME</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("bilgislem@polyteks.com.tr"));
            message.From = new MailAddress(GonderenMail);
            message.Subject = "Sistem Mail Gönderme Deneme - ";
            message.Body = body;
            message.Priority = MailPriority.High;
            message.IsBodyHtml = true;
            try
            {
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = GonderenMail,
                        Password = MailSifre
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "10.10.1.5";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);

                    return true;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
        }
        public static List<DropItem> NumuneYapilabilirlikBaseYapilabilirlikDurumlariGetir()
        {
            var liste = new List<DropItem>();
            liste.Add(new DropItem() { Id = "0", Tanim = "Seçim Yapılmadı" });
            liste.Add(new DropItem() { Id = "1", Tanim = "YAPILAMAZ" });
            liste.Add(new DropItem() { Id = "2", Tanim = "YAPILABİLİR" });


            return liste;
        }
        public static List<DropItem> NumuneYapilabilirlikDenemeDurumlariGetir()
        {
            var liste = new List<DropItem>();
            liste.Add(new DropItem() { Id = "0", Tanim = "Seçim Yapılmadı" });
            liste.Add(new DropItem() { Id = "1", Tanim = "YAPILDI" });
            liste.Add(new DropItem() { Id = "2", Tanim = "YAPILMADI" });
     

            return liste;
        }
        public static List<DropItem> NumuneYapilabilirlikSipariseDonmeDurumlariGetir()
        {
            var liste = new List<DropItem>();
            liste.Add(new DropItem() { Id = "0", Tanim = "Seçim Yapılmadı" });
            liste.Add(new DropItem() { Id = "1", Tanim = "NUMUNE KAYIDI OLUŞTURULDU" });
            liste.Add(new DropItem() { Id = "2", Tanim = "SİPARİŞ OLARAK OLARAK GÜNCELLENDİ" });
            liste.Add(new DropItem() { Id = "3", Tanim = "OLUMSUZ OLARAK BELİRLENDİ" });
        

            return liste;
        }

        public static List<DropItem> PartiSonuTakipPaketlemeDurumlari()
        {
            var liste = new List<DropItem>();
            liste.Add(new DropItem() { IdInt = 0, Tanim = "Paketleme Kontrolsüz Parti Sonu" });
            liste.Add(new DropItem() { IdInt = 1, Tanim = "Bilgi Onayları Bekleniyor" });
            liste.Add(new DropItem() { IdInt = 2, Tanim = "Paketleme Onayı Bekleniyor" });
            liste.Add(new DropItem() { IdInt = 3, Tanim = "Paketleme Kontrollü Parti Sonu Yapıldı" });
            return liste;
        }
    }
}
