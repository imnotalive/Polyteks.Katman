﻿namespace Polyteks.Katman.LaborantGIAS
{
    partial class FrmGunlukImalatEkle
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBobinSayisi = new System.Windows.Forms.TextBox();
            this.cmbBirimler = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtBobinSayisi);
            this.groupBox1.Controls.Add(this.cmbBirimler);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(5, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 278);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Günlük İmalat";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 209);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(232, 47);
            this.button1.TabIndex = 3;
            this.button1.Text = "Kaydet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(30, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "TOPLAM BOBİN SAYISI";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Birim";
            // 
            // txtBobinSayisi
            // 
            this.txtBobinSayisi.Location = new System.Drawing.Point(29, 160);
            this.txtBobinSayisi.Name = "txtBobinSayisi";
            this.txtBobinSayisi.Size = new System.Drawing.Size(232, 31);
            this.txtBobinSayisi.TabIndex = 1;
            this.txtBobinSayisi.Click += new System.EventHandler(this.TxtBobinSayisi_Click);
            // 
            // cmbBirimler
            // 
            this.cmbBirimler.FormattingEnabled = true;
            this.cmbBirimler.Location = new System.Drawing.Point(29, 81);
            this.cmbBirimler.Name = "cmbBirimler";
            this.cmbBirimler.Size = new System.Drawing.Size(232, 33);
            this.cmbBirimler.TabIndex = 0;
            // 
            // FrmGunlukImalatEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 294);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmGunlukImalatEkle";
            this.Text = "FrmGunlukImalatEkle";
            this.Load += new System.EventHandler(this.FrmGunlukImalatEkle_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtBobinSayisi;
        public System.Windows.Forms.ComboBox cmbBirimler;
        private System.Windows.Forms.Button button1;
    }
}