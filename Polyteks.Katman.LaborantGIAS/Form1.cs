using Polyteks.Katman.DAL.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Polyteks.Katman.LaborantGIAS.Models;

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public SrcnKullanicis Kullanici { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            /*  int genislik = this.Size.Width;
              int yukseklik = this.Size.Height;
              MessageBox.Show(string.Format("Genişlik: {0} Yükseklik:{1}", genislik, yukseklik));

              int genislikE = Screen.PrimaryScreen.Bounds.Width;
              int yukseklikE = Screen.PrimaryScreen.Bounds.Height;
              MessageBox.Show(string.Format("Ekran Genişlik: {0} Yükseklik:{1}", genislikE, yukseklikE));
              */
            lblIsim.Text = Kullanici.IsimSoyisim.ToUpper();
          
            if (Kullanici.Resim != null)
            {
                // Convert byte[] to Image
                using (var ms = new MemoryStream(Kullanici.Resim, 0, Kullanici.Resim.Length))
                {
                    pictureBox1.Image = Image.FromStream(ms, true);
                }


            }

            var Liste = _dbPoly.SrcnGunlukImalatDosyas.OrderBy(a => a.KayitTarihi).Select(a => new GunlukImalatItem()
            {
                KayitTarihi = a.KayitTarihi,
                Birim = a.BirimAdi,
                //AnalizDurumu = a.LabAnalizDurumuAdi.ToUpper(),
                KayitNo = a.GunlukImalatDosyaId,
                TalepEden = a.KayitYapanKulAdi
            }).ToList();
            dtGunlukImalatlar.DataSource = Liste.ToList();
            
            dtGunlukImalatlar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void DtGunlukImalatlar_DoubleClick(object sender, EventArgs e)
        {
            GunlukImalatItem GunlukImalatItem = (GunlukImalatItem)dtGunlukImalatlar.CurrentRow.DataBoundItem;
            new FrmGunlukImalatDetay() {GunlukImalatItem = GunlukImalatItem, Kullanici = Kullanici}.Show();
        }
    }
}
