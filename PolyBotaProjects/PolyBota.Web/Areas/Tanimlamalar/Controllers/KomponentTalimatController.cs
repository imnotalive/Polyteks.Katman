using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PolyBota.DAL;
using PolyBota.Entities;
using PolyBota.Web.Areas.Tanimlamalar.Models;
using PolyBota.Web.Controllers;

namespace PolyBota.Web.Areas.Tanimlamalar.Controllers
{
    public class KomponentTalimatController : BaseController
    {
        // GET: Tanimlamalar/KomponentTalimat

        public List<DropItem> PeriyotlariGetir()
        {
            var lst = new List<DropItem>();

            var gruplar = _db.PeriyotTipiTanims.AsNoTracking().OrderBy(a => a.PeriyotTipiAdi).ToList();
            var itemlar = _db.PeriyotTanims.AsNoTracking().OrderBy(a => a.PeriyotTanimAdi).ToList();

            foreach (var item in itemlar)
            {
                var str = "";
                if (gruplar.Any(a=>a.Id==item.PeriyotTipi))
                {
                    str = gruplar.First(a => a.Id == item.PeriyotTipi).PeriyotTipiAdi;

                    str = string.Format("{0} - {1} >> {2} Hafta", str, item.PeriyotTanimAdi, item.PeriyotDonemi);

                    lst.Add(new DropItem()
                    {
                        Tanim = str,
                        Id = item.Id.ToString()
                    });
                }
            }

            return lst.OrderBy(a => a.Tanim).ToList();
        }
        public List<DropItem> KomponentleriGetir()
        {
            var lst = new List<DropItem>();

            var gruplar = _db.KomponentTanimGrups.AsNoTracking().OrderBy(a => a.KomponentTanimGrupAdi).ToList();
            var itemlar = _db.KomponentTanims.AsNoTracking().OrderBy(a => a.KomponentTanimAdi).ToList();

            foreach (var item in itemlar)
            {
                var str = "";
                if (gruplar.Any(a => a.Id == item.KomponentTanimGrupId))
                {
                    str = gruplar.First(a => a.Id == item.KomponentTanimGrupId).KomponentTanimGrupAdi;

                    str = string.Format("{0} >> {1}", str,item.KomponentTanimAdi);

                    lst.Add(new DropItem()
                    {
                        Tanim = str,
                        Id = item.Id.ToString()
                    });
                }
            }

            return lst.OrderBy(a => a.Tanim).ToList();
        }


        public ActionResult KomponentTalimatListe()
        {
            var model = new TanimlamaKompTalimatModel()
            {
                KomponentTalimatGrups = _db.KomponentTalimatGrups.OrderBy(a=>a.KomponentTalimatGrupAdi).ToList(),
                DropPeriyotlar = PeriyotlariGetir(),
                DropKomponentler = KomponentleriGetir(),
            };
            return View(model);
        }

        public ActionResult KomponentTalimatGrupDetay(int id = 0)
        {
            var model = new TanimlamaKompTalimatModel()
            {
              DropPeriyotlar = PeriyotlariGetir(),
              DropKomponentler = KomponentleriGetir()
            };

            if (id!=0)
            {
                model.KomponentTalimatGrup = _db.KomponentTalimatGrups.Find(id);

                model.KomponentTalimats = _db.KomponentTalimats.Where(a => a.KomponentTalimatGrupId == id).ToList();

                var idler = model.KomponentTalimats.Select(a => a.TalimatTanimId).Distinct().ToList();

                foreach (var i in idler)
                {
                    model.TalimatTanims.Add(_db.TalimatTanims.Find(i));
                }

                model.TalimatTanims = model.TalimatTanims.OrderBy(a => a.TalimatAciklama).ToList();

                var lst = new List<KomponentTalimat>();


                foreach (var item in model.TalimatTanims)
                {
                    lst.AddRange(model.KomponentTalimats.Where(a=>a.TalimatTanimId==item.Id));
                }

                model.KomponentTalimats = lst;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult KomponentTalimatGrupDetay(TanimlamaKompTalimatModel model)
        {
            var grup = model.KomponentTalimatGrup;

            if (grup.KomponentTalimatGrupAdi==null)
            {
                TempDataCreate(4);
            }
            else
            {
                if (grup.Id == 0)
                {
                    var yeniItem = new KomponentTalimatGrup()
                    {
                        KomponentTalimatGrupAdi = grup.KomponentTalimatGrupAdi,
                        KomponentTanimId = grup.KomponentTanimId,
                        PeriyotTanimId = grup.PeriyotTanimId,
                    };
                    _db.KomponentTalimatGrups.Add(yeniItem);
                    _db.SaveChanges();

                    grup.Id = yeniItem.Id;
                }
                else
                {
                    var editItem = _db.KomponentTalimatGrups.Find(grup.Id);
                    editItem.KomponentTalimatGrupAdi = grup.KomponentTalimatGrupAdi;
                    editItem.KomponentTanimId = grup.KomponentTanimId;
                    editItem.PeriyotTanimId = grup.PeriyotTanimId;
                    _db.SaveChanges();

                }
                TempDataCreate(2);
            }

            return RedirectToAction("KomponentTalimatGrupDetay", "KomponentTalimat", new {id = grup.Id});
        }

        public PartialViewResult KomponentTalimatEkleDuzenle(int id = 0, int talimatId=0)
        {
            var model = new TanimlamaKompTalimatModel()
            {
                TalimatGrupTanims = _db.TalimatGrupTanims.OrderBy(a => a.TalimatGrupTanimAdi).ToList(),
                TalimatTanims = _db.TalimatTanims.OrderBy(a=>a.TalimatAciklama).ToList(),
                
            };

            if (talimatId != 0)
            {
                model.KomponentTalimat = _db.KomponentTalimats.Find(talimatId);
            }
            else
            {
                model.KomponentTalimat = new KomponentTalimat() {KomponentTalimatGrupId = id};
            }
            return PartialView("_KomponentTalimatEkleDuzenle", model);
        }

        [HttpPost]
        public ActionResult KomponentTalimatEkleDuzenle(TanimlamaKompTalimatModel model)
        {
            var talimat = model.KomponentTalimat;
            if (talimat.TalimatTanimId == 0)
            {
                TempDataCreate(4);
            }
            else
            {
                if (talimat.Id == 0)
                {
                    var yeniItem = new KomponentTalimat()
                    {
                        KomponentTalimatGrupId = talimat.KomponentTalimatGrupId,
                        TalimatTanimId = talimat.TalimatTanimId
                    };
                    _db.KomponentTalimats.Add(yeniItem);
                    _db.SaveChanges();
                }
                else
                {
                    var editItem = _db.KomponentTalimats.Find(talimat.Id);
                    editItem.TalimatTanimId = talimat.TalimatTanimId;
                    _db.SaveChanges();
                }
            }
        


            return RedirectToAction("KomponentTalimatGrupDetay", "KomponentTalimat",
                new {id = talimat.KomponentTalimatGrupId});


        }

        public ActionResult KomponentTalimatSil(int id)
        {

            var item = _db.KomponentTalimats.Find(id);

            var idd = item.KomponentTalimatGrupId;

            _db.KomponentTalimats.Remove(item);
            _db.SaveChanges();
            TempDataCreate(3);
            return RedirectToAction("KomponentTalimatGrupDetay", "KomponentTalimat", new {id = idd});
        }
    }
}