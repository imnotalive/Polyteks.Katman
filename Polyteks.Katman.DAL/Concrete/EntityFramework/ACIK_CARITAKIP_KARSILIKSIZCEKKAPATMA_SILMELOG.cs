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
    
    public partial class ACIK_CARITAKIP_KARSILIKSIZCEKKAPATMA_SILMELOG
    {
        public int KarsiliksizID { get; set; }
        public int OdemeHareketID { get; set; }
        public Nullable<System.DateTime> KarsiliksizTarih { get; set; }
        public string CariNo { get; set; }
        public Nullable<decimal> CekTutar { get; set; }
        public string CekDB { get; set; }
        public Nullable<decimal> CekTutarKapatilan { get; set; }
        public Nullable<decimal> HareketTutarKapatan { get; set; }
        public string OdemeTipi { get; set; }
        public Nullable<decimal> OdemeGercekTutar { get; set; }
        public string OdemeGercekDB { get; set; }
        public string CekAciklama { get; set; }
        public System.DateTime SilmeTarihi { get; set; }
    }
}
