using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class IkModel
    {
        public IkModel()
        {
            IkFirmalar = new List<SrcnIkFirmas>();
            IkEgitimler = new List<SrcnIkEgitims>();
            IkEgitimFirmalar = new List<SrcnIkEgitimFirmas>();
            IkEgitimTipleri = new List<SrcnIkEgitimTipis>();
            SrcnKullanicilar = new List<SrcnKullanicis>();
            OzelDropMenuler = new List<DropItem>();
            EgitimOturumlari = new List<SrcnIkEgitimOturums>();
            EgitimOturumSrcnKullanicilar = new List<SrcnIkEgitimOturumSrcnKullanicis>();
            EgitimKatilimlari = new List<SrcnIkEgitimKatilimTanim>();

            IkFirma = new SrcnIkFirmas();
            IkEgitim = new SrcnIkEgitims();
            IkEgitimFirma = new SrcnIkEgitimFirmas();
            EgitimTipi = new SrcnIkEgitimTipis();
            SrcnKullanici = new SrcnKullanicis();
            EgitimOturum = new SrcnIkEgitimOturums(){EgitimKatilimTanimId = 1};
            EgitimOturumSrcnKullanici = new SrcnIkEgitimOturumSrcnKullanicis() { EgitimKatilimTanimId = 4 };
            PersonelSicilOzetItemlar = new List<PersonelSicilOzetItem>();


        }
        public List<SrcnIkFirmas> IkFirmalar { get; set; }
        public List<SrcnIkEgitims> IkEgitimler { get; set; }
        public List<SrcnIkEgitimFirmas> IkEgitimFirmalar { get; set; }
        public List<SrcnIkEgitimTipis> IkEgitimTipleri { get; set; }
        public List<SrcnKullanicis> SrcnKullanicilar { get; set; }
        public List<SrcnIkEgitimOturums> EgitimOturumlari { get; set; }
        public List<SrcnIkEgitimKatilimTanim> EgitimKatilimlari { get; set; }
        public List<SrcnIkEgitimOturumSrcnKullanicis> EgitimOturumSrcnKullanicilar { get; set; }

        public List<DropItem> OzelDropMenuler { get; set; }

        public List<PersonelSicilOzetItem> PersonelSicilOzetItemlar { get; set; }

        public SrcnIkFirmas IkFirma { get; set; }
        public SrcnIkEgitims IkEgitim { get; set; }
        public SrcnIkEgitimFirmas IkEgitimFirma { get; set; }
        public SrcnIkEgitimTipis EgitimTipi { get; set; }
        public SrcnKullanicis SrcnKullanici { get; set; }
        public SrcnIkEgitimOturums EgitimOturum { get; set; }

        public SrcnIkEgitimOturumSrcnKullanicis EgitimOturumSrcnKullanici { get; set; }

        public SelectList DropFirmalar { get; set; }
        public SelectList DropEgitimler { get; set; }
        public SelectList DropKullanicilar { get; set; }

        public int SecimId { get; set; }

        
    }

    public class PersonelSicilOzetItem
    {
        public PersonelSicilOzetItem()
        {
            EgitimFirma = new SrcnIkEgitimFirmas();
            EgitimOturum = new SrcnIkEgitimOturums();
            EgitimOturumSrcnKullanici = new SrcnIkEgitimOturumSrcnKullanicis();
        }

        public SrcnIkEgitimFirmas EgitimFirma { get; set; }
        public SrcnIkEgitimOturums EgitimOturum { get; set; }
        public SrcnIkEgitimOturumSrcnKullanicis EgitimOturumSrcnKullanici { get; set; }


    }

    public class PersonelOzlukBilgiModel
    {
        public PersonelOzlukBilgiModel()
        {
            SrcnKullaniciCalismaPozisyonOzetler = new List<ZzzSrcnKullaniciCalismaPozisyonOzet>();
            DbPersonel = new List<Personel>();
            SrcnKullanici = new SrcnKullanicis();
            OzelDropMenuler = new List<DropItem>();
        }
        public List<DropItem> OzelDropMenuler { get; set; }
        public List<ZzzSrcnKullaniciCalismaPozisyonOzet> SrcnKullaniciCalismaPozisyonOzetler { get; set; }
        public List<Personel> DbPersonel { get; set; }
        public SrcnKullanicis SrcnKullanici { get; set; }
        public SelectList DropFabBirimleri { get; set; }
        public SelectList DropCalismaPozisyonlari { get; set; }
        public SelectList DropGruplar { get; set; }
        public SelectList DropVardiyalar { get; set; }
        public SelectList DropFabrikaBolumleri { get; set; }

        public int BirimId { get; set; }
        public int BolumId { get; set; }
        public int  CalismaPozisyonu { get; set; }
        public int GrupId { get; set; }
        public int Vardiya { get; set; }
        public string SicilNo { get; set; }
        public string IsyeriKodu { get; set; }
        public string NufusaKayitliOlduguIl { get; set; }
        public string PersonelKodu { get; set; }

        public string GercekSicilNo { get; set; }

    }

}