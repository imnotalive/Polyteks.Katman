using Polyteks.Katman.DAL.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class AmbarTurleriModel
    {
        public string MamulAmbar { get; set; }
        public string HammaddeAmbari { get; set; }
        public string YedekParcaAmbari { get; set; }
        public ICollection<StokAmbar> StokAmbar { get; set; }
    }
}