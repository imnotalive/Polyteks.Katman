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
    
    public partial class MRPSonuc
    {
        public System.DateTime RaporTarihi { get; set; }
        public Nullable<int> UstSiraNo { get; set; }
        public Nullable<int> SiraNo { get; set; }
        public Nullable<int> UASiraNo { get; set; }
        public string StokKodu { get; set; }
        public Nullable<double> BirimIhtiyac { get; set; }
        public Nullable<System.DateTime> Termin { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<double> StokMiktari { get; set; }
        public Nullable<double> SatinalmaMiktari { get; set; }
        public Nullable<double> IhtiyacMiktari { get; set; }
        public Nullable<double> SiparisMiktar { get; set; }
        public Nullable<double> PartiBuyuklugu { get; set; }
        public Nullable<double> MinimumStokDuzeyi { get; set; }
        public Nullable<double> KalanMiktar { get; set; }
        public Nullable<int> PartiKatsayisi { get; set; }
        public Nullable<bool> Hesaplandi { get; set; }
        public Nullable<double> MevcutStokMiktari { get; set; }
        public Nullable<double> MevcutSatinalmaMiktari { get; set; }
        public Nullable<double> KullanimMiktari { get; set; }
        public string SiparisNo { get; set; }
        public Nullable<int> SiparisSiraNo { get; set; }
        public Nullable<int> BagliSiraNo { get; set; }
        public string TeminSekli { get; set; }
        public bool IhtiyacHesaplanmayacak { get; set; }
        public Nullable<int> MRPSonucSiraNo { get; set; }
        public bool SatinalmaHesaplandi { get; set; }
    }
}
