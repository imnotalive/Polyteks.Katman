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
    
    public partial class UrunAgaci
    {
        public string StokKodu { get; set; }
        public byte SiraNo { get; set; }
        public string BilesenStokKodu { get; set; }
        public Nullable<decimal> KullanimMiktari { get; set; }
        public string OlcuBirimi { get; set; }
        public Nullable<decimal> FireYuzde { get; set; }
        public Nullable<decimal> En { get; set; }
        public Nullable<decimal> Boy { get; set; }
        public Nullable<System.DateTime> BaslangicGecerlilikTarihi { get; set; }
        public Nullable<System.DateTime> BitisGecerlilikTarihi { get; set; }
        public string Aciklama { get; set; }
        public Nullable<System.DateTime> SonKullanimTarihi { get; set; }
        public string SonKullaniciKodu { get; set; }
        public Nullable<decimal> FiiliKullanimMiktari { get; set; }
        public bool MRPdeFireliHesapla { get; set; }
        public string KullanilabilirKaliteler { get; set; }
        public bool UretimLotuOlusturur { get; set; }
        public Nullable<int> UrunSablonID { get; set; }
        public Nullable<decimal> Yukseklik { get; set; }
        public Nullable<decimal> Kalinlik { get; set; }
        public bool IsEmriOlusturur { get; set; }
    }
}
