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

namespace Polyteks.Katman.WFATefrikProgrami
{
    public partial class FrmNumpadPartiNo : Form
    {
        public FrmNumpadPartiNo()
        {
            InitializeComponent();
        }
        public string Yazi { get; set; }
        public POTA_PTKSEntities _dbPoly { get; set; }
        public FrmTefrikGiris FrmTefrikGiris { get; set; }

        #region Butonlar
        private void BtnSil_Click(object sender, EventArgs e)
        {
           
            rchYazi.Text = rchYazi.Text.Substring(0,rchYazi.Text.Length - 1);
            if(rchYazi.Text=="")
            {
                rchYazi.Text = "0";
            }
            //string yeniyazi = Yazi.Substring(0, Yazi.Length - 1);
            //if (Convert.ToDouble(btnSil.Text) > 0)
            //{
            //    btnSil.Text = btnSil.Text.Remove(btnSil.Text.Length - 1, 1);
            //}

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
        #endregion
        

        private void FrmNumpadPartiNo_Load(object sender, EventArgs e)
        {
            _dbPoly = new POTA_PTKSEntities();
            grpPartiSecimi.Visible = false;
            grpMakineSecimi.Visible = false;
            btnSecimiKaydet.Visible = false;
        }

        private void RchYazi_TextChanged(object sender, EventArgs e)
        {
            var partiler = new List<SrcnPartiAnaKlasors>();
            if (Yazi == null)
            {
                MessageBox.Show("Lütfen Parti No Belirleyiniz.");
                grpPartiSecimi.Visible = false;
                grpMakineSecimi.Visible = false;
                btnSecimiKaydet.Visible = false;
            }
            else
            {
                if (Yazi.ToCharArray().Length>2)
                {
                    partiler = _dbPoly.SrcnPartiAnaKlasors.Where(a => a.PartiAdi.Contains(Yazi)).ToList();
                 
                    grpPartiSecimi.Visible = true;
                    grpMakineSecimi.Visible = true;
                    btnSecimiKaydet.Visible = true;
                }
          
            }
            lstPartiler.DataSource = partiler;
            lstPartiler.DisplayMember = "PartiAdi";

            var item = lstPartiler.SelectedItem;
            var liste = new List<ZzzSrcnMakineIdli>();


            lstMakineler.DataSource = liste;
            lstMakineler.DisplayMember = "MakineNo";


        }

        private void LstPartiler_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = lstPartiler.SelectedItem;
            var liste = new List<ZzzSrcnMakineIdli>();
            lblBirim.Text = "";
            if (item!=null)
            {
                var partiAnaKlasor = (SrcnPartiAnaKlasors) item;
                liste = _dbPoly.ZzzSrcnMakineIdli.Where(a => a.BirimId == partiAnaKlasor.BirimId)
                    .OrderBy(a => a.MakineNo).ToList();
         
                lblBirim.Text = partiAnaKlasor.BirimAdi;

            }
            lstMakineler.DataSource = liste;
            lstMakineler.DisplayMember = "MakineNo";
        }

        private void btnSecimiKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var Parti = (SrcnPartiAnaKlasors)lstPartiler.SelectedItem;
                var Makine = (ZzzSrcnMakineIdli)lstMakineler.SelectedItem;

                if (Parti.PartiAnaKlasorId==0 || Makine.BirimId==null)
                {
                    MessageBox.Show("Lütfen Seçim Yapın");

                }
                else
                {
                    FrmTefrikGiris.PartiAnaKlasor = Parti;
                    FrmTefrikGiris.MakineIdli = Makine;
                    FrmTefrikGiris.OnTefrikBilgiGoster();
                    this.Close();
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Bilinmeyen Bir Hata Oluştu");
            }
          

        }

        private void button11_Click(object sender, EventArgs e)
        {
            Yazi = "";
            var charList = Yazi.ToCharArray();
            string yeniyazi = "";
            for (int i = 0; i < charList.Length - 1; i++)
            {
                yeniyazi += charList[i];
            }

            Yazi = yeniyazi;
            rchYazi.Text = Yazi;
        }

        private void BtnTamam_Click_1(object sender, EventArgs e)
        {
            if (Yazi == null)
            {
                MessageBox.Show("Lütfen Parti No Belirleyiniz.");
            }
            else
            {
                var partiler = _dbPoly.SrcnPartiAnaKlasors.Where(a => a.PartiAdi.Contains(Yazi)).ToList();
                lstPartiler.DataSource = partiler;
                lstPartiler.DisplayMember = "PartiAdi";
            }
        }
    }
}
