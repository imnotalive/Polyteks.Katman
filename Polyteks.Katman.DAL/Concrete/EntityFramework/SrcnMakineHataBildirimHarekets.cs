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
    
    public partial class SrcnMakineHataBildirimHarekets
    {
        public int MakineHataBildirimHareketId { get; set; }
        public int MakineHataBildirimId { get; set; }
        public string PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public System.DateTime Tarih { get; set; }
        public string HareketAdi { get; set; }
        public Nullable<int> HareketTipi { get; set; }
    }
}
