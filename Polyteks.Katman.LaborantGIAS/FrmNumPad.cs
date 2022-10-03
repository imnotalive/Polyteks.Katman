using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.LaborantGIAS.Models;

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmNumPad : Form
    {
        public FrmNumPad()
        {
            InitializeComponent();
        }
        public void LabHareketLogGuncelle(int LabAnalizId, string YeniDeger, string EskiDeger, string KullanilanAlan)
        {
            _dbPoly = new POTA_PTKSEntities();
            var aa = new SrcnLabAnalizLogs()
            {
                LabAnalizId = LabAnalizId,
                KayitTarihi = DateTime.Now,
                YeniDeger = YeniDeger,
                EskiDeger = EskiDeger,
                Aciklama = string.Format("{0} alanı değeri {1} değerinden {2} değeri olarak olarak güncellenmiştir. - {3}", KullanilanAlan, EskiDeger, YeniDeger, Kullanici.IsimSoyisim.ToString())
            };
            try
            {
                _dbPoly.SrcnLabAnalizLogs.Add(aa);
                _dbPoly.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



        }
        public SrcnLabAnalizItemAnalizCesitSonucs SonucuOlustur()
        {
            int kanalNo = Convert.ToInt32(KanalNo);
            var LabAnalizler = _dbPoly.SrcnLabAnalizs
                .Where(a => a.AnalizTipi == 0 && a.BagliOlduguDosyaId == GunlukImalatItem.KayitNo && a.RefakartKartNo == ReferansNo && a.KanalNo == kanalNo)
                .OrderBy(a => a.PartiNo).ToList();

            foreach (var item in LabAnalizler)
            {
                var labAnalizItemlar = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == item.LabAnalizId).OrderBy(a => a.BobinAdi).ToList();

                var labAnalizItemSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();

                foreach (var aa in labAnalizItemlar)
                {
                    labAnalizItemSonuclar.AddRange(_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                        .Where(a => a.LabAnalizItemId == aa.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi).ToList());
                }

                var SatirIdler = labAnalizItemSonuclar.OrderBy(a => a.LabAnalizCesitAdi).GroupBy(a => a.LabAnalizCesitId).ToList();

                int SatirId = Convert.ToInt32(SatirIdler[SatirDegeri].Key);

                var Sutunlar = labAnalizItemSonuclar.Where(a => a.LabAnalizCesitId == SatirId)
                    .OrderBy(a => a.LabAnalizCesitAdi).ToList();


                return Sutunlar[SutunDegeri-1];


            }
            return new  SrcnLabAnalizItemAnalizCesitSonucs();
        }

        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
   
        public string Yazi { get; set; }
        public int YapilacakIslem { get; set; } //1- tabpage satır sutun değeri
        public string ReferansNo { get; set; }
        public string KanalNo { get; set; }
        public int SatirDegeri { get; set; }
        public int SutunDegeri { get; set; }
        public int SayfaDegeri { get; set; }
        public int BirimId { get; set; }
        public int BobinSayisi { get; set; }
        public GunlukImalatItem GunlukImalatItem { get; set; }
        public FrmGunlukImalatDetay FrmGunlukImalatDetay { get; set; }
        public SrcnKullanicis Kullanici { get; set; }
        public FrmGunlukImalatEkle FrmGunlukImalatEkle { get; set; }
        public FrmDenemeNumuneDetayDuzenle FrmDenemeNumuneDetayDuzenle { get; set; }

        public FrmGunlukImalatDetayDuzenle FrmGunlukImalatDetayDuzenle { get; set; }
        public void DurumaGoreGroupBox()
        {
            switch (YapilacakIslem)
            {
                case 1:
                    grpAdi.Text = "ANALİZ DEĞERİ";
                    break;
                case 2:
                    grpAdi.Text = "KULLANICI ŞİFRENİZ";
                    break;
                case 3:
                    grpAdi.Text = "TOPLAM BOBİN SAYISI";
                    break;
                case 4:
                    grpAdi.Text = "ANALİZ DEĞERİ";
                    break;
                case 5:
                    grpAdi.Text = "POZİSYONSUZ BOBİN SAYISI";
                    break;

            }
        }
        private void FrmNumPad_Load(object sender, EventArgs e)
        {
            DurumaGoreGroupBox();
            rchYazi.Text = Yazi;
            this.Text = grpAdi.Text;
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            if (Yazi.Contains(','))
            {
                MessageBox.Show("2. virgül konulamaz");
            }
            else
            {
                Yazi += ",";
                rchYazi.Text = Yazi;
            }
        }

        private void BtnTumunuSil_Click(object sender, EventArgs e)
        {
            Yazi = null;
            rchYazi.Text = Yazi;
        }

        private void BtnTamam_Click(object sender, EventArgs e)
        {
            if (YapilacakIslem == 1)
            {

                FrmGunlukImalatDetayDuzenle.AnalizDegerGuncellemeYap(SatirDegeri, SutunDegeri, SayfaDegeri, Yazi);
            }

            if (YapilacakIslem == 2)
            {
                if (Kullanici.Sifre == Yazi)
                {
                    new FrmYapilacakIsler
                    {
                        Kullanici = Kullanici
                    }.Show();
                }
                else
                {
                    MessageBox.Show("Girilen Şifre Hatalıdır");

                }
            }


            if (YapilacakIslem == 3)
            {
                FrmGunlukImalatEkle.txtBobinSayisi.Text = Yazi;
            }

            if (YapilacakIslem == 4)
            {
                FrmDenemeNumuneDetayDuzenle.AnalizDegerGuncellemeYap(SatirDegeri, SutunDegeri,  Yazi);
            }

            this.Close();

        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Yazi += "0";
            rchYazi.Text = Yazi;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Yazi += "1";
            rchYazi.Text = Yazi;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Yazi += "2";
            rchYazi.Text = Yazi;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Yazi += "3";
            rchYazi.Text = Yazi;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Yazi += "4";
            rchYazi.Text = Yazi;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Yazi += "5";
            rchYazi.Text = Yazi;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Yazi += "6";
            rchYazi.Text = Yazi;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Yazi += "7";
            rchYazi.Text = Yazi;
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Yazi += "8";
            rchYazi.Text = Yazi;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            Yazi += "9";
            rchYazi.Text = Yazi;
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            var charList = Yazi.ToCharArray();
            string yeniyazi = "";
            for (int i = 0; i < charList.Length - 1; i++)
            {
                yeniyazi += charList[i];
            }

            Yazi = yeniyazi;
            rchYazi.Text = Yazi;
        }

        private void BtnKopyala_Click(object sender, EventArgs e)
        {
            Yazi = rchYazi.Text;
            if (YapilacakIslem == 1)
            {
                FrmGunlukImalatDetayDuzenle.HafizayaKopya = Yazi;
            
                MessageBox.Show("Kopyalama İşlemi Yapılmıştır");
            }

       
        }

        private void BtnYapistir_Click(object sender, EventArgs e)
        {
            if (YapilacakIslem == 1)
            {
             
                Yazi = FrmGunlukImalatDetayDuzenle.HafizayaKopya;
                rchYazi.Text = Yazi;

            }
        }
    }
}
