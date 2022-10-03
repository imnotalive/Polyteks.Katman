using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyteks.Katman.TefrikBildirim.Areas.Deneme.Models
{
    public class MerveModel
    {
        public int Id { get; set; }
        public MerveModel()
        {
            Matris = new List<MerveSatirItem>();
        }
        public List<MerveSatirItem> Matris { get; set; }
    }

    public class Customer
    {

        public int Id{ get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

    }

    
    public class MerveSatirItem
    {
        public MerveSatirItem()
        {
            Sutunlar = new List<string>();
        }
        public List<string> Sutunlar { get; set; }
    }
    public class MerveSatirItem1
    {
        public MerveSatirItem1()
        {
            Sutunlar = new List<string>();
        }
        public List<string> Sutunlar { get; set; }
    }
}