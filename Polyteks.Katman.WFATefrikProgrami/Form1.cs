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
using Polyteks.Katman.WFATefrikProgrami.Model;

namespace Polyteks.Katman.WFATefrikProgrami
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public POTA_PTKSEntities _dbPoly { get; set; }
        
        public Personel Personel { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            _dbPoly = new POTA_PTKSEntities();

            lstKullanicilar.DataSource = _dbPoly.Personel.Where(a => a.KullaniciKodu == "TEFRIK" && a.EvTelefonu != null && a.IstenCikisTarihi == null).OrderBy(a => a.PersonelAdi).ToList();

            lstKullanicilar.DisplayMember = "PersonelAdi";
            WindowState = FormWindowState.Maximized;
           lblIsimSoyisim.Visible = false;
           fillChart();
        }

        private void LstKullanicilar_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (Personel)lstKullanicilar.SelectedItem;
            lblIsimSoyisim.Text = item.PersonelAdi.ToUpper();
            lblIsimSoyisim.Visible = true;
            picResim.Image = null;
            if (item.Resim != null)
            {
                // Convert byte[] to Image
                using (var ms = new MemoryStream(item.Resim, 0, item.Resim.Length))
                {
                    picResim.Image = Image.FromStream(ms, true);
                }


            }
        }

        private void TxtSifre_Click(object sender, EventArgs e)
        {
            new FrmNumPadParolacs()
            {
                Durum = 1,
                Yazi = txtSifre.Text,
                Form1 =  this
          

            }.Show();
            //  this.Close();
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            var personel = (Personel) lstKullanicilar.SelectedItem;

            if (personel.EvTelefonu.Trim()== txtSifre.Text)
            {
                new FrmTefrikGiris()
                {
                    Personel = personel
                }.Show();
            }
            else
            {
                MessageBox.Show("ŞİFRENİZ HATALIDIR");
            }
        }

        private void fillChart()
        {
            var vardiyaBaslangic = DateTime.Now.AddDays(-1).Date.AddHours(23);
            var bugnHareketler = _dbPoly.UretimHareket.Where(a => a.UretimBitisTarihi > vardiyaBaslangic && a.SiraNo == 300 && a.CalisanPersonel1 != null && a.UretimBitisTarihi != null).ToList();
            var personelKodlar = bugnHareketler.Select(a => a.CalisanPersonel1).Distinct().ToList();
            var GrafikItemlar = new List<GrafikItem>();
            foreach (var item in personelKodlar)
            {
                if (_dbPoly.Personel.Any(a => a.PersonelNo.Replace(" ", "") == item.Replace(" ", "")))
                {
                    var itt = new GrafikItem();
                    itt.Xdeger = _dbPoly.Personel.First(a => a.PersonelNo.Replace(" ", "") == item.Replace(" ", ""))
                        .PersonelAdi;

                    var personelHareketler = bugnHareketler.Where(a => a.CalisanPersonel1.Replace(" ", "") == item.Replace(" ", "")).ToList();


                    itt.Ydeger = (decimal)bugnHareketler.Where(a => a.CalisanPersonel1.Replace(" ", "") == item.Replace(" ", "")).Sum(a => a.Miktar1);
                    GrafikItemlar.Add(itt);
                }
            }

            double say = 0;
            foreach (var item in GrafikItemlar.OrderByDescending(a => a.Ydeger).ToList())
            {
                say++;
                chart1.Series[0].Points.AddXY(say, item.Ydeger);
                chart1.ChartAreas[0].AxisX.CustomLabels.Add(say - 0.5, say + 0.5, item.Xdeger);
            }
            chart1.Series["Series1"].LegendText = "Tefrik Miktarı";
            chart1.Titles.Add("PERSONEL TEFRİK MİKTAR LİSTESİ");
            chart1.Series["Series1"].IsVisibleInLegend = false;
            chart1.Series["Series1"].Color = Color.Red;
            chart1.Series["Series1"].IsVisibleInLegend = false;
            chart1.Series["Series1"].IsValueShownAsLabel = true;
        }

        private void BtnVeriYenile_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vScrolKullanici_Scroll(object sender, ScrollEventArgs e)
        {
            var deger = vScrolKullanici.Value;

            lstKullanicilar.TopIndex = deger;
        }
    }
    }

