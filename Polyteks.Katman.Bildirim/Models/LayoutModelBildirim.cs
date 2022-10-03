using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.Bildirim.Models
{
    public class LayoutModelBildirim
    {
        public LayoutModelBildirim()
        {
            Kullanici = new Kullanici();
            HeaderUstBildirimItemlar = new List<HeaderUstBildirimItem>();
        }
        public Kullanici Kullanici { get; set; }
        public List<HeaderUstBildirimItem> HeaderUstBildirimItemlar { get; set; }

    
    }
}