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
    public partial class FrmNumPadParolacs : Form
    {
        public FrmNumPadParolacs()
        {
            InitializeComponent();
        }

        private void grpAdi_Enter(object sender, EventArgs e)
        {
            grpAdi.Text = "PAROLANIZ";
        }
        public string Yazi { get; set; }
        public FrmTefrikGiris FrmTefrikGiris { get; set; }
        public int Durum { get; set; }
        public Form1 Form1 { get; set; }

        private void FrmNumPad_Load(object sender, EventArgs e)
        {
            textBox1.Text = Yazi;
           
        }
        private void btnsfr_Click(object sender, EventArgs e)
        {
            if (textBox1.PasswordChar.ToString() == "*")
            {
                textBox1.PasswordChar = char.Parse("\0");
                btnsfr.Text = "Gizle";
            }
            else
            {
                textBox1.PasswordChar = char.Parse("*");
                btnsfr.Text = "Göster";
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Set to no text.

            textBox1.Text = "";
            // The control will allow no more than 14 characters.
            textBox1.MaxLength = 14;
        }
     
        private void button1_Click_1(object sender, EventArgs e)
        {
            Yazi += "1";
            textBox1.Text = Yazi;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Yazi += "2";
            textBox1.Text = Yazi;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Yazi += "3";
            textBox1.Text = Yazi;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Yazi += "4";
            textBox1.Text = Yazi;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Yazi += "5";
            textBox1.Text = Yazi;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Yazi += "6";
            textBox1.Text = Yazi;
        }
        private void button7_Click_1(object sender, EventArgs e)
        {
            Yazi += "7";
            textBox1.Text = Yazi;
        }
        private void button8_Click_1(object sender, EventArgs e)
        {
            Yazi += "8";
            textBox1.Text = Yazi;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Yazi += "9";
            textBox1.Text = Yazi;
        }
        private void button10_Click_1(object sender, EventArgs e)
        {
            Yazi += "0";
            textBox1.Text = Yazi;
        }

        private void btnSil_Click_1(object sender, EventArgs e)
        {
            var charList = Yazi.ToCharArray();
            string yeniyazi = "";
            for (int i = 0; i < charList.Length - 1; i++)
            {
                yeniyazi += charList[i];
            }

            Yazi = yeniyazi;
            textBox1.Text = Yazi;
        }

        private void btnTamam_Click_1(object sender, EventArgs e)
        {
            Form1.txtSifre.Text = Yazi;
            Form1.btnGiris.PerformClick();
            this.Close();
        }

        
    }
    }
        
