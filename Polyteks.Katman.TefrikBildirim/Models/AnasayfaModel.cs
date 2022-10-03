using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class AnasayfaModel
    {
        public AnasayfaModel()
        {
            BildirimItems = new List<HeaderUstBildirimItem>();
        }
        public List<HeaderUstBildirimItem> BildirimItems { get; set; }
    }


}