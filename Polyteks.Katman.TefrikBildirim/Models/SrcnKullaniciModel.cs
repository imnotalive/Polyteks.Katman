using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class SrcnKullaniciModel
    {
        public SrcnKullaniciModel()
        {
            Kullanicilar = new List<SrcnKullanicis>();
        }
        public string BirimId { get; set; }
        public string BolumId { get; set; }

        public int GrupId { get; set; }

        public SelectList UretimHatlari { get; set; }
        public SelectList UretimHatBolumleri { get; set; }
        public List<SrcnKullanicis> Kullanicilar { get; set; }

    }
}