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
    
    public partial class KumasTopHataKontrol
    {
        public int KontrolNo { get; set; }
        public string TopNo { get; set; }
        public byte HataNo { get; set; }
        public string HataKodu { get; set; }
        public Nullable<decimal> BaslangicMt { get; set; }
        public Nullable<decimal> HataUzunlugu { get; set; }
        public Nullable<byte> HataliBantSayisi { get; set; }
        public Nullable<System.DateTime> UretimTarih { get; set; }
        public string UretimVardiya { get; set; }
        public Nullable<decimal> HataBitisMt { get; set; }
        public string HataPuan { get; set; }
        public string HataDurumu { get; set; }
        public Nullable<decimal> HataEnBoyu { get; set; }
        public bool PasifHata { get; set; }
        public string HataDerecesi { get; set; }
    }
}
