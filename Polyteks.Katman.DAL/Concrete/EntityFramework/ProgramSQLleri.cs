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
    
    public partial class ProgramSQLleri
    {
        public int ProgramSQLleriID { get; set; }
        public string KullaniciKodu { get; set; }
        public int FormTanimID { get; set; }
        public string GridAdi { get; set; }
        public string SQLAna { get; set; }
        public string SQLGroupByOrderBy { get; set; }
        public string GridTipi { get; set; }
        public string Aciklama { get; set; }
        public Nullable<int> KontrolID { get; set; }
        public Nullable<int> SQLNo { get; set; }
    }
}
