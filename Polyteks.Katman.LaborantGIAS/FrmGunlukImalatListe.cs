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
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.LaborantGIAS.Models;

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmGunlukImalatListe : Form
    {
        public FrmGunlukImalatListe()
        {
            InitializeComponent();
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public SrcnKullanicis Kullanici { get; set; }
        /*
         var liste = _dbPoly.SrcnGunlukImalatDosyas.Where(a => a.DosyaDurumId == 11 || a.DosyaDurumId == 12)
                .Select(a => new GunlukImalatItem
                {
                    KayitTarihi = a.KayitTarihi,
                    Birim = a.BirimAdi,
                    KayitNo = a.DosyaDurumId,
                    TalepEden = a.KayitYapanKulAdi,
                    AnalizDurumu = a.DosyaDurumAdi
                });

            dtGridImalatListe.DataSource = liste;
            lblIsim.Text = Kullanici.IsimSoyisim.ToUpper();

            if (Kullanici.Resim != null)
            {
                // Convert byte[] to Image
                using (var ms = new MemoryStream(Kullanici.Resim, 0, Kullanici.Resim.Length))
                {
                    pictureBox1.Image = Image.FromStream(ms, true);
                }


            }
         */
        private void FrmGunlukImalatListe_Load(object sender, EventArgs e)
        {
            this.StartPosition =  FormStartPosition.CenterScreen;
        }

        private void FrmGunlukImalatListe_Load_1(object sender, EventArgs e)
        {
            lblIsim.Text = Kullanici.IsimSoyisim;

            var liste = _dbPoly.SrcnGunlukImalatDosyas.Where(a => a.DosyaDurumId == 11 || a.DosyaDurumId == 12)
                .Select(a => new GunlukImalatItem
                {
                    KayitTarihi = a.KayitTarihi,
                    Birim = a.BirimAdi,
                    KayitNo = a.GunlukImalatDosyaId,
                    TalepEden = a.KayitYapanKulAdi,
                    AnalizDurumu = a.DosyaDurumAdi
                }).ToList();
          
            dtGunlukImalatlar.DataSource = liste;

            dtGunlukImalatlar.Columns[0].FillWeight = 10;
            dtGunlukImalatlar.Columns[1].FillWeight = 10;
            dtGunlukImalatlar.Columns[2].FillWeight = 15;
            dtGunlukImalatlar.Columns[3].FillWeight = 10;
            dtGunlukImalatlar.Columns[4].FillWeight = 20;
            dtGunlukImalatlar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (DataGridViewRow x in dtGunlukImalatlar.Rows)
            {
                x.MinimumHeight = 38;
                x.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            //lblIsim.Text = Kullanici.IsimSoyisim.ToUpper();

            //if (Kullanici.Resim != null)
            //{
            //    // Convert byte[] to Image
            //    using (var ms = new MemoryStream(Kullanici.Resim, 0, Kullanici.Resim.Length))
            //    {
            //        pictureBox1.Image = Image.FromStream(ms, true);
            //    }


            //}
        }

        private void BtnGunlukImalatEkle_Click(object sender, EventArgs e)
        {
            /*  if (_dbPoly.SrcnGunlukImalatDosyas.Any(a => (a.DosyaDurumId == 11 || a.DosyaDurumId == 12) && a.KayitYapanKulKodu==Kullanici.KullaniciId.ToString()))
              {
                  MessageBox.Show("Tamamlamanız Gereken Bu Kategoride Analizleriniz Bulunmaktadır");
              }
              else
              {
                  new FrmGunlukImalatEkle { Kullanici = Kullanici }.Show();
              }
            */
            new FrmGunlukImalatEkle { Kullanici = Kullanici }.Show();
        }

        private void dtGunlukImalatlar_DoubleClick(object sender, EventArgs e)
        {
            var seciliSatir = (GunlukImalatItem) dtGunlukImalatlar.CurrentRow.DataBoundItem;


            new FrmGunlukImalatDetayDuzenle { Kullanici = Kullanici, GunlukImalatDosyaId = seciliSatir.KayitNo}.Show();
            this.Close();
            
        }

        private void FrmGunlukImalatListe_FormClosing(object sender, FormClosingEventArgs e)
        {
            //new FrmYapilacakIsler()
            //{
            //    Kullanici = Kullanici
            //}.Show();
        }
    }
}
