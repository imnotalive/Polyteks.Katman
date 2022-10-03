using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.Has.Models;
using Polyteks.Katman.TefrikBildirim.Models;

namespace Polyteks.Katman.Has.Controllers
{
    /*
     this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            */
    public class MorrisEntity
    {
        public string x1 { get; set; }
        public string y1 { get; set; }
        public string x2 { get; set; }
        public string y2 { get; set; }
    }

    public class HomeController : BaseController
    {
        public void GunlukImalatGuncelle()
        {
            var liste = _dbPoly.SrcnGunlukImalatDosyas
                .Where(a => a.ToplamBobinSayisi == null || a.ToplamBobinSayisi == 0).ToList();
            var silListe = new List<SrcnGunlukImalatDosyas>();

            foreach (var item in liste)
            {
                if (_dbPoly.SrcnLabAnalizItems.Count(a=>a.BagliOlduguDosyaId==item.GunlukImalatDosyaId && a.DosyaTipi==1)==0)
                {
                    if (_dbPoly.SrcnLabAnalizs.Count(a => a.BagliOlduguDosyaId == item.GunlukImalatDosyaId && a.DosyaTipi == 1)==0)
                    {
                        silListe.Add(item);
                    }
                    else
                    {
                        var arliste = _dbPoly.SrcnLabAnalizs
                            .Where(a => a.BagliOlduguDosyaId == item.GunlukImalatDosyaId && a.DosyaTipi == 1)
                            .Select(a => a.LabAnalizId).ToList();
                        var labAnalizItemListe = _dbPoly.SrcnLabAnalizItems
                            .Where(a => arliste.Any(b => b == a.LabAnalizId)).ToList();
                        item.ToplamBobinSayisi = labAnalizItemListe.Count;
                        _dbPoly.SaveChanges();
                    }
                }
                else
                {
                    item.ToplamBobinSayisi = _dbPoly.SrcnLabAnalizItems.Count(a =>
                        a.BagliOlduguDosyaId == item.GunlukImalatDosyaId && a.DosyaTipi == 1);
                    _dbPoly.SaveChanges();
                }
            }

            _dbPoly.SrcnGunlukImalatDosyas.RemoveRange(silListe);
            _dbPoly.SaveChanges();
        }
        public void DenyeEkle()
        {
            int denye = 170;
            for (int i = 21; i < 50; i++)
            {
                denye += 10;
                _dbPoly.SrcnIplikOznitelikTipis.Add(new SrcnIplikOznitelikTipis()
                {
                    IplikOzNitelikKategoriTipi = 2,
                    IplikOzNitelikKategoriTipiAdi = "Denye",
                    IplikOzNitelikTipi = i,
                    IplikOzNitelikTipiAdi = denye.ToString()
                });
                _dbPoly.SaveChanges();

            }
        }
        public void OtomatikPartiKapat()
        {
            var tarih = DateTime.Now.AddDays(-5);
            var liste = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Where(a => a.PartiSonuTakipHareketTipi == 2 && a.PlanlamaTermini < tarih && a.PartiSonuTakipId != null && a.KayitTarihi < tarih).Select(a => a.PartiSonuTakipId).Distinct().ToList();


            if (liste.Any())
            {
                var bilgiOnayıBekleyenPartiler = _dbPoly.SrcnPartiSonuTakips.Where(a => liste.Any(b => b == a.PartiSonuTakipId)).ToList();


                var refListe = new List<RefakatKarti>();
                foreach (var item in bilgiOnayıBekleyenPartiler)
                {
                    if (_dbPoly.RefakatKarti.Any(a => a.RefakatNo == item.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemBitisTarihi == null && a.IslemNo != "900"))
                    {
                        var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == item.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemBitisTarihi == null && a.IslemNo != "900");
                        refKart.IslemBitisTarihi = DateTime.Now;
                        _dbPoly.SaveChanges();
                        refListe.Add(refKart);
                        var aradurum = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(8);
                        item.PartiSonuTakipHareketTipi = aradurum.PartiSonuTakipHareketTipi;
                        item.PartiSonuTakipHareketAdi = aradurum.PartiSonuTakipHareketAdi;
                        if (item.PartiKapatmaTarihi == null)
                        {
                            item.PartiKapatmaTarihi = DateTime.Now;
                        }

                        _dbPoly.SaveChanges();
                        var araListe = _dbPoly.SrcnPartiSonuTakipBilgiOnays
                            .Where(a => a.OnayapanKulId == 0 && a.OnayTarihi == null && a.PartiSonuTakipId == item.PartiSonuTakipId).Select(a => a.PartiSonuTakipBilgiId).ToList();
                        foreach (var i in araListe)
                        {
                            var itt = _dbPoly.SrcnPartiSonuTakipBilgiOnays.Find(i);
                            itt.OnayTarihi = DateTime.Now;
                            var kul = _dbPoly.SrcnKullanicis.AsNoTracking().First(a => a.KullaniciId == 636);
                            itt.OnayapanKulId = kul.KullaniciId;
                            itt.OnayapanKulAdi = kul.IsimSoyisim;
                            _dbPoly.SaveChanges();
                        }
                    }


                }

                if (refListe.Any())
                {
                    var mailIcerik = PartiSonuOzetTabloOlustur(refListe);


                    GenelDurumMailGonder("kvural@polyteks.com.tr", new List<string>(), mailIcerik, "Guncellenenler");
                }
            }

        }

        public void PartiNoKontrol()
        {


            var liste = _dbPoly.ZzzSrcnYeniPartiKlasorListes.ToList();

            foreach (var i in liste)
            {

                if (_dbPoly.SrcnPartiAnaKlasors.Count(a => a.PartiAdi.Trim().ToUpper() == i.PartiNo.Trim().ToUpper()) == 0)
                {
                    _dbPoly.SrcnPartiAnaKlasors.Add(new SrcnPartiAnaKlasors()
                    {
                        PartiAdi = i.PartiNo.Trim(),
                        OlusturmaTarihi = DateTime.Now,
                        BirimId = i.BirimId,
                        BirimAdi = i.BirimAdi
                    });
                    _dbPoly.SaveChanges();
                }

            }
            var aa = _dbPoly.SrcnPartiAnaKlasors.GroupBy(i => i.PartiAdi).Where(g => g.Count() > 1).Select(a => a.Key).ToList();

            var silListe = new List<SrcnPartiAnaKlasors>();
            foreach (var item in aa)
            {
                if (_dbPoly.SrcnPartiAnaKlasors.Count(a => a.PartiAdi == item) > 1)
                {
                    var araListe = _dbPoly.SrcnPartiAnaKlasors.Where(a => a.PartiAdi == item).ToList();

                    if (araListe.Count() > 1)
                    {
                        int say = 0;
                        foreach (var itt in araListe)
                        {
                            say++;
                            if (say != 1)
                            {
                                silListe.Add(itt);
                            }

                        }
                    }
                }


            }

            _dbPoly.SrcnPartiAnaKlasors.RemoveRange(silListe);
            _dbPoly.SaveChanges();

        }

        public void MailBildirimGrupGuncelle()
        {
            foreach (var item in _dbPoly.SrcnFabrikaBirims.AsNoTracking().ToList())
            {
                if (_dbPoly.SrcnMailBildirimGrups.Count(a => a.MailBildirimGrupId == item.BirimId) == 0)
                {
                    _dbPoly.SrcnMailBildirimGrups.Add(new SrcnMailBildirimGrups()
                    {
                        MailBildirimGrupId = item.BirimId,
                        MailBildirimGrupAdi = item.BirimAdi
                    });
                    _dbPoly.SaveChanges();
                }
            }
        }
        public void CariyiAnalizeBagla()
        {
            var analizler = _dbPoly.SrcnLabAnalizs.Where(a => a.AnalizTipi == 2 || a.AnalizTipi == 3)
                .Where(a => a.CariAdi == null && a.BagliOlduguDosyaId != 0).ToList();
            foreach (var item in analizler)
            {
                if (item.AnalizTipi == 3)
                {
                    item.CariAdi = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(item.BagliOlduguDosyaId).CariAdi;
                    _dbPoly.SaveChanges();
                }
                else
                {
                    item.CariAdi = _dbPoly.SrcnMusteriSikayetDosyas.Find(item.BagliOlduguDosyaId).CariAdi;
                    _dbPoly.SaveChanges();
                }
            }
        }
        public void personelEkle()
        {
            var liste = new List<DropItem>();


            liste.Add(new DropItem() { Tanim = "OSMAN OZYURT", Id = "301455" });
            liste.Add(new DropItem() { Tanim = "GOKHAN DUZGUN", Id = "302159" });
            //liste.Add(new DropItem() { Tanim = "HATİCE KEYİK", Id = "302839 " });
            //liste.Add(new DropItem() { Tanim = "HÜLYA AVCI", Id = "302856 " });
            //liste.Add(new DropItem() { Tanim = "KADİR DÜNDAR", Id = "302854 " });
            //liste.Add(new DropItem() { Tanim = "MERT ŞEKUR", Id = "302814 " });
            //liste.Add(new DropItem() { Tanim = "MUHAMMET KAYA", Id = "302843 " });
            //liste.Add(new DropItem() { Tanim = "MÜMİNE YILMAZ", Id = "302811 " });
            //liste.Add(new DropItem() { Tanim = "ONUR ALEVLİ", Id = "302855 " });
            //liste.Add(new DropItem() { Tanim = "PAPUR CANLI", Id = "302753 " });
            //liste.Add(new DropItem() { Tanim = "SABRİYE FİLİZ", Id = "302834 " });
            //liste.Add(new DropItem() { Tanim = "SALİH ÇULHA", Id = "302877 " });
            //liste.Add(new DropItem() { Tanim = "SALİHA ALTIN", Id = "302836 " });
            //liste.Add(new DropItem() { Tanim = "SEBİL COPÜR", Id = "302862 " });
            //liste.Add(new DropItem() { Tanim = "SELÇUK DİŞBUDAK", Id = "302857 " });
            //liste.Add(new DropItem() { Tanim = "SELÇUK KIVANÇ", Id = "302871 " });
            //liste.Add(new DropItem() { Tanim = "SÜMEYRA SÜZENER", Id = "302823 " });
            //liste.Add(new DropItem() { Tanim = "ZEYNİ ÖZKUL", Id = "302813 " });


            int baslangic = 878;

            foreach (var dropItem in liste)
            {
                baslangic++;
                var item = new Personel()
                {
                    PersonelNo = "P" + baslangic.ToString(),
                    PersonelAdi = dropItem.Tanim,
                    EvTelefonu = dropItem.Id.Replace(" ", "").Replace("30", ""),
                    NufusaKayitliOlduguIl = "1",
                    IsyeriKodu = "0"

                };
                _dbPoly.Personel.Add(item);
                _dbPoly.SaveChanges();
            }
        }
        public void KullaniciKontrolEt()
        {
            var potaKullanicilar = _dbPoly.Kullanici.OrderBy(a => a.KullaniciAdi).ToList();

            foreach (var kullanici in potaKullanicilar)
            {
                if (_dbPoly.SrcnKullanicis.Count(a => a.KullaniciKodu == Kullanici.KullaniciKodu.Replace(" ", "")) == 0)
                {
                    _dbPoly.SrcnKullanicis.Add(new SrcnKullanicis()
                    {
                        KullaniciKodu = Kullanici.KullaniciKodu.Replace(" ", ""),
                        IsimSoyisim = Kullanici.IsimSoyisim,
                        Sifre = kullanici.Sifre.Replace(" ", "")


                    });
                    _dbPoly.SaveChanges();
                }
            }
        }
        public void PersoneliKullaniciyaBagla()
        {
            var personeller = _dbPoly.Personel.AsNoTracking().ToList();

            foreach (var personel in personeller)
            {
                //if (personeller.Count(a=>a.KullaniciKodu==personel.KullaniciKodu)==1)
                //{
                //if (_dbPoly.SrcnKullanicis.Any(a=>a.KullaniciKodu==personel.KullaniciKodu))
                //{
                //    var editKullanici = _dbPoly.SrcnKullanicis.First(a => a.KullaniciKodu == personel.KullaniciKodu);

                //    editKullanici.PersonelKodu = personel.PersonelNo.Replace(" ", "");
                //    if (personel.Resim!=null)
                //    {
                //        editKullanici.Resim = personel.Resim;
                //    }

                //    _dbPoly.SaveChanges();
                //}
                //else
                //{
                if (_dbPoly.SrcnKullanicis.Count(a => a.PersonelKodu == personel.PersonelNo) == 0)
                {
                    string evTelefonu = null;
                    if (personel.EvTelefonu != null)
                    {
                        evTelefonu = personel.EvTelefonu.Replace(" ", "");
                    }
                    _dbPoly.SrcnKullanicis.Add(new SrcnKullanicis()
                    {
                        KullaniciKodu = null,
                        Resim = personel.Resim,
                        IsimSoyisim = personel.PersonelAdi,
                        Sifre = evTelefonu,
                        PersonelKodu = personel.PersonelNo.Replace(" ", "")
                    });
                    _dbPoly.SaveChanges();
                }
                //}
                //}
            }
        }
        public void personelResimGuncelle()
        {
            var liste = _dbPoly.Personel.Where(a => a.Resim != null).ToList();

            foreach (var personel in liste)
            {
                if (_dbPoly.SrcnKullanicis.Any(a => a.PersonelKodu == personel.PersonelNo))
                {
                    var editPers = _dbPoly.SrcnKullanicis.First(a => a.PersonelKodu == personel.PersonelNo);
                    editPers.Resim = personel.Resim;
                    _dbPoly.SaveChanges();
                }
            }
        }

        public void TumAnalizGruplarinaEkle()
        {
            var labGruplari = _dbPoly.SrcnLabGrups.Where(a => a.AnalizTipi == 0).Select(a => a.LabGrupId).ToList();

            foreach (var i in labGruplari)
            {
                var labgrupAnalizler = _dbPoly.SrcnLabGrupAnalizCesits.AsNoTracking().Where(a => a.LabGrupId == i).ToList();

                if (labgrupAnalizler.Any())
                {
                    if (labgrupAnalizler.Count(a => a.LabAnalizCesitId == 46) == 0)
                    {
                        var labGrup = _dbPoly.SrcnLabGrups.Find(i);
                        var analizCesit = _dbPoly.SrcnLabAnalizCesits.Find(46);
                        _dbPoly.SrcnLabGrupAnalizCesits.Add(new SrcnLabGrupAnalizCesits()
                        {
                            LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi,
                            LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                            LabGrupId = labGrup.LabGrupId,
                            DegerTipi = 1,
                            LabGrupAdi = labGrup.LabGrupAdi,
                            SeciliMi = false
                        });
                        _dbPoly.SaveChanges();
                    }
                }
            }
        }
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


            return model;
        }
        public void AnalizSonucEngGuncelle()
        {
            var liste = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Where(a => a.LabAnalizCesitAdiEng == null).ToList();

            foreach (var item in liste)
            {
                if (_dbPoly.SrcnLabAnalizCesits.Any(a => a.LabAnalizCesitId == item.LabAnalizCesitId))
                {
                    var cesit = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().First(a => a.LabAnalizCesitId == item.LabAnalizCesitId);
                    item.LabAnalizCesitAdiEng = cesit.LabAnalizCesitAdiEng;
                    _dbPoly.SaveChanges();

                }

            }
        }
        // GET: Home
        public ActionResult Index()
        {
            //DenyeEkle();
            //personelEkle();
            //PersoneliKullaniciyaBagla();
            PartiNoKontrol();
            OtomatikPartiKapat();

            /*
            var personel = _dbPoly.Personel.First(a => a.PersonelNo.ToUpper() == "P021");
            var kullanici = _dbPoly.SrcnKullanicis.Find(27);
            kullanici.Resim = personel.Resim;
            _dbPoly.SaveChanges();
            */
            //GunlukImalatGuncelle();
            bool MailVarMi = true;
            var model = new AnasayfaModel();
            if (Kullanici.EmailAdres != null)
            {
                if (Kullanici.EmailAdres.Length < 2)
                {
                    MailVarMi = false;
                }
            }
            else
            {
                MailVarMi = false;
            }
            if (MailVarMi == false)
            {
                model.BildirimItems.Add(new HeaderUstBildirimItem() { Aciklama = "E-mail Adresiniz Tanımlı Değildir. Tanımlamak için tıklayınız", ActionAdi = "Duzenle", ControllerAdi = "KisiselBilgiler" });
            }



            /* 
             var controllers = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t)).OrderBy(a => a.Name).Select(t => t);
             foreach (Type controller in controllers)
             {
                 var actions = controller.GetMethods().Where(m => m.IsPublic && (m.ReturnType == typeof(ActionResult))).OrderBy(a => a.Name).Select(a=>a.Name).Distinct().ToList();
                 foreach (var actionName in actions)
                 {

                     model.BildirimItems.Add(new HeaderUstBildirimItem() { Aciklama = string.Format("{0} / {1}", controller.Name.Replace("Controller",""), actionName) });
                 }
             }
             */



            return View(model);
        }
        public ActionResult Bolum(/*string id*/int id)
        {
            //TempData["Gizle"] = "gizle";
            //var model = TefrikAnalizModeliGetir(id);


            //var zZzMakine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.MakineId == model.Makine.SBSonrasiCalismaSaati);

            //var SrcnKullanicilar = _dbPoly.SrcnKullanicis.AsNoTracking().Where(a => a.BolumId == zZzMakine.BolumId)
            //    .OrderBy(a => a.IsimSoyisim).ToList();
            //model.SrcnKullanicilar = SrcnKullanicilar;

            //var aa = SrcnKullanicilar.Where(a => a.GrupId != null).Select(a => a.GrupId).Distinct();
            //if (aa.Any())
            //{
            //    model.GrupPersonelleri = (List<int>)aa.OrderBy(a => a.HasValue);
            //}


            //return View(model);
            TempData["Gizle"] = "gizle";
            var model = TefrikAnalizModeliGetir(id.ToString());

            var zZzMakine = _dbPoly.ZzzSrcnMakineIdli.First(a => a.BolumId == id);

            var SrcnKullanicilar = _dbPoly.SrcnKullanicis.AsNoTracking().Where(a => a.BolumId == zZzMakine.BolumId)
                .OrderBy(a => a.IsimSoyisim).ToList();
            model.SrcnKullanicilar = SrcnKullanicilar;
            foreach (var item in SrcnKullanicilar.Where(a => a.GrupId != null && a.BolumId == zZzMakine.BolumId).OrderBy(a => a.GrupId).ToList())
            {
                if (model.GrupPersonelleri.Count(a => a == item.GrupId) == 0)
                {
                    int grupId = Convert.ToInt32(item.GrupId);
                    model.TefrikGrupPersonelModeller.Add(new TefrikGrupPersonelModel()
                    {
                        DropPersoneller = new SelectList(SrcnKullanicilar.Where(a => a.GrupId == grupId).OrderBy(a => a.IsimSoyisim), "KullaniciId", "IsimSoyisim"),
                        GrupNo = grupId
                    });
                    model.GrupPersonelleri.Add(grupId);
                }
            }
            model.GrupPersonelleri.Sort();
            return View(model);
        }

        public ActionResult MakineHatalari(int id)
        {
            TempData["Gizle"] = "gizle";

            var makine = _dbPoly.Makine.First(a => a.SayacOndalikSayisi == id);

            string idd = null;

            if (makine.ToplamCalismaSaati != null)
            {
                idd = makine.ToplamCalismaSaati.ToString();
            }
            var model = TefrikAnalizModeliGetir(idd);
            model.MakineHataBildirimleri = _dbPoly.SrcnMakineHataBildirims.Where(a => a.MakineId == makine.SayacOndalikSayisi).OrderBy(a => a.MakinePozisyonNo).ToList();
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
            model.MakineHataBildirimleri = model.MakineHataBildirimleri.OrderByDescending(a => a.KayitTarihi)
                .OrderBy(a => a.MakinePozisyonNo).ToList();


            return View(model);
        }
        public ActionResult MakineHataGoruldu(int id)
        {
            var makineHatalar = _dbPoly.SrcnMakineHataBildirims.Where(a => a.MakineId == id).ToList();
            var makine = _dbPoly.Makine.First(a => a.SayacOndalikSayisi == id);
            foreach (var item in makineHatalar)
            {

                item.BildirimDurumuId = 1;
                item.BildirimDurumuAdi = "İncelendi";
                item.BildirimIncelemeTarihi = DateTime.Now;
                item.BildirimSonlandirmaTarihi = DateTime.Now;
                item.SonGuncellemeYapanPersonelNo = Kullanici.KullaniciId.ToString();
                item.SonGuncellemeYapanPersonelAdi = Kullanici.IsimSoyisim;
                _dbPoly.SaveChanges();
                _dbPoly.SrcnMakineHataBildirimHarekets.Add(new SrcnMakineHataBildirimHarekets()
                {
                    MakineHataBildirimId = item.MakineHataBildirimId,
                    HareketAdi = "İncelendi",
                    PersonelAdi = Kullanici.IsimSoyisim,
                    PersonelId = Kullanici.PersonelKodu,
                    Tarih = DateTime.Now
                });
                _dbPoly.SaveChanges();
            }

            TempDataOlustur("İnceleme işlemi yapılmıştır", true);
            return RedirectToAction("Bolum", "Home", new { id = makine.ToplamCalismaSaati });
        }

        [HttpPost]
        public ActionResult MakineHatalari(TefrikAnalizModel model)
        {
            var makine = model.Makine;
            var makinehatalari = model.MakineHataBildirimleri.Where(a => a.SeciliMi);

            return View();
        }
    }
}