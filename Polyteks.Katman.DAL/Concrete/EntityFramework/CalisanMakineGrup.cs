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
    
    public partial class CalisanMakineGrup
    {
        public int CalisanMakineGrupID { get; set; }
        public string GrupNo { get; set; }
        public System.DateTime BaslangicTarihi { get; set; }
        public System.DateTime BitisTarihi { get; set; }
        public string PersonelNo { get; set; }
        public string DevirPersonelNo { get; set; }
        public string Aciklama { get; set; }
        public Nullable<System.DateTime> TanimlamaTarihi { get; set; }
        public Nullable<System.DateTime> DegistirmeTarihi { get; set; }
    }
}
