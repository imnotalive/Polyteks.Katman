using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Toplanti.Models;

namespace Polyteks.Katman.Toplanti.Controllers
{
    public class HomeController : BaseController
    {
        /*
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {           
           var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
         */
        public ToplantiRandevuModel ToplantiRandevuModeliGetir(int? id)
        {
            if (id == null)
            {
                id = 0;
            }
            var model = new ToplantiRandevuModel();
            model.GunSay = (int)id;
            var secilenGun = DateTime.Now.Date.AddDays((int)id);
            model.SecilenGun = secilenGun.ToString("dd MMMM yyyy");
            //model.BaseSaatler = _dbPoly.SrcnToplantiBaseSaats.AsNoTracking().OrderBy(a => a.SaatId).ThenBy(a => a.BaslangicSaat).ToList();//
            //model.BaseSaatler = _dbPoly.SrcnToplantiBaseSaats.AsNoTracking().OrderBy(a => a.SaatId).ToList();
            //model.ToplantiSalonlari = _dbPoly.SrcnToplantiSalons.AsNoTracking().OrderBy(a => a.ToplantiSalonAdi).ToList();
            model.BaseSaatler = _dbPoly.SrcnToplantiBaseSaats.AsNoTracking().OrderBy(a => a.SaatId).ToList();
            

            model.ToplantiSalonlari = _dbPoly.SrcnToplantiSalons.AsNoTracking().OrderBy(a => a.ToplantiSalonAdi).ToList();

            var SecilenGunToplantiRandevular = _dbPoly.SrcnToplantiRadevus.AsNoTracking().Where(a => a.ToplantiRandevuDurumId == 1)
                .Where(a => a.ToplantiTarihi == secilenGun).ToList();
            model.ToplantiRandevular = SecilenGunToplantiRandevular;
            var randevular = new List<SrcnToplantiRandevuSaats>();
           
            foreach (var i in SecilenGunToplantiRandevular)
            {
                randevular.AddRange(_dbPoly.SrcnToplantiRandevuSaats.AsNoTracking().Where(a => a.ToplantiRandevuId == i.ToplantiRandevuId).ToList());
            }
            foreach (var baseSaat in model.BaseSaatler)
            {
                var item =
                    new ToplantiRandevuSaatler()
                    {
                        Saat = baseSaat
                    };

                foreach (var salon in model.ToplantiSalonlari)
                {
                    if (randevular.Any(a => a.SaatId == baseSaat.SaatId && a.ToplantiSalonId == salon.ToplantiSalonId && a.PasifeCekiliMi==null))
                    {
                        var randevuu = randevular.First(a =>
                            a.SaatId == baseSaat.SaatId && a.ToplantiSalonId == salon.ToplantiSalonId);
                        item.ToplantiRandevuSalonModelleri.Add(new ToplantiRandevuSalonModel()
                        {
                            DoluMu = true,
                            ToplantiSalonu = salon,
                            ToplantiRandevuSaat = randevuu,
                            ToplantiRandevu = SecilenGunToplantiRandevular.First(a => a.ToplantiRandevuId == randevuu.ToplantiRandevuId)
                        });
                    }
                    else
                    {
                        item.ToplantiRandevuSalonModelleri.Add(new ToplantiRandevuSalonModel()
                        {
                            DoluMu = false,
                            ToplantiSalonu = salon
                        });
                    }
                }




                model.ToplantiRandevuSaatlerModeller.Add(item);

            }
            return model;
        }
        // GET: Home
        public ActionResult Index(int? id)
        {
            /*
            var personel = _dbPoly.Personel.First(a => a.PersonelNo.ToUpper() == "P124");
            var kulla = _dbPoly.SrcnKullanicis.First(a => a.PersonelKodu.ToUpper() == "P124");
            kulla.Resim = personel.Resim;
            _dbPoly.SaveChanges();
            */

            //foreach (var item in _dbPoly.SrcnToplantiBaseSaats.ToList())
            //{
            //    var ayir = item.BaslangicSaat.Split('-');
            //    if (ayir.Length==2)
            //    {
            //        item.BaslangicSaat = ayir[0];
            //        item.BitisSaat = ayir[1];
            //        _dbPoly.SaveChanges();
            //    }
            //}


            if (Session["trhFark"] != null)
            {
                id = Convert.ToInt32(Session["trhFark"]);
                Session["trhFark"] = null;
            }
            var model = ToplantiRandevuModeliGetir(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult SeciliTariheGit(ToplantiRandevuModel model)
        {
            var tarih = Convert.ToDateTime(model.SecilenGun);
            var fark = tarih - DateTime.Now.Date;

            Session["trhFark"] = fark.TotalDays;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ToplantiOlustur(ToplantiRandevuModel model)
        {

            var secilenToplantiSaatOdalar = model.ToplantiRandevuSaatlerModeller.Where(a => a.SecilenToplantiSalonuId != 0).ToList();
            var toplantiTarihi = DateTime.Now.AddDays(model.GunSay).Date;
            var toplantiRandevu = new SrcnToplantiRadevus
            {
                KayitTarihi = DateTime.Now,
                TalepEdenKulAdi = Kullanici.IsimSoyisim,
                TalepEdenKulKodu = Kullanici.KullaniciId.ToString(),
                ToplantiAciklamaca = "Açıklama",
                ToplantiBasligi = "başlık",
                ToplantiTarihi = toplantiTarihi,
                ToplantiRandevuDurumId = 1,
                ToplantiRandevuDurumAdi = "Talep Oluşturuldu"
            };
            _dbPoly.SrcnToplantiRadevus.Add(toplantiRandevu);
            _dbPoly.SaveChanges();
            foreach (var item in secilenToplantiSaatOdalar)
            {
                var saat = _dbPoly.SrcnToplantiBaseSaats.Find(item.Saat.SaatId);
                var toplantiSalon = _dbPoly.SrcnToplantiSalons.Find(item.SecilenToplantiSalonuId);
                var randevuItem = new SrcnToplantiRandevuSaats()
                {
                    SaatId = saat.SaatId,
                    BaslangicSaat = saat.BaslangicSaat,
                    ToplantiSalonId = toplantiSalon.ToplantiSalonId,
                    ToplantiSalonAdi = toplantiSalon.ToplantiSalonAdi,
                    BitisSaat = saat.BitisSaat,
                    ToplantiRandevuId = toplantiRandevu.ToplantiRandevuId,
                    TalepEdenKullaniciAdi = toplantiRandevu.TalepEdenKulAdi
                };
                _dbPoly.SrcnToplantiRandevuSaats.Add(randevuItem);
                _dbPoly.SaveChanges();
            }
            TempDataOlustur("Kayıt İşlemi Yapılmıştır", true);
            return RedirectToAction("Index");
        }
        public void KullanicIdYeCevir()
        {
            foreach (var randevu in _dbPoly.SrcnToplantiRadevus.Where(a => a.KayitTarihi < DateTime.Now).ToList())
            {
                int kulId = 0;
                try
                {
                    kulId = Convert.ToInt32(randevu.TalepEdenKulKodu);
                }
                catch (Exception e)
                {
                    var kull = _dbPoly.SrcnKullanicis.AsNoTracking().First(a => a.KullaniciKodu == randevu.TalepEdenKulKodu);

                    kulId = kull.KullaniciId;
                }
                randevu.TalepEdenKulKodu = kulId.ToString();
                _dbPoly.SaveChanges();
            }
        }
        public ActionResult Toplantilar(int? id)
        {
            int idd = 0;
            if (id == null || id == 0)
            {
                idd = 1;
            }
            else
            {
                idd = Convert.ToInt32(id);
            }

            var model = new ToplantiDetayModel();
            var istenenDurum = _dbPoly.SrcnToplantiRandevuDurums.Find(idd);
            model.RandevuDurum = istenenDurum;
            bool YetkiliMi = _dbPoly.SrcnKullaniciGrupKullanicis.Any(a => a.KullaniciId == Kullanici.KullaniciId && a.KullaniciGrupId == 1);
            var liste = new List<SrcnToplantiRadevus>();

            if (YetkiliMi)
            {
                liste = _dbPoly.SrcnToplantiRadevus.Where(a => a.ToplantiRandevuDurumId == istenenDurum.ToplantiRandevuDurumId).OrderByDescending(a => a.ToplantiTarihi).ThenBy(a => a.KayitTarihi)
                    .ToList();
            }
            else
            {
                liste = _dbPoly.SrcnToplantiRadevus.Where(a => a.TalepEdenKulKodu == Kullanici.KullaniciId.ToString() && a.ToplantiRandevuDurumId == istenenDurum.ToplantiRandevuDurumId).OrderByDescending(a => a.ToplantiTarihi).ThenBy(a => a.KayitTarihi)
                    .ToList();
            }
            var Durumlar = _dbPoly.SrcnToplantiRandevuDurums.AsNoTracking().OrderBy(a => a.ToplantiRandevuDurumId).ToList();

            foreach (var itt in Durumlar)
            {

                model.ToplantiDetayModelItemlar.Add(new ToplantiDetayModelItem()
                {
                    RandevuDurum = itt
                });

            }

            model.Randevular = liste;
            return View(model);
        }
        public ActionResult ToplantiDuzenle(int id)
        {
            var model = new ToplantiDetayModel();
            var toplanti = _dbPoly.SrcnToplantiRadevus.Find(id);
            bool duzenleyebilirMi = false;
            var Idler = new List<int>();
            Idler.Add(1);
            Idler.Add(2);
            if (toplanti.TalepEdenKulKodu == Kullanici.KullaniciId.ToString())
            {
                duzenleyebilirMi = true;
            }
            else
            {
                bool yetkiliMi = _dbPoly.SrcnKullaniciGrupKullanicis.Any(a => a.KullaniciId == Kullanici.KullaniciId && a.KullaniciGrupId == 1);
                duzenleyebilirMi = yetkiliMi;
                model.YetkiliMi = yetkiliMi;
                Idler.Add(3);
            }

          
            if (duzenleyebilirMi)
            {
                model.Randevu = toplanti;
                var istenenDurum = _dbPoly.SrcnToplantiRandevuDurums.Find(toplanti.ToplantiRandevuDurumId);
                model.RandevuDurum = istenenDurum;
                foreach (var i in Idler)
                {
                    model.RandevuDurumlar.Add(_dbPoly.SrcnToplantiRandevuDurums.Find(i));
                }

                model.RandevuSaatleri = _dbPoly.SrcnToplantiRandevuSaats
                    .Where(a => a.ToplantiRandevuId == toplanti.ToplantiRandevuId).OrderBy(a => a.SaatId).ToList();
                return View(model);
            }
            else
            {
                TempDataOlustur("Yetkiniz olmayan bir randevuyu düzenlemeye çalıştınız", false);
                return RedirectToAction("Toplantilar", "Home", new { id = toplanti.ToplantiRandevuDurumId });
            }



        }
        public ActionResult ToplantiSalonRandevuSaatPasifEt(int id)
        {
            var item = _dbPoly.SrcnToplantiRandevuSaats.Find(id);
            item.PasifeCekiliMi = "1";
            _dbPoly.SaveChanges();
            TempDataOlustur("Güncelleme işlemi yapılmıştır", true);
            return RedirectToAction("ToplantiDuzenle", "Home", new {id = item.ToplantiRandevuId});
        }
        public ActionResult ToplantiSalonRandevuSaatAktifEt(int id)
        {
            var item = _dbPoly.SrcnToplantiRandevuSaats.Find(id);
            item.PasifeCekiliMi = null;
            _dbPoly.SaveChanges();
            TempDataOlustur("Güncelleme işlemi yapılmıştır", true);
            return RedirectToAction("ToplantiDuzenle", "Home", new { id = item.ToplantiRandevuId });
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ToplantiDuzenle(ToplantiDetayModel model)
        {
            var toplanti = model.Randevu;
            var edittoplanti = _dbPoly.SrcnToplantiRadevus.Find(toplanti.ToplantiRandevuId);

            var durum = _dbPoly.SrcnToplantiRandevuDurums.Find(toplanti.ToplantiRandevuDurumId);

            edittoplanti.ToplantiRandevuDurumId = durum.ToplantiRandevuDurumId;
            edittoplanti.ToplantiRandevuDurumAdi = durum.ToplantiRandevuDurumAdi;
            edittoplanti.ToplantiBasligi = toplanti.ToplantiBasligi;
            edittoplanti.ToplantiAciklamaca = toplanti.ToplantiAciklamaca + "</br>" + Kullanici.IsimSoyisim;
           
                _dbPoly.SaveChanges();
           
         
            TempDataOlustur("Güncelleme işlemi yapılmıştır", true);
            return RedirectToAction("Toplantilar", "Home", new { id = toplanti.ToplantiRandevuDurumId });
        }
    }
}