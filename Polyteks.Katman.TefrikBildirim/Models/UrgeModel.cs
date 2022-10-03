using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class UrgeModel
    {
        public UrgeModel()
        {
            NumuneYapilabilirlikDosyalar = new List<SrcnNumuneYapilabilirlikDosyas>();
            NumuneYapilabilirlikDosya = new SrcnNumuneYapilabilirlikDosyas();
            DosyaDurumlari = new List<SrcnDosyaDurums>();
            Birimler = new List<SrcnFabrikaBirims>();
            DenemeDosyalari = new List<SrcnDenemeDosya>();
            DosyaDurumu = new SrcnDosyaDurums();

            DosyaNumuneLabAnalizTablolar = new List<DosyaNumuneLabAnalizTablo>();
        }
        public List<SrcnNumuneYapilabilirlikDosyas> NumuneYapilabilirlikDosyalar { get; set; }
        public SrcnNumuneYapilabilirlikDosyas NumuneYapilabilirlikDosya { get; set; }

        public List<SrcnDosyaDurums> DosyaDurumlari { get; set; }
        public SrcnDosyaDurums DosyaDurumu { get; set; }
        public List<SrcnFabrikaBirims> Birimler { get; set; }
        public List<DosyaNumuneLabAnalizTablo> DosyaNumuneLabAnalizTablolar { get; set; }
        public List<SrcnDenemeDosya> DenemeDosyalari { get; set; }
    }
}