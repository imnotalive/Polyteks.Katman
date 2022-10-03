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
using Polyteks.Katman.Entities;
using Polyteks.Katman.LaborantGIAS.Models;

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmDenemeNumuneDetayDuzenle : Form
    {
        public void AnalizDegerGuncellemeYap(int satir, int sutun,  string deger)
        {
            bool SorunVarMi = false;
            var liste = new List<DropItem>();
            var hucreDetayItem = TumHucreler.First(a => a.SatirId == satir && a.SutunId == sutun);
            // analizSonuc
            var analizSonuc = hucreDetayItem.AnalizItemCesitSonuc;
            bool eklenecekMi = false;
            if (analizSonuc.LabAnalizItemAnalizCesitId != 0)
            {
                if (_dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Any(a => a.LabAnalizItemId == hucreDetayItem.LabAnalizItem.LabAnalizItemId && a.LabAnalizCesitId == analizSonuc.LabAnalizCesitId))
                {
                    var editSonuc = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.First(a =>
                        a.LabAnalizItemId == hucreDetayItem.LabAnalizItem.LabAnalizItemId &&
                        a.LabAnalizCesitId == analizSonuc.LabAnalizCesitId);
                    editSonuc.AnalizDegeriString = deger;
                    _dbPoly.SaveChanges();
                    analizSonuc = editSonuc;
                }
                else
                {
                    eklenecekMi = true;
                }
            }
            else
            {
                eklenecekMi = true;
            }

            if (eklenecekMi)
            {
                var analizCesit = _dbPoly.SrcnLabAnalizCesits.Find(analizSonuc.LabAnalizCesitId);

                _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs.Add(new SrcnLabAnalizItemAnalizCesitSonucs
                {
                    AnalizDegeriString = deger,
                    LabAnalizCesitId = analizCesit.LabAnalizCesitId,
                    LabAnalizItemId = hucreDetayItem.LabAnalizItem.LabAnalizItemId,
                    LabAnalizCesitAdi = analizCesit.LabAnalizCesitAdi,
                    LabAnalizCesitAdiEng = analizCesit.LabAnalizCesitAdiEng,

                });
                _dbPoly.SaveChanges();
            }


            liste.Add(new DropItem() { IdInt = satir, Id = sutun.ToString(), Tanim = deger });



            foreach (var dropItem in liste)
            {
                int c = Convert.ToInt32(dropItem.Id);
                int r = dropItem.IdInt;
                string ydeger = dropItem.Tanim;
                dtDenemeNumune.Rows[r].Cells[c].Value = ydeger;
            }

        }
        public FrmDenemeNumuneDetayDuzenle()
        {
            InitializeComponent();
        }
        public void TumHucreleriGuncelle()
        {
            //tabControl.TabPages.Clear();
            
           TumHucreler = new List<HucreDetayItem>();
           

            var labItemlar = _dbPoly.SrcnLabAnalizItems.Where(a => a.LabAnalizId==LabAnalizId).ToList();

            var labItemIdler = labItemlar.Select(a => a.LabAnalizItemId).ToList();
            var TumAnalizCesitSonuclari = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                .Where(a => labItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();
            var distLabAnalizCesitIdler = TumAnalizCesitSonuclari.Select(a => a.LabAnalizCesitId).Distinct().ToList();
            var labAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => distLabAnalizCesitIdler.Any(b => b == a.LabAnalizCesitId)).OrderBy(a => a.Sira).ToList();




            dtDenemeNumune.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtDenemeNumune.ColumnHeadersHeight = 28;
            dtDenemeNumune.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtDenemeNumune.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtDenemeNumune.RowsDefaultCellStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
            dtDenemeNumune.AllowUserToOrderColumns = false;

            dtDenemeNumune.CellClick += new DataGridViewCellEventHandler(dtgrid_CellClick);

                var Headerlar = labItemlar.GroupBy(a => a.BobinAdi).ToList();
                dtDenemeNumune.ColumnCount = Headerlar.Count() + 1;
                dtDenemeNumune.RowCount = labAnalizCesitleri.Count();
                dtDenemeNumune.Columns[0].HeaderText = "ANALİZ";
                int ii = 1;
                foreach (var j in Headerlar)
                {
                    dtDenemeNumune.Columns[ii].HeaderText = j.Key.ToString();
                    ii++;
                }

                int Satir = 0;
                int Sutun = 0;




                Sutun = 0;

                foreach (var aa in labAnalizCesitleri)
                {
                    dtDenemeNumune.Rows[Satir].Cells[0].Value = aa.LabAnalizCesitAdi;
                    foreach (var c in labItemlar)
                    {
                        Sutun++;
                        var analizCesitSonuc = new SrcnLabAnalizItemAnalizCesitSonucs();

                        var hucreDetayItem = new HucreDetayItem();




                        if (TumAnalizCesitSonuclari.Any(a => a.LabAnalizItemId == c.LabAnalizItemId && a.LabAnalizCesitId == aa.LabAnalizCesitId))
                        {
                            analizCesitSonuc = TumAnalizCesitSonuclari.First(a =>
                                a.LabAnalizItemId == c.LabAnalizItemId && a.LabAnalizCesitId == aa.LabAnalizCesitId);
                        }
                        else
                        {
                            analizCesitSonuc.LabAnalizItemId = c.LabAnalizItemId;
                            analizCesitSonuc.LabAnalizCesitId = aa.LabAnalizCesitId;
                        }
                    
                        hucreDetayItem.AnalizItemCesitSonuc = analizCesitSonuc;
                        hucreDetayItem.LabAnalizItem = c;
                        hucreDetayItem.SatirId = Satir;
                        hucreDetayItem.SutunId = Sutun;
                        TumHucreler.Add(hucreDetayItem);

                        dtDenemeNumune.Rows[Satir].Cells[Sutun].Value = analizCesitSonuc.AnalizDegeriString;
                    }

                    Sutun = 0;

                    Satir++;
                }


                foreach (DataGridViewColumn column in dtDenemeNumune.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                foreach (DataGridViewRow x in dtDenemeNumune.Rows)
                {
                    x.MinimumHeight = 38;
                }



                dtDenemeNumune.Columns[0].Width = 140;
                dtDenemeNumune.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
               
             
                
            
        }
        public void labelOlustur()
        {
            var labelListe = new List<Label>();

            labelListe.Add(new Label
            {
                Location = new Point(1, 25),
                Name = "lbl",
                ForeColor = Color.Black,
                AutoSize = true,
                Text = "Deneme Kodu:",

            });
            labelListe.Add(new Label
            {
                Location = new Point(150, 25),
                Name = "lbl",
                ForeColor = Color.Red,
                AutoSize = true,
                Text = string.Format("{0}/{1}", DenemeDosyaItem.DenemeAdi, DenemeDosyaItem.Sira)

            });

            labelListe.Add(new Label
            {
                Location = new Point(550, 25),
                Name = "lbl",
                ForeColor = Color.Black,
                AutoSize = true,
                Text = "Birim:",

            });
            labelListe.Add(new Label
            {
                Location = new Point(640, 25),
                Name = "lbl",
                ForeColor = Color.Red,
                AutoSize = true,
                Text = string.Format("{0}", LabAnaliz.BirimAdi)

            });
            labelListe.Add(new Label
            {
                Location = new Point(550, 60),
                Name = "lbl",
                ForeColor = Color.Black,
                AutoSize = true,
                Text = "Makine:",

            });
            labelListe.Add(new Label
            {
                Location = new Point(640, 60),
                Name = "lbl",
                ForeColor = Color.Red,
                AutoSize = true,
                Text = string.Format("{0}", DenemeDosyaItem.MakineNo.Trim())

            });

            labelListe.Add(new Label
            {
                Location = new Point(1, 60),
                Name = "lbl",
                ForeColor = Color.Black,
                AutoSize = true,
                Text = "Cari:",

            });
            labelListe.Add(new Label
            {
                Location = new Point(150, 60),
                Name = "lbl",
                ForeColor = Color.Red,
                AutoSize = true,
                Text = string.Format("{0}", LabAnaliz.CariAdi.Trim())

            });

            foreach (var label in labelListe)
            {
                grpGenelBilgiler.Controls.Add(label);
            }
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public int LabAnalizId { get; set; }
        public SrcnKullanicis Kullanici { get; set; }
        public SrcnDenemeDosyaItems DenemeDosyaItem { get; set; }
        public SrcnLabAnalizs LabAnaliz { get; set; }
        public List<HucreDetayItem> TumHucreler { get; set; }
        private void FrmDenemeNumuneDetayDuzenle_Load(object sender, EventArgs e)
        {
           // LabAnalizId = 288;
            this.WindowState = FormWindowState.Maximized;
            DenemeDosyaItem = new SrcnDenemeDosyaItems();
            LabAnaliz = new SrcnLabAnalizs();
            LabAnaliz = _dbPoly.SrcnLabAnalizs.Find(LabAnalizId);
            DenemeDosyaItem = _dbPoly.SrcnDenemeDosyaItems.Find(LabAnaliz.BagliOlduguDosyaId);

            labelOlustur();

            TumHucreleriGuncelle();

        }

        private void dtgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;



            int SayfaNo = 0;
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

                        YapilacakIslem = 4,
                      
                        SatirDegeri = satir,
                        SutunDegeri = sutun,
                        Yazi = guncelDeger,
                        FrmDenemeNumuneDetayDuzenle = this,

                        Kullanici = Kullanici

                    }.Show();

                }
            


        }

        private void FrmDenemeNumuneDetayDuzenle_FormClosing(object sender, FormClosingEventArgs e)
        {
            new FrmDenemeNumuneListe()
            {
                Kullanici = Kullanici
            }.Show();
        }
    }
}
