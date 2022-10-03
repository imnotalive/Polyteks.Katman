using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class PaketlemeGunlukTalimatModel
    {
        public PaketlemeGunlukTalimatModel()
        {
            GunlukTalimatTipleri = new List<SrcnPaketlemeGunlukTalimatTipis>();
            GunlukTalimatItemlar = new List<SrcnPaketlemeGunlukTalimatItems>();

            GunlukTalimatTipi = new SrcnPaketlemeGunlukTalimatTipis();
            GunlukTalimat = new SrcnPaketlemeGunlukTalimats();
            GunlukTalimatItem = new SrcnPaketlemeGunlukTalimatItems();

            FiltreSecimleri = new List<DropItem>();
            GunlukTalimatItems = new List<SrcnPaketlemeGunlukTalimatItems>();


            


        }
        public List<SrcnPaketlemeGunlukTalimatTipis> GunlukTalimatTipleri { get; set; }
        public List<SrcnPaketlemeGunlukTalimatItems> GunlukTalimatItemlar { get; set; }

         public List<SrcnPaketlemeGunlukTalimatItems> GunlukTalimatItems{ get; set; }
        public SelectList Partiler { get; set; }
        public SelectList Baslik { get; set; }
        public SelectList GunlukTalimatTipiDrop { get; set; }
        public SelectList GunlukTalimatTipiDrops { get; set; }
 
        public SelectList Birimler { get; set; }
        public string YeniPartiAdi { get; set; }
        public int BirimId { get; set; }

        public PagedList<SrcnPaketlemeGunlukTalimats> GunlukTalimatlar { get; set; }

        public SrcnPaketlemeGunlukTalimats GunlukTalimat { get; set; }
        public SrcnPaketlemeGunlukTalimatTipis GunlukTalimatTipi { get; set; }

        public SrcnPaketlemeGunlukTalimatItems GunlukTalimatItem { get; set; }
        [AllowHtml]
        public string TalimatBaslik  { get; set; }
        
        public string SecilenTarih { get; set; }
        public int PartiAnaKlasorId { get; set; }
        public int GunlukTalimatTipiId { get; set; }
        public List<DropItem> FiltreSecimleri { get; set; }


    }
}