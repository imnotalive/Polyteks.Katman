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
    
    public partial class ChatMesajKullaniciBilgi
    {
        public int KullaniciBilgiID { get; set; }
        public string KullaniciKodu { get; set; }
        public string GonderilenKullaniciKodu { get; set; }
        public Nullable<int> MesajSayisi { get; set; }
        public Nullable<int> EskiMesajSayisi { get; set; }
        public Nullable<System.DateTime> SonGoruntulenmeTarihi { get; set; }
    }
}
