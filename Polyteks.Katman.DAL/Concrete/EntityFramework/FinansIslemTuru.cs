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
    
    public partial class FinansIslemTuru
    {
        public string FinansIslemTurKodu { get; set; }
        public string FinansIslemTuru0 { get; set; }
        public string FinansIslemTuru1 { get; set; }
        public string FinansIslemTuru2 { get; set; }
        public string BorcAlacak { get; set; }
        public string Aciklama { get; set; }
        public string IslemDetayFormAdi { get; set; }
        public bool IslemDetayFormGiris { get; set; }
        public string RaporGrupAdi { get; set; }
        public bool IrsaliyeOlustur { get; set; }
        public string FinansIslemTurGrubu { get; set; }
        public bool IhracatFaturasi { get; set; }
        public string IhracatIthalat { get; set; }
    
        public virtual FinansIslemGrubu FinansIslemGrubu { get; set; }
        public virtual FinansIslemYeri FinansIslemYeri { get; set; }
    }
}
