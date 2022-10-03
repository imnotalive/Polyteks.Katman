using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class ImalatDenemeModel
    {
        public ImalatDenemeModel()
        {
            DenemeDosya = new SrcnDenemeDosya();
            Birimler = new List<SrcnFabrikaBirims>();
            DenemeDosyalari = new List<SrcnDenemeDosya>();
            Birim = new SrcnFabrikaBirims();
            Makine = new ZzzSrcnMakineIdli();
            Islem = new Islem();
        }

        public SelectList DropBirimMakineleri { get; set; }
        public ZzzSrcnMakineIdli Makine { get; set; }
        public string SecilenId { get; set; }
        public SrcnDenemeDosya DenemeDosya { get; set; }
        public List<SrcnFabrikaBirims> Birimler { get; set; }
        public SrcnFabrikaBirims Birim { get; set; }
        public List<SrcnDenemeDosya> DenemeDosyalari { get; set; }
        public Islem Islem { get; set; }
    }
}