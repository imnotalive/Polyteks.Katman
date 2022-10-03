using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class IzlenebilirlikPartiSonuModel
    {
        public IzlenebilirlikPartiSonuModel()
        {
            PartiSonuTakipIzlenebilirlikler = new List<ZzzSrcnPartiSonuTakipIzlenebilirlik>();
            PlanlamaBaslangicTarihi = DateTime.Now.ToShortDateString();
            PlanlamaBitisTarihi = DateTime.Now.ToShortDateString();
            PlanlamaTarihFiltresiYapilsinMi = false;
            PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik();
            PartiSonuTakip = new SrcnPartiSonuTakips();
            PartiSonuTakipBilgiOnaylari = new List<SrcnPartiSonuTakipBilgiOnays>();
            Siparis = new Siparis();
            SiparisAna = new SiparisAna();
        }
        public List<ZzzSrcnPartiSonuTakipIzlenebilirlik> PartiSonuTakipIzlenebilirlikler { get; set; }
        public ZzzSrcnPartiSonuTakipIzlenebilirlik PartiSonuTakipIzlenebilirlik { get; set; }
        public SrcnPartiSonuTakips PartiSonuTakip { get; set; }

        public List<SrcnPartiSonuTakipBilgiOnays> PartiSonuTakipBilgiOnaylari { get; set; }

        public Siparis Siparis { get; set; }
        public SiparisAna SiparisAna { get; set; }
        public bool PlanlamaTarihFiltresiYapilsinMi { get; set; }
        public string PlanlamaBaslangicTarihi { get; set; }
        public string PlanlamaBitisTarihi { get; set; }

        public bool CariFiltresiYapilsinMi { get; set; }
        public string SecilenCariKod { get; set; }

        public bool PartiFiltresiYapilsinMi { get; set; }
        public string SecilenParti { get; set; }

        public bool SiparisNoFiltresiYapilsinMi { get; set; }
        public string SiparisNo { get; set; }

        public bool PartiSonuTakipDurumuFiltresiYapilsinMi { get; set; }
        public int PartiSonuTakipDurumId { get; set; }

        public string SecilenStokKodu { get; set; }
        public string SecilenRefNo { get; set; }

        public SelectList Cariler { get; set; }
        public SelectList Partiler { get; set; }
        public SelectList PartisonuTakipDurumlar { get; set; }
        public SelectList SiparisNolar { get; set; }
        public SelectList Stoklar { get; set; }
        public SelectList RefakatNolar { get; set; }

    }
    public class SessionIzlenebilirlikPartiSonuDroplar
    {
        public DateTime Tarih { get; set; }
        public SelectList Cariler { get; set; }
        public SelectList Partiler { get; set; }
        public SelectList PartisonuTakipDurumlar { get; set; }
        public SelectList SiparisNolar { get; set; }
        public SelectList Stoklar { get; set; }
        public SelectList RefakatNolar { get; set; }
    }

}