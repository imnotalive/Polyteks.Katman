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
    
    public partial class IrsaliyeAna
    {
        public string IrsaliyeNo { get; set; }
        public string CariNo { get; set; }
        public System.DateTime IrsaliyeTarihi { get; set; }
        public string Aciklama { get; set; }
        public string TeslimCariNo { get; set; }
        public string NakliyeCariNo { get; set; }
        public string IslemTuru { get; set; }
        public string BelgeTuru { get; set; }
        public string SevkAdresi { get; set; }
        public string BankaHesapNo { get; set; }
        public string DovizBirimi { get; set; }
        public Nullable<decimal> DovizAlis { get; set; }
        public Nullable<decimal> DovizSatis { get; set; }
        public Nullable<int> VadeGunu { get; set; }
    }
}
