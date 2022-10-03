using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmGunlukImalatEkle : Form
    {
        public FrmGunlukImalatEkle()
        {
            InitializeComponent();
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public SrcnKullanicis Kullanici { get; set; }

        public void FrmGunlukImalatEkle_Load(object sender, EventArgs e)
        {
            var liste = _dbPoly.SrcnFabrikaBirims.Where(a => a.BirimId < 5)
                .Select(a => new DropItem {Tanim = a.BirimAdi, IdInt = a.BirimId}).OrderBy(a => a.Tanim).ToList();

            cmbBirimler.DataSource = liste;
            cmbBirimler.DisplayMember = "Tanim";
            cmbBirimler.ValueMember = "IdInt";
        }

        private void TxtBobinSayisi_Click(object sender, EventArgs e)
        {
            var BirimId = ((DropItem) cmbBirimler.SelectedItem).IdInt;
            new FrmNumPad()
            {
                YapilacakIslem = 3,
                Yazi = txtBobinSayisi.Text,
                BirimId = BirimId,
                FrmGunlukImalatEkle= this

            }.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var BirimId = ((DropItem)cmbBirimler.SelectedItem).IdInt;
            var BobinSayisi = txtBobinSayisi.Text;
            if (BobinSayisi==null)
            {
                MessageBox.Show("LÜTFEN BOBİN SAYISI BELİRLEYİNİZ");
            }
            else
            {
                try
                {
                    if (BobinSayisi.Contains(","))
                    {
                        MessageBox.Show("LÜTFEN BOBİN SAYISI TEKRAR BELİRLEYİNİZ");
                    }
                    else
                    {
                        var intBobinSayisi = Convert.ToInt32(BobinSayisi);
                        if (intBobinSayisi>0)
                        {
                            var birim = _dbPoly.SrcnFabrikaBirims.Find(BirimId);
                            var dosyaDurum = _dbPoly.SrcnDosyaDurums.Find(11);
                 

                            var yeniGunlukImalatDosya = new SrcnGunlukImalatDosyas()
                            {
                                BirimId = birim.BirimId,
                                Aciklama = "",
                                BirimAdi = birim.BirimAdi,
                                KayitTarihi = DateTime.Now,

                                KayitYapanKulAdi = Kullanici.IsimSoyisim,
                                KayitYapanKulKodu = Kullanici.KullaniciId.ToString(),
                                DosyaDurumAdi = dosyaDurum.DosyaDurumAdi,
                                DosyaDurumId = dosyaDurum.DosyaDurumId,
                                ToplamBobinSayisi = intBobinSayisi
                            };
                            _dbPoly.SrcnGunlukImalatDosyas.Add(yeniGunlukImalatDosya);
                            _dbPoly.SaveChanges();

                            new FrmGunlukImalatDetayDuzenle()
                            {
                                GunlukImalatDosyaId = yeniGunlukImalatDosya.GunlukImalatDosyaId,
                                Kullanici = Kullanici
                            }.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("LÜTFEN BOBİN SAYISI TEKRAR BELİRLEYİNİZ");
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("LÜTFEN BOBİN SAYISI TEKRAR BELİRLEYİNİZ");
                }
            }

        }
    }
}
