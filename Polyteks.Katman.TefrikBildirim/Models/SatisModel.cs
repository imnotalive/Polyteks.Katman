using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class SatisModel : LaboratuvarAnalizBase
    {
        public SatisModel()
        {

            LabAnalizCesit = new SrcnLabAnalizCesits();
            LabGrupAnalizCesit = new SrcnLabGrupAnalizCesits();
            MusteriSikayetGrupItemModeller = new List<MusteriSikayetGrupItemModel>();
            NumuneYapilabilirlikDosyalar = new List<SrcnNumuneYapilabilirlikDosyas>();
            MusteriSikayetDosyalar = new List<SrcnMusteriSikayetDosyas>();
            MusteriSikayetDosya = new SrcnMusteriSikayetDosyas();
            NumuneYapilabilirlikDosya = new SrcnNumuneYapilabilirlikDosyas();
            LabAnalizeGondermeDurumu = 1;
            MusteriSikayetOlusturmaModeller = new List<MusteriSikayetOlusturmaModel>();
            NumuneDosyaTipi = new SrcnNumuneDosyaTipis();
            NumuneDosyaTipleri = new List<SrcnNumuneDosyaTipis>();
            DosyaDurumlari = new List<SrcnDosyaDurums>();
            DosyaDurumu = new SrcnDosyaDurums();
            DosyaMusteriLabAnalizTablolar = new List<DosyaMusteriLabAnalizTablo>();
        }
        public List<DosyaMusteriLabAnalizTablo> DosyaMusteriLabAnalizTablolar { get; set; }
        public List<SrcnDosyaDurums> DosyaDurumlari { get; set; }
        public SrcnDosyaDurums DosyaDurumu { get; set; }
        public List<MusteriSikayetOlusturmaModel> MusteriSikayetOlusturmaModeller { get; set; }
        public List<SrcnNumuneDosyaTipis> NumuneDosyaTipleri { get; set; }
        public SrcnNumuneDosyaTipis NumuneDosyaTipi { get; set; }
        public string DbTipi { get; set; }
        public string CariKodu { get; set; }
        public string StokKodu { get; set; }

        public int LabAnalizeGondermeDurumu { get; set; }
        public List<SrcnNumuneYapilabilirlikDosyas> NumuneYapilabilirlikDosyalar { get; set; }
        public List<SrcnMusteriSikayetDosyas> MusteriSikayetDosyalar { get; set; }

        public SrcnNumuneYapilabilirlikDosyas NumuneYapilabilirlikDosya { get; set; }
        public SrcnMusteriSikayetDosyas MusteriSikayetDosya { get; set; }
        public int RadioSecimId { get; set; }
        public SrcnLabAnalizCesits LabAnalizCesit { get; set; }

        public SrcnLabGrupAnalizCesits LabGrupAnalizCesit { get; set; }

        public SelectList GenelSelectList { get; set; }

        public SelectList CarilerPoly { get; set; }
        public SelectList CarilerTasd { get; set; }
        public SelectList CarilerPset { get; set; }
        public string CariKoduPoly { get; set; }
        public string CariKoduTasd { get; set; }
        public string CariKoduPset { get; set; }

        public List<MusteriSikayetGrupItemModel> MusteriSikayetGrupItemModeller { get; set; }




    }
    public class MusteriSikayetGrupItemModel
    {
        public MusteriSikayetGrupItemModel()
        {
            AnaGrup = new SrcnMusteriSikayetGrups();
            AltItemGruplar = new List<SrcnMusteriSikayetGrups>();
        }
        public SrcnMusteriSikayetGrups AnaGrup { get; set; }
        public List<SrcnMusteriSikayetGrups> AltItemGruplar { get; set; }
        public int RadioId { get; set; }

    }

    public class MusteriSikayetOlusturmaModel
    {
        public int Sira { get; set; }
        public bool SeciliMi { get; set; }
        public DateTime? SevkTarihi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string RefKartNo { get; set; }
        public string PartiNo { get; set; }
        public string CariAdi { get; set; }
        public decimal? SevkMiktari { get; set; }
        public string IrsalıyeNo { get; set; }
        public string CariNo { get; set; }
        public string StokKoduCizgili { get; set; }
        public string CariNoCizgili { get; set; }
    }
}