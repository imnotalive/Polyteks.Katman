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
    
    public partial class Siparis
    {
        public string SiparisNo { get; set; }
        public short SiraNo { get; set; }
        public string StokTuru { get; set; }
        public string HamStokKodu { get; set; }
        public string StokKodu { get; set; }
        public string LotNo { get; set; }
        public string RenkKodu { get; set; }
        public Nullable<decimal> Miktar { get; set; }
        public string OlcuBirimi { get; set; }
        public string IslemGrubu { get; set; }
        public Nullable<System.DateTime> Termin { get; set; }
        public Nullable<decimal> Fiyat { get; set; }
        public string DovizBirimi { get; set; }
        public string Aciklama { get; set; }
        public string RotaNo { get; set; }
        public Nullable<System.DateTime> KapatmaTarihi { get; set; }
        public string Kapatan { get; set; }
        public Nullable<System.DateTime> IptalTarihi { get; set; }
        public string IptalEden { get; set; }
        public string IptalNedeni { get; set; }
        public Nullable<decimal> ToleransYuzde { get; set; }
        public Nullable<System.DateTime> OnayTarihi { get; set; }
        public string Onaylayan { get; set; }
        public Nullable<decimal> Miktar2 { get; set; }
        public string OlcuBirimi2 { get; set; }
        public Nullable<decimal> Miktar3 { get; set; }
        public string OlcuBirimi3 { get; set; }
        public string OdemeSekli { get; set; }
        public Nullable<decimal> StoktanKarsilananMiktar { get; set; }
        public Nullable<decimal> SevkToleransYuzde { get; set; }
        public string Ilavemi { get; set; }
        public string Tebdilmi { get; set; }
        public string TebdilPartiNo { get; set; }
        public Nullable<System.DateTime> PlanlamaTermini { get; set; }
        public string NakliyeSekli { get; set; }
        public string TeslimSekli { get; set; }
        public Nullable<decimal> KDVOrani { get; set; }
        public string KDVDahilmi { get; set; }
        public Nullable<decimal> IskontoOrani { get; set; }
        public string EtiketBilgisi1 { get; set; }
        public string EtiketBilgisi2 { get; set; }
        public string EtiketBilgisi3 { get; set; }
        public string EtiketBilgisi4 { get; set; }
        public string EtiketBilgisi5 { get; set; }
        public string FiyatOlcuBirimi { get; set; }
        public Nullable<decimal> Fiyat2 { get; set; }
        public string DovizBirimi2 { get; set; }
        public Nullable<decimal> Fiyat3 { get; set; }
        public string DovizBirimi3 { get; set; }
        public string IliskiliSiparisNo { get; set; }
        public Nullable<int> IliskiliSiparisSiraNo { get; set; }
        public string PlanlamaUyari { get; set; }
        public string PaketlemeSekli { get; set; }
        public Nullable<byte> HPSNo { get; set; }
        public Nullable<decimal> AgirlikToleransYuzde { get; set; }
        public string KoleksiyonKodu { get; set; }
        public Nullable<System.DateTime> SonKullanimTarihi { get; set; }
        public string SonKullaniciKodu { get; set; }
        public string UreticiKodu { get; set; }
        public bool ExceldenAktarildi { get; set; }
        public string TebdilNedeni { get; set; }
    }
}
