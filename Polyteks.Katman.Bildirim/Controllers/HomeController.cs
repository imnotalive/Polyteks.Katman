using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.Has.Controllers;
using Polyteks.Katman.Has.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Polyteks.Katman.Bildirim.Controllers
{
    public class HomeController : BaseController
    {

        /*
liste.Add(new DropItem() { Tanim ="ADİL VİNÇ", Id = "302376"});
liste.Add(new DropItem() { Tanim ="AHMET KURT", Id = "301729"});
liste.Add(new DropItem() { Tanim ="ALİ YAVAŞ", Id = "301737"});
liste.Add(new DropItem() { Tanim ="AYDIN ÖZER", Id = "302114"});
liste.Add(new DropItem() { Tanim ="AYŞE ERÖZ", Id = "302447"});
liste.Add(new DropItem() { Tanim ="BASRİ SÜY", Id = "302605"});
liste.Add(new DropItem() { Tanim ="BAYRAM YORULMAZ", Id = "301038"});
liste.Add(new DropItem() { Tanim ="BİRCAN ÇAYIRLI", Id = "302574"});
liste.Add(new DropItem() { Tanim ="BİRGÜL ELİÇORA", Id = "302431"});
liste.Add(new DropItem() { Tanim ="BURHAN ERKUL", Id = "302239"});
liste.Add(new DropItem() { Tanim ="CEMİL KARAGÖZ", Id = "302243"});
liste.Add(new DropItem() { Tanim ="CEMİLE ÜLKER", Id = "302607"});
liste.Add(new DropItem() { Tanim ="CENGİZ GÜLCÜ", Id = "301522"});
liste.Add(new DropItem() { Tanim ="DAVUT ŞENGELEN", Id = "301449"});
liste.Add(new DropItem() { Tanim ="DUYGU KOYUN", Id = "302500"});
liste.Add(new DropItem() { Tanim ="EKREM CANSEVER", Id = "302167"});
liste.Add(new DropItem() { Tanim ="ENGİN TURUS", Id = "	300737"});
liste.Add(new DropItem() { Tanim ="ERDİNÇ AYDEMİR", Id = "	300720"});
liste.Add(new DropItem() { Tanim ="FADİME ORAL", Id = "	302637"});
liste.Add(new DropItem() { Tanim ="FATMA ÇİFTÇİ", Id = "	302483"});
liste.Add(new DropItem() { Tanim ="FATMA ERGÜN", Id = "	302698"});
liste.Add(new DropItem() { Tanim ="FERİDE KASAPOĞLU", Id = "	302448"});
liste.Add(new DropItem() { Tanim ="FERİDE PEHLİVAN", Id = "	302537"});
liste.Add(new DropItem() { Tanim ="FİKRİ GÜLER", Id = "	301506"});
liste.Add(new DropItem() { Tanim ="GÖKHAN TURHAN", Id = "	301974"});
liste.Add(new DropItem() { Tanim ="GÜLSER AYDEMİR", Id = "	302647"});
liste.Add(new DropItem() { Tanim ="GÜLSEREN KAPUCU", Id = "	302617"});
liste.Add(new DropItem() { Tanim ="GÜVEN ŞAHİN", Id = "	302634"});
liste.Add(new DropItem() { Tanim ="HAKAN AKSU", Id = "	302158"});
liste.Add(new DropItem() { Tanim ="HALİL KILIÇ", Id = "	301336"});
liste.Add(new DropItem() { Tanim ="HAMDİ ORUÇ", Id = "	302432"});
liste.Add(new DropItem() { Tanim ="HASAN EYLENCE", Id = "	301806"});
liste.Add(new DropItem() { Tanim ="HASAN MERMER", Id = "	302674"});
liste.Add(new DropItem() { Tanim ="HATEM CAN", Id = "	301782"});
liste.Add(new DropItem() { Tanim ="HATİCE GÜMÜŞ", Id = "	302420"});
liste.Add(new DropItem() { Tanim ="HÜSEYİN ERDİNÇ", Id = "	301861"});
liste.Add(new DropItem() { Tanim ="HÜSEYİN ERİ", Id = "	300622"});
liste.Add(new DropItem() { Tanim ="HÜSEYİN GÖKAŞAR", Id = "	300991"});
liste.Add(new DropItem() { Tanim ="HÜSEYİN KAYA", Id = "	302173"});
liste.Add(new DropItem() { Tanim ="HÜSEYİN MEMİŞOĞLU", Id = "	301936"});
liste.Add(new DropItem() { Tanim ="HÜSEYİN YILMAZ", Id = "	301798"});
liste.Add(new DropItem() { Tanim ="İBRAHİM ULUDAĞ", Id = "	302242"});
liste.Add(new DropItem() { Tanim ="İSA DEMİR", Id = "	302061"});
liste.Add(new DropItem() { Tanim ="İSMAİL FERİK", Id = "	301529"});
liste.Add(new DropItem() { Tanim ="İSMAİL KAZANCI", Id = "	302667"});
liste.Add(new DropItem() { Tanim ="İSMAİL ŞAHİN", Id = "	302675"});
liste.Add(new DropItem() { Tanim ="KADİR UFAT", Id = "	302043"});
liste.Add(new DropItem() { Tanim ="KEMAL AKBAŞ", Id = "	302307"});
liste.Add(new DropItem() { Tanim ="KIYMET ÖZTÜRK", Id = "	302419"});
liste.Add(new DropItem() { Tanim ="M. KUDRET BERRAK", Id = "	301903"});
liste.Add(new DropItem() { Tanim ="MEHMET AKIN", Id = "	300817"});
liste.Add(new DropItem() { Tanim ="MEHMET GÜRLÜLER", Id = "	300994"});
liste.Add(new DropItem() { Tanim ="MENEKŞE KESKİN", Id = "	302396"});
liste.Add(new DropItem() { Tanim ="MİTHAT KEŞANLI", Id = "	302016"});
liste.Add(new DropItem() { Tanim ="MURAT KARABULUT", Id = "	302118"});
liste.Add(new DropItem() { Tanim ="MUSTAFA GEDİK", Id = "	301515"});
liste.Add(new DropItem() { Tanim ="MUSTAFA UYSAL", Id = "	300645"});
liste.Add(new DropItem() { Tanim ="MUSTAFA ZENGİN", Id = "	302027"});
liste.Add(new DropItem() { Tanim ="MÜFİT OCAK", Id = "	300615"});
liste.Add(new DropItem() { Tanim ="MÜMİN ÇELENK", Id = "	300988"});
liste.Add(new DropItem() { Tanim ="NEJDET MUTLU", Id = "	301803"});
liste.Add(new DropItem() { Tanim ="NEŞE FERYAT", Id = "	302638"});
liste.Add(new DropItem() { Tanim ="NEVİN GÜDÜK", Id = "	302461"});
liste.Add(new DropItem() { Tanim ="NURCAN BALPINAR", Id = "	301734"});
liste.Add(new DropItem() { Tanim ="NURGÜL GÜNEL", Id = "	302533"});
liste.Add(new DropItem() { Tanim ="NURSEL SEVİNÇ", Id = "	302083"});
liste.Add(new DropItem() { Tanim ="ORHAN BULAÇ", Id = "	301053"});
liste.Add(new DropItem() { Tanim ="OSMAN DİNÇER", Id = "	301513"});
liste.Add(new DropItem() { Tanim ="ÖMER ŞEN", Id = "	301797"});
liste.Add(new DropItem() { Tanim ="PEMBE CİHAN", Id = "	302648"});
liste.Add(new DropItem() { Tanim ="RAMAZAN KİRAZ", Id = "	301514"});
liste.Add(new DropItem() { Tanim ="RAŞİT MUTLU", Id = "	302446"});
liste.Add(new DropItem() { Tanim ="RECEP ÇALIK", Id = "	301842"});
liste.Add(new DropItem() { Tanim ="SABRİ GEÇGEL", Id = "	301501"});
liste.Add(new DropItem() { Tanim ="SABRİ KAHRAMAN", Id = "	301805"});
liste.Add(new DropItem() { Tanim ="SALİHA KARAYEL", Id = "	302462"});
liste.Add(new DropItem() { Tanim ="SAMİ DİLE	", Id = "301992"});
liste.Add(new DropItem() { Tanim ="SEDAT AY", Id = "	300840"});
liste.Add(new DropItem() { Tanim ="SELAHATTİN YURTER	", Id = "301732"});
liste.Add(new DropItem() { Tanim ="SEMİHA ŞENOL	", Id = "302689"});
liste.Add(new DropItem() { Tanim ="SERKAN ÖZÇAY", Id = "	300879"});
liste.Add(new DropItem() { Tanim ="SERPİL DOĞU	", Id = "302476"});
liste.Add(new DropItem() { Tanim ="SEVGİ KARATEPE", Id = "	302496"});
liste.Add(new DropItem() { Tanim ="SEVİM DEDEOĞLU	", Id = "302397"});
liste.Add(new DropItem() { Tanim ="SEYRAN AYDIN	", Id = "300756"});
liste.Add(new DropItem() { Tanim ="SİNAN AKBULUT", Id = "	301804"});
liste.Add(new DropItem() { Tanim ="SÜLEYMAN ORTAÇ", Id = "	302402"});
liste.Add(new DropItem() { Tanim ="ŞABAN  DUNDAR", Id = "	302426"});
liste.Add(new DropItem() { Tanim ="ŞERİF AKBAŞ	", Id = "302671"});
liste.Add(new DropItem() { Tanim ="ŞEYHİSENAN IŞIK	", Id = "301521"});
liste.Add(new DropItem() { Tanim ="ŞEYMA ŞAHİN	", Id = "302672"});
liste.Add(new DropItem() { Tanim ="TANER SATIR	", Id = "301563"});
liste.Add(new DropItem() { Tanim ="ÜMİT TABAN	", Id = "301618"});
liste.Add(new DropItem() { Tanim ="VEDAT SAKİN", Id = "	302133"});
liste.Add(new DropItem() { Tanim ="YELİZ ALADAĞ	", Id = "302697"});
liste.Add(new DropItem() { Tanim ="YUSUF KARAAĞAÇ", Id = "	300833"});
liste.Add(new DropItem() { Tanim ="ZELİHA MİYANYEDİ", Id = "	302699"});
liste.Add(new DropItem() { Tanim ="ZEYNEP ÖZDEMİR	", Id = "302527"});

         */
        // GET: Home
        public string VerlileriDoldur(List<SrcnMakineHataBildirims> Hatalar)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            List<MorrisEntity> listem = new List<MorrisEntity>();

            var UniqueHatalar = Hatalar.Select(a => a.HataNedeni).Distinct().ToList();





            foreach (var itt in UniqueHatalar)
            {
                int uniqueHataKumulatif = 0;
                foreach (var item in Hatalar.Where(a => a.HataNedeni == itt).OrderBy(a => a.KayitTarihi).ToList())
                {
                    uniqueHataKumulatif++;
                    listem.Add(new MorrisEntity() { x1 = item.KayitTarihi.ToString(), x2 = item.HataNedeni.Replace(" ", ""), y1 = uniqueHataKumulatif.ToString(), y2 = item.MakinePozisyonNo.ToString() });
                }
            }






            //listem.Add(new MorrisEntity() { y = DateTime.Now.AddDays(-9).ToString(), a = "50", b = "50" });
            //listem.Add(new MorrisEntity() { y = DateTime.Now.AddDays(-10).ToString(), a = "75", b = "60" });
            //listem.Add(new MorrisEntity() { y = DateTime.Now.AddDays(-5).ToString(), a = "80", b = "65" });
            //listem.Add(new MorrisEntity() { y = DateTime.Now.AddDays(-2).ToString(), a = "90", b = "70" });

            //listem.Add(new MorrisEntity(){ y= "2020", a= "100", b= "75" });
            //listem.Add(new MorrisEntity(){ y= "2021", a= "115", b= "75" });
            //listem.Add(new MorrisEntity(){ y= "2022", a= "120", b= "85" });
            //listem.Add(new MorrisEntity(){ y= "2023", a= "145", b= "85" });
            //listem.Add(new MorrisEntity(){ y= "2024", a= "160", b= "95" });


            foreach (var item in listem)
            {
                sb.Append("{");
                sb.Append(string.Format("x1 :'{0}', x2:'{1}',y1:{2},y2:{3}", item.x1, item.x2, item.y1, item.y2));
                sb.Append("},");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();

        }

 
        public TefrikAnalizModel TefrikAnalizModeliGetir(string id)
        {
            var model = new TefrikAnalizModel();
            var liste = new List<DropItem> {new DropItem() {Tanim = "", Id = ""}};
            model.Makine = new Makine();
            var MakinelerToplu = _dbPoly.Makine.Where(a => a.OzelAlan14 != null && a.SBSonrasiCalismaSaati != null && a.SBSonrasiCalismaSaati != 5).OrderBy(a => a.MakineGrubu).ThenBy(a => a.OzelAlan14).ToList();
            /* SBSonrasiCalismaSaati = BirimId(Texture)
             
            ToplamCalismaSaati== salonId
            SayacOndalikSayisi = makineId
             */

            foreach (var item in MakinelerToplu)
            {
                if (model.MakineTurListe.Count(a => a.Id == item.ToplamCalismaSaati.ToString()) == 0)
                {
                    string TanimFormat = string.Format("{0} - {1}", item.MakineGrubu, item.OzelAlan14);
                    model.MakineTurListe.Add(new DropItem() { Id = item.ToplamCalismaSaati.ToString(), Tanim = TanimFormat });
                }

            }



            if (id != null)
            {
                int sayId = Convert.ToInt32(id);
                var Makineler = _dbPoly.Makine.Where(a => a.ToplamCalismaSaati == sayId).OrderBy(a => a.MakineAdi).ToList();
                if (Makineler.Any())
                {
                    var makine = Makineler[0];
                    model.Makine = makine;

                    model.MakineTur = new DropItem() { Id = makine.MakineTuru.Replace(" ", ""), Tanim = makine.MakineTuru };
                    var bugn = DateTime.Now.Date;
                    var vardiya = DateTime.Now;
                    DateTime baslangic;
                    DateTime bitis;
                    foreach (var mak in Makineler)
                    {
                        var idd = 0;
                        if (mak.SayacOndalikSayisi != null)
                        {
                            idd = Convert.ToInt32(mak.SayacOndalikSayisi);
                        }

                        var item = new TefrikMakineDurumModelItem { Makine = mak };

                        if (vardiya > bugn.AddHours(7) && bugn.AddHours(15) > vardiya)
                        {

                            //birinci Vardiya
                            baslangic = bugn.AddHours(7);
                            bitis = bugn.AddHours(15);
                        }
                        else
                        {
                            if (vardiya > bugn.AddHours(15) && bugn.AddHours(23) > vardiya)
                            {
                                baslangic = bugn.AddHours(15);
                                bitis = bugn.AddHours(23);

                                //ikinci vardiya
                            }
                            else
                            {
                                if (vardiya.Hour >= 23)
                                {
                                    baslangic = bugn.AddHours(23);
                                    bitis = bugn.AddHours(31);
                                    // saat 23.00-24.00 arasında
                                }
                                else
                                {
                                    baslangic = bugn.AddHours(-1);
                                    bitis = bugn.AddHours(7);
                                    // saat 00.00-07:arasında
                                }



                            }
                        }

                        DateTime oncekiVardiyaBaslangic = baslangic.AddHours(-8);
                        DateTime oncekiVardiyaBitis = baslangic;

                        item.HataSayisi = _dbPoly.SrcnMakineHataBildirims.Count(a => a.MakineId == idd && a.BildirimDurumuId == 0);

                        item.SuankiVardiyaHataSayisi = _dbPoly.SrcnMakineHataBildirims.Count(a => a.MakineId == idd && a.BildirimDurumuId == 0 && a.KayitTarihi > baslangic && a.KayitTarihi < bitis);

                        item.OncekiVardiyaHataSayisi = _dbPoly.SrcnMakineHataBildirims.Count(a => a.MakineId == idd && a.BildirimDurumuId == 0 && a.KayitTarihi > oncekiVardiyaBaslangic && a.KayitTarihi < oncekiVardiyaBitis);

                        item.BakilanMiktar = _dbPoly.SrcnMakineHataBildirims.Count(a => a.MakineId == idd && a.BildirimDurumuId == 1);
                        item.KalanMiktar = item.HataSayisi - item.BakilanMiktar;
                        model.TefrikMakineDurumModelItemlar.Add(item);
                    }
                }
            }

            model.TefrikMakineDurumModelItemlar =
                model.TefrikMakineDurumModelItemlar.OrderBy(a => a.KalanMiktar).ToList();
            return model;
        }
        // GET: Home
        public ActionResult Index()
        {
            foreach (var item in _dbPoly.SrcnMakineHataBildirims.Where(a=>a.StokAdi==null).ToList())
            {
                if (_dbPoly.ZzzSrcnRefakatKartNoStokAdlari.Any(a=>a.RefakatKartNo==item.RefakatKartNo))
                {
                    var itt = _dbPoly.ZzzSrcnRefakatKartNoStokAdlari.AsNoTracking().First(a => a.RefakatKartNo == item.RefakatKartNo);

                    item.StokAdi = itt.StokAdi;
                    item.StokKodu = itt.StokKodu;
                    _dbPoly.SaveChanges();


                }
            }
            TempData["Gizle"] = "gizle";
            return View(TefrikAnalizModeliGetir(null));
        }

        public ActionResult Bolum(int id)//Personel girişi kontrolü sağlanıyor
        {
            TempData["Gizle"] = "gizle";
            var model = TefrikAnalizModeliGetir(id.ToString());

            var zZzMakine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.BolumId == id);
            
            var SrcnKullanicilar = _dbPoly.SrcnKullanicis.AsNoTracking().Where(a => a.BolumId == zZzMakine.BolumId)
                .OrderBy(a => a.IsimSoyisim).ToList();
            model.SrcnKullanicilar = SrcnKullanicilar;
            foreach (var item in SrcnKullanicilar.Where(a => a.GrupId != null && a.BolumId==zZzMakine.BolumId).OrderBy(a=>a.GrupId).ToList())
            {
                if (model.GrupPersonelleri.Count(a=>a==item.GrupId)==0)
                {
                    int grupId = Convert.ToInt32(item.GrupId);
                    model.TefrikGrupPersonelModeller.Add(new TefrikGrupPersonelModel()
                    {
                        DropPersoneller = new SelectList(SrcnKullanicilar.Where(a=>a.GrupId== grupId).OrderBy(a=>a.IsimSoyisim),"KullaniciId", "IsimSoyisim"),
                        GrupNo = grupId
                    });
                    model.GrupPersonelleri.Add(grupId);
                }
            }
            model.GrupPersonelleri.Sort();
            return View(model);
        }

        [HttpPost]
        public ActionResult PersonelMakineHataIncele(TefrikAnalizModel model)
        {
            

            var item = model.TefrikGrupPersonelModeller.First(a => a.GrupNo == model.SecilenGrupNo);
            var srcnKullanici = _dbPoly.SrcnKullanicis.Find(item.SrcnKulaniciId);
            var makine = _dbPoly.Makine.First(a => a.SayacOndalikSayisi == model.MakineId);
            if (srcnKullanici.Sifre.ToUpper()!=item.Sifre.ToUpper())
            {
                TempDataOlustur("Personel Şifresi Hatalıdır",false);
                return RedirectToAction("Bolum", "Home", new {id = makine.SBSonrasiCalismaSaati});
            }
         
       
            var sessionModel = new TefrikGrupPersonelModel();
            sessionModel.SrcnKullanici = srcnKullanici;
            sessionModel.Makine = makine;

            Session["makHataModel"] = sessionModel;
            return RedirectToAction("MakineHatalari");
        }

        public ActionResult MakineHatalari()
        {
            if (Session["makHataModel"]==null)
            {
                return RedirectToAction("Index","Home");
            }

            var araModel = (TefrikGrupPersonelModel) Session["makHataModel"];

            TempData["Gizle"] = "gizle";

            var makine = araModel.Makine;

            string idd = null;

            if (makine.ToplamCalismaSaati != null)
            {
                idd = makine.ToplamCalismaSaati.ToString();
            }
            var model = TefrikAnalizModeliGetir(idd);
            model.MakineHataBildirimleri = _dbPoly.SrcnMakineHataBildirims.Where(a => a.MakineId == makine.SayacOndalikSayisi && a.BildirimDurumuId == 0).OrderBy(a => a.MakinePozisyonNo).ToList();
            model.Makine = makine;
            foreach (var item in model.MakineHataBildirimleri.Where(a => a.PartiNo == null).ToList())
            {
                var editItem = _dbPoly.SrcnMakineHataBildirims.Find(item.MakineHataBildirimId);
                if (_dbPoly.ZzzSrcnHataliTefrikMakineleri.Any(a =>
                    a.RefakatKartNo == editItem.RefakatKartNo && a.UretimSiraNo == editItem.UretimSiraNo &&
                    a.HSiraNo == editItem.HataSiraNo && a.SayacOndalikSayisi == editItem.MakineId && a.Pozisyon == editItem.MakinePozisyonNo))
                {
                    editItem.PartiNo = _dbPoly.ZzzSrcnHataliTefrikMakineleri.First(a =>
                        a.RefakatKartNo == editItem.RefakatKartNo && a.UretimSiraNo == editItem.UretimSiraNo &&
                        a.HSiraNo == editItem.HataSiraNo && a.SayacOndalikSayisi == editItem.MakineId && a.Pozisyon == editItem.MakinePozisyonNo).PartiNo;
                    _dbPoly.SaveChanges();
                }

            }

        
            model.Kullanicilar = new SelectList(_dbPoly.Kullanici.Where(a=>a.OnaylayanKullaniciKodu==makine.SBSonrasiCalismaSaati.ToString()).OrderBy(a => a.KullaniciAdi).ToList(), "KullaniciKodu", "KullaniciAdi");

            model.MakineHataBildirimleri = model.MakineHataBildirimleri.OrderByDescending(a => a.KayitTarihi)
                .OrderBy(a => a.MakinePozisyonNo).ToList();

            model.SrcnKullanici = araModel.SrcnKullanici;
            return View(model);
        }

        [HttpPost]
        public ActionResult MakineHataGoruldu(TefrikAnalizModel model)
        {
            int? id = model.Makine.SayacOndalikSayisi;

            var girenKullanici = _dbPoly.SrcnKullanicis.Find(model.SrcnKullanici.KullaniciId);
            //if (_dbPoly.Kullanici.Count(a => a.KullaniciKodu == model.KullaniciKodu) == 0)
            //{
            //    TempDataOlustur("Kullanıcı Seçiniz", false);
            //    return RedirectToAction("MakineHatalari", "Home", new { id = model.Makine.SayacOndalikSayisi });
            //}

            //girenKullanici = _dbPoly.Kullanici.First(a => a.KullaniciKodu == model.KullaniciKodu);
            //if (girenKullanici.Sifre.ToUpper() != model.Sifre.ToUpper())
            //{
            //    TempDataOlustur("Seçilen Kullanici İçin Girilen Şifre Hatalıdır", false);
            //    return RedirectToAction("MakineHatalari", "Home", new { id = model.Makine.SayacOndalikSayisi });
            //}
      
            var makine = _dbPoly.Makine.First(a => a.SayacOndalikSayisi == id);
            var makineHatalar = model.MakineHataBildirimleri.Where(a => a.SeciliMi).ToList();
            foreach (var itt in makineHatalar)
            {
                var item = _dbPoly.SrcnMakineHataBildirims.Find(itt.MakineHataBildirimId);
                item.BildirimDurumuId = 1;
                item.BildirimDurumuAdi = "İncelendi";
                item.BildirimIncelemeTarihi = DateTime.Now;
                item.BildirimSonlandirmaTarihi = DateTime.Now;
                item.SonGuncellemeYapanPersonelNo = girenKullanici.KullaniciId.ToString();
                item.SonGuncellemeYapanPersonelAdi = girenKullanici.IsimSoyisim;
                _dbPoly.SaveChanges();
                _dbPoly.SrcnMakineHataBildirimHarekets.Add(new SrcnMakineHataBildirimHarekets()
                {
                    MakineHataBildirimId = item.MakineHataBildirimId,
                    HareketAdi = "İncelendi",
                    PersonelAdi = girenKullanici.IsimSoyisim,
                    PersonelId = girenKullanici.KullaniciId.ToString(),
                    Tarih = DateTime.Now,
                    HareketTipi = 1
                });
                _dbPoly.SaveChanges();
            }

            TempDataOlustur("İnceleme işlemi yapılmıştır", true);
            return RedirectToAction("Bolum", "Home", new { id = makine.ToplamCalismaSaati });
        }
    }
}