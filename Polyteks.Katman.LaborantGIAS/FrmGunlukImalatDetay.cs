using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Polyteks.Katman.DAL.Concrete.EntityFramework;
using Polyteks.Katman.LaborantGIAS.Models;

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmGunlukImalatDetay : Form
    {
        public FrmGunlukImalatDetay()
        {
            InitializeComponent();
        }

        public GunlukImalatItem GunlukImalatItem { get; set; }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public SrcnKullanicis Kullanici { get; set; }

        private void FrmGunlukImalatDetay_Load(object sender, EventArgs e)
        {
            var LabAnalizler = _dbPoly.SrcnLabAnalizs
                .Where(a => a.AnalizTipi == 0 && a.BagliOlduguDosyaId == GunlukImalatItem.KayitNo)
                .OrderBy(a => a.PartiNo);
            int i = 50;
            int red = 255;
            int blue = 0;
            foreach (var item in LabAnalizler)
            {
                red -= i;
                blue += i;

                var id = item.RefakartKartNo.Trim() + "-" + item.KanalNo;
                if (red < 0)
                {
                    red = 255;
                }
                if (blue > 255)
                {
                    blue = 0;
                }

                var rnd = new Random();

                var yeniTabPage = new TabPage()
                {
                    Name = "tab" + id,
                    Text = item.PartiNo.ToUpper(),
                    Width = this.Size.Width,
                    Height = this.Size.Height,
                    BackColor = Color.FromArgb(rnd.Next(0,255), rnd.Next(0, 255), rnd.Next(0, 255))

                };
                var labAnalizItemlar = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId == item.LabAnalizId).OrderBy(a => a.BobinAdi).ToList();
                var labAnalizItemSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();

                foreach (var aa in labAnalizItemlar)
                {
                    labAnalizItemSonuclar.AddRange(_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                        .Where(a => a.LabAnalizItemId == aa.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi).ToList());
                }

                var dtgrid = new DataGridView();

                var Headerlar = labAnalizItemlar.GroupBy(a => a.BobinAdi).ToList();


                var labAnalizCesitleri = labAnalizItemSonuclar.GroupBy(a => a.LabAnalizCesitAdi).ToList();
                var SatirIdler = labAnalizItemSonuclar.OrderBy(a => a.LabAnalizCesitAdi).GroupBy(a => a.LabAnalizCesitId).ToList();

                dtgrid.ColumnCount = Headerlar.Count() + 1;
                dtgrid.RowCount = labAnalizCesitleri.Count();
                dtgrid.Columns[0].HeaderText = "ANALİZ";
                // dtgrid.Columns[1].HeaderText = "Pozisyon";
                int ii = 1;
                foreach (var j in Headerlar)
                {
                    dtgrid.Columns[ii].HeaderText = j.Key.ToString();
                    ii++;
                }

                dtgrid.Name = "dt" + id;
                dtgrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dtgrid.ColumnHeadersHeight = 60;
                dtgrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgrid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgrid.RowsDefaultCellStyle.Font = new Font("Verdana", 12, FontStyle.Bold);
       
                dtgrid.Location = new Point(5, 100);
                dtgrid.AllowUserToOrderColumns = false;

                dtgrid.Width = 1240;
                dtgrid.Height = 540;
                dtgrid.CellClick += new DataGridViewCellEventHandler(dtgrid_CellClick);
                int Satir = 0;
                int Sutun = 0;
                foreach (var aa in labAnalizCesitleri)
                {
                    dtgrid.Rows[Satir].Cells[Sutun].Value = aa.Key;
                    Satir++;
                }
                Satir = -1;

                foreach (var SatirId in SatirIdler.ToList())
                {

                    var idd = Convert.ToInt32(SatirId.Key);
                    var Sutunlar = labAnalizItemSonuclar.Where(a => a.LabAnalizCesitId == idd)
                        .OrderBy(a => a.LabAnalizCesitAdi).Select(a => a.AnalizDegeriString).ToList();
                    Satir++;
                    Sutun = 0;
                    foreach (var s in Sutunlar)
                    {
                        Sutun++;
                        dtgrid.Rows[Satir].Cells[Sutun].Value = s;
                    }

                }
                //dtgrid.DataSource = labAnalizItemlar;
                foreach (DataGridViewColumn column in dtgrid.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                foreach (DataGridViewRow x in dtgrid.Rows)
                {
                    x.MinimumHeight = 30;
                }

                var RefakatKartNoStokAd = _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.First(a => a.RefakatNo == item.RefakartKartNo);
                string lblText = string.Format("PARTİ: {0}, STOK ADI: {1},\r\nSTOK KODU: {2}, İŞ EMRİ NUMARASI: {3}",
                    item.PartiNo.Trim(), 
                    RefakatKartNoStokAd.StokAdi.TrimEnd(), RefakatKartNoStokAd.StokKodu.Trim(), item.RefakartKartNo.Trim());

                var label = new Label
                {
                    Location =  new Point(1,20),
                    Name = "lbl"+id,
                    ForeColor = Color.White,
               

            };
                label.AutoSize = false;
                label.Width = 800;
                label.Height = 40;
              
                label.Text = lblText;
                yeniTabPage.Controls.Add(label);
                yeniTabPage.Controls.Add(dtgrid);
                tabControl.TabPages.Add(yeniTabPage);

            }

        }
        private void dtgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;


            string Bilgi = grid.Name.Replace("dt", "");

            var arrayBilgi = Bilgi.Split('-').ToArray();

            string refNo = arrayBilgi[0].ToString();
            string KanalNo = arrayBilgi[1].ToString();
            // MessageBox.Show(refNo + " " + KanalNo);


            int sutun = grid.CurrentCell.ColumnIndex;
            int satir = grid.CurrentRow.Index;


            if (sutun > 0)
            {
                string guncelDeger = null;
                if (grid.Rows[satir].Cells[sutun].Value != null)
                {
                    guncelDeger = grid.Rows[satir].Cells[sutun].Value.ToString();
                }
                new FrmNumPad()
                {
                    KanalNo = KanalNo,
                    YapilacakIslem = 1,
                    ReferansNo = refNo,
                    SatirDegeri = satir,
                    SutunDegeri = sutun,
                    Yazi = guncelDeger,
                    //FrmGunlukImalatDetay = this,
                    GunlukImalatItem = GunlukImalatItem,
                    Kullanici = Kullanici

                }.Show();
            }


            //  Do stuff
        }

    }
}
