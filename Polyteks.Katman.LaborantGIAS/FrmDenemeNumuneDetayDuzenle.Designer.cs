namespace Polyteks.Katman.LaborantGIAS
{
    partial class FrmDenemeNumuneDetayDuzenle
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
            this.grpGenelBilgiler = new System.Windows.Forms.GroupBox();
            this.dtDenemeNumune = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtDenemeNumune)).BeginInit();
            this.SuspendLayout();
            // 
            // grpGenelBilgiler
            // 
            this.grpGenelBilgiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.grpGenelBilgiler.Location = new System.Drawing.Point(13, 13);
            this.grpGenelBilgiler.Name = "grpGenelBilgiler";
            this.grpGenelBilgiler.Size = new System.Drawing.Size(780, 100);
            this.grpGenelBilgiler.TabIndex = 0;
            this.grpGenelBilgiler.TabStop = false;
            this.grpGenelBilgiler.Text = "Analiz Genel Bilgileri";
            // 
            // dtDenemeNumune
            // 
            this.dtDenemeNumune.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtDenemeNumune.Location = new System.Drawing.Point(13, 120);
            this.dtDenemeNumune.Name = "dtDenemeNumune";
            this.dtDenemeNumune.Size = new System.Drawing.Size(780, 930);
            this.dtDenemeNumune.TabIndex = 1;
            // 
            // FrmDenemeNumuneDetayDuzenle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 1061);
            this.Controls.Add(this.dtDenemeNumune);
            this.Controls.Add(this.grpGenelBilgiler);
            this.Name = "FrmDenemeNumuneDetayDuzenle";
            this.Text = "FrmDenemeNumuneDetayDuzenle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDenemeNumuneDetayDuzenle_FormClosing);
            this.Load += new System.EventHandler(this.FrmDenemeNumuneDetayDuzenle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtDenemeNumune)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpGenelBilgiler;
        private System.Windows.Forms.DataGridView dtDenemeNumune;
    }
}