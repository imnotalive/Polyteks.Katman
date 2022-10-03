using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class MalzemeLabAnalizModel
    {
        public MalzemeLabAnalizModel()
        {
            LabAnalizCesitleri = new List<SrcnLabAnalizCesits>();
        }
        public string MalzemeAdi { get; set; }
        public int MalzemeTipi { get; set; }
        public List<SrcnLabAnalizCesits> LabAnalizCesitleri { get; set; }
    }
}