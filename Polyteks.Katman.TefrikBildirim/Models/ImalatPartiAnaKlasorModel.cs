using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class ImalatPartiAnaKlasorModel
    {
        public ImalatPartiAnaKlasorModel()
        {
            Birim = new  SrcnFabrikaBirims();
            Birimler = new List<SrcnFabrikaBirims>();
            IslemMakineParametreler = new List<ZzzSrcnIslemMakineParametre>();
        }
        public int Id { get; set; }
        public int IkinciButonGrup { get; set; }

        public SrcnFabrikaBirims Birim { get; set; }
        public List<SrcnFabrikaBirims> Birimler { get; set; }

        public SelectList DropPartiler { get; set; }

        public List<ZzzSrcnIslemMakineParametre> IslemMakineParametreler { get; set; }
        public string SecilenPartiNo { get; set; }
      
    }
}