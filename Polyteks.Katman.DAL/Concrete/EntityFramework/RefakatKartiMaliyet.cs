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
    
    public partial class RefakatKartiMaliyet
    {
        public int SiraNo { get; set; }
        public string RefakatNo { get; set; }
        public short IslemSiraNo { get; set; }
        public string MaliyetAdi { get; set; }
        public Nullable<decimal> Maliyet { get; set; }
        public string DovizBirimi { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public Nullable<int> StokHareketFisNo { get; set; }
        public Nullable<int> StokHareketFisSiraNo { get; set; }
    }
}
