using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class LaboratuvarIsModel
    {
        public LaboratuvarIsModel()
        {
            LaboratuvarIsModelItemlar = new List<LaboratuvarIsModelItem>();
            LabGrup = new SrcnLabGrups();
        }
        public SrcnLabGrups LabGrup { get; set; }
        public List<LaboratuvarIsModelItem> LaboratuvarIsModelItemlar { get; set; }
        public int ToplamAdet { get; set; }
    }

    public class LaboratuvarIsModelItem
    {
        public LaboratuvarIsModelItem()
        {
            LabAnalizDurum = new SrcnLabAnalizDurumus();
        }
        public SrcnLabAnalizDurumus LabAnalizDurum { get; set; }
        public int Adet { get; set; }
    }

    public class BobinIplikModelItem
    {
        public BobinIplikModelItem()
        {
            LabAnalizCesitleri = new List<SrcnLabAnalizCesits>();

        }
        public bool SeciliMi { get; set; }
        public int IplikNo { get; set; }
        public string BobinAdi { get; set; }
        public int LabAnalizItemId { get; set; }
        public List<SrcnLabAnalizCesits> LabAnalizCesitleri { get; set; }

    }

    public class LaboratuvarAnalizItemSonuclarModel
    {
        public LaboratuvarAnalizItemSonuclarModel()
        {
            LabAnalizItem = new SrcnLabAnalizItems();
            LabAnalizItemAnalizCesitSonuclari = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
        }
        public SrcnLabAnalizItems LabAnalizItem { get; set; }
        public List<SrcnLabAnalizItemAnalizCesitSonucs> LabAnalizItemAnalizCesitSonuclari { get; set; }
    }

}