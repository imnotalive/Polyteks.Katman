using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.Has.Models
{
    public class TefrikGrupPersonelModel
    {
        public TefrikGrupPersonelModel()
        {
            SrcnKullanici = new SrcnKullanicis();
            Makine = new Makine();
        }
        public int GrupNo { get; set; }
        public SrcnKullanicis SrcnKullanici { get; set; }
        public int SrcnKulaniciId { get; set; }
        public SelectList DropPersoneller { get; set; }
        public Makine Makine { get; set; }
        public string Sifre { get; set; }

    }
}