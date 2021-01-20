namespace Tez_Analiz
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
            this.formAssistant1 = new DevExpress.XtraBars.FormAssistant();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Yol = new DevExpress.XtraEditors.TextEdit();
            this.btn_Gozat = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Analiz = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lbl_Bilgi = new DevExpress.XtraEditors.LabelControl();
            this.pc_Resim = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Yol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pc_Resim)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(159, 293);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(53, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Dosya yolu";
            // 
            // txt_Yol
            // 
            this.txt_Yol.Location = new System.Drawing.Point(159, 312);
            this.txt_Yol.Name = "txt_Yol";
            this.txt_Yol.Size = new System.Drawing.Size(204, 20);
            this.txt_Yol.TabIndex = 2;
            // 
            // btn_Gozat
            // 
            this.btn_Gozat.Location = new System.Drawing.Point(369, 312);
            this.btn_Gozat.Name = "btn_Gozat";
            this.btn_Gozat.Size = new System.Drawing.Size(41, 20);
            this.btn_Gozat.TabIndex = 3;
            this.btn_Gozat.Text = "...";
            this.btn_Gozat.Click += new System.EventHandler(this.btn_Gozat_Click);
            // 
            // btn_Analiz
            // 
            this.btn_Analiz.Location = new System.Drawing.Point(159, 338);
            this.btn_Analiz.Name = "btn_Analiz";
            this.btn_Analiz.Size = new System.Drawing.Size(251, 20);
            this.btn_Analiz.TabIndex = 4;
            this.btn_Analiz.Text = "Analizi başlat";
            this.btn_Analiz.Click += new System.EventHandler(this.btn_Analiz_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(159, 364);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(186, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Lütfen Word yada PDF dosyası seçiniz.";
            // 
            // lbl_Bilgi
            // 
            this.lbl_Bilgi.Appearance.Options.UseTextOptions = true;
            this.lbl_Bilgi.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lbl_Bilgi.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_Bilgi.Location = new System.Drawing.Point(218, 293);
            this.lbl_Bilgi.Name = "lbl_Bilgi";
            this.lbl_Bilgi.Size = new System.Drawing.Size(192, 13);
            this.lbl_Bilgi.TabIndex = 6;
            // 
            // pc_Resim
            // 
            this.pc_Resim.Image = global::Tez_Analiz.Properties.Resources.file;
            this.pc_Resim.Location = new System.Drawing.Point(159, 34);
            this.pc_Resim.Margin = new System.Windows.Forms.Padding(150, 25, 150, 5);
            this.pc_Resim.Name = "pc_Resim";
            this.pc_Resim.Size = new System.Drawing.Size(251, 251);
            this.pc_Resim.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pc_Resim.TabIndex = 0;
            this.pc_Resim.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(548, 364);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "?";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 386);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_Bilgi);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btn_Analiz);
            this.Controls.Add(this.btn_Gozat);
            this.Controls.Add(this.txt_Yol);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.pc_Resim);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Image = global::Tez_Analiz.Properties.Resources.archive;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tez Analiz";
            ((System.ComponentModel.ISupportInitialize)(this.txt_Yol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pc_Resim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.FormAssistant formAssistant1;
        private System.Windows.Forms.PictureBox pc_Resim;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_Yol;
        private DevExpress.XtraEditors.SimpleButton btn_Gozat;
        private DevExpress.XtraEditors.SimpleButton btn_Analiz;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lbl_Bilgi;
        private System.Windows.Forms.Label label1;
    }
}

