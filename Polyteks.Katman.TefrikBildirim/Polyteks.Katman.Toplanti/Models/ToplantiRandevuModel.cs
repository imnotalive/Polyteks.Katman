using Polyteks.Katman.DAL.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.Toplanti.Models
{
    public class ToplantiRandevuModel
    {
        public ToplantiRandevuModel()
        {
            ToplantiRadevulari = new List<SrcnToplantiRadevus>();
            BaseSaatler = new List<SrcnToplantiBaseSaats>();
            ToplantiSalonlari = new List<SrcnToplantiSalons>();
            ToplantiRandevuSaatlerModeller = new List<ToplantiRandevuSaatler>();
            RandevuSaatleri = new List<SrcnToplantiRandevuSaats>();
            ToplantiRandevular = new List<SrcnToplantiRadevus>();
        }
        public List<SrcnToplantiRadevus> ToplantiRadevulari { get; set; }
        public List<SrcnToplantiBaseSaats> BaseSaatler { get; set; }
        public List<SrcnToplantiSalons> ToplantiSalonlari { get; set; }
        public List<ToplantiRandevuSaatler> ToplantiRandevuSaatlerModeller { get; set; }
        public string SecilenGun { get; set; }
        public List<SrcnToplantiRandevuSaats> RandevuSaatleri { get; set; }
        public List<SrcnToplantiRadevus> ToplantiRandevular { get; set; }
        public int GunSay { get; set; }
    }

    public class ToplantiRandevuSaatler
    {
        public ToplantiRandevuSaatler()
        {
            Saat = new SrcnToplantiBaseSaats();
            //  RandevuSaatleri = new List<SrcnToplantiRandevuSaats>();
            ToplantiRandevuSalonModelleri = new List<ToplantiRandevuSalonModel>();

        }
        public SrcnToplantiBaseSaats Saat { get; set; }
        public int SecilenToplantiSalonuId { get; set; }
      //  public List<SrcnToplantiRandevuSaats> RandevuSaatleri { get; set; }
        public List<ToplantiRandevuSalonModel> ToplantiRandevuSalonModelleri { get; set; }
      
  
    }

    public class ToplantiRandevuSalonModel
    {
        public ToplantiRandevuSalonModel()
        {
            ToplantiSalonu = new SrcnToplantiSalons();
            ToplantiRandevuSaat = new SrcnToplantiRandevuSaats();
            ToplantiRandevu = new SrcnToplantiRadevus();
        }
        public SrcnToplantiSalons ToplantiSalonu { get; set; }
        public SrcnToplantiRandevuSaats ToplantiRandevuSaat { get; set; }
        public bool DoluMu { get; set; }
        public SrcnToplantiRadevus ToplantiRandevu { get; set; }
    }

}