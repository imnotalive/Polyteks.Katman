using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Areas.Yonetim.Controllers
{
    public class YonetimHomeController : YonetimBaseController
    {
        // GET: Yonetim/YonetimHome
        public ActionResult Index()
        {
            return View();
        }
    }
}