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
    
    public partial class Mrv_KaliteBildirimDetay
    {
        public int ID { get; set; }
        public Nullable<int> SiraNo { get; set; }
        public Nullable<System.DateTime> MudahaleTarihSaat { get; set; }
        public string MudahaleKullanici { get; set; }
        public string MudahaleKullaniciBolum { get; set; }
        public Nullable<int> Isletme { get; set; }
        public Nullable<int> Hata1 { get; set; }
        public string Hata1YapilanMudahale { get; set; }
        public Nullable<int> Hata2 { get; set; }
        public string Hata2YapilanMudahale { get; set; }
        public Nullable<int> Hata3 { get; set; }
        public string Hata3YapilanMudahale { get; set; }
        public string Aciklama { get; set; }
        public Nullable<int> BildirimId { get; set; }
        public Nullable<int> MakineAdi { get; set; }
        public Nullable<int> KanalNo { get; set; }
        public Nullable<int> PozisyonNo { get; set; }
        public string PartiNo { get; set; }
        public string IsletmeAciklama { get; set; }
        public Nullable<int> Durum { get; set; }
    
        public virtual Mrv_KaliteBildirim_Makine Mrv_KaliteBildirim_Makine { get; set; }
        public virtual Mrv_KaliteBildirimAna Mrv_KaliteBildirimAna { get; set; }
        public virtual Mrv_KaliteBildirimBolumleri Mrv_KaliteBildirimBolumleri { get; set; }
        public virtual Mrv_KaliteBildirimHata Mrv_KaliteBildirimHata { get; set; }
        public virtual Mrv_KaliteBildirimHata Mrv_KaliteBildirimHata1 { get; set; }
        public virtual Mrv_KaliteBildirimHata Mrv_KaliteBildirimHata2 { get; set; }
    }
}
