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
    
    public partial class SrcnNumuneYapilabilirlikDosyas
    {
        public int NumuneYapilabilirlikDosyaId { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public int FirmaTipi { get; set; }
        public string CariKodu { get; set; }
        public string CariAdi { get; set; }
        public int LabAnalizDurumu { get; set; }
        public string LabAnalizDurumuAdi { get; set; }
        public int DosyaDurumId { get; set; }
        public string DosyaDurumAdi { get; set; }
        public string KayitYapanKulKodu { get; set; }
        public string KayitYapanKulAdi { get; set; }
        public int AnalizYapilacakBobinSayisi { get; set; }
        public int AnalizYapilacakKumasSayisi { get; set; }
        public string Aciklama { get; set; }
        public Nullable<int> YapilabilirlikDurumu { get; set; }
        public string YapilabilirlikDurumuAdi { get; set; }
        public int DenemeYapilmaDurumu { get; set; }
        public string DenemeYapilmaDurumuAdi { get; set; }
        public int SipariseDonmeDurumu { get; set; }
        public string SipariseDonmeDurumuAdi { get; set; }
        public string DenemeKodu { get; set; }
        public string SiparisNo { get; set; }
        public Nullable<int> NumuneDosyaTipi { get; set; }
        public string NumuneDosyaTipiAdi { get; set; }
        public Nullable<int> NumuneDosyaTipiKategoriTipi { get; set; }
        public string NumuneDosyaTipiKategoriTipiAdi { get; set; }
        public string UrgeYorumu { get; set; }
        public string LabYorumu { get; set; }
        public string SatisYorumu { get; set; }
        public Nullable<int> FabrikaBirimId { get; set; }
        public string FabrikaBirimAdi { get; set; }
        public string NumuneTalepDetayi { get; set; }
        public string CariSecim { get; set; }
        public Nullable<int> AnalizYapilacakRenkCalismasi { get; set; }
    }
}
