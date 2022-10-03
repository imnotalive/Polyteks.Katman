using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polyteks.Katman.WFATefrikProgrami
{
    public partial class FrmNumPad : Form
    {
        public void DurumaGoreGroupBox()
        {
            switch (Durum)
            {
                case 1:
                    grpAdi.Text = "PAROLANIZ";
                    break;
                case 2:
                    grpAdi.Text = "İŞ EMRİ NUMARASI";
                    break;
                case 3:
                    grpAdi.Text = "TOPLAM BOBİN SAYISI";
                    break;
                case 4:
                    grpAdi.Text = "KISA BOBİN SAYISI";
                    break;
                case 5:
                    grpAdi.Text = "POZİSYONSUZ BOBİN SAYISI";
                    break;
                case 6:
                    grpAdi.Text = "ÖN TEFRİK TOPLAM BOBİN SAYISI";
                    break;
                case 7:
                    grpAdi.Text = "ÖN TEFRİK KISA BOBİN SAYISI";
                    break;
                case 8:
                    grpAdi.Text = "ÖN TEFRİK POZİSYONSUZ BOBİN SAYISI";
                    break;
                case 9:
                    grpAdi.Text = "POZİSYON NUMARASI";
                    break;
                case 10:
                    grpAdi.Text = "ÖN POZİSYON NUMARASI";
                    break;
                case 11:
                    grpAdi.Text = "ÇORAP ARABA NUMARASI";
                    break;
            }
        }

        public FrmNumPad()
        {
            InitializeComponent();
        }
        public int Durum { get; set; }
        public Form1 Form1 { get; set; }
        public string Yazi { get; set; }
        public FrmTefrikGiris FrmTefrikGiris { get; set; }
        private void FrmNumPad_Load(object sender, EventArgs e)
        {
            rchYazi.Text = Yazi;
            DurumaGoreGroupBox();
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

        private void BtnTamam_Click(object sender, EventArgs e)
        {
            if (Durum == 1)
            {
                Form1.txtSifre.Text = Yazi;
                Form1.btnGiris.PerformClick();
            }
            if (Durum == 2)
            {
                FrmTefrikGiris.txtIsEmri.Text = Yazi;
                FrmTefrikGiris.btnIsEmriKontrol.PerformClick();
            }
            if (Durum == 3)
            {
                rchYazi.Text = "";
                int say = Convert.ToInt32(Yazi);
                if (say>350)
                {
                    MessageBox.Show("Lütfen Toplam Bobin Sayısını Kontrol Ediniz");
                }
                else
                {
                    FrmTefrikGiris.txtToplamBobin.Text = Yazi;
         
                }
              
            }
            if (Durum == 4)
            {
                rchYazi.Text = "";
                int say = Convert.ToInt32(Yazi);
                if (say > 350)
                {
                    MessageBox.Show("Lütfen Kısa Bobin Sayısını Kontrol Ediniz");
                }
                else
                {
                    FrmTefrikGiris.txtKisaBobin.Text = Yazi;

                }
          
            }
            if (Durum == 5)
            {
                FrmTefrikGiris.txtPozisyonsuz.Text = Yazi;
                FrmTefrikGiris.btnPozisyonsuzEkle.PerformClick();
            }

            //
            if (Durum == 6)
            {
                int say = Convert.ToInt32(Yazi);
                if (say > 350)
                {
                    MessageBox.Show("Lütfen Toplam Bobin Sayısını Kontrol Ediniz");
                }
                else
                {
                    FrmTefrikGiris.txtOnTefToplamBobin.Text = Yazi;

                }

            }
            if (Durum == 7)
            {
                int say = Convert.ToInt32(Yazi);
                if (say > 350)
                {
                    MessageBox.Show("Lütfen Kısa Bobin Sayısını Kontrol Ediniz");
                }
                else
                {
                    FrmTefrikGiris.txtOnTefKisaBobin.Text = Yazi;

                }

            }
            if (Durum == 8)
            {
                FrmTefrikGiris.txtOnTefPozisyonsuz.Text = Yazi;
                FrmTefrikGiris.btnOnTefrikPozisyonsuzEkle.PerformClick();
            }
            this.Close();
            if(Durum == 9)
            {   
                    FrmTefrikGiris.txtPozisyonArama.Text = Yazi;
                     FrmTefrikGiris.button6.PerformClick();

            }
            if (Durum == 10)
            {
                FrmTefrikGiris.txtPozisyonSearch.Text = Yazi;
                FrmTefrikGiris.btnPozisyonSearch.PerformClick();

            }
            if (Durum == 11)
            {
                int say = Convert.ToInt32(Yazi);
                if (say==0 || say >200)
                {
                    MessageBox.Show("Çorap Araba Numarasını Kontrol Ediniz");
                }
                else
                {
                    FrmTefrikGiris.txtCorapArabaNo.Text = Yazi;

                }

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
    }
}
