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
    
    public partial class MalzemeTalep
    {
        public string MalzemeTalepNo { get; set; }
        public byte SiraNo { get; set; }
        public string TalepEdenAmbarNo { get; set; }
        public string TalepEdilenAmbarNo { get; set; }
        public string StokTuru { get; set; }
        public string StokKodu { get; set; }
        public string LotNo { get; set; }
        public Nullable<double> Miktar1 { get; set; }
        public Nullable<double> Miktar2 { get; set; }
        public Nullable<double> Miktar3 { get; set; }
        public string Aciklama { get; set; }
        public string Onaylayan { get; set; }
        public Nullable<System.DateTime> OnayTarihi { get; set; }
        public string IptalEden { get; set; }
        public Nullable<System.DateTime> IptalTarihi { get; set; }
        public string Kapatan { get; set; }
        public Nullable<System.DateTime> KapatmaTarihi { get; set; }
        public string SatinalmaIstekNo { get; set; }
        public Nullable<byte> SatinalmaIstekSiraNo { get; set; }
    }
}
