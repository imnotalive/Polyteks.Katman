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
    
    public partial class OnMaliyetStokUAFiyatLog
    {
        public string StokKodu { get; set; }
        public int SiraNo { get; set; }
        public string MaliyetTipi { get; set; }
        public string BilesenStokKodu { get; set; }
        public string Deger { get; set; }
        public Nullable<double> Fiyat { get; set; }
        public Nullable<double> Maliyet { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string KullaniciKodu { get; set; }
    }
}
