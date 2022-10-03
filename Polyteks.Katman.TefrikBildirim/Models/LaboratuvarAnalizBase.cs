using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class LaboratuvarAnalizBase
    {
        public LaboratuvarAnalizBase()
        {
            IstekTarihi = DateTime.Now.ToString("dd.MM.yyyy");

            LabAnaliz = new SrcnLabAnalizs();
            Kullanici = new SrcnKullanicis();
            LabGrup = new SrcnLabGrups();
            Resim = new SrcnResims();

            LabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
            LabGrubuAnalizleri = new List<SrcnLabGrupAnalizCesits>();
            SrcnLabGruplari = new List<SrcnLabGrups>();
            LabAnalizleri = new List<SrcnLabAnalizs>();
            LabAnalizDetayModeller = new List<LabAnalizDetayModel>();
            AnalizHareketleri = new List<SrcnLabAnalizHarekets>();
            DosyaResimleri = new List<SrcnResims>();
            DosyaHareketleri = new List<SrcnDosyaHarekets>();
            DosyaHareket = new SrcnDosyaHarekets();
            LabAnalizItemAnalizCesitSonuclari = new List<SrcnLabAnalizItemAnalizCesitSonucs>();

            LabAnalizHareketleri = new List<SrcnLabAnalizHarekets>();
            LaboratuvarAnalizItemSonuclarModeller = new List<LaboratuvarAnalizItemSonuclarModel>();
            LabAnalizLoglari = new List<SrcnLabAnalizLogs>();
            Birimler = new List<SrcnFabrikaBirims>();
        }

        public List<SrcnFabrikaBirims> Birimler { get; set; }
        public SrcnKullanicis Kullanici { get; set; }
        public string IstekTarihi { get; set; }
        public SrcnLabAnalizs LabAnaliz { get; set; }
        public SrcnLabGrups LabGrup { get; set; }
        public List<SrcnLabAnalizHarekets> LabAnalizHareketleri { get; set; }
        public List<SrcnLabAnalizHarekets> AnalizHareketleri { get; set; }
        public List<SrcnLabAnalizs> LabAnalizleri { get; set; }
        public List<SrcnLabAnalizCesits> LabAnalizCesitleri { get; set; }
        public List<SrcnLabGrupAnalizCesits> LabGrubuAnalizleri { get; set; }
        public List<SrcnLabGrups> SrcnLabGruplari { get; set; }
        public List<SrcnLabAnalizItems> LabAnalizItems { get; set; }
        public SrcnResims Resim { get; set; }
        public List<SrcnResims> DosyaResimleri { get; set; }
        public List<LabAnalizDetayModel> LabAnalizDetayModeller { get; set; }
        public List<SrcnDosyaHarekets> DosyaHareketleri { get; set; }
        public SrcnDosyaHarekets DosyaHareket { get; set; }
        public List<SrcnLabAnalizLogs> LabAnalizLoglari { get; set; }
        public List<SrcnLabAnalizItemAnalizCesitSonucs> LabAnalizItemAnalizCesitSonuclari { get; set; }

        public bool PosttaSorunVarMi { get; set; }

        public List<LaboratuvarAnalizItemSonuclarModel> LaboratuvarAnalizItemSonuclarModeller { get; set; }
    }
}