using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class MalzemeTalepSepet
    {
        public MalzemeTalepSepet()
        {
            Malzemeler = new List<string>();
        }
        public List<string> Malzemeler { get; set; }
        public int EskiMiktar { get; set; }
        public int YeniMiktar { get; set; }
        public DateTime Tarih { get; set; }
        public string AmbarAdi { get; set; }
        public string PartiNo { get; set; }
        public string Kalite { get; set; }
    }
}