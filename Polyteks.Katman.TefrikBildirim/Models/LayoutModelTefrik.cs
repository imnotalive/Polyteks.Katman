using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.Has.Models
{
    public class LayoutModelTefrik
    {
        public LayoutModelTefrik()
        {
            Kullanici = new SrcnKullanicis();
            HeaderUstBildirimItemlar = new List<HeaderUstBildirimItem>();
            PartiAltKlasorTipleri = new List<SrcnPartiAltKlasorTipis>();
        }
        public SrcnKullanicis Kullanici { get; set; }
        public List<HeaderUstBildirimItem> HeaderUstBildirimItemlar { get; set; }
        public List<SrcnPartiAltKlasorTipis> PartiAltKlasorTipleri { get; set; }
    }
}