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
    
    public partial class ProjeAdimlariDurumu
    {
        public int ProjeAdimlariDurumuID { get; set; }
        public Nullable<int> ProjeAdimlariID { get; set; }
        public Nullable<System.DateTime> BaslamaTarihi { get; set; }
        public Nullable<System.DateTime> BitisTarihi { get; set; }
        public Nullable<int> ProjeDurusNedenleriID { get; set; }
        public string Bitti { get; set; }
        public string CalisanKullaniciKodu { get; set; }
        public string Aciklama { get; set; }
        public string AktifPasif { get; set; }
    }
}
