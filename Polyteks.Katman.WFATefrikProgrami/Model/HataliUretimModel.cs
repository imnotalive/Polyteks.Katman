using Polyteks.Katman.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.WFATefrikProgrami.Model
{
    public class HataliUretimModel
    {
        public HataliUretimModel()
        {
            Pozisyon = new DropItem();
            HataliUretimHareketler = new List<HataliUretimHareket>();
            HataliUretimHareket = new HataliUretimHareket();
        }
        public DropItem Pozisyon { get; set; }
        public List<HataliUretimHareket> HataliUretimHareketler { get; set; }
        public string HataNedeni { get; set; }
        public HataliUretimHareket HataliUretimHareket { get; set; }
    }
}
