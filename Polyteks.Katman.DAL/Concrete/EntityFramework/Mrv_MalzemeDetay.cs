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
    
    public partial class Mrv_MalzemeDetay
    {
        public int ID { get; set; }
        public Nullable<int> MalzemeNo { get; set; }
        public Nullable<int> SiraNo { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string Parti { get; set; }
        public string Kalite { get; set; }
        public Nullable<decimal> Miktar { get; set; }
        public string StokAdi { get; set; }
        public string StokKodu { get; set; }
        public string Birim { get; set; }
        public string Aciklama { get; set; }
        public string ChipsSilosu { get; set; }
        public string Firma { get; set; }
        public string StokTuru { get; set; }
    
        public virtual Mrv_MalzemeAna Mrv_MalzemeAna { get; set; }
    }
}
