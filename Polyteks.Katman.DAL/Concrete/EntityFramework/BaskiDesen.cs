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
    
    public partial class BaskiDesen
    {
        public string DesenNo { get; set; }
        public string OrijinalNumune { get; set; }
        public string MusteriDesenNo { get; set; }
        public string Desinator { get; set; }
        public Nullable<byte> SablonSayisi { get; set; }
        public string DosyaNo { get; set; }
        public Nullable<System.DateTime> DosyaNoAlmaTarihi { get; set; }
        public Nullable<System.DateTime> GelisTarihi { get; set; }
        public string MedyaNo { get; set; }
        public string DesenSahibi { get; set; }
        public string DesenDosyaAdresi { get; set; }
        public Nullable<decimal> DesenEni { get; set; }
        public string PatentNo { get; set; }
        public Nullable<System.DateTime> PatentTarihi { get; set; }
        public string CalismaDurumu { get; set; }
        public string IptalNedeni { get; set; }
        public Nullable<System.DateTime> IptalTarihi { get; set; }
        public string IptalEden { get; set; }
        public string Aciklama { get; set; }
        public string BaskiDesenGrubu { get; set; }
        public string SablonaCektirenFirma { get; set; }
        public string DesinatorFirma { get; set; }
        public string BaskiIslemTuru { get; set; }
        public string BaskiDesenAltGrubu { get; set; }
        public Nullable<decimal> DesenBoyu { get; set; }
        public string BaskiDesenTuru { get; set; }
        public string PartiNo { get; set; }
        public bool KullanimdaMi { get; set; }
    }
}
