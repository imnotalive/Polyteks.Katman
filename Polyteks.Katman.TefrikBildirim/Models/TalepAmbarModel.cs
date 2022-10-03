using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class TalepAmbarModel
    {
        public TalepAmbarModel()
        {
            //StokKarts = new List<StokKart>();
            Ambars = new List<Ambar>();
            //AmbarStokKarts = new List<AmbarStokKartModel>();
            //PagedListSrcn = new PagedListSrcn();
            User = new SrcnKullanicis();
           
        }

        public List<SrcnKullanicis> Users { get; set; }
        public List<Ambar> Ambars { get; set; }

        //public List<StokKart> StokKarts { get; set; }

        public int Id { get; set; }

        public List<DropItem> DropItems { get; set; }

        //public StokKart StokKart { get; set; }
        public Ambar Ambar { get; set; }
        //public List<AmbarStokTalep> AmbarStokTaleps { get; set; }
        //public AmbarStokTalep AmbarStokTalep { get; set; }

        public List<Ambar> TigerAmbars { get; set; }

        public int tip { get; set; }

        public int durum { get; set; }

        public int MiktarInt { get; set; }
        public SrcnKullanicis User { get; set; }
    }
}
