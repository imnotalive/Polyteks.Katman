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
    
    public partial class SevkiyatPlani
    {
        public int SevkiyatPlanNo { get; set; }
        public int SiraNo { get; set; }
        public string SiparisNo { get; set; }
        public Nullable<int> SiparisSiraNo { get; set; }
        public Nullable<decimal> SevkMiktari { get; set; }
        public string Aciklama { get; set; }
        public string TanimlayanKullaniciKodu { get; set; }
        public Nullable<System.DateTime> TanimlandigiTarih { get; set; }
    }
}
