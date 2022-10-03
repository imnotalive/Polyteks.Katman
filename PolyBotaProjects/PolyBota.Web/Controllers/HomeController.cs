using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PolyBota.DAL;

namespace PolyBota.Web.Controllers
{
    public class HomeController : BaseController
    {
     
        public void yeniKullaniciKontrol()
        {
            var lst = _db.SpEksikPersonelListe().ToList();

            if (lst.Any())
            {
                var yeniListe = new List<User>();
                foreach (var item in lst)
                {
                    yeniListe.Add(new User()
                    {
                        Id = 0,
                     
                        Name = item.IsimSoyisim,
                        Password = item.Sifre,
                        PersonelCode = item.PersonelKodu,
                        UserName = item.KullaniciKodu
                    });
                }

                _db.Users.AddRange(yeniListe);
                _db.SaveChanges();
            }
        }
        // GET: Home
        public ActionResult Index()
        {
        //    foreach (var user in _db.Users.ToList())
        //    {
        //        if (string.IsNullOrEmpty(user.UserRoleIds))
        //        {
        //            user.UserRoleIds = "1";
        //            _db.SaveChanges();
        //        }
               
        //    }

            PtksStokKartAktar();
            Session["kul"] = null;
            Session["userRoles"] = null;
            Session["UserHeaderItemList"] = null;
            Session["kulLink"] = null;
            return View();

        }
    }
}