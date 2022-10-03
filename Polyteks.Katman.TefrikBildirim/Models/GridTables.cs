using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class GridTables
    {
        public Nullable<System.DateTime> Tarih { get; set; }
        public string PartiNo { get; set; }
        public string Denye_Flaman { get; set; }
        public string Uretim_Mak_Kan_Poz { get; set; }
        public string ProblemTanimi { get; set; }
        public string YapilanMudahale { get; set; }
        public string Teksture_Makine_No { get; set; }
        public Nullable<int> Teksture_Pozisyon_No { get; set; }
        public Nullable<System.DateTime> LabIncelemeTarihi { get; set; }
        public string LabSonuc { get; set; }
        public string TekstureSonuc { get; set; }
    }
}