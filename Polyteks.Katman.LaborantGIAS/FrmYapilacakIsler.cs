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

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmYapilacakIsler : Form
    {
        public FrmYapilacakIsler()
        {
            InitializeComponent();
        }

        public SrcnKullanicis Kullanici { get; set; }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();

        public void GuncellemeYap()
        {
            var giatSay = _dbPoly.SrcnGunlukImalatDosyas.Count(a => a.DosyaDurumId == 11 || a.DosyaDurumId == 12);
            btnGIAT.Text = string.Format("GÜNLÜK İMALAT ANALİZLERİ ({0})", giatSay);

            int denemeNumuneSay = _dbPoly.SrcnLabAnalizs.Count(a => a.DosyaTipi == 3 && (a.LabAnalizDurumu == 10 || a.LabAnalizDurumu == 11));

            if (denemeNumuneSay == 0)
            {
                btnDenemeNumune.Text = "DENEME NUMUNE ANALİZLERİ";
                btnDenemeNumune.BackColor = Color.BurlyWood;
            }
            else
            {
                btnDenemeNumune.Text = string.Format("DENEME NUMUNE ANALİZLERİ ({0})", denemeNumuneSay);
                btnDenemeNumune.BackColor = Color.Red;
            }

            lblTarih.Text = DateTime.Now.ToString();
        }
        private void FrmYapilacakIsler_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.StartPosition = FormStartPosition.CenterScreen;
            label1.Text = Kullanici.IsimSoyisim.ToUpper();
            GuncellemeYap();
        }
        private void BtnOAT_Click(object sender, EventArgs e)
        {
            // özel analiz talebi
        }

    

        private void BtnGIAT_Click(object sender, EventArgs e)
        {
            // günlük imalat analiz talebi

            new FrmGunlukImalatListe()
            {
                Kullanici = Kullanici
            }.Show();
          

        }

        private void BtnDenemeNumune_Click(object sender, EventArgs e)
        {
            new FrmDenemeNumuneListe()
            {
                Kullanici = Kullanici
            }.Show();
           
        }

        private int sayac = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            sayac++;
        
            if (sayac==120)
            {
                sayac = 0;
                GuncellemeYap();
            }
        }
    }
}
