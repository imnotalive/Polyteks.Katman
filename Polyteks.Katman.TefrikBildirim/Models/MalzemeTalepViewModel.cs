using Polyteks.Katman.DAL.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class MalzemeTalepViewModel
    {

        public View_STOK_DURUM_PTKS_TASD View_STOK_DURUM_PTKS_TASD { get; set; }
        public Mrv_MalzemeAna Mrv_MalzemeAna { get; set; }
        public Mrv_MalzemeDetay Mrv_MalzemeDetay { get; set; }
    }
}