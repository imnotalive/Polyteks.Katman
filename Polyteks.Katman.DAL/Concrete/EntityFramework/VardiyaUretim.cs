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
    
    public partial class VardiyaUretim
    {
        public int Id { get; set; }
        public string MakineNo { get; set; }
        public Nullable<System.DateTime> Zaman { get; set; }
        public Nullable<decimal> Uretim { get; set; }
        public decimal Uretim2 { get; set; }
        public Nullable<int> Devir { get; set; }
        public string RefakatNo { get; set; }
        public string VardiyaKodu { get; set; }
        public Nullable<System.DateTime> SonGuncellemeTarihi { get; set; }
        public string PersonelNo { get; set; }
    }
}
