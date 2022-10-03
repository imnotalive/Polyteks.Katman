using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class ImalatModel : LaboratuvarAnalizBase
    {
        public ImalatModel()
        {
            LottanOzelAlanaGecisItemlar = new List<LottanOzelAlanaGecisItem>();
            Birim = new SrcnFabrikaBirims();
            GunlukImalatMatrisHucreSatirlar = new List<GunlukImalatMatrisHucreSatir>();
            Kanallar = new List<DropItem>();
        }
        public int IsEmriNumarasiHazirlikDurumu { get; set; } // 0 seçim yapılmadı, 1- is emri var, 2- is emri yok

        public string UretimHattiId { get; set; }
        public string SecilenParti { get; set; }
        public string MakineId { get; set; }
        public string PartiNoTakimSaati { get; set; }
        public int PartiNoBobinSayisi { get; set; }
        public SelectList UretimHatlari { get; set; }
        public SelectList Makineler { get; set; }
        public SrcnFabrikaBirims Birim { get; set; }
        public SelectList PartiNolar { get; set; }
        public List<LottanOzelAlanaGecisItem> LottanOzelAlanaGecisItemlar { get; set; }
        public List<DropItem> Kanallar { get; set; }

        public List<GunlukImalatMatrisHucreSatir> GunlukImalatMatrisHucreSatirlar { get; set; }
    }
    public class LottanOzelAlanaGecisItem
    {
        public LottanOzelAlanaGecisItem()
        {
            LottanOzelAlanaGecis = new ZzzSrcnLottanOzelAlanaGecisler();
            Kanallar = new List<DropItem>();
        }
        public ZzzSrcnLottanOzelAlanaGecisler LottanOzelAlanaGecis { get; set; }
        public string TakimSaati { get; set; }
        public int BobinSayisi { get; set; }
        public List<DropItem> Kanallar { get; set; }
        public bool SeciliMi { get; set; }
    }

    public class GunlukImalatMatrisHucreSatir
    {
        public GunlukImalatMatrisHucreSatir()
        {
            LottanOzelAlanaGecis = new ZzzSrcnLottanOzelAlanaGecisler();
            GunlukImalatMatrisHucreler = new List<GunlukImalatMatrisHucre>();
        }
        public ZzzSrcnLottanOzelAlanaGecisler LottanOzelAlanaGecis { get; set; }
        public string TakimSaati { get; set; }
        public int BobinSayisi { get; set; }
        public List<GunlukImalatMatrisHucre> GunlukImalatMatrisHucreler { get; set; }

    }


    public class GunlukImalatMatrisHucre
    {
        public bool MevcutMu { get; set; }
        public string TakimSaati { get; set; }
        public int KanalNo { get; set; }
        public string KanalAdi { get; set; }
        public int BobinSayisi { get; set; }
        public ZzzSrcnLottanOzelAlanaGecisler LottanOzelAlanaGecis { get; set; }

    }

}