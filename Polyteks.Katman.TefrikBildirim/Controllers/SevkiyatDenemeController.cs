using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Controllers
{
    public class SevkiyatDenemeController : Controller
    {
        private POTA_PTKSEntities db = new POTA_PTKSEntities();

        // GET: SevkiyatDeneme
        public ActionResult Index()
        {
            return View(db.ZzzMrvStokDetayTakip.ToList());
        }

        // GET: SevkiyatDeneme/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZzzMrvStokDetayTakip zzzMrvStokDetayTakip = db.ZzzMrvStokDetayTakip.Find(id);
            if (zzzMrvStokDetayTakip == null)
            {
                return HttpNotFound();
            }
            return View(zzzMrvStokDetayTakip);
        }

        // GET: SevkiyatDeneme/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SevkiyatDeneme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AmbarNo,AmbarAdi,StokKodu,LotNo,Miktar1,StokAdi,OlcuBirimi1,StokTuru")] ZzzMrvStokDetayTakip zzzMrvStokDetayTakip)
        {
            if (ModelState.IsValid)
            {
                db.ZzzMrvStokDetayTakip.Add(zzzMrvStokDetayTakip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(zzzMrvStokDetayTakip);
        }

        // GET: SevkiyatDeneme/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZzzMrvStokDetayTakip zzzMrvStokDetayTakip = db.ZzzMrvStokDetayTakip.Find(id);
            if (zzzMrvStokDetayTakip == null)
            {
                return HttpNotFound();
            }
            return View(zzzMrvStokDetayTakip);
        }

        // POST: SevkiyatDeneme/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AmbarNo,AmbarAdi,StokKodu,LotNo,Miktar1,StokAdi,OlcuBirimi1,StokTuru")] ZzzMrvStokDetayTakip zzzMrvStokDetayTakip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zzzMrvStokDetayTakip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(zzzMrvStokDetayTakip);
        }

        // GET: SevkiyatDeneme/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZzzMrvStokDetayTakip zzzMrvStokDetayTakip = db.ZzzMrvStokDetayTakip.Find(id);
            if (zzzMrvStokDetayTakip == null)
            {
                return HttpNotFound();
            }
            return View(zzzMrvStokDetayTakip);
        }

        // POST: SevkiyatDeneme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ZzzMrvStokDetayTakip zzzMrvStokDetayTakip = db.ZzzMrvStokDetayTakip.Find(id);
            db.ZzzMrvStokDetayTakip.Remove(zzzMrvStokDetayTakip);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
