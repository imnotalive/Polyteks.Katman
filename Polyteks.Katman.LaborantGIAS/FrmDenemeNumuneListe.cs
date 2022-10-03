using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.LaborantGIAS.Models;

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmDenemeNumuneListe : Form
    {
        public FrmDenemeNumuneListe()
        {
            InitializeComponent();
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public SrcnKullanicis Kullanici { get; set; }
        private void FrmDenemeNumuneListe_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            lblIsim.Text = Kullanici.IsimSoyisim;
          var liste =  _dbPoly.SrcnLabAnalizs.Where(a => a.DosyaTipi == 3 && (a.LabAnalizDurumu == 10 || a.LabAnalizDurumu == 11)).OrderBy(a=>a.KayitTarihi).Select(a=> new DenemeNumuneItem
          {
              KayitTarihi = a.KayitTarihi,
              Birim = a.BirimAdi,
              Cari = a.CariAdi.Trim(),
              TalepEden = a.KayitYapanKulAdi,
              AnalizDurumu = a.LabAnalizDurumuAdi,
              KayitNo = a.LabAnalizId
          }).ToList();
          dtDenemeNumuneler.DataSource = liste;

          dtDenemeNumuneler.Columns[0].FillWeight = 7;
          dtDenemeNumuneler.Columns[1].FillWeight = 10;
          dtDenemeNumuneler.Columns[2].FillWeight = 13;
          dtDenemeNumuneler.Columns[3].FillWeight = 15;
          dtDenemeNumuneler.Columns[4].FillWeight = 15;
          dtDenemeNumuneler.Columns[5].FillWeight = 20;
            dtDenemeNumuneler.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
          foreach (DataGridViewRow x in dtDenemeNumuneler.Rows)
          {
              x.MinimumHeight = 38;
              x.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
              x.DefaultCellStyle.Font = new Font("Tahoma", 10);
          }

        }

        private void DtDenemeNumuneler_DoubleClick(object sender, EventArgs e)
        {
            var seciliSatir = (DenemeNumuneItem)dtDenemeNumuneler.CurrentRow.DataBoundItem;


            new FrmDenemeNumuneDetayDuzenle() { Kullanici = Kullanici, LabAnalizId = seciliSatir.KayitNo }.Show();
            this.Close();
        }

        private void FrmDenemeNumuneListe_FormClosing(object sender, FormClosingEventArgs e)
        {
            //new FrmYapilacakIsler()
            //{
            //    Kullanici = Kullanici
            //}.Show();
        }
    }
}
