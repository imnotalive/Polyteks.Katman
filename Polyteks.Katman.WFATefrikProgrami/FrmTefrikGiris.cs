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
using Polyteks.Katman.WFATefrikProgrami.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Diagnostics;

namespace Polyteks.Katman.WFATefrikProgrami
{
    public partial class FrmTefrikGiris : Form
    {
        public FrmTefrikGiris()
        {
            InitializeComponent();
        }
        #region Genel Tanımlamalar
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        public Personel Personel { get; set; }
        public ZzzSrcnTefrikRefakatFisListe RefakatFisModel { get; set; }
        public SrcnPartiAnaKlasors PartiAnaKlasor { get; set; }
        public ZzzSrcnMakineIdli MakineIdli { get; set; }
        public List<HataliUretimModel> HataliUretimModeller { get; set; }

        public List<HataliUretimModel> HataliUretimOnTefrikModeller { get; set; }
        #endregion
        #region Metotlar
        #region Liste Doldurma
        public void SabitListeleriDoldur()
        {
           
            lstHatalar.Items.Clear();
            lstPozisyonlar.Items.Clear();
            lstOnTefrikHatalar.Items.Clear();
            lstOnTefrikPozisyonlar.Items.Clear();
            var liste = _dbPoly.UretimHata/*.Where(a=>a.HataNedeni=="KK"||a.HataNedeni=="ATLAMA"||a.HataNedeni=="ÇAP"||a.HataNedeni=="TAK"||a.HataNedeni=="SURG"||a.HataNedeni== "REZERVESIZ" || a.HataNedeni=="LEKELİ").OrderBy(a => a.HataNedeni).*/.ToList();
            foreach (var uretimHata in liste)
            {
                lstHatalar.Items.Add(uretimHata.HataNedeni);
                lstOnTefrikHatalar.Items.Add(uretimHata.HataNedeni);
            }
            for (int i = 0; i < 350; i++)
            {
                lstPozisyonlar.Items.Add(i.ToString());
                lstOnTefrikPozisyonlar.Items.Add(i.ToString());

            }


        }

        public void HatalariDoldur()
        {
            lstHataliUretimler.DataSource = HataliUretimModeller.OrderBy(a => a.Pozisyon.IdInt).ToList();
            lstHataliUretimler.DisplayMember = "HataNedeni";
            lstOnTefrikHataliUretimler.DataSource = HataliUretimOnTefrikModeller.OrderBy(a => a.Pozisyon.IdInt).ToList();
            lstOnTefrikHataliUretimler.DisplayMember = "HataNedeni";
            SabitListeleriDoldur();
        }
        #endregion


        public void OnTefrikKaydet()
        {

            int ToplamBobinSayisi = Convert.ToInt32(txtOnTefToplamBobin.Text);
            int KisaBobinSayisi = Convert.ToInt32(txtOnTefKisaBobin.Text);

            string refekatKartNo = txtIsEmri.Text;
            string PartiNo = PartiAnaKlasor.PartiAdi;
            string makineNo = MakineIdli.MakineNo;
            int BirimId = Convert.ToInt32(MakineIdli.BirimId);
            string BirimAdi = MakineIdli.BirimAdi;
            var OntefrikGrup = new SrcnOntefrikGrups
            {
                KisaBobinSayisi = KisaBobinSayisi,
                PartiNo = PartiNo,
                PersonelAdi = Personel.PersonelAdi,
                PersonelNo = Personel.PersonelNo,
                Tarih = DateTime.Now,
                ToplamBobinSayisi = ToplamBobinSayisi
            };
            _dbPoly.SrcnOntefrikGrups.Add(OntefrikGrup);
            _dbPoly.SaveChanges();
            try
            {



                int hSira = 0;
                foreach (var hataliUretimModel in HataliUretimOnTefrikModeller)
                {
                    foreach (var item in hataliUretimModel.HataliUretimHareketler)
                    {
                        hSira++;

                        int pozisyon = hataliUretimModel.Pozisyon.IdInt;
                        string hataNedeni = item.HataNedeni;

                        var yeniOntefrik = new SrcnOntefriks
                        {
                            BirimAdi = BirimAdi,
                            BirimId = BirimId,
                            HataNedeni = hataNedeni,
                            KisaBobin = KisaBobinSayisi,
                            MakineNo = makineNo,
                            OntefrikGrupId = OntefrikGrup.OntefrikGrupId,
                            PartiNo = PartiNo,
                            Pozisyon = pozisyon,
                            Tarih = DateTime.Now,
                            ToplamBobin = ToplamBobinSayisi
                        };
                        _dbPoly.SrcnOntefriks.Add(yeniOntefrik);
                        _dbPoly.SaveChanges();

                    }
                }
                MessageBox.Show("Kayıt Yapılmıştır.  \n Parti No:" + PartiNo, "Kayıt Yapıldı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                HataliUretimOnTefrikModeller = new List<HataliUretimModel>();
                HatalariDoldur();

                new FrmTefrikGiris()
                {
                    Personel = Personel
                }.Show();

                this.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Bilinmeyen Bir Hata Oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public void TefrikKaydet()
        {

            Decimal Miktar1 = Convert.ToDecimal(txtToplamBobin.Text);
            Decimal Miktar2 = Convert.ToDecimal(txtKisaBobin.Text);
           string Miktar3 = txtCorapArabaNo.Text;




            string refekatKartNo = txtIsEmri.Text;

            try
            {

                #region Üretim Hareket update-create
                var updateUretimHareket = _dbPoly.UretimHareket.First(a => a.RefakatKartNo.Replace(" ", "") == refekatKartNo && a.SiraNo == 300 && a.UretimBitisTarihi == null);

                updateUretimHareket.CalisanPersonel1 = Personel.PersonelNo;
                updateUretimHareket.CalisanPersonel2 = Personel.PersonelNo;
                updateUretimHareket.CalisanPersonel3 = Personel.PersonelNo;

                updateUretimHareket.UretimBitisTarihi = DateTime.Now;
                updateUretimHareket.Miktar1 = Miktar1;
                updateUretimHareket.Miktar2 = Miktar2;
                updateUretimHareket.IlaveReceteNo2 = Miktar3;
              
                updateUretimHareket.IlaveReceteNo3 = "srcn-tablet";
                _dbPoly.SaveChanges();
                var YeniUretimHareket = new UretimHareket()
                {
                    CalisanPersonel1 = Personel.PersonelNo,
                    CalisanPersonel2 = Personel.PersonelNo,
                    CalisanPersonel3 = Personel.PersonelNo,
                    RefakatKartNo = updateUretimHareket.RefakatKartNo,
                    SiraNo = updateUretimHareket.SiraNo,
                    UretimSiraNo = updateUretimHareket.UretimSiraNo + 1,
                    SiparisNo = updateUretimHareket.SiparisNo,
                    SiparisSiraNo = updateUretimHareket.SiparisSiraNo,
                    PartiNo = updateUretimHareket.PartiNo,
                    UretimBaslamaTarihi = DateTime.Now,
                    MakineNo = "TEFRIK",
                    UretimBitisTarihi = null,
               IlaveReceteNo2=Miktar3,
                    IlaveReceteNo3 = "srcn-tablet"
                };
                _dbPoly.UretimHareket.Add(YeniUretimHareket);
                _dbPoly.SaveChanges();
                #endregion

                int hSira = 0;
                foreach (var hataliUretimModel in HataliUretimModeller)
                {
                    foreach (var item in hataliUretimModel.HataliUretimHareketler)
                    {
                        hSira++;


                        var hataliUretimHareket = new HataliUretimHareket()
                        {
                            SiraNo = 300,
                            RefakatKartNo = updateUretimHareket.RefakatKartNo,
                            UretimSiraNo = updateUretimHareket.UretimSiraNo,
                            HataNedeni = item.HataNedeni,
                            HataMiktari = hataliUretimModel.Pozisyon.IdInt,
                            HSiraNo = hSira
                        };
                        _dbPoly.HataliUretimHareket.Add(hataliUretimHareket);

                        _dbPoly.SaveChanges();

                        if (_dbPoly.ZzzSrcnHataliTefrikMakineleri.Any(a => a.UretimSiraNo == hataliUretimHareket.UretimSiraNo &&
                                                                                        a.HSiraNo == hataliUretimHareket.HSiraNo &&
                                                                                        a.HataNedeni == hataliUretimHareket.HataNedeni &&
                                                                                        a.SiraNo == 300 &&
                                                                                       a.RefakatKartNo == hataliUretimHareket.RefakatKartNo))
                        {
                            var HataliTefrikMakine = _dbPoly.ZzzSrcnHataliTefrikMakineleri.AsNoTracking().First(a =>
                                a.UretimSiraNo == hataliUretimHareket.UretimSiraNo &&
                                a.HSiraNo == hataliUretimHareket.HSiraNo &&
                                a.HataNedeni == hataliUretimHareket.HataNedeni &&
                                a.SiraNo == 300 &&
                                a.RefakatKartNo == hataliUretimHareket.RefakatKartNo);

                            int pozisyon = Convert.ToInt32(hataliUretimHareket.HataMiktari);

                            var stokBilgi = new ZzzSrcnRefakatKartNoStokAdlari();
                            if (_dbPoly.ZzzSrcnRefakatKartNoStokAdlari.Any(a =>
                                a.RefakatKartNo == hataliUretimHareket.RefakatKartNo.Replace(" ", "")))
                            {
                                stokBilgi = _dbPoly.ZzzSrcnRefakatKartNoStokAdlari.AsNoTracking().First(a =>
                                   a.RefakatKartNo == hataliUretimHareket.RefakatKartNo.Replace(" ", ""));
                            }

                            var BildirimDurumuId = -1;
                            var BildirimDurumuAdi = "Müdahale Edilemez";
                            if (MudahaleEdilebilirMi(hataliUretimHareket.HataNedeni.ToString()))
                            {
                                if (pozisyon != 0)
                                {
                                    BildirimDurumuId = 0;
                                    BildirimDurumuAdi = "Müdahale Bekleniyor";
                                }
                            }
                            var MakineHataBildirim = new SrcnMakineHataBildirims
                            {
                                BildirimDurumuId = BildirimDurumuId,
                                BildirimDurumuAdi = BildirimDurumuAdi,
                                
                                KayitTarihi = DateTime.Now,
                                SonGuncellemeYapanPersonelAdi = Personel.PersonelAdi.ToString(),
                                SonGuncellemeYapanPersonelNo = Personel.PersonelNo.ToString(),
                                MakineId = (int)HataliTefrikMakine.SayacOndalikSayisi,
                                MakinePozisyonNo = pozisyon,
                                UretimSiraNo = hataliUretimHareket.UretimSiraNo,
                                RefakatKartNo = hataliUretimHareket.RefakatKartNo.ToString(),
                                HataNedeni = hataliUretimHareket.HataNedeni.ToString(),
                                HataSiraNo = hataliUretimHareket.HSiraNo,
                                PartiNo = HataliTefrikMakine.PartiNo.ToString(),
                                SeciliMi = false,
                                StokAdi = stokBilgi.StokAdi.ToString(),
                                StokKodu = stokBilgi.StokKodu.ToString()

                            };


                            _dbPoly.SrcnMakineHataBildirims.Add(MakineHataBildirim);
                            _dbPoly.SaveChanges();
                            _dbPoly.SrcnMakineHataBildirimHarekets.Add(new SrcnMakineHataBildirimHarekets()
                            {
                                PersonelAdi = Personel.PersonelAdi,
                                MakineHataBildirimId = MakineHataBildirim.MakineHataBildirimId,
                                HareketAdi = "Hata Bildirimi Yapıldı",
                                PersonelId = Personel.PersonelNo,
                                Tarih = DateTime.Now,
                                HareketTipi = BildirimDurumuId
                            });
                            _dbPoly.SaveChanges();

                        }

                    }
                }

                string TefrikKod = string.Format("#{0}-{1}", YeniUretimHareket.RefakatKartNo, updateUretimHareket.UretimSiraNo);
                MessageBox.Show("Kayıt Yapılmıştır.  \n Kontrol Numaranız:" + TefrikKod.Replace(" ", ""), "Kayıt Yapıldı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                HataliUretimModeller = new List<HataliUretimModel>();
                HatalariDoldur();

                new FrmTefrikGiris()
                {
                    Personel = Personel
                }.Show();

                this.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("Bilinmeyen Bir Hata Oluştu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public void OnTefrikBilgiGoster()
        {
            grpOnTefrik.Visible = true;
            grpOntefrikalan.Visible = true;
            lblOnTefBirim.Text = PartiAnaKlasor.BirimAdi.ToUpper();
            lblOnTefParti.Text = PartiAnaKlasor.PartiAdi.ToUpper();
            lblOnTefMakine.Text = MakineIdli.MakineNo.Trim().ToUpper();
        }
        public bool MudahaleEdilebilirMi(string HataNedeni)
        {
            bool sonuc = HataNedeni.Replace(" ", "") != "BOYA500";
            if (HataNedeni.Replace(" ", "") == "BOYAAR")
            {
                sonuc = false;
            }
            if (HataNedeni.Replace(" ", "") == "BOYACZ")
            {
                sonuc = false;
            }
            if (HataNedeni.Replace(" ", "") == "BOYADOK")
            {
                sonuc = false;
            }
            if (HataNedeni.Replace(" ", "") == "BOYAKR")
            {
                sonuc = false;
            }
            if (HataNedeni.Replace(" ", "") == "GENLESME")
            {
                sonuc = false;
            }
            if (HataNedeni.Replace(" ", "") == "KIRIK")
            {
                sonuc = false;
            }
            if (HataNedeni.Replace(" ", "") == "USTER")
            {
                sonuc = false;
            }
            if (HataNedeni.Replace(" ", "") == "YAG")
            {
                sonuc = false;
            }
            return sonuc;
        }
        public void FormLoadAyarlariGetir()
        {
            WindowState = FormWindowState.Maximized;
            HataliUretimModeller = new List<HataliUretimModel>();
            HataliUretimOnTefrikModeller = new List<HataliUretimModel>();
            RefakatFisModel = new ZzzSrcnTefrikRefakatFisListe();
            PartiAnaKlasor = new SrcnPartiAnaKlasors();
            MakineIdli = new ZzzSrcnMakineIdli();
            lblIsimSoyisim.Text = Personel.PersonelAdi.ToUpper();
            lblMakineNo.Text = RefakatFisModel.MakineNo;
            grpoFisBilgileri.Visible = false;
            grpTefrikGirisi.Visible = false;
            btnUrunHareket.Visible = false;
            grpOnTefrik.Visible = false;
            grpOntefrikalan.Visible = false;

            var GunlukTalimatTipleri = _dbPoly.SrcnPaketlemeGunlukTalimatTipis
                .OrderBy(a => a.PaketlemeGunlukTalimatTipiAdi).ToList();
            lstPaketlemeGunlukTalimatTipleri.DataSource = GunlukTalimatTipleri.ToList();
            lstPaketlemeGunlukTalimatTipleri.DisplayMember = "PaketlemeGunlukTalimatTipiAdi";

            var sureOnce = DateTime.Now.AddHours(-2);
            var sonTefriklerSelect = _dbPoly.UretimHareket.Where(a => a.SiraNo == 300 && a.CalisanPersonel1 == Personel.PersonelNo && a.UretimBitisTarihi > sureOnce)
                .Select(a => new { a.RefakatKartNo, a.UretimSiraNo });

            var SonTefrikler = new List<DropItem>();
            var SonOnTefrikler = new List<DropItem>();


            foreach (var item in sonTefriklerSelect)
            {
                SonTefrikler.Add(new DropItem() { Tanim = item.RefakatKartNo.Trim() + "-" + item.UretimSiraNo, Id = item.RefakatKartNo, IdInt = item.UretimSiraNo });
            }

            var sonOnTefriklerSelect = _dbPoly.SrcnOntefrikGrups.Where(a => a.PersonelNo == Personel.PersonelNo && a.Tarih > sureOnce)
          .Select(a => new { a.PartiNo, a.Sira, a.OntefrikGrupId });

            foreach (var item in sonOnTefriklerSelect)
            {
                SonOnTefrikler.Add(new DropItem() { Tanim = item.PartiNo.Trim(), Id = item.OntefrikGrupId.ToString() });
            }

            lstSonTefrikler.DataSource = SonTefrikler;
            lstSonTefrikler.DisplayMember = "Tanim";

            lstSonOntefrikler.DataSource = SonOnTefrikler;
            lstSonOntefrikler.DisplayMember = "Tanim";
            SabitListeleriDoldur();
        }
        public void RefakatFisLabelDoldur(ZzzSrcnTefrikRefakatFisListe RefakatFisModel)
        {
            lblPartiNo.Text = RefakatFisModel.PartiNo;
            lblRefakatNo.Text = RefakatFisModel.RefakatNo;
            lblRenk.Text = RefakatFisModel.Renk;
            lblStokAdi.Text = RefakatFisModel.StokAdi;
            lblMakineNo.Text = RefakatFisModel.MakineNo;

        }
        #endregion

        private void FrmTefrikGiris_Load(object sender, EventArgs e)
        {
            FormLoadAyarlariGetir();
            txtPozisyonArama.Text = "POZİSYON ARAMAK İÇİN TIKLAYINIZ";
            txtPozisyonArama.ForeColor = Color.DarkRed;
            txtPozisyonSearch.Text = "POZİSYON ARAMAK İÇİN TIKLAYINIZ";
            txtPozisyonSearch.ForeColor = Color.DarkRed;

        }

        #region Talimatlar Bölümü
        private void lstTalimatTarihleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            var talimatTipi = (SrcnPaketlemeGunlukTalimatTipis)lstPaketlemeGunlukTalimatTipleri.SelectedItem;
            var talimat = (SrcnPaketlemeGunlukTalimats)lstTalimatTarihleri.SelectedItem;

            lblSeciliGun.Text = string.Format("Seçili Talimat Günü: {0} {1}", talimat.TalimatTarihi, talimatTipi.PaketlemeGunlukTalimatTipiAdi);
            lblSeciliGun.BackColor = Color.Red;
            var itemlar = _dbPoly.SrcnPaketlemeGunlukTalimatItems
               .Where(a => a.PaketlemeGunlukTalimatId == talimat.PaketlemeGunlukTalimatId)
               .OrderBy(a => a.PaketlemeGunlukTalimatItemId).Select(a => new { a.TalimatBaslik, a.TalimatIcerik }).OrderBy(a => a.TalimatBaslik).ToList();
            if (itemlar.Any())
            {

                int say = -1;
                string tbody = "<tbody>";
                foreach (var item in itemlar)
                {
                    say++;

                    tbody += string.Format("<tr>" +
                             "<td align=\"center\">{0}</td>" +
                             "<td align=\"center\">{1}</td>" +
                             "<td align=\"center\">{2}</td>" +
                             "</tr>", (say + 1).ToString(), item.TalimatBaslik, item.TalimatIcerik);

                }
                tbody += "<tbody>";


                string html = "<table border=\"1\" style=\"width:100%\"><thead><tr>" +
                              "<th style=\"width:10%\" align=\"center\">#</th>" +
                              "<th style=\"width:25%\" align=\"center\">Başlık</th> " +
                              "<th style=\"width:65%\" align=\"center\">Açıklama</th>" +
                              "</tr></thead>" + tbody + "</table>";
                webBrowser1.DocumentText = html;

            }
        }

        private void lstPaketlemeGunlukTalimatTipleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            var talimatTipi = (SrcnPaketlemeGunlukTalimatTipis)lstPaketlemeGunlukTalimatTipleri.SelectedItem;
            var bugn = DateTime.Now;
            var limitTarih = bugn.AddDays(-15);
            var tarihler = _dbPoly.SrcnPaketlemeGunlukTalimats
                .Where(a => a.TalimatTarihiDatetime > limitTarih && a.TalimatTarihiDatetime < bugn && a.PaketlemeGunlukTalimatTipi == talimatTipi.PaketlemeGunlukTalimatTipi).OrderByDescending(a => a.TalimatTarihiDatetime).ToList();
            lstTalimatTarihleri.DataSource = tarihler;
            lstTalimatTarihleri.DisplayMember = "TalimatTarihi";

        }


        #endregion

        #region Tefrik Bölümü
        private void TxtIsEmri_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {
                FrmTefrikGiris = this,
                Yazi = txtIsEmri.Text,
                Durum = 2
            }.Show();
        }
        public void BtnIsEmriKontrol_Click(object sender, EventArgs e)
        {
            btnUrunHareket.Visible = false;
            grpoFisBilgileri.Visible = false;
            string refNo = txtIsEmri.Text;

            if (refNo == null)
            {
                MessageBox.Show("Lütfen İş emri numarasını giriniz");
            }
            else
            {
                if (_dbPoly.ZzzSrcnTefrikRefakatFisListe.Any(a => a.RefakatNo == refNo))
                {
                    if (_dbPoly.RefakatKarti.Any(a => a.RefakatNo == refNo && a.IslemSiraNo == 100))
                    {
                        var refKart = _dbPoly.RefakatKarti.First(a => a.RefakatNo == refNo && a.IslemSiraNo == 100);
                        if (refKart.IslemBitisTarihi == null)
                        {
                            RefakatFisModel = _dbPoly.ZzzSrcnTefrikRefakatFisListe.First(a => a.RefakatNo == refNo);
                            RefakatFisLabelDoldur(RefakatFisModel);
                            grpoFisBilgileri.Visible = true;
                            btnUrunHareket.Visible = true;
                        }
                        else
                        {
                            //MessageBox.Show("Bu iş emri işletme tarafından kapatılmıştır. Lütfen işletme ile temasa geçin");
                            MessageBox.Show("Bu iş emrinin siparişi kapatılmıştır. Lütfen işletme ile temasa geçin");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Bilgi işlem ile irtibata geçiniz");
                    }



                }
                else
                {
                    MessageBox.Show("Böyle Bir Kayıt Bulunamadı");
                }

            }

        }
        private void BtnUrunHareket_Click(object sender, EventArgs e)
        {
            HatalariDoldur();
            grpTefrikGirisi.Visible = true;
        }

        private void txtToplamBobin_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {
                FrmTefrikGiris = this,
                Yazi = txtToplamBobin.Text,
                Durum = 3
            }.Show();
        }

        private void txtKisaBobin_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {
                FrmTefrikGiris = this,
                Yazi = txtKisaBobin.Text,
                Durum = 4
            }.Show();
        }
 

        private void vScrolPozisyon_Scroll(object sender, ScrollEventArgs e)
        {
            var deger = vScrolPozisyon.Value * 4;


            if (deger > 334)
            {
                deger = 334;
            }
            lstPozisyonlar.TopIndex = deger;
        }

        private void vScrolHatalar_Scroll(object sender, ScrollEventArgs e)
        {
            var deger = vScrolHatalar.Value;

            lstHatalar.TopIndex = deger;
        }
        private void BtnHataListeyeEkle_Click(object sender, EventArgs e)
        {
            int HataTipSay = lstHatalar.SelectedItems.Count;
            int PozisyonSay = lstPozisyonlar.SelectedItems.Count;
      

            if (HataTipSay == 0 || PozisyonSay == 0)
            {
                MessageBox.Show("Lütfen Hata Tipi ve Pozisyon Seçiniz Seçiniz");
            }
            else
            {
                foreach (var pozisyon in lstPozisyonlar.SelectedItems)
                {
                    var item = new HataliUretimModel();
                    string HataNedeni = "Pozisyon: " + pozisyon.ToString();
                    item.Pozisyon = new DropItem() { Tanim = pozisyon.ToString(), IdInt = Convert.ToInt32(pozisyon.ToString()) };
                    foreach (var hata in lstHatalar.SelectedItems)
                    {
                        HataNedeni += " - " + hata.ToString();

                        item.HataliUretimHareketler.Add(new HataliUretimHareket()
                        {
                            HataNedeni = hata.ToString(),

                        });


                    }

                    item.HataNedeni = HataNedeni;
                    HataliUretimModeller.Add(item);
                }
                MessageBox.Show("Ekleme İşlemi Yapılmıştır");
                HatalariDoldur();
            }
        }
        #endregion

        private void BtnPozisyonsuzEkle_Click(object sender, EventArgs e)
        {
            if (txtPozisyonsuz.Text != null)
            {
                if (lstPozisyonlar.SelectedItems.Count > 0)
                {
                    MessageBox.Show("Pozisyonsuz olarak seçtiniz fakat pozisyon belirttiniz");
                }
                else
                {
                    int Sayi = Convert.ToInt32(txtPozisyonsuz.Text);
                    if (Sayi == 0)
                    {
                        MessageBox.Show("Lütfen Pozisyonu Olmayan Bobin Sayı Belirtiniz");
                    }
                    else
                    {
                        if (lstHatalar.SelectedItems.Count == 0)
                        {
                            MessageBox.Show("Lütfen Hata Seçimi Yapınız");
                        }
                        else
                        {

                            for (int i = 0; i < Sayi; i++)
                            {

                                var item = new HataliUretimModel();
                                string HataNedeni = "Poz: 0";
                                item.Pozisyon = new DropItem() { Tanim = "0", IdInt = 0 };

                                foreach (var hataCHK in lstHatalar.SelectedItems)
                                {
                                    HataNedeni += " - " + hataCHK.ToString();

                                    item.HataliUretimHareketler.Add(new HataliUretimHareket()
                                    {
                                        HataNedeni = hataCHK.ToString(),

                                    });


                                }

                                item.HataNedeni = HataNedeni;
                                HataliUretimModeller.Add(item);

                            }
                            HatalariDoldur();

                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Pozisyonsuz Bobin Sayısı Belirtiniz");
            }

        }
        private void TxtPozisyonsuz_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {
                FrmTefrikGiris = this,
                Yazi = txtPozisyonsuz.Text,
                Durum = 5
            }.Show();
        }
        private void btnOnTefrikAc_Click(object sender, EventArgs e)
        {
            new FrmNumpadPartiNo
            {
                FrmTefrikGiris = this
            }.Show();
        }
        private void btnHataliUretimSil_Click(object sender, EventArgs e)
        {
            string mesaj = "";
            try
            {
                var item = (HataliUretimModel)lstHataliUretimler.SelectedItem;

                if (item == null)
                {
                    mesaj = "Hatası olmayan tefrik listesini silmeye çalıştınız";
                }
                else
                {
                    HataliUretimModeller.Remove(item);
                     mesaj = "Silme İşlemi Yapılmıştır";
                }

            }
            catch (Exception)
            {
                mesaj = "Bilinmeyen Bir Hata Oluştur";
            }

            MessageBox.Show(mesaj);
            HatalariDoldur();
        }
        private void BtnTefrikFinalKaydet_Click(object sender, EventArgs e)
        {
            Decimal Miktar1 = Convert.ToDecimal(txtToplamBobin.Text);
            //Decimal Miktar2 = nmrKisaBobin.Value;

            if (Miktar1 == 0)
            {
                MessageBox.Show("Lütfen Toplam Bobin Sayısını Giriniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (HataliUretimModeller.Any())
                {
                    DialogResult cikis = new DialogResult();
                    cikis = MessageBox.Show("Tüm bilgileri kontrol edip kaydedilmesini onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (cikis == DialogResult.Yes)
                    {
                        TefrikKaydet();
                    }
                    //TefrikKaydet();
                }
                else
                {
                    DialogResult cikis = new DialogResult();
                    cikis = MessageBox.Show("Hatalı Bobin Çıkmadığını Onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (cikis == DialogResult.Yes)
                    {
                        TefrikKaydet();
                    }
                }
            }
        }
        private void LstSonTefrikler_SelectedIndexChanged(object sender, EventArgs e)
        {/*
            try
            {


                var item = (DropItem)lstSonTefrikler.SelectedItem;
                if (item != null)
                {

                    string RefNo = item.Id;
                    int UretimSiraNo = item.IdInt;
                    var uretimHareket = _dbPoly.UretimHareket.First(a =>
                        a.RefakatKartNo == RefNo && a.SiraNo == 300 && a.UretimSiraNo == UretimSiraNo);

                    var hataliUretimler = _dbPoly.HataliUretimHareket
                        .Where(a => a.RefakatKartNo == RefNo && a.UretimSiraNo == UretimSiraNo).OrderBy(a => a.HataMiktari)
                        .ToList();

                    var Headerlar = new List<string>();

                    Headerlar.Add("POZİSYON");
                    Headerlar.Add("HATA");

                    dtSonTefrik.ColumnCount = Headerlar.Count();
                    dtSonTefrik.RowCount = hataliUretimler.Count();
                    dtSonTefrik.Columns[0].Width = 150;
                    dtSonTefrik.Columns[1].Width = 350;

                    foreach (DataGridViewRow x in dtSonTefrik.Rows)
                    {
                        x.MinimumHeight = 38;
                    }

                    for (int i = 0; i < Headerlar.Count; i++)
                    {
                        dtSonTefrik.Columns[i].HeaderText = Headerlar[i];
                        dtSonTefrik.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    var tbody = new List<List<string>>();

                    foreach (var hataliUretim in hataliUretimler)
                    {
                        var tbodyItem = new List<string>();

                        int pozisyon = 0;
                        try
                        {
                            pozisyon = Convert.ToInt32(hataliUretim.HataMiktari);
                        }
                        catch (Exception exception)
                        {

                        }
                        tbodyItem.Add(pozisyon.ToString());
                        tbodyItem.Add(hataliUretim.HataNedeni.ToString());

                        tbody.Add(tbodyItem);
                    }

                    for (int i = 0; i < tbody.Count; i++)
                    {
                        for (int j = 0; j < tbody[i].Count; j++)
                        {
                            dtSonTefrik.Rows[i].Cells[j].Value = tbody[i][j];
                        }
                    }



                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Bilinmeyen Bir Hata Oluştu");
            }

            */
        }


        private void VScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            var deger = vScrollBar4.Value * 4;


            if (deger > 334)
            {
                deger = 334;
            }
            lstOnTefrikPozisyonlar.TopIndex = deger;
        }

        private void VScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            var deger = vScrollBar3.Value;

            lstOnTefrikHatalar.TopIndex = deger;
        }

        private void TxtOnTefToplamBobin_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {
                FrmTefrikGiris = this,
                Yazi = txtToplamBobin.Text,
                Durum = 6
            }.Show();
        }

        private void TxtOnTefKisaBobin_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {
                FrmTefrikGiris = this,
                Yazi = txtToplamBobin.Text,
                Durum = 7
            }.Show();
        }

        private void TxtOnTefPozisyonsuz_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {
                FrmTefrikGiris = this,
                Yazi = txtToplamBobin.Text,
                Durum = 8
            }.Show();
        }

        private void BtnOnTefrikPozisyonsuzEkle_Click(object sender, EventArgs e)
        {
            if (txtOnTefPozisyonsuz.Text != null)
            {
                if (lstOnTefrikPozisyonlar.SelectedItems.Count > 0)
                {
                    MessageBox.Show("Pozisyonsuz olarak seçtiniz fakat pozisyon belirttiniz");
                }
                else
                {
                    int Sayi = Convert.ToInt32(txtOnTefPozisyonsuz.Text);
                    if (Sayi == 0)
                    {
                        MessageBox.Show("Lütfen Pozisyonu Olmayan Bobin Sayısı Belirtiniz");
                    }
                    else
                    {
                        if (lstOnTefrikHatalar.SelectedItems.Count == 0)
                        {
                            MessageBox.Show("Lütfen Hata Seçimi Yapınız");
                        }
                        else
                        {

                            for (int i = 0; i < Sayi; i++)
                            {

                                var item = new HataliUretimModel();
                                string HataNedeni = "Poz: 0";
                                item.Pozisyon = new DropItem() { Tanim = "0", IdInt = 0 };

                                foreach (var hataCHK in lstOnTefrikHatalar.SelectedItems)
                                {
                                    HataNedeni += " - " + hataCHK.ToString();

                                    item.HataliUretimHareketler.Add(new HataliUretimHareket()
                                    {
                                        HataNedeni = hataCHK.ToString(),

                                    });


                                }

                                item.HataNedeni = HataNedeni;
                                HataliUretimOnTefrikModeller.Add(item);

                            }
                            HatalariDoldur();

                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Pozisyonsuz Bobin Sayısı Belirtiniz");
            }
        }

        private void BtnOnTefHataListeyeEkle_Click(object sender, EventArgs e)
        {
            int HataTipSay = lstOnTefrikHatalar.SelectedItems.Count;
            int PozisyonSay = lstOnTefrikPozisyonlar.SelectedItems.Count;


            if (HataTipSay == 0 || PozisyonSay == 0)
            {
                MessageBox.Show("Lütfen Hata Tipi ve Pozisyon Seçiniz Seçiniz");
            }
            else
            {
                foreach (var pozisyon in lstOnTefrikPozisyonlar.SelectedItems)
                {
                    var item = new HataliUretimModel();
                    string HataNedeni = "Pozisyon: " + pozisyon.ToString();
                    item.Pozisyon = new DropItem() { Tanim = pozisyon.ToString(), IdInt = Convert.ToInt32(pozisyon.ToString()) };
                    foreach (var hata in lstOnTefrikHatalar.SelectedItems)
                    {
                        HataNedeni += " - " + hata.ToString();

                        item.HataliUretimHareketler.Add(new HataliUretimHareket()
                        {
                            HataNedeni = hata.ToString(),

                        });


                    }

                    item.HataNedeni = HataNedeni;
                    HataliUretimOnTefrikModeller.Add(item);
                }
                MessageBox.Show("Ekleme İşlemi Yapılmıştır");
                HatalariDoldur();
            }
        }

        private void BtnOnTefHataliUretimSil_Click(object sender, EventArgs e)
        {
            string mesaj = "";
            try
            {
                var item = (HataliUretimModel)lstOnTefrikHataliUretimler.SelectedItem;

                if (item == null)
                {
                    mesaj = "Hatası olmayan tefrik listesini silmeye çalıştınız";
                }
                else
                {
                    HataliUretimOnTefrikModeller.Remove(item);
                    mesaj = "Silme İşlemi Yapılmıştır";
                }

            }
            catch (Exception exception)
            {
                mesaj = "Bilinmeyen Bir Hata Oluştur";
            }

            MessageBox.Show(mesaj);
            HatalariDoldur();
        }

        private void BtnOnTefrikFinalKaydet_Click(object sender, EventArgs e)
        {
            int ToplamBobinSayisi = Convert.ToInt32(txtOnTefToplamBobin.Text);
            int KisaBobinSayisi = Convert.ToInt32(txtOnTefKisaBobin.Text);

            if (ToplamBobinSayisi == 0)
            {
                MessageBox.Show("Lütfen Toplam Bobin Sayısını Giriniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (HataliUretimOnTefrikModeller.Any())
                {
                    DialogResult cikis = new DialogResult();
                    cikis = MessageBox.Show("Tüm bilgileri kontrol edip kaydedilmesini onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (cikis == DialogResult.Yes)
                    {
                        OnTefrikKaydet();
                    }

                }
                else
                {
                    DialogResult cikis = new DialogResult();
                    cikis = MessageBox.Show("Hatalı Bobin Çıkmadığını Onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (cikis == DialogResult.Yes)
                    {
                        OnTefrikKaydet();
                    }
                }
            }
        }
        /*  var dropItemUretimHareket = (DropItem)lstSonTefrikler.SelectedItem;

            var uretimHareket = _dbPoly.UretimHareket.First(a => a.RefakatKartNo.Trim() == dropItemUretimHareket.Tanim && a.UretimSiraNo == dropItemUretimHareket.IdInt);

            var hataliUretimler = _dbPoly.HataliUretimHareket.Where(a => a.RefakatKartNo == uretimHareket.RefakatKartNo && a.UretimSiraNo == uretimHareket.UretimSiraNo).Select(b => new DropItem { Tanim = string.Format("Poz: {0} - {1}", Convert.ToInt32(b.HataMiktari), b.HataNedeni.Trim()), Id = b.HataNedeni.Trim(), Sira = b.HSiraNo }).ToList();
            lstSonTefrikItems.DataSource = hataliUretimler;
            lstSonTefrikItems.DisplayMember = "Tanim";*/
        private void LstSonOntefrikler_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dropOntefrikBilgi = (DropItem)lstSonOntefrikler.SelectedItem;

            decimal OntefrikGrupId = Convert.ToDecimal(dropOntefrikBilgi.Id);

            var liste = _dbPoly.SrcnOntefriks.Where(a => a.OntefrikGrupId == OntefrikGrupId).ToList();

            var dropHataliUretimler = new List<DropItem>();



            if (liste.Any())
            {
                dropHataliUretimler = liste.Select(b => new DropItem
                {
                    Tanim = string.Format("Poz: {0}-{1}", Convert.ToInt32(b.Pozisyon), b.HataNedeni.Trim()),
                    Id = b.OntefrikId.ToString(),

                }).ToList();
            }


            lstSonOnTefrikItems.DataSource = dropHataliUretimler;
            lstSonOnTefrikItems.DisplayMember = "Tanim";
        }

        private void LstSonTefrikler_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var dropItemUretimHareket = (DropItem)lstSonTefrikler.SelectedItem;

            if (dropItemUretimHareket != null)
            {
                if (dropItemUretimHareket.Id != null)
                {
                    var uretimHareket = _dbPoly.UretimHareket.First(a => a.RefakatKartNo.Trim() == dropItemUretimHareket.Id && a.UretimSiraNo == dropItemUretimHareket.IdInt);

                    var hataliUretimler = _dbPoly.HataliUretimHareket.Where(a => a.RefakatKartNo == uretimHareket.RefakatKartNo && a.UretimSiraNo == uretimHareket.UretimSiraNo).ToList();

                    var dropHataliUretimler = new List<DropItem>();

                    if (hataliUretimler.Any())
                    {
                        dropHataliUretimler = hataliUretimler.Select(b => new DropItem
                        {
                            Tanim = string.Format("Poz: {0}-{1}", Convert.ToInt32(b.HataMiktari), b.HataNedeni.Trim()),
                            Id = b.HataNedeni.Trim(),
                            Sira = b.HSiraNo
                        }).ToList();
                    }


                    lstSonTefrikItems.DataSource = dropHataliUretimler;
                    lstSonTefrikItems.DisplayMember = "Tanim";

                    var refFisModel = _dbPoly.ZzzSrcnTefrikRefakatFisListe.First(a => a.RefakatNo.Trim() == dropItemUretimHareket.Id.Trim());
                    lblSonTefrikParti.Text = refFisModel.PartiNo;
                    lblSonTefrikIsEmri.Text = refFisModel.RefakatNo;
                    lblSonTefrikRenk.Text = refFisModel.Renk;
                    lblSonTefrikStokAdi.Text = refFisModel.StokAdi;
                    lblSonTefrikMakineNo.Text = refFisModel.MakineNo;

                }
            }

        }

        private void LblSonTefrikIsEmri_Click(object sender, EventArgs e)
        {

        }

        private void BtnTefrikSil_Click(object sender, EventArgs e)
        {


            var selectedTefrik = (DropItem)lstSonTefrikler.SelectedItem;

            DialogResult cikis = new DialogResult();
            cikis = MessageBox.Show("Tefrik işlemini silmeyi onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (cikis == DialogResult.Yes)
            {
                if (selectedTefrik != null)
                {
                    if (selectedTefrik.Id != null)
                    {
                        var selectedUretimHareket = _dbPoly.UretimHareket.First(a => a.RefakatKartNo.Trim() == selectedTefrik.Id.Trim() && a.UretimSiraNo == selectedTefrik.IdInt && a.SiraNo == 300);

                        var selectedHataliUretimler = _dbPoly.HataliUretimHareket.Where(a => a.RefakatKartNo.Trim() == selectedTefrik.Id.Trim() && a.UretimSiraNo == selectedTefrik.IdInt && a.SiraNo == 300).ToList();

                        if (selectedUretimHareket != null && selectedHataliUretimler != null)
                        {
                            _dbPoly.UretimHareket.Remove(selectedUretimHareket);

                            foreach (var item in selectedHataliUretimler)
                            {
                                _dbPoly.HataliUretimHareket.Remove(item);
                            }
                        }

                        var selectedMakineBildirim = _dbPoly.SrcnMakineHataBildirims.Where(a => a.RefakatKartNo.Trim() == selectedTefrik.Id.Trim() && a.UretimSiraNo == selectedTefrik.IdInt).ToList();
                        if (selectedMakineBildirim != null)
                        {
                            foreach (var item in selectedMakineBildirim)
                            {
                                _dbPoly.SrcnMakineHataBildirims.Remove(item);
                                var selectedMakineBildirimHareket = _dbPoly.SrcnMakineHataBildirimHarekets.Where(a => a.MakineHataBildirimId == item.MakineHataBildirimId).FirstOrDefault();
                                _dbPoly.SrcnMakineHataBildirimHarekets.Remove(selectedMakineBildirimHareket);
                            }
                        }
                    }
                    _dbPoly.SaveChanges();
                    MessageBox.Show("Silme işlemi gerçekleşmiştir.");
                }
                else
                {
                    MessageBox.Show("Lütfen silmek istediğiniz kaydı seçiniz.");
                }




                new FrmTefrikGiris()
                {
                    Personel = Personel
                }.Show();

                this.Close();
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 ss = new Form1();
            ss.Show();

        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 ss = new Form1();
            ss.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 ss = new Form1();
            ss.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 ss = new Form1();
            ss.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (txtPozisyonArama.Text == "")
            {
                lstPozisyonlar.SelectedItem = null;
                MessageBox.Show("Pozisyon numarası girmediniz!!!");

            }
            else
            {
                for (int i = 0; i < lstPozisyonlar.Items.Count; i++)
                {
                    //if (lstPozisyonlar.Items[i].ToString().Contains(txtPozisyonArama.Text))
                    //{
                    //    lstPozisyonlar.SetSelected(i, true);
                    //}
                    if (lstPozisyonlar.Items[i].ToString() == txtPozisyonArama.Text)
                    {
                        lstPozisyonlar.SetSelected(i, true);
                    }
                }

            }
        }

        private void txtPozisyonArama_Click_1(object sender, EventArgs e)
        {
            txtPozisyonArama.Text = null;

            txtPozisyonArama.ForeColor = Color.Black;
            new FrmNumPad()
            {

                FrmTefrikGiris = this,
                Yazi = txtPozisyonArama.Text,
                Durum = 9

            }.Show();
        }

        private void txtPozisyonSearch_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {

                FrmTefrikGiris = this,
                Yazi = txtPozisyonSearch.Text,
                Durum = 10

            }.Show();
        }

        //private void btnHataAra_Click(object sender, EventArgs e)
        //{
        //    if (txtHata.Text == "")
        //    {
        //        lstHatalar.SelectedItem = null;
        //        MessageBox.Show("Hata adı girmediniz!!!");

        //    }
        //    else
        //    {
        //        for (int i = 0; i < lstHatalar.Items.Count; i++)
        //        {
        //            if (lstHatalar.Items[i].ToString().ToLower().Contains(txtHata.Text.ToLower()))
        //            {
        //                lstHatalar.SetSelected(i, true);
        //            }
        //        }

        //    }
        //}

        private void btnPozisyonSearch_Click(object sender, EventArgs e)
        {
            if (txtPozisyonSearch.Text == "")
            {
                lstOnTefrikPozisyonlar.SelectedItem = null;
                MessageBox.Show("Pozisyon numarası girmediniz!!!");

            }
            else
            {
                for (int i = 0; i < lstOnTefrikPozisyonlar.Items.Count; i++)
                {
                    //if (lstPozisyonlar.Items[i].ToString().Contains(txtPozisyonArama.Text))
                    //{
                    //    lstPozisyonlar.SetSelected(i, true);
                    //}
                    if (lstOnTefrikPozisyonlar.Items[i].ToString() == txtPozisyonSearch.Text)
                    {
                        lstOnTefrikPozisyonlar.SetSelected(i, true);
                    }
                }

            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 ss = new Form1();
            ss.Show();
        }

        private void btnOnTefrik_Click(object sender, EventArgs e)
        {
            var selectedOnTefrik = (DropItem)lstSonOntefrikler.SelectedItem;

            DialogResult cikis = new DialogResult();
            cikis = MessageBox.Show("Tefrik işlemini silmeyi onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (cikis == DialogResult.Yes)
            {
                if (selectedOnTefrik != null)
                {
                    if (selectedOnTefrik != null)
                    {
                        int OntefrikGrupId = Convert.ToInt32(selectedOnTefrik.Id);


                        var selectedOnUretimHareket = _dbPoly.SrcnOntefrikGrups.Where(a => a.OntefrikGrupId == OntefrikGrupId).First();

                        var selectedOnHataliUretimler = _dbPoly.SrcnOntefriks.Where(a => a.OntefrikGrupId == OntefrikGrupId).ToList();

                        if (selectedOnHataliUretimler != null)
                        {

                            _dbPoly.SrcnOntefrikGrups.Remove(selectedOnUretimHareket);
                            _dbPoly.SaveChanges();
                            foreach (var item in selectedOnHataliUretimler)
                            {

                                _dbPoly.SrcnOntefriks.Remove(item);
                                _dbPoly.SaveChanges();
                            }
                        }


                    }
                    _dbPoly.SaveChanges();
                    MessageBox.Show("Silme işlemi gerçekleşmiştir.");
                }
                else
                {
                    MessageBox.Show("Lütfen silmek istediğiniz kaydı seçiniz.");
                }




                new FrmTefrikGiris()
                {
                    Personel = Personel
                }.Show();

                this.Close();

            }





            //private void txtHata_MouseClick(object sender, MouseEventArgs e)
            //{
            //    Process.Start("osk.exe");
            //    //System.Diagnostics.Process.Start("osk.exe");
            //}


        }

        

        private void txtCorapArabaNo_Click(object sender, EventArgs e)
        {
            new FrmNumPad()
            {

                FrmTefrikGiris = this,
                Yazi = txtCorapArabaNo.Text,
                Durum = 11

            }.Show();

        }
    }
}

