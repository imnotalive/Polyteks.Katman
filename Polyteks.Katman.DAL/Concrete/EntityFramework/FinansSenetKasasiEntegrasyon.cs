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
    
    public partial class FinansSenetKasasiEntegrasyon
    {
        public string FinansIslemTurKodu { get; set; }
        public int SenetKasasiID { get; set; }
        public int HesapPlaniID { get; set; }
        public int FinansDonemi { get; set; }
        public string KurTipi { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public string TanimlayanKullaniciKodu { get; set; }
        public System.DateTime DegistirilmeTarihi { get; set; }
        public string DegistirenKullaniciKodu { get; set; }
    }
}
