namespace Polyteks.Katman.LaborantGIAS
{
    partial class FrmGunlukImalatListe
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtGunlukImalatlar = new System.Windows.Forms.DataGridView();
            this.btnGunlukImalatEkle = new System.Windows.Forms.Button();
            this.lblIsim = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGunlukImalatlar)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtGunlukImalatlar);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(0, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(804, 993);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GÜNLÜK İMALAT ANALİZLERİ";
            // 
            // dtGunlukImalatlar
            // 
            this.dtGunlukImalatlar.AllowUserToAddRows = false;
            this.dtGunlukImalatlar.AllowUserToDeleteRows = false;
            this.dtGunlukImalatlar.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGunlukImalatlar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGunlukImalatlar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGunlukImalatlar.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dtGunlukImalatlar.BackgroundColor = System.Drawing.Color.Tan;
            this.dtGunlukImalatlar.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGunlukImalatlar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtGunlukImalatlar.ColumnHeadersHeight = 35;
            this.dtGunlukImalatlar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtGunlukImalatlar.DefaultCellStyle = dataGridViewCellStyle3;
            this.dtGunlukImalatlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGunlukImalatlar.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dtGunlukImalatlar.Location = new System.Drawing.Point(3, 27);
            this.dtGunlukImalatlar.MultiSelect = false;
            this.dtGunlukImalatlar.Name = "dtGunlukImalatlar";
            this.dtGunlukImalatlar.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dtGunlukImalatlar.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dtGunlukImalatlar.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dtGunlukImalatlar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtGunlukImalatlar.Size = new System.Drawing.Size(798, 963);
            this.dtGunlukImalatlar.TabIndex = 1;
            this.dtGunlukImalatlar.DoubleClick += new System.EventHandler(this.dtGunlukImalatlar_DoubleClick);
            // 
            // btnGunlukImalatEkle
            // 
            this.btnGunlukImalatEkle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGunlukImalatEkle.Location = new System.Drawing.Point(15, 12);
            this.btnGunlukImalatEkle.Name = "btnGunlukImalatEkle";
            this.btnGunlukImalatEkle.Size = new System.Drawing.Size(259, 35);
            this.btnGunlukImalatEkle.TabIndex = 1;
            this.btnGunlukImalatEkle.Text = "Yeni Kayıt Ekle";
            this.btnGunlukImalatEkle.UseVisualStyleBackColor = true;
            this.btnGunlukImalatEkle.Click += new System.EventHandler(this.BtnGunlukImalatEkle_Click);
            // 
            // lblIsim
            // 
            this.lblIsim.AutoSize = true;
            this.lblIsim.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblIsim.Location = new System.Drawing.Point(476, 37);
            this.lblIsim.Name = "lblIsim";
            this.lblIsim.Size = new System.Drawing.Size(76, 25);
            this.lblIsim.TabIndex = 4;
            this.lblIsim.Text = "label1";
            // 
            // FrmGunlukImalatListe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 1061);
            this.Controls.Add(this.lblIsim);
            this.Controls.Add(this.btnGunlukImalatEkle);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmGunlukImalatListe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmGunlukImalatListe_FormClosing);
            this.Load += new System.EventHandler(this.FrmGunlukImalatListe_Load_1);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGunlukImalatlar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGunlukImalatEkle;
        private System.Windows.Forms.Label lblIsim;
        private System.Windows.Forms.DataGridView dtGunlukImalatlar;
    }
}