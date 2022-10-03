using Polyteks.Katman.Has.Controllers;
using Polyteks.Katman.TefrikBildirim.Areas.Deneme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polyteks.Katman.TefrikBildirim.Areas.Deneme.Controllers
{
    public class MerveController : BaseController
    {
        // GET: Deneme/Merve
        public ActionResult Index()
        {
            
            
            
            var model = new MerveModel();
            //model.Matris = new List<MerveSatirItem>();
            //model.Id = 2;
            //model.Matris = fromdb; ;

            for (int i = 0; i < 8; i++)
            {

                var araItem = new MerveSatirItem();

                for (int j = 0; j < 5; j++)
                {
                    string konumu = string.Format("{0} , {1}", i, j);
                    araItem.Sutunlar.Add(konumu);
                }
                model.Matris.Add(araItem);
            }
            return View(model);

        }

        public ActionResult GetAllUser()
        {
            List<Customer> c = GetAllUserr();



            return View( c);
        }

        public List<Customer> GetAllUserr()
        {

            return new List<Customer>();

        }
    }


   
}