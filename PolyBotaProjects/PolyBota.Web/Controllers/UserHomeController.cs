using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolyBota.Web.Controllers
{
    public class UserHomeController : BaseController
    {
        // GET: UserHome
        public ActionResult Info()
        {
            return View();
        }

        public ActionResult Bildirim()
        {
            return View();
        }
    }
}