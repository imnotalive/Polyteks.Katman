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
    
    public partial class MaliTablo
    {
        public int MaliTabloID { get; set; }
        public int MaliTabloAnaID { get; set; }
        public short SiraNo { get; set; }
        public string Aciklama { get; set; }
        public int BaslangicHesapPlaniID { get; set; }
        public int BitisHesapPlaniID { get; set; }
        public int BagliMaliTabloID { get; set; }
        public int BagliMaliTablo2ID { get; set; }
        public int BagliMaliTablo3ID { get; set; }
        public double HesapTutari { get; set; }
        public string IslemTuru { get; set; }
        public string Gorunmesin { get; set; }
        public string Formul { get; set; }
    }
}
