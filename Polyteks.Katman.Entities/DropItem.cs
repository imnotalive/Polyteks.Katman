using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Polyteks.Katman.Entities
{
  public class DropItem
    {
        public string Id { get; set; }
        public int IdInt { get; set; }
        public string Tanim { get; set; }
        public string DigerTanim { get; set; }
        public bool SeciliMi  { get; set; }
        public int Sira { get; set; }
        public int OnId { get; set; }
        public int OnIdInt { get; set; }
    }
}
