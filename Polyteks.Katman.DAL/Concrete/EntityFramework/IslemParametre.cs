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
    
    public partial class IslemParametre
    {
        public string IslemNo { get; set; }
        public short ParametreNo { get; set; }
        public string ParametreAdi { get; set; }
        public string OlcuBirimi { get; set; }
        public string Deger1 { get; set; }
        public string Deger2 { get; set; }
        public bool Deger1Zorunlu { get; set; }
        public bool Deger2Zorunlu { get; set; }
        public bool DegerKontroluYapilacak { get; set; }
        public string Aciklama { get; set; }
        public bool UretimLotuOlusturur { get; set; }
    }
}
