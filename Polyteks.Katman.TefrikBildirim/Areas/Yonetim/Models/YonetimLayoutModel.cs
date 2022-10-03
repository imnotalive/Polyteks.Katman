using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.TefrikBildirim.Areas.Yonetim.Models
{
    public class YonetimLayoutModel
    {
        public YonetimLayoutModel()
        {
            Kullanici = new SrcnKullanicis();
            HeaderUstBildirimItemlar = new List<HeaderUstBildirimItem>();
        }
        public SrcnKullanicis Kullanici { get; set; }
        public List<HeaderUstBildirimItem> HeaderUstBildirimItemlar { get; set; }

    }
}