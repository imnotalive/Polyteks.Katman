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
    
    public partial class BakimHareket
    {
        public int BakimHareketID { get; set; }
        public string MakineNo { get; set; }
        public string PersonelNo { get; set; }
        public string BakimNedeni { get; set; }
        public Nullable<System.DateTime> BakimBaslamaTarihi { get; set; }
        public Nullable<System.DateTime> BakimBitisTarihi { get; set; }
    }
}
