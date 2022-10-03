using Polyteks.Katman.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Polyteks.Katman.Entities;
using Polyteks.Katman.DAL.Concrete.EntityFramework;

namespace Polyteks.Katman.WFAanalizGonderim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public DropItem UretimHatti { get; set; }
        public Makine Makine { get; set; }
        public SrcnLabGrups Labgrup { get; set; }
        public int pozBaslangic { get; set; }
        public int pozBitis { get; set; }
        public string PartiNo { get; set; }
        public string PersonelNo { get; set; }
        public int YazdirmaBicimi { get; set; }
        public string Dtex { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            UretimHatti = new DropItem();
            Makine = new Makine();
            Labgrup = new SrcnLabGrups();
            var UretimHatlari = Helper.UretimHatlariGetir();
            cmbUretimHatlari.DataSource = UretimHatlari.ToList();
            cmbUretimHatlari.DisplayMember = "Tanim";
            cmbUretimHatlari.ValueMember = "Id";
            var ImalatLabGruplar = _dbPoly.SrcnLabGrups.Where(a => a.AnalizTipi == 0 && a.UstLabGrupId != 0)
                .OrderBy(a => a.LabGrupAdi).ToList();
            cmbIplıkler.DataSource = ImalatLabGruplar.ToList();
            cmbIplıkler.DisplayMember = "LabGrupAdi";
            cmbIplıkler.ValueMember = "LabGrupId";

        }
        private void CmbUretimHatlari_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (DropItem)cmbUretimHatlari.SelectedItem;
            int id = Convert.ToInt32(item.Id);
            var Makineler = _dbPoly.Makine.Where(a => a.SBSonrasiCalismaSaati == id).OrderBy(a => a.MakineNo).ToList();

            cmbMakineler.DataSource = Makineler.ToList();
            cmbMakineler.DisplayMember = "MakineNo";
            cmbMakineler.ValueMember = "SayacOndalikSayisi";
        }
        private void BtnYazdir_Click(object sender, EventArgs e)
        {
            UretimHatti = (DropItem)cmbUretimHatlari.SelectedItem;
            Makine = (Makine)cmbMakineler.SelectedItem;
            Labgrup = (SrcnLabGrups)cmbIplıkler.SelectedItem;
            PartiNo = txtPartiNo.Text;
            pozBaslangic = (int)numericUpDown1.Value;
            pozBitis = (int)numericUpDown2.Value;
            Dtex = txtDtex.Text;
            PersonelNo = txtPersonelNo.Text;
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                //  printDocument1.Print();
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.PrintPreviewControl.Zoom = 1;
                printPreviewDialog1.ShowDialog();
            }
        }
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           
            
            int baslangic = 0;
            int bitis = 0;
            var font = new Font("Arial", 10, FontStyle.Regular);
            var firca = Brushes.Black;
            if (pozBaslangic < pozBitis)
            {
                bitis = pozBitis;
                baslangic = pozBaslangic;
            }
            else
            {
                baslangic = pozBitis;
                bitis = pozBaslangic;
            }

            int satirSay = 0;
            int sutunSay = 0;
            int xArttirim = 140;
            int yArttirim = 105;

            int xbaslangic = 50;
            int ybaslangic = 50;
            
            var ListeTumSayilar = new List<int>();
            var ListeTekSayilar = new List<int>();
            var ListeCiftSayilar = new List<int>();
            var CıkartilecekSayilar = new List<int>();
            for (int i = baslangic; i <= bitis; i++)
            {
                ListeTumSayilar.Add(i);

                if (i % 2 == 0)
                {
                    ListeCiftSayilar.Add(i);
                }
                else
                {
                    ListeTekSayilar.Add(i);
                }
            }

            if (rdTumunuYazdir.Checked)
            {
                CıkartilecekSayilar = ListeTumSayilar;
            }
            else
            {
                if (rdCiftlerYazdir.Checked)
                {
                    CıkartilecekSayilar = ListeCiftSayilar;
                }
                else
                {
                    CıkartilecekSayilar = ListeTekSayilar;
                }
            }

            int SayfaYukseklik = yArttirim;
        

            foreach (var i in CıkartilecekSayilar)
            {

             satirSay++;
                sutunSay++;
               
                SayfaYukseklik +=yArttirim;
                Pen selPen = new Pen(Color.Blue);
                e.Graphics.DrawRectangle(selPen, xbaslangic, ybaslangic, 135, 100);

                e.Graphics.DrawString(PartiNo + " " + i.ToString(), font, firca, xbaslangic, ybaslangic);

                e.Graphics.DrawString(Makine.MakineNo.Replace(" ","") + " " + i.ToString(), font, firca, xbaslangic, ybaslangic+14);
                
                e.Graphics.DrawString(PersonelNo.Replace(" ", "") + " " + i.ToString(), font, firca, xbaslangic, ybaslangic + 28);
                e.Graphics.DrawString(Dtex.Replace(" ", "") + " " + i.ToString(), font, firca, xbaslangic, ybaslangic + 42);
                e.Graphics.DrawString(Labgrup.LabGrupAdi + " " + i.ToString(), font, firca, xbaslangic, ybaslangic + 56);
                //if (ybaslangic >= e.MarginBounds.Bottom)
                //{
                //    e.HasMorePages = true;
                //    ybaslangic = e.MarginBounds.Top;
                //}
                //else
                //{
                //    //e.HasMorePages = false;
                //    e.Graphics.DrawString(UretimHatti.Tanim + " " + i.ToString(), font, firca, xbaslangic, ybaslangic);
                //}

                //e.Graphics.DrawString(UretimHatti.Tanim + " " + i.ToString(), font, firca, xbaslangic, ybaslangic);

                if (sutunSay == 3)
                {
                    sutunSay = 0;

                    ybaslangic += yArttirim;
                }

                if (satirSay == 3)
                {
                    xbaslangic -= (3 * xArttirim);
                    satirSay = 0;
                }
                xbaslangic += xArttirim;

             
            }

            e.HasMorePages = false;
        }
    }
}
