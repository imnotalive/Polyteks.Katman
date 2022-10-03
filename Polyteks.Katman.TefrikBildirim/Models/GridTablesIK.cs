using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class GridTablesIK
    {
        public int EgitimOturumId { get; set; }
        public string EgitimOturumAdi { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public string OturumTarihi { get; set; }
        public Nullable<System.DateTime> OturumTarihiDateTime { get; set; }
        public int KayitYapanKulId { get; set; }
        public string KayitYapanKulAdi { get; set; }
        public int IkEgitimFirmaId { get; set; }
        public int IkEgitimId { get; set; }
        public string IkEgitimAdi { get; set; }
        public int IkFirmaId { get; set; }
        public string IkFirmaAdi { get; set; }
        public string Aciklama { get; set; }
        public int EgitimKatilimTanimId { get; set; }
        public string KatilimDurumAdi { get; set; }
        public string OturumTarihiSaati { get; set; }
        public string OturumTarihiBitisi { get; set; }
        public string OturumTarihiBitisSaati { get; set; }
        public Nullable<System.DateTime> OturumTarihiBitisDateTime { get; set; }
        public string OturumSureAciklama { get; set; }
    }
}