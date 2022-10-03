using Polyteks.Katman.DAL.Concrete.EntityFramework;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class ImalatPartiSonuModel
    {
        public ImalatPartiSonuModel()
        {
            PartiSonuDurumOzetCheckItemlar = new List<PartiSonuDurumOzetCheckItem>();
            Birimler = new List<SrcnFabrikaBirims>();
            Birim = new SrcnFabrikaBirims();
            OnayAlinacakBirimler = new List<SrcnFabrikaBirims>();
            YapilacakPartiSonuIslemTipi = 2;
            PartiSonuTakipHareketleri = new List<SrcnPartiSonuTakipHareketTakipTanims>();
            PartiSonuDurumOzetCheckItem = new PartiSonuDurumOzetCheckItem();
            PartiSonuEkIslemItemlar = new List<PartiSonuEkIslemItem>();
            PartiSonuEkIslemTanimlari = new List<SrcnPartiSonuEkIslemTanims>();
        }
        public int Id { get; set; }
        public int IkinciButonGrup { get; set; }
        public string SecilenParti { get; set; }
        public int YapilacakPartiSonuIslemTipi { get; set; } // 1-Partiyi Paketlemeye Göndermeden Sonlandır, 2- Paketlemeye Parti Sonu Yapması için talep oluştur

        //   public int TalepOlusturulanBirim { get; set; }
        public SelectList DropPartiNolar { get; set; }
        public SrcnFabrikaBirims Birim { get; set; }
        public List<SrcnFabrikaBirims> Birimler { get; set; }
        public List<SrcnFabrikaBirims> OnayAlinacakBirimler { get; set; }
        public List<PartiSonuDurumOzetCheckItem> PartiSonuDurumOzetCheckItemlar { get; set; }
        public List<SrcnPartiSonuTakipHareketTakipTanims> PartiSonuTakipHareketleri { get; set; }

        public PartiSonuDurumOzetCheckItem PartiSonuDurumOzetCheckItem { get; set; }

        public List<PartiSonuEkIslemItem> PartiSonuEkIslemItemlar { get; set; }

        public List<SrcnPartiSonuEkIslemTanims> PartiSonuEkIslemTanimlari { get; set; }
    }

    public class PartiSonuDurumOzetCheckItem
    {
        public PartiSonuDurumOzetCheckItem()
        {
            SeciliMi = false;
            PartiSonuDurumOzet = new ZzzSrcnPartiSonuDurumOzet();
            PartiSonuTakip = new SrcnPartiSonuTakips();
            PartiSonuTakipBilgiOnay = new SrcnPartiSonuTakipBilgiOnays();
            PartiSonuTakipBilgiOnaylar = new List<SrcnPartiSonuTakipBilgiOnays>();
            PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik();
        }
        public bool SeciliMi { get; set; }
        public ZzzSrcnPartiSonuDurumOzet PartiSonuDurumOzet { get; set; }
        public SrcnPartiSonuTakips PartiSonuTakip { get; set; }
        public SrcnPartiSonuTakipBilgiOnays PartiSonuTakipBilgiOnay { get; set; }
        public List<SrcnPartiSonuTakipBilgiOnays> PartiSonuTakipBilgiOnaylar { get; set; }
        public ZzzSrcnPartiSonuTakipIzlenebilirlik PartiSonuTakipIzlenebilirlik { get; set; }
    }

    public class PartiSonuEkIslemItem
    {
        public PartiSonuEkIslemItem()
        {
            Birim = new SrcnFabrikaBirims();
            PartiSonuEkIslemler = new List<SrcnPartiSonuEkIslems>();
        }
        public SrcnFabrikaBirims Birim { get; set; }
        public List<SrcnPartiSonuEkIslems> PartiSonuEkIslemler { get; set; }
    }
}