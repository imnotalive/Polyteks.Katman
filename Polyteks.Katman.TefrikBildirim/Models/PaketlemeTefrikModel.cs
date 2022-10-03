using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class PaketlemeTefrikModel
    {
        public PaketlemeTefrikModel()
        {
            BaslangicTarihiDateTime = new DateTime();
            BitisTarihiDateTime = new DateTime();
            TefrikRaporOzeItems = new List<ZzzSrcnTefrikRaporOzets>();
        }
        public string BaslangicTarihi { get; set; }
        public string BitisTarihi { get; set; }

        public DateTime BaslangicTarihiDateTime { get; set; }
        public DateTime BitisTarihiDateTime { get; set; }

        public List<ZzzSrcnTefrikRaporOzets> TefrikRaporOzeItems { get; set; }
    }
}