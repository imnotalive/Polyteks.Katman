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

namespace Polyteks.Katman.LaborantGIAS
{
    public partial class FrmGiris : Form
    {
        public FrmGiris()
        {
            InitializeComponent();
        }
        public POTA_PTKSEntities _dbPoly = new POTA_PTKSEntities();
        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void BtnGirisYap_Click(object sender, EventArgs e)
        {

        }

        private void FrmGiris_Load(object sender, EventArgs e)
        {
            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;

            var liste = _dbPoly.SrcnKullanicis.Where(a => a.GizlemeDurumu == 0 && a.BirimId==18)
                .OrderBy(a => a.IsimSoyisim).ToList();

            lstKullanicilar.DataSource = liste;
            lstKullanicilar.DisplayMember = "IsimSoyisim";

          
        }

        private void LstKullanicilar_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (SrcnKullanicis) lstKullanicilar.SelectedItem;
            lblIsimSoyisim.Text = item.IsimSoyisim.ToUpper();
            lblIsimSoyisim.ForeColor = Color.White;
            picResim.Image = null;
            if (item.Resim!=null)
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
            new FrmNumPad()
            {
                YapilacakIslem = 2,
                Yazi = txtSifre.Text,
                Kullanici =  (SrcnKullanicis)lstKullanicilar.SelectedItem

        }.Show();
            //  this.Close();
        }
    }
}
