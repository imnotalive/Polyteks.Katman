namespace Polyteks.Katman.WFATefrikProgrami
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vScrolKullanici = new System.Windows.Forms.VScrollBar();
            this.button1 = new System.Windows.Forms.Button();
            this.lstKullanicilar = new System.Windows.Forms.ListBox();
            this.btnGiris = new System.Windows.Forms.Button();
            this.lblIsimSoyisim = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.picResim = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnVeriYenile = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResim)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
         
            this.groupBox1.Controls.Add(this.vScrolKullanici);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.lstKullanicilar);
            this.groupBox1.Controls.Add(this.btnGiris);
            this.groupBox1.Controls.Add(this.lblIsimSoyisim);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSifre);
            this.groupBox1.Controls.Add(this.picResim);
        
         
            this.groupBox1.Location = new System.Drawing.Point(-4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1085, 684);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Giriş Yap";
            // 
            // vScrolKullanici
            // 
            this.vScrolKullanici.Location = new System.Drawing.Point(522, 49);
            this.vScrolKullanici.Name = "vScrolKullanici";
            this.vScrolKullanici.Size = new System.Drawing.Size(67, 532);
            this.vScrolKullanici.TabIndex = 7;
            this.vScrolKullanici.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrolKullanici_Scroll);
            // 
            // button1
            // 
      
            this.button1.Location = new System.Drawing.Point(627, 596);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(334, 54);
            this.button1.TabIndex = 6;
            this.button1.Text = "UYGULAMADAN ÇIK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstKullanicilar
            // 
     
            this.lstKullanicilar.FormattingEnabled = true;
            this.lstKullanicilar.ItemHeight = 33;
            this.lstKullanicilar.Location = new System.Drawing.Point(22, 49);
            this.lstKullanicilar.Name = "lstKullanicilar";
            this.lstKullanicilar.Size = new System.Drawing.Size(497, 532);
            this.lstKullanicilar.TabIndex = 4;
            this.lstKullanicilar.SelectedIndexChanged += new System.EventHandler(this.LstKullanicilar_SelectedIndexChanged);
            // 
            // btnGiris
            // 
        
            this.btnGiris.Location = new System.Drawing.Point(627, 527);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(334, 54);
            this.btnGiris.TabIndex = 5;
            this.btnGiris.Text = "GİRİŞ YAP";
            this.btnGiris.UseVisualStyleBackColor = false;
            this.btnGiris.Click += new System.EventHandler(this.BtnGiris_Click);
            // 
            // lblIsimSoyisim
            // 
            this.lblIsimSoyisim.AutoSize = true;

            this.lblIsimSoyisim.Location = new System.Drawing.Point(621, 403);
            this.lblIsimSoyisim.Name = "lblIsimSoyisim";
            this.lblIsimSoyisim.Size = new System.Drawing.Size(46, 31);
            this.lblIsimSoyisim.TabIndex = 2;
            this.lblIsimSoyisim.Text = "ad";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(623, 450);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "ŞİFRE";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "İSİM SOYİSİM";
            // 
            // txtSifre
            // 
        
            this.txtSifre.Location = new System.Drawing.Point(627, 473);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '*';
            this.txtSifre.Size = new System.Drawing.Size(334, 38);
            this.txtSifre.TabIndex = 1;
            this.txtSifre.Click += new System.EventHandler(this.TxtSifre_Click);
            // 
            // picResim
            // 
            this.picResim.Location = new System.Drawing.Point(627, 49);
            this.picResim.Name = "picResim";
            this.picResim.Size = new System.Drawing.Size(334, 336);
       
            this.picResim.TabIndex = 0;
            this.picResim.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
        
            this.tabControl1.ItemSize = new System.Drawing.Size(188, 36);
            this.tabControl1.Location = new System.Drawing.Point(1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1085, 724);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1077, 680);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "GİRİŞ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnVeriYenile);
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 40);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1077, 680);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "GRAFİK";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnVeriYenile
            // 
   
            this.btnVeriYenile.Location = new System.Drawing.Point(0, 612);
            this.btnVeriYenile.Name = "btnVeriYenile";
            this.btnVeriYenile.Size = new System.Drawing.Size(1077, 62);
            this.btnVeriYenile.TabIndex = 1;
            this.btnVeriYenile.Text = "Verileri Yenile";
            this.btnVeriYenile.UseVisualStyleBackColor = false;
            this.btnVeriYenile.Click += new System.EventHandler(this.BtnVeriYenile_Click);
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1077, 606);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
       
            this.ClientSize = new System.Drawing.Size(1084, 722);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "GİRİŞ YAP";
      
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResim)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstKullanicilar;
        private System.Windows.Forms.Label lblIsimSoyisim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picResim;
        public System.Windows.Forms.Button btnGiris;
        public System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btnVeriYenile;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.VScrollBar vScrolKullanici;
    }
}

