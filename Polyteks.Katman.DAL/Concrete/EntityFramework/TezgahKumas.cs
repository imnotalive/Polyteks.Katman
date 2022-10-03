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
    
    public partial class TezgahKumas
    {
        public string TezgahKumasKodu { get; set; }
        public string TezgahKumasAdi { get; set; }
        public string Kalite { get; set; }
        public string DesenKodu { get; set; }
        public string Renk { get; set; }
        public Nullable<int> AltCozguRaporSablonID { get; set; }
        public Nullable<int> UstCozguRaporSablonID { get; set; }
        public Nullable<int> AtkiRaporSablonID { get; set; }
        public Nullable<decimal> CozguTelSayisi { get; set; }
        public Nullable<decimal> CozguSikligi { get; set; }
        public string TaharRaporu { get; set; }
        public string TarakNoRaporu { get; set; }
        public Nullable<decimal> TarakEni { get; set; }
        public Nullable<decimal> TarakCozguEni { get; set; }
        public Nullable<decimal> AltCozguGerginligi { get; set; }
        public Nullable<decimal> UstCozguGerginligi { get; set; }
        public Nullable<decimal> CozguTelSayisi1 { get; set; }
        public Nullable<decimal> CozguTelSayisi2 { get; set; }
        public Nullable<decimal> PisKenarCozguTelSayisi { get; set; }
        public Nullable<decimal> CozguAgirligi1 { get; set; }
        public Nullable<decimal> CozguAgirligi2 { get; set; }
        public Nullable<decimal> PisKenarCozguAgirligi { get; set; }
        public Nullable<decimal> CozguFire { get; set; }
        public Nullable<decimal> AtkiSikligi { get; set; }
        public Nullable<decimal> AtkiSayisi { get; set; }
        public Nullable<decimal> AtkiRaporBoyu { get; set; }
        public Nullable<decimal> AtkiEni1 { get; set; }
        public Nullable<decimal> AtkiEni2 { get; set; }
        public Nullable<decimal> PisKenarAtkiEni { get; set; }
        public Nullable<decimal> AtkiAgirligi1 { get; set; }
        public Nullable<decimal> AtkiAgirligi2 { get; set; }
        public Nullable<decimal> PisKenarAtkiAgirligi { get; set; }
        public Nullable<decimal> AtkiFire { get; set; }
        public Nullable<decimal> AtkiYonuRaporTekrari { get; set; }
        public Nullable<decimal> AtkiyonuRaporGenisligi { get; set; }
        public Nullable<decimal> CozguYonuRaporGenisligi { get; set; }
        public Nullable<decimal> AtkiToplamasi { get; set; }
        public Nullable<decimal> CozguCalismasi { get; set; }
        public string PisKenarRaporu { get; set; }
        public string YalanciKenarRaporu { get; set; }
        public Nullable<decimal> AtkiCekmesi { get; set; }
        public Nullable<decimal> CozguCekmesi { get; set; }
        public string Komposizyon { get; set; }
        public string KullanmaKategorisi { get; set; }
        public string KumasBakimOzellikleri { get; set; }
        public string BoyaBilgileri { get; set; }
        public string ApreBilgileri { get; set; }
        public string BoyahaneRenkKodu { get; set; }
        public Nullable<decimal> HamGramajm2 { get; set; }
        public Nullable<decimal> MamulGramajm2 { get; set; }
        public Nullable<int> DesenRaporID { get; set; }
        public string DesenResimAdresi { get; set; }
        public Nullable<decimal> ParcaEni { get; set; }
        public Nullable<decimal> ParcaBoyu { get; set; }
        public Nullable<decimal> HavOrani { get; set; }
        public Nullable<byte> BantAdedi { get; set; }
        public Nullable<decimal> KisaHavBoyu { get; set; }
        public Nullable<byte> BordurSayisi { get; set; }
        public Nullable<decimal> BordurGenisligi { get; set; }
        public Nullable<decimal> KenarBezBoyu { get; set; }
        public Nullable<decimal> MamulGrm2 { get; set; }
        public string HavluMuBezMi { get; set; }
        public Nullable<decimal> AnaHavBoyu { get; set; }
        public Nullable<decimal> CozguOrani { get; set; }
        public Nullable<decimal> HavYuksekligi { get; set; }
        public Nullable<decimal> HamGrAdetMtul { get; set; }
        public Nullable<decimal> MamulGrAdetMtul { get; set; }
        public string KenarPlani { get; set; }
        public Nullable<int> TarakRaporSablonID { get; set; }
        public Nullable<decimal> HamGramajFiresi { get; set; }
        public Nullable<decimal> BirKenarEni { get; set; }
        public string KenarDislisi { get; set; }
        public string CariNo { get; set; }
        public Nullable<System.DateTime> DesenOlusturmaTarihi { get; set; }
        public string UygunTezgah { get; set; }
        public string TezgahTaharPlani { get; set; }
        public Nullable<int> HavRaporID { get; set; }
        public Nullable<decimal> KadifeMamulGramajm2 { get; set; }
        public string Aciklama { get; set; }
        public string LansetBilgisi { get; set; }
        public string KumasCinsi { get; set; }
        public string HavDislisi { get; set; }
        public Nullable<decimal> HavKumasOrani { get; set; }
        public string HavluTipi { get; set; }
        public Nullable<decimal> NormalBukleBoyu { get; set; }
        public Nullable<decimal> YuksekBukleBoyu { get; set; }
        public Nullable<decimal> NormalBukleFire { get; set; }
        public Nullable<decimal> YuksekBukleFire { get; set; }
        public Nullable<decimal> FiiliBordurBoyu { get; set; }
        public Nullable<decimal> FiiliKenarBezBoyu { get; set; }
        public Nullable<decimal> FiiliKisaHavBoyu { get; set; }
        public Nullable<decimal> FiiliAnaHavBoyu { get; set; }
        public Nullable<decimal> HavOrani2 { get; set; }
        public string DenemeNo { get; set; }
        public string NumuneIstekNo { get; set; }
        public string PersonelNo { get; set; }
        public Nullable<decimal> FiiliMKumasAgirligi { get; set; }
        public Nullable<decimal> PliseBoyu { get; set; }
        public string KullanimdaMi { get; set; }
        public Nullable<int> CozguRaporSablon3ID { get; set; }
        public Nullable<int> CozguRaporSablon4ID { get; set; }
        public Nullable<decimal> CozguRaporSablon4SarfOrani { get; set; }
        public bool MaliyetReferansi { get; set; }
        public string SablonNo { get; set; }
        public string SayacKodu { get; set; }
        public Nullable<int> KayitSiraNo { get; set; }
        public decimal FiiliHKumasAgirligi { get; set; }
        public string KolleksiyonKodu { get; set; }
        public string AtkiDislisi { get; set; }
        public string TezgahKumasSablonNo { get; set; }
        public Nullable<System.DateTime> SonKullanimTarihi { get; set; }
        public string SonKullaniciKodu { get; set; }
        public string TanimlayanKullaniciKodu { get; set; }
        public string TezgahKumasGrubu { get; set; }
        public string KomposizyonManuel { get; set; }
        public string KadifeUygunTezgah { get; set; }
        public Nullable<int> SablonSiraNo { get; set; }
        public string Kombin { get; set; }
        public decimal CozguRaporSablon1SarfOrani { get; set; }
        public decimal CozguRaporSablon2SarfOrani { get; set; }
        public decimal CozguRaporSablon3SarfOrani { get; set; }
        public Nullable<decimal> HKumasAgirligi { get; set; }
        public Nullable<decimal> MKumasAgirligi { get; set; }
        public Nullable<int> OrmeRaporID { get; set; }
        public string OrmeUygunTezgahTip { get; set; }
        public decimal OrmeMamulGramajm2 { get; set; }
    }
}
