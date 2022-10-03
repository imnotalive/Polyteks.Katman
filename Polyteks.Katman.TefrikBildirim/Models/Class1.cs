using Polyteks.Katman.DAL.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class Class1
    {
        public IEnumerable<SelectListItem> Isletme { get; set; }
        public IEnumerable<SelectListItem> Hatalar { get; set; }
        public IEnumerable<SelectListItem> Mudahale { get; set; }
        public Mrv_KaliteBildirimDetay aaa  { get; set; }
    }
}