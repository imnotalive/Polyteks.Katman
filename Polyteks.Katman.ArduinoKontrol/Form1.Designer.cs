namespace Polyteks.Katman.ArduinoKontrol
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
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnYak = new System.Windows.Forms.Button();
            this.cmbPortlar = new System.Windows.Forms.ComboBox();
            this.btnSondur = new System.Windows.Forms.Button();
            this.txtVeriler = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtVeriler);
            this.groupBox1.Controls.Add(this.cmbPortlar);
            this.groupBox1.Controls.Add(this.btnSondur);
            this.groupBox1.Controls.Add(this.btnYak);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(26, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 241);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Arduino";
            // 
            // btnYak
            // 
            this.btnYak.Location = new System.Drawing.Point(36, 81);
            this.btnYak.Name = "btnYak";
            this.btnYak.Size = new System.Drawing.Size(75, 23);
            this.btnYak.TabIndex = 0;
            this.btnYak.Text = "Yak";
            this.btnYak.UseVisualStyleBackColor = true;
            this.btnYak.Click += new System.EventHandler(this.btnYak_Click);
            // 
            // cmbPortlar
            // 
            this.cmbPortlar.FormattingEnabled = true;
            this.cmbPortlar.Location = new System.Drawing.Point(36, 42);
            this.cmbPortlar.Name = "cmbPortlar";
            this.cmbPortlar.Size = new System.Drawing.Size(121, 23);
            this.cmbPortlar.TabIndex = 1;
            // 
            // btnSondur
            // 
            this.btnSondur.Location = new System.Drawing.Point(36, 122);
            this.btnSondur.Name = "btnSondur";
            this.btnSondur.Size = new System.Drawing.Size(75, 23);
            this.btnSondur.TabIndex = 0;
            this.btnSondur.Text = "Söndür";
            this.btnSondur.UseVisualStyleBackColor = true;
            this.btnSondur.Click += new System.EventHandler(this.btnSondur_Click);
            // 
            // txtVeriler
            // 
            this.txtVeriler.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtVeriler.Location = new System.Drawing.Point(208, 42);
            this.txtVeriler.Name = "txtVeriler";
            this.txtVeriler.Size = new System.Drawing.Size(100, 31);
            this.txtVeriler.TabIndex = 2;
            this.txtVeriler.Text = "0000000";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 354);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbPortlar;
        private System.Windows.Forms.Button btnSondur;
        private System.Windows.Forms.Button btnYak;
        private System.Windows.Forms.TextBox txtVeriler;
    }
}

