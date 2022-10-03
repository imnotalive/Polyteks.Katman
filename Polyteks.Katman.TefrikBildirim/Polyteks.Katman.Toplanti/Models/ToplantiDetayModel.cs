using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.Toplanti.Models
{
    public class ToplantiDetayModel
    {
        public ToplantiDetayModel()
        {
            ToplantiDetayModelItemlar = new List<ToplantiDetayModelItem>();
            RandevuDurum = new SrcnToplantiRandevuDurums();
            Randevu = new SrcnToplantiRadevus();
            RandevuDurumlar = new List<SrcnToplantiRandevuDurums>();
            RandevuSaatleri = new List<SrcnToplantiRandevuSaats>();
        }
        public List<ToplantiDetayModelItem> ToplantiDetayModelItemlar { get; set; }
        public SrcnToplantiRandevuDurums RandevuDurum { get; set; }
        public List<SrcnToplantiRadevus> Randevular { get; set; }
        public SrcnToplantiRadevus Randevu { get; set; }
        public bool YetkiliMi { get; set; }
        public List<SrcnToplantiRandevuDurums> RandevuDurumlar { get; set; }
        public List<SrcnToplantiRandevuSaats> RandevuSaatleri { get; set; }
   
    }
    public class ToplantiDetayModelItem
    {
        public ToplantiDetayModelItem()
        {
            RandevuDurum = new SrcnToplantiRandevuDurums();
            Randevular = new List<SrcnToplantiRadevus>();
        }
        public SrcnToplantiRandevuDurums RandevuDurum { get; set; }
        public List<SrcnToplantiRadevus> Randevular { get; set; }
    }
}