using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using Polyteks.Katman.Helpers;
using Polyteks.Katman.Has.Models;
using Polyteks.Katman.TefrikBildirim.Models;
using Filter = Polyteks.Katman.Has.Filters.Filter;
using System.Text.RegularExpressions;

namespace Polyteks.Katman.Has.Controllers
{
    [Filter.CookieLoginRequired]
    public class BaseController : Controller
    {
        #region Silme İşlemleri
        public void LabAnalizVeritabanındanSil(int id)
        {
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            var labAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == id).ToList();
            var labAnalizSonucs = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
            var labAnalizlogs = _dbPoly.SrcnLabAnalizLogs.Where(a => a.LabAnalizId == id).ToList();
            var AnalizHareketleri = _dbPoly.SrcnLabAnalizHarekets.Where(a => a.LabAnalizId == id)
                .OrderByDescending(a => a.KayitTarihi).ToList();
            foreach (var item in labAnalizItems)
            {
                labAnalizSonucs.AddRange(_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Where(a => a.LabAnalizItemId == item.LabAnalizItemId).ToList());

            }
            _dbPoly.SrcnLabAnalizLogs.RemoveRange(labAnalizlogs);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.RemoveRange(labAnalizSonucs);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnLabAnalizItems.RemoveRange(labAnalizItems);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnLabAnalizs.Remove(labAnaliz);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnLabAnalizHarekets.RemoveRange(AnalizHareketleri);
            _dbPoly.SaveChanges();

        }
        public void NumuneYapilabilirDosyaSil(int id)
        {
            var numune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            var LabIdler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.DosyaTipi == 2 && a.BagliOlduguDosyaId == id).Select(a => a.LabAnalizId).ToList();
            foreach (var i in LabIdler)
            {
                LabAnalizVeritabanındanSil(i);
            }

            _dbPoly.SrcnNumuneYapilabilirlikDosyas.Remove(numune);
            _dbPoly.SaveChanges();

            /*     var fotolar = _dbPoly.SrcnResims.Where(a => a.BagliOlduguDosyaId == id && a.ResimKategoriId == id2).ToList();
            var dosyaHareketler = _dbPoly.SrcnDosyaHarekets.Where(a => a.DosyaTipi == id2 && a.SikayetNumuneDosyaId == id).ToList();


            foreach (var i in analizIdler)
            {
                LabAnalizVeritabanındanSil(i);
            }

            _dbPoly.SrcnResims.RemoveRange(fotolar);
            _dbPoly.SaveChanges();
            _dbPoly.SrcnDosyaHarekets.RemoveRange(dosyaHareketler);
            _dbPoly.SaveChanges();*/

        }
        #endregion

        public List<DosyaMusteriLabAnalizTablo> MusteriDosyaLabAnalizTabloOlustur(int id)
        {

            var model = new List<DosyaMusteriLabAnalizTablo>();

            {

                var labAnalizler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.DosyaTipi == 3 && a.BagliOlduguDosyaId == id).OrderBy(a => a.PartiNo).ToList();
                var LabIdler = labAnalizler.Select(a => a.LabAnalizId).ToList();

                if (_dbPoly.SrcnLabAnalizItems.AsNoTracking()
                    .Count(a => LabIdler.Any(b => b == a.LabAnalizId)) == 0)
                {
                    // labItemOlustur
                    MusteriSikayetLabItemlariOlustur(labAnalizler);
                }
                var TumLabAnalizItemlar = _dbPoly.SrcnLabAnalizItems.AsNoTracking()
                    .Where(a => LabIdler.Any(b => b == a.LabAnalizId)).ToList();
                var LabAnalizItemIdler = TumLabAnalizItemlar.Select(a => a.LabAnalizItemId).ToList();
                var TumAnalizItemCesitSonuclar = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AsNoTracking()
                    .Where(a => LabAnalizItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();
                var distIdler = TumAnalizItemCesitSonuclar.Select(a => a.LabAnalizCesitId).Distinct().ToList();
                var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 0 && a.Sira > 0)
                    .OrderBy(a => a.Sira).ToList();


                var IplikLabAnalizler = labAnalizler.Where(a => a.AnaliziYapilanBilesenSayisi != 2)
                    .OrderBy(a => a.LabAnalizId).ToList();
                var KumasLabAnalizler = labAnalizler.Where(a => a.AnaliziYapilanBilesenSayisi == 2)
                    .OrderBy(a => a.LabAnalizId).ToList();
                foreach (var ipliklabAnaliz in IplikLabAnalizler)
                {
                    var AtkiLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == ipliklabAnaliz.LabAnalizId && a.MakinePozisyonNo != 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();

                    var aramodel = new DosyaMusteriLabAnalizTablo();

                    aramodel.LabAnaliz = ipliklabAnaliz;
                    var Ipliktablo = new DosyaLabAnalizTablo()
                    {
                        LabAnaliz = ipliklabAnaliz,
                        PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
                        LabAnalizItemlar = AtkiLabAnalizItems,

                    };
                    foreach (var analizCesit in TumIplikAnalizCesitleri)
                    {
                        var tabloSatir = new DosyaLabAnalizTabloSatir
                        {
                            LabAnalizCesit = analizCesit
                        };
                        var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                        foreach (var grpLabAnalizItem in AtkiLabAnalizItems)
                        {
                            if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
                            {
                                analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                            }
                            else
                            {
                                analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                                {
                                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                                    DegerTipi = analizCesit.LabAnalizCesitId,
                                    LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
                                });
                            }
                        }
                        tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
                        Ipliktablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                    }

                    aramodel.AtkiTablo = Ipliktablo;
                    model.Add(aramodel);
                }


                foreach (var kumaslabAnaliz in KumasLabAnalizler)
                {
                    var AtkiLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == kumaslabAnaliz.LabAnalizId && a.MakinePozisyonNo != 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();
                    var CozguLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == kumaslabAnaliz.LabAnalizId && a.MakinePozisyonNo == 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();

                    var aramodel = new DosyaMusteriLabAnalizTablo();

                    aramodel.LabAnaliz = kumaslabAnaliz;
                    var Ipliktablo = new DosyaLabAnalizTablo()
                    {
                        LabAnaliz = kumaslabAnaliz,
                        PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
                        LabAnalizItemlar = AtkiLabAnalizItems,

                    };
                    foreach (var analizCesit in TumIplikAnalizCesitleri)
                    {
                        var tabloSatir = new DosyaLabAnalizTabloSatir
                        {
                            LabAnalizCesit = analizCesit
                        };
                        var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                        foreach (var grpLabAnalizItem in AtkiLabAnalizItems)
                        {
                            if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
                            {
                                analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                            }
                            else
                            {
                                analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                                {
                                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                                    DegerTipi = analizCesit.LabAnalizCesitId,
                                    LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
                                });
                            }
                        }
                        tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
                        Ipliktablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                    }

                    //#region Çözgü

                    //var Cozgutablo = new DosyaLabAnalizTablo()
                    //{
                    //    LabAnaliz = kumaslabAnaliz,
                    //    PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
                    //    LabAnalizItemlar = AtkiLabAnalizItems,

                    //};
                    //foreach (var analizCesit in TumIplikAnalizCesitleri)
                    //{
                    //    var tabloSatir = new DosyaLabAnalizTabloSatir
                    //    {
                    //        LabAnalizCesit = analizCesit
                    //    };
                    //    var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                    //    foreach (var grpLabAnalizItem in CozguLabAnalizItems)
                    //    {
                    //        if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
                    //        {
                    //            analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                    //        }
                    //        else
                    //        {
                    //            analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                    //            {
                    //                LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                    //                DegerTipi = analizCesit.LabAnalizCesitId,
                    //                LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
                    //            });
                    //        }
                    //    }
                    //    tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
                    //    Cozgutablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                    //}

                    //#endregion

                    aramodel.AtkiTablo = Ipliktablo;
                    //aramodel.CozguTablo = Cozgutablo;

                    model.Add(aramodel);
                }


            }

            return model;

        }

        public void GunlukImalatAnalizItemOlustur(int LabAnalizId)
        {
            var LabAnaliz = _dbPoly.SrcnLabAnalizs.Find(LabAnalizId);
            var labGrup = _dbPoly.SrcnLabGrups.Find(LabAnaliz.LabGrupId);
            var analizCesitleri = _dbPoly.SrcnLabGrupAnalizCesits.AsNoTracking().Where(a => a.LabGrupId == labGrup.LabGrupId).OrderBy(a => a.LabAnalizCesitAdi).ToList();

            for (int i = 1; i <= LabAnaliz.AnalizYapilacakBobinSayisi; i++)
            {
                var labAnalizItem = new SrcnLabAnalizItems()
                {
                    LabGrupId = labGrup.LabGrupId,
                    LabGrupAdi = labGrup.LabGrupAdi,
                    LabAnalizId = LabAnaliz.LabAnalizId,
                    KayitTarihi = DateTime.Now,
                    BobinAdi = " BOBIN-" + i.ToString(),
                    IplikNo = i
                };
                _dbPoly.SrcnLabAnalizItems.Add(labAnalizItem);
                _dbPoly.SaveChanges();
                var araListe = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                foreach (var analizCesit in analizCesitleri)
                {
                    araListe.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                    {
                        DegerTipi = analizCesit.DegerTipi,
                        LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                        LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi,
                        LabAnalizItemId = labAnalizItem.LabAnalizItemId,
                        IplikNo = i
                    });
                }
                _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AddRange(araListe);
                _dbPoly.SaveChanges();
            }
        }

        public void NumuneLabItemlariOlustur(List<SrcnLabAnalizs> labAnalizler)
        {

            // makinepozisyon no => 0= atkı, 1=cozgu
            foreach (var item in labAnalizler)
            {
                var liste = new List<SrcnLabAnalizItems>();
                for (int i = 0; i < 5; i++)
                {
                    string BobinAdi = "Ana İplik";

                    if (i != 0)
                    {
                        BobinAdi = i + ". İplik";
                    }
                    liste.Add(new SrcnLabAnalizItems()
                    {
                        MakinePozisyonNo = 0,
                        IplikNo = i,
                        KayitTarihi = DateTime.Now,
                        BobinAdi = BobinAdi,
                        LabAnalizId = item.LabAnalizId,
                        LabGrupId = item.LabGrupId,
                        LabGrupAdi = item.LabGrupAdi

                    });
                }

                if (item.AnaliziYapilanBilesenSayisi == 2)

                {
                    //kumaş
                    for (int i = 0; i < 5; i++)
                    {
                        string BobinAdi = "Ana İplik";

                        if (i != 0)
                        {
                            BobinAdi = i + ". İplik";
                        }
                        liste.Add(new SrcnLabAnalizItems()
                        {
                            MakinePozisyonNo = 1,
                            IplikNo = i,
                            KayitTarihi = DateTime.Now,
                            BobinAdi = BobinAdi,
                            LabAnalizId = item.LabAnalizId,
                            LabGrupId = item.LabGrupId,
                            LabGrupAdi = item.LabGrupAdi

                        });
                    }
                }

                _dbPoly.SrcnLabAnalizItems.AddRange(liste);
                _dbPoly.SaveChanges();
            }

            _dbPoly = new POTA_PTKSEntities();
        }

        public void MusteriSikayetLabItemlariOlustur(List<SrcnLabAnalizs> labAnalizler)
        {

            // makinepozisyon no => 0= atkı, 1=cozgu
            foreach (var item in labAnalizler)
            {
                var liste = new List<SrcnLabAnalizItems>();
                for (int i = 0; i < 5; i++)
                {
                    string BobinAdi = "Ana Hata";

                    if (i != 0)
                    {
                        BobinAdi = i + ". Hatalı";
                    }
                    liste.Add(new SrcnLabAnalizItems()
                    {
                        MakinePozisyonNo = 0,
                        IplikNo = i,
                        KayitTarihi = DateTime.Now,
                        BobinAdi = BobinAdi,
                        LabAnalizId = item.LabAnalizId,
                        LabGrupId = item.LabGrupId,
                        LabGrupAdi = item.LabGrupAdi

                    });
                }

                if (item.AnaliziYapilanBilesenSayisi == 2)

                {
                    //kumaş
                    for (int i = 0; i < 5; i++)
                    {
                        string BobinAdi = "Ana Hata";

                        if (i != 0)
                        {
                            BobinAdi = i + ". Hatalı";
                        }
                        liste.Add(new SrcnLabAnalizItems()
                        {
                            MakinePozisyonNo = 1,
                            IplikNo = i,
                            KayitTarihi = DateTime.Now,
                            BobinAdi = BobinAdi,
                            LabAnalizId = item.LabAnalizId,
                            LabGrupId = item.LabGrupId,
                            LabGrupAdi = item.LabGrupAdi

                        });
                    }
                }

                _dbPoly.SrcnLabAnalizItems.AddRange(liste);
                _dbPoly.SaveChanges();
            }

            _dbPoly = new POTA_PTKSEntities();
        }
        //public List<DosyaNumuneLabAnalizTablo> DosyaMusteriSikayetLabAnalizTablolarıOlustur(int id)
        //{
        //    var model = new List<DosyaNumuneLabAnalizTablo>();

        //    {

        //        var labAnalizler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.DosyaTipi == 3 && a.BagliOlduguDosyaId == id).OrderBy(a => a.PartiNo).ToList();
        //        var LabIdler = labAnalizler.Select(a => a.LabAnalizId).ToList();

        //        if (_dbPoly.SrcnLabAnalizItems.AsNoTracking()
        //            .Count(a => LabIdler.Any(b => b == a.LabAnalizId)) == 0)
        //        {
        //            // labItemOlustur
        //            MusteriSikayetLabItemlariOlustur(labAnalizler);
        //        }

        //        var TumLabAnalizItemlar = _dbPoly.SrcnLabAnalizItems.AsNoTracking()
        //            .Where(a => LabIdler.Any(b => b == a.LabAnalizId)).ToList();
        //        var LabAnalizItemIdler = TumLabAnalizItemlar.Select(a => a.LabAnalizItemId).ToList();
        //        var TumAnalizItemCesitSonuclar = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AsNoTracking()
        //            .Where(a => LabAnalizItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();
        //        var distIdler = TumAnalizItemCesitSonuclar.Select(a => a.LabAnalizCesitId).Distinct().ToList();
        //        var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 0 && a.Sira > 0)
        //            .OrderBy(a => a.Sira).ToList();


        //        var IplikLabAnalizler = labAnalizler.Where(a => a.AnaliziYapilanBilesenSayisi != 2)
        //            .OrderBy(a => a.LabAnalizId).ToList();
        //        var KumasLabAnalizler = labAnalizler.Where(a => a.AnaliziYapilanBilesenSayisi == 2)
        //            .OrderBy(a => a.LabAnalizId).ToList();
        //        foreach (var ipliklabAnaliz in IplikLabAnalizler)
        //        {
        //            var AtkiLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == ipliklabAnaliz.LabAnalizId && a.MakinePozisyonNo != 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();
        //            var CozguLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == ipliklabAnaliz.LabAnalizId && a.MakinePozisyonNo == 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();

        //            var aramodel = new DosyaNumuneLabAnalizTablo();

        //            aramodel.LabAnaliz = ipliklabAnaliz;
        //            var Ipliktablo = new DosyaLabAnalizTablo()
        //            {
        //                LabAnaliz = ipliklabAnaliz,
        //                PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
        //                LabAnalizItemlar = AtkiLabAnalizItems,

        //            };
        //            foreach (var analizCesit in TumIplikAnalizCesitleri)
        //            {
        //                var tabloSatir = new DosyaLabAnalizTabloSatir
        //                {
        //                    LabAnalizCesit = analizCesit
        //                };
        //                var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
        //                foreach (var grpLabAnalizItem in AtkiLabAnalizItems)
        //                {
        //                    if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
        //                    {
        //                        analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
        //                    }
        //                    else
        //                    {
        //                        analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
        //                        {
        //                            LabAnalizCesitId = analizCesit.LabAnalizCesitId,
        //                            DegerTipi = analizCesit.LabAnalizCesitId,
        //                            LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
        //                        });
        //                    }
        //                }
        //                tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
        //                Ipliktablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
        //            }

        //            aramodel.AtkiTablo = Ipliktablo;
        //            model.Add(aramodel);
        //        }


        //        foreach (var kumaslabAnaliz in KumasLabAnalizler)
        //        {
        //            var AtkiLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == kumaslabAnaliz.LabAnalizId && a.MakinePozisyonNo != 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();
        //            var CozguLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == kumaslabAnaliz.LabAnalizId && a.MakinePozisyonNo == 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();

        //            var aramodel = new DosyaNumuneLabAnalizTablo();

        //            aramodel.LabAnaliz = kumaslabAnaliz;
        //            var Ipliktablo = new DosyaLabAnalizTablo()
        //            {
        //                LabAnaliz = kumaslabAnaliz,
        //                PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
        //                LabAnalizItemlar = AtkiLabAnalizItems,

        //            };
        //            foreach (var analizCesit in TumIplikAnalizCesitleri)
        //            {
        //                var tabloSatir = new DosyaLabAnalizTabloSatir
        //                {
        //                    LabAnalizCesit = analizCesit
        //                };
        //                var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
        //                foreach (var grpLabAnalizItem in AtkiLabAnalizItems)
        //                {
        //                    if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
        //                    {
        //                        analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
        //                    }
        //                    else
        //                    {
        //                        analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
        //                        {
        //                            LabAnalizCesitId = analizCesit.LabAnalizCesitId,
        //                            DegerTipi = analizCesit.LabAnalizCesitId,
        //                            LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
        //                        });
        //                    }
        //                }
        //                tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
        //                Ipliktablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
        //            }

        //            #region Çözgü

        //            var Cozgutablo = new DosyaLabAnalizTablo()
        //            {
        //                LabAnaliz = kumaslabAnaliz,
        //                PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
        //                LabAnalizItemlar = AtkiLabAnalizItems,

        //            };
        //            foreach (var analizCesit in TumIplikAnalizCesitleri)
        //            {
        //                var tabloSatir = new DosyaLabAnalizTabloSatir
        //                {
        //                    LabAnalizCesit = analizCesit
        //                };
        //                var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
        //                foreach (var grpLabAnalizItem in CozguLabAnalizItems)
        //                {
        //                    if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
        //                    {
        //                        analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
        //                    }
        //                    else
        //                    {
        //                        analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
        //                        {
        //                            LabAnalizCesitId = analizCesit.LabAnalizCesitId,
        //                            DegerTipi = analizCesit.LabAnalizCesitId,
        //                            LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
        //                        });
        //                    }
        //                }
        //                tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
        //                Cozgutablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
        //            }

        //            #endregion

        //            aramodel.AtkiTablo = Ipliktablo;
        //            aramodel.CozguTablo = Cozgutablo;

        //            model.Add(aramodel);
        //        }


        //    }

        //    return model;
        //}
        public List<DosyaNumuneLabAnalizTablo> DosyaNumuneLabAnalizTablolariOlustur(int id)
        {
            var model = new List<DosyaNumuneLabAnalizTablo>();

            {

                var labAnalizler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.DosyaTipi == 2 && a.BagliOlduguDosyaId == id).OrderBy(a => a.PartiNo).ToList();
                var LabIdler = labAnalizler.Select(a => a.LabAnalizId).ToList();

                if (_dbPoly.SrcnLabAnalizItems.AsNoTracking()
                    .Count(a => LabIdler.Any(b => b == a.LabAnalizId)) == 0)
                {
                    // labItemOlustur
                    NumuneLabItemlariOlustur(labAnalizler);
                }

                var TumLabAnalizItemlar = _dbPoly.SrcnLabAnalizItems.AsNoTracking()
                    .Where(a => LabIdler.Any(b => b == a.LabAnalizId)).ToList();
                var LabAnalizItemIdler = TumLabAnalizItemlar.Select(a => a.LabAnalizItemId).ToList();
                var TumAnalizItemCesitSonuclar = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AsNoTracking()
                    .Where(a => LabAnalizItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();
                var distIdler = TumAnalizItemCesitSonuclar.Select(a => a.LabAnalizCesitId).Distinct().ToList();
                var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 0 && a.Sira > 0)
                    .OrderBy(a => a.Sira).ToList();


                var IplikLabAnalizler = labAnalizler.Where(a => a.AnaliziYapilanBilesenSayisi != 2)
                    .OrderBy(a => a.LabAnalizId).ToList();
                var KumasLabAnalizler = labAnalizler.Where(a => a.AnaliziYapilanBilesenSayisi == 2)
                    .OrderBy(a => a.LabAnalizId).ToList();
                foreach (var ipliklabAnaliz in IplikLabAnalizler)
                {
                    var AtkiLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == ipliklabAnaliz.LabAnalizId && a.MakinePozisyonNo != 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();
                    var CozguLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == ipliklabAnaliz.LabAnalizId && a.MakinePozisyonNo == 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();

                    var aramodel = new DosyaNumuneLabAnalizTablo();

                    aramodel.LabAnaliz = ipliklabAnaliz;
                    var Ipliktablo = new DosyaLabAnalizTablo()
                    {
                        LabAnaliz = ipliklabAnaliz,
                        PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
                        LabAnalizItemlar = AtkiLabAnalizItems,

                    };
                    foreach (var analizCesit in TumIplikAnalizCesitleri)
                    {
                        var tabloSatir = new DosyaLabAnalizTabloSatir
                        {
                            LabAnalizCesit = analizCesit
                        };
                        var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                        foreach (var grpLabAnalizItem in AtkiLabAnalizItems)
                        {
                            if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
                            {
                                analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                            }
                            else
                            {
                                analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                                {
                                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                                    DegerTipi = analizCesit.LabAnalizCesitId,
                                    LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
                                });
                            }
                        }
                        tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
                        Ipliktablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                    }

                    aramodel.AtkiTablo = Ipliktablo;
                    model.Add(aramodel);
                }


                foreach (var kumaslabAnaliz in KumasLabAnalizler)
                {
                    var AtkiLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == kumaslabAnaliz.LabAnalizId && a.MakinePozisyonNo != 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();
                    var CozguLabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == kumaslabAnaliz.LabAnalizId && a.MakinePozisyonNo == 1).OrderBy(a => a.MakinePozisyonNo).ThenBy(a => a.IplikNo).ToList();

                    var aramodel = new DosyaNumuneLabAnalizTablo();

                    aramodel.LabAnaliz = kumaslabAnaliz;
                    var Ipliktablo = new DosyaLabAnalizTablo()
                    {
                        LabAnaliz = kumaslabAnaliz,
                        PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
                        LabAnalizItemlar = AtkiLabAnalizItems,

                    };
                    foreach (var analizCesit in TumIplikAnalizCesitleri)
                    {
                        var tabloSatir = new DosyaLabAnalizTabloSatir
                        {
                            LabAnalizCesit = analizCesit
                        };
                        var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                        foreach (var grpLabAnalizItem in AtkiLabAnalizItems)
                        {
                            if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
                            {
                                analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                            }
                            else
                            {
                                analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                                {
                                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                                    DegerTipi = analizCesit.LabAnalizCesitId,
                                    LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
                                });
                            }
                        }
                        tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
                        Ipliktablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                    }

                    #region Çözgü

                    var Cozgutablo = new DosyaLabAnalizTablo()
                    {
                        LabAnaliz = kumaslabAnaliz,
                        PartiSonuTakipIzlenebilirlik = new ZzzSrcnPartiSonuTakipIzlenebilirlik(),
                        LabAnalizItemlar = AtkiLabAnalizItems,

                    };
                    foreach (var analizCesit in TumIplikAnalizCesitleri)
                    {
                        var tabloSatir = new DosyaLabAnalizTabloSatir
                        {
                            LabAnalizCesit = analizCesit
                        };
                        var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                        foreach (var grpLabAnalizItem in CozguLabAnalizItems)
                        {
                            if (TumAnalizItemCesitSonuclar.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId))
                            {
                                analizCesitSonuclar.Add(TumAnalizItemCesitSonuclar.First(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                            }
                            else
                            {
                                analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                                {
                                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                                    DegerTipi = analizCesit.LabAnalizCesitId,
                                    LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
                                });
                            }
                        }
                        tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar;
                        Cozgutablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                    }

                    #endregion

                    aramodel.AtkiTablo = Ipliktablo;
                    aramodel.CozguTablo = Cozgutablo;

                    model.Add(aramodel);
                }


            }

            return model;
        }
        public List<DosyaLabAnalizTablo> DosyaLabAnalizTablolariOlustur(int Tip, int id)
        {
            //tip=> 1=günlük imalat, 2- numune yapılabilirlik, 3- denemeDosyaItem
            if (Tip == 1)
            {
                var araModel = new List<DosyaLabAnalizTablo>();
                var labAnalizler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.DosyaTipi == 1 && a.BagliOlduguDosyaId == id).OrderBy(a => a.PartiNo).ToList();
                var LabIdler = labAnalizler.Select(a => a.LabAnalizId).ToList();

                var TumLabAnalizItemlar = _dbPoly.SrcnLabAnalizItems.AsNoTracking()
                    .Where(a => LabIdler.Any(b => b == a.LabAnalizId)).ToList();
                var LabAnalizItemIdler = TumLabAnalizItemlar.Select(a => a.LabAnalizItemId).ToList();

                var TumAnalizItemCesitSonuclar = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AsNoTracking()
                    .Where(a => LabAnalizItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();

                var distIdler = TumAnalizItemCesitSonuclar.Select(a => a.LabAnalizCesitId).Distinct().ToList();
                var TumAnalizCesitler = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => distIdler.Any(b => b == a.LabAnalizCesitId)).OrderBy(a => a.Sira).ToList();
                // var TumAnalizCesitler = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0 && a.LabAnalizCesitId != 40 && a.LabAnalizCesitId != 41 && a.Sira > 0).OrderBy(a => a.Sira).ToList();

                foreach (var labAnaliz in labAnalizler)
                {

                    var LabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId).ToList();
                    var grupLabAnalizItemlar = new List<SrcnLabAnalizItems>();
                    int BobinSay = 0;
                    foreach (var aa in LabAnalizItems)
                    {
                        BobinSay++;
                        grupLabAnalizItemlar.Add(aa);
                        bool TabloOlusturulabilirMi = false;
                        if (BobinSay == 10)
                        {
                            TabloOlusturulabilirMi = true;
                            BobinSay = 0;
                        }
                        if (aa == LabAnalizItems.Last())
                        {
                            // son item kontrol et var mi 
                            if (grupLabAnalizItemlar.Any())
                            {
                                TabloOlusturulabilirMi = true;
                            }
                        }

                        if (TabloOlusturulabilirMi)
                        {
                            grupLabAnalizItemlar = grupLabAnalizItemlar.OrderBy(a => a.IplikNo).ToList();
                            // tablo hazırlanabilir
                            var tablo = new DosyaLabAnalizTablo()
                            {
                                LabAnaliz = labAnaliz,
                                PartiSonuTakipIzlenebilirlik = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.First(a => a.RefakatNo.Trim() == labAnaliz.RefakartKartNo.Trim()),
                                LabAnalizItemlar = grupLabAnalizItemlar
                            };
                            foreach (var analizCesit in TumAnalizCesitler)
                            {

                                var tabloSatir = new DosyaLabAnalizTabloSatir
                                {
                                    LabAnalizCesit = analizCesit
                                };
                                var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                                foreach (var grpLabAnalizItem in grupLabAnalizItemlar)
                                {
                                    var liste = TumAnalizItemCesitSonuclar.Where(a =>
                                        a.LabAnalizCesitId == analizCesit.LabAnalizCesitId &&
                                        a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId).ToList();

                                    if (liste.Count(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId) != 0)
                                    {
                                        if (analizCesitSonuclar.Count(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId) == 0)
                                        {
                                            analizCesitSonuclar.Add(liste.First(a =>
                                                a.LabAnalizCesitId == analizCesit.LabAnalizCesitId &&
                                                a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                                        }
                                    }
                                    else
                                    {
                                        analizCesitSonuclar.Add(new SrcnLabAnalizItemAnalizCesitSonucs()
                                        {
                                            LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                                            LabAnalizItemId = grpLabAnalizItem.LabAnalizItemId
                                        }
                                        );

                                    }


                                }

                                if (analizCesitSonuclar.Any())
                                {
                                    tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar.OrderBy(a => a.IplikNo).ToList();
                                    tablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                                }
                            }
                            araModel.Add(tablo);
                            grupLabAnalizItemlar = new List<SrcnLabAnalizItems>();
                            TabloOlusturulabilirMi = false;
                        }

                    }
                }
                return araModel;
            }

            if (Tip == 3)
            {
                //id = denemeDosyaItemId
                var araModel = new List<DosyaLabAnalizTablo>();
                var labAnalizler = _dbPoly.SrcnLabAnalizs.AsNoTracking().Where(a => a.DosyaTipi == 3 && a.BagliOlduguDosyaId == id).OrderBy(a => a.PartiNo).ToList();
                var LabIdler = labAnalizler.Select(a => a.LabAnalizId).ToList();

                var TumLabAnalizItemlar = _dbPoly.SrcnLabAnalizItems.AsNoTracking()
                    .Where(a => LabIdler.Any(b => b == a.LabAnalizId)).ToList();
                var LabAnalizItemIdler = TumLabAnalizItemlar.Select(a => a.LabAnalizItemId).ToList();

                var TumAnalizItemCesitSonuclar = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AsNoTracking()
                    .Where(a => LabAnalizItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();

                var distIdler = TumAnalizItemCesitSonuclar.Select(a => a.LabAnalizCesitId).Distinct().ToList();
                var TumAnalizCesitler = _dbPoly.SrcnLabAnalizCesits.AsNoTracking()
                    .Where(a => distIdler.Any(b => b == a.LabAnalizCesitId)).OrderBy(a => a.Sira).ToList();

                foreach (var labAnaliz in labAnalizler)
                {

                    var LabAnalizItems = TumLabAnalizItemlar.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId).ToList();
                    var grupLabAnalizItemlar = new List<SrcnLabAnalizItems>();
                    int BobinSay = 0;
                    foreach (var aa in LabAnalizItems)
                    {
                        BobinSay++;
                        grupLabAnalizItemlar.Add(aa);
                        bool TabloOlusturulabilirMi = false;
                        if (BobinSay == 6)
                        {
                            TabloOlusturulabilirMi = true;
                            BobinSay = 0;
                        }
                        if (aa == LabAnalizItems.Last())
                        {
                            // son item kontrol et var mi 
                            if (grupLabAnalizItemlar.Any())
                            {
                                TabloOlusturulabilirMi = true;
                            }
                        }

                        if (TabloOlusturulabilirMi)
                        {
                            grupLabAnalizItemlar = grupLabAnalizItemlar.OrderBy(a => a.IplikNo).ToList();
                            // tablo hazırlanabilir
                            var tablo = new DosyaLabAnalizTablo()
                            {
                                LabAnaliz = labAnaliz,

                                LabAnalizItemlar = grupLabAnalizItemlar
                            };
                            foreach (var analizCesit in TumAnalizCesitler)
                            {
                                var tabloSatir = new DosyaLabAnalizTabloSatir
                                {
                                    LabAnalizCesit = analizCesit
                                };
                                var analizCesitSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                                foreach (var grpLabAnalizItem in grupLabAnalizItemlar)
                                {
                                    analizCesitSonuclar.AddRange(TumAnalizItemCesitSonuclar.Where(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == grpLabAnalizItem.LabAnalizItemId));
                                }

                                if (analizCesitSonuclar.Any())
                                {
                                    tabloSatir.LabAnalizItemAnalizCesitSonuclari = analizCesitSonuclar.OrderBy(a => a.IplikNo).ToList();
                                    tablo.DosyaLabAnalizTabloSatirlar.Add(tabloSatir);
                                }
                            }
                            araModel.Add(tablo);
                            grupLabAnalizItemlar = new List<SrcnLabAnalizItems>();
                            TabloOlusturulabilirMi = false;
                        }

                    }
                }
                return araModel;
            }

            return new List<DosyaLabAnalizTablo>();
        }
        public string FormNoOlustur(int id, int tip)
        {
            var sonuc = "";

            //tip => 0= günlük imalat, 1=numune yapılabilirlik;

            var DropItemlar = new List<DropItem>();
            if (tip == 0)
            {
                // günlük imalat
                DropItemlar.Add(new DropItem() { DigerTanim = "Günlük İmalat TEKSTÜRE", IdInt = 1, Tanim = "f.160.43" });
                DropItemlar.Add(new DropItem() { DigerTanim = "Günlük İmalat FANTAZİ", IdInt = 2, Tanim = "f.160.44" });
                DropItemlar.Add(new DropItem() { DigerTanim = "Günlük İmalat Poy/Fdy", IdInt = 3, Tanim = "f.160.42" });
                DropItemlar.Add(new DropItem() { DigerTanim = "Günlük İmalat TEKNİK TEKSTİL", IdInt = 4, Tanim = "f.160.45" });
            }

            if (tip == 1)
            {
                //numune yapılabilirlik;
                DropItemlar.Add(new DropItem() { DigerTanim = "Numune Yapılabilirlik - İPLİK", IdInt = 1, Tanim = "FN-2/01" });
                DropItemlar.Add(new DropItem() { DigerTanim = "Numune Yapılabilirlik - KUMAŞ", IdInt = 2, Tanim = "FN-3/01" });
            }



            sonuc = DropItemlar.First(a => a.IdInt == id).Tanim;

            return sonuc.ToUpper();
        }

        public FileStreamResult MusteriSikayetAnaliziPDFOlustur(int id)
        {
            var musteriDosyaId = _dbPoly.SrcnMusteriSikayetDosyas.Find(id);
            var tablolar = MusteriDosyaLabAnalizTabloOlustur(id);
            #region Harici Fontlar
            Font beyazRenk = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            beyazRenk.SetStyle("Bold");
            beyazRenk.Color = BaseColor.WHITE;
            beyazRenk.Size = 10;
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion

            #region Tablo Genel Bilgileri
            PdfPTable pdfBaslik = new PdfPTable(1);
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;

            pdfBaslik.AddCell(new Phrase(string.Format("MÜŞTERİ ŞİKAYETİ"), baslik));


            #endregion


            var tabloAnalizler = new List<PdfPTable>();
            var tabloAnalizUstBilgiler = new List<PdfPTable>();

            #region Resim Yukleme
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            jpg.ScaleToFit(150, 25);
            //Give space before image 
            //jpg.SpacingBefore = 50f;
            ////Give some space after the image 
            //jpg.SpacingAfter = 13f;


            #endregion



            foreach (var tabloNumune in tablolar)
            {
                PdfPTable pdfGenelBilgiler = new PdfPTable(4);
                pdfGenelBilgiler.DefaultCell.BorderWidth = 0;
                pdfGenelBilgiler.AddCell(new Phrase("Dosya Takip Kodu / Tarih", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(string.Format("#{0} / {1}", musteriDosyaId.MusteriSikayetDosyaId, musteriDosyaId.KayitTarihi.ToShortDateString()), kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("Cari", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(musteriDosyaId.CariAdi, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("İstekte Bulunan", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(musteriDosyaId.KayitYapanKulAdi, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("Analiz  Kodu", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase("#" + tabloNumune.LabAnaliz.LabAnalizId, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("Parti No", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(musteriDosyaId.PartiNo, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("Satış Yorumu", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(musteriDosyaId.Aciklama, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("Laboratuvar Yorumu", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(musteriDosyaId.LabAciklama, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("İşletme Yorumu", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(musteriDosyaId.IsletmeAciklama, kalinharici));
                tabloAnalizUstBilgiler.Add(pdfGenelBilgiler);

                var araTablo = new PdfPTable(2);

                var yeniTablo = new PdfPTable(6);
                var yeniTabloCozgu = new PdfPTable(6);
                #region İplik veya Atkı Bölümü
                string tabloBaslik = "BOBİN ANALİZİ";
                if (tabloNumune.LabAnaliz.AnaliziYapilanBilesenSayisi == 2)
                {
                    tabloBaslik = "KUMAŞ ANALİZİ";
                }
                var celltabloBaslik = new PdfPCell(new Phrase(tabloBaslik, kalinharici));
                celltabloBaslik.HorizontalAlignment = Element.ALIGN_CENTER;
                celltabloBaslik.VerticalAlignment = Element.ALIGN_CENTER;
                celltabloBaslik.Colspan = 6;
                yeniTablo.AddCell(celltabloBaslik);
                yeniTablo.AddCell(new PdfPCell(new Phrase("Analiz", kalinharici)));


                foreach (var j in tabloNumune.AtkiTablo.LabAnalizItemlar)
                {
                    var cellDegiskenFormNo = new PdfPCell(new Phrase(j.BobinAdi, beyazRenk));
                    cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_CENTER;

                    cellDegiskenFormNo.BackgroundColor = BaseColor.BLACK;
                    yeniTablo.AddCell(cellDegiskenFormNo);
                }

                foreach (var i in tabloNumune.AtkiTablo.DosyaLabAnalizTabloSatirlar)
                {
                    var Cell = new PdfPCell(new Phrase(i.LabAnalizCesit.LabAnalizCesitAdi, kalinharici));
                    Cell.FixedHeight = 17f;
                    Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Cell.VerticalAlignment = Element.ALIGN_CENTER;
                    yeniTablo.AddCell(Cell);
                    foreach (var cesitSonuc in i.LabAnalizItemAnalizCesitSonuclari)
                    {
                        var bCell = new PdfPCell(new Phrase(cesitSonuc.AnalizDegeriString, kalinharici));
                        bCell.FixedHeight = 17f;
                        bCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        bCell.VerticalAlignment = Element.ALIGN_CENTER;
                        yeniTablo.AddCell(bCell);

                    }

                }
                #endregion


      
                // header

                var genislikler = new float[] { 3f, 1f, 1f, 1f, 1f, 1f };
                yeniTablo.SetWidths(genislikler);
                araTablo.AddCell(yeniTablo);
                if (tabloNumune.LabAnaliz.AnaliziYapilanBilesenSayisi == 2)
                {
                    yeniTabloCozgu.SetWidths(genislikler);
                    araTablo.AddCell(yeniTabloCozgu);
                    var cellDegiskenFormNo = new PdfPCell(new Phrase(FormNoOlustur(2, 1), hariciFont));
                    cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.Colspan = 2;
                    cellDegiskenFormNo.BorderWidth = 0;
                    araTablo.AddCell(cellDegiskenFormNo);
                }
                else
                {
                    araTablo.AddCell(new PdfPCell(new Phrase("", kalinharici)));
                    var cellDegiskenFormNo = new PdfPCell(new Phrase(FormNoOlustur(1, 1), hariciFont));
                    cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.Colspan = 2;
                    cellDegiskenFormNo.BorderWidth = 0;
                    araTablo.AddCell(cellDegiskenFormNo);
                }

                tabloAnalizler.Add(araTablo);
            }







            #region PDF Olusturma Alani 
            using (Document pdfDoc =
                new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    var genelBilgiWidths = new float[] { 2f, 3f, 1f, 4f };


                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();


                    for (int i = 0; i < tabloAnalizler.Count; i++)
                    {
                        tabloAnalizler[i].WidthPercentage = 99;
                        pdfDoc.Add(jpg);
                        pdfDoc.Add(pdfBaslik);
                        //pdfDoc.Add(pdfGenelBilgiler);
                        tabloAnalizUstBilgiler[i].WidthPercentage = 99;
                        tabloAnalizUstBilgiler[i].SetWidths(genelBilgiWidths);
                        pdfDoc.Add(tabloAnalizUstBilgiler[i]);
                        pdfDoc.Add(tabloAnalizler[i]);


                        if (tabloAnalizler[i] != tabloAnalizler.Last())
                        {
                            pdfDoc.NewPage();
                        }



                    }


                    //pdfDoc.Add(pdfAnalizSonuc);
                    //pdfDoc.Add(pdfAciklama);
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }
                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required
                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), musteriDosyaId.MusteriSikayetDosyaId));
            }

            #endregion
        }
        public FileStreamResult NumuneYapilabilirlikPdfOlustur(int id)
        {
            var numuneYapilabilirlik = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
            var tablolar = DosyaNumuneLabAnalizTablolariOlustur(id);
            #region Harici Fontlar
            Font beyazRenk = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            beyazRenk.SetStyle("Bold");
            beyazRenk.Color = BaseColor.WHITE;
            beyazRenk.Size = 10;
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion

            #region Tablo Genel Bilgileri
            PdfPTable pdfBaslik = new PdfPTable(1);
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;

            pdfBaslik.AddCell(new Phrase(string.Format("NUMUNE YAPILABİLİRLİK"), baslik));


            #endregion


            var tabloAnalizler = new List<PdfPTable>();
            var tabloAnalizUstBilgiler = new List<PdfPTable>();

            #region Resim Yukleme
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            jpg.ScaleToFit(150, 25);
            //Give space before image 
            //jpg.SpacingBefore = 50f;
            ////Give some space after the image 
            //jpg.SpacingAfter = 13f;


            #endregion



            foreach (var tabloNumune in tablolar)
            {
                PdfPTable pdfGenelBilgiler = new PdfPTable(4);
                pdfGenelBilgiler.DefaultCell.BorderWidth = 0;
                pdfGenelBilgiler.AddCell(new Phrase("Dosya Takip Kodu / Tarih", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(string.Format("#{0} / {1}", numuneYapilabilirlik.NumuneYapilabilirlikDosyaId, numuneYapilabilirlik.KayitTarihi.ToShortDateString()), kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("Cari", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(numuneYapilabilirlik.CariAdi, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("İstekte Bulunan", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(numuneYapilabilirlik.KayitYapanKulAdi, kalinharici));
                pdfGenelBilgiler.AddCell(new Phrase("Analiz  Kodu", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase("#" + tabloNumune.LabAnaliz.LabAnalizId, kalinharici));
                tabloAnalizUstBilgiler.Add(pdfGenelBilgiler);

                var araTablo = new PdfPTable(2);

                var yeniTablo = new PdfPTable(6);
                var yeniTabloCozgu = new PdfPTable(6);
                #region İplik veya Atkı Bölümü
                string tabloBaslik = "İPLİK ANALİZİ";
                if (tabloNumune.LabAnaliz.AnaliziYapilanBilesenSayisi == 2)
                {
                    tabloBaslik = "ATKI ANALİZİ";
                }
                var celltabloBaslik = new PdfPCell(new Phrase(tabloBaslik, kalinharici));
                celltabloBaslik.HorizontalAlignment = Element.ALIGN_CENTER;
                celltabloBaslik.VerticalAlignment = Element.ALIGN_CENTER;
                celltabloBaslik.Colspan = 6;
                yeniTablo.AddCell(celltabloBaslik);
                yeniTablo.AddCell(new PdfPCell(new Phrase("Analiz", kalinharici)));


                foreach (var j in tabloNumune.AtkiTablo.LabAnalizItemlar)
                {
                    var cellDegiskenFormNo = new PdfPCell(new Phrase(j.BobinAdi, beyazRenk));
                    cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_CENTER;

                    cellDegiskenFormNo.BackgroundColor = BaseColor.BLACK;
                    yeniTablo.AddCell(cellDegiskenFormNo);
                }

                foreach (var i in tabloNumune.AtkiTablo.DosyaLabAnalizTabloSatirlar)
                {
                    var Cell = new PdfPCell(new Phrase(i.LabAnalizCesit.LabAnalizCesitAdi, kalinharici));
                    Cell.FixedHeight = 17f;
                    Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Cell.VerticalAlignment = Element.ALIGN_CENTER;
                    yeniTablo.AddCell(Cell);
                    foreach (var cesitSonuc in i.LabAnalizItemAnalizCesitSonuclari)
                    {
                        var bCell = new PdfPCell(new Phrase(cesitSonuc.AnalizDegeriString, kalinharici));
                        bCell.FixedHeight = 17f;
                        bCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        bCell.VerticalAlignment = Element.ALIGN_CENTER;
                        yeniTablo.AddCell(bCell);

                    }

                }
                #endregion


                #region Çözgü Bölümü

                if (tabloNumune.LabAnaliz.AnaliziYapilanBilesenSayisi == 2)
                {


                    var celltabloBaslikCozgu = new PdfPCell(new Phrase("ÇÖZGÜ ANALİZİ", kalinharici));
                    celltabloBaslikCozgu.HorizontalAlignment = Element.ALIGN_CENTER;
                    celltabloBaslikCozgu.VerticalAlignment = Element.ALIGN_CENTER;
                    celltabloBaslikCozgu.Colspan = 6;
                    yeniTabloCozgu.AddCell(celltabloBaslikCozgu);
                    yeniTabloCozgu.AddCell(new PdfPCell(new Phrase("Analiz", kalinharici)));


                    foreach (var j in tabloNumune.CozguTablo.LabAnalizItemlar)
                    {
                        var cellDegiskenFormNo = new PdfPCell(new Phrase(j.BobinAdi, beyazRenk));
                        cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_CENTER;

                        cellDegiskenFormNo.BackgroundColor = BaseColor.BLACK;
                        yeniTabloCozgu.AddCell(cellDegiskenFormNo);
                    }

                    foreach (var i in tabloNumune.CozguTablo.DosyaLabAnalizTabloSatirlar)
                    {
                        var Cell = new PdfPCell(new Phrase(i.LabAnalizCesit.LabAnalizCesitAdi, kalinharici));
                        Cell.FixedHeight = 17f;
                        Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        Cell.VerticalAlignment = Element.ALIGN_CENTER;
                        yeniTabloCozgu.AddCell(Cell);
                        foreach (var cesitSonuc in i.LabAnalizItemAnalizCesitSonuclari)
                        {
                            var bCell = new PdfPCell(new Phrase(cesitSonuc.AnalizDegeriString, kalinharici));
                            bCell.FixedHeight = 17f;
                            bCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bCell.VerticalAlignment = Element.ALIGN_CENTER;
                            yeniTabloCozgu.AddCell(bCell);

                        }

                    }

                }
                #endregion
                // header

                var genislikler = new float[] { 3f, 1f, 1f, 1f, 1f, 1f };
                yeniTablo.SetWidths(genislikler);
                araTablo.AddCell(yeniTablo);
                if (tabloNumune.LabAnaliz.AnaliziYapilanBilesenSayisi == 2)
                {
                    yeniTabloCozgu.SetWidths(genislikler);
                    araTablo.AddCell(yeniTabloCozgu);
                    var cellDegiskenFormNo = new PdfPCell(new Phrase(FormNoOlustur(2, 1), hariciFont));
                    cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.Colspan = 2;
                    cellDegiskenFormNo.BorderWidth = 0;
                    araTablo.AddCell(cellDegiskenFormNo);
                }
                else
                {
                    araTablo.AddCell(new PdfPCell(new Phrase("", kalinharici)));
                    var cellDegiskenFormNo = new PdfPCell(new Phrase(FormNoOlustur(1, 1), hariciFont));
                    cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.Colspan = 2;
                    cellDegiskenFormNo.BorderWidth = 0;
                    araTablo.AddCell(cellDegiskenFormNo);
                }

                tabloAnalizler.Add(araTablo);
            }







            #region PDF Olusturma Alani 
            using (Document pdfDoc =
                new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    var genelBilgiWidths = new float[] { 2f, 3f, 1f, 4f };


                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();


                    for (int i = 0; i < tabloAnalizler.Count; i++)
                    {
                        tabloAnalizler[i].WidthPercentage = 99;
                        pdfDoc.Add(jpg);
                        pdfDoc.Add(pdfBaslik);
                        //pdfDoc.Add(pdfGenelBilgiler);
                        tabloAnalizUstBilgiler[i].WidthPercentage = 99;
                        tabloAnalizUstBilgiler[i].SetWidths(genelBilgiWidths);
                        pdfDoc.Add(tabloAnalizUstBilgiler[i]);
                        pdfDoc.Add(tabloAnalizler[i]);


                        if (tabloAnalizler[i] != tabloAnalizler.Last())
                        {
                            pdfDoc.NewPage();
                        }



                    }


                    // pdfDoc.Add(pdfAnalizSonuc);
                    // pdfDoc.Add(pdfAciklama);
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }
                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required
                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), numuneYapilabilirlik.NumuneYapilabilirlikDosyaId));
            }

            #endregion
        }
        public FileStreamResult GunlukImalatDosyaPdfOlusturYeni(int id)
        {
            var gunlukImalatDosya = _dbPoly.SrcnGunlukImalatDosyas.Find(id);
            var tablolar = DosyaLabAnalizTablolariOlustur(1, id);
            #region Harici Fontlar
            Font beyazRenk = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            beyazRenk.SetStyle("Bold");
            beyazRenk.Color = BaseColor.WHITE;
            beyazRenk.Size = 10;
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion

            #region Tablo Genel Bilgileri



            PdfPTable pdfBaslik = new PdfPTable(1);
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;
            PdfPTable pdfGenelBilgiler = new PdfPTable(4);
            PdfPTable pdfGenelBilgiOzet = new PdfPTable(4);
            pdfBaslik.AddCell(new Phrase(string.Format("{0} GÜNLÜK İMALAT", gunlukImalatDosya.KayitYapanKulAdi.ToUpper().Trim()), baslik));


            pdfGenelBilgiler.DefaultCell.BorderWidth = 0;
            pdfGenelBilgiler.AddCell(new Phrase("Dosya Takip Kodu / Tarih", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(string.Format("#{0} / {1}", gunlukImalatDosya.GunlukImalatDosyaId, gunlukImalatDosya.KayitTarihi.ToShortDateString()), kalinharici));
            pdfGenelBilgiler.AddCell(new Phrase("İstekte Bulunan", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(gunlukImalatDosya.KayitYapanKulAdi, kalinharici));



            #endregion

            var araTablo = new PdfPTable(2);
            var tabloAnalizler = new List<PdfPTable>();
            var tabloAnalizUstBilgiler = new List<PdfPTable>();

            #region Resim Yukleme
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            jpg.ScaleToFit(150, 25);
            //Give space before image 
            //jpg.SpacingBefore = 50f;
            ////Give some space after the image 
            //jpg.SpacingAfter = 13f;


            #endregion
            int oncekiId = 0;
            int baslikOncekiId = 0;

            var SutunAnalizTablolar = new List<int>();
            var TumTablolarIdler = new List<List<int>>();

            var tumLabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
            int sutunSay = 0;
            int analizSutunSayisi = 0;

            int k = -1;
            foreach (var tablo in tablolar)
            {
                k++;

                analizSutunSayisi = tablo.LabAnalizItemlar.Count;

                if ((sutunSay + analizSutunSayisi) <= 8)
                {
                    sutunSay += analizSutunSayisi;
                    SutunAnalizTablolar.Add(k);
                }
                else
                {
                    TumTablolarIdler.Add(SutunAnalizTablolar);
                    SutunAnalizTablolar = new List<int>();
                    sutunSay = 0;
                }

                if (tablo == tablolar.Last())
                {
                    if (TumTablolarIdler.Count == 0)
                    {
                        TumTablolarIdler.Add(SutunAnalizTablolar);
                    }
                }

                foreach (var TabloSatir in tablo.DosyaLabAnalizTabloSatirlar)
                {
                    if (tumLabAnalizCesitleri.Count(a => a.LabAnalizCesitId == TabloSatir.LabAnalizCesit.LabAnalizCesitId) == 0)
                    {
                        tumLabAnalizCesitleri.Add(TabloSatir.LabAnalizCesit);
                    }

                }

            }
            var yeniTablo = new PdfPTable(4);

            //for (int i = 1; i < 4; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        for (int k = 0; k < 2; k++)
            //        {
            //            var bCell = new PdfPCell(new Phrase(string.Format("{0}.{1}.{2}",i,j,k), kalinharici));
            //            bCell.FixedHeight = 17f;
            //            bCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //            bCell.VerticalAlignment = Element.ALIGN_CENTER;

            //            yeniTablo.AddCell(bCell);
            //        }
            //    }

            //}



            foreach (var i in TumTablolarIdler)
            {
                // toparlanmış tablo
                yeniTablo = new PdfPTable(i.Count + 1);
                yeniTablo.AddCell(new PdfPCell(new Phrase("Analiz", kalinharici)));
                foreach (var j in i)
                {
                    var cellDegiskenFormNo = new PdfPCell(new Phrase(tablolar[j].LabAnaliz.PartiNo, beyazRenk)); cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.Colspan = tablolar[j].LabAnalizItemlar.Count;
                    cellDegiskenFormNo.BorderWidth = 0;
                    cellDegiskenFormNo.BackgroundColor = BaseColor.BLACK;
                    yeniTablo.AddCell(cellDegiskenFormNo);
                }

                foreach (var baseAnalizCesit in tumLabAnalizCesitleri)
                {
                    var Cell = new PdfPCell(new Phrase(baseAnalizCesit.LabAnalizCesitAdi, kalinharici));
                    Cell.FixedHeight = 17f;
                    Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    Cell.VerticalAlignment = Element.ALIGN_CENTER;
                    yeniTablo.AddCell(Cell);


                    foreach (var j in i)
                    {
                        var araYeniSatir = new PdfPTable(tablolar[j].LabAnalizItemlar.Count);



                        int analizDegerSonucu = 0;

                        foreach (var TabloSatir in tablolar[j].DosyaLabAnalizTabloSatirlar)
                        {
                            if (TabloSatir.LabAnalizCesit.LabAnalizCesitId == baseAnalizCesit.LabAnalizCesitId)
                            {

                                foreach (var hucre in TabloSatir.LabAnalizItemAnalizCesitSonuclari)
                                {
                                    analizDegerSonucu++;
                                    var bCell = new PdfPCell(new Phrase(hucre.AnalizDegeriString, kalinharici));
                                    bCell.FixedHeight = 17f;
                                    bCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    bCell.VerticalAlignment = Element.ALIGN_CENTER;

                                    //yeniTablo.AddCell(bCell);
                                    araYeniSatir.AddCell(bCell);
                                }
                            }


                        }

                        if (analizDegerSonucu == 0)
                        {
                            var bCell = new PdfPCell(new Phrase(" ", kalinharici));
                            bCell.FixedHeight = 17f;
                            bCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            bCell.VerticalAlignment = Element.ALIGN_CENTER;

                            yeniTablo.AddCell(bCell);
                        }
                        else
                        {
                            yeniTablo.AddCell(araYeniSatir);
                        }

                    }
                }

                tabloAnalizler.Add(yeniTablo);
            }





            #region PDF Olusturma Alani 
            using (Document pdfDoc =
                new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    var genelBilgiWidths = new float[] { 2f, 3f, 1f, 3f };
                    pdfGenelBilgiler.SetWidths(genelBilgiWidths);
                    //pdfGenelBilgiler.HorizontalAlignment = Element.ALIGN_LEFT;
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();


                    for (int i = 0; i < tabloAnalizler.Count; i++)
                    {
                        tabloAnalizler[i].WidthPercentage = 99;
                        pdfDoc.Add(jpg);
                        pdfDoc.Add(pdfBaslik);
                        pdfDoc.Add(pdfGenelBilgiler);
                        //tabloAnalizUstBilgiler[i].SetWidths(genelBilgiWidths);
                        //pdfDoc.Add(tabloAnalizUstBilgiler[i]);
                        pdfDoc.Add(tabloAnalizler[i]);


                        if (tabloAnalizler[i] != tabloAnalizler.Last())
                        {
                            pdfDoc.NewPage();
                        }



                    }


                    // pdfDoc.Add(pdfAnalizSonuc);
                    // pdfDoc.Add(pdfAciklama);
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }
                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required
                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), gunlukImalatDosya.GunlukImalatDosyaId));
            }

            #endregion
        }

        public FileStreamResult GunlukImalatDosyaPdfOlustur(int id)
        {
            var gunlukImalatDosya = _dbPoly.SrcnGunlukImalatDosyas.Find(id);

            #region Harici Fontlar
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion
            #region Tablo Genel Bilgileri

            PdfPTable pdfBaslik = new PdfPTable(1);
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;
            PdfPTable pdfGenelBilgiler = new PdfPTable(4);

            PdfPTable pdfGenelBilgiOzet = new PdfPTable(4);
            pdfBaslik.AddCell(new Phrase(string.Format("{0} GÜNLÜK İMALAT ANALİZİ", gunlukImalatDosya.BirimAdi.ToUpper().Trim()), baslik));


            pdfGenelBilgiler.DefaultCell.BorderWidth = 0;
            pdfGenelBilgiler.AddCell(new Phrase("Takip Kodu / Tarih", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(string.Format("#{0} / {1}", gunlukImalatDosya.GunlukImalatDosyaId, gunlukImalatDosya.KayitTarihi.ToShortDateString()), kalinharici));
            pdfGenelBilgiler.AddCell(new Phrase("İstekte Bulunan", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(gunlukImalatDosya.KayitYapanKulAdi, kalinharici));



            #endregion
            var tabloAnalizler = new List<PdfPTable>();
            var tabloAnalizUstBilgiler = new List<PdfPTable>();
            #region Resim Yukleme
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            jpg.ScaleToFit(150, 25);
            //Give space before image 
            //jpg.SpacingBefore = 50f;
            ////Give some space after the image 
            //jpg.SpacingAfter = 13f;


            #endregion

            var labAnalizler = _dbPoly.SrcnLabAnalizs
                .Where(a => a.ImalatAnalizYapilmaTipi == 1 && a.BagliOlduguDosyaId == id).OrderBy(a => a.PartiNo)
                .ToList();
            int ToplamAnaliz = labAnalizler.Count;
            int SiradakiAnaliz = 0;
            int sayfaSay = 0;
            foreach (var labAnaliz in labAnalizler)
            {
                SiradakiAnaliz++;
                var SecilenLabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
                var labAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == labAnaliz.LabAnalizId)
                    .OrderBy(a => a.IplikNo).ToList();

                var LabAnalizDetayModeller = new List<LabAnalizDetayModel>();
                var LabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
                var TumLabAnalizItemAnalizCesitSonuclari = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                var toplamBobinSayisi = labAnalizItems.Count;
                foreach (var analizItem in labAnalizItems)
                {
                    var labAnalizItemAnalizCesitSonuclari = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                        .Where(a => a.LabAnalizItemId == analizItem.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi)
                        .ToList();
                    var item = new LabAnalizDetayModel()
                    {
                        LabAnalizItem = analizItem,
                        LabAnalizItemAnalizCesitSonuclari = labAnalizItemAnalizCesitSonuclari
                    };

                    TumLabAnalizItemAnalizCesitSonuclari.AddRange(labAnalizItemAnalizCesitSonuclari);


                    foreach (var aa in item.LabAnalizItemAnalizCesitSonuclari)
                    {
                        if (SecilenLabAnalizCesitleri.Count(a => a.LabAnalizCesitId == aa.LabAnalizCesitId && a.LabAnalizCesitAdi == aa.LabAnalizCesitAdi) == 0)
                        {
                            SecilenLabAnalizCesitleri.Add(new SrcnLabAnalizCesits()
                            {
                                LabAnalizCesitAdi = aa.LabAnalizCesitAdi,
                                LabAnalizCesitId = aa.LabAnalizCesitId
                            });
                        }

                    }
                    LabAnalizDetayModeller.Add(item);

                }
                var distinctLabAnalizCesitler = TumLabAnalizItemAnalizCesitSonuclari.Select(a => a.LabAnalizCesitId)
                    .Distinct().ToList();
                LabAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits
                    .Where(a => distinctLabAnalizCesitler.Any(b => b == a.LabAnalizCesitId)).OrderBy(a => a.Sira)
                    .ToList();
                int BobinSay = 0;
                var araModel = new List<List<SrcnLabAnalizItems>>();
                var araBisey = new List<SrcnLabAnalizItems>();
                foreach (var aa in labAnalizItems)
                {
                    BobinSay++;
                    araBisey.Add(aa);
                    if (BobinSay == 10)
                    {
                        araModel.Add(araBisey);
                        araBisey = new List<SrcnLabAnalizItems>();
                        BobinSay = 0;
                    }


                    if (aa == labAnalizItems.Last())
                    {
                        // son item kontrol et var mi 
                        if (araBisey.Any())
                        {
                            araModel.Add(araBisey);
                            araBisey = new List<SrcnLabAnalizItems>();
                        }

                    }

                }
                var ToplamSayfa = araModel.Count;
                foreach (var araItem in araModel)
                {
                    sayfaSay++;
                    var yeniTablo = new PdfPTable(1);
                    if (araItem.Count >= 10)
                    {
                        yeniTablo = new PdfPTable(11);
                    }
                    else
                    {
                        yeniTablo = new PdfPTable(araItem.Count + 1);
                    }

                    var AnalizCell = new PdfPCell(new Phrase("ANALİZ", kalinharici));
                    AnalizCell.FixedHeight = 17f;
                    AnalizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    AnalizCell.VerticalAlignment = Element.ALIGN_CENTER;

                    yeniTablo.AddCell(AnalizCell);

                    List<float> yeniTabloWidths = new List<float>();
                    yeniTabloWidths.Add(3);
                    foreach (var item in araItem)
                    {
                        var analizCell = new PdfPCell(new Phrase(item.BobinAdi, kalinharici));
                        analizCell.FixedHeight = 17f;
                        analizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        analizCell.VerticalAlignment = Element.ALIGN_CENTER;

                        yeniTablo.AddCell(analizCell);
                        yeniTabloWidths.Add(1);
                    }

                    foreach (var item in LabAnalizCesitleri.OrderBy(a => a.Sira).ToList())
                    {
                        var LabAnalizCesitAdiCell = new PdfPCell(new Phrase(item.LabAnalizCesitAdi, kalinharici));
                        LabAnalizCesitAdiCell.FixedHeight = 17f;
                        LabAnalizCesitAdiCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        LabAnalizCesitAdiCell.VerticalAlignment = Element.ALIGN_CENTER;
                        yeniTablo.AddCell(LabAnalizCesitAdiCell);


                        foreach (var labAnalizItem in araItem)
                        {

                            var analizCesitSonuc = TumLabAnalizItemAnalizCesitSonuclari.First(a =>
                                a.LabAnalizCesitId == item.LabAnalizCesitId &&
                                a.LabAnalizItemId == labAnalizItem.LabAnalizItemId);

                            var analizCell = new PdfPCell(new Phrase(analizCesitSonuc.AnalizDegeriString, kalinharici));
                            analizCell.FixedHeight = 17f;
                            analizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            analizCell.VerticalAlignment = Element.ALIGN_CENTER;

                            yeniTablo.AddCell(analizCell);

                            // yeniTablo.AddCell(new Phrase(analizCesitSonuc.AnalizDegeriString, kalinharici));
                        }

                    }
                    yeniTablo.SetWidths(yeniTabloWidths.ToArray());



                    #region değişken FormNo

                    var cellDegiskenFormNo = new PdfPCell(new Phrase(FormNoOlustur(gunlukImalatDosya.BirimId, 0), hariciFont));
                    cellDegiskenFormNo.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.VerticalAlignment = Element.ALIGN_LEFT;
                    cellDegiskenFormNo.Colspan = araItem.Count + 1;
                    cellDegiskenFormNo.BorderWidth = 0;
                    yeniTablo.AddCell(cellDegiskenFormNo);

                    #endregion
                    yeniTablo.WidthPercentage = 99;


                    tabloAnalizler.Add(yeniTablo);

                    pdfGenelBilgiOzet = new PdfPTable(4);
                    pdfGenelBilgiOzet.DefaultCell.BorderWidth = 0;
                    pdfGenelBilgiOzet.AddCell(new Phrase("Parti / İŞ EMRİ ", hariciFont));
                    pdfGenelBilgiOzet.AddCell(new Phrase(string.Format("{0} / {1}", labAnaliz.PartiNo, labAnaliz.RefakartKartNo.Trim()), kalinharici));
                    pdfGenelBilgiOzet.AddCell(new Phrase("Sayfa - Analiz", hariciFont));
                    pdfGenelBilgiOzet.AddCell(new Phrase($"{ToplamAnaliz}.{SiradakiAnaliz}.{ToplamSayfa}-{sayfaSay}. Sayfa ", kalinharici));
                    pdfGenelBilgiOzet.AddCell(new Phrase("TAKIM SAATİ ", hariciFont));
                    pdfGenelBilgiOzet.AddCell(new Phrase(string.Format("{0}", labAnaliz.TakimSaati), kalinharici));
                    pdfGenelBilgiOzet.AddCell(new Phrase("KANAL NO ", hariciFont));
                    pdfGenelBilgiOzet.AddCell(new Phrase(string.Format("{0}", labAnaliz.KanalNo), kalinharici));


                    if (_dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Any(a => a.RefakatNo.Trim() == labAnaliz.RefakartKartNo.Trim()))
                    {
                        var izlenebilirlik =
                            _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.First(a =>
                                a.RefakatNo.Trim() == labAnaliz.RefakartKartNo.Trim());

                        pdfGenelBilgiOzet.AddCell(new Phrase("Stok Adı", hariciFont));
                        pdfGenelBilgiOzet.AddCell(new Phrase(izlenebilirlik.StokAdi.Trim(), kalinharici));
                        pdfGenelBilgiOzet.AddCell(new Phrase("Makine", hariciFont));
                        pdfGenelBilgiOzet.AddCell(new Phrase($"{izlenebilirlik.MakineNo.Trim()} - {izlenebilirlik.MakineAdi.Trim()}", kalinharici));

                    }
                    tabloAnalizUstBilgiler.Add(pdfGenelBilgiOzet);
                    int araSay = 0;
                }


            }
            #region PDF Olusturma Alani 
            using (Document pdfDoc =
                new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    var genelBilgiWidths = new float[] { 1f, 3f, 1f, 3f };
                    pdfGenelBilgiler.SetWidths(genelBilgiWidths);
                    //pdfGenelBilgiler.HorizontalAlignment = Element.ALIGN_LEFT;
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();


                    for (int i = 0; i < tabloAnalizler.Count; i++)
                    {

                        pdfDoc.Add(jpg);
                        pdfDoc.Add(pdfBaslik);
                        pdfDoc.Add(pdfGenelBilgiler);
                        tabloAnalizUstBilgiler[i].SetWidths(genelBilgiWidths);
                        pdfDoc.Add(tabloAnalizUstBilgiler[i]);
                        pdfDoc.Add(tabloAnalizler[i]);


                        if (tabloAnalizler[i] != tabloAnalizler.Last())
                        {
                            pdfDoc.NewPage();
                        }



                    }


                    // pdfDoc.Add(pdfAnalizSonuc);
                    // pdfDoc.Add(pdfAciklama);
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }
                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required

                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), gunlukImalatDosya.GunlukImalatDosyaId));
            }

            #endregion

        }
        public static string StripHTML(string htmlString)
        {
            var input = Regex.Replace(htmlString, "<.*?>", String.Empty);



            input = input.Replace("&Ccedil;", "Ç");
            input = input.Replace("&#286;", "Ğ");
            input = input.Replace("&#304;", "İ");
            input = input.Replace("&Ouml;", "Ö");
            input = input.Replace("&#350;", "Ş");
            input = input.Replace("&Uuml;", "Ü");

            input = input.Replace("&ccedil;", "ç");
            input = input.Replace("&#287;", "ğ");
            input = input.Replace("&#305;", "ı");
            input = input.Replace("&ouml;", "ö");
            input = input.Replace("&#351;", "ş");
            input = input.Replace("&uuml;", "ü");
            input = input.Replace("&nbsp;", " ");


            return input;
        }

        public FileStreamResult PaketlemeGunlukTalimatPdf(int GunlukTalimatId)
        {
            var talimat = _dbPoly.SrcnPaketlemeGunlukTalimats.Find(GunlukTalimatId);

            #region Harici Fontlar
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion
            #region Resim Yukleme


            iTextSharp.text.Image personelResim;


            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            logo.ScaleToFit(150, 45);


            //  logo.Alignment = Image.UNDERLYING;

            #endregion
            PdfPTable pdfBaslik = new PdfPTable(1);
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;
            pdfBaslik.AddCell(new Phrase("", baslik));
            pdfBaslik.AddCell(new Phrase("", baslik));
            pdfBaslik.AddCell(new Phrase(string.Format("{0} - {1} GÜNLÜK TALİMAT", talimat.TalimatTarihi, talimat.PaketlemeGunlukTalimatTipiAdi), baslik));
            pdfBaslik.AddCell(new Phrase("", baslik));
            var pdfAlinanEgitimler = new PdfPTable(3);
            pdfAlinanEgitimler.WidthPercentage = 95;
            pdfAlinanEgitimler.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfAlinanEgitimler.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;

            int say = 0;

            var tabloBaslik = new List<string>();
            tabloBaslik.Add("#");
            tabloBaslik.Add("Talimat Başlığı");
            tabloBaslik.Add("Talimat İçeriği");

            foreach (var s in tabloBaslik)
            {
                var analizCell = new PdfPCell(new Phrase(s, kalinharici));
                analizCell.FixedHeight = 21f;
                analizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                analizCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfAlinanEgitimler.AddCell(analizCell);
            }

            foreach (var s in _dbPoly.SrcnPaketlemeGunlukTalimatItems.Where(a => a.PaketlemeGunlukTalimatId == talimat.PaketlemeGunlukTalimatId)
                .OrderBy(a => a.TalimatBaslik).ToList())
            {
                say++;
                pdfAlinanEgitimler.AddCell(new PdfPCell(new Phrase(say.ToString(), kalinharici)) { /*FixedHeight = 21f,*/ HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER, FixedHeight = 70 });
                pdfAlinanEgitimler.AddCell(new PdfPCell(new Phrase(s.TalimatBaslik, kalinharici)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });



                pdfAlinanEgitimler.AddCell(new PdfPCell(new Phrase(StripHTML(s.TalimatIcerik), kalinharici)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });

            }



            #region PDF Olusturma Alani 

            using (Document pdfDoc =
                new Document(PageSize.A4, 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();


                try
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();
                    pdfDoc.Add(logo);
                    pdfDoc.Add(pdfBaslik);
                    float[] widths = new float[] { 5f, 10f, 70f };
                    pdfAlinanEgitimler.SetWidths(widths);

                    pdfDoc.Add(pdfAlinanEgitimler);
                    pdfDoc.NewPage();
                    pdfDoc.Add(pdfAlinanEgitimler);

                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }

                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required

                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), "1"));
            }

            #endregion
        }
        #region Mail Gönderme Detaylar
        public string PartiSonuOzetTabloOlustur(List<RefakatKarti> refListe)
        {
            var sonuc = "<table border=\"1\">";

            var theadItems = new List<string>();
            var tbodyItems = new List<string>();
            theadItems.Add("CARİ");
            theadItems.Add("PARTİ");
            theadItems.Add("SİPARİŞ NO");
            theadItems.Add("SIRA");
            theadItems.Add("İŞ EMRİ");
            theadItems.Add("STOK KODU");
            theadItems.Add("STOK ADI");
            theadItems.Add("SİPARİŞ TÜRÜ");
            theadItems.Add("SİP. MİKTAR");
            theadItems.Add("Ürt. Başl. Tar");
            theadItems.Add("ÜRT.  MİKTAR");
            theadItems.Add("İŞLEM");
            theadItems.Add("MAKİNE");
            string theadOlustur = "<thead><tr>";
            foreach (var s in theadItems)
            {
                theadOlustur += "<th>" + s + "</th>";
            }

            theadOlustur += "</tr></thead>";
            string tbodyOlustur = "<tbody>";

            foreach (var refakatKarti in refListe)
            {
                tbodyOlustur += "<tr>";
                tbodyItems = new List<string>();
                var item = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.First(z => z.RefakatNo == refakatKarti.RefakatNo);

                tbodyItems.Add(item.CariAdiSecili);
                tbodyItems.Add(item.PartiNo);
                tbodyItems.Add(item.SiparisNo);
                tbodyItems.Add(item.SiparisSiraNo.ToString());
                tbodyItems.Add(item.RefakatNo);
                tbodyItems.Add(item.StokKodu);
                tbodyItems.Add(item.StokAdi);
                tbodyItems.Add(item.SiparisTuru);
                tbodyItems.Add(item.Miktar.ToString());
                tbodyItems.Add(item.IslemBaslamaTarihi.ToString());
                tbodyItems.Add(item.NetKG.ToString());
                tbodyItems.Add(item.IslemAdi);
                tbodyItems.Add(item.MakineNo);
               


                foreach (var s in tbodyItems)
                {
                    tbodyOlustur += "<td>" + s + "</td>";
                }

                tbodyOlustur += "</tr>";
            }

            tbodyOlustur += "</tbody>";

            sonuc += theadOlustur;
            sonuc += tbodyOlustur;
            return sonuc += "</table>";
        }
        public bool GenelDurumMailGonder(string ccKullanici, List<string> Gonderilecekler, string mailIcerik, string konu)
        {
            string GonderenMail = "pota_bilgi@polyteks.com.tr";
            string MailSifre = "ptks1986";


            var body = mailIcerik;
            var message = new MailMessage();


            if (Gonderilecekler.Any())
            {
                foreach (var s in Gonderilecekler)
                {
                    message.To.Add(new MailAddress(s));

                }
            }

            if (ccKullanici != null)
            {
                message.CC.Add(new MailAddress(ccKullanici));
            }
            message.From = new MailAddress(GonderenMail);
            message.Subject = konu;
            message.Body = body;
            message.Priority = MailPriority.High;
            message.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = GonderenMail,
                        Password = MailSifre
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "10.10.1.5";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                    return true;
                }
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return false;
            }
        }
        public bool SistemMailGonder()
        {
            string GonderenMail = "pota_bilgi@polyteks.com.tr";
            string MailSifre = "ptks1986";
            // string GonderenMail = mailKullanici.EmailAdresi;
            // string MailSifre = mailKullanici.EmailSifre;

            var body = "<p>DENEME</p>";


            var message = new MailMessage();

            message.To.Add(new MailAddress("kvural@polyteks.com.tr"));
            message.CC.Add(new MailAddress("edogan@polyteks.com.tr"));
            message.From = new MailAddress(GonderenMail);
            message.Subject = "Sistem Mail Gönderme Deneme - ";
            message.Body = body;
            message.Priority = MailPriority.High;
            message.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = GonderenMail,
                        Password = MailSifre
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "10.10.1.5";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);

                    return true;

                }
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return false;
            }
        }
        #endregion
        #region Metotlar



        public void partiSonuBilgiDegisiklikYap(int PartiSonuTakipBilgiId, int PartiSonuTakipId)
        {
            var partiSonuTakipBilgiOnay = new SrcnPartiSonuTakipBilgiOnays();
            var partisonuTakip = new SrcnPartiSonuTakips();
            string mailKonu = "";
            string mailIcerik = "";
            var refListe = new List<RefakatKarti>();
            var ccMailler = new List<string>();
            string Gonderici = "kvural@polyteks.com.tr";
            bool MailGondermeSorunVarmi = false;


            if (PartiSonuTakipBilgiId == 0)
            {
                partisonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(PartiSonuTakipId);
                if (partisonuTakip.PartiSonuTakipHareketTipi == 6)
                {
                    // parti sonu yapıldı
                    ccMailler.Add("planlama@polyteks.local");
                    ccMailler.Add("edogan@polyteks.com.tr");
                    ccMailler.Add("sevkiyat@polyteks.local");
                    ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == 8 || a.MailBildirimGrupId == 10).Select(a => a.EmailAdres).ToList());
                }
                refListe.AddRange(_dbPoly.RefakatKarti.Where(a => a.RefakatNo == partisonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemNo != "900").ToList());
            }
            else
            {
                partiSonuTakipBilgiOnay = _dbPoly.SrcnPartiSonuTakipBilgiOnays.Find(PartiSonuTakipBilgiId);
                partisonuTakip = _dbPoly.SrcnPartiSonuTakips.Find(partiSonuTakipBilgiOnay.PartiSonuTakipId);

                var BirimId = partiSonuTakipBilgiOnay.BirimId;
                if (_dbPoly.SrcnMailBildirimGrups.Any(a => a.MailBildirimGrupId == BirimId))
                {
                    if (_dbPoly.SrcnMailBildirimGrupItems.Any(a => a.MailBildirimGrupId == BirimId))
                    {
                        ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == BirimId).Select(a => a.EmailAdres).ToList());
                    }
                    else
                    {
                        MailGondermeSorunVarmi = true;
                    }
                }

                refListe.AddRange(_dbPoly.RefakatKarti.Where(a => a.RefakatNo == partisonuTakip.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemBitisTarihi == null && a.IslemNo != "900").ToList());

            }

            if (Kullanici.EmailAdres != null)
            {
                if (Kullanici.EmailAdres.Contains("@polyteks.com.tr"))
                {
                    Gonderici = Kullanici.EmailAdres;
                }

            }
            if (partisonuTakip.BirimId != 0)
            {
                var BirimId = partisonuTakip.BirimId;
                if (_dbPoly.SrcnMailBildirimGrups.Any(a => a.MailBildirimGrupId == BirimId))
                {
                    if (_dbPoly.SrcnMailBildirimGrupItems.Any(a => a.MailBildirimGrupId == BirimId))
                    {
                        ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == BirimId).Select(a => a.EmailAdres).ToList());
                    }
                    else
                    {
                        MailGondermeSorunVarmi = true;
                    }

                }
                else
                {
                    MailGondermeSorunVarmi = true;
                }

            }
            if (MailGondermeSorunVarmi)
            {
                ccMailler.Add("kvural@polyteks.com.tr");
            }
            if (PartiSonuTakipBilgiId == 0)
            {
                mailKonu = partisonuTakip.PartiNo.Replace(" ", "") + " PARTİ SONU DURUM DEĞİŞİKLİĞİ";
                mailIcerik = string.Format("PAKETLEME DURUM DEĞİŞİKLİĞİ YAPILMIŞTIR. </br> YENİ DURUM: {0}</br>", partisonuTakip.PartiSonuTakipHareketAdi);
            }
            else
            {
                mailKonu = partisonuTakip.PartiNo.Replace(" ", "") + " PARTİ SONU ONAY DURUM DEĞİŞİKLİĞİ";
                if (partiSonuTakipBilgiOnay.OnayapanKulId == 0)
                {
                    // onay geri çekildi
                    mailIcerik = "VERİLEN PARTİ ONAYI GERİ ÇEKİLMİŞTİR. </br>";
                }
                else
                {
                    // onay verildi
                    mailIcerik = "PARTİ SONU TAKİP İÇİN BİLGİ ONAYI VERİLMİŞTİR. </br>";
                }

                if (partisonuTakip.PartiSonuTakipHareketTipi == 3)
                {
                    ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == 8).Select(a => a.EmailAdres).ToList());
                }
            }
            mailIcerik += PartiSonuOzetTabloOlustur(refListe);
            mailIcerik += "</br> " + Kullanici.IsimSoyisim;

            if (Kullanici.BirimId != null)
            {
                if (_dbPoly.SrcnMailBildirimGrupItems.Any(a => a.MailBildirimGrupId == Kullanici.BirimId))
                {
                    ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == Kullanici.BirimId).Select(a => a.EmailAdres).ToList());
                }
            }
            GenelDurumMailGonder(Gonderici, ccMailler.Distinct().ToList(), mailIcerik, mailKonu);
        }


        public List<PersonelSicilOzetItem> PersonelSicilOzetItemlarOlustur(int id)
        {
            var model = new List<PersonelSicilOzetItem>();
            var kul = _dbPoly.SrcnKullanicis.Find(id);

            var EgitimPersoneller =
                _dbPoly.SrcnIkEgitimOturumSrcnKullanicis.AsNoTracking().Where(a => a.SrcnKullaniciId == id).ToList();
            var OturumIdler = EgitimPersoneller.Select(a => a.EgitimOturumId).Distinct().ToList();

            var AnaOturumlar = _dbPoly.SrcnIkEgitimOturums.AsNoTracking().Where(a => OturumIdler.Any(b => b == a.EgitimOturumId))
                .OrderByDescending(a => a.OturumTarihiDateTime).ToList();
            var EgitimFirmaIdler = AnaOturumlar.Select(a => a.IkEgitimFirmaId).Distinct().ToList();
            var EgitimFirmalar = _dbPoly.SrcnIkEgitimFirmas.AsNoTracking().Where(a => EgitimFirmaIdler.Any(b => b == a.IkEgitimFirmaId))
                .ToList();

            foreach (var egtPers in EgitimPersoneller)
            {
                var anaoturum = AnaOturumlar.First(a => a.EgitimOturumId == egtPers.EgitimOturumId);
                var egitimFirma = EgitimFirmalar.First(a => a.IkEgitimFirmaId == anaoturum.IkEgitimFirmaId);
                model.Add(new PersonelSicilOzetItem
                {
                    EgitimOturum = anaoturum,
                    EgitimFirma = egitimFirma,
                    EgitimOturumSrcnKullanici = egtPers
                });
            }
            return model.
                OrderByDescending(a => a.EgitimOturum.OturumTarihiDateTime).ThenBy(a => a.EgitimFirma.IkEgitimTipiAdi)
                .ThenBy(a => a.EgitimFirma.IkEgitimAdi).ThenBy(a => a.EgitimFirma.IkFirmaAdi).ToList();
        }


        public void RecursiveLabGrups(int UstGrupId)
        {
            if (UstGrupId != 0)
            {
                var labgrup = _dbPoly.SrcnLabGrups.Find(UstGrupId);
                LabGruplari.Add(labgrup);
                RecursiveLabGrups(labgrup.UstLabGrupId);
            }
        }
        public void TempDataOlustur(string Mesaj, bool OlumLuMu)
        {
            TempData["Msg"] = Mesaj;
            TempData["class"] = "danger";
            if (OlumLuMu)
            {
                TempData["class"] = "success";
            }
        }

        /// <summary>
        /// 1-Create
        /// 2-Update
        /// 3-Delete
        /// </summary>
        /// <param name="Durum"></param>
        public void TempDataCRUDOlustur(int Durum)
        {
            string Mesaj = "Kayıt İşlemi Yapılmıştır";
            TempData["class"] = "success";
            bool OlumLuMu = true;

            if (Durum == 2)
            {
                // güncelleme
                Mesaj = "Güncelleme İşlemi Yapılmıştır";
            }
            if (Durum == 3)
            {
                // sİLME
                Mesaj = "Silme İşlemi Yapılmıştır";
                OlumLuMu = false;
            }
            TempData["Msg"] = Mesaj;

            if (OlumLuMu == false)
            {
                TempData["class"] = "danger";
            }
        }
        #endregion
        public void PartiSonuTakipKontrolEt()
        {
            var IsemriKapatmaSilinecekListe = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Where(a => a.PartiSonuTakipHareketTipi > 0 && a.PartiSonuTakipId != null).Where(a => a.PartiSonuTakipHareketTipi != 6 && a.PartiSonuTakipHareketTipi != 7 && a.PartiSonuTakipHareketTipi != 8 && a.YapilacakPartiSonuIslemTipi == 2 && a.IslemBitisTarihi != null).Select(a => a.RefakatNo).ToList();

            var IsemriKapatilacakListe = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Where(a => a.PartiSonuTakipHareketTipi > 0)
                .Where(a => (a.PartiSonuTakipHareketTipi == 6 && a.IslemBitisTarihi == null)).Select(a => a.RefakatNo).ToList();

            var IsemriOtomatikKapatilacakListe = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Where(a => a.YapilacakPartiSonuIslemTipi == 3 && a.PartiSonuTakipHareketTipi == 8 && a.IslemBitisTarihi == null).Select(a => a.RefakatNo).ToList();

            var DirekKapatilacakListe = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik
                .Where(a => a.YapilacakPartiSonuIslemTipi == 1 && a.IslemBitisTarihi == null).Select(a => a.RefakatNo).ToList();
            foreach (var i in IsemriKapatmaSilinecekListe)
            {
                if (_dbPoly.RefakatKarti.Any(
                    a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi != null))
                {
                    var item = _dbPoly.RefakatKarti.First(
                        a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi != null);
                    item.IslemBitisTarihi = null;
                    _dbPoly.SaveChanges();
                }
            }
            foreach (var i in DirekKapatilacakListe)
            {

                if (_dbPoly.RefakatKarti.Any(
                    a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null))
                {
                    var item = _dbPoly.RefakatKarti.First(
                        a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null);
                    item.IslemBitisTarihi = DateTime.Now;
                    _dbPoly.SaveChanges();
                }
            }

            foreach (var i in IsemriKapatilacakListe)
            {

                if (_dbPoly.RefakatKarti.Any(
                    a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null))
                {
                    var item = _dbPoly.RefakatKarti.First(
                        a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null);
                    item.IslemBitisTarihi = DateTime.Now;
                    _dbPoly.SaveChanges();
                }
            }
            foreach (var i in IsemriOtomatikKapatilacakListe)
            {

                if (_dbPoly.RefakatKarti.Any(
                    a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null))
                {
                    var item = _dbPoly.RefakatKarti.First(
                        a => a.RefakatNo == i && a.IslemSiraNo == 100 && a.IslemBitisTarihi == null);
                    item.IslemBitisTarihi = DateTime.Now;
                    _dbPoly.SaveChanges();
                }
            }


            foreach (var i in _dbPoly.SrcnPartiSonuTakips.AsNoTracking().Where(a => a.PartiSonuTakipHareketTipi == 6 && a.KapatanKulId == null).Select(a => a.PartiSonuTakipId).Distinct().ToList())
            {
                var item = _dbPoly.SrcnPartiSonuTakips.Find(i);
                item.KapatanKulId = 780;
                item.PartiKapatmaTarihi = DateTime.Now;
                item.KapatanKulAdi = "Kutay VURAL - Admin";
                _dbPoly.SaveChanges();
            }


            var referansTarih = DateTime.Now.AddHours(-36);


            var kontrolListe = _dbPoly.SrcnPartiSonuTakips
                .Where(a => a.YapilacakPartiSonuIslemTipi == 3 && a.PartiKapatmaTarihi == null &&
                            a.KayitTarihi < referansTarih).Select(a => new { a.PartiSonuTakipId, a.RefakatNo }).ToList();
            foreach (var item in kontrolListe)
            {
                var aradurum = _dbPoly.SrcnPartiSonuTakipHareketTakipTanims.Find(8);
                var editItem = _dbPoly.SrcnPartiSonuTakips.Find(item.PartiSonuTakipId);
                editItem.PartiSonuTakipHareketTipi = aradurum.PartiSonuTakipHareketTipi;
                editItem.PartiSonuTakipHareketAdi = aradurum.PartiSonuTakipHareketAdi;
                editItem.PartiKapatmaTarihi = DateTime.Now;
                editItem.KapatanKulId = 0;
                editItem.KapatanKulAdi = "Sistem";
                _dbPoly.SaveChanges();
                var refListe = new List<RefakatKarti>();
                string mailKonu = "";
                string mailIcerik = "";
                var ccMailler = new List<string>();
                var ccOnayMailler = new List<string>();
                string Gonderici = "kvural@polyteks.com.tr";



                var refKart = _dbPoly.RefakatKarti.AsNoTracking().First(a => a.RefakatNo == item.RefakatNo && a.IslemSiraNo == 100 && a.IslemNo != "001" && a.IslemNo != "900");
                refKart.SabitlenenMakineNo = "1-srcn";
                refKart.IslemBitisTarihi = DateTime.Now;
                mailKonu = refKart.RefakatNo.Trim() + " İŞ EMRİ KAPATILAN İMALAT";
                _dbPoly.SaveChanges();
                refListe.Add(refKart);

                bool MailGondermeSorunVarmi = false;
                if (Kullanici.BirimId != null)
                {
                    var BirimId = Convert.ToInt32(editItem.BirimId);
                    if (_dbPoly.SrcnMailBildirimGrups.Any(a => a.MailBildirimGrupId == BirimId))
                    {
                        if (_dbPoly.SrcnMailBildirimGrupItems.Any(a => a.MailBildirimGrupId == BirimId))
                        {
                            ccMailler.AddRange(_dbPoly.SrcnMailBildirimGrupItems.Where(a => a.MailBildirimGrupId == BirimId).Select(a => a.EmailAdres).ToList());
                        }
                        else
                        {
                            MailGondermeSorunVarmi = true;
                        }

                    }
                    else
                    {
                        MailGondermeSorunVarmi = true;
                    }

                }
                else
                {
                    MailGondermeSorunVarmi = true;
                }
                mailIcerik = PartiSonuOzetTabloOlustur(refListe);

                if (MailGondermeSorunVarmi)
                {
                    ccMailler.Add("kvural@polyteks.com.tr");
                }


                if (Kullanici.EmailAdres != null)
                {
                    if (Kullanici.EmailAdres.Contains("@polyteks.com.tr"))
                    {
                        Gonderici = Kullanici.EmailAdres;
                    }
                }

                if (ccOnayMailler.Any())
                {
                    ccMailler.AddRange(ccOnayMailler);
                }
                GenelDurumMailGonder(Gonderici, ccMailler, mailIcerik, mailKonu);

            }

        }
        public FileStreamResult PdfPersonelSicilOlustur(int KullaniciId)
        {
            var personel = _dbPoly.SrcnKullanicis.Find(KullaniciId);
            var PersonelSicilOzetItemlar = PersonelSicilOzetItemlarOlustur(KullaniciId);

            #region Harici Fontlar
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion
            #region Resim Yukleme


            iTextSharp.text.Image personelResim;

            if (personel.Resim == null)
            {
                personelResim = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/fotoyok.png"));
            }
            else
            {
                personelResim = iTextSharp.text.Image.GetInstance(personel.Resim);
            }
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            logo.ScaleToFit(250, 45);


            logo.Alignment = Image.UNDERLYING;
            //Resize image depend upon your need 
            personelResim.ScaleToFit(80, 100);


            personelResim.Alignment = Image.RIGHT_ALIGN;
            #endregion
            PdfPTable pdfBaslik = new PdfPTable(1);
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;
            pdfBaslik.AddCell(new Phrase(personel.IsimSoyisim, baslik));

            var pdfAlinanEgitimler = new PdfPTable(6);
            pdfAlinanEgitimler.WidthPercentage = 95;
            pdfAlinanEgitimler.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfAlinanEgitimler.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;

            int say = 0;

            var tabloBaslik = new List<string>();
            tabloBaslik.Add("#");
            tabloBaslik.Add("Tarih");
            tabloBaslik.Add("Eğitim Tipi");
            tabloBaslik.Add("Eğitim");
            tabloBaslik.Add("Firma");
            tabloBaslik.Add("Katılım Durumu");
            foreach (var s in tabloBaslik)
            {
                var analizCell = new PdfPCell(new Phrase(s, kalinharici));
                analizCell.FixedHeight = 21f;
                analizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                analizCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfAlinanEgitimler.AddCell(analizCell);
            }

            foreach (var item in PersonelSicilOzetItemlar)
            {
                say++;
                var liste = new List<string>();
                liste.Add(say.ToString());
                liste.Add(item.EgitimOturum.OturumTarihi);
                liste.Add(item.EgitimFirma.IkEgitimTipiAdi);
                liste.Add(item.EgitimFirma.IkEgitimAdi);
                liste.Add(item.EgitimFirma.IkFirmaAdi);
                liste.Add(item.EgitimOturumSrcnKullanici.KatilimDurumAdi);

                foreach (var s in liste)
                {
                    var analizCell = new PdfPCell(new Phrase(s, hariciFont));
                    analizCell.FixedHeight = 21f;
                    analizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    analizCell.VerticalAlignment = Element.ALIGN_CENTER;
                    pdfAlinanEgitimler.AddCell(analizCell);
                }

            }



            #region PDF Olusturma Alani 

            using (Document pdfDoc =
                new Document(PageSize.A4, 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();


                try
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();
                    pdfDoc.Add(logo);
                    pdfDoc.Add(personelResim);
                    pdfDoc.Add(pdfBaslik);
                    pdfDoc.Add(pdfAlinanEgitimler);
                    //pdfDoc.Add(pdfAnalizSonuc);
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }

                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required

                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), "1"));
            }

            #endregion
        }

        #region PDF oluşturma
        public PdfPTable pdfAnalizSonucNumuneDetayOlustur(SrcnLabAnalizs labAnaliz, Font hariciFont, Font beyazRenk, int FormTipi, int DilSecimi)
        {
            if (FormTipi == 1)
            {
                var pdfBolunmemis = new PdfPTable(2);
                var pdfCozguBolumu = new PdfPTable(6);
                var pdfAtkiBolumu = new PdfPTable(6);
                pdfBolunmemis.WidthPercentage = 99;
                pdfBolunmemis.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfBolunmemis.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfBolunmemis.DefaultCell.BorderWidth = 0;


                #region Atki Bölümü
                pdfAtkiBolumu.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfAtkiBolumu.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;

                var cellAtki = new PdfPCell(new Phrase(labAnaliz.AnalizAdi, beyazRenk));

                if (labAnaliz.AnaliziYapilanBilesenSayisi == 2)
                {
                    cellAtki = new PdfPCell(new Phrase("ATKI- " + labAnaliz.AnalizAdi, beyazRenk));
                }

                cellAtki.HorizontalAlignment = Element.ALIGN_CENTER;
                cellAtki.VerticalAlignment = Element.ALIGN_CENTER;
                cellAtki.Colspan = 6;
                cellAtki.BackgroundColor = BaseColor.BLACK;
                pdfAtkiBolumu.AddCell(cellAtki);


                pdfAtkiBolumu.AddCell(new Phrase("ANALİZ", hariciFont));


                #endregion
                #region Çözgü Bölümü
                pdfCozguBolumu.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCozguBolumu.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;

                var cellCozgu = new PdfPCell(new Phrase("ÇÖZGÜ- " + labAnaliz.AnalizAdi, beyazRenk));
                cellCozgu.HorizontalAlignment = Element.ALIGN_CENTER;
                cellCozgu.VerticalAlignment = Element.ALIGN_CENTER;
                cellCozgu.Colspan = 6;
                cellCozgu.BackgroundColor = BaseColor.BLACK;
                pdfCozguBolumu.AddCell(cellCozgu);
                pdfCozguBolumu.AddCell(new Phrase("ANALİZ", hariciFont));
                #endregion

                for (int k = 0; k <= 4; k++)
                {
                    string ipAdi = "ANA IPLIK";
                    if (k != 0)
                    {
                        ipAdi = "IPLIK-" + k.ToString();
                    }
                    pdfAtkiBolumu.AddCell(new Phrase(ipAdi, hariciFont));
                    pdfCozguBolumu.AddCell(new Phrase(ipAdi, hariciFont));
                }

                var TumIplikAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.Where(a => a.MalzemeTipi == 0 && a.Sira > 0)
                    .OrderBy(a => a.Sira).Select(a => a.LabAnalizCesitAdi).ToList();
                foreach (var i in TumIplikAnalizCesitleri)
                {

                    var analizCell = new PdfPCell(new Phrase(i, hariciFont));
                    analizCell.FixedHeight = 17f;
                    analizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    analizCell.VerticalAlignment = Element.ALIGN_CENTER;
                    pdfAtkiBolumu.AddCell(analizCell);
                    pdfCozguBolumu.AddCell(analizCell);
                    for (int j = 1; j <= 5; j++)
                    {
                        pdfAtkiBolumu.AddCell(new Phrase("", hariciFont));
                        pdfCozguBolumu.AddCell(new Phrase("", hariciFont));
                    }
                }

                float[] widths = new float[] { 25f, 10f, 10f, 10f, 10f, 10f };

                pdfAtkiBolumu.SetWidths(widths);
                pdfCozguBolumu.SetWidths(widths);
                pdfBolunmemis.AddCell(pdfAtkiBolumu);
                string FormNo = "FN-2/01";
                if (labAnaliz.AnaliziYapilanBilesenSayisi == 2)
                {// kumaş
                    pdfBolunmemis.AddCell(pdfCozguBolumu);
                    FormNo = "FN-3/01";
                }
                else
                {
                    // iplik
                    pdfBolunmemis.AddCell(new Phrase(" ", hariciFont));
                }

                var cellFormNo = new PdfPCell(new Phrase(FormNo, hariciFont));

                cellFormNo.HorizontalAlignment = Element.ALIGN_LEFT;
                cellFormNo.VerticalAlignment = Element.ALIGN_LEFT;
                cellFormNo.Colspan = 2;
                cellFormNo.BorderWidth = 0;
                pdfBolunmemis.AddCell(cellFormNo);
                return pdfBolunmemis;
            }
            else
            {
                // müşteri  
                var analizItems = _dbPoly.SrcnLabAnalizItems.AsNoTracking().Where(a => a.LabAnalizId == labAnaliz.LabAnalizId).OrderBy(a => a.IplikNo).ThenBy(a => a.BobinAdi).ToList();
                var sutunSayisi = analizItems.Count + 2;
                var pdfAnalizSonuc = new PdfPTable(sutunSayisi);
                pdfAnalizSonuc.WidthPercentage = 95;
                pdfAnalizSonuc.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfAnalizSonuc.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                var cell = new PdfPCell(new Phrase(labAnaliz.AnalizAdi, beyazRenk));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.Colspan = sutunSayisi;
                cell.BackgroundColor = BaseColor.BLACK;
                pdfAnalizSonuc.AddCell(cell);
                pdfAnalizSonuc.AddCell(new Phrase("#", hariciFont));
                if (DilSecimi == 1)
                {
                    pdfAnalizSonuc.AddCell(new Phrase("ANALİZ", hariciFont));
                }
                else
                {
                    pdfAnalizSonuc.AddCell(new Phrase("ANALYSIS", hariciFont));
                }


                var analizSonucs = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                foreach (var item in analizItems)
                {
                    pdfAnalizSonuc.AddCell(new Phrase(item.BobinAdi, hariciFont)); // ilk satır
                    analizSonucs.AddRange(_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.AsNoTracking().Where(a => a.LabAnalizItemId == item.LabAnalizItemId).ToList());
                }
                var analizCesitIdler = analizSonucs.Select(a => a.LabAnalizCesitId).Distinct().ToList();
                var analizCesitler = _dbPoly.SrcnLabAnalizCesits
                    .Where(a => analizCesitIdler.Any(b => b == a.LabAnalizCesitId)).OrderBy(a => a.LabAnalizCesitAdi).ToList();
                if (DilSecimi == 2)
                {
                    analizCesitler = analizCesitler.OrderBy(a => a.LabAnalizCesitAdiEng).ToList();
                }
                int SiraSay = 0;


                foreach (var analizCesit in analizCesitler)
                {
                    SiraSay++;
                    pdfAnalizSonuc.AddCell(new Phrase(SiraSay.ToString(), hariciFont));
                    if (DilSecimi == 1)
                    {
                        pdfAnalizSonuc.AddCell(new Phrase(analizCesit.LabAnalizCesitAdi.ToString(), hariciFont));
                    }
                    else
                    {
                        pdfAnalizSonuc.AddCell(new Phrase(analizCesit.LabAnalizCesitAdiEng, hariciFont));
                    }

                    foreach (var analizItem in analizItems)
                    {
                        string analizDegeri = "-";
                        if (analizSonucs.Any(a => a.LabAnalizCesitId == analizCesit.LabAnalizCesitId && a.LabAnalizItemId == analizItem.LabAnalizItemId))
                        {
                            analizDegeri = analizSonucs.First(a =>
                                a.LabAnalizCesitId == analizCesit.LabAnalizCesitId &&
                                a.LabAnalizItemId == analizItem.LabAnalizItemId).AnalizDegeriString;
                        }
                        var analizCell = new PdfPCell(new Phrase(analizDegeri, hariciFont));
                        analizCell.FixedHeight = 21f;
                        analizCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        analizCell.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfAnalizSonuc.AddCell(analizCell);
                    }

                }

                return pdfAnalizSonuc;
            }
        }

        public FileStreamResult PdfDetayliOlustur(int LabAnalizId, int FormTipi, int DilSecimi)
        {
            // Form tipi = 1 lab içib, 2 müşteri için
            // DilSecimi = 1 türkçe , 2 ingilzice

            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(LabAnalizId);
            #region Harici Fontlar
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion
            #region Resim Yukleme
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            jpg.ScaleToFit(250, 25);
            //Give space before image 
            //jpg.SpacingBefore = 50f;
            ////Give some space after the image 
            //jpg.SpacingAfter = 13f;

            jpg.Alignment = Image.UNDERLYING;
            #endregion
            PdfPTable pdfBaslik = new PdfPTable(1);
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;
            PdfPTable pdfGenelBilgiler = new PdfPTable(4);
            pdfGenelBilgiler.DefaultCell.BorderWidth = 0;
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(labAnaliz.LabGrupId);
            string Baslik = "";
            LabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            if (LabGruplari.Any())
            {
                foreach (var item in LabGruplari)
                {
                    if (item != LabGruplari.Last())
                    {
                        Baslik += item.LabGrupAdi + " >> ";

                    }
                    else
                    {
                        Baslik += item.LabGrupAdi;
                    }

                }

            }
            pdfBaslik.AddCell(new Phrase(" ", baslik));
            pdfBaslik.AddCell(new Phrase(Baslik, baslik));
            pdfBaslik.AddCell(new Phrase(" ", baslik));

            PdfPTable pdfAciklama = new PdfPTable(1);
            pdfAciklama.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfAciklama.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfAciklama.DefaultCell.BorderWidth = 0;
            if (labAnaliz.AnalizTipi == 3)
            {

                // numune yapılabilirlik
                if (FormTipi == 1)
                {// lab için
                    pdfGenelBilgiler.AddCell(new Phrase("Takip Kodu / Tarih", hariciFont));
                    pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.LabAnalizId.ToString() + " / " + labAnaliz.IstenenTerminTarihi.Value.ToShortDateString(), kalinharici));
                    pdfGenelBilgiler.AddCell(new Phrase("İstekte Bulunan", hariciFont));
                    pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.KayitYapanKulAdi, kalinharici));
                    pdfGenelBilgiler.AddCell(new Phrase("Kayıt Tarihi", hariciFont));
                    pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.KayitTarihi.ToString(), kalinharici));

                    pdfGenelBilgiler.AddCell(new Phrase("Cari:", hariciFont));
                    pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.CariAdi, kalinharici));
                    pdfAciklama.AddCell(new Phrase("Analiz Açıklama", baslik));
                    pdfAciklama.AddCell(new Phrase(labAnaliz.Aciklama, baslik));
                }
                else
                {

                    // müşteri için
                    var numuneYapilabilirlik =
                        _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(labAnaliz.BagliOlduguDosyaId);

                    if (DilSecimi == 1)
                    {
                        pdfGenelBilgiler.AddCell(new Phrase("Takip Kodu", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.LabAnalizId.ToString(), kalinharici));
                        pdfGenelBilgiler.AddCell(new Phrase("İstekte Bulunan", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.KayitYapanKulAdi, kalinharici));
                        pdfGenelBilgiler.AddCell(new Phrase("Cari", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase(numuneYapilabilirlik.CariAdi.ToString(), kalinharici));

                        pdfGenelBilgiler.AddCell(new Phrase("Durum", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase("Analiz Tamamlandı", kalinharici));
                    }
                    else
                    {
                        pdfGenelBilgiler.AddCell(new Phrase("Tracking Code", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.LabAnalizId.ToString(), kalinharici));
                        pdfGenelBilgiler.AddCell(new Phrase("Claimant", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.KayitYapanKulAdi, kalinharici));
                        pdfGenelBilgiler.AddCell(new Phrase("Customer", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase(numuneYapilabilirlik.CariAdi.ToString(), kalinharici));

                        pdfGenelBilgiler.AddCell(new Phrase("Status", hariciFont));
                        pdfGenelBilgiler.AddCell(new Phrase("Analysis Completed", kalinharici));
                    }
                }

            }
            var pdfAnalizSonuc = pdfAnalizSonucOlustur(LabAnalizId, hariciFont, kalinharici, FormTipi, DilSecimi);
            pdfAciklama.AddCell(new Phrase(labAnaliz.LabAciklama, baslik));
            #region PDF Olusturma Alani 
            using (Document pdfDoc =
                new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    pdfGenelBilgiler.SetWidths(new float[] { 50f, 60f, 50f, 150f });
                    //pdfGenelBilgiler.HorizontalAlignment = Element.ALIGN_LEFT;
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();
                    pdfDoc.Add(jpg);
                    pdfDoc.Add(pdfBaslik);
                    pdfDoc.Add(pdfGenelBilgiler);
                    pdfDoc.Add(pdfAnalizSonuc);
                    // pdfDoc.Add(pdfAciklama);
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }
                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required

                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), labAnaliz.LabAnalizId));
            }

            #endregion
        }
        public PdfPTable pdfAnalizSonucOlustur(int LabAnalizId, Font hariciFont, Font kalinharici, int FormTipi, int DilSecimi)
        {
            Font beyazRenk = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            beyazRenk.SetStyle("Bold");
            beyazRenk.Color = BaseColor.WHITE;
            beyazRenk.Size = 10;
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(LabAnalizId);
            if (labAnaliz.AnalizTipi == 0)
            {
                // Günlük İmalat
                var SecilenLabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
                var labAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == LabAnalizId)
                    .OrderBy(a => a.YardimciTesisKontrolNoktaAdi).ToList();
                var LabAnalizDetayModeller = new List<LabAnalizDetayModel>();
                var LabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
                foreach (var analizItem in labAnalizItems)
                {
                    var item = new LabAnalizDetayModel()
                    {
                        LabAnalizItem = analizItem,
                        LabAnalizItemAnalizCesitSonuclari = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Where(a => a.LabAnalizItemId == analizItem.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi).ToList()
                    };
                    foreach (var aa in item.LabAnalizItemAnalizCesitSonuclari)
                    {
                        if (SecilenLabAnalizCesitleri.Count(a => a.LabAnalizCesitId == aa.LabAnalizCesitId && a.LabAnalizCesitAdi == aa.LabAnalizCesitAdi) == 0)
                        {
                            SecilenLabAnalizCesitleri.Add(new SrcnLabAnalizCesits()
                            {
                                LabAnalizCesitAdi = aa.LabAnalizCesitAdi,
                                LabAnalizCesitId = aa.LabAnalizCesitId
                            });
                        }
                    }
                    LabAnalizDetayModeller.Add(item);

                }

                LabAnalizCesitleri = SecilenLabAnalizCesitleri.OrderBy(a => a.LabAnalizCesitAdi).ToList();


                var pdfAnalizSonuc = new PdfPTable(LabAnalizCesitleri.Count + 2);
                pdfAnalizSonuc.WidthPercentage = 95;
                pdfAnalizSonuc.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfAnalizSonuc.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfAnalizSonuc.AddCell(new Phrase("#", hariciFont));
                pdfAnalizSonuc.AddCell(new Phrase("Pozisyon", hariciFont));
                foreach (var item in LabAnalizCesitleri)
                {
                    pdfAnalizSonuc.AddCell(new Phrase(item.LabAnalizCesitAdi, hariciFont));
                }

                int satirSay = 0;
                foreach (var itt in LabAnalizDetayModeller)
                {
                    satirSay++;

                    pdfAnalizSonuc.AddCell(new Phrase(satirSay.ToString(), hariciFont));
                    if (itt.LabAnalizItem.MakinePozisyonNo == 0)
                    {
                        pdfAnalizSonuc.AddCell(new Phrase("", kalinharici));
                    }
                    else
                    {
                        pdfAnalizSonuc.AddCell(new Phrase(itt.LabAnalizItem.MakinePozisyonNo.ToString(), kalinharici));
                    }

                    foreach (var item in itt.LabAnalizItemAnalizCesitSonuclari)
                    {
                        if (item.AnalizDegeri == 0)
                        {
                            pdfAnalizSonuc.AddCell(new Phrase(" ", kalinharici));
                        }
                        else
                        {
                            pdfAnalizSonuc.AddCell(new Phrase(item.AnalizDegeri.ToString("F"), kalinharici));
                        }
                    }
                }

                return pdfAnalizSonuc;
            }
            if (labAnaliz.AnalizTipi == 4)
            {
                // yardımcı tesis
                var SecilenLabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
                var labAnalizItems = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == LabAnalizId)
                    .OrderBy(a => a.YardimciTesisKontrolNoktaAdi).ToList();
                var LabAnalizDetayModeller = new List<LabAnalizDetayModel>();
                var LabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
                foreach (var analizItem in labAnalizItems)
                {
                    var item = new LabAnalizDetayModel()
                    {
                        LabAnalizItem = analizItem,
                        LabAnalizItemAnalizCesitSonuclari = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Where(a => a.LabAnalizItemId == analizItem.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi).ToList()
                    };
                    foreach (var aa in item.LabAnalizItemAnalizCesitSonuclari)
                    {
                        if (SecilenLabAnalizCesitleri.Count(a => a.LabAnalizCesitId == aa.LabAnalizCesitId && a.LabAnalizCesitAdi == aa.LabAnalizCesitAdi) == 0)
                        {
                            SecilenLabAnalizCesitleri.Add(new SrcnLabAnalizCesits()
                            {
                                LabAnalizCesitAdi = aa.LabAnalizCesitAdi,
                                LabAnalizCesitId = aa.LabAnalizCesitId
                            });
                        }
                    }
                    LabAnalizDetayModeller.Add(item);

                }
                LabAnalizCesitleri = SecilenLabAnalizCesitleri.OrderBy(a => a.LabAnalizCesitAdi).ToList();
                var pdfAnalizSonuc = new PdfPTable(LabAnalizCesitleri.Count + 2);
                pdfAnalizSonuc.WidthPercentage = 95;
                pdfAnalizSonuc.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfAnalizSonuc.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfAnalizSonuc.AddCell(new Phrase("#", hariciFont));
                pdfAnalizSonuc.AddCell(new Phrase("Kontrol Noktası", hariciFont));
                foreach (var item in LabAnalizCesitleri)
                {
                    pdfAnalizSonuc.AddCell(new Phrase(item.LabAnalizCesitAdi, hariciFont));
                }

                int satirSay = 0;
                foreach (var itt in LabAnalizDetayModeller)
                {
                    satirSay++;

                    pdfAnalizSonuc.AddCell(new Phrase(satirSay.ToString(), hariciFont));
                    pdfAnalizSonuc.AddCell(new Phrase(itt.LabAnalizItem.YardimciTesisKontrolNoktaAdi, kalinharici));
                    foreach (var item in itt.LabAnalizItemAnalizCesitSonuclari)
                    {
                        if (item.AnalizDegeri == 0)
                        {
                            pdfAnalizSonuc.AddCell(new Phrase(" ", kalinharici));
                        }
                        else
                        {
                            pdfAnalizSonuc.AddCell(new Phrase(item.AnalizDegeri.ToString("F"), kalinharici));
                        }
                    }
                }

                return pdfAnalizSonuc;
            }
            if (labAnaliz.AnalizTipi == 3)
            {
                var pdfAnalizSonuc = pdfAnalizSonucNumuneDetayOlustur(labAnaliz, hariciFont, beyazRenk, FormTipi, DilSecimi);
                return pdfAnalizSonuc;
            }
            return new PdfPTable(2);
        }
        public FileResult PdfOlarakIndir(int id)
        {
            var labAnaliz = _dbPoly.SrcnLabAnalizs.Find(id);
            #region Harici Fontlar
            Font hariciFont = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            Font kalinharici = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            kalinharici.SetStyle("Bold");
            kalinharici.Color = BaseColor.BLACK;
            kalinharici.Size = 10;
            Font baslik = new Font(BaseFont.CreateFont(Server.MapPath("~/Temalar/fonts/ARIALBD.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED));
            baslik.SetStyle("Bold");

            hariciFont.Size = 8;
            baslik.Size = 10;

            #endregion
            #region Resim Yukleme
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Temalar/Resimler/polyteks.jpg"));

            //Resize image depend upon your need 
            jpg.ScaleToFit(250, 45);
            //Give space before image 
            //jpg.SpacingBefore = 50f;
            ////Give some space after the image 
            //jpg.SpacingAfter = 13f;


            jpg.Alignment = Image.UNDERLYING;
            #endregion

            PdfPTable pdfBaslik = new PdfPTable(1);

            //pdfBaslik.WidthPercentage = 80;
            pdfBaslik.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
            pdfBaslik.DefaultCell.BorderWidth = 0;

            PdfPTable pdfGenelBilgiler = new PdfPTable(4);
            pdfGenelBilgiler.DefaultCell.BorderWidth = 0;
            //  pdfGenelBilgiler.SpacingAfter = 20;

            //pdfGenelBilgiler.TotalWidth = 100;
            LabGruplari = new List<SrcnLabGrups>();
            RecursiveLabGrups(labAnaliz.LabGrupId);
            string Baslik = "";
            LabGruplari = LabGruplari.OrderBy(a => a.UstLabGrupId).ToList();
            if (LabGruplari.Any())
            {
                foreach (var item in LabGruplari)
                {
                    if (item != LabGruplari.Last())
                    {
                        Baslik += item.LabGrupAdi + " >> ";

                    }
                    else
                    {
                        Baslik += item.LabGrupAdi;
                    }

                }
                // Baslik += "  Analizi";
            }
            pdfBaslik.AddCell(new Phrase(" ", baslik));
            pdfBaslik.AddCell(new Phrase(Baslik, baslik));
            pdfBaslik.AddCell(new Phrase(" ", baslik));

            pdfGenelBilgiler.AddCell(new Phrase("Takip Kodu", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.LabAnalizId.ToString(), kalinharici));
            pdfGenelBilgiler.AddCell(new Phrase("İstekte Bulunan", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.KayitYapanKulAdi, kalinharici));

            pdfGenelBilgiler.AddCell(new Phrase("Kayıt Tarihi", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.KayitTarihi.ToString(), kalinharici));

            pdfGenelBilgiler.AddCell(new Phrase("İstenen Termin", hariciFont));
            pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.IstenenTerminTarihi.Value.ToShortDateString(), kalinharici));

            if (labAnaliz.AnalizTipi == 0)
            {
                pdfGenelBilgiler.AddCell(new Phrase("Takım Saati", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.TakimSaati, kalinharici));

                pdfGenelBilgiler.AddCell(new Phrase("Parti & İş Emri No", hariciFont));
                pdfGenelBilgiler.AddCell(new Phrase(labAnaliz.PartiNo + " - " + labAnaliz.RefakartKartNo, kalinharici));
            }
            var pdfAnalizSonuc = pdfAnalizSonucOlustur(id, hariciFont, kalinharici, 1, 1);

            #region PDF Olusturma Alani 

            using (Document pdfDoc =
                new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f))
            {
                MemoryStream stream = new MemoryStream();


                try
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Open();
                    pdfDoc.Add(jpg);
                    pdfDoc.Add(pdfBaslik);
                    pdfDoc.Add(pdfGenelBilgiler);
                    pdfDoc.Add(pdfAnalizSonuc);
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }

                pdfDoc.Close();

                stream.Flush(); //Always catches me out
                stream.Position = 0; //Not sure if this is required

                return File(stream, "application/pdf",
                    string.Format("{0}-{1}.pdf", Kullanici.IsimSoyisim.Replace(" ", ""), labAnaliz.LabAnalizId));
            }

            #endregion
        }
        #endregion
        #region Global degiskenler
        private LayoutModelTefrik BildirimKontrol(LayoutModelTefrik model)
        {

            return model;
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public List<SrcnLabGrups> LabGruplari { get; set; }
        public SrcnKullanicis Kullanici { get; set; }

        private static POTA_PTKSEntities _appContext;

        public static POTA_PTKSEntities _dbPolyCreate(bool yenilensinMi)
        {
            if (_appContext == null)
            {

                _appContext = new POTA_PTKSEntities() { Configuration = { LazyLoadingEnabled = false, ProxyCreationEnabled = false } };


            }
            return _appContext;
        }
        #endregion
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            //_dbPoly = new POTA_PTKSEntities(){Configuration = { LazyLoadingEnabled = false, ProxyCreationEnabled  = false}};


            var httpContext = requestContext.HttpContext;
            var LayoutModelTefrik = new LayoutModelTefrik();
            LabGruplari = new List<SrcnLabGrups>();
            bool SorunVarmi = true;
            if (httpContext.Session["kull"] != null)
            {
                Kullanici = (SrcnKullanicis)httpContext.Session["kull"];
                SorunVarmi = false;
            }
            else
            {
                var cookieBase = Helpers.Helper.CookieBaseBilgiGetir(1);
                var reqCookie = requestContext.HttpContext.Request.Cookies[cookieBase.CookieName];
                if (reqCookie != null)
                {
                    int KullaniciId = Convert.ToInt32(reqCookie.Value);
                    Kullanici = _dbPoly.SrcnKullanicis.Find(KullaniciId);
                    httpContext.Session["kull"] = Kullanici;
                    SorunVarmi = false;
                }
            }
            if (SorunVarmi == false)
            {
                LayoutModelTefrik.Kullanici = Kullanici;
                LayoutModelTefrik = BildirimKontrol(LayoutModelTefrik);
            }

            LayoutModelTefrik.PartiAltKlasorTipleri =
                _dbPoly.SrcnPartiAltKlasorTipis.OrderBy(a => a.PartiAltKlasorTipiId).ToList();

            string controllerName = requestContext.RouteData.Values["controller"].ToString();
            string actionName = requestContext.RouteData.Values["action"].ToString();

            //  httpContext.Session["CurrentURL"] = controllerName + " / " + actionName;
            ViewBag.LayoutModelTefrik = LayoutModelTefrik;
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}