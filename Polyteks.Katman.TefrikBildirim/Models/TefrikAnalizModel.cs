using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;

namespace Polyteks.Katman.Has.Models
{
    public class TefrikAnalizModel
    {
        public TefrikAnalizModel()
        {
            MakineTurListe = new List<DropItem>();
            TefrikMakineDurumModelItemlar = new List<TefrikMakineDurumModelItem>();
            MakineTur = new DropItem();
            Makine = new Makine();
            MakineHataBildirimleri = new List<SrcnMakineHataBildirims>();
            SrcnKullanicilar = new List<SrcnKullanicis>();
            GrupPersonelleri = new List<int>();
            SrcnKullanici = new SrcnKullanicis();
            TefrikGrupPersonelModeller = new List<TefrikGrupPersonelModel>();
        }

        public List<TefrikGrupPersonelModel> TefrikGrupPersonelModeller { get; set; }
        public List<DropItem> MakineTurListe { get; set; }
        public DropItem MakineTur { get; set; }
        public List<TefrikMakineDurumModelItem> TefrikMakineDurumModelItemlar { get; set; }
        public Makine Makine { get; set; }
        public List<int> GrupPersonelleri { get; set; }
        public List<SrcnKullanicis> SrcnKullanicilar { get; set; }
        public List<SrcnMakineHataBildirims> MakineHataBildirimleri { get; set; }
        public SrcnKullanicis SrcnKullanici { get; set; }
        public SelectList Kullanicilar { get; set; }
        public string KullaniciKodu { get; set; }
        public string Sifre { get; set; }
        public int SrcnKulaniciId { get; set; }
        public int MakineId { get; set; }
        public int SecilenGrupNo { get; set; }
        public string Veriler { get; set; }

    }
    public class TefrikMakineDurumModelItem
    {
        public TefrikMakineDurumModelItem()
        {
            Makine=new Makine();
        }
        public Makine Makine { get; set; }
        public int HataSayisi { get; set; }
        public int BakilanMiktar { get; set; }
        public int KalanMiktar { get; set; }
        public int OncekiVardiyaHataSayisi  { get; set; }

        public int SuankiVardiyaHataSayisi { get; set; }





    }
}