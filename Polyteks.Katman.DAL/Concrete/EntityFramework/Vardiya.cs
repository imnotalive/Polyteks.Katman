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
    
    public partial class Vardiya
    {
        public string VardiyaKodu { get; set; }
        public string MaliyetMerkezNo { get; set; }
        public string VardiyaAdi { get; set; }
        public string BaslangicSaati { get; set; }
        public string BitisSaati { get; set; }
        public Nullable<System.DateTime> SonDegisiklikTarihi { get; set; }
        public Nullable<short> DegisimAraligi { get; set; }
        public Nullable<int> CalismaSuresi { get; set; }
        public Nullable<byte> DegisimSirasi { get; set; }
        public bool UretimVardiyasimi { get; set; }
    }
}
