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
    
    public partial class Hata
    {
        public string HataKodu { get; set; }
        public string HataAdi { get; set; }
        public string MaliyetMerkezNo { get; set; }
        public Nullable<short> StdDuzeltmeSuresi { get; set; }
        public string HataTipi { get; set; }
        public string HataGrupKodu { get; set; }
        public Nullable<decimal> HataPuani1 { get; set; }
        public Nullable<decimal> HataPuani2 { get; set; }
        public Nullable<decimal> HataPuani3 { get; set; }
        public Nullable<decimal> HataPuani4 { get; set; }
        public Nullable<decimal> HataPuani5 { get; set; }
        public Nullable<decimal> HataPuani6 { get; set; }
        public Nullable<decimal> HataPuani7 { get; set; }
        public Nullable<decimal> HataPuani8 { get; set; }
        public string HotKey { get; set; }
        public Nullable<decimal> EnBolumBirimHataPuani { get; set; }
        public Nullable<decimal> EnBolumOnHataPuani { get; set; }
        public Nullable<decimal> MinMtHataPuani { get; set; }
        public string HataTuru { get; set; }
        public Nullable<decimal> MaxHataUzunlugu { get; set; }
        public bool UADetaySecilecek { get; set; }
        public bool HamKontrol { get; set; }
        public bool MamulKontrol { get; set; }
        public bool AraKontrol { get; set; }
        public bool KullanimdaMi { get; set; }
    }
}
