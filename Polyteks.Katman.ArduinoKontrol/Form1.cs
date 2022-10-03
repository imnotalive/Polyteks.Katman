using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using  System.IO.Ports;

namespace Polyteks.Katman.ArduinoKontrol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var item in SerialPort.GetPortNames())
            {
                cmbPortlar.Items.Add(item);
            }
        }

        public void SeriportYaz(string veri)
        {
            if (serialPort1.IsOpen==false)
            {
                serialPort1.PortName = cmbPortlar.SelectedItem.ToString();
                serialPort1.Open();
            }
       

            serialPort1.Write(veri);
            //serialPort1.Close();
        }
        public string SeriportVeri { get; set; }
        private void btnYak_Click(object sender, EventArgs e)
        {
            SeriportYaz(txtVeriler.Text);
        }

        private void btnSondur_Click(object sender, EventArgs e)
        {
            SeriportYaz("2");
        }
    }
}
