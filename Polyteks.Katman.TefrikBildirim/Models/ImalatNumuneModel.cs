using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class ImalatNumuneModel
    {
        public ImalatNumuneModel()
        {
            NumuneYapilabilirlikDosyalar = new List<SrcnNumuneYapilabilirlikDosyas>();
            NumuneYapilabilirlikDosya = new SrcnNumuneYapilabilirlikDosyas();
            DosyaDurumlari = new List<SrcnDosyaDurums>();
            Birimler = new List<SrcnFabrikaBirims>();
            DenemeDosyalari = new List<SrcnDenemeDosya>();
            DosyaDurumu = new SrcnDosyaDurums();
            DosyaNumuneLabAnalizTablolar = new List<DosyaNumuneLabAnalizTablo>();
            BirimDenemeAraModeller = new List<BirimDenemeAraModel>();
            Birim = new SrcnFabrikaBirims();
            Makine = new ZzzSrcnMakineIdli();
            Islem = new Islem();
            DenemeDosyalar = new List<SrcnDenemeDosya>();
            DenemeDosya = new SrcnDenemeDosya();
            DenemeDosyaItemlar = new List<SrcnDenemeDosyaItems>();
            DenemeDosyaItem = new SrcnDenemeDosyaItems();
            DosyaItemMakineParametreler = new List<SrcnDosyaItemMakineParametres>();
            LabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
        }
        public List<SrcnNumuneYapilabilirlikDosyas> NumuneYapilabilirlikDosyalar { get; set; }
        public SrcnNumuneYapilabilirlikDosyas NumuneYapilabilirlikDosya { get; set; }
        public ZzzSrcnMakineIdli Makine { get; set; }
        public Islem Islem { get; set; }
        public SrcnFabrikaBirims Birim { get; set; }
        public SrcnDenemeDosyaItems DenemeDosyaItem { get; set; }
        public List<SrcnDosyaDurums> DosyaDurumlari { get; set; }
        public SrcnDosyaDurums DosyaDurumu { get; set; }
        public List<SrcnFabrikaBirims> Birimler { get; set; }
        public List<DosyaNumuneLabAnalizTablo> DosyaNumuneLabAnalizTablolar { get; set; }
        public List<SrcnDenemeDosya> DenemeDosyalari { get; set; }
        public List<BirimDenemeAraModel> BirimDenemeAraModeller { get; set; }
        public List<SrcnDenemeDosya> DenemeDosyalar { get; set; }
        public SrcnDenemeDosya DenemeDosya { get; set; }
        public List<SrcnDenemeDosyaItems> DenemeDosyaItemlar { get; set; }
        public SelectList DropBirimMakineleri { get; set; }
        public List<SrcnDosyaItemMakineParametres> DosyaItemMakineParametreler { get; set; }
        public int AnalizYapilacakBobinSayisi { get; set; }
        public List<SrcnLabAnalizCesits> LabAnalizCesitleri { get; set; }
    }

    //
   

    public class BirimDenemeAraModel
    {
        public BirimDenemeAraModel()
        {
            Birim = new SrcnFabrikaBirims();
            DenemeDosyalar = new List<SrcnDenemeDosya>();
            DenemeDosyaItem = new SrcnDenemeDosyaItems();
            DosyaItemMakineParametreler = new List<SrcnDosyaItemMakineParametres>();
            DosyaLabAnalizTablolar = new List<DosyaLabAnalizTablo>();
        }
    public SrcnFabrikaBirims Birim { get; set; }
    public List<SrcnDenemeDosya> DenemeDosyalar { get; set; }
    public SrcnDenemeDosyaItems DenemeDosyaItem { get; set; }
    public List<SrcnDosyaItemMakineParametres> DosyaItemMakineParametreler { get; set; }
    public List<DosyaLabAnalizTablo> DosyaLabAnalizTablolar { get; set; }
    }
}