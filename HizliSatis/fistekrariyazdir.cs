using PrinterUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace HizliSatis
{
    public class fistekrariyazdir
    {
        string faturano, tarih;
        double tutar;
        string a1, a2, a3, tel, sirket;
        DataGridView tablo1;
        byte[] degerbit = Encoding.ASCII.GetBytes(string.Empty);
        PrinterSettings s = new PrinterSettings();
        PrinterUtility.EscPosEpsonCommands.EscPosEpson obje = new PrinterUtility.EscPosEpsonCommands.EscPosEpson();
        string slashn = "\n";


        public void verilerial(string gelenfaturano, double gelentutar, string gelentarih, DataGridView tablo, string sirketadi, string adres1, string adres2, string adres3, string telefon)
        {
            faturano = gelenfaturano;
            tutar = gelentutar;
            tarih = gelentarih;
            tablo1 = tablo;
            a1 = adres1;
            a2 = adres2;
            a3 = adres3;
            tel = telefon;
            sirket = sirketadi;
            fisyaz();
        }

        public void fisyaz()
        {

            try
            {
                degerbit = Encoding.ASCII.GetBytes(string.Empty);
                headerekle();
                pageekle();
                footerekle();
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth3());
                PrintExtensions.Print(degerbit, @"\\" +Genesis.pcadi  + @"\" + Properties.Settings.Default.yaziciAdi + "");
                //yazdir();
            }
            catch (Exception E)
            {
                MessageBox.Show("Yazdırma sırasında bir hata oluştu. Ayarlarınızı kontrol edin.\n" + E, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void headerekle()
        {
            if (Genesis.yaziciMM == 80)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth3());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.FontSelect.FontA());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(sirket).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.FontSelect.FontB());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(a1).Trim() + slashn + ConvertTurkishChars(a2).Trim() + slashn + ConvertTurkishChars(a3).Trim() + slashn + "Tel: " + ConvertTurkishChars(tel).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Left());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(" Tarih: " + tarih.Substring(0, 10) + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("  Saat: " + tarih.Substring(11, 5) + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Fis No: " + faturano + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Separator());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
            }
            else if (Genesis.yaziciMM == 58)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.FontSelect.SpecialFontA());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(sirket).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                //degerbit = PrintExtensions.AddBytes(degerbit, obje.FontSelect.FontB());
                //degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(a1).Trim() + slashn + ConvertTurkishChars(a2).Trim() + slashn + ConvertTurkishChars(a3).Trim() + slashn + "Tel: " + ConvertTurkishChars(tel).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Left());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(" Tarih: " + tarih.Substring(0, 10) + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("  Saat: " + tarih.Substring(11, 5) + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Fis No: " + faturano + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("--------------------------------" + slashn));
                //degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                //degerbit = PrintExtensions.AddBytes(degerbit, obje.Separator());
                //degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
            }
            
        }

        void pageekle()
        {
            string metin;
            string tutar;
            int bosluk;
            for (int i = 0; i < tablo1.RowCount; i++)
            {
                metin = tablo1.Rows[i].Cells[2].Value.ToString() + " x " + ConvertTurkishChars(tablo1.Rows[i].Cells[1].Value.ToString());
                tutar = string.Format("{0:#,##0.00}", Convert.ToDouble(tablo1.Rows[i].Cells[6].Value.ToString()));

                if (metin.Length > 22) metin = metin.Substring(0, 22);
                bosluk = 32 - metin.Length - tutar.Length - 4;

                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Left());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(metin));
                for (int j = 0; j < bosluk; j++)
                {
                    degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(" "));
                }
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(" " + tutar + " TL" + slashn));
            }
            degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(slashn));
        }

        void footerekle()
        {
            if (Genesis.yaziciMM == 80)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Right());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Genel Toplam: " + string.Format("{0:#,##0.00}", Convert.ToDouble(tutar)) + " TL" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Separator());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Mali Degeri Yoktur" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Bizi Tercih Ettiginiz Icin" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Tesekkur Ederiz..." + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.BarCode.Code128(faturano));
                //degerbit = PrintExtensions.AddBytes(degerbit, CutPage());
            }
            else if (Genesis.yaziciMM == 58)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Right());
                //degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Genel Toplam: " + string.Format("{0:#,##0.00}", Convert.ToDouble(tutar)) + " TL" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("--------------------------------" + slashn));
                //degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Mali Degeri Yoktur" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Bizi Tercih Ettiginiz Icin" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Tesekkur Ederiz..." + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.BarCode.Code128(faturano));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(slashn + slashn + slashn + " "));
                //degerbit = PrintExtensions.AddBytes(degerbit, CutPage());
            }
        }

        public static string ConvertTurkishChars(string text)
        {
            String[] olds = { "Ğ", "ğ", "Ü", "ü", "Ş", "ş", "İ", "ı", "Ö", "ö", "Ç", "ç" };
            String[] news = { "G", "g", "U", "u", "S", "s", "I", "i", "O", "o", "C", "c" };

            for (int i = 0; i < olds.Length; i++)
            {
                text = text.Replace(olds[i], news[i]);
            }
            return text;
        }
        public byte[] CutPage()
        {
            List<byte> oby = new List<byte>();
            oby.Add(Convert.ToByte(Convert.ToChar(0x1D)));
            oby.Add(Convert.ToByte('V'));
            oby.Add((byte)66);
            oby.Add((byte)3);
            return oby.ToArray();
        }

        public void yazdir()
        {
            PrintDocument PD = new PrintDocument();
            PD.PrintPage += new PrintPageEventHandler(OnPrintDocument);

            try
            {
                PD.Print();
            }
            catch
            {
                Console.WriteLine("Yazıcı çıktısı alınamıyor...");
            }
            finally
            {
                PD.Dispose();
            }
        }

        private static void OnPrintDocument(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, 0, 0, 0.05F, 0);
        }
    }
}
