//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Polyteks.Katman.DAL.Concrete.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class KapaliUlkelerStokKod
    {
        public int KapaliUlkelerStokKodID { get; set; }
        public string StokKodu { get; set; }
        public string Ulke { get; set; }
        public string CariGrup { get; set; }
        public string CariNo { get; set; }
        public Nullable<System.DateTime> BaslangicGecerlilikTarihi { get; set; }
        public Nullable<System.DateTime> BitisGecerlilikTarihi { get; set; }
        public string Aciklama { get; set; }
        public Nullable<System.DateTime> OnayTarihi { get; set; }
    }
}
