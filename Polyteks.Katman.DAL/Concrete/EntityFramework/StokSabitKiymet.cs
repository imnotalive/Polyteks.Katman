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
    
    public partial class StokSabitKiymet
    {
        public string StokKodu { get; set; }
        public string SabitKiymetTuru { get; set; }
        public bool AmortismanTuru { get; set; }
        public decimal AmortismanOrani { get; set; }
        public short AmortismanSuresi { get; set; }
        public bool YenidenDegerleme { get; set; }
        public bool DegerlemeAmortismani { get; set; }
        public bool KıstAmortismani { get; set; }
        public Nullable<System.DateTime> AlisTarihi { get; set; }
        public Nullable<System.DateTime> AmortismanBaslangicTarihi { get; set; }
        public decimal AlisTutari { get; set; }
        public string AlisDovizBirimi { get; set; }
        public decimal AlisKDVOrani { get; set; }
        public decimal AlisMiktari { get; set; }
        public decimal SatilanMiktar { get; set; }
        public decimal SatisTutari { get; set; }
        public string SatisDovizBirimi { get; set; }
        public decimal HurdayaCikanMiktar { get; set; }
        public decimal HurdaDegeri { get; set; }
        public string HurdaDovizBirimi { get; set; }
        public string Aciklama { get; set; }
    }
}
