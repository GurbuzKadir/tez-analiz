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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.Pdf;
using iTextSharp.text;
using Microsoft.Office.Interop.Word;
using SautinSoft;
using SautinSoft.Document;
/*
Çıktı olarak gönderilecek sonuçlar 
*/

/*
        #[İçindekiler]
 
        List<string> icindekiler_b1_baslik = new List<string>(); - içindekiler başlıkları
        List<string> icindekiler_b1_sayfa = new List<string>(); - içindekiler sayfa numaraları
        List<string> icindekiler_varyok = new List<string>(); - içindekiler sayfa numaraları tutarlı mı

        List<string> loglar = new List<string>();

    report
*/
/*
        #[Şekiller]

        List<string> sekiller_b1_baslik = new List<string>(); - şekil başlıkları
        List<string> sekiller_b1_sayfa = new List<string>(); - şekil sayfa numaraları
        List<string> sekiller_varyok = new List<string>(); - sayfa numaraları tutarlı mı
        List<string> sekil_uyum_sonuc = new List<string>(); - başlıklar ile uyumlu yerde mi sonucu

        List<string> loglar = new List<string>();

    report
*/
/*
       #[Tablolar]
 
        List<string> tablolar_b1_baslik = new List<string>();
        List<string> tablolar_b1_sayfa = new List<string>();
        List<string> tablolar_varyok = new List<string>();
        List<string> tablo_uyum_sonuc = new List<string>();

        List<string> loglar = new List<string>();

    report
*/
/*
       #[Önsöz] - teşekkür ibaresi var mı
 
        onsoz_durum; - var yada yok
        onsoz_yazi; - önsöz ilk sayfası

        List<string> loglar = new List<string>();

    report
*/
/*
       #[Denklem kontrolü]
 
        List<string> tablo_uyum_sonuc = new List<string>(); - ana başlıklar listesi
        List<string> baslangic_s = new List<string>(); - ana başlık başlangıç sayfaları
        List<string> bitis_s = new List<string>(); - ana başlık bitiş sayfaları
        List<string> denklem_denklemler = new List<string>(); - uyumsuz denklem listesi

        List<string> loglar = new List<string>();

    report
*/
/*
        #[Kaynaklar]
 
        kaynak_s - int - kaynak sayısı
        List<string> kaynaklar_ayri = new List<string>(); - tüm kaynaklar ayrılmış şekilde
        kaynak_atif1 - byte[] - atıf durumları
        atif_durum - string - atıf yapılmayan kaynak var mı yok mu (VAR yada YOK)

        List<string> loglar = new List<string>();

    report
*/
/*
        #[Çift tırnak içindeki cümle sayısı]
 
        count_kelime - int 

        List<string> loglar = new List<string>();

    report
*/
/*
        #[Giriş bölümü]
 
        giris_ayir_str - string - giriş bölümü yazısı

        List<string> loglar = new List<string>();

    report
*/
/*
        #[İki satırdan az paragraf kontrol]
 
        List<string> iki_satir_az = new List<string>(); - iki satırdan az paragraflar
        pp - int - iki satırdan az paragraf sayısı

        List<string> loglar = new List<string>();

    report
*/
/*
    ## Analiz ekranında loading animasyonu yapılır ve alt kısmında ise log kayıtları akar.
    PDF önizleme de yapılabilirse çok iyi olur.
    
    using iTextSharp.text;
    using Microsoft.Office.Interop.Word;
    using SautinSoft;
    using SautinSoft.Document;
    using iTextSharp.text.pdf;

    Bu kütüphaneler önemli burada yaptığım işlemlerde kullanıldılar. Nugette var.
    Sautinsoft reklam basıyor bu reklamları isaretle() fonksiyonu içerisinde kaldırdım.
*/
namespace Tez_Analiz
{
    public partial class frmAnaliz : DevExpress.XtraEditors.XtraForm
    {
        public frmAnaliz()
        {
            InitializeComponent();
        }
        /*
         Loglar

            içindekiler
            tablolar
            şekiller
            önsöz
            tablo uyum
            şekil uyum
            denklem kontrol
            kaynaklar
            giris_ayir
            pdf işaretle

             */

        List<string> loglar = new List<string>();
        string icindekiler_report = "", tablolar_report = "",onsoz_report="", sekiller_report = "", denklemler_report = "", kaynaklar_report = "", iki_satir_report = "", giris_report = "",loglar_report="";

        string onsoz_ilk = "";
        int onsoz_char = 5;
        string onsoz_durum = "";
        string[] onsoz_kelime = new string[] { "teşekkür", "teşekkürler","minnettar" };

        int icindekiler_sayfa=0;
        string icindekiler_diger_icerik = "";
        List<string> icindekiler_baslik = new List<string>();
        List<string> icindekiler_b1 = new List<string>();
        List<string> icindekiler_b1_baslik = new List<string>();
        List<string> icindekiler_b1_sayfa = new List<string>();
        List<string> icindekiler_varyok = new List<string>();
        List<string> icindekiler_durum = new List<string>();
        //List<string> icindekiler_pdf_sayfa = new List<string>();
        string list_bas;
        int nokta_bas, nokta_son;
        int satir_s = 1;
        int satir_karakter = 0;
        string icindekiler_tpl = "";


        int tablolar_sayfa = 0;
        string tablolar_diger_icerik="";
        List<string> tablolar_baslik = new List<string>();
        List<string> tablolar_b1 = new List<string>();
        List<string> tablolar_b1_baslik = new List<string>();
        List<string> tablolar_b1_sayfa = new List<string>();
        List<string> tablolar_varyok = new List<string>();
        List<string> tablolar_durum = new List<string>();
        string tablolar_list_bas;
        int tablolar_nokta_bas, tablolar_nokta_son;
        int tablolar_satir_s = 1;
        int tablolar_satir_karakter = 0;
        string tablolar_tpl = "";

        int sekiller_sayfa = 0;
        string sekiller_diger_icerik = "";
        List<string> sekiller_baslik = new List<string>();
        List<string> sekiller_b1 = new List<string>();
        List<string> sekiller_b1_baslik = new List<string>();
        List<string> sekiller_b1_sayfa = new List<string>();
        List<string> sekiller_varyok = new List<string>();
        List<string> sekiller_durum = new List<string>();
        string sekiller_list_bas;
        int sekiller_nokta_bas, sekiller_nokta_son;
        int sekiller_satir_s = 1;
        int sekiller_satir_karakter = 0;
        string sekiller_tpl = "";

        string dosya_yolu1 = "";
        int sayfa1 = 1;
        int sayfa_kaynak,kaynak_s=1;
        string tum_kaynaklar = "";
        string atif1 = "";
        string att1 = "";

        string atif_durum = "";
        string onsoz_yazi="";
        void onsoz()
        {
            if (File.Exists(dosya_yolu1))
            {
                loglar.Add("Önsöz pdf okunuyor");
                PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                string icerik = "", bulunan = "", bulunan_k = "";
                for (int sayfa = 1; sayfa <= pdfOkuyucu.NumberOfPages; sayfa++)
                {
                    //loglar.Add("Önsöz pdf "+sayfa+". sayfa okunuyor");
                    bulunan = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                    bulunan_k = $"" + (PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)).ToLower().ToString() + "";
                    icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";


                    if (bulunan_k.IndexOf("önsöz") != -1&& bulunan_k.IndexOf("içindekiler") == -1)
                    {
                        loglar.Add("Önsöz bulundu.");
                        //MessageBox.Show("bulundu.\n\n\n"+bulunan.ToString()+"\n\nsayfa no:"+sayfa.ToString()+"");
                        onsoz_yazi = bulunan.ToString();
                        //burada textboxa eklenecek
                        txt_onsoz_tumu.Text = "" + bulunan.ToString() + "\n\nSayfa no:" + sayfa.ToString() + "[Belge sayfası]";
                        onsoz_report = ""+Environment.NewLine+ "///////////////////////////////////////" + Environment.NewLine + "ÖNSÖZ" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
                        onsoz_report += "" + bulunan.ToString() + "" + Environment.NewLine + "" + Environment.NewLine + "Sayfa no:" + sayfa.ToString() + "[Belge sayfası]" + Environment.NewLine + "";
                        while (bulunan[onsoz_char]!='.')
                        {
                            onsoz_ilk += $""+bulunan[onsoz_char].ToString()+"";
                            onsoz_char++;
                        }
                        //MessageBox.Show("İlk cümle : \n"+onsoz_kelime.Length.ToString()+"");
                        for (int i = 0; i < onsoz_kelime.Length; i++)
                        {
                            if (onsoz_ilk.ToLower().Trim().ToString().IndexOf(onsoz_kelime[i]) != -1)
                            {
                                onsoz_durum = "VAR";
                                break;
                            }
                            else
                            {
                                onsoz_durum = "YOK";
                            }
                        }
                        loglar.Add("Önsöz durum :"+ onsoz_durum.ToString());
                        //MessageBox.Show(onsoz_durum.ToString());
                        lbl_onsoz_tes.Text = onsoz_durum.ToString();
                    }
                }
            }
        }
        void icindekiler()
        {
            if (File.Exists(dosya_yolu1))
            {
                loglar.Add("İçindekiler pdf okunuyor");
                PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                string icerik = "", bulunan = "", bulunan_k = "";
                for (int sayfa = 1; sayfa <= pdfOkuyucu.NumberOfPages; sayfa++)
                {
                    //loglar.Add("İçindekiler "+sayfa+". sayfa okunuyor");
                    bulunan = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                    bulunan_k = $"" + (PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)).ToLower().ToString() + "";
                    //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";


                    if (bulunan_k.IndexOf("içindekiler") != -1 && bulunan_k.IndexOf(".....") != -1)
                    {
                        loglar.Add("İçindekiler bulundu.");
                        //MessageBox.Show("bulundu.\n\n\n" + bulunan.ToString() + "\n\nsayfa no:" + sayfa1.ToString() + "");
                        icindekiler_sayfa = sayfa;
                        do
                        {
                            icindekiler_diger_icerik = PdfTextExtractor.GetTextFromPage(pdfOkuyucu, (icindekiler_sayfa));
                            icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, icindekiler_sayfa) + "";
                            icindekiler_sayfa++;
                            //icindekiler_diger_icerik = PdfTextExtractor.GetTextFromPage(pdfOkuyucu, (icindekiler_sayfa));
                        } while (icindekiler_diger_icerik.ToLower().ToString().Trim().IndexOf("özgeçmiş") == -1);

                        //icerik = icerik.Replace(".."," ");
                        //MessageBox.Show("" + icerik + "");
                        break;
                        //Burada noktaları buldurup siliyoruz.
                    }
                }


                /************************************************/
                /*
                 * Burada icerik[i] == '\n' komutunu if içinde yazarak ayırdığımız sayfayı kontrol ediyoruz ve 
                 * her '\n' olduğunda yani her bir yeni satıra geçildiğinde bizde oraya kadar olan yazıyı bir liste
                 * içine atacaz ve daha sonra liste içerisindeki tüm elemanları dolaşarak nokta silme işlemi yapacaz.
                 * Daha sonra ise noktaların yerine 1 tane boşluk bırakacaz. Boşluk burada ayırt edici birşey olacak.
                 * Başlıklar ile sayfa numaralarını da boşluk durumuna göre belirleyip daha sonra sayfa no larını başka
                 * bir diziye atabiliriz. Yada sayfa içinde arama işlemini de direk orada yaparız son 20 karakter içinde
                 * sayfa numarası varsa doğru deriz yoksa yanlış yerde yazarız.
                 */
                /************************************************/
                loglar.Add("İçindekiler satırlar okunuyor");
                for (int i = 0; i < icerik.Length; i++)
                {
                    if (icerik[i] == '\n')
                    {
                        //MessageBox.Show("" + icerik.Substring(satir_karakter, (i - satir_karakter)) + "");
                        icindekiler_baslik.Add("" + icerik.Substring(satir_karakter, (i - satir_karakter)).Trim().ToString() + "");
                        satir_karakter = i;
                        satir_s++;
                        //MessageBox.Show(satir_s.ToString());
                    }
                }
                loglar.Add("İçindekiler "+satir_s+" satır.");
                loglar.Add("İçindekiler sayfa numaralarıyla ayrılıyor.");
                for (int a = 0; a < icindekiler_baslik.Count; a++)
                {
                    //MessageBox.Show("" + icindekiler_baslik[a].ToString() + "");
                    for (int i = 0; i < icindekiler_baslik[a].Length; i++)
                    {
                        if (icindekiler_baslik[a][i].ToString() == $"." && icindekiler_baslik[a][i + 1].ToString() == $"." && icindekiler_baslik[a][i + 2].ToString() == $".")
                        {
                            nokta_bas = i;
                            //MessageBox.Show(nokta_bas.ToString());
                            for (int b = nokta_bas; b < icindekiler_baslik[a].Length; b++)
                            {
                                if (icindekiler_baslik[a][b].ToString() != ".")
                                {
                                    nokta_son = b;
                                    //MessageBox.Show(nokta_son.ToString());
                                    list_bas = icindekiler_baslik[a].Remove(nokta_bas, (nokta_son - nokta_bas));
                                    list_bas = list_bas.Insert(nokta_bas, "|");
                                    //MessageBox.Show(list_bas.ToString());
                                    icindekiler_b1.Add("" + list_bas.ToString().Trim() + "");
                                    //icindekiler_baslik.Add("" + list_bas.ToString() + "");
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                loglar.Add("İçindekiler sayfa numaralarından ayrıldı.");
                loglar.Add("Sayfa numaraları eşleştiriliyor.");
                icindekiler_tpl = "";
                string ic_baslik = "";
                string str_sayfa = "";
                for (int y = 0; y < icindekiler_b1.Count; y++)
                {
                    icindekiler_tpl += "" + icindekiler_b1[y].ToString() + "\n";
                    for (int t = icindekiler_b1[y].Trim().Length - 1; t > 0; t--)
                    {
                        if (icindekiler_b1[y][t] != '|')
                        {

                        }
                        else
                        {
                            ic_baslik = icindekiler_b1[y].Substring(0, t);
                            //MessageBox.Show(t.ToString());
                            //MessageBox.Show((icindekiler_b1[y].Trim().Length - 1).ToString());
                            str_sayfa = icindekiler_b1[y].Substring((t + 1), ((icindekiler_b1[y].Length) - (t + 1)));
                            break;
                        }
                    }
                    //MessageBox.Show("Başlık :\n" + ic_baslik.ToString().Trim() + "\nSayfa :\n"+ str_sayfa .ToString().Trim()+ "");
                    icindekiler_b1_baslik.Add(ic_baslik.ToString().Trim());
                    icindekiler_b1_sayfa.Add(str_sayfa.ToString().Trim());
                }
                //MessageBox.Show("" + icindekiler_tpl.ToString() + "");
            }
            loglar.Add("Sayfa numaraları eşleştirildi.");
            loglar.Add("İçindekiler sayfa tutarlılıkları kontrol ediliyor.");
            // Sayfanın son 20 harfi içini tarama
            if (File.Exists(dosya_yolu1))
            {
                PdfReader pdfOkuyucu1 = new PdfReader(dosya_yolu1);
                string bulunan = "", bulunan_son = "", bulunan_sonraki = "", bulunan_onceki = "", bulunan_son2 = "", bulunan_son3 = "";
                for (int i = 0; i < icindekiler_b1_baslik.Count; i++)
                {
                    for (int sayfa = 1; sayfa <= pdfOkuyucu1.NumberOfPages; sayfa++)
                    {
                        bulunan = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa) + "";
                        if (bulunan.Length > 0)
                        {
                            bulunan_son = $"" + bulunan.Substring((bulunan.Length) - 10, 10) + "";
                        }
                        //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";

                        if (bulunan.IndexOf("" + icindekiler_b1_baslik[i].Trim().ToString() + "") != -1 && bulunan_son.IndexOf("" + icindekiler_b1_sayfa[i].ToString() + "") != -1)
                        {
                            //Evet var
                            icindekiler_varyok.Add("Sayfa tutarlı.");
                            //icindekiler_pdf_sayfa.Add(""+sayfa.ToString()+"");
                            //MessageBox.Show(icindekiler_pdf_sayfa[sayfa-1].ToString());
                            icindekiler_durum.Add("" + icindekiler_b1_baslik[i].ToString() + " ----- " + icindekiler_b1_sayfa[i].ToString() + "  ----- " + icindekiler_varyok[i].ToString() + "");
                            break;
                        }
                        else
                        {
                            if (sayfa < pdfOkuyucu1.NumberOfPages)
                            {
                                bulunan_sonraki = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa + 1) + "";
                                if (sayfa > 1)
                                {
                                    bulunan_onceki = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa - 1) + "";
                                }
                                if (bulunan_sonraki.Length > 0)
                                {
                                    bulunan_son2 = $"" + bulunan_sonraki.Substring((bulunan_sonraki.Length) - 10, 10) + "";
                                }
                                if (bulunan_onceki.Length > 0)
                                {
                                    bulunan_son3 = $"" + bulunan_onceki.Substring((bulunan_onceki.Length) - 10, 10) + "";
                                }
                                try
                                {
                                    if (bulunan_sonraki.Length > 0 && bulunan_son2.IndexOf("" + (int.Parse(icindekiler_b1_sayfa[i]) + 1) + "") != -1)
                                    {
                                        icindekiler_varyok.Add("Sayfa tutarlı.");
                                        //icindekiler_pdf_sayfa.Add("" + sayfa.ToString() + "");
                                        //MessageBox.Show(icindekiler_pdf_sayfa[sayfa - 1].ToString());
                                        icindekiler_durum.Add("" + icindekiler_b1_baslik[i].ToString() + " ----- " + icindekiler_b1_sayfa[i].ToString() + "  ----- " + icindekiler_varyok[i].ToString() + "");
                                        break;
                                    }
                                    else if (sayfa > 1 && bulunan_onceki.Length > 0 && bulunan_son3.IndexOf("" + (int.Parse(icindekiler_b1_sayfa[i]) - 1) + "") != -1)
                                    {
                                        icindekiler_varyok.Add("Sayfa tutarlı.");
                                        //icindekiler_pdf_sayfa.Add("" + sayfa.ToString() + "");
                                        //MessageBox.Show(icindekiler_pdf_sayfa[sayfa - 1].ToString());
                                        icindekiler_durum.Add("" + icindekiler_b1_baslik[i].ToString() + " ----- " + icindekiler_b1_sayfa[i].ToString() + "  ----- " + icindekiler_varyok[i].ToString() + "");
                                        break;
                                    }
                                }
                                catch
                                {

                                }

                            }
                            else
                            {
                                icindekiler_varyok.Add("Sayfa tutarsız!");
                                //icindekiler_pdf_sayfa.Add("-");
                                icindekiler_durum.Add("" + icindekiler_b1_baslik[i].ToString() + " ----- " + icindekiler_b1_sayfa[i].ToString() + "  ----- " + icindekiler_varyok[i].ToString() + "");
                            }
                            //hayır yok
                        }
                        
                    }
                    //MessageBox.Show("" + icindekiler_durum[i].ToString() + "");
                    
                }
            }
            loglar.Add("Sayfa tutarlılıkları kontrol edildi.");
            loglar.Add("İçindekiler son işlem.");
            //string toplam_ic = "";
            icindekiler_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "İÇİNDEKİLER" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            for (int i = 0; i < icindekiler_durum.Count; i++)
            {
                //toplam_ic += ""+icindekiler_durum[i].ToString()+"";
                icindekiler_report += "" + icindekiler_durum[i].ToString() + "" + Environment.NewLine + "";
                list_icindekiler_tumu.Items.Add(""+icindekiler_durum[i].ToString()+"");
            }
            if (icindekiler_varyok.Contains("Sayfa tutarsız!")==true)
            {
                lbl_tutarsiz_sayfa.Text = "VAR";
            }
            else
            {
                lbl_tutarsiz_sayfa.Text = "YOK";
            }
            //MessageBox.Show(toplam_ic.ToString());
        }

        int tablo_icindekiler_index;
        string tablo_sonraki_icindekiler;
        string tablo_icindekiler_sonraki_index;
        void tablolar()
        {
            loglar.Add("Tablolar okunuyor");
            if (icindekiler_b1_baslik.Contains("TABLOLAR LİSTESİ"))
            {
                tablo_icindekiler_index = icindekiler_b1_baslik.IndexOf("TABLOLAR LİSTESİ");
                //MessageBox.Show("Bulundu\n"+ tablo_icindekiler_index.ToString() + "");
                tablo_sonraki_icindekiler = icindekiler_b1_baslik[tablo_icindekiler_index + 1].ToString();
                //MessageBox.Show("Bulundu\n" + tablo_sonraki_icindekiler.ToString() + "");
                tablo_icindekiler_sonraki_index = icindekiler_b1_sayfa[tablo_icindekiler_index + 1].ToString();
                //MessageBox.Show("Bulundu\n" + tablo_icindekiler_sonraki_index.ToString() + "");
            }
            else
            {
                MessageBox.Show("Bulunamadı.");
                tablo_icindekiler_index = -1;
                tablo_sonraki_icindekiler = "";
                tablo_icindekiler_sonraki_index = "";
            }


            if (File.Exists(dosya_yolu1))
            {
                PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                string icerik = "", bulunan = "", bulunan_k = "";
                for (int sayfa = 1; sayfa <= pdfOkuyucu.NumberOfPages; sayfa++)
                {
                    bulunan = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                    bulunan_k = $"" + (PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)).ToLower().ToString() + "";
                    //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";


                    if (bulunan_k.IndexOf("tablolar listesi") != -1 && bulunan_k.IndexOf("..") != -1&& bulunan_k.IndexOf("tablo ") != -1 && bulunan_k.IndexOf(""+tablo_sonraki_icindekiler.ToLower().ToString()+"") == -1)
                    {
                        loglar.Add("Tablolar listesi bulundu");
                        //MessageBox.Show("bulundu.\n\n\n" + bulunan.ToString() + "\n\nsayfa no:" + sayfa1.ToString() + "");
                        tablolar_sayfa = sayfa;
                        
                        do
                        {
                            //sekiller_diger_icerik = PdfTextExtractor.GetTextFromPage(pdfOkuyucu, (sekiller_sayfa)).ToLower().ToString();
                            
                            //MessageBox.Show(""+ PdfTextExtractor.GetTextFromPage(pdfOkuyucu, tablolar_sayfa) + "");
                            icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, tablolar_sayfa) + "";
                            tablolar_sayfa++;
                            //MessageBox.Show("sayfa "+tablolar_sayfa.ToString());
                            tablolar_diger_icerik = PdfTextExtractor.GetTextFromPage(pdfOkuyucu, (tablolar_sayfa)).ToLower().ToString();
                        } while (tablolar_diger_icerik.Substring(0, 5).ToLower().IndexOf("tablo") != -1);

                        //icerik = icerik.Replace(".."," ");
                        //MessageBox.Show("" + icerik + "");
                        break;
                        //Burada noktaları buldurup siliyoruz.
                    }
                }
                /*!!!!!Burada mantık hatası var while içerisinde yazılan fonksiyon diğer sayfayı hiç kontrol etmiyor.
                 sadece tek sayfaya bakıyor ve buna göre işlem yapıyor.
                 burada diğer sayfa yani tablolar_sayfa dan gelen verilere göre bir arama yapmalı
                 içerikler kontrol sayfası da güncellenmeli
                 */

                /************************************************/
                /*
                 * Burada icerik[i] == '\n' komutunu if içinde yazarak ayırdığımız sayfayı kontrol ediyoruz ve 
                 * her '\n' olduğunda yani her bir yeni satıra geçildiğinde bizde oraya kadar olan yazıyı bir liste
                 * içine atacaz ve daha sonra liste içerisindeki tüm elemanları dolaşarak nokta silme işlemi yapacaz.
                 * Daha sonra ise noktaların yerine 1 tane boşluk bırakacaz. Boşluk burada ayırt edici birşey olacak.
                 * Başlıklar ile sayfa numaralarını da boşluk durumuna göre belirleyip daha sonra sayfa no larını başka
                 * bir diziye atabiliriz. Yada sayfa içinde arama işlemini de direk orada yaparız son 20 karakter içinde
                 * sayfa numarası varsa doğru deriz yoksa yanlış yerde yazarız.
                 */
                /************************************************/
                
                loglar.Add("Tablolar satırlar okunuyor");
                for (int i = 0; i < icerik.Length; i++)
                {
                    if (icerik[i] == '\n')
                    {
                        //MessageBox.Show("" + icerik.Substring(tablolar_satir_karakter, (i - tablolar_satir_karakter)) + "");
                        tablolar_baslik.Add("" + icerik.Substring(tablolar_satir_karakter, (i - tablolar_satir_karakter)).Trim().ToString() + "");
                        tablolar_satir_karakter = i;
                        tablolar_satir_s++;
                        //MessageBox.Show(icerik.Substring(tablolar_satir_karakter, (i - tablolar_satir_karakter)).Trim().ToString());
                    }
                }
                loglar.Add("Tablolar " + tablolar_satir_s + " satır.");
                loglar.Add("Tablolar sayfa numaralarıyla ayrılıyor.");
                for (int a = 0; a < tablolar_baslik.Count; a++)
                {
                    //MessageBox.Show("" + tablolar_baslik[a].ToString() + "");
                    for (int i = 0; i < tablolar_baslik[a].Length; i++)
                    {
                        if (tablolar_baslik[a][i].ToString() == $"." && tablolar_baslik[a][i + 1].ToString() == $"." && tablolar_baslik[a][i + 2].ToString() == $".")
                        {
                            tablolar_nokta_bas = i;
                            //MessageBox.Show(nokta_bas.ToString());
                            for (int b = tablolar_nokta_bas; b < tablolar_baslik[a].Length; b++)
                            {
                                if (tablolar_baslik[a][b].ToString() != ".")
                                {
                                    tablolar_nokta_son = b;
                                    //MessageBox.Show(nokta_son.ToString());
                                    tablolar_list_bas = tablolar_baslik[a].Remove(tablolar_nokta_bas, (tablolar_nokta_son - tablolar_nokta_bas));
                                    tablolar_list_bas = tablolar_list_bas.Insert(tablolar_nokta_bas, "");
                                    //MessageBox.Show(list_bas.ToString());
                                    tablolar_b1.Add("" + tablolar_list_bas.ToString().Trim() + "");
                                    //icindekiler_baslik.Add("" + list_bas.ToString() + "");
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                loglar.Add("Tablolar sayfa numaralarından ayrıldı.");
                loglar.Add("Sayfa numaraları eşleştiriliyor.");
                
                tablolar_tpl = "";
                string ic_baslik = "";
                string str_sayfa = "";
                string basliklar_tpl = "";
                string sayfalar_tpl = "";
                for (int y = 0; y < tablolar_b1.Count; y++)
                {
                    tablolar_tpl += "" + tablolar_b1[y].ToString() + "\n";
                    for (int t = tablolar_b1[y].Trim().Length - 1; t > 0; t--)
                    {
                        if (tablolar_b1[y][t] != ' ')
                        {

                        }
                        else
                        {
                            ic_baslik = tablolar_b1[y].Substring(0, t);
                            //MessageBox.Show(t.ToString());
                            //MessageBox.Show((icindekiler_b1[y].Trim().Length - 1).ToString());
                            str_sayfa = tablolar_b1[y].Substring((t + 1), ((tablolar_b1[y].Length) - (t + 1)));
                            basliklar_tpl += ic_baslik+"\n";
                            sayfalar_tpl += str_sayfa+"\n";
                            break;
                        }
                    }
                    //MessageBox.Show("Başlık :\n" + ic_baslik.ToString().Trim() + "\nSayfa :\n"+ str_sayfa .ToString().Trim()+ "");
                    tablolar_b1_baslik.Add(ic_baslik.ToString().Trim());
                    tablolar_b1_sayfa.Add(str_sayfa.ToString().Trim());
                }
                //MessageBox.Show("" + tablolar_tpl.ToString() + "");
                //MessageBox.Show("" + basliklar_tpl.ToString() + "");
                //MessageBox.Show("" + sayfalar_tpl.ToString() + "");
            }
            loglar.Add("Sayfa numaraları eşleştirildi.");
            loglar.Add("Tablolar sayfa tutarlılıkları kontrol ediliyor.");
            MessageBox.Show("Tablolar sayfa tutarlılıkları kontrol ediliyor...");
            // Sayfanın son 20 harfi içini tarama
            if (File.Exists(dosya_yolu1))
            {
                PdfReader pdfOkuyucu1 = new PdfReader(dosya_yolu1);
                string bulunan = "", bulunan_son = "", bulunan_sonraki = "", bulunan_onceki = "", bulunan_son2 = "", bulunan_son3 = "";
                for (int i = 0; i < tablolar_b1_baslik.Count; i++)
                {
                    for (int sayfa = 1; sayfa <= pdfOkuyucu1.NumberOfPages; sayfa++)
                    {
                        bulunan = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa) + "";
                        if (bulunan.Length > 0)
                        {
                            bulunan_son = $"" + bulunan.Substring((bulunan.Length) - 10, 10) + "";
                        }
                        //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";

                        if (bulunan.IndexOf("" + tablolar_b1_baslik[i].Trim().ToString() + "") != -1 && bulunan_son.IndexOf("" + tablolar_b1_sayfa[i].ToString() + "") != -1)
                        {
                            //Evet var
                            tablolar_varyok.Add("Sayfa tutarlı.");
                            tablolar_durum.Add("" + tablolar_b1_baslik[i].ToString() + " ----- " + tablolar_b1_sayfa[i].ToString() + "  ----- " + tablolar_varyok[i].ToString() + "");
                            break;
                        }
                        else
                        {
                            if (sayfa < pdfOkuyucu1.NumberOfPages)
                            {
                                bulunan_sonraki = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa + 1) + "";
                                if (sayfa > 1)
                                {
                                    bulunan_onceki = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa - 1) + "";
                                }
                                if (bulunan_sonraki.Length > 0)
                                {
                                    bulunan_son2 = $"" + bulunan_sonraki.Substring((bulunan_sonraki.Length) - 10, 10) + "";
                                }
                                if (bulunan_onceki.Length > 0)
                                {
                                    bulunan_son3 = $"" + bulunan_onceki.Substring((bulunan_onceki.Length) - 10, 10) + "";
                                }
                                try
                                {
                                    if (bulunan_sonraki.Length > 0 && bulunan_son2.IndexOf("" + (int.Parse(tablolar_b1_sayfa[i]) + 1) + "") != -1)
                                    {
                                        tablolar_varyok.Add("Sayfa tutarlı.");
                                        tablolar_durum.Add("" + tablolar_b1_baslik[i].ToString() + " ----- " + tablolar_b1_sayfa[i].ToString() + "  ----- " + tablolar_varyok[i].ToString() + "");
                                        break;
                                    }
                                    else if (sayfa > 1 && bulunan_onceki.Length > 0 && bulunan_son3.IndexOf("" + (int.Parse(tablolar_b1_sayfa[i]) - 1) + "") != -1)
                                    {
                                        tablolar_varyok.Add("Sayfa tutarlı.");
                                        tablolar_durum.Add("" + tablolar_b1_baslik[i].ToString() + " ----- " + tablolar_b1_sayfa[i].ToString() + "  ----- " + tablolar_varyok[i].ToString() + "");
                                        break;
                                    }
                                }
                                catch
                                {

                                }

                            }
                            else
                            {
                                tablolar_varyok.Add("Sayfa tutarsız!");
                                tablolar_durum.Add("" + tablolar_b1_baslik[i].ToString() + " ----- " + tablolar_b1_sayfa[i].ToString() + "  ----- " + tablolar_varyok[i].ToString() + "");
                            }
                            //hayır yok
                        }
                    }
                    //MessageBox.Show("" + icindekiler_durum[i].ToString() + "");
                }
            }
            loglar.Add("Sayfa tutarlılıkları kontrol edildi.");
            loglar.Add("Tablolar son işlem.");
            //string toplam_ic = "";
            tablo_uyum_calistir();
            tablolar_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "TABLOLAR" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            for (int i = 0; i < tablolar_durum.Count; i++)
            {
                //toplam_ic += "" + tablolar_durum[i].ToString() + "";
                tablolar_report += "" + tablolar_durum[i].ToString() + " ----- " + tablo_uyum_sonuc[i].ToString() + "" + Environment.NewLine + "";
                list_tablolar_tumu.Items.Add("" + tablolar_durum[i].ToString() + " ----- "+tablo_uyum_sonuc[i].ToString()+"");
            }
            //MessageBox.Show(toplam_ic.ToString());
        }

        int sekil_icindekiler_index;
        string sekil_sonraki_icindekiler;
        string sekil_icindekiler_sonraki_index;
        void sekiller()
        {
            loglar.Add("Şekiller okunuyor");
            if (icindekiler_b1_baslik.Contains("ŞEKİLLER LİSTESİ"))
            {
                sekil_icindekiler_index = icindekiler_b1_baslik.IndexOf("ŞEKİLLER LİSTESİ");
                //MessageBox.Show("Bulundu\n"+ tablo_icindekiler_index.ToString() + "");
                sekil_sonraki_icindekiler = icindekiler_b1_baslik[sekil_icindekiler_index + 1].ToString();
                //MessageBox.Show("Bulundu\n" + tablo_sonraki_icindekiler.ToString() + "");
                sekil_icindekiler_sonraki_index = icindekiler_b1_sayfa[sekil_icindekiler_index + 1].ToString();
                //MessageBox.Show("Bulundu\n" + tablo_icindekiler_sonraki_index.ToString() + "");
            }
            else
            {
                MessageBox.Show("Bulunamadı.");
                sekil_icindekiler_index = -1;
                sekil_sonraki_icindekiler = "";
                sekil_icindekiler_sonraki_index = "";
            }


            if (File.Exists(dosya_yolu1))
            {
                PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                string icerik = "", bulunan = "", bulunan_k = "";
                for (int sayfa = 1; sayfa <= pdfOkuyucu.NumberOfPages; sayfa++)
                {
                    bulunan = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                    bulunan_k = $"" + (PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)).ToLower().ToString() + "";
                    //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";


                    if (bulunan_k.IndexOf("şekiller listesi") != -1 && bulunan_k.IndexOf("..") != -1 && bulunan_k.IndexOf("şekil ") != -1 && bulunan_k.IndexOf("" + sekil_sonraki_icindekiler.ToLower().ToString() + "") == -1)
                    {
                        loglar.Add("Şekiller listesi bulundu");
                        //MessageBox.Show("bulundu.\n\n\n" + bulunan.ToString() + "\n\nsayfa no:" + sayfa1.ToString() + "");
                        sekiller_sayfa = sayfa;
                        do
                        {
                            
                            //MessageBox.Show(""+ PdfTextExtractor.GetTextFromPage(pdfOkuyucu, tablolar_sayfa) + "");
                            icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sekiller_sayfa) + "";
                            sekiller_sayfa++;
                            sekiller_diger_icerik = PdfTextExtractor.GetTextFromPage(pdfOkuyucu, (sekiller_sayfa)).ToLower().ToString();
                            //MessageBox.Show("sayfa "+tablolar_sayfa.ToString());
                        } while (sekiller_diger_icerik.Substring(0,5).ToLower().IndexOf("şekil") != -1);

                        //icerik = icerik.Replace(".."," ");
                        //MessageBox.Show("" + icerik + "");
                        break;
                        //Burada noktaları buldurup siliyoruz.
                    }
                }
                /*!!!!!Burada mantık hatası var while içerisinde yazılan fonksiyon diğer sayfayı hiç kontrol etmiyor.
                 sadece tek sayfaya bakıyor ve buna göre işlem yapıyor.
                 burada diğer sayfa yani tablolar_sayfa dan gelen verilere göre bir arama yapmalı
                 içerikler kontrol sayfası da güncellenmeli
                 */

                /************************************************/
                /*
                 * Burada icerik[i] == '\n' komutunu if içinde yazarak ayırdığımız sayfayı kontrol ediyoruz ve 
                 * her '\n' olduğunda yani her bir yeni satıra geçildiğinde bizde oraya kadar olan yazıyı bir liste
                 * içine atacaz ve daha sonra liste içerisindeki tüm elemanları dolaşarak nokta silme işlemi yapacaz.
                 * Daha sonra ise noktaların yerine 1 tane boşluk bırakacaz. Boşluk burada ayırt edici birşey olacak.
                 * Başlıklar ile sayfa numaralarını da boşluk durumuna göre belirleyip daha sonra sayfa no larını başka
                 * bir diziye atabiliriz. Yada sayfa içinde arama işlemini de direk orada yaparız son 20 karakter içinde
                 * sayfa numarası varsa doğru deriz yoksa yanlış yerde yazarız.
                 */
                /************************************************/
                
                loglar.Add("Şekiller satırlar okunuyor");
                for (int i = 0; i < icerik.Length; i++)
                {
                    if (icerik[i] == '\n')
                    {
                        //MessageBox.Show("" + icerik.Substring(tablolar_satir_karakter, (i - tablolar_satir_karakter)) + "");
                        sekiller_baslik.Add("" + icerik.Substring(sekiller_satir_karakter, (i - sekiller_satir_karakter)).Trim().ToString() + "");
                        sekiller_satir_karakter = i;
                        sekiller_satir_s++;
                        //MessageBox.Show(icerik.Substring(tablolar_satir_karakter, (i - tablolar_satir_karakter)).Trim().ToString());
                    }
                }
                loglar.Add("Şekiller " + sekiller_satir_s + " satır.");
                loglar.Add("Şekiller sayfa numaralarıyla ayrılıyor.");
                for (int a = 0; a < sekiller_baslik.Count; a++)
                {
                    //MessageBox.Show("" + tablolar_baslik[a].ToString() + "");
                    for (int i = 0; i < sekiller_baslik[a].Length; i++)
                    {
                        if (sekiller_baslik[a][i].ToString() == $"." && sekiller_baslik[a][i + 1].ToString() == $"." && sekiller_baslik[a][i + 2].ToString() == $".")
                        {
                            sekiller_nokta_bas = i;
                            //MessageBox.Show(nokta_bas.ToString());
                            for (int b = sekiller_nokta_bas; b < sekiller_baslik[a].Length; b++)
                            {
                                if (sekiller_baslik[a][b].ToString() != ".")
                                {
                                    sekiller_nokta_son = b;
                                    //MessageBox.Show(nokta_son.ToString());
                                    sekiller_list_bas = sekiller_baslik[a].Remove(sekiller_nokta_bas, (sekiller_nokta_son - sekiller_nokta_bas));
                                    sekiller_list_bas = sekiller_list_bas.Insert(sekiller_nokta_bas, "");//önemli hata tablolarda
                                    //MessageBox.Show(list_bas.ToString());
                                    sekiller_b1.Add("" + sekiller_list_bas.ToString().Trim() + "");
                                    //icindekiler_baslik.Add("" + list_bas.ToString() + "");
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                loglar.Add("Şekiller sayfa numaralarından ayrıldı.");
                loglar.Add("Sayfa numaraları eşleştiriliyor.");
                
                sekiller_tpl = "";
                string ic_baslik = "";
                string str_sayfa = "";
                string basliklar_tpl = "";
                string sayfalar_tpl = "";
                for (int y = 0; y < sekiller_b1.Count; y++)
                {
                    sekiller_tpl += "" + sekiller_b1[y].ToString() + "\n";
                    for (int t = sekiller_b1[y].Trim().Length - 1; t > 0; t--)
                    {
                        if (sekiller_b1[y][t] != ' ')
                        {

                        }
                        else
                        {
                            ic_baslik = sekiller_b1[y].Substring(0, t);
                            //MessageBox.Show(t.ToString());
                            //MessageBox.Show((icindekiler_b1[y].Trim().Length - 1).ToString());
                            str_sayfa = sekiller_b1[y].Substring((t + 1), ((sekiller_b1[y].Length) - (t + 1)));
                            basliklar_tpl += ic_baslik + "\n";
                            sayfalar_tpl += str_sayfa + "\n";
                            break;
                        }
                    }
                    //MessageBox.Show("Başlık :\n" + ic_baslik.ToString().Trim() + "\nSayfa :\n"+ str_sayfa .ToString().Trim()+ "");
                    sekiller_b1_baslik.Add(ic_baslik.ToString().Trim());
                    sekiller_b1_sayfa.Add(str_sayfa.ToString().Trim());
                }
                //MessageBox.Show("" + tablolar_tpl.ToString() + "");
                //MessageBox.Show("" + basliklar_tpl.ToString() + "");
                //MessageBox.Show("" + sayfalar_tpl.ToString() + "");
            }
            loglar.Add("Sayfa numaraları eşleştirildi.");
            loglar.Add("Şekiller sayfa tutarlılıkları kontrol ediliyor.");
            MessageBox.Show("Şekiller sayfa tutarlılıkları kontrol ediliyor...");
            // Sayfanın son 20 harfi içini tarama
            if (File.Exists(dosya_yolu1))
            {
                PdfReader pdfOkuyucu1 = new PdfReader(dosya_yolu1);
                string bulunan = "", bulunan_son = "", bulunan_sonraki = "", bulunan_onceki = "", bulunan_son2 = "", bulunan_son3 = "";
                for (int i = 0; i < sekiller_b1_baslik.Count; i++)
                {
                    for (int sayfa = 1; sayfa <= pdfOkuyucu1.NumberOfPages; sayfa++)
                    {
                        bulunan = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa) + "";
                        if (bulunan.Length > 0)
                        {
                            bulunan_son = $"" + bulunan.Substring((bulunan.Length) - 10, 10) + "";
                        }
                        //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";

                        if (bulunan.IndexOf("" + sekiller_b1_baslik[i].Trim().ToString() + "") != -1 && bulunan_son.IndexOf("" + sekiller_b1_sayfa[i].ToString() + "") != -1)
                        {
                            //Evet var
                            sekiller_varyok.Add("Sayfa tutarlı.");
                            sekiller_durum.Add("" + sekiller_b1_baslik[i].ToString() + " ----- " + sekiller_b1_sayfa[i].ToString() + "  ----- " + sekiller_varyok[i].ToString() + "");
                            break;
                        }
                        else
                        {
                            if (sayfa < pdfOkuyucu1.NumberOfPages)
                            {
                                bulunan_sonraki = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa + 1) + "";
                                if (sayfa > 1)
                                {
                                    bulunan_onceki = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu1, sayfa - 1) + "";
                                }
                                if (bulunan_sonraki.Length > 0)
                                {
                                    bulunan_son2 = $"" + bulunan_sonraki.Substring((bulunan_sonraki.Length) - 10, 10) + "";
                                }
                                if (bulunan_onceki.Length > 0)
                                {
                                    bulunan_son3 = $"" + bulunan_onceki.Substring((bulunan_onceki.Length) - 10, 10) + "";
                                }
                                try
                                {
                                    if (bulunan_sonraki.Length > 0 && bulunan_son2.IndexOf("" + (int.Parse(sekiller_b1_sayfa[i]) + 1) + "") != -1)
                                    {
                                        sekiller_varyok.Add("Sayfa tutarlı.");
                                        sekiller_durum.Add("" + sekiller_b1_baslik[i].ToString() + " ----- " + sekiller_b1_sayfa[i].ToString() + "  ----- " + sekiller_varyok[i].ToString() + "");
                                        break;
                                    }
                                    else if (sayfa > 1 && bulunan_onceki.Length > 0 && bulunan_son3.IndexOf("" + (int.Parse(sekiller_b1_sayfa[i]) - 1) + "") != -1)
                                    {
                                        sekiller_varyok.Add("Sayfa tutarlı.");
                                        sekiller_durum.Add("" + sekiller_b1_baslik[i].ToString() + " ----- " + sekiller_b1_sayfa[i].ToString() + "  ----- " + sekiller_varyok[i].ToString() + "");
                                        break;
                                    }
                                }
                                catch
                                {

                                }

                            }
                            else
                            {
                                sekiller_varyok.Add("Sayfa tutarsız!");
                                sekiller_durum.Add("" + sekiller_b1_baslik[i].ToString() + " ----- " + sekiller_b1_sayfa[i].ToString() + "  ----- " + sekiller_varyok[i].ToString() + "");
                            }
                            //hayır yok
                        }
                    }
                    //MessageBox.Show("" + icindekiler_durum[i].ToString() + "");
                }
            }
            loglar.Add("Sayfa tutarlılıkları kontrol edildi.");
            loglar.Add("Şekiller son işlem.");
            //string toplam_ic = "";
            sekil_uyum_calistir();
            sekiller_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "ŞEKİLLER" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            for (int i = 0; i < sekiller_durum.Count; i++)
            {
                //toplam_ic += "" + sekiller_durum[i].ToString() + "";
                sekiller_report += "" + sekiller_durum[i].ToString() + " ----- " + sekil_uyum_sonuc[i].ToString() + "" + Environment.NewLine + "";
                list_sekiller_tumu.Items.Add("" + sekiller_durum[i].ToString() + " ----- "+sekil_uyum_sonuc[i].ToString()+"");
            }
            //MessageBox.Show(toplam_ic.ToString());
        }

        List<string> tablo_uyum_list = new List<string>();
        List<string> tablo_uyum_sonuc = new List<string>();
        List<string> tablo_uyum_sayfa = new List<string>();
        List<string> tablo_icindekiler_varyok = new List<string>();
        string tablo_bitti="";
        int bir_sonraki;
        void tablo_uyum()
        {
            loglar.Add("Tabloların uyumu kontrol ediliyor.");
            string tablo_sil = "";
            int nokta_sayac = 1;
            string tablo_durum = "";
            for (int i = 0; i < tablolar_b1_baslik.Count; i++)
            {
                tablo_sil = tablolar_b1_baslik[i].Replace("Tablo ","");
                //MessageBox.Show(tablo_sil.ToString());
                for (int a = 0; a < tablolar_b1_baslik[i].Length; a++)
                {
                    if (tablolar_b1_baslik[i][a].ToString()==" ")
                    {
                        tablo_sil = tablo_sil.Substring(0,a);
                        //MessageBox.Show(tablo_sil.ToString());
                        break;
                    }
                }
                for (int b = tablo_sil.Length-1; b > 0; b--)
                {
                    if (tablo_sil[b].ToString()==$"."&&nokta_sayac<2)
                    {
                        nokta_sayac++;
                    }
                    else
                    {
                        tablo_sil = tablo_sil.Substring(0,b);
                        nokta_sayac = 1;
                    }
                }
                //if (char.IsDigit(tablo_sil[0])==true&&tablo_sil.ToString().Trim()!="")
                //{
                    tablo_uyum_list.Add("" + tablo_sil.ToString() + "");
                //}
            }
            for (int i = 0; i < tablo_uyum_list.Count; i++)
            {
                tablo_durum += ""+ tablo_uyum_list[i].ToString() + "\n";
            }

            //MessageBox.Show("" + tablo_durum.ToString() + "");

            for (int i = 0; i < tablo_uyum_list.Count; i++)
            {
                for (int o = 0; o < icindekiler_b1_baslik.Count; o++)
                {
                    if (icindekiler_b1_baslik[o].Substring(0, tablo_uyum_list[i].Length).IndexOf(""+tablo_uyum_list[i].ToString()+"")!=-1)
                    {
                        tablo_uyum_sayfa.Add(""+icindekiler_b1_sayfa[o].ToString()+"");
                        tablo_icindekiler_varyok.Add("" + icindekiler_varyok[o].ToString() + "");
                        bir_sonraki = o+1;
                    }
                }
                tablo_uyum_sayfa.Add(""+icindekiler_b1_sayfa[bir_sonraki]+"");
                if (int.Parse(tablolar_b1_sayfa[i]) >= int.Parse(tablo_uyum_sayfa[0]) && int.Parse(tablolar_b1_sayfa[i]) <= int.Parse(tablo_uyum_sayfa[(tablo_uyum_sayfa.Count-1)]))
                {
                    if (tablolar_varyok[i] == "Sayfa tutarlı." && tablo_icindekiler_varyok[i] == "Sayfa tutarlı.")
                    {
                        tablo_uyum_sonuc.Add("Başlıkla Uyumlu");
                    }
                    else
                    {
                        tablo_uyum_sonuc.Add("Başlıkla Uyumsuz!");
                    }
                }
                else
                {
                    tablo_uyum_sonuc.Add("Başlıkla Uyumsuz!");
                }             
                
            }
            for (int a = 0; a < tablo_uyum_sonuc.Count; a++)
            {
                tablo_bitti += ""+ tablo_uyum_sonuc[a].ToString()+ "\n";
            }
            //MessageBox.Show(""+tablo_bitti.ToString()+"");
            loglar.Add("Tabloların uyumu kontrol edildi.");
        }

        List<string> sekil_uyum_list = new List<string>();
        List<string> sekil_uyum_sonuc = new List<string>();
        List<string> sekil_uyum_sayfa = new List<string>();
        List<string> sekil_icindekiler_varyok = new List<string>();
        string sekil_bitti = "";
        int sekil_bir_sonraki;
        void sekil_uyum()
        {
            loglar.Add("Şekiller uyumu kontrol ediliyor.");
            string sekil_sil = "";
            int nokta_sayac = 1;
            string sekil_durum = "";
            for (int i = 0; i < sekiller_b1_baslik.Count; i++)
            {
                sekil_sil = sekiller_b1_baslik[i].Replace("Şekil ", "");
                //MessageBox.Show(sekil_sil.ToString());
                for (int a = 0; a < sekiller_b1_baslik[i].Length; a++)
                {
                    if (sekiller_b1_baslik[i][a].ToString() == " ")
                    {
                        sekil_sil = sekil_sil.Substring(0, a);
                        //MessageBox.Show(sekil_sil.ToString());
                        break;
                    }
                }
                for (int b = sekil_sil.Length - 1; b > 0; b--)
                {
                    if (sekil_sil[b].ToString() == $"." && nokta_sayac < 2)
                    {
                        nokta_sayac++;
                    }
                    else
                    {
                        sekil_sil = sekil_sil.Substring(0, b);
                        nokta_sayac = 1;
                    }
                }
                //if (char.IsDigit(tablo_sil[0])==true&&tablo_sil.ToString().Trim()!="")
                //{
                sekil_uyum_list.Add("" +sekil_sil.ToString() + "");
                //}
            }
            for (int i = 0; i < sekil_uyum_list.Count; i++)
            {
                sekil_durum += "" + sekil_uyum_list[i].ToString() + "\n";
            }

            //MessageBox.Show("" + sekil_durum.ToString() + "");

            for (int i = 0; i < sekil_uyum_list.Count; i++)
            {
                for (int o = 0; o < icindekiler_b1_baslik.Count; o++)
                {
                    if (icindekiler_b1_baslik[o].Substring(0, sekil_uyum_list[i].Length).IndexOf("" + sekil_uyum_list[i].ToString() + "") != -1)
                    {
                        sekil_uyum_sayfa.Add("" + icindekiler_b1_sayfa[o].ToString() + "");
                        sekil_icindekiler_varyok.Add(""+ icindekiler_varyok[o].ToString() + "");
                        sekil_bir_sonraki = o + 1;
                    }
                }
                sekil_uyum_sayfa.Add("" + icindekiler_b1_sayfa[sekil_bir_sonraki] + "");
                if (int.Parse(sekiller_b1_sayfa[i]) >= int.Parse(sekil_uyum_sayfa[0]) && int.Parse(sekiller_b1_sayfa[i]) <= int.Parse(sekil_uyum_sayfa[(sekil_uyum_sayfa.Count - 1)]))
                {
                    if (sekiller_varyok[i] == "Sayfa tutarlı." && sekil_icindekiler_varyok[i] == "Sayfa tutarlı.")
                    {
                        sekil_uyum_sonuc.Add("Başlıkla Uyumlu");
                    }
                    else
                    {
                        sekil_uyum_sonuc.Add("Başlıkla Uyumsuz!");
                    }
                }
                else
                {
                    sekil_uyum_sonuc.Add("Başlıkla Uyumsuz!");
                }

            }
            for (int a = 0; a < sekil_uyum_sonuc.Count; a++)
            {
                sekil_bitti += "" + sekil_uyum_sonuc[a].ToString() + "\n";
            }
            //MessageBox.Show("" + sekil_bitti.ToString() + "");
            loglar.Add("Şekiller uyumu kontrol edildi.");
        }
        List<string> denklem_uyum_sonuc = new List<string>();
        List<string> denklem_pdf_sayfa = new List<string>();
        List<string> denklem_noktali = new List<string>();
        List<string> denklem_denklemler = new List<string>();
        List<int> baslangic_s = new List<int>();
        List<int> bitis_s = new List<int>();
        string denklem_sonucu = "";
        void denklem_kontrol()
        {
            loglar.Add("Denklem uyumları kontrol ediliyor.");
            if (File.Exists(dosya_yolu1))
            {
                PdfReader pdfOkuyucu1 = new PdfReader(dosya_yolu1);
                for (int i = 0; i < icindekiler_b1_baslik.Count; i++)
                {
                    for (int a = 0; a < icindekiler_b1_baslik.Count; a++)
                    {
                        if (icindekiler_b1_baslik[a].IndexOf("" + (i + 1) + ". ", 0, (icindekiler_b1_baslik[a].Length - 1)) == 0)
                        {
                            denklem_uyum_sonuc.Add("" + icindekiler_b1_baslik[a].ToString() + "");
                            //denklem_pdf_sayfa.Add(""+ icindekiler_pdf_sayfa[a].ToString() + "");
                            denklem_noktali.Add("" + (i + 1) + ".");
                        }

                    }
                }
                //burada başlanıçlar da alındı
                if (File.Exists(dosya_yolu1))
                {
                    PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                    string bulunan_k_d = "";
                    for (int sayfa = 1; sayfa <= pdfOkuyucu.NumberOfPages; sayfa++)
                    {
                        //bulunan_d = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                        bulunan_k_d = $"" + (PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)).ToLower().ToString() + "";
                        //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";

                        for (int i = 0; i < denklem_uyum_sonuc.Count; i++)
                        {
                            if (bulunan_k_d.IndexOf("" + denklem_uyum_sonuc[i].ToLower().ToString() + "") != -1)
                            {
                                //MessageBox.Show("bulundu.\n\n\n"+bulunan.ToString()+"\n\nsayfa no:"+sayfa1.ToString()+"");
                                if (bulunan_k_d.IndexOf("......") == -1)
                                {
                                    baslangic_s.Add(sayfa);
                                    break;
                                }
                            }
                        }
                    }
                }
                //bitişler tamamlandı
                for (int a = 1; a < baslangic_s.Count + 1; a++)
                {
                    if (a < baslangic_s.Count)
                    {
                        bitis_s.Add(baslangic_s[a]);
                    }
                    else
                    {
                        bitis_s.Add(pdfOkuyucu1.NumberOfPages);
                    }
                }

                //kontrol zamanı

                if (File.Exists(dosya_yolu1))
                {
                    PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                    string bulunan_k_d_b = "";
                    for (int t = 0; t < denklem_uyum_sonuc.Count; t++)
                    {
                        for (int sayfa = baslangic_s[t]; sayfa < bitis_s[t]; sayfa++)
                        {
                            //bulunan_d = $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                            bulunan_k_d_b = $"" + (PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)).ToLower().ToString() + "";
                            //icerik += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                            if (bulunan_k_d_b.IndexOf("denklem ")!=-1)
                            {
                                for (int i = 0; i < denklem_noktali.Count; i++)
                                {
                                    if (t==i)
                                    {

                                    }
                                    else
                                    {
                                        if ((bulunan_k_d_b.IndexOf("denklem " + denklem_noktali[i] + "") != -1))
                                        {
                                            denklem_denklemler.Add("" + sayfa.ToString() + ". sayfada uyumsuz denklem bulunuyor.[Belge sayfası]");
                                            break;
                                        }
                                    }
                                }
                                
                                //MessageBox.Show(denklem_denklemler.ToString());
                            }
                        }
                    }
                }
                /*  kaynaklardaki arama gibi arama yapılacak her başlık için ve sayfa numaraları bulunacak. daha sonra
                    listeye eklenecek sayfa numaraları.
                    eğer sayfa numaraları listedeki son elemana denk gelirse bitişi pdf bitiş sayfası olarak verilecek
                    oda kodlar arasında var .
             */

            }
            denklemler_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "DENKLEMLER" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            for (int i = 0; i < denklem_denklemler.Count; i++)
            {
                denklem_sonucu += ""+ denklem_denklemler[i] + "\n";
                denklemler_report += "" + denklem_denklemler[i].ToString() + "" + Environment.NewLine + "";
                list_denklemler_tumu.Items.Add("" + denklem_denklemler[i].ToString() + "");
                //denklem_sonucu += "" + denklem_uyum_sonuc[i].ToString() + "\n"+baslangic_s[i].ToString()+"----"+bitis_s[i]+"\n"+denklem_noktali[i]+"\n+++++++++++++\n";
            }
            //MessageBox.Show(denklem_sonucu.ToString());
            loglar.Add("Denklem uyumları kontrol edildi.");
        }
        byte[] kaynak_atif1;
        void kaynak_sayisi()
        {
            loglar.Add("Kaynaklar belirleniyor.");
            if (File.Exists(dosya_yolu1))
            {
                loglar.Add("Kaynaklar pdf okunuyor.");
                PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                string icerik = "",bulunan="",bulunan_k="";
                for (int sayfa = 1; sayfa <= pdfOkuyucu.NumberOfPages; sayfa++)
                {
                    bulunan = $""+PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)+"";
                    bulunan_k = $""+(PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)).ToLower().ToString()+"";
                    icerik += $""+PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa)+"";
                    //MessageBox.Show("" + bulunan.ToString() + "");
                    //for (int i = bulunan.Length; i > bulunan.Length-4; i--)
                    //{
                    //    MessageBox.Show("" + bulunan[i-1].ToString() + "");
                    //}
                    
                    if (bulunan_k.IndexOf("kaynaklar")!=-1)
                    {
                        //MessageBox.Show("bulundu.\n\n\n"+bulunan.ToString()+"\n\nsayfa no:"+sayfa1.ToString()+"");
                        if (bulunan_k.IndexOf("[1]") != -1&& bulunan_k.IndexOf("[2]") != -1 && bulunan_k.IndexOf("[3]") != -1)
                        {
                            loglar.Add("Kaynaklar bulundu");
                            loglar.Add("Kaynak sayısı belirleniyor.");
                            //MessageBox.Show("evet kaynaklar burasıdır hehe :) ");
                            sayfa_kaynak = sayfa1;
                            while (true)
                            {
                                if (bulunan_k.IndexOf("["+kaynak_s+"]") != -1)
                                {
                                    kaynak_s++;
                                }
                                else
                                {
                                    sayfa_kaynak++;
                                    bulunan = PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa_kaynak);
                                    bulunan_k = (PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa_kaynak)).ToLower().ToString();
                                    //MessageBox.Show("sonraki sayfa:\n\n"+bulunan.ToString()+"");
                                    if (bulunan_k.IndexOf("[" + kaynak_s + "]") != -1)
                                    {
                                        kaynak_s++;
                                    }
                                    else
                                    {
                                        kaynak_s--;
                                        //MessageBox.Show("kaynak sayısı : "+kaynak_s.ToString()+"");
                                        lbl_kaynak_s_sonuc.Text = kaynak_s.ToString();



                                        for (int i = sayfa1; i < sayfa_kaynak; i++)
                                        {
                                            tum_kaynaklar += ""+ PdfTextExtractor.GetTextFromPage(pdfOkuyucu, i).ToString() + "";
                                            
                                        }
                                        kaynak_atif1 = new byte[kaynak_s];
                                        for (int i = 0; i < kaynak_s; i++)
                                        {
                                            kaynak_atif1[i] = 0;
                                        }
                                        //buradan sonra kaynak atıfları kontrol edilecek.
                                        //çünkü sayfa 1 den sonra asıl kaynaklar başlıyor
                                        for (int a = 1; a < sayfa1; a++)
                                        {
                                            atif1 = PdfTextExtractor.GetTextFromPage(pdfOkuyucu, a);
                                            for (int c = 1; c < kaynak_s; c++)
                                            {
                                                if (atif1.IndexOf("["+c.ToString()+"]")!=-1)
                                                {
                                                    kaynak_atif1[(c - 1)] += 1;
                                                    //continue;
                                                }
                                            }

                                        }
                                        for (int i = 0; i < kaynak_atif1.Length; i++)
                                        {
                                            if (kaynak_atif1[i]==0)
                                            {
                                                atif_durum = "VAR";
                                                break;
                                            }
                                            else
                                            {
                                                atif_durum = "YOK";
                                            }
                                        }
                                        lbl_kaynak_atif_sonuc.Text = atif_durum.ToString();
                                        //aynı sayfada olan atıflar 1 tane farklı sayfa olduğundan +1 olarak sayılır.
                                        for (int i = 1; i < kaynak_s; i++)
                                        {
                                            att1 += "Kaynak["+i+"] = "+kaynak_atif1[(i-1)].ToString()+"\n";
                                        }
                                        //MessageBox.Show(""+att1.ToString()+""); //Kaynakların atıfları
                                        //MessageBox.Show("kaynaklar göster:\n"+tum_kaynaklar.ToString()+""); //Kaynakların atıfları
                                        break;

                                    }
                                }
                            }
                            loglar.Add("Kaynak sayısı belirlendi.");
                        }
                        else
                        {
                            //MessageBox.Show("hayır");
                            //MessageBox.Show(""+bulunan_k+"\n");
                        }
                    }
                    sayfa1++;
                }
                pdfOkuyucu.Close();
            }
            else
            {
                MessageBox.Show("Dosya bulunamadı");
            }
            loglar.Add("Kaynak işlemleri tamamlandı.");
        }
        List<string> kaynaklar_ayri = new List<string>();
        int kaynaklar_ayri_basla, kaynaklar_ayri_bitis;
        void kaynak_ayir()
        {
            loglar.Add("Kaynak ayrıştırılıyor.");
            for (int i = 1; i <= kaynak_s; i++)
            {
                if (i==kaynak_s)
                {
                    //burada [son kaynaktan] ten lenght e kadar alınacak ve list e aktarılacak
                    kaynaklar_ayri_basla = tum_kaynaklar.IndexOf("[" + i.ToString() + "]");
                    kaynaklar_ayri_bitis = tum_kaynaklar.Length;
                }
                else
                {
                    //burada [i] den [i+1] e kadar ve list e aktarılacak 
                    kaynaklar_ayri_basla = tum_kaynaklar.IndexOf("["+i.ToString()+"]");
                    kaynaklar_ayri_bitis= tum_kaynaklar.IndexOf("[" + (i+1).ToString() + "]");
                }
                //ayrilmis_b1_kaynak = tum_kaynaklar.Substring(kaynaklar_ayri_basla, (kaynaklar_ayri_bitis- kaynaklar_ayri_basla));
                //MessageBox.Show(i.ToString()+"\n"+kaynaklar_ayri_basla+"\n"+ kaynaklar_ayri_bitis+"\n"+ ayrilmis_b1_kaynak);
                kaynaklar_ayri.Add(""+ tum_kaynaklar.Substring(kaynaklar_ayri_basla, (kaynaklar_ayri_bitis - kaynaklar_ayri_basla)) + "");
            }
            //for (int i = 0; i < kaynaklar_ayri.Count; i++)
            //{
            //    ayri_kaynak_mesaj += kaynaklar_ayri[i].ToString() + "\n";
            //}
            //MessageBox.Show(ayri_kaynak_mesaj.ToString());


            //burada listeye yazılacak
            kaynaklar_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "KAYNAKLAR" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            for (int i = 0; i < kaynaklar_ayri.Count; i++)
            {
                kaynaklar_report += "" + kaynaklar_ayri[i].ToString() + " " + Environment.NewLine + "----- Atıf sayısı[Sayfa] = " + kaynak_atif1[i].ToString() + "" + Environment.NewLine + "" + Environment.NewLine + "";
                list_kaynaklar_tumu.Items.Add(""+ kaynaklar_ayri[i].ToString() + " ----- Atıf sayısı[Sayfa] = "+kaynak_atif1[i].ToString()+"");
            }


            loglar.Add("Kaynak ayrışma tamamlandı.");
        }
        string giris_ayir_str = "";
        void giris_ayir()
        {
            loglar.Add("Giriş metni ayrılıyor.");
            if (File.Exists(dosya_yolu1))
            {
                PdfReader pdfOkuyucu = new PdfReader(dosya_yolu1);
                //denklem_uyum_sonuc

                for (int sayfa = baslangic_s[0]; sayfa < bitis_s[0]; sayfa++)
                {
                    giris_ayir_str += $"" + PdfTextExtractor.GetTextFromPage(pdfOkuyucu, sayfa) + "";
                }
                //MessageBox.Show(baslangic_s[0].ToString()+"\n"+ bitis_s[0].ToString());
                //MessageBox.Show(giris_ayir_str.ToString());

                //burada giriş text yazdır
                txt_giris_tumu.Text = ""+ giris_ayir_str.ToString() + "";

                giris_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "GİRİŞ BÖLÜMÜ" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
                giris_report += "" + giris_ayir_str.ToString() + "" + Environment.NewLine + "";


            }
            loglar.Add("Giriş metni ayrıldı.");
        }
        int count = 0,count_kelime=0;
        void pdf_ara()
        {
            loglar.Add("Çift tırnak kelime sayısı belirleniyor.");
            count = 0;
            try
            {
                using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                {
                    documentProcessor.LoadDocument(dosya_yolu1);
                    while (documentProcessor.FindText("“").Status == PdfTextSearchStatus.Found)
                    {
                        count++;
                    }
                }
            }
            catch { }
            //MessageBox.Show(count+" adet bulundu.");
            pdf_ara2();
        }
        void pdf_ara2()
        {
            try
            {
                using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                {
                    documentProcessor.LoadDocument(dosya_yolu1);
                    while (documentProcessor.FindText("\"").Status == PdfTextSearchStatus.Found)
                    {
                        count++;
                    }
                }
                
            }
            catch { }
            count_kelime = count / 2;
            //MessageBox.Show(count_kelime + " adet bulundu.");
            lbl_tirnak_ici_sonuc.Text = count_kelime.ToString();

            


            loglar.Add("Çift tırnak kelime sayısı belirlendi");
        }
        string word_yolu = "";
        string veri = "";
        List<string> iki_satir_az = new List<string>();
        void temizle()
        {
            list_denklemler_tumu.Items.Clear();
            list_icindekiler_tumu.Items.Clear();
            list_kaynaklar_tumu.Items.Clear();
            list_sekiller_tumu.Items.Clear();
            list_tablolar_tumu.Items.Clear();
            txt_iki_satir_tumu.Text = "";
            txt_onsoz_tumu.Text = "";
            txt_giris_tumu.Text = "";
            txt_loglar_tumu.Text = "";
        }
        void log_yazdir()
        {
            string str_log="";
            loglar_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "LOGLAR" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            for (int i = 0; i < loglar.Count; i++)
            {
                str_log += "["+(i+1).ToString()+"] --- "+loglar[i].ToString()+ " ---   " + Environment.NewLine + "";
                loglar_report += "[" + (i + 1).ToString() + "] --- " + loglar[i].ToString() + " ---   " + Environment.NewLine + "";
            }
            txt_loglar_tumu.Text = ""+ str_log.ToString() + "";
        }
        void isaretle()
        {
            loglar.Add("İki satırdan az paragraf bulunuyor.");
            string varmi = dosya_yolu1.Replace(".pdf", ".docx");
            //MessageBox.Show(varmi.ToString());
            string dosya_dizini = @"" + varmi.ToString() + "";
            if (File.Exists(dosya_dizini) == true) // dizindeki dosya var mı ?
            {
                word_yolu = dosya_dizini;
                //MessageBox.Show("var");

                object fileName = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "" + word_yolu.ToString() + "");
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application { Visible = false };
                Microsoft.Office.Interop.Word.Document aDoc = wordApp.Documents.Open(fileName, ReadOnly: false, Visible: false);
                try
                {
                    aDoc.Activate();
                }
                catch
                {

                }

                //foreach (Microsoft.Office.Interop.Word.Range docRange in aDoc.Words)
                //{

                //    if (docRange.Text.Trim().Equals("“",
                //       StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        docRange.HighlightColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                //        docRange.Font.ColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;
                //    }
                //    if (docRange.Text.Trim().Equals("\"",
                //       StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        docRange.HighlightColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                //        docRange.Font.ColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;
                //    }
                //    if (docRange.Text.Trim().Equals("”",
                //       StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        docRange.HighlightColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                //        docRange.Font.ColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;
                //    }
                //}

                aDoc.SaveAs2(word_yolu.ToString());
                aDoc.Close();
            }
            else
            {
                word_yolu = dosya_dizini;


                SautinSoft.PdfFocus pf = new SautinSoft.PdfFocus();
                pf.OpenPdf(@"" + dosya_yolu1.ToString() + "");
                if (pf.PageCount > 0)
                {
                    pf.WordOptions.Format = SautinSoft.PdfFocus.CWordOptions.eWordDocument.Docx;
                    pf.ToWord(@"" + word_yolu.ToString() + "");

                }
                pf.ClosePdf();
                //MessageBox.Show(word_yolu.ToString());
                object fileName = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "" + word_yolu.ToString() + "");
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application { Visible = false };
                Microsoft.Office.Interop.Word.Document aDoc = wordApp.Documents.Open(fileName, ReadOnly: false, Visible: false);
                try
                {
                    aDoc.Activate();
                }
                catch
                {

                }

                //aDoc.Activate();
                //Sautinsoft reklamı sil
                int shapess = aDoc.Shapes.Count;
                aDoc.Shapes[shapess].Delete();

                //foreach (Microsoft.Office.Interop.Word.Range docRange in aDoc.Words)
                //{
                //    if (docRange.Text.Trim().Equals("“",
                //      StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        docRange.HighlightColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                //        docRange.Font.ColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;
                //    }
                //    if (docRange.Text.Trim().Equals("\"",
                //       StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        docRange.HighlightColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                //        docRange.Font.ColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;
                //    }
                //    if (docRange.Text.Trim().Equals("”",
                //       StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        docRange.HighlightColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                //        docRange.Font.ColorIndex =
                //          Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;
                //    }
                //}


                aDoc.SaveAs2(word_yolu.ToString());
                aDoc.Close();


            }

            string filePath = @"" + word_yolu.ToString() + "";
            DocumentCore dc = DocumentCore.Load(filePath);
            foreach (SautinSoft.Document.Paragraph par in dc.GetChildElements(true, ElementType.Paragraph))
            {
                if (par.Content.ToString().Trim().IndexOf("GİRİŞ") != -1)
                {
                    cc++;
                }
                else
                {
                    if (cc > 1)
                    {
                        if (par.Content.ToString().Trim().IndexOf("KAYNAKLAR") != -1 && par.ParagraphFormat.Alignment == SautinSoft.Document.HorizontalAlignment.Left)
                        {
                            break;
                        }
                        if (par.ParagraphFormat.Alignment == SautinSoft.Document.HorizontalAlignment.Justify)
                        {
                            if (par.Content.ToString().Trim().Length > 50 && par.Content.ToString().Trim().Length < 200
                            && par.Content.ToString().Trim() != "" &&
                            Char.IsDigit(par.Content.ToString().Trim()[0]) != true &&
                            par.Content.ToString().Trim() != par.Content.ToString().Trim().ToUpper() &&
                            par.Content.ToString().Trim().Substring(0, 5) != "Tablo" && par.Content.ToString().Trim().Substring(0, 5) != "Şekil" && par.Content.ToString().Trim().Substring(0, 5) != "Trial" && par.Content.ToString().Trim().Substring(0, 6) != "Denklem"
                            )
                            {
                                //MessageBox.Show(docs.Paragraphs[i + 1].Range.Text.ToString());
                                //par.ParagraphFormat.Borders.Add(MultipleBorderTypes.All, SautinSoft.Document.BorderStyle.Single, SautinSoft.Document.Color.Red, 5f);
                                veri += "" + Environment.NewLine + "" + par.Content.ToString() + ""+Environment.NewLine+ " --------------------------------- " + Environment.NewLine + "";
                                iki_satir_az.Add(""+ par.Content.ToString() + "");
                                pp++;

                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            //MessageBox.Show(veri.ToString());
            txt_iki_satir_tumu.Text = ""+ veri.ToString() + "";
            iki_satir_report = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "İKİ SATIRDAN AZ PARAGRAF" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            iki_satir_report += "" + veri.ToString() + "" + Environment.NewLine + "";
            //MessageBox.Show(satir_iki.ToString());
            lbl_iki_satir_sonuc.Text = pp.ToString();
            //string str = GetTextFromWord();
            //MessageBox.Show(pp.ToString());
            loglar.Add("İki satırdan az paragraf bulundu.");
        }

        string report_tumu = "";
        void tum_report()
        {
            

            report_tumu = "" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "DASHBOARD" + Environment.NewLine + "///////////////////////////////////////" + Environment.NewLine + "";
            report_tumu += "Kaynak Sayısı = "+lbl_kaynak_s_sonuc.Text.ToString()+ "" + Environment.NewLine + "";
            report_tumu += "Kaynak Atıf İhlali = " + lbl_kaynak_atif_sonuc.Text.ToString() + "" + Environment.NewLine + "";
            report_tumu += "Önsöz Teşekkür = " + lbl_onsoz_tes.Text.ToString() + "" + Environment.NewLine + "";
            report_tumu += "İki Satırdan Az Paragraf = " + lbl_iki_satir_sonuc.Text.ToString() + "" + Environment.NewLine + "";
            report_tumu += "Tırnak İçi Cümle Sayısı = " + lbl_tirnak_ici_sonuc.Text.ToString() + "" + Environment.NewLine + "";
            report_tumu += "Tutarsız Sayfa = " + lbl_tutarsiz_sayfa.Text.ToString() + "" + Environment.NewLine + "";
            report_tumu += icindekiler_report.ToString();
            report_tumu += tablolar_report.ToString();
            report_tumu += sekiller_report.ToString();
            report_tumu += denklemler_report.ToString();
            report_tumu += kaynaklar_report.ToString();
            report_tumu += onsoz_report.ToString();
            report_tumu += iki_satir_report.ToString();
            report_tumu += giris_report.ToString();
            report_tumu += loglar_report.ToString();
        }

        private void btn_tekrar_tara_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Analiz işlemine başlıyor.\nLütfen bekleyiniz...\nAnaliz süresi tahmini sayfa başına 1.4(sn)");
            temizle();
            MessageBox.Show("Kaynak sayısı,atıfları taranıyor...");
            kaynak_sayisi_calistir();
            kaynak_ayir_calistir();
            MessageBox.Show("Çift tırnaklar taranıyor...");
            pdf_ara_calistir();
            MessageBox.Show("İki satırdan az paragraflar taranıyor...");
            isaretle_calistir();
            MessageBox.Show("Önsöz taranıyor...");
            onsoz_calistir();//word e çevirme yapılıyor.
            MessageBox.Show("İçindekiler,başlıkları,sayfa numaraları taranıyor...");
            icindekiler_calistir();
            MessageBox.Show("Tablolar,başlıkları,sayfa numaraları taranıyor...");
            tablolar_calistir();
            //tablo_uyum_calistir();//buradan itibaren devam edecek
            MessageBox.Show("Şekiller,başlıkları,sayfa numaraları taranıyor...");
            sekiller_calistir();
            //sekil_uyum_calistir();
            MessageBox.Show("Denklemler taranıyor...");
            denklem_kontrol_calistir();
            MessageBox.Show("Giriş bölümü taranıyor...");
            giris_ayir_calistir();
            MessageBox.Show("Loglar oluşturuluyor...");
            log_yazdir();
            MessageBox.Show("TÜM ANALİZLER TAMAMLANDI.\nSAĞ KISIMDAN SONUÇLARA GÖZATABİLİRSİNİZ.");

            

        }

        void kaynak_sayisi_calistir()
        {
            try
            {
                kaynak_sayisi();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Kaynak işlemleri hatası "+a.ToString()+"");
            }
        }

        void kaynak_ayir_calistir()
        {
            try
            {
                kaynak_ayir();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Kaynak ayırma işlemleri hatası " + a.ToString() + "");
            }
        }

        void pdf_ara_calistir()
        {
            try
            {
                pdf_ara();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Çift tırnak işlemleri hatası " + a.ToString() + "");
            }
        }

        void isaretle_calistir()
        {
            try
            {
                isaretle();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("İki satırdan az paragraf işlemleri hatası " + a.ToString() + "");
            }
        }

        void onsoz_calistir()
        {
            try
            {
                onsoz();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Önsöz işlemleri hatası " + a.ToString() + "");
            }
        }

        void icindekiler_calistir()
        {
            try
            {
                icindekiler();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("İçindekiler işlemleri hatası " + a.ToString() + "");
            }
        }

        void tablolar_calistir()
        {
            try
            {
                tablolar();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Tablolar işlemleri hatası " + a.ToString() + "");
            }
        }

        void tablo_uyum_calistir()
        {
            try
            {
                tablo_uyum();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Tablo uyumu işlemleri hatası " + a.ToString() + "");
            }
        }

        void sekiller_calistir()
        {
            try
            {
                sekiller();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Şekil işlemleri hatası " + a.ToString() + "");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //burada txt dosyasına veri yazılıp kayıt edilecek 
            //burada txt dosyasına yazma var
            tum_report();
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Metin Dosyası|*.txt";
            save.OverwritePrompt = true;
            save.CreatePrompt = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter Kayit = new StreamWriter(save.FileName);
                Kayit.WriteLine(report_tumu.ToString());
                Kayit.Close();
            }
        }

        void sekil_uyum_calistir()
        {
            try
            {
                sekil_uyum();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Şekil uyum işlemleri hatası " + a.ToString() + "");
            }
        }

        void denklem_kontrol_calistir()
        {
            try
            {
                denklem_kontrol();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Denklem işlemleri hatası " + a.ToString() + "");
            }
        }

        void giris_ayir_calistir()
        {
            try
            {
                giris_ayir();
                //burada log liste başarılı yazılacak
            }
            catch (Exception a)
            {
                //burada log liste hata yazdırılacak
                loglar.Add("Giriş metni işlemleri hatası " + a.ToString() + "");
            }
        }

        
        int pp = 0,cc=0;
        private void frmAnaliz_Load(object sender, EventArgs e)
        {
            dosya_yolu1 = Form1.dosya_yolu.ToString();
            this.pdfViewer1.LoadDocument(@""+Form1.dosya_yolu.ToString()+"");
            MessageBox.Show("Analiz işlemine başlıyor.\nLütfen bekleyiniz...\nAnaliz süresi tahmini sayfa başına 1.4(sn)");
            MessageBox.Show("Kaynak sayısı,atıfları taranıyor...");
            kaynak_sayisi_calistir();
            kaynak_ayir_calistir();
            MessageBox.Show("Çift tırnaklar taranıyor...");
            pdf_ara_calistir();
            MessageBox.Show("İki satırdan az paragraflar taranıyor...");
            isaretle_calistir();
            MessageBox.Show("Önsöz taranıyor...");
            onsoz_calistir();//word e çevirme yapılıyor.
            MessageBox.Show("İçindekiler,başlıkları,sayfa numaraları taranıyor...");
            icindekiler_calistir();
            MessageBox.Show("Tablolar,başlıkları,sayfa numaraları taranıyor...");
            tablolar_calistir();
            //tablo_uyum_calistir();//buradan itibaren devam edecek
            MessageBox.Show("Şekiller,başlıkları,sayfa numaraları taranıyor...");
            sekiller_calistir();
            //sekil_uyum_calistir();
            MessageBox.Show("Denklemler taranıyor...");
            denklem_kontrol_calistir();
            MessageBox.Show("Giriş bölümü taranıyor...");
            giris_ayir_calistir();
            MessageBox.Show("Loglar oluşturuluyor...");
            log_yazdir();
            MessageBox.Show("TÜM ANALİZLER TAMAMLANDI.\nSAĞ KISIMDAN SONUÇLARA GÖZATABİLİRSİNİZ.");
        }

        private void frmAnaliz_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
