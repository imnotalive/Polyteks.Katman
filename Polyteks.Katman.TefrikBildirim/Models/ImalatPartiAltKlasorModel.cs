using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class ImalatPartiAltKlasorModel
    {
        public ImalatPartiAltKlasorModel()
        {
            Birim = new  SrcnFabrikaBirims();
            Birimler = new List<SrcnFabrikaBirims>();
            PartiAltKlasorTipi = new SrcnPartiAltKlasorTipis();
            PartiAltKlasorleri = new List<SrcnPartiAltKlasors>();
            IslemMakineParametreler = new List<ZzzSrcnIslemMakineParametre>();
            PartiAltKlasor = new SrcnPartiAltKlasors();
            PartiAltKlasorDosyaItemlar = new List<SrcnPartiAltKlasorDosyaItems>();
            Islem = new Islem();
            PartiAltKlasorDosyalar = new List<SrcnPartiAltKlasorDosyas>();


        }
        public int Id { get; set; }
        public int IkinciButonGrup { get; set; }
        public string SecilenPartiNo { get; set; }
        public Islem Islem { get; set; }


        public SrcnFabrikaBirims Birim { get; set; }
        public SrcnPartiAltKlasorTipis PartiAltKlasorTipi { get; set; }
        public SrcnPartiAltKlasors PartiAltKlasor { get; set; }
        public SrcnPartiAltKlasorDosyas PartiAltKlasorDosya { get; set; }
        public ZzzSrcnMakineIdli Makine { get; set; }
     


        public List<SrcnFabrikaBirims> Birimler { get; set; }
        public List<SrcnPartiAltKlasors> PartiAltKlasorleri { get; set; }
        public List<SrcnPartiAltKlasorDosyas> PartiAltKlasorDosyalar { get; set; }
        public List<SrcnPartiAltKlasorDosyaItems> PartiAltKlasorDosyaItemlar { get; set; }
        public List<ZzzSrcnIslemMakineParametre> IslemMakineParametreler { get; set; }

        public SelectList DropBirimMakineleri { get; set; }
        public SelectList DropPartiler { get; set; }

    }
}