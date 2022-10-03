using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.TefrikBildirim.Models;

namespace Polyteks.Katman.Has.Controllers
{
    public class UrgeController : BaseController
    {
        public UrgeModel UrgeModeliBaseOlustur(int id, int tip)
        {
            // id = numune dosyaId
            //tip => 1= Ürge için deniz hanım, 2= mesut bülent aydın için
            var model = new UrgeModel();
            var birimler = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5).OrderBy(a => a.BirimAdi).ToList();
            model.Birimler = birimler;
            if (tip==1)
            {
              
                model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 8 || a.DosyaDurumId == 9 || a.DosyaDurumId == 10).ToList();
             
            }
            if (tip == 2)
            {
                model.DosyaDurumlari = _dbPoly.SrcnDosyaDurums.Where(a => a.DosyaDurumId == 17).ToList();

            }
            
            if (id>0)
            {
                model.NumuneYapilabilirlikDosya = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(id);
                model.DosyaNumuneLabAnalizTablolar = DosyaNumuneLabAnalizTablolariOlustur( id);
                var numDenemeDosyalari = _dbPoly.SrcnDenemeDosya
                    .Where(a => a.BagliOlduguDosyaTipi == 2 && a.BagliOlduguDosyaId == id).ToList();
                foreach (var birim in birimler)
                {
                    if (numDenemeDosyalari.Any(a=>a.BirimId==birim.BirimId))
                    {
                        model.DenemeDosyalari.Add(numDenemeDosyalari.First(a => a.BirimId == birim.BirimId));
                    }
                    else
                    {
                        model.DenemeDosyalari.Add(new SrcnDenemeDosya()
                        {
                            BirimAdi = birim.BirimAdi,
                            BirimId = birim.BirimId
                        });
                    }
                    
                }
            }
            return model;
        }
        public ActionResult NumuneYapilabilirlikYorumOnaylari(int? id)
        {
            var model = UrgeModeliBaseOlustur(0,1);
            if (id==null)
            {
                var numDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == 9 || a.DosyaDurumId == 8)
                    .OrderByDescending(a => a.KayitTarihi).ToList();
                model.NumuneYapilabilirlikDosyalar = numDosyalar;
                model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(9);
            }
            else
            {
                int idd = Convert.ToInt32(id);
                if (idd==9 || idd==0)
                {
                    idd = 8;
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(9);
                }
                else
                {
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(idd);
                }
            
                model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == idd)
                    .OrderByDescending(a => a.KayitTarihi).ToList();
            }
       

           
            return View(model);
        }
        public ActionResult NumuneYapilabilirlikYorum(int id)
        {
            var model = UrgeModeliBaseOlustur(id,1);
            return View(model);
        }
        [HttpPost]
        public ActionResult NumuneYapilabilirlikYorum(UrgeModel model)
        {
            var numuneDosya = model.NumuneYapilabilirlikDosya;
            int YapilabilirLikDurum = 0;
            string YapilabilirLikDurumAdi = "";
            if (numuneDosya.YapilabilirlikDurumu!=null)
            {
                YapilabilirLikDurum = Convert.ToInt32(numuneDosya.YapilabilirlikDurumu);
            }
            bool SorunVarmi = false;
            if (YapilabilirLikDurum==0)
            {
                SorunVarmi = true;
                TempDataOlustur("Lütfen Yapılabilirlik Durumunu Belirleyiniz", false);
            }
         

            if (SorunVarmi)
            {
                return RedirectToAction("NumuneYapilabilirlikYorum", "Urge", new {id = numuneDosya.DosyaDurumId});
            }

            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(10);
            var editNumune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(numuneDosya.NumuneYapilabilirlikDosyaId);
            editNumune.YapilabilirlikDurumu = YapilabilirLikDurum;
            editNumune.YapilabilirlikDurumuAdi = YapilabilirLikDurumAdi;
            editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
            editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
            editNumune.UrgeYorumu = numuneDosya.UrgeYorumu;
            _dbPoly.SaveChanges();
            _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
            {
                KayitTarihi = DateTime.Now,
                DosyaHareketTanimId = editNumune.DosyaDurumId,
                SikayetNumuneDosyaId = editNumune.NumuneYapilabilirlikDosyaId,
                DosyaAdi = "Numune Yapılabilirlik",
                DosyaTipi = 2,
                HareketAdi = editNumune.DosyaDurumAdi+" "+ YapilabilirLikDurumAdi + " - " + Kullanici.IsimSoyisim
            });

            // yönetici yorumunda
            if (false)
            {
                var seciliBirimler = model.DenemeDosyalari.Where(a => a.SeciliMi).ToList();
                // yapilabilir
                if (YapilabilirLikDurum == 1)
                {
                    YapilabilirLikDurumAdi = "YAPILABİLİR";
                    // yapilabilir
           
                    if (!seciliBirimler.Any())
                    {
                        SorunVarmi = true;
                        TempDataOlustur("Lütfen İlgili Birimleri Seçiniz", false);
                    }
                }
                else
                {
                    YapilabilirLikDurumAdi = "YAPILAMAZ";
                }
             

                foreach (var item in seciliBirimler)
                {
                    var birim = _dbPoly.SrcnFabrikaBirims.Find(item.BirimId);
                    _dbPoly.SrcnDenemeDosya.Add(new SrcnDenemeDosya()
                    {
                        OlusturmaTarihi = DateTime.Now,
                        BirimAdi = birim.BirimAdi,
                        BagliOlduguDosyaId = editNumune.NumuneYapilabilirlikDosyaId,
                        BirimId = birim.BirimId,
                        SeciliMi = true,
                        BagliOlduguDosyaTipi = 2,
                        BagliOlduguDosyaTipiAdi = "Numune Yapılabilirlik",
                        DenemeAdi = null,
                        PartiAdi = "",
                        PartiAnaKlasorId = 0,
                        KayitYapanKulAdi = Kullanici.IsimSoyisim,
                        KayitYapanKulId = Kullanici.KullaniciId,
                        CariAdi = editNumune.CariAdi,
                        CariNo = editNumune.CariKodu,
                        FirmaTipi = editNumune.FirmaTipi
                    });
                    _dbPoly.SaveChanges();

                }
            }

          TempDataCRUDOlustur(2);

            return RedirectToAction("NumuneYapilabilirlikYorumOnaylari");
          
        }

        public ActionResult NumuneYapilmaTalepleri(int? id)
        {
            var model = UrgeModeliBaseOlustur(0,2);
            if (id == null)
            {
                var numDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == 17)
                    .OrderBy(a => a.KayitTarihi).ToList();
                model.NumuneYapilabilirlikDosyalar = numDosyalar;
                model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(17);
            }
            else
            {
                int idd = Convert.ToInt32(id);
                if (idd == 0)
                {
                    idd = 17;
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(17);
                }
                else
                {
                    model.DosyaDurumu = _dbPoly.SrcnDosyaDurums.Find(idd);
                }

                model.NumuneYapilabilirlikDosyalar = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Where(a => a.DosyaDurumId == idd)
                    .OrderBy(a => a.KayitTarihi).ToList();
            }



            return View(model);
        }

        public ActionResult NumuneYapilmaTalebi(int id)
        {
            var model = UrgeModeliBaseOlustur(id, 2);
            ViewBag.Birimler = model.Birimler;
            return View(model);
        }

        [HttpPost]
        public ActionResult NumuneYapilmaTalebi(UrgeModel model)
        {
            var numuneDosya = model.NumuneYapilabilirlikDosya;
           
            var editNumune = _dbPoly.SrcnNumuneYapilabilirlikDosyas.Find(numuneDosya.NumuneYapilabilirlikDosyaId);
            if (numuneDosya.DosyaDurumId!= editNumune.DosyaDurumId)
            {
                var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(numuneDosya.DosyaDurumId);
                editNumune.DosyaDurumId = dosyaDurum.DosyaDurumId;
                editNumune.DosyaDurumAdi = dosyaDurum.DosyaDurumAdi;
                editNumune.FabrikaBirimId = model.NumuneYapilabilirlikDosya.FabrikaBirimId;
                editNumune.NumuneTalepDetayi = model.NumuneYapilabilirlikDosya.NumuneTalepDetayi;
                var fabrikaBirimAdi = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId == model.NumuneYapilabilirlikDosya.FabrikaBirimId).Select(a => a.BirimAdi).FirstOrDefault();
                editNumune.FabrikaBirimAdi = fabrikaBirimAdi;
                _dbPoly.SaveChanges();
                _dbPoly.SrcnDosyaHarekets.Add(new SrcnDosyaHarekets()
                {
                    KayitTarihi = DateTime.Now,
                    DosyaHareketTanimId = editNumune.DosyaDurumId,
                    SikayetNumuneDosyaId = editNumune.NumuneYapilabilirlikDosyaId,
                    DosyaAdi = "Numune Yapılabilirlik",
                    DosyaTipi = 2,
                    HareketAdi = editNumune.DosyaDurumAdi +  " - " + Kullanici.IsimSoyisim
                });

            }


            // yönetici yorumunda
            if (false)
            {
                int YapilabilirLikDurum = 0;
                string YapilabilirLikDurumAdi = "";
                bool SorunVarmi = false;
                var seciliBirimler = model.DenemeDosyalari.Where(a => a.SeciliMi).ToList();
                // yapilabilir
                if (YapilabilirLikDurum == 1)
                {
                    YapilabilirLikDurumAdi = "YAPILABİLİR";
                    // yapilabilir

                    if (!seciliBirimler.Any())
                    {
                        SorunVarmi = true;
                        TempDataOlustur("Lütfen İlgili Birimleri Seçiniz", false);
                    }
                }
                else
                {
                    YapilabilirLikDurumAdi = "YAPILAMAZ";
                }


                foreach (var item in seciliBirimler)
                {
                    var birim = _dbPoly.SrcnFabrikaBirims.Find(item.BirimId);
                    _dbPoly.SrcnDenemeDosya.Add(new SrcnDenemeDosya()
                    {
                        OlusturmaTarihi = DateTime.Now,
                        BirimAdi = birim.BirimAdi,
                        BagliOlduguDosyaId = editNumune.NumuneYapilabilirlikDosyaId,
                        BirimId = birim.BirimId,
                        SeciliMi = true,
                        BagliOlduguDosyaTipi = 2,
                        BagliOlduguDosyaTipiAdi = "Numune Yapılabilirlik",
                        DenemeAdi = null,
                        PartiAdi = "",
                        PartiAnaKlasorId = 0,
                        KayitYapanKulAdi = Kullanici.IsimSoyisim,
                        KayitYapanKulId = Kullanici.KullaniciId,
                        CariAdi = editNumune.CariAdi,
                        CariNo = editNumune.CariKodu,
                        FirmaTipi = editNumune.FirmaTipi
                    });
                    _dbPoly.SaveChanges();

                }
            }

            TempDataCRUDOlustur(2);

            return RedirectToAction("NumuneYapilmaTalepleri");

        }

    }
}