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
    
    public partial class SrcnPartiAltKlasorDosyas
    {
        public int PartiAltKlasorDosyaId { get; set; }
        public System.DateTime OlusturmaTarihi { get; set; }
        public int PartiAltKlasorId { get; set; }
        public string PartiAltKlasorKodAdi { get; set; }
        public int SiraNo { get; set; }
        public Nullable<int> MakineId { get; set; }
        public string MakineNo { get; set; }
        public string KayitYapanKulAdi { get; set; }
        public Nullable<int> KayitYapanKulId { get; set; }
        public string IslemTuru { get; set; }
        public string IslemNo { get; set; }
    }
}
