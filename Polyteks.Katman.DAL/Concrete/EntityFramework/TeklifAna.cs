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
    
    public partial class TeklifAna
    {
        public int TeklifAnaID { get; set; }
        public string TeklifNo { get; set; }
        public Nullable<System.DateTime> DuzenlemeTarihi { get; set; }
        public Nullable<System.DateTime> GecerlilikTarihi { get; set; }
        public string CariNo { get; set; }
        public string SevkCariNo { get; set; }
        public string ReferansNo { get; set; }
        public string YurtIciDisiTeklif { get; set; }
        public string TeklifAnaAciklama { get; set; }
        public string TeklifiVerenNo { get; set; }
        public string TeklifVerilen { get; set; }
        public string FiyatListeNo { get; set; }
        public string SevkYeri { get; set; }
        public Nullable<System.DateTime> Termin { get; set; }
        public string OdemeSekli { get; set; }
        public string TeslimSekli { get; set; }
        public string RevizyonNedeni { get; set; }
        public Nullable<int> ProjeID { get; set; }
        public string Onay { get; set; }
        public Nullable<System.DateTime> OnayTarihi { get; set; }
        public string OnaylayanKodu { get; set; }
        public string TeklifOnYazisi { get; set; }
        public Nullable<double> IskontoTutari { get; set; }
        public Nullable<double> IskontoOrani { get; set; }
        public Nullable<double> ToplamYerelFiyat { get; set; }
        public string NakliyeSekli { get; set; }
    }
}
