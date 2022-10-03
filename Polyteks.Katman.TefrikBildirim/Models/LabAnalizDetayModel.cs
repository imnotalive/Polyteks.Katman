using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class LabAnalizDetayModel
    {
        public LabAnalizDetayModel()
        {
            LabAnalizItem = new SrcnLabAnalizItems();
            LabAnalizItemAnalizCesitSonuclari = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
        }
        public SrcnLabAnalizItems LabAnalizItem { get; set; }
        public List<SrcnLabAnalizItemAnalizCesitSonucs> LabAnalizItemAnalizCesitSonuclari { get; set; }
    }
}