using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class YardimciTesisModel : LaboratuvarAnalizBase
    {
        public YardimciTesisModel()
        {
            YardimciTesisKategoriModeller = new List<YardimciTesisKategoriModel>();

        }

     
        public List<YardimciTesisKategoriModel> YardimciTesisKategoriModeller { get; set; }


    }
}