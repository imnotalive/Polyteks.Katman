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
    
    public partial class ACIK_CARITAKIP_HAREKETLER
    {
        public int HareketID { get; set; }
        public string Tarih { get; set; }
        public string HareketTipi { get; set; }
        public string CariNo { get; set; }
        public string BankaHesabi { get; set; }
        public string Vadesi { get; set; }
        public string CekSenetTipi { get; set; }
        public Nullable<decimal> Tutar { get; set; }
        public string DovizBirimi { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public string Aciklama { get; set; }
        public Nullable<int> CekID { get; set; }
        public string IslemGrubu { get; set; }
    }
}
