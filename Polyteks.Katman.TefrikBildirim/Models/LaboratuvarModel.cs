using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class LaboratuvarModel : LaboratuvarAnalizBase
    {
        public LaboratuvarModel()
        {
            LabAnalizCesit = new SrcnLabAnalizCesits();
            LabGrupAnalizCesit = new SrcnLabGrupAnalizCesits();
            MalzemeLabAnalizModeller = new List<MalzemeLabAnalizModel>();
            YardimciTesisKategoriModeller = new List<YardimciTesisKategoriModel>();
            LaboratuvarIsModeller = new List<LaboratuvarIsModel>();
            LabAnalizDurumu = new SrcnLabAnalizDurumus();
            BobinIplikModelItemlar = new List<BobinIplikModelItem>();
            LabAnalizDurumlari = new List<SrcnLabAnalizDurumus>();
            PlanlananTarih = DateTime.Now.ToString("dd.MM.yyyy");
            Makine = new Makine();
            MusteriSikayetDosya = new SrcnMusteriSikayetDosyas();
            LabSatirItemlar = new List<LabSatirItem>();
            Theadler = new List<string>();
            GunlukImalatDosya = new SrcnGunlukImalatDosyas();
            GunlukImalatDosyalar = new List<SrcnGunlukImalatDosyas>();
            DosyaLabAnalizTablolar = new List<DosyaLabAnalizTablo>();
            NumuneYapilabilirlikDosyalar = new List<SrcnNumuneYapilabilirlikDosyas>();
            AnalizDurumlari = new List<SrcnLabAnalizDurumus>();
            Birim = new SrcnFabrikaBirims();
            DosyaDurumlari = new List<SrcnDosyaDurums>();
            DosyaDurumu = new SrcnDosyaDurums();
            DosyaNumuneLabAnalizTablolar = new List<DosyaNumuneLabAnalizTablo>();
            MusteriSikayetDosyalar = new List<SrcnMusteriSikayetDosyas>();
            DosyaMusteriLabAnalizTablo = new List<DosyaMusteriLabAnalizTablo>();
            MusteriSikayetDosyaSikayetGrups = new List<SrcnMusteriSikayetDosyaSikayetGrups>();
            MusteriSikayetAnalizBaslik1 = new SrcnMusteriSikayetGrups();
            MusteriSikayetAnalizBaslik2 = new SrcnMusteriSikayetGrups();
            MusteriSikayetAnalizBaslik3 = new SrcnMusteriSikayetGrups();
            MusteriSikayetAnalizBaslik4 = new SrcnMusteriSikayetGrups();
            MusteriSikayetAnalizBaslik5 = new SrcnMusteriSikayetGrups();


        }

        public string PlanlananTarih { get; set; }
        public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik1 { get; set; }
        public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik2 { get; set; }
        public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik3 { get; set; }
        public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik4 { get; set; }
        public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik5 { get; set; }

        /// ///////////////////
        /// </summary>
        //public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik1 { get; set; }
        //public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik2 { get; set; }
        //public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik3 { get; set; }
        //public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik4 { get; set; }
        //public SrcnMusteriSikayetGrups MusteriSikayetAnalizBaslik5 { get; set; }
        public SrcnLabAnalizDurumus LabAnalizDurumu { get; set; }
        public SrcnLabAnalizCesits LabAnalizCesit { get; set; }
        public SrcnLabGrupAnalizCesits LabGrupAnalizCesit { get; set; }
        public int IsEmriNumarasiHazirlikDurumu { get; set; } // 0 seçim yapılmadı, 1- is emri var, 2- is emri yok
        public string UretimHattiId { get; set; }
       
        public string MakineId { get; set; }
        public Makine Makine { get; set; }
        public SelectList UretimHatlari { get; set; }
        public SelectList Makineler { get; set; }

        public SrcnFabrikaBirims Birim { get; set; }

        public List<SrcnDosyaDurums> DosyaDurumlari { get; set; }

        public SrcnDosyaDurums DosyaDurumu { get; set; }

        public List<SrcnLabAnalizDurumus> AnalizDurumlari { get; set; }
        public List<LaboratuvarIsModel> LaboratuvarIsModeller { get; set; }
        public List<MalzemeLabAnalizModel> MalzemeLabAnalizModeller { get; set; }
        public List<YardimciTesisKategoriModel> YardimciTesisKategoriModeller { get; set; }

        public SrcnNumuneYapilabilirlikDosyas NumuneYapilabilirlikDosya { get; set; }
        public SrcnMusteriSikayetDosyas MusteriSikayetDosya { get; set; }
        public List<SrcnMusteriSikayetDosyas> MusteriSikayetDosyalar { get; set; }
        public List<SrcnLabAnalizDurumus> LabAnalizDurumlari { get; set; }
        public List<BobinIplikModelItem> BobinIplikModelItemlar { get; set; }
        public List<LabSatirItem> LabSatirItemlar { get; set; }
        public List<string> Theadler { get; set; }
        public SrcnGunlukImalatDosyas GunlukImalatDosya { get; set; }
        public List<SrcnNumuneYapilabilirlikDosyas> NumuneYapilabilirlikDosyalar { get; set; }

        public List<SrcnGunlukImalatDosyas> GunlukImalatDosyalar { get; set; }
        public List<DosyaLabAnalizTablo> DosyaLabAnalizTablolar { get; set; }
        public List<DosyaNumuneLabAnalizTablo> DosyaNumuneLabAnalizTablolar { get; set; }

        public List<DosyaMusteriLabAnalizTablo> DosyaMusteriLabAnalizTablo { get; set; }

        public List<SrcnMusteriSikayetDosyaSikayetGrups> MusteriSikayetDosyaSikayetGrups { get; set; }

    }


    public class DosyaLabAnalizTablo
    {
        public DosyaLabAnalizTablo()
        {
            LabAnalizItemlar = new List<SrcnLabAnalizItems>();
            DosyaLabAnalizTabloSatirlar = new List<DosyaLabAnalizTabloSatir>();
       
        }

        public SrcnLabAnalizs LabAnaliz { get; set; }
        public ZzzSrcnPartiSonuTakipIzlenebilirlik PartiSonuTakipIzlenebilirlik { get; set; }
        public List<SrcnLabAnalizItems> LabAnalizItemlar { get; set; }
        public List<DosyaLabAnalizTabloSatir> DosyaLabAnalizTabloSatirlar { get; set; }
      
    }

    public class DosyaLabAnalizTabloSatir
    {
        public DosyaLabAnalizTabloSatir()
        {
            LabAnalizCesit = new SrcnLabAnalizCesits();
            LabAnalizItemAnalizCesitSonuclari = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
            SikayetlerBaslık = new List<SrcnMusteriSikayetGrups>();
        }
        public SrcnLabAnalizCesits LabAnalizCesit { get; set; }
        public List<SrcnLabAnalizItemAnalizCesitSonucs> LabAnalizItemAnalizCesitSonuclari { get; set; }
        public List<SrcnMusteriSikayetGrups> SikayetlerBaslık { get; set; }
    }

    public class LabSatirItem
    {
        public LabSatirItem()
        {
            LabAnalizItemAnalizCesitSonuclari = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
            LabAnalizCesit = new SrcnLabAnalizCesits();
            SikayetlerBaslık = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
        }
        public string LabAnalizCesitAdi { get; set; }
        public int LabAnalizCesitId { get; set; }
        public List<SrcnLabAnalizItemAnalizCesitSonucs> SikayetlerBaslık { get; set; }
        public SrcnLabAnalizCesits LabAnalizCesit { get; set; }
        public List<SrcnLabAnalizItemAnalizCesitSonucs> LabAnalizItemAnalizCesitSonuclari { get; set; }
        public List<SrcnLabAnalizItemAnalizCesitSonucs> MusteriSikayetBaslik { get; set; }
    }

    public class DosyaNumuneLabAnalizTablo
    {
        public DosyaNumuneLabAnalizTablo()
        {
            LabAnaliz = new SrcnLabAnalizs();
            AtkiTablo = new DosyaLabAnalizTablo();
            CozguTablo = new DosyaLabAnalizTablo();
        }

        public SrcnLabAnalizs LabAnaliz { get; set; }
        public DosyaLabAnalizTablo AtkiTablo { get; set; }
        public DosyaLabAnalizTablo CozguTablo { get; set; }

    }


    public class DosyaMusteriLabAnalizTablo
    {
        public DosyaMusteriLabAnalizTablo()
        {
            LabAnaliz = new SrcnLabAnalizs();
            AtkiTablo = new DosyaLabAnalizTablo();
            CozguTablo = new DosyaLabAnalizTablo();
        }

        public SrcnLabAnalizs LabAnaliz { get; set; }
        public DosyaLabAnalizTablo AtkiTablo { get; set; }
        public DosyaLabAnalizTablo CozguTablo { get; set; }


    }

}