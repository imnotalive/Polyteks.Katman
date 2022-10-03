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
    public partial class FrmGunlukImalatDetayDuzenle : Form
    {

        public void AnalizDegerGuncellemeYap(int satir, int sutun, int sayfa, string deger)
        {
            bool SorunVarMi = false;
            var liste = new List<DropItem>();
            var hucreDetayItem = TumHucreler.First(a => a.SayfaNo == sayfa && a.SatirId == satir && a.SutunId == sutun);
            if (satir <= 4)
            {
                var editLabItem = _dbPoly.SrcnLabAnalizItems.Find(hucreDetayItem.LabAnalizItem.LabAnalizItemId);
                //0 iş emri
                //1 parti No
                //2 makine
                //3 kanal
                //4 takım saati
                //5 pozisyon
                if (satir == 0)
                {
                    if (deger != null)
                    {
                        if (_dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.Any(a => a.RefakatNo.Trim() == deger.Trim()))
                        {
                            var TakipIzlenebilirlik =
                                _dbPoly.ZzzSrcnPartiSonuTakipIzlenebilirlik.First(a =>
                                    a.RefakatNo == deger.Trim());

                            editLabItem.RefakartKartNo = deger;
                            editLabItem.MakineNo = TakipIzlenebilirlik.MakineNo.Trim();
                            editLabItem.PartiNo = TakipIzlenebilirlik.PartiNo.Trim();
                            _dbPoly.SaveChanges();


                            liste.Add(new DropItem() { IdInt = 0, Id = sutun.ToString(), Tanim = deger });
                            liste.Add(new DropItem() { IdInt = 1, Id = sutun.ToString(), Tanim = String.Format("{0} {1}",TakipIzlenebilirlik.PartiNo.Trim(), TakipIzlenebilirlik.MakineNo.Trim()) });
                            //liste.Add(new DropItem() { IdInt = 2, Id = sutun.ToString(), Tanim = TakipIzlenebilirlik.MakineNo.Trim() });

                        }
                        else
                        {
                            SorunVarMi = true;
                        }
                    }
                    else
                    {
                        SorunVarMi = true;
                    }
                }
                else
                {
                    if (satir == 2)
                    {
                        editLabItem.KanalNo = deger;
                 
                     
                    }
                    if (satir == 3)
                    {
                        editLabItem.TakimSaati = deger;
                        _dbPoly.SaveChanges();
     
                    }
                    if (satir == 4)
                    {
                        editLabItem.MakinePozisyon = deger;
                        _dbPoly.SaveChanges();
                      
                    }

                    liste.Add(new DropItem() { IdInt = satir, Id = sutun.ToString(), Tanim = deger });
                }

              

            }
            else
            {
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
            }

            if (SorunVarMi)
            {
                MessageBox.Show("Lütfen Girilen Bilgileri Kontrol Ediniz!!");
            }
            else
            {
                string dtIsim = "dt" + sayfa;
                foreach (Control cntrl in tabControl.TabPages[sayfa - 1].Controls)
                {
                    if (cntrl.Name == dtIsim)
                    {
                        foreach (var dropItem in liste)
                        {
                            int c = Convert.ToInt32(dropItem.Id);
                            int r = dropItem.IdInt;
                            string ydeger = dropItem.Tanim;
                            ((DataGridView)cntrl).Rows[r].Cells[c].Value = ydeger;
                        }

                    }

                }
                tabControl.SelectedIndex = sayfa - 1;
            }



        }
        public List<SrcnLabAnalizItems> GunlukImalatLabItemsGetir(SrcnGunlukImalatDosyas gunlukImalat)
        {
            var labItems = new List<SrcnLabAnalizItems>();
            if (!(_dbPoly.SrcnLabAnalizItems.Any(a => a.BagliOlduguDosyaId == gunlukImalat.GunlukImalatDosyaId && a.DosyaTipi == 1)))

            {
                int SayfaNo = 1;
                int BobinSay = -1;
                for (int i = 1; i <= gunlukImalat.ToplamBobinSayisi; i++)
                {
                    BobinSay++;
                    if (BobinSay == 6)
                    {
                        SayfaNo++;
                        BobinSay = 0;
                    }


                    _dbPoly.SrcnLabAnalizItems.Add(new SrcnLabAnalizItems()
                    {
                        LabAnalizId = 0,
                        DosyaTipi = 1,
                        DosyaTipiAdi = "Günlük İmalat Analizleri",
                        Aciklama = null,
                        BagliOlduguDosyaId = gunlukImalat.GunlukImalatDosyaId,
                        BobinAdi = i + ". Bobin",
                        IplikNo = i,
                        KayitTarihi = DateTime.Now,
                        SayfaNo = SayfaNo

                    });
                    _dbPoly.SaveChanges();
                }
            }

            labItems = _dbPoly.SrcnLabAnalizItems
                .Where(a => a.BagliOlduguDosyaId == gunlukImalat.GunlukImalatDosyaId && a.DosyaTipi == 1)
                .OrderBy(a => a.SayfaNo).ThenBy(a => a.IplikNo).ToList();

            return labItems;
        }
        public FrmGunlukImalatDetayDuzenle()
        {
            InitializeComponent();
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public int GunlukImalatDosyaId { get; set; }
        public SrcnKullanicis Kullanici { get; set; }
        public List<HucreDetayItem> TumHucreler { get; set; }
        public SrcnGunlukImalatDosyas GunlukImalatDosya { get; set; }
        public string Yazi { get; set; }
        public string HafizayaKopya { get; set; }
        public void TumHucreleriGuncelle()
        {
            //tabControl.TabPages.Clear();
            GunlukImalatDosya = _dbPoly.SrcnGunlukImalatDosyas.Find(GunlukImalatDosyaId);
            var labItems = GunlukImalatLabItemsGetir(GunlukImalatDosya);
            TumHucreler = new List<HucreDetayItem>();
            var sayfalar = labItems.Select(a => a.SayfaNo).Distinct().ToList();

            var labItemIdler = labItems.Select(a => a.LabAnalizItemId).ToList();

            var TumAnalizCesitSonuclari = _dbPoly.SrcnLabAnalizItemAnalizCesitSonucs
                .Where(a => labItemIdler.Any(b => b == a.LabAnalizItemId)).ToList();

            var labAnalizCesitleri = _dbPoly.SrcnLabAnalizCesits.AsNoTracking().Where(a => a.MalzemeTipi == 0 && a.LabAnalizCesitId != 40 && a.LabAnalizCesitId != 41 && a.Sira > 0).OrderBy(a => a.Sira).ToList();

            foreach (var s in sayfalar)
            {
                #region tabPage Oluşturma
                int i = 50;
                int red = 255;
                int blue = 0;
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
                    Name = "tab" + s,
                    Text = s.ToString() + ". Sayfa",
                    Width = this.Size.Width,
                    Height = this.Size.Height,
                    BackColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))

                };
                #endregion

                var SayfalabItems = labItems.Where(a => a.SayfaNo == s).ToList();

                var sayfaLabAnalizItemSonuclar = new List<SrcnLabAnalizItemAnalizCesitSonucs>();
                foreach (var aa in SayfalabItems)
                {
                    sayfaLabAnalizItemSonuclar.AddRange(TumAnalizCesitSonuclari
                        .Where(a => a.LabAnalizItemId == aa.LabAnalizItemId).OrderBy(a => a.LabAnalizCesitAdi)
                        .ToList());
                }

                var dtgrid = new DataGridView();
                dtgrid.Name = "dt" + s;
                dtgrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dtgrid.ColumnHeadersHeight = 28;
                dtgrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgrid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgrid.RowsDefaultCellStyle.Font = new Font("Verdana", 9, FontStyle.Bold);
                dtgrid.Location = new Point(5, 20);
                dtgrid.AllowUserToOrderColumns = false;
                
                dtgrid.CellClick += new DataGridViewCellEventHandler(dtgrid_CellClick);

                var Headerlar = SayfalabItems.GroupBy(a => a.BobinAdi).ToList();
                dtgrid.ColumnCount = Headerlar.Count() + 1;
                dtgrid.RowCount = labAnalizCesitleri.Count() + 5;
                dtgrid.Columns[0].HeaderText = "ANALİZ";
                int ii = 1;
                foreach (var j in Headerlar)
                {
                    dtgrid.Columns[ii].HeaderText = j.Key.ToString();
                    ii++;
                }

                int Satir = 5;
                int Sutun = 0;


                dtgrid.Rows[0].Cells[0].Value = "İŞ EMRİ";
                dtgrid.Rows[1].Cells[0].Value = "PARTİ MAKİNE";
                dtgrid.Rows[2].Cells[0].Value = "KANAL";
                dtgrid.Rows[3].Cells[0].Value = "TAKIM SAATİ";
                dtgrid.Rows[4].Cells[0].Value = "POZİSYON";
     

                /// İLK 4 SATIR İÇİN
                foreach (var c in SayfalabItems)
                {
                    Sutun++;

                    TumHucreler.Add(new HucreDetayItem()
                    {
                        SayfaNo = Convert.ToInt32(s),
                        SatirId = 0,
                        SutunId = Sutun,
                        LabAnalizItem = c

                    });
                    TumHucreler.Add(new HucreDetayItem()
                    {
                        SayfaNo = Convert.ToInt32(s),
                        SatirId = 1,
                        SutunId = Sutun,
                        LabAnalizItem = c

                    });
                    TumHucreler.Add(new HucreDetayItem()
                    {
                        SayfaNo = Convert.ToInt32(s),
                        SatirId = 2,
                        SutunId = Sutun,
                        LabAnalizItem = c

                    });
                    TumHucreler.Add(new HucreDetayItem()
                    {
                        SayfaNo = Convert.ToInt32(s),
                        SatirId = 3,
                        SutunId = Sutun,
                        LabAnalizItem = c

                    });
                    TumHucreler.Add(new HucreDetayItem()
                    {
                        SayfaNo = Convert.ToInt32(s),
                        SatirId = 4,
                        SutunId = Sutun,
                        LabAnalizItem = c

                    });
                    //TumHucreler.Add(new HucreDetayItem()
                    //{
                    //    SayfaNo = Convert.ToInt32(s),
                    //    SatirId = 5,
                    //    SutunId = Sutun,
                    //    LabAnalizItem = c

                    //});
                    if (c.RefakartKartNo != null)
                    {
                        dtgrid.Rows[0].Cells[Sutun].Value = c.RefakartKartNo.Trim();
                        dtgrid.Rows[1].Cells[Sutun].Value = String.Format("{0} {1}", c.PartiNo, c.MakineNo);
                    }
                    dtgrid.Rows[2].Cells[Sutun].Value = c.KanalNo;
                    dtgrid.Rows[3].Cells[Sutun].Value = c.MakinePozisyon;
                    dtgrid.Rows[4].Cells[Sutun].Value = c.TakimSaati;

                    dtgrid.Rows[0].Cells[Sutun].Style.ForeColor = Color.DarkGreen;

                    dtgrid.Rows[1].Cells[Sutun].Style.Font = new Font("Verdana", 7, FontStyle.Bold);
                    dtgrid.Rows[1].Cells[Sutun].Style.ForeColor = Color.Red;

                    dtgrid.Rows[2].Cells[Sutun].Style.ForeColor = Color.Blue;


                }


                Sutun = 0;

                foreach (var aa in labAnalizCesitleri)
                {
                    dtgrid.Rows[Satir].Cells[0].Value = aa.LabAnalizCesitAdi;
                    foreach (var c in SayfalabItems)
                    {
                        Sutun++;
                        var analizCesitSonuc = new SrcnLabAnalizItemAnalizCesitSonucs();

                        var hucreDetayItem = new HucreDetayItem();




                        if (sayfaLabAnalizItemSonuclar.Any(a => a.LabAnalizItemId == c.LabAnalizItemId && a.LabAnalizCesitId == aa.LabAnalizCesitId))
                        {
                            analizCesitSonuc = sayfaLabAnalizItemSonuclar.First(a =>
                                a.LabAnalizItemId == c.LabAnalizItemId && a.LabAnalizCesitId == aa.LabAnalizCesitId);
                        }
                        else
                        {
                            analizCesitSonuc.LabAnalizItemId = c.LabAnalizItemId;
                            analizCesitSonuc.LabAnalizCesitId = aa.LabAnalizCesitId;
                        }
                        hucreDetayItem.SayfaNo = Convert.ToInt32(s);
                        hucreDetayItem.AnalizItemCesitSonuc = analizCesitSonuc;
                        hucreDetayItem.LabAnalizItem = c;
                        hucreDetayItem.SatirId = Satir;
                        hucreDetayItem.SutunId = Sutun;
                        TumHucreler.Add(hucreDetayItem);

                        dtgrid.Rows[Satir].Cells[Sutun].Value = analizCesitSonuc.AnalizDegeriString;
                    }

                    Sutun = 0;

                    Satir++;
                }


                foreach (DataGridViewColumn column in dtgrid.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                foreach (DataGridViewRow x in dtgrid.Rows)
                {
                    x.MinimumHeight = 38;
                }
                dtgrid.Height = 1105;
                dtgrid.Width = 780;
                var btn = new Button
                {
                    Location = new Point(5, 1140),
                    Name = "btn" + s,
                    Width = 780,
                    Height = 40,
                    Text = "ANALİZLERİN TÜMÜ YAPILDI OLARAK İŞARETLE",
                    BackColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)),
                    Font = new Font("Verdana", 13, FontStyle.Bold)
            };
                btn.Click += new EventHandler(button_Click);
                var label = new Label
                {
                    Location = new Point(1, 5),
                    Name = "lbl" + s,
                    ForeColor = Color.White,
                };
                dtgrid.Columns[0].Width = 140;
                dtgrid.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dtgrid.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                label.AutoSize = false;
                label.Width = 800;
                label.Height = 15;
                var lblText = Kullanici.IsimSoyisim.Trim().ToUpper();
                label.Text = lblText;
                yeniTabPage.Controls.Add(label);
                yeniTabPage.Controls.Add(dtgrid);
                yeniTabPage.Controls.Add(btn);
                tabControl.TabPages.Add(yeniTabPage);
            }
        }
        private void FrmGunlukImalatDetayDuzenle_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            TumHucreleriGuncelle();



        }


        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            DialogResult cikis = new DialogResult();
            cikis = MessageBox.Show("Tüm bilgileri kontrol edip kaydedilmesini onaylıyır musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (cikis == DialogResult.Yes)
            {
                var labItems = _dbPoly.SrcnLabAnalizItems.Where(a =>
                    a.DosyaTipi == 1 && a.BagliOlduguDosyaId == GunlukImalatDosya.GunlukImalatDosyaId).ToList();

                if (labItems.Any(a => a.RefakartKartNo == null))
                {
                    MessageBox.Show("İş Emri Belirlenmemiş Bobinler Bulunmaktadır Kontrol Ediniz");
                }
                else
                {
                    //MessageBox.Show("İşleminiz yapılıyor...");

                    var labIdler = labItems.Select(a => a.LabAnalizId).Distinct().ToList();
                    var LabAnalizler = _dbPoly.SrcnLabAnalizs.Where(a =>a.LabAnalizId != 0 && labIdler.Any(b => b == a.LabAnalizId)).ToList();
                    foreach (var item in labItems)
                    {
                        if (!(LabAnalizler.Any(a => a.LabAnalizId == item.LabAnalizId && a.RefakartKartNo == item.RefakartKartNo && a.PartiNo == item.PartiNo)))
                        {
                            // güncellenecek değişti item
                            if (_dbPoly.SrcnLabAnalizs.Any(a => a.RefakartKartNo == item.RefakartKartNo && a.PartiNo == item.PartiNo && a.DosyaTipi == 1))
                            {
                                // bu analiz var
                                item.LabAnalizId = _dbPoly.SrcnLabAnalizs.First(a =>
                                    a.RefakartKartNo == item.RefakartKartNo && a.PartiNo == item.PartiNo && a.DosyaTipi==1).LabAnalizId;
                                _dbPoly.SaveChanges();
                            }
                            else
                            {
                                int MakineId = 0;
                                if (item.MakineId != null)
                                {
                                    MakineId = Convert.ToInt32(item.MakineId);
                                }
                                var yeniAnaliz = new SrcnLabAnalizs() { DosyaTipi = 1, BagliOlduguDosyaId = GunlukImalatDosyaId, AnalizAdi = "GÜNLÜK İMALAT ANALİZ", IsEmriNumarasiDurumu = 1, KayitTarihi = DateTime.Now, LabAnalizDurumu = 3, MakineId = MakineId, PartiNo = item.PartiNo, RefakartKartNo = item.RefakartKartNo, AnalizBaslangicSaati = GunlukImalatDosya.KayitTarihi, AnalizBitisSaati = DateTime.Now, IstenenTerminTarihi = DateTime.Now, AnalizTipi = 1, KayitYapanKulAdi = Kullanici.IsimSoyisim, KayitYapanKulKodu = Kullanici.KullaniciId.ToString(), LabGrupAdi = "Günlük İmalat"};
                                _dbPoly.SrcnLabAnalizs.Add(yeniAnaliz);
                                _dbPoly.SaveChanges();
                                item.LabAnalizId = yeniAnaliz.LabAnalizId;
                                _dbPoly.SaveChanges();

                            }
                        }


                    }
                    var guncelLabIdler = _dbPoly.SrcnLabAnalizItems.Where(a =>
                        a.DosyaTipi == 1 && a.BagliOlduguDosyaId == GunlukImalatDosya.GunlukImalatDosyaId).Select(a => a.LabAnalizId).ToList();
                    var KayitliLabAnalizler = _dbPoly.SrcnLabAnalizs.Where(a => a.DosyaTipi == 1 && a.BagliOlduguDosyaId == GunlukImalatDosyaId)
                        .ToList();
                    var silincekLabAnalizler = new List<SrcnLabAnalizs>();
                    foreach (var item in KayitliLabAnalizler)
                    {
                        if (guncelLabIdler.Count(a => a == item.LabAnalizId) == 0)
                        {
                            silincekLabAnalizler.Add(item);
                        }
                    }
                    if (silincekLabAnalizler.Any())
                    {
                        _dbPoly.SrcnLabAnalizs.RemoveRange(silincekLabAnalizler);
                        _dbPoly.SaveChanges();
                    }

                    var durum = _dbPoly.SrcnDosyaDurums.Find(13);
                    var gunlukItem = _dbPoly.SrcnGunlukImalatDosyas.Find(GunlukImalatDosyaId);
                    gunlukItem.DosyaDurumId = durum.DosyaDurumId;
                    gunlukItem.DosyaDurumAdi = durum.DosyaDurumAdi;
                    _dbPoly.SaveChanges();
                    MessageBox.Show("Güncelleme işlemi yapılmıştır");
                    this.Close();
                }

               
              

            }


        }

        private void dtgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;



            int SayfaNo = Convert.ToInt32(grid.Name.Replace("dt", ""));
            int sutun = grid.CurrentCell.ColumnIndex;
            int satir = grid.CurrentRow.Index;

            //if (satir % 2 == 0)
            //{
            //    tabControl.SelectedIndex = 1;
            //}
            //else
            //{
            //    tabControl.SelectedIndex = 2;
            //}
            if (satir!=1 )
            {
                // makine otomatik Yazılcak



                if (sutun > 0)
                {
                    string guncelDeger = null;
                    if (grid.Rows[satir].Cells[sutun].Value != null)
                    {
                        guncelDeger = grid.Rows[satir].Cells[sutun].Value.ToString();

                    }

                 
                    new FrmNumPad()
                    {

                        YapilacakIslem = 1,
                        SayfaDegeri = SayfaNo,
                        SatirDegeri = satir,
                        SutunDegeri = sutun,
                        Yazi = guncelDeger,
                        FrmGunlukImalatDetayDuzenle = this,

                        Kullanici = Kullanici

                    }.Show();

                }
            }


        }

        private void FrmGunlukImalatDetayDuzenle_FormClosing(object sender, FormClosingEventArgs e)
        {
            new FrmGunlukImalatListe()
            {
                Kullanici = Kullanici
            }.Show();
        }
    }
}
