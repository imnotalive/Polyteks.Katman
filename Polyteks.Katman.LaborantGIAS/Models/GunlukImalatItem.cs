using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.LaborantGIAS.Models
{
    public class DenemeNumuneItem
    {
        [DisplayName("Kayıt No")]
        public int KayitNo { get; set; }

        [DisplayName("Tarih")]
        public DateTime KayitTarihi { get; set; }

        [DisplayName("Kayıt Yapan")]
        public string TalepEden { get; set; }

        [DisplayName("Birim")]

        public string Birim { get; set; }

        [DisplayName("Cari")]

        public string Cari { get; set; }

        [DisplayName("Analiz Durumu")]
        public string AnalizDurumu { get; set; }

    


    }

    public class GunlukImalatItem
    {
        [DisplayName("Kayıt No")]
        public int KayitNo { get; set; }

        [DisplayName( "Tarih")]
        public DateTime KayitTarihi { get; set; }

        [DisplayName("Kayıt Yapan")]
        public string TalepEden { get; set; }

        [DisplayName("Birim")]

        public string Birim { get; set; }

        [DisplayName("Analiz Durumu")]
        public string AnalizDurumu { get; set; }


    }

    public class HucreDetayItem
    {
        public HucreDetayItem()
        {
            TakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik();
            AnalizItemCesitSonuc = new SrcnLabAnalizItemAnalizCesitSonucs();
            LabAnalizItem = new SrcnLabAnalizItems();
        }
        public SrcnLabAnalizItemAnalizCesitSonucs AnalizItemCesitSonuc { get; set; }
        public SrcnLabAnalizItems LabAnalizItem { get; set; }
        public int SayfaNo { get; set; }
        public int SatirId { get; set; }
        public int SutunId  { get; set; }
        public ZzzSrcnPartiSonuTakipIzlenebilirlik TakipIzlenebilirlik { get; set; }
    }
}
