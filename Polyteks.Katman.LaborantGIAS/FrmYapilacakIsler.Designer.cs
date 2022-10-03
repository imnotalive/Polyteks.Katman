namespace Polyteks.Katman.LaborantGIAS
{
    partial class FrmYapilacakIsler
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
            this.components = new System.ComponentModel.Container();
            this.btnGIAT = new System.Windows.Forms.Button();
            this.btnOAT = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDenemeNumune = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTarih = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnGIAT
            // 
            this.btnGIAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGIAT.Location = new System.Drawing.Point(36, 12);
            this.btnGIAT.Name = "btnGIAT";
            this.btnGIAT.Size = new System.Drawing.Size(433, 86);
            this.btnGIAT.TabIndex = 0;
            this.btnGIAT.Text = "GÜNLÜK İMALAT ANALİZLERİ";
            this.btnGIAT.UseVisualStyleBackColor = true;
            this.btnGIAT.Click += new System.EventHandler(this.BtnGIAT_Click);
            // 
            // btnOAT
            // 
            this.btnOAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOAT.Location = new System.Drawing.Point(36, 196);
            this.btnOAT.Name = "btnOAT";
            this.btnOAT.Size = new System.Drawing.Size(433, 86);
            this.btnOAT.TabIndex = 0;
            this.btnOAT.Text = "ANALİZ TALEPLERİ";
            this.btnOAT.UseVisualStyleBackColor = true;
            this.btnOAT.Click += new System.EventHandler(this.BtnOAT_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(48, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "İSİM SOYİSİM";
            // 
            // btnDenemeNumune
            // 
            this.btnDenemeNumune.BackColor = System.Drawing.Color.Silver;
            this.btnDenemeNumune.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDenemeNumune.Location = new System.Drawing.Point(36, 104);
            this.btnDenemeNumune.Name = "btnDenemeNumune";
            this.btnDenemeNumune.Size = new System.Drawing.Size(433, 86);
            this.btnDenemeNumune.TabIndex = 0;
            this.btnDenemeNumune.Text = "DENEME  NUMUNE ANALİZ TALEPLERİ";
            this.btnDenemeNumune.UseVisualStyleBackColor = false;
            this.btnDenemeNumune.Click += new System.EventHandler(this.BtnDenemeNumune_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(475, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Son Güncelleme Tarihi ";
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTarih.Location = new System.Drawing.Point(475, 47);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(184, 18);
            this.lblTarih.TabIndex = 2;
            this.lblTarih.Text = "Son Güncelleme Tarihi ";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FrmYapilacakIsler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Polyteks.Katman.LaborantGIAS.Properties.Resources.back2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(804, 419);
            this.Controls.Add(this.lblTarih);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDenemeNumune);
            this.Controls.Add(this.btnOAT);
            this.Controls.Add(this.btnGIAT);
            this.Name = "FrmYapilacakIsler";
            this.Text = "PLANLANAN GÖREVLER";
            this.Load += new System.EventHandler(this.FrmYapilacakIsler_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGIAT;
        private System.Windows.Forms.Button btnOAT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDenemeNumune;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.Timer timer1;
    }
}