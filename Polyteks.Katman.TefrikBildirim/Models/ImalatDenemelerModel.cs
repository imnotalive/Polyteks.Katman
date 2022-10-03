using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class ImalatDenemelerModel
    {
        public ImalatDenemelerModel()
        {
            Birim = new  SrcnFabrikaBirims();
            Birimler = new List<SrcnFabrikaBirims>();
            IslemMakineParametreler = new List<ZzzSrcnIslemMakineParametre>();
        }
        public int Id { get; set; }
        public int IkinciButonGrup { get; set; }

        public SrcnFabrikaBirims Birim { get; set; }
        public List<SrcnFabrikaBirims> Birimler { get; set; }

        public SelectList DropBirimMakineleri { get; set; }

        public List<ZzzSrcnIslemMakineParametre> IslemMakineParametreler { get; set; }
        public int SecilenMakineId { get; set; }
    }
}