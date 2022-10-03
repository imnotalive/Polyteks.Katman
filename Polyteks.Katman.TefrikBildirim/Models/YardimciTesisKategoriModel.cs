using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.TefrikBildirim.Models
{
    public class YardimciTesisKategoriModel
    {
        public YardimciTesisKategoriModel()
        {
            YardimciTesisKontrolNoktalari = new List<SrcnYardimciTesisKontrolNoktas>();
        }
        public string Kategori { get; set; }

        public List<SrcnYardimciTesisKontrolNoktas> YardimciTesisKontrolNoktalari { get; set; }
    }
}