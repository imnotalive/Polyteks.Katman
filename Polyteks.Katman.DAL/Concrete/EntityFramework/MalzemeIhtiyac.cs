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
    
    public partial class MalzemeIhtiyac
    {
        public System.DateTime RaporTarihi { get; set; }
        public string StokKodu { get; set; }
        public string StokTuru { get; set; }
        public string StokAdi { get; set; }
        public Nullable<decimal> IhtiyacMiktar1 { get; set; }
        public string OlcuBirimi1 { get; set; }
        public Nullable<decimal> IhtiyacMiktar2 { get; set; }
        public string OlcuBirimi2 { get; set; }
        public Nullable<decimal> GirilenIhtiyacMiktar1 { get; set; }
        public Nullable<decimal> GirilenIhtiyacMiktar2 { get; set; }
        public string SiparisNo { get; set; }
        public Nullable<short> SiparisSiraNo { get; set; }
    }
}
