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
using WordToPDF;

namespace Tez_Analiz
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string dosya_yolu = "";
        public static string dosya_adi = "";
        public static string dosya_uzanti = "";
        public static string dosya_site = "";
        bool durum = false, tur;

        private void btn_Analiz_Click(object sender, EventArgs e)
        {
            if (durum == true)
            {
                if (tur == true)
                {
                    //word dosyası olarak geliyor.
                    Word2Pdf objWorPdf = new Word2Pdf();
                    string d_adi = dosya_yolu.Replace(dosya_adi,"");
                    string backfolder1 = @""+d_adi.ToString()+"";
                    string strFileName = ""+dosya_adi.ToString()+"";
                    object FromLocation = backfolder1 + "\\" + strFileName;
                    string FileExtension = Path.GetExtension(strFileName);
                    string ChangeExtension = strFileName.Replace(FileExtension, ".pdf");
                    if (FileExtension == ".doc" || FileExtension == ".docx")
                    {
                        object ToLocation = backfolder1 + "\\" + ChangeExtension;
                        objWorPdf.InputLocation = FromLocation;
                        objWorPdf.OutputLocation = ToLocation;
                        objWorPdf.Word2PdfCOnversion();
                        dosya_yolu = ToLocation.ToString();
                    }
                }
                else
                {
                    //pdf dosyası olarak geliyor.
                }
                
                frmAnaliz analiz = new frmAnaliz();
                this.Hide();
                analiz.Show();
                
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tez Analizi v1.0\nauthor:Kadir GÜRBÜZ");
        }

        private void btn_Gozat_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            file.Title = "Word-Pdf Seçiniz";
            file.ShowReadOnly = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                dosya_yolu = file.FileName.ToString();
                dosya_adi = file.SafeFileName.ToString();
                dosya_uzanti = Path.GetExtension(file.FileName);
                //MessageBox.Show(dosya_uzanti.ToString());
                if (dosya_uzanti == ".pdf")
                {
                    pc_Resim.Image = Properties.Resources.pdf;
                    lbl_Bilgi.ForeColor = Color.Green;
                    lbl_Bilgi.Text = "PDF";
                    txt_Yol.Text = dosya_yolu.ToString();
                    durum = true;
                    tur = false;
                }
                else if (dosya_uzanti == ".doc" || dosya_uzanti == ".docx")
                {
                    pc_Resim.Image = Properties.Resources.word;
                    lbl_Bilgi.ForeColor = Color.Green;
                    lbl_Bilgi.Text = "Word";
                    txt_Yol.Text = dosya_yolu.ToString();
                    durum = true;
                    tur = true;
                }
                else
                {
                    pc_Resim.Image = Properties.Resources.error;
                    lbl_Bilgi.ForeColor = Color.Red;
                    lbl_Bilgi.Text = "Desteklenmiyor";
                    txt_Yol.Text = "";
                    durum = false;
                }
            }
        }
    }
}
