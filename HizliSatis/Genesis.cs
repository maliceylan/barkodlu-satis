using Microsoft.SqlServer.Management.Smo;
using PrinterUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace HizliSatis
{
    public partial class Genesis : Form
    {
        //Diziler
        PictureBox[] resimler = new PictureBox[20];
        Label[] isimler = new Label[20];
        Label[] miktarlar = new Label[20];
        Label[] fiyatlar = new Label[20];
        String[] barkodlar = new String[20];
        String[] stokKodlari = new String[20];
        int[] kritikler = new int[20];
        ArrayList stokmiktar = new ArrayList();
        ArrayList kritikseviye = new ArrayList();
        ArrayList kritikseviyePCB = new ArrayList(20);
        //Değişkenler
        public static bool demo = false;
        public static bool terazi = false;
        public static bool musteriEkrani = true;
        public static int yaziciMM;
        int satisKod = 1;
        int grupID = 1;
        int secilenKisayol = 0;
        double alinanPara = 0;
        int seciliTabIndex = 0;
        double toplamTutar = 0;
        double paraUstu = 0;
        double adet = 1;
        string dosyayol = "";
        string resimYol = "";
        string hesapAktarID1 = "";
        string sifreAcilis = "";
        string teraziDeger = "";
        public static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Thread threadAcilis = new Thread(new ThreadStart(loading));
        public static string baglantiadresi = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Hizlisatis.mdf;Integrated Security=True;Connect Timeout=30";
        //public string baglantiadresi = "Data Source=ACER-BILGISAYAR;Initial Catalog=Hizlisatis;Persist Security Info=True;User ID=genesis;Password=genesis";
        //FİS TANIMLAMALARI

        public Byte[] degerbit = Encoding.ASCII.GetBytes(string.Empty);
        PrinterUtility.EscPosEpsonCommands.EscPosEpson obje = new PrinterUtility.EscPosEpsonCommands.EscPosEpson();
        public static string pcadi = Environment.MachineName;
        public static string varsayilanyazici;
        public PrinterSettings s = new PrinterSettings();
        string slashn = "\n";
        public static TabControl tbm = new TabControl();
        public static string baglantiAdresi()
        {
            return @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\HizliSatis.mdf;Integrated Security=True;Connect Timeout=15";
        }

        SerialPort sp;
        SerialPort sp2;


        [DllImport("api_com.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool com_init(
        int com,
        int baud
        );

        public Genesis()
        {
            //threadAcilis.Start();
            InitializeComponent();

            //Resim dizi ekleme
            resimler[0] = pcb1; resimler[5] = pcb6; resimler[10] = pcb11; resimler[15] = pcb16;
            resimler[1] = pcb2; resimler[6] = pcb7; resimler[11] = pcb12; resimler[16] = pcb17;
            resimler[2] = pcb3; resimler[7] = pcb8; resimler[12] = pcb13; resimler[17] = pcb18;
            resimler[3] = pcb4; resimler[8] = pcb9; resimler[13] = pcb14; resimler[18] = pcb19;
            resimler[4] = pcb5; resimler[9] = pcb10; resimler[14] = pcb15; resimler[19] = pcb20;


            //Miktar dizi ekleme
            miktarlar[0] = lblStok1; miktarlar[5] = lblStok6; miktarlar[10] = lblStok11; miktarlar[15] = lblStok16;
            miktarlar[1] = lblStok2; miktarlar[6] = lblStok7; miktarlar[11] = lblStok12; miktarlar[16] = lblStok17;
            miktarlar[2] = lblStok3; miktarlar[7] = lblStok8; miktarlar[12] = lblStok13; miktarlar[17] = lblStok18;
            miktarlar[3] = lblStok4; miktarlar[8] = lblStok9; miktarlar[13] = lblStok14; miktarlar[18] = lblStok19;
            miktarlar[4] = lblStok5; miktarlar[9] = lblStok10; miktarlar[14] = lblStok15; miktarlar[19] = lblStok20;

            //İsim dizi ekleme
            isimler[0] = lblAdi1; isimler[5] = lblAdi6; isimler[10] = lblAdi11; isimler[15] = lblAdi16;
            isimler[1] = lblAdi2; isimler[6] = lblAdi7; isimler[11] = lblAdi12; isimler[16] = lblAdi17;
            isimler[2] = lblAdi3; isimler[7] = lblAdi8; isimler[12] = lblAdi13; isimler[17] = lblAdi18;
            isimler[3] = lblAdi4; isimler[8] = lblAdi9; isimler[13] = lblAdi14; isimler[18] = lblAdi19;
            isimler[4] = lblAdi5; isimler[9] = lblAdi10; isimler[14] = lblAdi15; isimler[19] = lblAdi20;

            //Fiyat dizi ekleme
            fiyatlar[0] = lblFiyat1; fiyatlar[5] = lblFiyat6; fiyatlar[10] = lblFiyat11; fiyatlar[15] = lblFiyat16;
            fiyatlar[1] = lblFiyat2; fiyatlar[6] = lblFiyat7; fiyatlar[11] = lblFiyat12; fiyatlar[16] = lblFiyat17;
            fiyatlar[2] = lblFiyat3; fiyatlar[7] = lblFiyat8; fiyatlar[12] = lblFiyat13; fiyatlar[17] = lblFiyat18;
            fiyatlar[3] = lblFiyat4; fiyatlar[8] = lblFiyat9; fiyatlar[13] = lblFiyat14; fiyatlar[18] = lblFiyat19;
            fiyatlar[4] = lblFiyat5; fiyatlar[9] = lblFiyat10; fiyatlar[14] = lblFiyat15; fiyatlar[19] = lblFiyat20;

            dbkontrol();
            imgklasorkontrol();
            varsayilanyazici = s.PrinterName;
            tbm = tbMenu;
        }
        string dbkonumu = @"D:\Hizlisatis.mdf";
        HizliSatisEkrani ekran = new HizliSatisEkrani();
        public static void loading()
        {
            Application.Run(new acilisEkrani());
        }

        public bool demoKontrol()
        {
            bool demo = false;
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                baglanti.Open();
                komut.CommandText = "select COUNT(*) from Stok";
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetInt32(0) > 4) demo = true;
                }
                dr.Close();
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return demo;
        }

        void imgklasorkontrol()
        {
            if (Directory.Exists(Environment.CurrentDirectory + "\\Images"))
            {
                // MessageBox.Show("Images klasörü var.");
            }

            //if (Directory.Exists(Environment.CurrentDirectory.))
            //{
            //}
        }

        void dbkontrol()
        {
            if (!File.Exists(dbkonumu))
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source=(localdb)\v11.0;Integrated Security=True";
                    conn.Open();

                    SqlCommand komut = new SqlCommand();
                    komut.Connection = conn;
                    komut.CommandType = CommandType.Text;

                    komut.CommandText = @"CREATE DATABASE Hizlisatis ON PRIMARY (NAME = HizliSatis, FILENAME = 'D:\Hizlisatis.mdf', SIZE = 8MB, MAXSIZE = UNLIMITED, FILEGROWTH = 10%) LOG ON (NAME = kod5_Log, FILENAME = 'D:\Hizlisatis_Log.ldf', SIZE = 8MB, MAXSIZE = 2048GB, FILEGROWTH = 10%) COLLATE Turkish_100_CI_AS";
                    komut.ExecuteNonQuery();

                    komut.CommandText = @"CREATE TABLE [Hizlisatis].[dbo].[CariHareket]([MusteriID] [int] NOT NULL,[CariNo] [int] IDENTITY(1,1) NOT NULL, [Tur] [nvarchar](50) NULL,	[Aciklama] [nvarchar](300) NULL,[SonOdemeTarihi] [date] NULL,[Borc] [float] NULL,[Tahsilat] [float] NULL,	[NakitPOS] [nvarchar](50) NULL,	[IslemTarihi] [datetime] NULL, CONSTRAINT [PK_CariHareket] PRIMARY KEY CLUSTERED (	[CariNo] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    komut.ExecuteNonQuery();

                    komut.CommandText = @"CREATE TABLE [Hizlisatis].[dbo].[Fatura](	[SatisID] [int] IDENTITY(1,1) NOT NULL,	[FaturaNo] [nvarchar](50) NULL,	[BarkodNo] [nvarchar](50) NULL,	[UrunAdi] [nvarchar](200) NULL,	[Miktar] [float] NULL,	[Birim] [nvarchar](50) NULL,	[KDV] [int] NULL,[Fiyat] [float] NULL,	[Tutar] [float] NULL,	[Tarih] [datetime] NULL, CONSTRAINT [PK_Fatura] PRIMARY KEY CLUSTERED (	[SatisID] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    komut.ExecuteNonQuery();

                    komut.CommandText = @"CREATE TABLE [Hizlisatis].[dbo].[Kasa](	[IslemNo] [nvarchar](50) NOT NULL,	[Tur] [nvarchar](50) NULL,	[Aciklama] [nvarchar](200) NULL,	[NakitPOS] [nvarchar](50) NULL,	[Miktar] [float] NULL,	[Tarih] [datetime] NULL,	[Kullanici] [nvarchar](50) NULL) ON [PRIMARY]";
                    komut.ExecuteNonQuery();

                    komut.CommandText = @"CREATE TABLE [Hizlisatis].[dbo].[Musteriler](	[MusteriID] [int] NOT NULL,	[Adi] [nvarchar](200) NULL,	[Grubu] [nvarchar](50) NULL,	[Tur] [nvarchar](50) NULL,	[Telefon] [nvarchar](50) NULL,	[GSM] [nvarchar](50) NULL,	[Adres] [nvarchar](200) NULL,	[VergiDairesi] [nvarchar](50) NULL,	[VergiNo] [nvarchar](50) NULL,	[TCNo] [nvarchar](50) NULL,	[SonIslemTarihi] [date] NULL,	[SonOdemeTarihi] [date] NULL,	[Bakiye] [float] NULL,	[Hesap] [int] NULL, CONSTRAINT [PK_Musteriler] PRIMARY KEY CLUSTERED (	[MusteriID] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    komut.ExecuteNonQuery();

                    komut.CommandText = @"CREATE TABLE [Hizlisatis].[dbo].[Stok]([StokID] [int] IDENTITY(1,1) NOT NULL,	[Adi] [nvarchar](200) NULL,	[Grubu] [nvarchar](50) NULL,	[Barkod] [nvarchar](50) NULL,	[StokKodu] [nvarchar](50) NULL,	[Miktar] [float] NULL,	[Birim] [nvarchar](50) NULL,	[AlisFiyati] [float] NULL,	[SatisFiyati1] [float] NULL,	[SatisFiyati2] [float] NULL,	[KDV] [int] NULL,	[OTV] [int] NULL,	[KritikSeviye] [int] NULL,[KisayolNo] [int] NULL, CONSTRAINT [PK_Stok] PRIMARY KEY CLUSTERED (	[StokID] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    komut.ExecuteNonQuery();

                    komut.CommandText = @"CREATE TABLE [Hizlisatis].[dbo].[StokHareket]([HareketID] [int] IDENTITY(1,1) NOT NULL,	[Adi] [nvarchar](200) NULL,	[Grubu] [nvarchar](50) NULL,	[Barkod] [nvarchar](50) NULL,	[Miktar] [float] NULL,	[Tur] [nvarchar](50) NULL,	[Islem] [nvarchar](50) NULL,	[Aciklama] [nvarchar](200) NULL,	[Tarih] [datetime] NULL,	[AlisFiyati] [float] NULL,	[SatisFiyati] [float] NULL, CONSTRAINT [PK_StokHareket] PRIMARY KEY CLUSTERED (	[HareketID] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    komut.ExecuteNonQuery();

                    komut.CommandText = @"INSERT Stok SELECT 'Tanımsız Ürün','GENEL','00000000','','0','Adet','0','0','0','0','0','0','0';";
                    conn.Close();

                    conn.ConnectionString = baglantiAdresi();
                    conn.Open();
                    komut.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Database oluşturuldu.");
                }
            }
            else
            {
                //MessageBox.Show("Hoşgeldiniz "+txtSirketAdi.Text+" Hayırlı İşler !");
            }
        }
        //------------------------------------------------------------------------------HIZLI--SATIS--EKRANI-----------------------------------------------------------------
        void uyari(Label adi, Label stok, Label fiyat)
        {
            if (adi.ForeColor == Color.Red)
            {
                adi.ForeColor = Color.Black;
                stok.ForeColor = Color.Black;
                fiyat.ForeColor = Color.Black;
            }
            else
            {
                if (rbKritikRenkIkazA.Checked == true)
                {
                    adi.ForeColor = Color.Red;
                    stok.ForeColor = Color.Red;
                    fiyat.ForeColor = Color.Red;
                }
            }
        }

        public void baglantiKontrol()
        {
            if (sp.IsOpen)
            {
                lblBaglantiDurumu.Text = "Aktif";
                lblBaglantiDurumu.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblBaglantiDurumu.Text = "Pasif";
                lblBaglantiDurumu.ForeColor = Color.Maroon;
            }
        }

        public void teraziBaglan()
        {

            try
            {
                sp = new SerialPort(HizliSatis.Properties.Settings.Default.teraziCOM, 9600, Parity.None, 8, StopBits.One);
                sp.Open();
                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Teraziye bağlanılamadı!\n" + ex.Message, "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            teraziDeger = sp.ReadLine();
            lblBaglantiDurumu.Text = teraziDeger;
        }

        double teraziDegerAl()
        {
            double sayi;
            string gelenveri = teraziDeger;
            try
            {
                sayi = Convert.ToDouble(gelenveri.Substring(6, gelenveri.Length - 6).Replace("kg", "").Replace(".", ","));
                if (sayi < Convert.ToDouble(HizliSatis.Properties.Settings.Default.teraziMinDeger)) sayi = 0;
                sayi = Math.Round(sayi, 2);
            }
            catch
            {
                //MessageBox.Show("Terazi değer okuma hatası!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sayi = 0;
            }

            return sayi;
        }

        string stokKoduAl(string barkod)
        {
            string stokKodu = "";

            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                baglanti.Open();
                komut.CommandText = "select ISNULL(StokKodu,0) from Stok where Barkod=@barkod;";
                komut.Parameters.AddWithValue("@barkod", barkod);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    stokKodu = dr.GetString(0);
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return stokKodu;
        }

        void urunAl(string barkod)
        {
            string stokkodu = stokKoduAl(barkod);
            double miktar;
            if (Properties.Settings.Default.teraziStokKodu == stokkodu) miktar = teraziDegerAl();
            else miktar = Convert.ToDouble(txtAdet.Text);

            double fiyat;
            toplamTutar = 0;
            bool varmi = false;
            string ad;
            double tutar;
            for (int i = 0; i < tblFis.Rows.Count; i++)
            {
                if (tblFis.Rows[i].Cells[2].Value.ToString() == barkod)
                {
                    varmi = true;
                    if (swMiktarDus.Value == false)
                    {
                        miktar += Convert.ToDouble(tblFis.Rows[i].Cells[3].Value.ToString());
                    }
                    else
                    {
                        miktar = Convert.ToDouble(tblFis.Rows[i].Cells[3].Value.ToString()) - miktar;
                    }
                    if (miktar <= 0 || swUrunSil.Value == true)
                    {
                        tblFis.Rows.RemoveAt(i);
                        stokmiktar.RemoveAt(i);
                        kritikseviye.RemoveAt(i);
                        swUrunSil.Value = false;
                    }
                    else
                    {
                        if (miktar > Convert.ToDouble(stokmiktar[i].ToString()))
                        {
                            if (rbStokYetersizA.Checked == true)
                            {
                                DialogResult sonuc = MessageBox.Show("Ürünün mevcut stok miktarı: " + stokmiktar[i] + "\nHızlı stok girişi yapmak ister misiniz?\nAksi halde stok miktarınız eksiye düşecektir.", "Stok Yetersiz!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                                if (sonuc == DialogResult.Yes)
                                {
                                    tbMenu.SelectedIndex = 1;
                                    txtBarkodStokHareket.Text = barkod;
                                    txtUrunAdiStokHareket.Text = tblFis.Rows[i].Cells[1].Value.ToString();
                                    stokHareketAktiflestir();
                                    txtMiktarStokHareket.Focus();
                                    txtMiktarStokHareket.SelectAll();
                                }
                                else if (sonuc == DialogResult.No)
                                {
                                    tblFis.Rows[i].Cells[3].Value = miktar;
                                    tblFis.Rows[i].Cells[7].Value = Convert.ToDouble(tblFis.Rows[i].Cells[6].Value.ToString()) * miktar;
                                }
                            }
                            else
                            {
                                tblFis.Rows[i].Cells[3].Value = miktar;
                                tblFis.Rows[i].Cells[7].Value = Convert.ToDouble(tblFis.Rows[i].Cells[6].Value.ToString()) * miktar;
                            }
                        }
                        else
                        {
                            if (((Convert.ToDouble(stokmiktar[i]) - miktar) <= Convert.ToDouble(kritikseviye[i].ToString())) && rbKritikMesajA.Checked == true)
                            {
                                MessageBox.Show("Ürün stok miktarı kritik seviyeye ulaşmıştır!", "Kritik Seviye", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            tblFis.Rows[i].Cells[3].Value = miktar;
                            tblFis.Rows[i].Cells[7].Value = Convert.ToDouble(tblFis.Rows[i].Cells[6].Value.ToString()) * miktar;
                        }
                    }
                }
            }
            if (varmi == false && swMiktarDus.Value == false && swUrunSil.Value == false)
            {
                try
                {
                    SqlConnection bag = new SqlConnection(baglantiadresi);
                    bag.Open();
                    SqlCommand komut = new SqlCommand("select Adi,Miktar,SatisFiyati" + satisKod.ToString() + ",Birim,KDV,KritikSeviye from Stok where Barkod='" + barkod + "'", bag);
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        ad = dr.GetString(0);
                        fiyat = dr.GetDouble(2);
                        if (swFiyatGor.Value == false)
                        {
                            if (miktar > dr.GetDouble(1))
                            {
                                if (rbStokYetersizA.Checked == true)
                                {
                                    DialogResult sonuc = MessageBox.Show("Ürünün mevcut stok miktarı: " + dr.GetDouble(1).ToString() + "\nHızlı stok girişi yapmak ister misiniz?\nAksi halde stok miktarınız eksiye düşecektir.", "Stok Yetersiz!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                                    if (sonuc == DialogResult.Yes)
                                    {
                                        tbMenu.SelectedIndex = 1;
                                        txtAciklamaStokHareket.Enabled = true;
                                        txtMiktarStokHareket.Enabled = true;
                                        btnStokGirisi.Enabled = true;
                                        btnStokCikisi.Enabled = true;
                                        cmbIslemTuruStokHareket.Enabled = true;
                                        btnStokHareketSil.Enabled = false;
                                        txtBarkodStokHareket.Text = barkod;
                                        txtUrunAdiStokHareket.Text = ad;
                                        txtMiktarStokHareket.Focus();
                                        txtMiktarStokHareket.SelectAll();
                                    }
                                    else if (sonuc == DialogResult.No)
                                    {
                                        tutar = Math.Round(miktar * fiyat, 2);
                                        tblFis.Rows.Add(tblFis.RowCount + 1, ad, barkod, miktar.ToString(), dr.GetString(3), dr.GetInt32(4).ToString(), fiyat, tutar);
                                        stokmiktar.Add(dr.GetDouble(1));
                                        kritikseviye.Add(dr.GetInt32(5));
                                        satisButonAktiflestir();
                                    }
                                }
                                else
                                {
                                    tutar = Math.Round(miktar * fiyat, 2);
                                    tblFis.Rows.Add(tblFis.RowCount + 1, ad, barkod, miktar.ToString(), dr.GetString(3), dr.GetInt32(4).ToString(), fiyat, tutar);
                                    stokmiktar.Add(dr.GetDouble(1));
                                    kritikseviye.Add(dr.GetInt32(5));
                                    satisButonAktiflestir();
                                }
                            }
                            else
                            {
                                tutar = Math.Round(miktar * fiyat, 2);
                                tblFis.Rows.Add(tblFis.RowCount + 1, ad, barkod, miktar.ToString(), dr.GetString(3), dr.GetInt32(4).ToString(), fiyat, tutar);
                                stokmiktar.Add(dr.GetDouble(1));
                                kritikseviye.Add(dr.GetInt32(5));
                                satisButonAktiflestir();
                            }
                        }
                        else
                        {
                            MessageBox.Show("'" + ad + "' Ürününün fiyatı: " + fiyat.ToString() + "TL dir.", "Fiyat Gör", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (rbUrunBulunamadiA.Checked == true)
                        {
                            DialogResult sonuc = MessageBox.Show("Stoğunuzda bu barkod numarasına ait ürün bulunamadı!\nÜrün kaydı yapmak ister misiniz?", "Ürün Bulunamadı!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (sonuc == DialogResult.Yes)
                            {
                                tbMenu.SelectedIndex = 1;
                                btnYeniUrunStok.PerformClick();
                                txtBarkodNoStokBilgi.Text = barkod;
                                txtUrunAdi.Focus();
                            }
                        }
                    }
                    dr.Close();
                    bag.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            tutarHesapBitir(true);
        }


        void tutarHesapBitir(bool deger)
        {
            string toplamtutar;
            if (lblToplam.BackColor == Color.DarkRed) toplamtutar = ekran.toplamTutarHesap(tblFis, toplamTutar).ToString();
            else toplamtutar = alisFiyatiToplam();
            lblToplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(toplamtutar));
            paraHesap(0);
            txtAdet.Text = "1";
            txtBarkod.Text = "";
            txtBarkod.Focus();
            if (deger) tabloSonSatirSec(tblFis);
        }

        public void tabloSonSatirSec(DataGridView tbl)
        {
            if (tbl.Rows.Count > 0)
            {
                tbl.CurrentCell = tbl.Rows[tbl.RowCount - 1].Cells[0];
            }
        }

        public void resimCek(int grupID)
        {
            bool oto = true;
            if (cbManuelResimCek.Checked == true) oto = false;
            try
            {
                SqlConnection bag = new SqlConnection(baglantiadresi);
                bag.Open();
                if (oto == true)
                {
                    SqlCommand komut = new SqlCommand("SELECT * FROM (SELECT Adi,Barkod,Miktar,SatisFiyati" + satisKod.ToString() + ",KritikSeviye,StokKodu, ROW_NUMBER() OVER (ORDER BY Adi) AS RowNum FROM Stok WHERE (StokKodu='A' or StokKodu='T') and SatisFiyati1!=0)" +
                    " AS MyDerivedTable WHERE MyDerivedTable.RowNum BETWEEN " + ((grupID - 1) * 20 + 1) + " AND " + grupID * 20, bag);
                    SqlDataReader dr = komut.ExecuteReader();
                    kritikseviyePCB.Clear();
                    for (int i = 0; i < 20; i++)
                    {
                        if (dr.Read())
                        {
                            isimler[i].Text = dr.GetString(0);
                            miktarlar[i].Text = dr.GetValue(2).ToString();
                            fiyatlar[i].Text = dr.GetValue(3).ToString() + " TL";
                            barkodlar[i] = dr.GetString(1);
                            kritikseviyePCB.Insert(i, dr.GetInt32(4));
                            stokKodlari[i] = dr.GetString(5);

                            if (File.Exists(Application.StartupPath + "\\Images\\Urunler\\" + barkodlar[i] + ".jpg"))
                            {
                                using (FileStream fs = new FileStream(Application.StartupPath + "\\Images\\Urunler\\" + barkodlar[i]
                               + ".jpg", FileMode.Open, FileAccess.Read))
                                {
                                    resimler[i].Image = Image.FromStream(fs);
                                    fs.Dispose();
                                }
                            }
                            else { resimler[i].Image = Image.FromFile(Application.StartupPath + "\\Images\\Icon\\resim.png"); }
                        }
                        else
                        {
                            isimler[i].Text = "Boş";
                            miktarlar[i].Text = "0";
                            fiyatlar[i].Text = "0,00 TL";
                            resimler[i].Image = Image.FromFile(Application.StartupPath + "\\Images\\Icon\\resim.png");
                            barkodlar[i] = "";
                            stokKodlari[i] = "";
                        }
                    }
                }
                else
                {
                    kritikseviyePCB.Clear();
                    for (int i = 0; i < 20; i++)
                    {
                        isimler[i].Text = "Boş";
                        miktarlar[i].Text = "0";
                        fiyatlar[i].Text = "0,00 TL";
                        resimler[i].Image = Image.FromFile(Application.StartupPath + "\\Images\\Icon\\resim.png");
                        barkodlar[i] = "";
                        stokKodlari[i] = "";
                    }

                    SqlCommand komut = new SqlCommand();
                    komut.Connection = bag;
                    komut.CommandText = "SELECT Adi,Barkod,Miktar,SatisFiyati" + satisKod.ToString() + ",KritikSeviye,KisayolNo,ISNULL(StokKodu,0) FROM Stok where KisayolNo BETWEEN " + ((grupID - 1) * 20 + 1) + " AND " + grupID * 20;
                    SqlDataReader dr = komut.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(5) - ((grupID - 1) * 20) - 1;
                        isimler[id].Text = dr.GetString(0);
                        miktarlar[id].Text = dr.GetValue(2).ToString();
                        fiyatlar[id].Text = dr.GetValue(3).ToString() + " TL";
                        barkodlar[id] = dr.GetString(1);
                        stokKodlari[id] = dr.GetString(6);
                        //(kritikseviyePCB.Insert(id, dr.GetInt32(4));

                        if (File.Exists(Application.StartupPath + "\\Images\\Urunler\\" + barkodlar[id] + ".jpg"))
                        {
                            using (FileStream fs = new FileStream(Application.StartupPath + "\\Images\\Urunler\\" + barkodlar[id]
                           + ".jpg", FileMode.Open, FileAccess.Read))
                            {
                                resimler[id].Image = Image.FromStream(fs);
                                fs.Dispose();
                            }
                        }
                        else { resimler[id].Image = Image.FromFile(Application.StartupPath + "\\Images\\Icon\\resim.png"); }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void zaman()
        {
            lblTarih.Text = DateTime.Now.ToLongDateString();
            lblSaat.Text = DateTime.Now.ToLongTimeString();
        }

        private void tmrZaman_Tick(object sender, EventArgs e)
        {
            zaman();
        }

        public void paraHesap(double para)
        {
            alinanPara = Convert.ToDouble(txtAlinanPara.Text);
            alinanPara += para;
            txtAlinanPara.Text = alinanPara.ToString();
            toplamTutar = Convert.ToDouble(lblToplam.Text);
            Math.Round(toplamTutar, 2);
            paraUstu = alinanPara - toplamTutar;
            Math.Round(paraUstu, 2);
            txtParaUstu.Text = paraUstu.ToString();
            txtBarkod.Focus();
        }

        public void adetAyar(int sayi)
        {
            if (txtAdet.Text == "") txtAdet.Text = "0";
            adet = Convert.ToDouble(txtAdet.Text);
            adet += sayi;
            txtAdet.Text = adet.ToString();
        }

        private void pcb5tl_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(5);
        }

        private void pcb10tl_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(10);
        }

        private void pcb20tl_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(20);
        }

        private void pcb50tl_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(50);
        }

        private void pcb100tl_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(100);
        }

        private void pcb200tl_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(200);
        }

        void paraUstuTemizle()
        {
            alinanPara = 0;
            paraUstu = 0;
            txtParaUstu.Text = "0";
            txtAlinanPara.Text = "0";
        }
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            paraUstuTemizle();
        }
        private void btnArti_MouseDown(object sender, MouseEventArgs e)
        {
            adetAyar(1);
            txtAdet.BackColor = Color.PaleGreen;
        }

        private void btnEksi_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtAdet.Text != "1") adetAyar(-1);
            txtAdet.BackColor = Color.LightCoral;
        }

        private void txtAdet_MouseClick(object sender, MouseEventArgs e)
        {
            txtAdet.Text = "1";
            txtAdet.SelectAll();
            txtAdet.BackColor = Color.WhiteSmoke;
        }

        private void btnGrp1_Click(object sender, EventArgs e)
        {
            grupID = 1;
            resimCek(grupID);
            btnGrp1.ImageIndex = 1;
            btnGrp2.ImageIndex = 0;
            btnGrp3.ImageIndex = 0;
            btnGrp4.ImageIndex = 0;
            btnGrp5.ImageIndex = 0;
            btnGrp6.ImageIndex = 0;
            btnGrp7.ImageIndex = 0;
            btnGrp8.ImageIndex = 0;
        }

        private void btnGrp2_Click(object sender, EventArgs e)
        {
            grupID = 2;
            resimCek(grupID);
            btnGrp1.ImageIndex = 0;
            btnGrp2.ImageIndex = 1;
            btnGrp3.ImageIndex = 0;
            btnGrp4.ImageIndex = 0;
            btnGrp5.ImageIndex = 0;
            btnGrp6.ImageIndex = 0;
            btnGrp7.ImageIndex = 0;
            btnGrp8.ImageIndex = 0;
        }

        private void btnGrp3_Click(object sender, EventArgs e)
        {
            grupID = 3;
            resimCek(grupID);
            btnGrp1.ImageIndex = 0;
            btnGrp2.ImageIndex = 0;
            btnGrp3.ImageIndex = 1;
            btnGrp4.ImageIndex = 0;
            btnGrp5.ImageIndex = 0;
            btnGrp6.ImageIndex = 0;
            btnGrp7.ImageIndex = 0;
            btnGrp8.ImageIndex = 0;
        }

        private void btnGrp4_Click(object sender, EventArgs e)
        {
            grupID = 4;
            resimCek(grupID);
            btnGrp1.ImageIndex = 0;
            btnGrp2.ImageIndex = 0;
            btnGrp3.ImageIndex = 0;
            btnGrp4.ImageIndex = 1;
            btnGrp5.ImageIndex = 0;
            btnGrp6.ImageIndex = 0;
            btnGrp7.ImageIndex = 0;
            btnGrp8.ImageIndex = 0;
        }

        private void btnGrp5_Click(object sender, EventArgs e)
        {
            grupID = 5;
            resimCek(grupID);
            btnGrp1.ImageIndex = 0;
            btnGrp2.ImageIndex = 0;
            btnGrp3.ImageIndex = 0;
            btnGrp4.ImageIndex = 0;
            btnGrp5.ImageIndex = 1;
            btnGrp6.ImageIndex = 0;
            btnGrp7.ImageIndex = 0;
            btnGrp8.ImageIndex = 0;
        }

        private void btnGrp6_Click(object sender, EventArgs e)
        {
            grupID = 6;
            resimCek(grupID);
            btnGrp1.ImageIndex = 0;
            btnGrp2.ImageIndex = 0;
            btnGrp3.ImageIndex = 0;
            btnGrp4.ImageIndex = 0;
            btnGrp5.ImageIndex = 0;
            btnGrp6.ImageIndex = 1;
            btnGrp7.ImageIndex = 0;
            btnGrp8.ImageIndex = 0;
        }

        private void btnGrp7_Click(object sender, EventArgs e)
        {
            grupID = 7;
            resimCek(grupID);
            btnGrp1.ImageIndex = 0;
            btnGrp2.ImageIndex = 0;
            btnGrp3.ImageIndex = 0;
            btnGrp4.ImageIndex = 0;
            btnGrp5.ImageIndex = 0;
            btnGrp6.ImageIndex = 0;
            btnGrp7.ImageIndex = 1;
            btnGrp8.ImageIndex = 0;
        }

        private void btnGrp8_Click(object sender, EventArgs e)
        {
            grupID = 8;
            resimCek(grupID);
            btnGrp1.ImageIndex = 0;
            btnGrp2.ImageIndex = 0;
            btnGrp3.ImageIndex = 0;
            btnGrp4.ImageIndex = 0;
            btnGrp5.ImageIndex = 0;
            btnGrp6.ImageIndex = 0;
            btnGrp7.ImageIndex = 0;
            btnGrp8.ImageIndex = 1;
        }

        private void txtBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sadece rakam girişi.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '+';
            if (e.KeyChar == 13)
            {
                urunAl(txtBarkod.Text);
            }
            else if (e.KeyChar == '+')
            {
                try
                {
                    e.Handled = true;
                    txtBarkod.Text = txtBarkod.Text.Trim('+');
                    double tutar = Math.Round(Convert.ToDouble(txtBarkod.Text), 2);
                    tblFis.Rows.Add(tblFis.RowCount + 1, "Tanımsız Ürün", "00000000", "1", "Adet", "0", tutar, tutar);
                    stokmiktar.Add(0);
                    kritikseviye.Add(0);
                    satisButonAktiflestir();
                    tutarHesapBitir(true);
                }
                catch
                {
                    txtBarkod.Text = "";
                }
            }
        }

        void satisButonAktiflestir()
        {
            btnNakit.Enabled = true;
            btnNakitPOS.Enabled = true;
            btnPOS.Enabled = true;
            btnVeresiye.Enabled = true;
            btnIskonto.Enabled = true;
            btnSatisIptal.Enabled = true;
        }

        void satisButonPasiflestir()
        {
            btnNakit.Enabled = false;
            btnNakitPOS.Enabled = false;
            btnPOS.Enabled = false;
            btnVeresiye.Enabled = false;
            btnIskonto.Enabled = false;
            btnSatisIptal.Enabled = false;
            lblToplam.Text = "0,00";
            resimCek(grupID);
        }

        public void msUrunKisayolAc(int pcbNo)
        {
            secilenKisayol = pcbNo + (grupID - 1) * 20;
            if (barkodlar[pcbNo] == "")
            {
                msUrunKisayol.Items[0].Enabled = true;
                msUrunKisayol.Items[1].Enabled = false;
            }
            else
            {
                msUrunKisayol.Items[0].Enabled = false;
                msUrunKisayol.Items[1].Enabled = true;
            }
        }

        private void pcb1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[0]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(0);

        }

        private void pcb2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[1]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(1);
        }

        private void pcb3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[2]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(2);
        }

        private void pcb4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[3]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(3);
        }

        private void pcb5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[4]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(4);
        }

        private void pcb6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[5]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(5);
        }

        private void pcb7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[6]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(6);
        }

        private void pcb8_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[7]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(7);
        }

        private void pcb9_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[8]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(8);
        }

        private void pcb10_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[9]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(9);
        }

        private void pcb11_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[10]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(10);
        }

        private void pcb12_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[11]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(11);
        }
        private void pcb13_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[12]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(12);
        }
        private void pcb14_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[13]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(13);
        }
        private void pcb15_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[14]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(14);
        }

        private void pcb16_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[15]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(15);
        }

        private void pcb17_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[16]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(16);
        }

        private void pcb18_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[17]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(17);
        }

        private void pcb19_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[18]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(18);
        }

        private void pcb20_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) urunAl(barkodlar[19]);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) msUrunKisayolAc(19);
        }

        private void swMiktarDus_OnValueChange(object sender, EventArgs e)
        {
            if (swMiktarDus.Value == true)
            {
                swUrunSil.Value = false;
                swFiyatGor.Value = false;
            }
            txtBarkod.Focus();
        }

        private void swFiyatGor_OnValueChange(object sender, EventArgs e)
        {
            if (swFiyatGor.Value == true)
            {
                swUrunSil.Value = false;
                swMiktarDus.Value = false;
            }
            txtBarkod.Focus();
        }

        private void swUrunSil_OnValueChange(object sender, EventArgs e)
        {
            if (swUrunSil.Value == true)
            {
                //txtSifre.Clear();
                //panelAc(APnlSifre);
                //sifreAcilis = "swUrunSil";
            }

            if (swUrunSil.Value == true)
            {
                swFiyatGor.Value = false;
                swMiktarDus.Value = false;
            }
            txtBarkod.Focus();
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked == true)
            {
                satisKod = 1;
            }
            else
            {
                satisKod = 2;
            }
            resimCek(grupID);
            txtBarkod.Focus();
        }
        void grupDoldurStokHareket()
        {
            ArrayList barkodlar = new ArrayList();
            ArrayList gruplar = new ArrayList();
            ArrayList alisfiyat = new ArrayList();
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                baglanti.Open();
                komut.CommandText = "select distinct Stok.Grubu,Stok.Barkod,Stok.AlisFiyati from Stok join StokHareket on Stok.Barkod=StokHareket.Barkod where StokHareket.Grubu is null;";
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    barkodlar.Add(dr[1].ToString());
                    gruplar.Add(dr[0].ToString());
                    alisfiyat.Add(dr[2].ToString());
                }
                dr.Close();
                for (int i = 0; i < barkodlar.Count; i++)
                {
                    SqlCommand komut2 = new SqlCommand();
                    komut2.Connection = baglanti;
                    komut2.CommandText = "update StokHareket set Grubu=@grubu,AlisFiyati=@alisfiyati where Barkod=@barkod and Grubu is null;";
                    komut2.Parameters.AddWithValue("@grubu", gruplar[i].ToString());
                    komut2.Parameters.AddWithValue("@alisfiyati", alisfiyat[i].ToString().Trim('.').Replace(",", "."));
                    komut2.Parameters.AddWithValue("@barkod", barkodlar[i].ToString());
                    komut2.ExecuteNonQuery();
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void hizliSatis(string tur, string nakitpos)
        {
            if (demo) System.Diagnostics.Process.Start("http://www.genesisteknoloji.com");
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiAdresi());
                baglanti.Open();
                for (int i = 0; i < tblFis.RowCount; i++)
                {
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "insert into StokHareket (Adi,Barkod,Miktar,Tur,Islem,Aciklama,Tarih,SatisFiyati) values (@Adi,@barkod,@Miktar,@Tur,@Islem,@Aciklama,@Tarih,@fiyat);";
                    komut.Parameters.AddWithValue("@Adi", tblFis.Rows[i].Cells[1].Value);
                    komut.Parameters.AddWithValue("@Barkod", tblFis.Rows[i].Cells[2].Value);
                    komut.Parameters.AddWithValue("@Miktar", Convert.ToDouble(tblFis.Rows[i].Cells[3].Value));
                    komut.Parameters.AddWithValue("@Tur", cmbIslemHizliSatis.SelectedItem.ToString() + " (" + tur + ")");
                    komut.Parameters.AddWithValue("@Islem", "Çıkış");
                    komut.Parameters.AddWithValue("@Aciklama", "Fatura No:" + lblFaturaNo.Text);
                    komut.Parameters.AddWithValue("@Tarih", DateTime.Now);
                    komut.Parameters.AddWithValue("@fiyat", Convert.ToDouble(tblFis.Rows[i].Cells[6].Value));
                    komut.ExecuteNonQuery();
                    komut.CommandText = "update Stok set Miktar = Miktar-@Miktar where Barkod=@Barkod;";
                    komut.ExecuteNonQuery();
                    komut.CommandText = "insert into Fatura (FaturaNo,BarkodNo,UrunAdi,Miktar,Birim,KDV,Fiyat,Tutar,Tarih) values (@faturano,@Barkod,@Adi,@Miktar,@birim,@kdv,@fiyat,@tutar,GETDATE())";
                    komut.Parameters.AddWithValue("@faturano", lblFaturaNo.Text);
                    komut.Parameters.AddWithValue("@birim", tblFis.Rows[i].Cells[4].Value);
                    komut.Parameters.AddWithValue("@kdv", Convert.ToInt32(tblFis.Rows[i].Cells[5].Value));
                    komut.Parameters.AddWithValue("@tutar", Convert.ToDouble(tblFis.Rows[i].Cells[7].Value));
                    komut.ExecuteNonQuery();
                }
                if (tur != "Veresiye")
                {
                    double tutar = 0;
                    if (tur.Substring(0, 3) == "İsk")
                    {
                        tutar = Convert.ToDouble(txtPNLIskontoYeniTutar.Text);
                    }
                    else tutar = ekran.toplamTutarHesap(tblFis, toplamTutar);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "insert into Kasa(IslemNo,Tur,Aciklama,NakitPOS,Miktar,Tarih,Kullanici) values (@islemno,'Giris',@kasaaciklama,@nakitpos,@tutar,GETDATE(),@kullanici)";
                    komut.Parameters.AddWithValue("@islemno", Convert.ToInt32(lblFaturaNo.Text));
                    komut.Parameters.AddWithValue("@kullanici", cmbKasiyer.SelectedItem.ToString());
                    komut.Parameters.AddWithValue("@kasaaciklama", cmbIslemHizliSatis.SelectedItem.ToString() + " (" + tur + ")");
                    komut.Parameters.AddWithValue("@nakitpos", nakitpos);
                    komut.Parameters.AddWithValue("@tutar", tutar);
                    komut.ExecuteNonQuery();
                    lblToplam.Text = tutar.ToString();
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            paraUstuTemizle();
            fisyaz();
            grupDoldurStokHareket();
            stokHareketAramaTemizle();
            stokAramaTemizle();
            cmbDoldurStok();
            tblFis.Rows.Clear();
            satisButonPasiflestir();
            stokmiktar.Clear();
            txtBarkod.Focus();
        }

        void fisyaz()
        {
            if (swFis.Value == true)
            {
                try
                {
                    degerbit = Encoding.ASCII.GetBytes(string.Empty);
                    headerekle();
                    pageekle();
                    footerekle();
                    PrintExtensions.Print(degerbit, @"\\" + pcadi + @"\" + HizliSatis.Properties.Settings.Default.yaziciAdi + "");
                    //yazdir();
                }
                catch (Exception E)
                {
                    MessageBox.Show("Yazdırma sırasında bir hata oluştu. Ayarlarınızı kontrol edin." + E, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void pageekle()
        {
            string metin;
            string tutar;
            int bosluk;

            if (yaziciMM == 80)
            {
                for (int i = 0; i < tblFis.RowCount; i++)
                {
                    metin = tblFis.Rows[i].Cells[3].Value.ToString() + " x " + ConvertTurkishChars(tblFis.Rows[i].Cells[1].Value.ToString());
                    tutar = string.Format("{0:#,##0.00}", Convert.ToDouble(tblFis.Rows[i].Cells[7].Value.ToString()));
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
            else if (yaziciMM == 58)
            {
                for (int i = 0; i < tblFis.RowCount; i++)
                {
                    metin = tblFis.Rows[i].Cells[3].Value.ToString() + " x " + ConvertTurkishChars(tblFis.Rows[i].Cells[1].Value.ToString());
                    tutar = string.Format("{0:#,##0.00}", Convert.ToDouble(tblFis.Rows[i].Cells[7].Value.ToString()));
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
        }

        void footerekle()
        {
            if (yaziciMM == 80)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Right());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Genel Toplam: " + string.Format("{0:#,##0.00}", Convert.ToDouble(lblToplam.Text)) + " TL" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Separator());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Mali Degeri Yoktur" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Bizi Tercih Ettiginiz Icin" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Tesekkur Ederiz..." + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.BarCode.Code128(lblFaturaNo.Text));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(slashn + slashn + slashn + " "));
                //degerbit = PrintExtensions.AddBytes(degerbit, CutPage());
            }
            else if (yaziciMM == 58)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Right());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Genel Toplam: " + string.Format("{0:#,##0.00}", Convert.ToDouble(lblToplam.Text)) + " TL" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("--------------------------------" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Mali Degeri Yoktur" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Bizi Tercih Ettiginiz Icin" + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Tesekkur Ederiz..." + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.BarCode.Code128(lblFaturaNo.Text));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(slashn + slashn + slashn + " "));
                //degerbit = PrintExtensions.AddBytes(degerbit, CutPage());
            }

        }
        void headerekle()
        {
            if (yaziciMM == 80)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth3());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.FontSelect.FontA());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(txtSirketAdi.Text).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.FontSelect.FontB());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(txtSirketAdresi1.Text).Trim() + slashn + ConvertTurkishChars(txtSirketAdresi2.Text).Trim() + slashn + ConvertTurkishChars(txtSirketAdresi3.Text).Trim() + slashn + "Tel: " + ConvertTurkishChars(txtSirketTelefonu.Text).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Left());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(" Tarih: " + DateTime.Now.ToShortDateString() + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("  Saat: " + DateTime.Now.ToLongTimeString() + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Fis No: " + lblFaturaNo.Text + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Separator());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
            }
            else if (yaziciMM == 58)
            {
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.DoubleWidth2());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.FontSelect.SpecialFontA());
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Center());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(txtSirketAdi.Text).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.CharSize.Nomarl());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(ConvertTurkishChars(txtSirketAdresi1.Text).Trim() + slashn + ConvertTurkishChars(txtSirketAdresi2.Text).Trim() + slashn + ConvertTurkishChars(txtSirketAdresi3.Text).Trim() + slashn + "Tel: " + ConvertTurkishChars(txtSirketTelefonu.Text).Trim() + slashn + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, obje.Alignment.Left());
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes(" Tarih: " + DateTime.Now.ToShortDateString() + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("  Saat: " + DateTime.Now.ToLongTimeString() + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("Fis No: " + lblFaturaNo.Text + slashn));
                degerbit = PrintExtensions.AddBytes(degerbit, Encoding.ASCII.GetBytes("--------------------------------" + slashn));
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

        //public void yazdir()
        //{
        //    PrintDocument PD = new PrintDocument();
        //    PD.PrinterSettings.PrinterName = "POS58-2";
        //    PD.PrintPage += new PrintPageEventHandler(OnPrintDocument);

        //    try
        //    {
        //        PD.Print();
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Yazıcı çıktısı alınamıyor...");
        //    }
        //    finally
        //    {
        //        PD.Dispose();
        //    }
        //}

        private static void OnPrintDocument(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, 0, 0, 0.05F, 0);
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

        private void btnNakit_Click(object sender, EventArgs e)
        {
            hizliSatis("Nakit", "Nakit");
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            hizliSatis("POS", "POS");
        }

        private void btnSatisIptal_Click(object sender, EventArgs e)
        {
            iptal();
            //txtSifre.Clear();
            //panelAc(APnlSifre);
            //sifreAcilis = "btnSatisIptal";
        }
        void iptal()
        {
            tblFis.Rows.Clear();
            satisButonPasiflestir();
            stokmiktar.Clear();
            txtBarkod.Focus();
            paraUstuTemizle();
        }

        void panelAc(Panel panel)
        {
            tbMenu.Enabled = false;
            panel.Location = new Point(
            this.ClientSize.Width / 2 - panel.Size.Width / 2,
            this.ClientSize.Height / 2 - panel.Size.Height / 2);
            panel.Anchor = AnchorStyles.None;
            panel.Visible = true;
        }
        private void btnNakitPOS_Click(object sender, EventArgs e)
        {
            panelAc(ApnlNakitPos);
            double toplamtutar = Convert.ToDouble(lblToplam.Text);
            int nakit = Convert.ToInt32(toplamtutar / 2);
            double pos = toplamtutar - nakit;
            txtPnlNakitPOSNakit.Text = nakit.ToString();
            txtPnlNakitPOSPOS.Text = pos.ToString();
        }
        private void btnVeresiye_Click(object sender, EventArgs e)
        {
            if (cmbMusteri.SelectedIndex == -1)
            {
                MessageBox.Show("Müşteri seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                panelAc(ApnlVeresiye);
                txtPNLVeresiyeTutar.Text = lblToplam.Text;
                txtPNLVeresiyeAciklama.Text = cmbIslemHizliSatis.SelectedItem.ToString() + " Yapıldı";
                cmbMusteriDoldurPNLVeresiye();
            }
        }
        private void btnNakitPOSIptal_Click(object sender, EventArgs e)
        {
            ApnlNakitPos.Visible = false;
            tbMenu.Enabled = true;
        }

        private void txtPnlNakitPOSNakit_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(lblToplam.Text);
                double nakit = Convert.ToDouble(txtPnlNakitPOSNakit.Text);
                double pos = Convert.ToDouble(txtPnlNakitPOSPOS.Text);
                pos = toplam - nakit;
                txtPnlNakitPOSPOS.Text = pos.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPnlNakitPOSPOS_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(lblToplam.Text);
                double nakit = Convert.ToDouble(txtPnlNakitPOSNakit.Text);
                double pos = Convert.ToDouble(txtPnlNakitPOSPOS.Text);
                nakit = toplam - pos;
                if (nakit >= 0) txtPnlNakitPOSNakit.Text = nakit.ToString();
                else
                {
                    txtPnlNakitPOSPOS.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNakitPOSOdeme_Click(object sender, EventArgs e)
        {
            hizliSatis("Nakit-POS", "Nakit(" + txtPnlNakitPOSNakit.Text + ")-" + "POS(" + txtPnlNakitPOSPOS.Text + ")");
            btnNakitPOSIptal.PerformClick();
        }
        string alisFiyatiToplam()
        {
            double alistoplam = 0;
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                baglanti.Open();
                for (int i = 0; i < tblFis.RowCount; i++)
                {
                    komut.CommandText = "select AlisFiyati from Stok where Barkod='" + tblFis.Rows[i].Cells[2].Value.ToString() + "';";
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read()) alistoplam += dr.GetDouble(0) * Convert.ToDouble(tblFis.Rows[i].Cells[3].Value.ToString());
                    dr.Close();
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return alistoplam.ToString();
        }
        void stokMiktarGuncelle()
        {
            stokmiktar.Clear();
            try
            {
                SqlConnection bag = new SqlConnection(baglantiadresi);
                bag.Open();
                for (int i = 0; i < tblFis.RowCount; i++)
                {
                    SqlCommand komut = new SqlCommand("select Miktar from Stok where Barkod='" + tblFis.Rows[i].Cells[2].Value + "'", bag);
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read()) stokmiktar.Add(dr.GetDouble(0));
                    dr.Close();
                }
                bag.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void kritikseviyeGuncelle()
        {
            kritikseviye.Clear();
            try
            {
                SqlConnection bag = new SqlConnection(baglantiadresi);
                bag.Open();
                for (int i = 0; i < tblFis.RowCount; i++)
                {
                    SqlCommand komut = new SqlCommand("select KritikSeviye from Stok where Barkod='" + tblFis.Rows[i].Cells[2].Value + "'", bag);
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read()) kritikseviye.Add(dr.GetInt32(0));
                    dr.Close();

                }
                bag.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //-------------------------------------------------------------------------STOK--EKRANI---------------------------------------------------------------------------------------
        void stokHareketKaydı(string islem, string satis)
        {
            if (Convert.ToDouble(txtMiktarStokHareket.Text) > 0 && cmbIslemTuruStokHareket.SelectedIndex != -1)
            {
                string faturano = "-";
                if (cmbIslemTuruStokHareket.SelectedIndex != 2)
                {
                    faturano = txtFaturaNoStokHareket.Text;
                }
                string islem2 = "";
                if (islem == "+") islem2 = "Giriş";
                else
                {
                    islem2 = "Çıkış";
                }
                try
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    SqlCommand komut = new SqlCommand("insert into StokHareket (Adi,Grubu,Barkod,Miktar,Tur,Islem,Aciklama,Tarih,AlisFiyati,SatisFiyati) values (@Adi,@grubu,@barkod,@Miktar,@Tur,@Islem,@Aciklama,@tarih,@alis,@satis);", baglanti);
                    komut.Parameters.AddWithValue("@Adi", txtUrunAdiStokHareket.Text);
                    komut.Parameters.AddWithValue("@grubu", tblStok.CurrentRow.Cells[2].Value.ToString());
                    komut.Parameters.AddWithValue("@Barkod", txtBarkodStokHareket.Text);
                    komut.Parameters.AddWithValue("@Miktar", txtMiktarStokHareket.Text.Replace(".", "").Replace(",", "."));
                    komut.Parameters.AddWithValue("@Tur", cmbIslemTuruStokHareket.SelectedItem.ToString() + satis);
                    komut.Parameters.AddWithValue("@Islem", islem2);
                    komut.Parameters.AddWithValue("@Aciklama", txtAciklamaStokHareket.Text);
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.Parameters.AddWithValue("@alis", Convert.ToDouble(tblStok.CurrentRow.Cells[8].Value));
                    komut.Parameters.AddWithValue("@satis", Convert.ToDouble(tblStok.CurrentRow.Cells[9].Value));
                    SqlCommand komut2 = new SqlCommand("update Stok set Miktar = Miktar" + islem + "@miktar where Barkod=@barkod;", baglanti);
                    komut2.Parameters.AddWithValue("@barkod", txtBarkodStokHareket.Text);
                    komut2.Parameters.AddWithValue("@miktar", txtMiktarStokHareket.Text.Replace(".", "").Replace(",", "."));
                    baglanti.Open();
                    if (islem2 == "Çıkış") islem2 = "Giriş";
                    else islem2 = "Çıkış";
                    SqlCommand komut3 = new SqlCommand("insert into Kasa(IslemNo,Tur,Aciklama,NakitPOS,Miktar,Tarih,Kullanici) values (@islemno,@tur,@kasaaciklama,@nakitpos,@tutar,GETDATE(),@kullanici)", baglanti);
                    komut3.Parameters.AddWithValue("@islemno", faturano);
                    komut3.Parameters.AddWithValue("@tur", islem2);
                    komut3.Parameters.AddWithValue("@kullanici", cmbKasiyer.SelectedItem.ToString());
                    komut3.Parameters.AddWithValue("@kasaaciklama", cmbIslemTuruStokHareket.SelectedItem.ToString() + " (-" + txtBarkodStokHareket.Text + "- " + txtMiktarStokHareket.Text + " " + tblStok.CurrentRow.Cells[6].Value.ToString() + " " + txtUrunAdiStokHareket.Text + " " + txtAciklamaStokHareket.Text + ")");
                    komut3.Parameters.AddWithValue("@nakitpos", cmbNakitPOSStokHareket.SelectedItem.ToString());
                    komut3.Parameters.AddWithValue("@tutar", Convert.ToDouble(lblTutarStokHareket.Text.Substring(0, lblTutarStokHareket.Text.Length - 3)));

                    if (Convert.ToDouble(txtMiktarStokHareket.Text) > Convert.ToDouble(tblStok.CurrentRow.Cells[5].Value.ToString()) && islem == "-")
                    {
                        DialogResult sonuc = new DialogResult();
                        sonuc = MessageBox.Show("Ürünün geçerli stok miktarı:" + tblStok.CurrentRow.Cells[5].Value.ToString() + "\nGirdiğiniz miktarda çıkış yaparsanız stoğunuz eksiye düşecektir!\nYine de stok çıkışı yapmak istiyor musunuz?", "Stok Yetersiz!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (sonuc == DialogResult.Yes)
                        {
                            komut.ExecuteNonQuery();
                            komut2.ExecuteNonQuery();
                            if (rbKasaKaydıAktif.Checked == true) komut3.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        komut.ExecuteNonQuery();
                        komut2.ExecuteNonQuery();
                        if (rbKasaKaydıAktif.Checked == true) komut3.ExecuteNonQuery();
                    }
                    baglanti.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                stokAramaTemizle();
                cmbDoldurStok();
                stokHareketAramaTemizle();
                stokHareketTemizle();
                resimCek(grupID);
                stokMiktarGuncelle();
                kritikseviyeGuncelle();
                tblStokHareket.ClearSelection();
                tblStokHareket.Rows[tblStokHareket.RowCount - 1].Selected = true;
                tblStokHareket.FirstDisplayedScrollingRowIndex = tblStokHareket.RowCount - 1;
                tutarHesaplaStokHareket();
            }
            else
            {
                MessageBox.Show("Girişlerinizi kontrol ediniz!", "Giriş bilgileri hatalı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void secileniSilStokHareket()
        {
            try
            {
                DialogResult sonuc = MessageBox.Show("Secili olan Stok Hareket kayıtları silinecektir. Bu işlem geri alınamaz!\n Emin misiniz?", "Dikkat!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (sonuc == DialogResult.Yes)
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    baglanti.Open();
                    for (int i = 0; i < tblStokHareket.SelectedRows.Count; i++)
                    {
                        SqlCommand komut = new SqlCommand("delete from StokHareket where HareketID=" + tblStokHareket.SelectedRows[i].Cells[0].Value, baglanti);
                        komut.ExecuteNonQuery();
                    }
                    baglanti.Close();
                    stokHareketArama();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void secileniSilStok()
        {
            try
            {
                DialogResult sonuc = MessageBox.Show("Secili olan Stok kayıtları silinecektir. Bu işlem geri alınamaz!\n Emin misiniz?", "Dikkat!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (sonuc == DialogResult.Yes)
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    baglanti.Open();
                    for (int i = 0; i < tblStok.SelectedRows.Count; i++)
                    {
                        SqlCommand komut = new SqlCommand("delete from Stok where Barkod=" + tblStok.SelectedRows[i].Cells[3].Value, baglanti);
                        komut.ExecuteNonQuery();
                    }
                    baglanti.Close();
                    stokAramaTemizle();
                    cmbDoldurStok();
                    resimCek(grupID);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void secileniSilKasa()
        {
            try
            {
                DialogResult sonuc = MessageBox.Show("Seçili Kasa Hareket kayıtları ve bu kayıtlara bağlı satış detayları da silinecektir. Bu işlem geri alınamaz! Emin misiniz?", "Dikkat!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (sonuc == DialogResult.Yes)
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    for (int i = 0; i < tblKasa.SelectedRows.Count; i++)
                    {
                        try
                        {
                            komut.CommandText = "delete from Kasa where IslemNo=" + tblKasa.SelectedRows[i].Cells[0].Value.ToString();
                            komut.ExecuteNonQuery();
                            komut.CommandText = "delete from Fatura where FaturaNo=" + tblKasa.SelectedRows[i].Cells[0].Value.ToString();
                            komut.ExecuteNonQuery();
                        }
                        catch
                        {
                            //Eğer işlemno '-' ise açıklama üzerinden silme yapılır

                            komut.CommandText = "delete from Kasa where Aciklama='" + tblKasa.SelectedRows[i].Cells[2].Value.ToString() + "';";
                            komut.ExecuteNonQuery();
                        }

                    }
                    baglanti.Close();
                    kasaArama();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void cmbDoldurStok()
        {
            try
            {
                distinct("select distinct Grubu from Stok", cmbStokAramaGrup);
                distinct("select distinct Grubu from Stok", cmbGrup);
                distinct("select distinct Grubu from StokHareket", cmbGrubuStokHareket);
                distinct("select distinct Birim from Stok", cmbBirim);
                distinct("select distinct StokKodu from Stok", cmbAramaStokKodu);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void cmbDoldurSatisRapor()
        {
            distinct("select distinct Grubu from StokHareket", cmbGrubuSatisRapor);
            distinct("select distinct Tur from StokHareket where Islem='Çıkış'", cmbSatisTuruSatisRapor);
        }
        void cmbDoldurMusteri()
        {
            try
            {
                distinct("select distinct Grubu from Musteriler", cmbGrubuMusteriBilgileri);
                distinct("select distinct Grubu from Musteriler", cmbMusteriGrubuArama);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void cmbDoldurKasa()
        {
            try
            {
                distinct("select distinct Kullanici from Kasa", cmbKasaKasiyer);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void cmbMusteriArama()
        {
            cmbMusteriHesapAcma.Items.Clear();
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand("select Adi,MusteriID from Musteriler where Hesap=0", baglanti);
            baglanti.Open();
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                cmbMusteriHesapAcma.Items.Add(dr[0].ToString() + "(" + dr[1].ToString() + ")");
            }
            baglanti.Close();
        }
        void cmbMusteriDoldurHizliSatis()
        {
            cmbMusteri.Items.Clear();
            cmbMusteri.Items.Add("Genel");
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand("select Adi,MusteriID from Musteriler", baglanti);
            baglanti.Open();
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                cmbMusteri.Items.Add(dr[0].ToString() + "(" + dr[1].ToString() + ")");
            }
            baglanti.Close();
            cmbMusteri.SelectedIndex = 0;
        }

        void cmbMusteriDoldurPNLVeresiye()
        {
            cmbPNLVeresiyeMusteri.Items.Clear();
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand("select Adi,MusteriID from Musteriler where Hesap=1", baglanti);
            baglanti.Open();
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                cmbPNLVeresiyeMusteri.Items.Add(dr[0].ToString() + "(" + dr[1].ToString() + ")");
            }
            baglanti.Close();
            for (int i = 0; i < cmbPNLVeresiyeMusteri.Items.Count; i++)
            {
                if (cmbMusteri.SelectedItem.ToString() == cmbPNLVeresiyeMusteri.Items[i].ToString())
                {
                    cmbPNLVeresiyeMusteri.SelectedIndex = i;
                }
            }
        }

        void musteriAramaTemizle()
        {
            txtMusteriArama.Clear();
            cmbMusteriGrubuArama.SelectedIndex = -1;
            cmbHesapTuru.SelectedIndex = -1;
            musteriArama();
        }
        void stokAramaTemizle()
        {
            cmbAramaStokKodu.SelectedIndex = -1;
            cmbStokAramaGrup.SelectedIndex = -1;
            txtStokArama.Clear();
            stokArama();
        }
        void stokHareketAramaTemizle()
        {
            dtp2StokHareket.Value = DateTime.Today.AddDays(1);
            dtp1StokHareket.Value = DateTime.Today.AddMonths(-1);
            cmbIslemTuruStokHareketArama.SelectedIndex = -1;
            cmbHareketTuru.SelectedIndex = -1;
            cmbGrubuStokHareket.SelectedIndex = -1;
            txtMetinStokHareket.Clear();
            stokHareketArama();
        }
        void hesapAramaTemizle()
        {
            dtp2SonIslem.Value = DateTime.Today.AddDays(1);
            dtp1SonIslem.Value = DateTime.Today.AddMonths(-1);
            cmbHesapTuruHesaplar.SelectedIndex = -1;
            txtHesapArama.Clear();
            cbOdemeGunuHesaplar.Checked = false;
            hesapArama();
        }
        void cariAramaTemizle()
        {
            dtp2CariIslemTarihi.Value = DateTime.Today.AddDays(1);
            dtp1CariIslemTarihi.Value = DateTime.Today.AddMonths(-1);
            cmbIslemTuruCariHareket.SelectedIndex = -1;
            txtCariHareketArama.Clear();
            cbOdemeGunuCariHareket.Checked = false;
            cariArama();
        }

        void kasaAramaTemizle()
        {
            dtp2Kasa.Value = DateTime.Today.AddDays(1);
            dtp1Kasa.Value = DateTime.Today;
            cmbKasaAramaNakitPOS.SelectedIndex = -1;
            cmbKasaIslemTuru.SelectedIndex = -1;
            cmbKasaKasiyer.SelectedIndex = -1;
            txtKasaHareketArama.Clear();
            cbKasaTarih.Checked = true;
            kasaArama();
        }

        void satisRaporAramaTemizle(bool gunluk)
        {
            dtp2OzelTarihSatisRapor.Value = DateTime.Today.AddDays(1);
            dtp1OzelTarihSatisRapor.Value = DateTime.Today;
            dtpGunlukSatisRapor.Value = DateTime.Today;
            numYilSatisRapor.Value = DateTime.Today.Year;
            cmbAylarSatisRapor.SelectedIndex = DateTime.Today.Month - 1;
            rbGunlukSatisRapor.Checked = gunluk;
            rbAylikSatisRapor.Checked = false;
            rbOzelTarihSatisRapor.Checked = false;
            cmbSatisTuruSatisRapor.SelectedIndex = -1;
            cmbGrubuSatisRapor.SelectedIndex = -1;
            txtMetinSatisRapor.Clear();
            satisRaporArama();
        }

        void altBilgiStok()
        {
            if (tblStok.Rows.Count > 0)
            {
                double toplamMiktar = 0;
                double toplamAlisFiyati = 0;
                double toplamSatisFiyati1 = 0;
                double toplamSatisFiyati2 = 0;

                for (int i = 0; i < tblStok.Rows.Count; i++)
                {
                    toplamMiktar += Convert.ToDouble(tblStok.Rows[i].Cells[5].Value.ToString());
                    toplamAlisFiyati += (Convert.ToDouble(tblStok.Rows[i].Cells[5].Value.ToString()) * Convert.ToDouble(tblStok.Rows[i].Cells[8].Value.ToString()));
                    toplamSatisFiyati1 += (Convert.ToDouble(tblStok.Rows[i].Cells[5].Value.ToString()) * Convert.ToDouble(tblStok.Rows[i].Cells[9].Value.ToString()));
                    toplamSatisFiyati2 += (Convert.ToDouble(tblStok.Rows[i].Cells[5].Value.ToString()) * Convert.ToDouble(tblStok.Rows[i].Cells[10].Value.ToString()));
                }
                txtToplamMiktar.Text = toplamMiktar.ToString();
                txtAlisToplam.Text = toplamAlisFiyati.ToString();
                txtSatis1Toplam.Text = toplamSatisFiyati1.ToString();
                txtSatis2Toplam.Text = toplamSatisFiyati2.ToString();
                txtToplamKayit.Text = tblStok.RowCount.ToString();

                txtToplamMiktar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtToplamMiktar.Text));
                txtAlisToplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtAlisToplam.Text));
                txtSatis1Toplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtSatis1Toplam.Text));
                txtSatis2Toplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtSatis2Toplam.Text));
            }
        }
        void altBilgiHesaplar()
        {
            double toplamBakiye = 0;
            for (int i = 0; i < tblHesaplar.RowCount; i++)
            {
                toplamBakiye += Convert.ToDouble(tblHesaplar.Rows[i].Cells[5].Value.ToString());
            }
            txtBakiyeToplam.Text = toplamBakiye.ToString();
            txtBakiyeToplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtBakiyeToplam.Text));
            txtHesapToplam.Text = tblHesaplar.RowCount.ToString();
        }
        void altBilgiCari()
        {
            double toplamBorc = 0;
            double toplamTahsilat = 0;
            double toplamBakiye = 0;
            for (int i = 0; i < tblCariHareketler.RowCount; i++)
            {
                if (tblCariHareketler.Rows[i].Cells[4].Value.ToString() != "") toplamBorc += Convert.ToDouble(tblCariHareketler.Rows[i].Cells[4].Value.ToString());
                if (tblCariHareketler.Rows[i].Cells[5].Value.ToString() != "") toplamTahsilat += Convert.ToDouble(tblCariHareketler.Rows[i].Cells[5].Value.ToString());
            }
            toplamBakiye = (toplamBorc - toplamTahsilat);

            txtBorcToplam.Text = toplamBorc.ToString();
            txtTahsilatToplam.Text = toplamTahsilat.ToString();
            txtBakiyeToplamCari.Text = toplamBakiye.ToString();

            txtBorcToplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtBorcToplam.Text));
            txtTahsilatToplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtTahsilatToplam.Text));
            txtBakiyeToplamCari.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtBakiyeToplamCari.Text));

            txtIslemToplam.Text = tblCariHareketler.RowCount.ToString();
        }
        void altBilgiKasa()
        {

            double pos = 0;
            double nakit = 0;
            double toplamgiren = 0;
            double toplamcikan = 0;
            double bakiye = 0;
            double devirtutar = 0;
            for (int i = 0; i < tblKasa.RowCount; i++)
            {
                if (tblKasa.Rows[i].Cells[3].Value.ToString().StartsWith("Nakit("))
                {
                    string tumu;
                    tumu = tblKasa.Rows[i].Cells[3].Value.ToString();
                    string[] parcalar = tumu.Split('-');
                    parcalar[0] = parcalar[0].Remove(0, 6);
                    parcalar[0] = parcalar[0].Remove(parcalar[0].Length - 1, 1);
                    parcalar[1] = parcalar[1].Remove(0, 4);
                    parcalar[1] = parcalar[1].Remove(parcalar[1].Length - 1, 1);
                    nakit += Convert.ToDouble(parcalar[0]);
                    pos += Convert.ToDouble(parcalar[1]);

                }
                if (tblKasa.Rows[i].Cells[3].Value.ToString() == "POS" && tblKasa.Rows[i].Cells[1].Value.ToString() == "Giris") pos += Convert.ToDouble(tblKasa.Rows[i].Cells[4].Value.ToString());
                if (tblKasa.Rows[i].Cells[3].Value.ToString() == "Nakit" && tblKasa.Rows[i].Cells[1].Value.ToString() == "Giris") nakit += Convert.ToDouble(tblKasa.Rows[i].Cells[4].Value.ToString());
                if (tblKasa.Rows[i].Cells[1].Value.ToString() == "Giris") toplamgiren += Convert.ToDouble(tblKasa.Rows[i].Cells[4].Value.ToString());
                if (tblKasa.Rows[i].Cells[1].Value.ToString() == "Çıkış") toplamcikan += Convert.ToDouble(tblKasa.Rows[i].Cells[4].Value.ToString());
            }
            bakiye = toplamgiren - toplamcikan;
            txtToplamGirenPOS.Text = pos.ToString();
            txtToplamGirenNakit.Text = nakit.ToString();
            txtToplamGiren.Text = toplamgiren.ToString();
            txtToplamBakiye.Text = bakiye.ToString();
            txtToplamCikan.Text = toplamcikan.ToString();
            txtToplamGirenPOS.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtToplamGirenPOS.Text));
            txtToplamGirenNakit.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtToplamGirenNakit.Text));
            txtToplamGiren.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtToplamGiren.Text));
            txtToplamBakiye.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtToplamBakiye.Text));
            txtToplamCikan.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtToplamCikan.Text));
            txtListeIslemSayisi.Text = tblKasa.RowCount.ToString();
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                baglanti.Open();
                komut.CommandText = "select ISNULL (SUM(Miktar),0) from Kasa where Tur='Giriş' or Tur='Giris';";
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read()) toplamgiren = Convert.ToDouble(dr[0].ToString());
                dr.Close();
                komut.CommandText = "select ISNULL (SUM(Miktar),0) from Kasa where Tur='Çıkış' or Tur='Cıkıs';";
                dr = komut.ExecuteReader();
                if (dr.Read()) toplamcikan = Convert.ToDouble(dr[0].ToString());
                devirtutar = toplamgiren - toplamcikan;
                txtKasaDevirTutar.Text = devirtutar.ToString();
                txtKasaDevirTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtKasaDevirTutar.Text));
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void kritikRenk()
        {
            double miktar = 0;
            int kritik = 0;
            for (int i = 0; i < tblStok.RowCount; i++)
            {
                miktar = Convert.ToDouble(tblStok.Rows[i].Cells[5].Value.ToString());
                kritik = Convert.ToInt32(tblStok.Rows[i].Cells[7].Value.ToString());
                if (miktar <= kritik) tblStok.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
            }
        }
        public void stokArama()
        {
            if (txtStokGosterimSayisi.Text.Trim().Equals("")) txtStokGosterimSayisi.Text = "100";
            string top = txtStokGosterimSayisi.Text;
            //cmbDoldurStok();
            string grup = "";
            string stokkodu = "";
            if (cmbStokAramaGrup.SelectedIndex != -1) grup = "Grubu=@grubu and ";
            if (cmbAramaStokKodu.SelectedIndex != -1) stokkodu = "StokKodu=@stokkodu and ";
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                baglanti.Open();
                komut = new SqlCommand("select top " + top + " StokID[Stok No],Adi[Ürün Adı],Grubu,Barkod[Barkod No],StokKodu[Stok Kodu],Miktar,Birim,KritikSeviye[Kritik Seviye],AlisFiyati[Alış Fiyatı],SatisFiyati1[Satış Fiyatı 1],SatisFiyati2[Satış Fiyatı 2],KDV,OTV[ÖTV] from Stok where " + grup + stokkodu + " (Adi like '%'+@adi+'%' or Barkod like '%'+@barkod+'%');", baglanti);
                if (grup != "") komut.Parameters.AddWithValue("@grubu", cmbStokAramaGrup.SelectedItem.ToString());
                if (stokkodu != "") komut.Parameters.AddWithValue("@stokkodu", cmbAramaStokKodu.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@adi", txtStokArama.Text);
                komut.Parameters.AddWithValue("@barkod", txtStokArama.Text);
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblStok.DataSource = dt;
                baglanti.Close();
                tblStok.AutoResizeColumnHeadersHeight();
                tblStok.AutoResizeColumns();
                altBilgiStok();
                kritikRenk();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void distinct(string cmd, ComboBox cmb)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                baglanti.Open();
                SqlCommand komut = new SqlCommand(cmd, baglanti);
                SqlDataReader dr = komut.ExecuteReader();
                cmb.Items.Clear();

                while (dr.Read())
                {
                    cmb.Items.Add(dr[0].ToString());
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void stokPasiflestir()
        {
            btnYeniUrunStok.Enabled = true;
            btnUrunGuncelleStok.Enabled = true;
            btnUrunSilStok.Enabled = true;
            txtBarkodNoStokBilgi.Enabled = false;
            txtStokKodu.Enabled = false;
            txtKritikSeviye.Enabled = false;
            txtUrunAdi.Enabled = false;
            txtSatisFiyati1.Enabled = false;
            txtSatisFiyati2.Enabled = false;
            txtAlisFiyati.Enabled = false;
            cmbBirim.Enabled = false;
            cmbGrup.Enabled = false;
            pnlKDV.Enabled = false;
            pnlOTV.Enabled = false;
            txtKDV.Enabled = false;
            rbKDV0.Enabled = false;
            rbKDV1.Enabled = false;
            rbKDV8.Enabled = false;
            rbKDV18.Enabled = false;
            txtOTV.Enabled = false;
            btnGrupEkle.Enabled = false;
            btnBirimEkle.Enabled = false;
            btnResimSec.Enabled = false;
        }
        void stokAftiflestir()
        {
            txtBarkodNoStokBilgi.Enabled = true;
            txtStokKodu.Enabled = true;
            txtKritikSeviye.Enabled = true;
            txtUrunAdi.Enabled = true;
            txtSatisFiyati1.Enabled = true;
            txtSatisFiyati2.Enabled = true;
            txtAlisFiyati.Enabled = true;
            cmbBirim.Enabled = true;
            cmbGrup.Enabled = true;
            pnlKDV.Enabled = true;
            pnlOTV.Enabled = true;
            txtKDV.Enabled = true;
            rbKDV0.Enabled = true;
            rbKDV1.Enabled = true;
            rbKDV8.Enabled = true;
            rbKDV18.Enabled = true;
            txtOTV.Enabled = true;
            btnGrupEkle.Enabled = true;
            btnBirimEkle.Enabled = true;
            btnResimSec.Enabled = true;
        }

        public void StokGiristemizle()
        {
            txtBarkodStokHareket.Text = "";
            txtUrunAdiStokHareket.Text = "";
            txtMiktarStokHareket.Text = "0,00";
            txtAciklamaStokHareket.Text = "";
            txtBarkodNoStokBilgi.Text = "";
            txtStokKodu.Text = "";
            txtUrunAdi.Text = "";
            txtAlisFiyati.Text = "0,00";
            txtSatisFiyati1.Text = "0,00";
            txtSatisFiyati2.Text = "0,00";
            txtKritikSeviye.Text = "0";
            txtKDV.Text = "0";
            kdvSec();
            txtOTV.Text = "0";
            cmbBirim.SelectedIndex = -1;
            cmbGrup.SelectedIndex = -1;
            rbKDVdahil.Select();
            rbOTVdahil.Select();
            pcbUrun.Image = Image.FromFile(Application.StartupPath + "\\Images\\Icon\\resim.png");
            dosyayol = Application.StartupPath + "\\Images\\Icon\\resim.png";
        }

        public void kasaAktiflestir()
        {
            txtKasaIslemNo.Enabled = true;
            txtKasaMiktar.Enabled = true;
            cmbKasaAciklama.Enabled = true;
            btnKasaKaydet.Enabled = true;
            btnKasaIptal.Enabled = true;
            btnKasaNumaraUret.Enabled = true;
            cmbParaTuruKasa.Enabled = true;
        }
        public void kasaPasiflestir()
        {
            txtKasaIslemNo.Enabled = false;
            txtKasaMiktar.Enabled = false;
            cmbKasaAciklama.Enabled = false;
            btnKasaKaydet.Enabled = false;
            btnKasaIptal.Enabled = false;
            btnKasaNumaraUret.Enabled = false;
            btnKasaParaGiris.Checked = false;
            btnKasaParaCikis.Checked = false;
            cmbParaTuruKasa.Enabled = false;
        }

        public void kasaGirisTemizle()
        {
            txtKasaIslemNo.Clear();
            txtKasaMiktar.Text = "0,00";
            cmbKasaAciklama.Items.Clear();
            cmbKasaAciklama.Text = "";
        }
        public void cmbEkleme(ComboBox cmb, string tur, Button btn)
        {
            if (cmb.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                cmb.DropDownStyle = ComboBoxStyle.DropDown;
                cmb.SelectedIndex = -1;
                MessageBox.Show("Yeni " + tur + " ismini " + tur + " listesi bölümüne giriniz ve " + tur + " kaydet butonuna basınız.");
                btn.Text = tur + " Kaydet";
            }
            else
            {//find string exact aynı item yoksa -1 değeri gönderir.
                if (cmb.Text.Trim() != "" && cmb.FindStringExact(cmb.Text) == -1)
                {
                    cmb.Items.Add(cmb.Text);
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmb.SelectedIndex = cmb.Items.Count - 1;
                }
                else
                {
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                btn.Text = tur + " Ekle";
            }
        }
        private void Genesis_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbMenu.Enabled == true)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    //Environment.Exit(0);
                }
                if (tbMenu.SelectedIndex == 0)
                {

                    if (e.KeyCode == Keys.F4)
                    {
                        btnNakit.PerformClick();
                    }
                    else if (e.KeyCode == Keys.F5)
                    {
                        if (swUrunSil.Value == false)
                        {
                            swUrunSil.Value = true;
                            swFiyatGor.Value = false;
                            swMiktarDus.Value = false;
                        }
                        else swUrunSil.Value = false;
                    }
                    else if (e.KeyCode == Keys.F3)
                    {
                        btnPOS.PerformClick();
                    }
                    else if (e.KeyCode == Keys.F2)
                    {
                        btnNakitPOS.PerformClick();
                    }
                    else if (e.KeyCode == Keys.F1)
                    {
                        btnVeresiye.PerformClick();
                    }
                    else if (e.KeyCode == Keys.F9)
                    {
                        btnIskonto.PerformClick();
                    }
                    else if (e.KeyCode == Keys.F6)
                    {
                        btnSatisIptal.PerformClick();
                    }
                    else if (e.KeyCode == Keys.F7)
                    {
                        if (swMiktarDus.Value == false)
                        {
                            swMiktarDus.Value = true;
                            swUrunSil.Value = false;
                            swFiyatGor.Value = false;
                        }
                        else swMiktarDus.Value = false;
                    }
                    else if (e.KeyCode == Keys.F8)
                    {
                        if (swFis.Value == false) swFis.Value = true;
                        else swFis.Value = false;
                    }
                    else if (e.KeyCode == Keys.F10)
                    {
                        if (swFiyatGor.Value == false)
                        {
                            swFiyatGor.Value = true;
                            swMiktarDus.Value = false;
                            swUrunSil.Value = false;
                        }
                        else swFiyatGor.Value = false;
                    }
                    else if (e.KeyCode == Keys.F12)
                    {
                        txtBarkod.Focus();
                    }
                    else
                    {
                        //if (txtBarkod.Text.Equals("") && !txtBarkod.Focused)
                        //{
                        //    txtBarkod.Focus();
                        //    KeysConverter kc = new KeysConverter();
                        //    string keyChar = kc.ConvertToString(e.KeyData);
                        //    SendKeys.Send(keyChar);
                        //}
                    }
                }
            }
        }

        public void comPortDoldur()
        {
            string[] portlar = SerialPort.GetPortNames();
            cmbCOMAyarlar.Items.Clear();
            foreach (string prt in portlar)
            {
                cmbCOMAyarlar.Items.Add(prt);
                cmbCOMMusteriEkran.Items.Add(prt);
            }
        }

        private void Genesis_Load(object sender, EventArgs e)
        {
            if (demo) System.Diagnostics.Process.Start("http://www.genesisteknoloji.com");
            comPortDoldur();
            ayarCek();
            ayarUygula();
            cmbKasiyer.SelectedIndex = 0;
            cmbIslemHizliSatis.SelectedIndex = 0;
            cmbParaTuruKasa.SelectedIndex = 0;
            cmbCariParaTur.SelectedIndex = 0;
            resimCek(grupID);
            btnGrp1.ImageIndex = 1;
            zaman();
            tmrZaman.Start();
            stokHareketAramaTemizle();
            cmbNakitPOSStokHareket.SelectedIndex = 0;
            stokAramaTemizle();
            cmbDoldurStok();
            cmbDoldurMusteri();
            cmbDoldurKasa();
            musteriArama();
            hesapAramaTemizle();
            //tmrUyari.Start();
            cmbMusteriArama();
            cmbMusteriDoldurHizliSatis();
            kasaAramaTemizle();
            if (terazi)
            {
                teraziBaglan();
                baglantiKontrol();
                gbTerazi.Enabled = true;
            }
            if (musteriEkrani)
            {
                musteriEkranBaglan();
                ekranTutarYaz();
                gbMusteriEkrani.Enabled = true;
            }
            
            txtBarkod.Focus();
            //threadAcilis.Abort();
        }

        public void musteriEkranBaglan()
        {
            try
            {
                sp2 = new SerialPort(Properties.Settings.Default.ekranCOM, Convert.ToInt32(Properties.Settings.Default.ekranBaud), Parity.None, 8, StopBits.One);
                sp2.Open();
                sp2.Write(Convert.ToString((char)12));
                sp2.Write(Convert.ToString((char)27));
                sp2.Write(Convert.ToString((char)115));
                sp2.Write(Convert.ToString((char)50));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri ekranına bağlanılamadı!\n" + ex.Message, "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void musteriEkranYaz(string tutar)
        {
            if (sp2 != null)
            {
                sp2.Write(Convert.ToString((char)27));
                sp2.Write(Convert.ToString((char)81));
                sp2.Write(Convert.ToString((char)65));
                sp2.Write(tutar);
                sp2.Write(Convert.ToString((char)13));
            }
            else
            {
                //MessageBox.Show("Müşteri ekranı bağlantınızda sorun var!","Hata!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnYeniUrun_CheckedChanged(object sender, EventArgs e)
        {
            if (btnYeniUrunStok.Checked == true)
            {
                btnYeniUrunStok.ForeColor = Color.White;
                btnYeniUrunStok.BackColor = Color.DarkRed;
                btnKaydetStok.Enabled = true;
                btnIptalStok.Enabled = true;
                btnResimSec.Enabled = true;
                stokAftiflestir();
                StokGiristemizle();
                txtBarkodNoStokBilgi.Focus();
            }
            else
            {
                btnYeniUrunStok.ForeColor = Color.Black;
                btnYeniUrunStok.BackColor = Color.Transparent;
                btnKaydetStok.Enabled = false;
                btnIptalStok.Enabled = false;
            }
        }

        private void btnUrunGuncelle_CheckedChanged(object sender, EventArgs e)
        {
            if (btnUrunGuncelleStok.Checked == true)
            {
                stokAftiflestir();//
                btnUrunGuncelleStok.ForeColor = Color.White;
                btnUrunGuncelleStok.BackColor = Color.DarkRed;
                btnKaydetStok.Enabled = true;
                btnIptalStok.Enabled = true;
                txtBarkodNoStokBilgi.Enabled = false;
            }
            else
            {
                btnUrunGuncelleStok.ForeColor = Color.Black;
                btnUrunGuncelleStok.BackColor = Color.Transparent;
                btnKaydetStok.Enabled = false;
                btnIptalStok.Enabled = false;
                txtBarkodNoStokBilgi.Enabled = true;
            }
        }

        private void btnUrunSil_CheckedChanged(object sender, EventArgs e)
        {
            if (btnUrunSilStok.Checked == true)
            {
                btnUrunSilStok.ForeColor = Color.White;
                btnUrunSilStok.BackColor = Color.DarkRed;
                btnKaydetStok.Enabled = true;
                btnIptalStok.Enabled = true;
                stokPasiflestir();
            }
            else
            {
                btnUrunSilStok.ForeColor = Color.Black;
                btnUrunSilStok.BackColor = Color.Transparent;
                btnKaydetStok.Enabled = false;
                btnIptalStok.Enabled = false;
            }
        }

        private void btnYeniKayitMusteri_CheckedChanged(object sender, EventArgs e)
        {
            if (btnYeniKayitMusteri.Checked == true)
            {
                btnYeniKayitMusteri.ForeColor = Color.White;
                btnYeniKayitMusteri.BackColor = Color.DarkRed;
                btnKaydetMusteri.Enabled = true;
                btnIptalMusteri.Enabled = true;
                btnResimSec.Enabled = true;
                musteriAktiflestir();
                musteriGiristemizle();
                tblMusteriler.Sort(tblMusteriler.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
                if (tblMusteriler.Rows.Count > 0) txtMusteriKodu.Text = (Convert.ToInt32(tblMusteriler.Rows[tblMusteriler.RowCount - 1].Cells[0].Value) + 1).ToString();
                else txtMusteriKodu.Text = "1";

            }
            else
            {
                txtMusteriKodu.Clear();
                btnYeniKayitMusteri.ForeColor = Color.Black;
                btnYeniKayitMusteri.BackColor = Color.Transparent;
                btnKaydetMusteri.Enabled = false;
                btnIptalMusteri.Enabled = false;
            }
        }
        private void btnGuncelleMusteri_CheckedChanged(object sender, EventArgs e)
        {
            if (btnGuncelleMusteri.Checked == true)
            {
                btnGuncelleMusteri.ForeColor = Color.White;
                btnGuncelleMusteri.BackColor = Color.DarkRed;
                btnKaydetMusteri.Enabled = true;
                btnIptalMusteri.Enabled = true;
                btnResimSec.Enabled = true;
                musteriAktiflestir();
            }
            else
            {
                btnGuncelleMusteri.ForeColor = Color.Black;
                btnGuncelleMusteri.BackColor = Color.Transparent;
                btnKaydetMusteri.Enabled = false;
                btnIptalMusteri.Enabled = false;
            }
        }
        private void btnSilMusteri_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSilMusteri.Checked == true)
            {
                btnSilMusteri.ForeColor = Color.White;
                btnSilMusteri.BackColor = Color.DarkRed;
                btnKaydetMusteri.Enabled = true;
                btnIptalMusteri.Enabled = true;
                btnResimSec.Enabled = true;
            }
            else
            {
                btnSilMusteri.ForeColor = Color.Black;
                btnSilMusteri.BackColor = Color.Transparent;
                btnKaydetMusteri.Enabled = false;
                btnIptalMusteri.Enabled = false;
            }
        }
        public void musteriGiristemizle()
        {
            txtMusteriKodu.Clear();
            txtAdiMusteri.Clear();
            txtAdres.Clear();
            txtTelefonMusteri.Clear();
            txtGSMMusteri.Clear();
            txtVergiDairesiMusteri.Clear();
            txtVergiNoMusteri.Clear();
            txtTCNoMusteri.Clear();
            cmbGrubuMusteriBilgileri.SelectedIndex = -1;
            cmbGrubuMusteriBilgileri.SelectedIndex = -1;
            cmbHesapTuruMusteriBilgileri.SelectedIndex = -1;
            pcbMusteri.Image = Image.FromFile(Application.StartupPath + "\\Images\\Musteriler\\resim.png");
            dosyayol = Application.StartupPath + "\\Images\\Musteriler\\resim.png";
        }

        public void resimSec(PictureBox pcb)
        {
            string filename = "";
            long boyut = 0;
            try
            {
                if (resimSecici.ShowDialog() == DialogResult.OK)
                {
                    filename = resimSecici.FileName;
                    FileInfo info = new FileInfo(filename);
                    boyut = info.Length;
                    if (boyut > 204799) MessageBox.Show("Resim boyutu 200kb'tan büyük olamaz!", "Resim Boyutu Büyük", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        dosyayol = filename;
                        //Daha sonra aynı dosya ile tekrar işlem yapabilmek için filestream kullanıldı.
                        using (FileStream fs = new FileStream(dosyayol, FileMode.Open, FileAccess.Read))
                        {
                            pcb.Image = Image.FromStream(fs);
                            fs.Dispose();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Dosya hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResimSecMusteri_Click(object sender, EventArgs e)
        {
            resimSec(pcbMusteri);
        }
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            resimSec(pcbUrun);
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string stokid = "";
            string cmd = "";
            string grubu = "";
            string birimi = "";
            string stokkodu = txtStokKodu.Text.Trim();
            string alisfiyati = txtAlisFiyati.Text.Trim();
            string satisfiyati1 = txtSatisFiyati1.Text.Trim();
            string satisfiyati2 = txtSatisFiyati2.Text.Trim();
            string kdv = txtKDV.Text.Trim();
            string otv = txtOTV.Text.Trim();
            string kritik = txtKritikSeviye.Text.Trim();
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            baglanti.Open();
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            if (tblStok.Enabled == false)
            {
                try
                {
                    if (cmbGrup.SelectedIndex != -1) grubu = ",Grubu='" + cmbGrup.SelectedItem.ToString() + "'";
                    if (cmbBirim.SelectedIndex != -1) birimi = ",Birim='" + cmbBirim.SelectedItem.ToString() + "'";
                    if (!stokkodu.Equals("")) stokkodu = ",StokKodu='" + stokkodu + "'";
                    if (!alisfiyati.Equals("")) alisfiyati = ",AlisFiyati=" + alisfiyati.Trim('.').Replace(",", ".");
                    if (!satisfiyati1.Equals("")) satisfiyati1 = ",SatisFiyati1=" + satisfiyati1.Trim('.').Replace(",", ".");
                    if (!satisfiyati2.Equals("")) satisfiyati2 = ",SatisFiyati2=" + satisfiyati2.Trim('.').Replace(",", ".");
                    if (!kdv.Equals("")) kdv = ",KDV=" + kdv;
                    if (!otv.Equals("")) otv = ",OTV=" + otv;
                    if (!kritik.Equals("")) kritik = ",KritikSeviye=" + kritik;
                    for (int i = 0; i < tblStok.SelectedRows.Count; i++)
                    {
                        stokid = tblStok.SelectedRows[i].Cells[0].Value.ToString();
                        cmd = "update Stok set" + grubu + birimi + stokkodu + alisfiyati + satisfiyati1 + satisfiyati2 + kdv + otv + kritik + " where StokID =" + stokid + ";";
                        cmd = cmd.Replace("set,", "set ");
                        komut.CommandText = cmd;
                        komut.ExecuteNonQuery();
                    }
                    btnIptalStok.PerformClick();
                    stokArama();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                resimYol = Application.StartupPath + "\\Images\\Urunler\\" + txtBarkodNoStokBilgi.Text + ".jpg";
                if (txtBarkodNoStokBilgi.Text.Trim() == "" || txtUrunAdi.Text.Trim(' ') == "" || cmbBirim.SelectedIndex == -1 || cmbGrup.SelectedIndex == -1 || txtAlisFiyati.Text == "" || txtSatisFiyati1.Text == "" || txtSatisFiyati2.Text == "" || txtKDV.Text == "" || txtOTV.Text == "" || txtKritikSeviye.Text == "")
                {
                    MessageBox.Show("Girişler boş bırakılamaz!", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        komut.CommandText = "select barkod from stok where barkod='" + txtBarkodNoStokBilgi.Text + "'";
                        if (btnYeniUrunStok.Checked == true)
                        {
                            if (demo && demoKontrol())
                            {
                                MessageBox.Show("Programın DEMO versiyonunu kullanmaktasınız!\nEn fazla 5 ürün kaydı girebilirsiniz.\nProgramı satın almak ve sınırsız ürün kaydı girebilmek için bize ulaşınız.\nyazilimmarket.com GENESIS Teknoloji", "DEMO Versiyon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                System.Diagnostics.Process.Start("http://www.genesisteknoloji.com");
                            }
                            else
                            {
                                SqlDataReader dr = komut.ExecuteReader();
                                if (dr.Read())
                                {
                                    MessageBox.Show("Bu barkod numarası ile bir ürün zaten kayıtlı!", "Barkod No Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    dr.Close();
                                    komut.CommandText = "insert into Stok(Adi, Grubu, Barkod, StokKodu, Miktar, Birim, AlisFiyati, SatisFiyati1,SatisFiyati2, KDV, OTV,KritikSeviye) values " +
                                    "(@adi,@grup,@barkod,@stokkodu,@miktar,@birim,@alisfiyati,@satisfiyati1,@satisfiyati2,@kdv,@otv,@kritikseviye)";
                                    komut.Parameters.AddWithValue("@adi", txtUrunAdi.Text);
                                    komut.Parameters.AddWithValue("@grup", cmbGrup.SelectedItem.ToString());
                                    komut.Parameters.AddWithValue("@barkod", txtBarkodNoStokBilgi.Text);
                                    komut.Parameters.AddWithValue("@stokkodu", txtStokKodu.Text);
                                    komut.Parameters.AddWithValue("@miktar", 0);
                                    komut.Parameters.AddWithValue("@birim", cmbBirim.SelectedItem.ToString());
                                    komut.Parameters.AddWithValue("@alisfiyati", Convert.ToDouble(txtAlisFiyati.Text.Trim('.')));
                                    komut.Parameters.AddWithValue("@satisfiyati1", Convert.ToDouble(txtSatisFiyati1.Text.Trim('.')));
                                    komut.Parameters.AddWithValue("@satisfiyati2", Convert.ToDouble(txtSatisFiyati2.Text.Trim('.')));
                                    komut.Parameters.AddWithValue("@kdv", Convert.ToInt32(txtKDV.Text));
                                    komut.Parameters.AddWithValue("@otv", Convert.ToInt32(txtOTV.Text));
                                    komut.Parameters.AddWithValue("@kritikseviye", Convert.ToInt32(txtKritikSeviye.Text));
                                    komut.ExecuteNonQuery();
                                    rsmKontrol();
                                }
                            }
                        }
                        else if (btnUrunGuncelleStok.Checked == true)
                        {
                            komut.CommandText = "update Stok set Adi = @adi, Grubu =@grup , StokKodu =@stokkodu , Birim =@birim , AlisFiyati =@alisfiyati , SatisFiyati1 =@satisfiyati1 , SatisFiyati2 = @satisfiyati2, KDV =@kdv , OTV =@otv ,KritikSeviye=@kritikseviye where Barkod =@barkod ; ";
                            komut.Parameters.AddWithValue("@adi", txtUrunAdi.Text);
                            komut.Parameters.AddWithValue("@grup", cmbGrup.SelectedItem.ToString());
                            komut.Parameters.AddWithValue("@barkod", txtBarkodNoStokBilgi.Text);
                            komut.Parameters.AddWithValue("@stokkodu", txtStokKodu.Text);
                            komut.Parameters.AddWithValue("@birim", cmbBirim.SelectedItem.ToString());
                            komut.Parameters.AddWithValue("@alisfiyati", Convert.ToDouble(txtAlisFiyati.Text.Trim('.')));
                            komut.Parameters.AddWithValue("@satisfiyati1", Convert.ToDouble(txtSatisFiyati1.Text.Trim('.')));
                            komut.Parameters.AddWithValue("@satisfiyati2", Convert.ToDouble(txtSatisFiyati2.Text.Trim('.')));
                            komut.Parameters.AddWithValue("@kdv", Convert.ToInt32(txtKDV.Text));
                            komut.Parameters.AddWithValue("@otv", Convert.ToInt32(txtOTV.Text));
                            komut.Parameters.AddWithValue("@kritikseviye", Convert.ToInt32(txtKritikSeviye.Text));
                            komut.ExecuteNonQuery();
                            rsmKontrol();
                        }
                        else if (btnUrunSilStok.Checked == true)
                        {
                            komut.CommandText = "delete from Stok where Barkod='" + txtBarkodNoStokBilgi.Text + "';";
                            komut.ExecuteNonQuery();
                        }
                        baglanti.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    resimCek(grupID);
                    kritikseviyeGuncelle();
                    //stokAramaTemizle();
                    //cmbDoldurStok();
                    StokGiristemizle();
                    stokPasiflestir();
                    btnYeniUrunStok.Checked = false;
                    btnUrunGuncelleStok.Checked = false;
                    btnUrunSilStok.Checked = false;
                    btnUrunGuncelleStok.Focus();
                    txtStokArama.Focus();
                }
            }
        }

        public void rsmKontrol()
        {
            if (dosyayol != "" && dosyayol != Application.StartupPath + "\\Images\\Icon\\resim.png")
            {
                if (resimYol.Substring(resimYol.Length - 20, 20) == dosyayol.Substring(dosyayol.Length - 20, 20)) MessageBox.Show("Seçtiğiniz resim zaten listede kayıtlı!\nLütfen farklı bir klasörden resim seçiniz.", "Dosya Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (File.Exists(resimYol))
                    {
                        File.Delete(resimYol);
                    }
                    File.Copy(dosyayol, resimYol);
                    dosyayol = "";
                }
            }
        }
        private void btnIptal_Click(object sender, EventArgs e)
        {
            StokGiristemizle();
            stokPasiflestir();
            tblStok.Enabled = true;
            btnYeniUrunStok.Checked = false;
            btnUrunGuncelleStok.Checked = false;
            btnUrunSilStok.Checked = false;
            btnKaydetStok.Enabled = false;
            btnIptalStok.Enabled = false;
        }

        private void btnGrupEkle_Click(object sender, EventArgs e)
        {
            cmbEkleme(cmbGrup, "Grup", btnGrupEkle);
        }

        private void btnBirimEkle_Click(object sender, EventArgs e)
        {
            cmbEkleme(cmbBirim, "Birim", btnBirimEkle);
        }
        void kdvSec()
        {
            if (txtKDV.Text == "0")
            {
                rbKDV0.Checked = true;
            }
            else if (txtKDV.Text == "1")
            {
                rbKDV1.Checked = true;
            }
            else if (txtKDV.Text == "8")
            {
                rbKDV8.Checked = true;
            }
            else if (txtKDV.Text == "18")
            {
                rbKDV18.Checked = true;
            }
        }
        void stokHareketAktiflestir()
        {
            txtAciklamaStokHareket.Enabled = true;
            txtMiktarStokHareket.Enabled = true;
            cmbIslemTuruStokHareket.Enabled = true;
            cmbIslemTuruStokHareket.SelectedIndex = 2;
            rbKasaKaydıAktif.Enabled = true;
            rbKasaKaydıPasif.Enabled = true;
            btnStokHareketSil.Enabled = false;
            lblTutarStokHareket.Text = "0,00 TL";
            cmbNakitPOSStokHareket.Enabled = true;
            cmbNakitPOSStokHareket.SelectedIndex = 0;
        }

        public void stokTabloSecim()
        {
            if (tblStok.Rows.Count != 0)
            {
                txtBarkodNoStokBilgi.Text = tblStok.CurrentRow.Cells[3].Value.ToString();
                txtBarkodStokHareket.Text = tblStok.CurrentRow.Cells[3].Value.ToString();
                txtStokKodu.Text = tblStok.CurrentRow.Cells[4].Value.ToString();
                txtUrunAdi.Text = tblStok.CurrentRow.Cells[1].Value.ToString();
                txtUrunAdiStokHareket.Text = tblStok.CurrentRow.Cells[1].Value.ToString();
                txtAlisFiyati.Text = tblStok.CurrentRow.Cells[8].Value.ToString();
                txtSatisFiyati1.Text = tblStok.CurrentRow.Cells[9].Value.ToString();
                txtSatisFiyati2.Text = tblStok.CurrentRow.Cells[10].Value.ToString();
                txtKDV.Text = tblStok.CurrentRow.Cells[11].Value.ToString();
                txtOTV.Text = tblStok.CurrentRow.Cells[12].Value.ToString();
                cmbGrup.SelectedItem = tblStok.CurrentRow.Cells[2].Value.ToString();
                cmbBirim.SelectedItem = tblStok.CurrentRow.Cells[6].Value.ToString();
                txtKritikSeviye.Text = tblStok.CurrentRow.Cells[7].Value.ToString();
                kdvSec();
                stokHareketAktiflestir();

                resimYol = Application.StartupPath + "\\Images\\Urunler\\" + txtBarkodNoStokBilgi.Text + ".jpg";
                if (File.Exists(resimYol) && btnYeniUrunStok.Checked == false)
                {
                    //Daha sonra aynı dosya ile tekrar işlem yapabilmek için filestream kullanıldı.
                    using (FileStream fs = new FileStream(resimYol, FileMode.Open, FileAccess.Read))
                    {
                        pcbUrun.Image = Image.FromStream(fs);
                        fs.Dispose();
                    }
                }
                else
                {
                    pcbUrun.Image = Image.FromFile(Application.StartupPath + "\\Images\\Icon\\resim.png");
                }
                tutarHesaplaStokHareket();
            }
        }

        private void tblStok_Click(object sender, EventArgs e)
        {
            stokTabloSecim();
        }
        private void txtAlisFiyati_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sadece rakam ve virgül girişi.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',';
        }

        private void txtAlisFiyati_Leave(object sender, EventArgs e)
        {
            //Otomatik ondalık çevirici.
            try
            {
                txtAlisFiyati.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtAlisFiyati.Text));
            }
            catch
            {
                txtAlisFiyati.Text = "0,00";
            }

        }

        private void txtSatisFiyati_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sadece rakam ve virgül girişi.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',';
        }

        private void txtSatisFiyati_Leave(object sender, EventArgs e)
        {
            //Otomatik ondalık çevirici.
            try
            {
                txtSatisFiyati1.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtSatisFiyati1.Text));
            }
            catch
            {
                txtSatisFiyati1.Text = "0,00";
            }
            try
            {
                txtMiktarStokHareket.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtMiktarStokHareket.Text));
            }
            catch
            {
                txtMiktarStokHareket.Text = "0,00";
            }
            try
            {
                txtPnlNakitPOSNakit.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPnlNakitPOSNakit.Text));
            }
            catch
            {
                txtPnlNakitPOSNakit.Text = "0,00";
            }
            try
            {
                txtPnlNakitPOSPOS.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPnlNakitPOSPOS.Text));
            }
            catch
            {
                txtPnlNakitPOSPOS.Text = "0,00";
            }
            try
            {
                txtTutarCariHareket.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtTutarCariHareket.Text));
            }
            catch
            {
                txtTutarCariHareket.Text = "0,00";
            }
            try
            {
                txtKasaMiktar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtKasaMiktar.Text));
            }
            catch
            {
                txtKasaMiktar.Text = "0,00";
            }
        }

        private void txtKDV_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sadece rakam girişi.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBarkodNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Sadece rakam girişi.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmbAramaGrup_SelectedIndexChanged(object sender, EventArgs e)
        {
            stokArama();
        }
        private void cmbAramaStokKodu_SelectedIndexChanged(object sender, EventArgs e)
        {
            stokArama();
        }
        private void txtStokArama_TextChanged(object sender, EventArgs e)
        {
            stokArama();
        }
        private void btnTumu_Click(object sender, EventArgs e)
        {
            stokAramaTemizle();
        }
        void musteriAktiflestir()
        {
            txtAdiMusteri.Enabled = true;
            txtAdres.Enabled = true;
            txtTelefonMusteri.Enabled = true;
            txtGSMMusteri.Enabled = true;
            txtVergiDairesiMusteri.Enabled = true;
            txtVergiNoMusteri.Enabled = true;
            txtTCNoMusteri.Enabled = true;
            cmbGrubuMusteriBilgileri.Enabled = true;
            cmbGrubuMusteriBilgileri.Enabled = true;
            cmbHesapTuruMusteriBilgileri.Enabled = true;
            btnGrupEkleMusteri.Enabled = true;
            btnResimSecMusteri.Enabled = true;
            btnKaydetMusteri.Enabled = true;
            btnIptalMusteri.Enabled = true;
            cbCariHesap.Enabled = true;
        }
        void musteriPasiflestir()
        {
            txtAdiMusteri.Enabled = false;
            txtAdres.Enabled = false;
            txtTelefonMusteri.Enabled = false;
            txtGSMMusteri.Enabled = false;
            txtVergiDairesiMusteri.Enabled = false;
            txtVergiNoMusteri.Enabled = false;
            txtTCNoMusteri.Enabled = false;
            cmbGrubuMusteriBilgileri.Enabled = false;
            cmbGrubuMusteriBilgileri.Enabled = false;
            cmbHesapTuruMusteriBilgileri.Enabled = false;
            btnGrupEkleMusteri.Enabled = false;
            btnResimSecMusteri.Enabled = false;
            btnKaydetMusteri.Enabled = false;
            btnIptalMusteri.Enabled = false;
            btnYeniKayitMusteri.Checked = false;
            btnGuncelleMusteri.Checked = false;
            btnSilMusteri.Checked = false;
            cbCariHesap.Enabled = false;
        }

        void tutarHesaplaStokHareket()
        {
            try
            {
                double fiyat = 0;
                double miktar = Convert.ToDouble(txtMiktarStokHareket.Text);
                double tutar = 0;
                if (miktar > 0)
                {
                    if (cmbIslemTuruStokHareket.SelectedIndex == 0 || cmbIslemTuruStokHareket.SelectedIndex == 1)
                    {
                        fiyat = Convert.ToDouble(tblStok.CurrentRow.Cells[9].Value);
                    }
                    else fiyat = Convert.ToDouble(tblStok.CurrentRow.Cells[8].Value);
                    tutar = miktar * fiyat;
                    lblTutarStokHareket.Text = string.Format("{0:#,##0.00}", tutar) + " TL";
                }
            }
            catch
            {
                lblTutarStokHareket.Text = "0,00 TL";
            }
        }
        void stokHareketTemizle()
        {
            cmbNakitPOSStokHareket.Enabled = false;
            txtAciklamaStokHareket.Enabled = false;
            txtMiktarStokHareket.Enabled = false;
            btnStokGirisi.Enabled = false;
            btnStokCikisi.Enabled = false;
            cmbIslemTuruStokHareket.Enabled = false;
            rbKasaKaydıAktif.Enabled = false;
            rbKasaKaydıPasif.Enabled = false;
            btnStokHareketSil.Enabled = true;
            txtFaturaNoStokHareket.Clear();
            txtFaturaNoStokHareket.Enabled = false;
            txtBarkodStokHareket.Text = "";
            txtUrunAdiStokHareket.Text = "";
            txtMiktarStokHareket.Text = "0,00";
            cmbIslemTuruStokHareket.SelectedIndex = -1;
            cmbNakitPOSStokHareket.SelectedIndex = 0;
        }
        private void btnStokGirisi_Click(object sender, EventArgs e)
        {
            stokHareketKaydı("+", " (" + cmbNakitPOSStokHareket.SelectedItem.ToString() + ")");
        }

        private void btnStokHareketSil_Click(object sender, EventArgs e)
        {
            if (tblStokHareket.RowCount > 0)
            {
                DialogResult sonuc = new DialogResult();
                sonuc = MessageBox.Show("Seçili Stok Hareket Kaydını silmek istediğinize emin misiniz?", "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection baglanti = new SqlConnection(baglantiadresi);
                        SqlCommand komut = new SqlCommand("delete from StokHareket where HareketID=@id", baglanti);
                        komut.Parameters.AddWithValue("@id", tblStokHareket.CurrentRow.Cells[0].Value);
                        baglanti.Open();
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    stokHareketAramaTemizle();
                }
            }
        }

        private void tblStokHareket_Click(object sender, EventArgs e)
        {
            stokHareketTemizle();
        }

        private void btnStokCikisi_Click(object sender, EventArgs e)
        {
            stokHareketKaydı("-", " (" + cmbNakitPOSStokHareket.SelectedItem.ToString() + ")");
        }
        public void musteriArama()
        {
            string grup = "";
            string hesapturu = "";
            if (cmbMusteriGrubuArama.SelectedIndex != -1) grup = " and Grubu=@grup";
            if (cmbHesapTuru.SelectedIndex != -1) hesapturu = " and Tur=@tur ";
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand("select MusteriID[Kod],Adi[Müşteri Adı],Grubu,Tur[Hesap Türü],Telefon[Telefon No],GSM,Adres,VergiDairesi[Vergi Dairesi],VergiNo[Vergi No],TCNo[TC No]" +
                " from Musteriler where (Adi like '%'+@metin+'%' or Telefon like '%'+@metin+'%' or GSM like '%'+@metin+'%' or Adres like '%'+@metin+'%' or VergiDairesi like '%'+@metin+'%' or VergiNo like '%'+@metin+'%'" +
                "or TCNo like '%'+@metin+'%')" + grup + hesapturu, baglanti);
                if (grup != "") komut.Parameters.AddWithValue("@grup", cmbMusteriGrubuArama.SelectedItem);
                if (hesapturu != "") komut.Parameters.AddWithValue("@tur", cmbHesapTuru.SelectedItem);
                komut.Parameters.AddWithValue("@metin", txtMusteriArama.Text);
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblMusteriler.DataSource = dt;
                baglanti.Close();
                tblMusteriler.AutoResizeColumns();
                txtMusteriToplam.Text = tblMusteriler.RowCount.ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //---------------------------------------------------------------------------------------------------------------
        public void hesapArama()
        {
            string hesapturu = "";
            string sonodeme = "";
            string tarih = "";
            if (cbTarihHesapArama.Checked == true) tarih = "and SonIslemTarihi between @dtp1 and @dtp2;";
            if (cbOdemeGunuHesaplar.Checked == true) sonodeme = "and (GETDATE()>SonOdemeTarihi)";
            if (cmbHesapTuruHesaplar.SelectedIndex != -1) hesapturu = " and Tur=@tur ";
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand("select MusteriID[Kod],Adi[Adı],Tur[Hesap Türü],SonIslemTarihi[Son İşlem Tarihi],SonOdemeTarihi[Son Ödeme Tarihi],Bakiye from Musteriler where Adi like '%'+@metin+'%' " + hesapturu + sonodeme + " and Hesap=1 " + tarih, baglanti);
                if (hesapturu != "") komut.Parameters.AddWithValue("@tur", cmbHesapTuruHesaplar.SelectedItem);
                if (tarih != "")
                {
                    komut.Parameters.AddWithValue("@dtp1", dtp1SonIslem.Value);
                    komut.Parameters.AddWithValue("@dtp2", dtp2SonIslem.Value);
                }
                komut.Parameters.AddWithValue("@metin", txtHesapArama.Text);
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblHesaplar.DataSource = dt;
                baglanti.Close();
                tblHesaplar.AutoResizeColumns();
                tblHesaplar.AutoResizeColumnHeadersHeight();
                tblHesaplar.Columns[1].Width = 398;
                altBilgiHesaplar();
                cariRenk();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void cariRenk()
        {
            DateTime sonOdeme;
            for (int i = 0; i < tblHesaplar.RowCount; i++)
            {
                if (tblHesaplar.Rows[i].Cells[4].Value.ToString() != "")
                {
                    sonOdeme = Convert.ToDateTime(tblHesaplar.Rows[i].Cells[4].Value);
                    if (sonOdeme < DateTime.Today) tblHesaplar.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }
        public void cariArama()
        {
            if (tblHesaplar.RowCount > 0)
            {
                string islemturu = "";
                string sonodeme = "";
                string tarih = "";
                if (cbTarihCariArama.Checked == true) tarih = "and IslemTarihi between @dtp1 and @dtp2";
                if (cbOdemeGunuCariHareket.Checked == true) sonodeme = "and (GETDATE()>SonOdemeTarihi)";
                if (cmbIslemTuruCariHareket.SelectedIndex != -1) islemturu = " and Tur LIKE '%'+@tur+'%' ";
                try
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    SqlCommand komut = new SqlCommand("select CariNo[İşlem No],Tur[İşlem Türü],Aciklama[Açıklama],SonOdemeTarihi[Son Ödeme Tarihi],Borc[Borç],Tahsilat,NakitPOS[Nakit-POS],IslemTarihi[İşlem Tarihi] from CariHareket where MusteriID=@musteriId and Aciklama like '%'+@metin+'%' " + islemturu + sonodeme + tarih, baglanti);
                    if (islemturu != "") komut.Parameters.AddWithValue("@tur", cmbIslemTuruCariHareket.SelectedItem);
                    if (tarih != "")
                    {
                        komut.Parameters.AddWithValue("@dtp1", dtp1CariIslemTarihi.Value);
                        komut.Parameters.AddWithValue("@dtp2", dtp2CariIslemTarihi.Value);
                    }
                    komut.Parameters.AddWithValue("@metin", txtCariHareketArama.Text);
                    komut.Parameters.AddWithValue("@musteriId", tblHesaplar.CurrentRow.Cells[0].Value);
                    baglanti.Open();
                    SqlDataAdapter da = new SqlDataAdapter(komut);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    tblCariHareketler.DataSource = dt;
                    baglanti.Close();
                    tblCariHareketler.AutoResizeColumns();
                    tblCariHareketler.AutoResizeColumnHeadersHeight();
                    tblCariHareketler.Columns[2].Width = 250;
                    tblCariHareketler.Sort(tblCariHareketler.Columns[0], System.ComponentModel.ListSortDirection.Descending);
                    altBilgiCari();
                    cariHareketRenk();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void kasaRenk()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                baglanti.Open();
                komut.CommandText = "select distinct FaturaNo from Fatura";
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    for (int i = 0; i < tblKasa.RowCount; i++)
                    {
                        if (dr[0].ToString() == tblKasa.Rows[i].Cells[0].Value.ToString())
                        {
                            tblKasa.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                    }
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void kasaArama()
        {
            string islemturu = "";
            string kullanici = "";
            string nakitpos = "";
            string tarih = "";
            if (cmbKasaIslemTuru.SelectedIndex != -1) islemturu = " and Tur=@tur";
            if (cbKasaTarih.Checked == true) tarih = " and ( Tarih between @dtp1 and @dtp2 )";
            if (cmbKasaKasiyer.SelectedIndex != -1) kullanici = " and Kullanici=@kullanici";
            if (cmbKasaAramaNakitPOS.SelectedIndex != -1) nakitpos = " and NakitPOS like '%'+@nakitpos+'%'";
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select IslemNo[İşlem/Fiş No],Tur[İşlem Türü],Aciklama[Açıklama],NakitPOS[Nakit-POS],Miktar,Tarih[İşlem Tarihi],Kullanici[İşlemi Yapan] from Kasa where( Aciklama like '%'+@metin+'%' or IslemNo like '%'+@metin+'%')" + nakitpos + islemturu + kullanici + tarih + ";";
                komut.Parameters.AddWithValue("@metin", txtKasaHareketArama.Text);
                if (islemturu != "") komut.Parameters.AddWithValue("@tur", cmbKasaIslemTuru.SelectedItem.ToString());
                if (kullanici != "") komut.Parameters.AddWithValue("@kullanici", cmbKasaKasiyer.SelectedItem.ToString());
                if (nakitpos != "") komut.Parameters.AddWithValue("@nakitpos", cmbKasaAramaNakitPOS.SelectedItem.ToString());
                if (tarih != "")
                {
                    komut.Parameters.AddWithValue("@dtp1", dtp1Kasa.Value);
                    komut.Parameters.AddWithValue("@dtp2", dtp2Kasa.Value);
                }
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblKasa.DataSource = dt;
                baglanti.Close();
                tblKasa.AutoResizeColumns();
                tblKasa.AutoResizeColumnHeadersHeight();
                tblKasa.Columns[2].Width = 350;
                tblKasa.Sort(tblKasa.Columns[5], System.ComponentModel.ListSortDirection.Descending);
                altBilgiKasa();
                kasaRenk();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void altBilgiSatisRapor()
        {
            double miktar = 0;
            double alisfiyati = 0;
            double satisfiyati = 0;
            double fark = 0;

            for (int i = 0; i < tblSatisRapor.RowCount; i++)
            {
                miktar += Convert.ToDouble(tblSatisRapor.Rows[i].Cells[5].Value);
                try
                {
                    alisfiyati += Convert.ToDouble(tblSatisRapor.Rows[i].Cells[6].Value) * Convert.ToDouble(tblSatisRapor.Rows[i].Cells[5].Value);
                }
                catch
                {

                }

                satisfiyati += Convert.ToDouble(tblSatisRapor.Rows[i].Cells[7].Value) * Convert.ToDouble(tblSatisRapor.Rows[i].Cells[5].Value);
            }
            fark = satisfiyati - alisfiyati;

            txtSatisRaporToplamMiktar.Text = string.Format("{0:#,##0.00}", miktar);
            txtSatisRaporToplamAlisTutari.Text = string.Format("{0:#,##0.00}", alisfiyati);
            txtSatisRaporToplamSatisTutari.Text = string.Format("{0:#,##0.00}", satisfiyati);
            txtSatisRaporToplamFark.Text = string.Format("{0:#,##0.00}", fark);
            txtSatisRaporToplamKayit.Text = tblSatisRapor.RowCount.ToString();
            if (fark >= 0) txtSatisRaporToplamFark.BackColor = Color.LightGreen;
            else txtSatisRaporToplamFark.BackColor = Color.LightCoral;
        }
        public void satisRaporArama()
        {
            string grubu = "";
            string satisturu = "";
            string zarar = "";
            string gunluk = "";
            string aylik = "";
            string ozeltarih = "";
            if (cmbGrubuSatisRapor.SelectedIndex != -1) grubu = " and Grubu=@grubu";
            if (cmbSatisTuruSatisRapor.SelectedIndex != -1) satisturu = " and Tur=@satisturu";
            if (cbZararEttiklerim.Checked == true) zarar = " and (AlisFiyati>SatisFiyati)";
            if (rbGunlukSatisRapor.Checked == true) gunluk = " and (YEAR(Tarih) = @yil and MONTH(Tarih) = @ay and DAY(Tarih) = @gun)";
            if (rbAylikSatisRapor.Checked == true) aylik = " and (YEAR(Tarih) = @yil and MONTH(Tarih) = @ay)";
            if (rbOzelTarihSatisRapor.Checked == true) ozeltarih = " and (Tarih between @dtp1 and @dtp2)";

            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            try
            {
                baglanti.Open();
                komut.CommandText = "select ROW_NUMBER() over(order by Tarih) as [Sıra No], Adi[Ürün Adı], Grubu, Barkod, Tur[Satış Türü], Miktar, AlisFiyati[Alış Fiyatı], SatisFiyati[Satış Fiyatı], Tarih from StokHareket" +
                    " where Islem like '%Ç%' and (Adi like '%'+@metin+'%' or Barkod= @metin)" + grubu + satisturu + zarar + gunluk + aylik + ozeltarih;
                komut.Parameters.AddWithValue("@metin", txtMetinSatisRapor.Text);
                if (grubu != "") komut.Parameters.AddWithValue("@grubu", cmbGrubuSatisRapor.SelectedItem.ToString());
                if (satisturu != "") komut.Parameters.AddWithValue("@satisturu", cmbSatisTuruSatisRapor.SelectedItem.ToString());
                if (gunluk != "")
                {
                    komut.Parameters.AddWithValue("@yil", dtpGunlukSatisRapor.Value.Year);
                    komut.Parameters.AddWithValue("@ay", dtpGunlukSatisRapor.Value.Month);
                    komut.Parameters.AddWithValue("@gun", dtpGunlukSatisRapor.Value.Day);
                }
                else if (aylik != "")
                {
                    komut.Parameters.AddWithValue("@yil", numYilSatisRapor.Value);
                    komut.Parameters.AddWithValue("@ay", cmbAylarSatisRapor.SelectedIndex + 1);
                }
                else if (ozeltarih != "")
                {
                    komut.Parameters.AddWithValue("@dtp1", dtp1OzelTarihSatisRapor.Value);
                    komut.Parameters.AddWithValue("@dtp2", dtp2OzelTarihSatisRapor.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblSatisRapor.DataSource = dt;
                tblSatisRapor.AutoResizeColumns();
                tblSatisRapor.AutoResizeColumnHeadersHeight();
                tblSatisRapor.Columns[1].Width = 305;
                baglanti.Close();
                altBilgiSatisRapor();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void satisDetayCek()
        {
            try
            {
                string faturano = tblKasa.CurrentRow.Cells[0].Value.ToString();
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select BarkodNo[Barkod No],UrunAdi[Ürün Adı],Miktar,Birim,KDV,Fiyat,Tutar from Fatura where FaturaNo=@faturano";
                komut.Parameters.AddWithValue("@faturano", faturano);
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblSatisDetayi.DataSource = dt;
                baglanti.Close();
                tblSatisDetayi.AutoResizeColumns();
                tblSatisDetayi.AutoResizeColumnHeadersHeight();
                tblSatisDetayi.Columns[1].Width = 277;
                txtPNLSatisFaturaNo.Text = tblKasa.CurrentRow.Cells[0].Value.ToString();
                lblPNLSatisAciklama.Text = tblKasa.CurrentRow.Cells[2].Value.ToString();
                txtPNLSatisDetayiTutar.Text = tblKasa.CurrentRow.Cells[4].Value.ToString();
                lblPNLSatisTarih.Text = tblKasa.CurrentRow.Cells[5].Value.ToString().Substring(0, 16);
                lblPNLSatisNakitPOS.Text = "(" + tblKasa.CurrentRow.Cells[3].Value.ToString() + ")";
                txtPNLSatisDetayiTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLSatisDetayiTutar.Text));
                lblPNLSatisKullanici.Text = tblKasa.CurrentRow.Cells[6].Value.ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void stokHareketArama()
        {
            string islem = "";
            string tur = "";
            string tarih = "";
            string grup = "";
            if (cbTarihStokArama.Checked == true) tarih = " and Tarih between @dtp1 and @dtp2";
            if (cmbHareketTuru.SelectedIndex != -1) islem = cmbHareketTuru.SelectedItem.ToString(); ;
            if (cmbIslemTuruStokHareketArama.SelectedIndex != -1) tur = cmbIslemTuruStokHareketArama.SelectedItem.ToString();
            if (cmbGrubuStokHareket.SelectedIndex != -1) grup = " and Grubu=@grubu";
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                baglanti.Open();
                komut = new SqlCommand("select HareketID[Hareket No],Adi[Ürün Adı],Grubu,Barkod[Barkod No],Miktar,Tur[Tür],Islem[İşlem],Tarih[İşlem Tarihi],Aciklama[Açıklama]" +
                " from StokHareket where (Adi like '%'+@metin+'%' or Barkod= @metin) and Tur like '%'+@tur+'%' and Islem like '%'+@islem+'%'" + grup + tarih + ";", baglanti);
                if (tarih != "")
                {
                    komut.Parameters.AddWithValue("@dtp1", dtp1StokHareket.Value.AddDays(-1));
                    komut.Parameters.AddWithValue("@dtp2", dtp2StokHareket.Value.AddDays(1));
                }
                if (grup != "") komut.Parameters.AddWithValue("@grubu", cmbGrubuStokHareket.SelectedItem.ToString());
                komut.Parameters.AddWithValue("@metin", txtMetinStokHareket.Text);
                komut.Parameters.AddWithValue("@islem", islem);
                komut.Parameters.AddWithValue("@tur", tur);
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblStokHareket.DataSource = dt;
                baglanti.Close();
                tblStokHareket.AutoResizeColumns();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void msiStokExcel_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblStok, true, "STOK KARTLARI");
        }

        private void msiStokHareketExcel_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblStokHareket, true, "STOK HAREKETLERİ");
        }

        private void txtMetinStokHareket_TextChanged(object sender, EventArgs e)
        {
            stokHareketArama();
        }

        private void msiStokExcelSecilen_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblStok, false, "STOK KARTLARI");
        }

        private void msiStokHareketExcelSecilen_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblStokHareket, false, "STOK HAREKETLERİ");
        }
        //--------------------------------------------------------------------------------------MUSTERILER------------------------------------------------------------------------------------
        public void toplamMusteri()
        {
            txtMusteriToplam.Text = tblMusteriler.RowCount.ToString();
        }

        private void cmbAramaGrup_Click(object sender, EventArgs e)
        {
            cmbStokAramaGrup.SelectedIndex = -1;
        }

        private void cmbAramaStokKodu_Click(object sender, EventArgs e)
        {
            cmbAramaStokKodu.SelectedIndex = -1;
        }

        private void seçileniSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            secileniSilStokHareket();
        }

        private void seçileniSilToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            secileniSilStok();
        }

        private void btnIptalMusteri_Click(object sender, EventArgs e)
        {
            musteriGiristemizle();
            musteriPasiflestir();
            btnYeniKayitMusteri.Checked = false;
            btnGuncelleMusteri.Checked = false;
            btnSilMusteri.Checked = false;
        }

        private void btnKaydetMusteri_Click(object sender, EventArgs e)
        {
            resimYol = Application.StartupPath + "\\Images\\Musteriler\\" + txtMusteriKodu.Text + ".jpg";
            int hesap = 0;
            if (cbCariHesap.Checked == true) hesap = 1;
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                baglanti.Open();
                if (btnYeniKayitMusteri.Checked == true)
                {
                    if (txtAdiMusteri.Text.Trim() != "" && cmbGrubuMusteriBilgileri.SelectedIndex != -1 && cmbHesapTuruMusteriBilgileri.SelectedIndex != -1)
                    {
                        SqlCommand komut = new SqlCommand("insert into Musteriler (MusteriID,Adi,Grubu,Tur,Telefon,GSM,Adres,VergiDairesi,VergiNo,TCNo,Hesap,Bakiye) values (@id,@adi,@grubu,@tur,@telefon,@gsm,@adres,@vergidairesi,@vergino,@tcno,@hesap,0);", baglanti);
                        komut.Parameters.AddWithValue("@adi", txtAdiMusteri.Text);
                        komut.Parameters.AddWithValue("@grubu", cmbGrubuMusteriBilgileri.SelectedItem);
                        komut.Parameters.AddWithValue("@tur", cmbHesapTuruMusteriBilgileri.SelectedItem.ToString());
                        komut.Parameters.AddWithValue("@telefon", txtTelefonMusteri.Text);
                        komut.Parameters.AddWithValue("@gsm", txtGSMMusteri.Text);
                        komut.Parameters.AddWithValue("@adres", txtAdres.Text);
                        komut.Parameters.AddWithValue("@vergidairesi", txtVergiDairesiMusteri.Text);
                        komut.Parameters.AddWithValue("@vergino", txtVergiNoMusteri.Text);
                        komut.Parameters.AddWithValue("@tcno", txtTCNoMusteri.Text);
                        komut.Parameters.AddWithValue("@id", txtMusteriKodu.Text);
                        komut.Parameters.AddWithValue("@hesap", hesap);
                        komut.ExecuteNonQuery();
                        rsmKontrol();
                        musteriGiristemizle();
                        musteriPasiflestir();
                    }
                    else
                    {
                        MessageBox.Show("Girişler boş bırakılamaz!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (btnGuncelleMusteri.Checked == true)
                {
                    SqlCommand komut = new SqlCommand("update Musteriler set Adi=@adi, Grubu=@grubu,Tur=@tur,Telefon=@telefon,GSM=@gsm,Adres=@adres,VergiDairesi=@vergidairesi,VergiNo=@vergino,TCNo=@tcno where MusteriID=@id;", baglanti);
                    komut.Parameters.AddWithValue("@adi", txtAdiMusteri.Text);
                    komut.Parameters.AddWithValue("@grubu", cmbGrubuMusteriBilgileri.SelectedItem.ToString());
                    komut.Parameters.AddWithValue("@tur", cmbHesapTuruMusteriBilgileri.SelectedItem.ToString());
                    komut.Parameters.AddWithValue("@telefon", txtTelefonMusteri.Text);
                    komut.Parameters.AddWithValue("@gsm", txtGSMMusteri.Text);
                    komut.Parameters.AddWithValue("@adres", txtAdres.Text);
                    komut.Parameters.AddWithValue("@vergidairesi", txtVergiDairesiMusteri.Text);
                    komut.Parameters.AddWithValue("@vergino", txtVergiNoMusteri.Text);
                    komut.Parameters.AddWithValue("@tcno", txtTCNoMusteri.Text);
                    komut.Parameters.AddWithValue("@id", txtMusteriKodu.Text);
                    komut.ExecuteNonQuery();
                    rsmKontrol();
                    musteriGiristemizle();
                    musteriPasiflestir();
                    //----------------------------------------------------
                }
                else if (btnSilMusteri.Checked == true)
                {
                    SqlCommand komut = new SqlCommand("DELETE FROM Musteriler where MusteriID=@id", baglanti);
                    komut.Parameters.AddWithValue("@id", txtMusteriKodu.Text);
                    DialogResult sonuc = MessageBox.Show(txtAdiMusteri.Text + " adlı müşteri kaydını silmek istediğinize emin misiniz?", "Dikkat!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (sonuc == DialogResult.Yes)
                    {
                        komut.ExecuteNonQuery();
                    }
                    else
                    {
                        btnIptalMusteri.PerformClick();
                    }
                }
                baglanti.Close();
                cmbDoldurMusteri();
                musteriAramaTemizle();
                cmbMusteriArama();
                hesapAramaTemizle();
                cmbMusteriArama();
            }
            catch (SqlException ex)//sa
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void hesapGuncelle(int musterikodu)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select MIN(SonOdemeTarihi) from CariHareket where MusteriID=@id;";
                komut.Parameters.AddWithValue("@id", musterikodu);
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    string sonodemetarihi = dr[0].ToString();
                    dr.Close();
                    if (sonodemetarihi != "")
                    {
                        komut.CommandText = "Update Musteriler set SonOdemeTarihi=@sonodemetarihi where MusteriID=@id";
                        komut.Parameters.AddWithValue("@sonodemetarihi", Convert.ToDateTime(sonodemetarihi));
                        komut.ExecuteNonQuery();
                    }
                    else
                    {
                        komut.CommandText = "Update Musteriler set SonOdemeTarihi=NULL where MusteriID=@id";
                        komut.ExecuteNonQuery();
                    }
                }
                komut.CommandText = "select MAX(IslemTarihi) from CariHareket where MusteriID=@id;";
                SqlDataReader dr2 = komut.ExecuteReader();
                if (dr2.Read())
                {
                    string islemtarihi = dr2[0].ToString();
                    dr2.Close();
                    if (islemtarihi != "")
                    {
                        komut.CommandText = "Update Musteriler set SonIslemTarihi=@islemtarihi where MusteriID=@id";
                        komut.Parameters.AddWithValue("@islemtarihi", Convert.ToDateTime(islemtarihi));
                        komut.ExecuteNonQuery();
                    }
                    else
                    {
                        komut.CommandText = "Update Musteriler set SonIslemTarihi=NULL where MusteriID=@id";
                        komut.ExecuteNonQuery();
                    }
                }
                double bakiye = 0;
                komut.CommandText = "select SUM(Borc)-SUM(Tahsilat) from CariHareket where MusteriID=@id;";
                SqlDataReader dr3 = komut.ExecuteReader();
                if (dr3.Read())
                {
                    string strbakiye = dr3[0].ToString();
                    if (strbakiye == "") bakiye = 0;
                    else
                    {
                        bakiye = Convert.ToDouble(strbakiye);
                    }
                    dr3.Close();
                }
                else dr3.Close();
                komut.CommandText = "Update Musteriler set Bakiye=@bakiye where MusteriID=@id ";
                komut.Parameters.AddWithValue("@bakiye", bakiye);
                komut.ExecuteNonQuery();
                baglanti.Close();
                hesapAramaTemizle();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnTahsilatCari_Click(object sender, EventArgs e)
        {
            string secilikod;
            DialogResult sonuc = DialogResult.Cancel;
            if (btnTahsilatCari.BackColor == Color.LightGreen)
            {
                sonuc = MessageBox.Show("Seçilen " + tblCariHareketler.CurrentRow.Cells[4].Value.ToString() + "TL borç kaydını kapatmak istediğinize emin misiniz?\nBorç tutarında tahsilat kaydı otomatik olarak girilecektir.", "Borç Kapatma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
            if (tblHesaplar.RowCount > 0 && sonuc != DialogResult.No)
            {
                if (Convert.ToDouble(txtTutarCariHareket.Text) <= 0)
                {
                    MessageBox.Show("Lütfen tutar giriniz!", "Tutar Giriniz!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        SqlConnection baglanti = new SqlConnection(baglantiadresi);
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand();
                        if (cbSonOdeme.Checked == true)
                        {
                            komut.CommandText = "insert into CariHareket(MusteriID,Tur,Aciklama,SonOdemeTarihi,Borc,Tahsilat,IslemTarihi,NakitPOS) values (@id,@tur,@aciklama,@sonodeme,@borc,@tahsilat,GETDATE(),@nakitpos)";
                            komut.Parameters.AddWithValue("@sonodeme", dtpSonOdeme.Value);
                        }
                        else
                        {
                            komut.CommandText = "insert into CariHareket(MusteriID,Tur,Aciklama,SonOdemeTarihi,Borc,Tahsilat,IslemTarihi,NakitPOS) values (@id,@tur,@aciklama,null,@borc,@tahsilat,GETDATE(),@nakitpos)";
                        }
                        komut.Connection = baglanti;
                        komut.Parameters.AddWithValue("@id", tblHesaplar.CurrentRow.Cells[0].Value);
                        komut.Parameters.AddWithValue("@tur", "Tahsilat Alımı");
                        komut.Parameters.AddWithValue("@aciklama", txtAciklamaCariHareket.Text);
                        komut.Parameters.AddWithValue("@borc", 0);
                        komut.Parameters.AddWithValue("@tahsilat", Convert.ToDouble(txtTutarCariHareket.Text));
                        komut.Parameters.AddWithValue("@nakitpos", cmbCariParaTur.SelectedItem.ToString());
                        komut.ExecuteNonQuery();
                        int carino = 0;
                        komut.CommandText = "select MAX(CariNo) from CariHareket";
                        SqlDataReader dr = komut.ExecuteReader();
                        if (dr.Read())
                        {
                            carino = Convert.ToInt32(dr[0].ToString());
                        }
                        dr.Close();
                        komut.CommandText = "insert into Kasa(IslemNo,Tur,Aciklama,NakitPOS,Miktar,Tarih,Kullanici) values (@carino,'Giriş',@aciklamakasa,@nakitpos,@tahsilat,GETDATE(),@kullanici)";
                        komut.Parameters.AddWithValue("@carino", carino);
                        komut.Parameters.AddWithValue("@kullanici", cmbKasiyer.SelectedItem.ToString());
                        komut.Parameters.AddWithValue("@aciklamakasa","Tahsilat Alımı: "+ txtAciklamaCariHareket.Text);
                        komut.ExecuteNonQuery();
                        if (btnTahsilatCari.BackColor == Color.LightGreen)
                        {
                            komut.CommandText = "update CariHareket set SonOdemeTarihi=NULL,Aciklama=@aciklama2 where CariNo=@eskicari";
                            komut.Parameters.AddWithValue("@eskicari", Convert.ToInt32(tblCariHareketler.CurrentRow.Cells[0].Value));
                            komut.Parameters.AddWithValue("@aciklama2", tblCariHareketler.CurrentRow.Cells[2].Value.ToString() + "(Ödendi)");
                            komut.ExecuteNonQuery();
                        }
                        baglanti.Close();
                        secilikod = tblHesaplar.CurrentRow.Cells[0].Value.ToString();
                        cariAramaTemizle();
                        hesapGuncelle(Convert.ToInt32(tblHesaplar.CurrentRow.Cells[0].Value));
                        tblCariHareketler.ClearSelection();
                        tblCariHareketler.Rows[tblCariHareketler.RowCount - 1].Selected = true;
                        tblCariHareketler.FirstDisplayedScrollingRowIndex = tblCariHareketler.RowCount - 1;
                        txtTutarCariHareket.Text = "0,00";
                        cbSonOdeme.Checked = false;
                        sonSatirSecCari(secilikod);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnBorcEkleCari_Click(object sender, EventArgs e)
        {
            string secilikod;
            if (tblHesaplar.RowCount > 0)
            {
                if (Convert.ToDouble(txtTutarCariHareket.Text) <= 0)
                {
                    MessageBox.Show("Lütfen tutar giriniz!", "Tutar Giriniz!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        SqlConnection baglanti = new SqlConnection(baglantiadresi);
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand();
                        if (cbSonOdeme.Checked == true)
                        {
                            komut.CommandText = "insert into CariHareket(MusteriID,Tur,Aciklama,SonOdemeTarihi,Borc,Tahsilat,IslemTarihi,NakitPOS) values (@id,@tur,@aciklama,@sonodeme,@borc,@tahsilat,GETDATE(),@nakitpos)";
                            komut.Parameters.AddWithValue("@sonodeme", dtpSonOdeme.Value);
                        }
                        else
                        {
                            komut.CommandText = "insert into CariHareket(MusteriID,Tur,Aciklama,SonOdemeTarihi,Borc,Tahsilat,IslemTarihi,NakitPOS) values (@id,@tur,@aciklama,null,@borc,@tahsilat,GETDATE(),@nakitpos)";
                        }
                        komut.Connection = baglanti;
                        komut.Parameters.AddWithValue("@id", tblHesaplar.CurrentRow.Cells[0].Value);
                        komut.Parameters.AddWithValue("@tur", "Borç Ekleme");
                        komut.Parameters.AddWithValue("@aciklama", txtAciklamaCariHareket.Text);
                        komut.Parameters.AddWithValue("@borc", Convert.ToDouble(txtTutarCariHareket.Text));
                        komut.Parameters.AddWithValue("@tahsilat", 0);
                        komut.Parameters.AddWithValue("@nakitpos", cmbCariParaTur.SelectedItem.ToString());
                        komut.ExecuteNonQuery();
                        int carino = 0;
                        komut.CommandText = "select MAX(CariNo) from CariHareket";
                        SqlDataReader dr = komut.ExecuteReader();
                        if (dr.Read())
                        {
                            carino = Convert.ToInt32(dr[0].ToString());
                        }
                        dr.Close();
                        komut.CommandText = "insert into Kasa(IslemNo,Tur,Aciklama,NakitPOS,Miktar,Tarih,Kullanici) values (@carino,@gc,@tur,@nakitpos,@borc,GETDATE(),@kullanici)";
                        komut.Parameters.AddWithValue("@carino", carino);
                        komut.Parameters.AddWithValue("@kullanici", cmbKasiyer.SelectedItem.ToString());
                        komut.Parameters.AddWithValue("@gc","Çıkış");
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        secilikod = tblHesaplar.CurrentRow.Cells[0].Value.ToString();
                        cariAramaTemizle();
                        hesapGuncelle(Convert.ToInt32(tblHesaplar.CurrentRow.Cells[0].Value));
                        tblCariHareketler.ClearSelection();
                        tblCariHareketler.Rows[tblCariHareketler.RowCount - 1].Selected = true;
                        tblCariHareketler.FirstDisplayedScrollingRowIndex = tblCariHareketler.RowCount - 1;
                        txtTutarCariHareket.Text = "0,00";
                        cbSonOdeme.Checked = false;
                        dtpSonOdeme.Value = DateTime.Today;
                        sonSatirSecCari(secilikod);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void sonSatirSecCari(string secilikod)
        {
            if (tblHesaplar.Rows.Count > 0)
            {
                for (int i = 0; i < tblHesaplar.Rows.Count; i++)
                {
                    if (tblHesaplar.Rows[i].Cells[0].Value.ToString() == secilikod)
                    {
                        tblHesaplar.CurrentCell = tblHesaplar.Rows[i].Cells[0];
                    }
                }
            }
        }

        private void btnGrupEkleMusteri_Click(object sender, EventArgs e)
        {
            cmbEkleme(cmbGrubuMusteriBilgileri, "Grup", btnGrupEkleMusteri);
        }

        private void tblMusteriler_Click(object sender, EventArgs e)
        {
            if (tblMusteriler.RowCount != 0 && btnYeniKayitMusteri.Checked == false)
            {
                txtMusteriKodu.Text = tblMusteriler.CurrentRow.Cells[0].Value.ToString();
                txtAdiMusteri.Text = tblMusteriler.CurrentRow.Cells[1].Value.ToString();
                cmbGrubuMusteriBilgileri.SelectedItem = tblMusteriler.CurrentRow.Cells[2].Value;
                cmbHesapTuruMusteriBilgileri.SelectedItem = tblMusteriler.CurrentRow.Cells[3].Value;
                txtTelefonMusteri.Text = tblMusteriler.CurrentRow.Cells[4].Value.ToString();
                txtGSMMusteri.Text = tblMusteriler.CurrentRow.Cells[5].Value.ToString();
                txtAdres.Text = tblMusteriler.CurrentRow.Cells[6].Value.ToString();
                txtVergiDairesiMusteri.Text = tblMusteriler.CurrentRow.Cells[7].Value.ToString();
                txtVergiNoMusteri.Text = tblMusteriler.CurrentRow.Cells[8].Value.ToString();
                txtTCNoMusteri.Text = tblMusteriler.CurrentRow.Cells[9].Value.ToString();

                resimYol = Application.StartupPath + "\\Images\\Musteriler\\" + txtMusteriKodu.Text + ".jpg";
                if (File.Exists(resimYol) && btnYeniKayitMusteri.Checked == false)
                {
                    //Daha sonra aynı dosya ile tekrar işlem yapabilmek için filetream kullanıldı.
                    using (FileStream fs = new FileStream(resimYol, FileMode.Open, FileAccess.Read))
                    {
                        pcbMusteri.Image = Image.FromStream(fs);
                        fs.Dispose();
                    }
                }
                else
                {
                    pcbMusteri.Image = Image.FromFile(Application.StartupPath + "\\Images\\Musteriler\\resim.png");
                }
            }
        }

        private void txtMusteriArama_TextChanged(object sender, EventArgs e)
        {
            musteriArama();
        }

        private void btnTumuMusteri_Click(object sender, EventArgs e)
        {
            cmbHesapTuru.SelectedIndex = -1;
            cmbMusteriGrubuArama.SelectedIndex = -1;
            txtMusteriArama.Text = "";
            musteriArama();
        }

        private void cmbHesapTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            musteriArama();
        }

        private void cmbMusteriGrubuArama_SelectedIndexChanged(object sender, EventArgs e)
        {
            musteriArama();
        }

        private void cmbIslemTuruStokHareketArama_SelectedIndexChanged(object sender, EventArgs e)
        {
            stokHareketArama();
        }

        private void cmbHareketTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            stokHareketArama();
        }
        private void btnTumuStokHareket_Click(object sender, EventArgs e)
        {
            cbTarihStokArama.Checked = false;
            stokHareketAramaTemizle();
        }
        void tumunuSiyahlastir()
        {
            for (int i = 0; i < kritikseviyePCB.Count; i++)
            {
                isimler[i].ForeColor = Color.Black;
                miktarlar[i].ForeColor = Color.Black;
                fiyatlar[i].ForeColor = Color.Black;
            }
        }

        private void tbMenu_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (tbMenu.SelectedTab == tbHizli)
            {
                tumunuSiyahlastir();
                resimCek(grupID);
                txtBarkod.Focus();
            }
            else if (tbMenu.SelectedTab == tbStok)
            {
                kritikRenk();
                tblStok.ClearSelection();
                tblStokHareket.ClearSelection();
            }
            else if (tbMenu.SelectedTab == tbMusteriler)//MUSTERİLER
            {
                musteriAramaTemizle();
            }
            else if (tbMenu.SelectedTab == tbCari)//CARİ
            {
                hesapAramaTemizle();
                tblHesaplar.ClearSelection();
            }
            else if (tbMenu.SelectedTab == tbKasa)//KASA
            {
                kasaAramaTemizle();
            }

            else if (tbMenu.SelectedTab == tbRapor)//RAPORLAR
            {
                cmbDoldurSatisRapor();
                satisRaporAramaTemizle(true);
                grafikDoldurKar();
                grafikDoldurPasta();
            }
            else if (tbMenu.SelectedTab == tbAyarlar)//AYARLAR
            {
                cmbYazicilar.Items.Clear();
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    cmbYazicilar.Items.Add(printer);
                }
                try
                {
                    cmbYazicilar.SelectedItem = HizliSatis.Properties.Settings.Default.yaziciAdi;
                }
                catch
                {

                }
            }
        }

        void grafikDoldurPasta()
        {

            string deger = "";
            string tur = "";

            if (rbChartMiktar.Checked == true) deger = "Miktar";
            else deger = "Miktar*(SatisFiyati-AlisFiyati)";

            if (rbChartUrun.Checked == true) tur = "Adi";
            else tur = "Grubu";

            chartCokSatanlar.Series["Dilimler"].Points.Clear();
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            baglanti.Open();
            try
            {
                SqlCommand komut = new SqlCommand("select " + tur + ",SUM(" + deger + ") as Sonuc from StokHareket where Islem like '%Ç%' group by " + tur + " order by Sonuc desc;", baglanti);
                SqlDataReader dr = komut.ExecuteReader();
                int sayi = 0;
                while (dr.Read() && sayi < 4)
                {
                    chartCokSatanlar.Series[0].Points.AddY(Math.Round(Convert.ToDouble(dr[1]), 0));
                    chartCokSatanlar.Series[0].Points[chartCokSatanlar.Series[0].Points.Count - 1].LegendText = dr[0].ToString();
                    chartCokSatanlar.Series[0].Points[chartCokSatanlar.Series[0].Points.Count - 1].IsValueShownAsLabel = true;
                    chartCokSatanlar.Series[0].Points[chartCokSatanlar.Series[0].Points.Count - 1].Font = new Font("Arial", 12, FontStyle.Regular);
                    sayi++;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void grafikDoldurKar()
        {
            DateTime ay = DateTime.Today;
            int aysayi = 0;
            chartAylikKar.Series["Aylar"].Points.Clear();
            SqlConnection baglanti = new SqlConnection(baglantiadresi);
            baglanti.Open();
            try
            {
                for (int i = -2; i <= 0; i++)
                {
                    aysayi = ay.AddMonths(i).Month;
                    SqlCommand komut = new SqlCommand("select ISNULL (SUM((SatisFiyati-AlisFiyati)*Miktar),0) from StokHareket where Islem like '%Ç%' and (YEAR(Tarih) = YEAR(GETDATE()) and MONTH(Tarih) = " + aysayi.ToString() + ");", baglanti);
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read()) chartAylikKar.Series["Aylar"].Points.AddXY(cmbAylarSatisRapor.Items[aysayi - 1].ToString(), Math.Round(dr.GetDouble(0), 0));
                    dr.Close();
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrUyari_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < kritikseviyePCB.Count; i++)
            {
                if (Convert.ToDouble(miktarlar[i].Text) < Convert.ToInt32(kritikseviyePCB[i]))
                {
                    uyari(isimler[i], fiyatlar[i], miktarlar[i]);
                }
            }
        }

        private void cmbMusteriHesapArama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                cmbMusteriArama();
            }
        }
        private void txtHesapArama_TextChanged(object sender, EventArgs e)
        {
            hesapArama();
        }

        private void cmbHesapTuruHesaplar_SelectedIndexChanged(object sender, EventArgs e)
        {
            hesapArama();
        }
        private void cbOdemeGunuHesaplar_CheckedChanged(object sender, EventArgs e)
        {
            hesapArama();
        }
        private void btnTumuHesaplar_Click(object sender, EventArgs e)
        {
            cbTarihHesapArama.Checked = false;
            hesapAramaTemizle();
        }
        private void txtCariHareketArama_TextChanged(object sender, EventArgs e)
        {
            cariArama();
        }
        private void cmbIslemTuruCariHareket_SelectedIndexChanged(object sender, EventArgs e)
        {
            cariArama();
        }
        private void btnTumuCariHareket_Click(object sender, EventArgs e)
        {
            cbTarihCariArama.Checked = false;
            cariAramaTemizle();
        }
        private void cbOdemeGunuCariHareket_CheckedChanged(object sender, EventArgs e)
        {
            cariArama();
        }
        private void tblMusteriler_SelectionChanged(object sender, EventArgs e)
        {
            tblMusteriler_Click(sender, e);
        }
        private void tblStok_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            kritikRenk();
        }

        private void cbTarihStokArama_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTarihStokArama.Checked == true)
            {
                dtp1StokHareket.Enabled = true;
                dtp2StokHareket.Enabled = true;
            }
            else
            {
                dtp1StokHareket.Enabled = false;
                dtp2StokHareket.Enabled = false;
            }
            stokHareketArama();
        }

        private void dtp1StokHareket_ValueChanged(object sender, EventArgs e)
        {
            stokHareketArama();
        }

        private void dtp2StokHareket_ValueChanged(object sender, EventArgs e)
        {
            stokHareketArama();
        }

        private void cbTarihHesapArama_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTarihHesapArama.Checked == true)
            {
                dtp1SonIslem.Enabled = true;
                dtp2SonIslem.Enabled = true;
            }
            else
            {
                dtp1SonIslem.Enabled = false;
                dtp2SonIslem.Enabled = false;
            }
            hesapArama();
        }

        private void dtp1SonIslem_ValueChanged(object sender, EventArgs e)
        {
            hesapArama();
        }

        private void dtp2SonIslem_ValueChanged(object sender, EventArgs e)
        {
            hesapArama();
        }

        private void cbTarihCariArama_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTarihCariArama.Checked == true)
            {
                dtp1CariIslemTarihi.Enabled = true;
                dtp2CariIslemTarihi.Enabled = true;
            }
            else
            {
                dtp1CariIslemTarihi.Enabled = false;
                dtp2CariIslemTarihi.Enabled = false;
            }
            cariArama();
        }

        private void dtp1CariIslemTarihi_ValueChanged(object sender, EventArgs e)
        {
            cariArama();
        }

        private void dtp2CariIslemTarihi_ValueChanged(object sender, EventArgs e)
        {
            cariArama();
        }

        private void tblHesaplar_Click(object sender, EventArgs e)
        {
            if (tblHesaplar.Rows.Count > 0)
            {
                cariAramaTemizle();
                txtTutarCariHareket.Enabled = true;
                cmbCariParaTur.Enabled = true;
                txtAciklamaCariHareket.Enabled = true;
                cbSonOdeme.Enabled = true;
                btnBorcEkleCari.Enabled = true;
                btnTahsilatCari.Enabled = true;

                txtTutarCariHareket.Text = "0,00";
                txtAciklamaCariHareket.Text = "";
                cbSonOdeme.Enabled = true;
                txtTutarCariHareket.Enabled = true;
                btnBorcEkleCari.Enabled = true;
                btnTahsilatCari.Text = "Tahsilat Yap";
                btnTahsilatCari.BackColor = Color.Transparent;
            }
        }

        private void cbSonOdeme_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSonOdeme.Checked == true)
            {
                dtpSonOdeme.Enabled = true;
                btnTahsilatCari.Enabled = false;
            }
            else
            {
                dtpSonOdeme.Enabled = false;
                btnTahsilatCari.Enabled = true;
            }
        }
        public string IDBul(string metin, string basla, string bitir)
        {
            string sonuc;
            try
            {
                int IcerikBaslangicIndex = metin.IndexOf(basla) + basla.Length;
                int IcerikBitisIndex = metin.Substring(IcerikBaslangicIndex).IndexOf(bitir);
                sonuc = metin.Substring(IcerikBaslangicIndex, IcerikBitisIndex);
            }
            catch (Exception)
            {
                sonuc = null;
            }
            return sonuc;
        }
        private void btnHesapAc_Click(object sender, EventArgs e)
        {
            if (cmbMusteriHesapAcma.SelectedIndex != -1)
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update Musteriler set Hesap=1 where MusteriId=@id";
                    komut.Parameters.AddWithValue("@id", Convert.ToInt32(IDBul(cmbMusteriHesapAcma.SelectedItem.ToString(), "(", ")")));
                    komut.ExecuteNonQuery();
                    cmbMusteriArama();
                    btnTumuHesaplar.PerformClick();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Önce listeden müşteri seçiniz!", "Müşteri Seçilmedi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHesapSil_Click(object sender, EventArgs e)
        {
            if (tblHesaplar.RowCount > 0)
            {
                string id = tblHesaplar.CurrentRow.Cells[0].Value.ToString();
                string adi = tblHesaplar.CurrentRow.Cells[1].Value.ToString();
                string bakiye = tblHesaplar.CurrentRow.Cells[5].Value.ToString();
                DialogResult sonuc = MessageBox.Show(adi + " adlı hesabın " + bakiye + "TL tutarında bakiyesi mevcuttur.\nBu hesaba bağlı cari hareketler silinecektir..\nHesabı silmek istediğinize emin misiniz?", "Hesap Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection baglanti = new SqlConnection(baglantiadresi);
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand();
                        komut.Connection = baglanti;
                        komut.CommandText = "update Musteriler set Hesap=0,Bakiye=0,SonOdemeTarihi=NULL,SonIslemTarihi=NULL where MusteriId=@id";
                        komut.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                        komut.ExecuteNonQuery();
                        komut.CommandText = "delete from CariHareket where MusteriId=@id ";
                        komut.ExecuteNonQuery();
                        cmbMusteriArama();
                        btnTumuHesaplar.PerformClick();
                        cariArama();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnHesabiBosalt_Click(object sender, EventArgs e)
        {
            if (tblHesaplar.RowCount > 0)
            {
                string id = tblHesaplar.CurrentRow.Cells[0].Value.ToString();
                string adi = tblHesaplar.CurrentRow.Cells[1].Value.ToString();
                string bakiye = tblHesaplar.CurrentRow.Cells[5].Value.ToString();
                DialogResult sonuc = MessageBox.Show(adi + " adlı hesabın " + bakiye + "TL tutarında bakiyesi mevcuttur.\nBu hesaba bağlı cari hareketler silinecek ve bakiyesi sıfırlanacaktır.\nHesabı boşaltmak istediğinize emin misiniz?", "Hesap Boşaltma", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection baglanti = new SqlConnection(baglantiadresi);
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand();
                        komut.Connection = baglanti;
                        komut.CommandText = "update Musteriler set Bakiye=0,SonOdemeTarihi=NULL,SonIslemTarihi=NULL where MusteriId=@id";
                        komut.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                        komut.ExecuteNonQuery();
                        komut.CommandText = "delete from CariHareket where MusteriId=@id ";
                        komut.ExecuteNonQuery();
                        btnTumuHesaplar.PerformClick();
                        cariArama();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnHesapAktar_Click(object sender, EventArgs e)
        {
            if (tblHesaplar.RowCount > 0)
            {
                string id = tblHesaplar.CurrentRow.Cells[0].Value.ToString();
                string adi = tblHesaplar.CurrentRow.Cells[1].Value.ToString();
                if (hesapAktarID1 == "")
                {
                    hesapAktarID1 = id;
                    MessageBox.Show(adi + " adlı hesabın cari hareketlerini aktarmak istediğiniz hesabı seçiniz ve 'Aktarımı Kaydet' butonuna tıklayınız.\nİptal etmek için 'İptal!' butonuna tıklayabilirsiniz.", "Hesap Aktarma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnHesapAktar.Text = "Aktarımı Kaydet";
                    btnHesapAktar.Image = null;
                    btnHesapAktar.BackColor = Color.MediumSpringGreen;
                }
                else
                {
                    try
                    {
                        SqlConnection baglanti = new SqlConnection(baglantiadresi);
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand();
                        komut.Connection = baglanti;
                        komut.CommandText = "update CariHareket set MusteriID=@yeniid where MusteriID=@id";
                        komut.Parameters.AddWithValue("@yeniid", Convert.ToInt32(id));
                        komut.Parameters.AddWithValue("@id", Convert.ToInt32(hesapAktarID1));
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        btnTumuHesaplar.PerformClick();
                        cariArama();
                        hesapGuncelle(Convert.ToInt32(id));
                        hesapGuncelle(Convert.ToInt32(hesapAktarID1));
                        btnIptalHesapAktar.PerformClick();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnIptalHesapAktar_Click(object sender, EventArgs e)
        {
            hesapAktarID1 = "";
            btnHesapAktar.Text = "Hesabı Aktar";
            btnHesapAktar.BackColor = Color.Transparent;
            btnHesapAktar.Image = Image.FromFile(Application.StartupPath + "\\Images\\Icon\\aktar.png");
        }
        private void btnPNLVeresiyeOnayla_Click(object sender, EventArgs e)
        {
            if ((txtMusteriAdiPNLVeresiye.Text.Trim().Equals("") && txtMusteriAdiPNLVeresiye.Visible == true) || (cmbPNLVeresiyeMusteri.SelectedIndex == -1 && txtMusteriAdiPNLVeresiye.Visible == false))
            {
                MessageBox.Show("Müşteri adı giriniz ya da listeden seçiniz!", "Müşteri Seçilmedi!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string faturano=lblFaturaNo.Text;
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                try
                {
                    if (btnYeniPNLVeresiye.Checked == true)
                    {
                        SqlCommand komut2 = new SqlCommand();
                        komut2.Connection = baglanti;
                        int id = 0;
                        komut2.CommandText = "select ISNULL(MAX(MusteriID),0) from Musteriler";
                        SqlDataReader dr = komut2.ExecuteReader();
                        if (dr.Read()) id = dr.GetInt32(0) + 1;
                        dr.Close();
                        komut2.CommandText = "insert into Musteriler (MusteriID,Adi,Grubu,Tur,Bakiye,Hesap) values (@id,@adi,@grubu,@tur,0,1);";
                        komut2.Parameters.AddWithValue("@id", id);
                        komut2.Parameters.AddWithValue("@adi", txtMusteriAdiPNLVeresiye.Text);
                        komut2.Parameters.AddWithValue("@grubu", "Genel");
                        komut2.Parameters.AddWithValue("@tur", "Alıcı");
                        komut2.ExecuteNonQuery();
                        cmbMusteriDoldurPNLVeresiye();
                        btnYeniPNLVeresiye.Checked = false;
                        cmbPNLVeresiyeMusteri.SelectedIndex = cmbPNLVeresiyeMusteri.Items.Count - 1;
                    }
                    hizliSatis("Veresiye", "");

                    if (cbPNLVeresiye.Checked == true)
                    {
                        komut.CommandText = "insert into CariHareket(MusteriID,Tur,Aciklama,SonOdemeTarihi,Borc,Tahsilat,IslemTarihi) values (@id,@tur,@aciklama,@sonodeme,@borc,@tahsilat,GETDATE())";
                        komut.Parameters.AddWithValue("@sonodeme", dtpPNLVeresiye.Value);
                    }
                    else
                    {
                        komut.CommandText = "insert into CariHareket(MusteriID,Tur,Aciklama,SonOdemeTarihi,Borc,Tahsilat,IslemTarihi) values (@id,@tur,@aciklama,null,@borc,@tahsilat,GETDATE())";
                    }
                    komut.Parameters.AddWithValue("@id", IDBul(cmbPNLVeresiyeMusteri.SelectedItem.ToString(), "(", ")"));
                    komut.Parameters.AddWithValue("@tur", "Veresiye Satış ("+faturano+")");
                    komut.Parameters.AddWithValue("@aciklama", txtPNLVeresiyeAciklama.Text);
                    komut.Parameters.AddWithValue("@borc", Convert.ToDouble(txtPNLVeresiyeTutar.Text));
                    komut.Parameters.AddWithValue("@tahsilat", 0);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    hesapGuncelle(Convert.ToInt32(IDBul(cmbPNLVeresiyeMusteri.SelectedItem.ToString(), "(", ")")));
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnPNLVeresiyeIptal.PerformClick();
            }
        }

        private void btnPNLVeresiyeIptal_Click(object sender, EventArgs e)
        {
            ApnlVeresiye.Visible = false;
            tbMenu.Enabled = true;
            dtpPNLVeresiye.Value = DateTime.Now;
            cbPNLVeresiye.Checked = false;
            lblFaturaNo.Text = numaraUret(1);
        }

        private void txtTutarCariHareket_MouseClick(object sender, MouseEventArgs e)
        {
            txtTutarCariHareket.SelectAll();
        }

        private void tblFis_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int sonsatir = tblFis.RowCount;
            tblFis.Rows[sonsatir - 1].Selected = true;
        }
        private void cbPNLVeresiye_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPNLVeresiye.Checked == true)
            {
                dtpPNLVeresiye.Enabled = true;
            }
            else
            {
                dtpPNLVeresiye.Enabled = false;
            }
        }

        private void txtKasaHareketArama_TextChanged(object sender, EventArgs e)
        {
            kasaArama();
        }

        private void cbKasaTarih_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKasaTarih.Checked == true)
            {
                dtp1Kasa.Enabled = true;
                dtp2Kasa.Enabled = true;
            }
            else
            {
                dtp1Kasa.Enabled = false;
                dtp2Kasa.Enabled = false;
            }
            kasaArama();
        }

        private void dtp1Kasa_ValueChanged(object sender, EventArgs e)
        {
            kasaArama();
        }

        private void dtp2Kasa_ValueChanged(object sender, EventArgs e)
        {
            kasaArama();
        }

        private void cmbKasaIslemTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            kasaArama();
        }

        private void cmbKasaKasiyer_SelectedIndexChanged(object sender, EventArgs e)
        {
            kasaArama();
        }
        private void cmbKasaAramaNakitPOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            kasaArama();
        }

        private void tblHesaplar_SelectionChanged(object sender, EventArgs e)
        {
            if (tblHesaplar.CurrentRow.Cells[3].Value.ToString() == "" && btnHesapAktar.Text == "Hesap Aktar") btnHesapAktar.Enabled = false;
            else btnHesapAktar.Enabled = true;
        }

        void kasaRBChange()
        {
            if (btnKasaParaGiris.Checked == true)
            {
                kasaAktiflestir();
                kasaGirisTemizle();
                btnKasaNumaraUret.PerformClick();
                cmbKasaAciklama.Items.Clear();
                cmbKasaAciklama.Items.Add("POS");
                cmbKasaAciklama.Items.Add("Nakit");
            }
            else if (btnKasaParaCikis.Checked == true)
            {
                kasaAktiflestir();
                kasaGirisTemizle();
                btnKasaNumaraUret.PerformClick();
                cmbKasaAciklama.Items.Clear();
                cmbKasaAciklama.Items.Add("POS");
                cmbKasaAciklama.Items.Add("Nakit");
                cmbKasaAciklama.Items.Add("Personel Avans");
                cmbKasaAciklama.Items.Add("Fatura Ödemesi");
                cmbKasaAciklama.Items.Add("Personel Maaş Ödemesi");
                cmbKasaAciklama.Items.Add("Vergi Ödemesi");
                cmbKasaAciklama.Items.Add("Kira Ödemesi");
                cmbKasaAciklama.Items.Add("Yemek");
            }
        }
        private void btnKasaParaGiris_CheckedChanged(object sender, EventArgs e)
        {
            kasaRBChange();
        }
        string numaraUret(int kod)
        {
            DateTime sontarih = DateTime.Today;
            string tarih = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd");
            string maxkayit = "";
            string numara = "";
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select MAX(Tarih) from Fatura where FaturaNo like '" + tarih + kod.ToString() + "%';";
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    try
                    {
                        sontarih = dr.GetDateTime(0);
                    }
                    catch
                    {
                        maxkayit = "0";
                    }
                }
                dr.Close();
                if (maxkayit != "0")
                {
                    SqlCommand komut2 = new SqlCommand();
                    komut2.Connection = baglanti;
                    komut2.CommandText = "select FaturaNo from Fatura where Tarih=@tarih;";
                    komut2.Parameters.AddWithValue("@tarih", sontarih);
                    SqlDataReader dr2 = komut2.ExecuteReader();
                    if (dr2.Read())
                    {
                        maxkayit = dr2[0].ToString();
                        if (maxkayit == "") maxkayit = "0";
                        else
                        {
                            maxkayit = maxkayit.Remove(0, 7);
                        }
                    }
                }
                maxkayit = (Convert.ToInt32(maxkayit) + 1).ToString();
                baglanti.Close();
                numara = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + kod.ToString() + maxkayit;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return numara;
        }
        private void btnKasaNumaraUret_Click(object sender, EventArgs e)
        {
            txtKasaIslemNo.Text = numaraUret(2);
        }

        private void btnKasaIptal_Click(object sender, EventArgs e)
        {
            kasaPasiflestir();
            kasaGirisTemizle();
        }

        private void btnKasaParaCikis_CheckedChanged(object sender, EventArgs e)
        {
            kasaRBChange();
        }

        bool cariNumaraKontrol(string numara)
        {
            bool numaraVar = true;
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "Select IslemNo from Kasa where IslemNo=@islemno";
                komut.Parameters.AddWithValue("@islemno", numara);
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.HasRows)
                {
                    numaraVar = true;
                }
                else
                {
                    numaraVar = false;

                }
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return numaraVar;

        }
        bool kasaNumaraKontrol(string numara)
        {
            bool numaraVar = false;
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "Select CariNo from CariHareket where CariNo=@islemno";
                komut.Parameters.AddWithValue("@islemno", numara);
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.HasRows)
                {
                    numaraVar = true;
                }
                else
                {
                    numaraVar = false;

                }
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return numaraVar;
        }


        private void btnKasaKaydet_Click(object sender, EventArgs e)
        {
            bool numaraMevcutKasa = kasaNumaraKontrol(txtKasaIslemNo.Text);
            bool numaraMevcutCari = cariNumaraKontrol(txtKasaIslemNo.Text);
            if (numaraMevcutKasa == true)
            {
                MessageBox.Show("Aynı işlem numarası ile 2. kez kayıt yapılamaz, Lütfen farklı bir numara kullanınız.", "Mevcut İşlemNo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnKasaNumaraUret.PerformClick();
            }
            else if (numaraMevcutCari == true)
            {
                MessageBox.Show("Aynı işlem numarası ile 2. kez kayıt yapılamaz, Lütfen farklı bir numara kullanınız.", "Mevcut Cari NO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnKasaNumaraUret.PerformClick();
            }
            else
            {
                string tur = "";
                if (btnKasaParaGiris.Checked == true) tur = "Giriş";
                else tur = "Çıkış";
                try
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    baglanti.Open();
                    komut.CommandText = "insert into Kasa (IslemNo,Tur,Aciklama,NakitPOS,Miktar,Tarih,Kullanici) values (@islemno,@tur,@aciklama,@nakitpos,@miktar,@tarih,@kullanici)";
                    komut.Parameters.AddWithValue("@islemno", txtKasaIslemNo.Text);
                    komut.Parameters.AddWithValue("@tur", tur);
                    komut.Parameters.AddWithValue("@aciklama", cmbKasaAciklama.Text);
                    komut.Parameters.AddWithValue("@nakitpos", cmbParaTuruKasa.SelectedItem.ToString());
                    komut.Parameters.AddWithValue("@miktar", Convert.ToDouble(txtKasaMiktar.Text));
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.Parameters.AddWithValue("@kullanici", cmbKasiyer.SelectedItem.ToString());
                    komut.ExecuteNonQuery();
                    kasaAramaTemizle();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnKasaIptal.PerformClick();
            }

        }

        private void tbMenu_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            seciliTabIndex = tbMenu.SelectedIndex;
            if (tbMenu.SelectedTab == tbAyarlar) btnAyarIptalEt.PerformClick();
        }

        private void msiSatisDetayi_Click(object sender, EventArgs e)
        {
            panelAc(ApnlSatisDetayi);
            satisDetayCek();
        }

        private void btnPNLSatisKapat_Click(object sender, EventArgs e)
        {
            ApnlSatisDetayi.Visible = false;
            tbMenu.Enabled = true;
        }

        private void tblKasa_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            kasaRenk();
        }

        private void btnNakit_EnabledChanged(object sender, EventArgs e)
        {
            if (btnNakit.Enabled == true) lblFaturaNo.Text = numaraUret(1);
        }

        private void btnKasaSeciliSil_Click(object sender, EventArgs e)
        {
            secileniSilKasa();
        }

        private void msiKasaSecilenSil_Click(object sender, EventArgs e)
        {
            secileniSilKasa();
        }

        private void msKasa_Opened(object sender, EventArgs e)
        {
            if (tblKasa.RowCount <= 0) msKasa.Enabled = false;
            else
            {
                msKasa.Enabled = true;
                if (tblKasa.CurrentRow.DefaultCellStyle.BackColor == Color.LightGreen) msiSatisDetayi.Enabled = true;
                else msiSatisDetayi.Enabled = false;
            }
        }

        private void btnKasaTumunuGoster_Click(object sender, EventArgs e)
        {
            kasaAramaTemizle();
            cbKasaTarih.Checked = false;
        }

        private void btnKasaExcel_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblKasa, true, "KASA HAREKETLERİ");
        }

        private void msiKasaTumuExcel_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblKasa, true, "KASA HAREKETLERİ");
        }

        private void msiKasaExcel_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblKasa, false, "KASA HAREKETLERİ");
        }

        void topluGuncelleGirisAyari()
        {
            StokGiristemizle();
            stokAftiflestir();
            btnYeniUrunStok.Checked = false;
            btnUrunGuncelleStok.Checked = false;
            btnUrunSilStok.Checked = false;
            btnYeniUrunStok.Enabled = false;
            btnUrunGuncelleStok.Enabled = false;
            btnUrunSilStok.Enabled = false;
            btnKaydetStok.Enabled = true;
            btnIptalStok.Enabled = true;
            txtKDV.Text = "";
            rbKDV0.Checked = false;
            rbKDV1.Checked = false;
            rbKDV8.Checked = false;
            rbKDV18.Checked = false;
            txtOTV.Text = "";
            txtAlisFiyati.Text = "";
            txtSatisFiyati1.Text = "";
            txtSatisFiyati2.Text = "";
            txtStokKodu.Text = "";
            txtKritikSeviye.Text = "";
            cmbGrup.SelectedIndex = -1;
            cmbBirim.SelectedIndex = -1;
            btnGrupEkle.Enabled = false;
            btnBirimEkle.Enabled = false;
            btnResimSec.Enabled = false;
            txtBarkodNoStokBilgi.Enabled = false;
            pnlKDV.Enabled = false;
            pnlOTV.Enabled = false;
            tblStok.Enabled = false;
            txtUrunAdi.Enabled = false;
            MessageBox.Show("Yalnızca toplu güncelleştirme yapmak istediğiniz bilgileri giriniz ve kaydet butonuna tıklayınız.\nGirdiğiniz bilgiler tüm seçili satırlara işlenecektir! Komutu iptal edebilirsiniz.", "Toplu Güncelleştirme", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        private void msiStokSecileniGuncelle_Click(object sender, EventArgs e)
        {
            topluGuncelleGirisAyari();
        }

        private void txtSatisFiyati2_Leave(object sender, EventArgs e)
        {
            try
            {
                txtSatisFiyati2.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtSatisFiyati2.Text));
            }
            catch
            {
                txtSatisFiyati2.Text = "0,00";
            }
        }
        void alisGoster()
        {
            if (lblToplam.BackColor == Color.DarkRed)
            {
                lblAlisToplamı.Visible = true;
                lblToplam.BackColor = Color.Black;
                lblKur.BackColor = Color.Black;
                lblToplam.Font = new Font("Arial", 16, FontStyle.Bold);
                lblToplam.Text = alisFiyatiToplam();
            }
            else
            {
                lblAlisToplamı.Visible = false;
                lblToplam.BackColor = Color.DarkRed;
                lblKur.BackColor = Color.DarkRed;
                lblToplam.Font = new Font("Arial", 30, FontStyle.Bold);
                lblToplam.Text = ekran.toplamTutarHesap(tblFis, toplamTutar).ToString();
            }
            lblToplam.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(lblToplam.Text));
        }
        private void lblToplam_Click(object sender, EventArgs e)
        {
            alisGoster();
            txtBarkod.Focus();
        }
        void iskontoHesap()
        {
            try
            {
                double yenitutar = 0;
                if (rbPNLIskontoPara.Checked == true) yenitutar = Convert.ToDouble(txtPNLIskontoToplamTutar.Text) - Convert.ToDouble(txtPNLIskontoMiktar.Text);
                else yenitutar = Convert.ToDouble(txtPNLIskontoToplamTutar.Text) - (Convert.ToDouble(txtPNLIskontoToplamTutar.Text) * Convert.ToDouble(txtPNLIskontoMiktar.Text)) / 100;
                txtPNLIskontoYeniTutar.Text = yenitutar.ToString();
                txtPNLIskontoYeniTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLIskontoYeniTutar.Text));
            }
            catch
            {

            }
        }
        private void btnIskonto_Click(object sender, EventArgs e)
        {
            panelAc(ApnlIskonto);
            txtPNLIskontoToplamTutar.Text = ekran.toplamTutarHesap(tblFis, toplamTutar).ToString();
            txtPNLIskontoToplamTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLIskontoToplamTutar.Text));
            txtPNLIskontoToplamAlis.Text = alisFiyatiToplam();
            txtPNLIskontoToplamAlis.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLIskontoToplamAlis.Text));
            txtPNLIskontoMiktar.Text = "20";
            rbPNLIskontoYuzde.Checked = true;
            cmbPNLIskontoOdemeSekli.SelectedIndex = 0;
            iskontoHesap();
        }

        private void btnPNLIskontoIptal_Click(object sender, EventArgs e)
        {
            ApnlIskonto.Visible = false;
            tbMenu.Enabled = true;
        }

        private void rbPNLIskontoYuzde_CheckedChanged(object sender, EventArgs e)
        {
            iskontoHesap();
        }

        private void txtPNLIskontoMiktar_TextChanged(object sender, EventArgs e)
        {
            iskontoHesap();
        }

        private void btnPNLIskontoOnayla_Click(object sender, EventArgs e)
        {
            string parayuzde = "";
            if (rbPNLIskontoYuzde.Checked == true) parayuzde = "%";
            else parayuzde = "TL";
            hizliSatis("İskonto(" + txtPNLIskontoMiktar.Text + parayuzde + ")", cmbPNLIskontoOdemeSekli.SelectedItem.ToString());
            btnPNLIskontoIptal.PerformClick();
        }

        private void lblAlisToplamı_Click(object sender, EventArgs e)
        {
            alisGoster();
        }

        private void lblKur_Click(object sender, EventArgs e)
        {
            alisGoster();
        }
        private void msStok_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tblStok.RowCount > 0)
            {
                msStok.Enabled = true;
                if (tblStok.SelectedRows.Count > 1) msiStokSecileniGuncelle.Enabled = true;
                else msiStokSecileniGuncelle.Enabled = false;
            }
            else msStok.Enabled = false;
        }

        private void cmbIslemTuruStokHareket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIslemTuruStokHareket.SelectedIndex == 0)
            {
                btnStokCikisi.Enabled = true;
                btnStokGirisi.Enabled = false;
                txtFaturaNoStokHareket.Enabled = true;
                txtFaturaNoStokHareket.Text = numaraUret(3);
            }
            else if (cmbIslemTuruStokHareket.SelectedIndex == 1)
            {
                btnStokCikisi.Enabled = true;
                btnStokGirisi.Enabled = false;
                txtFaturaNoStokHareket.Enabled = true;
                txtFaturaNoStokHareket.Text = numaraUret(3);
            }
            else if (cmbIslemTuruStokHareket.SelectedIndex == 2)
            {
                btnStokCikisi.Enabled = false;
                btnStokGirisi.Enabled = true;
                txtFaturaNoStokHareket.Enabled = false;
                txtFaturaNoStokHareket.Clear();
            }
            else if (cmbIslemTuruStokHareket.SelectedIndex == 3)
            {
                btnStokCikisi.Enabled = false;
                btnStokGirisi.Enabled = true;
                txtFaturaNoStokHareket.Enabled = true;
                txtFaturaNoStokHareket.Clear();
            }
            tutarHesaplaStokHareket();
        }

        private void tblCariHareketler_Click(object sender, EventArgs e)
        {
            string aciklama = "";
            if (tblCariHareketler.RowCount > 0) tblCariHareketler.CurrentRow.Cells[2].Value.ToString();
            int indx = aciklama.IndexOf("(Ödendi)");
            if (tblCariHareketler.RowCount > 0)
            {
                string borc = tblCariHareketler.CurrentRow.Cells[4].Value.ToString();
                if (borc != "0" && indx == -1)
                {
                    txtTutarCariHareket.Text = borc;
                    txtAciklamaCariHareket.Text = tblCariHareketler.CurrentRow.Cells[0].Value.ToString() + "' numaralı borç kapatıldı.";
                    cbSonOdeme.Enabled = false;
                    txtTutarCariHareket.Enabled = false;
                    btnBorcEkleCari.Enabled = false;
                    btnTahsilatCari.Text = "Borcu Kapat";
                    btnTahsilatCari.BackColor = Color.LightGreen;
                }
                else
                {
                    txtTutarCariHareket.Text = "0,00";
                    txtAciklamaCariHareket.Text = "";
                    cbSonOdeme.Enabled = true;
                    txtTutarCariHareket.Enabled = true;
                    btnBorcEkleCari.Enabled = true;
                    btnTahsilatCari.Text = "Tahsilat Yap";
                    btnTahsilatCari.BackColor = Color.Transparent;
                }
            }
        }

        private void txtKasaMiktar_MouseClick(object sender, MouseEventArgs e)
        {
            txtKasaMiktar.SelectAll();
        }

        private void rbGunlukSatisRapor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGunlukSatisRapor.Checked == true)
            {
                dtpGunlukSatisRapor.Enabled = true;
                satisRaporArama();
            }
            else
            {
                dtpGunlukSatisRapor.Enabled = false;
            }
        }

        private void rbAylikSatisRapor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAylikSatisRapor.Checked == true)
            {
                cmbAylarSatisRapor.Enabled = true;
                numYilSatisRapor.Enabled = true;
                satisRaporArama();
            }
            else
            {
                cmbAylarSatisRapor.Enabled = false;
                numYilSatisRapor.Enabled = false;
            }
        }

        private void rbOzelTarihSatisRapor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOzelTarihSatisRapor.Checked == true)
            {
                dtp1OzelTarihSatisRapor.Enabled = true;
                dtp2OzelTarihSatisRapor.Enabled = true;
                satisRaporArama();
            }
            else
            {
                dtp1OzelTarihSatisRapor.Enabled = false;
                dtp2OzelTarihSatisRapor.Enabled = false;
            }
        }
        private void cmbSatisTuruSatisRapor_SelectedIndexChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void txtMetinSatisRapor_TextChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void cmbGrubuSatisRapor_SelectedIndexChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void dtpGunlukSatisRapor_ValueChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void cmbAylarSatisRapor_SelectedIndexChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void numYilSatisRapor_ValueChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void dtp1OzelTarihSatisRapor_ValueChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void dtp2OzelTarihSatisRapor_ValueChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        private void btnTumuSatisRapor_Click(object sender, EventArgs e)
        {
            satisRaporAramaTemizle(false);
        }

        private void rbKDVharic_CheckedChanged(object sender, EventArgs e)
        {
            if (rbKDVharic.Checked == true)
            {
                txtSatisFiyati1.Text = (Convert.ToDouble(txtSatisFiyati1.Text) * ((Convert.ToInt32(txtKDV.Text) / 100) + 1)).ToString();
                txtSatisFiyati1.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtSatisFiyati1.Text));
            }
        }

        private void btnYeniPNLVeresiye_CheckedChanged(object sender, EventArgs e)
        {
            if (btnYeniPNLVeresiye.Checked == true)
            {
                txtMusteriAdiPNLVeresiye.Visible = true;
            }
            else
            {
                txtMusteriAdiPNLVeresiye.Visible = false;
                txtMusteriAdiPNLVeresiye.Clear();
            }
        }

        private void txtMiktarStokHareket_TextChanged(object sender, EventArgs e)
        {
            tutarHesaplaStokHareket();
        }

        private void cbZararEttiklerim_CheckedChanged(object sender, EventArgs e)
        {
            satisRaporArama();
        }

        void barkodUret(int uzunluk)
        {
            Random rnd = new Random();
            string barkod = "869";
            int sayi = 0;
            for (int i = 0; i < uzunluk - 3; i++)
            {
                sayi = rnd.Next(0, 10);
                barkod += sayi.ToString();
            }
            txtBarkodNoStokBilgi.Text = barkod;
        }



        private void rbKDV0_CheckedChanged(object sender, EventArgs e)
        {
            if (rbKDV0.Checked == true) txtKDV.Text = "0";
        }

        private void rbKDV1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbKDV1.Checked == true) txtKDV.Text = "1";
        }

        private void rbKDV8_CheckedChanged(object sender, EventArgs e)
        {
            if (rbKDV8.Checked == true) txtKDV.Text = "8";
        }

        private void rbKDV18_CheckedChanged(object sender, EventArgs e)
        {
            if (rbKDV18.Checked == true) txtKDV.Text = "18";
        }

        private void txtAlinanPara_Leave(object sender, EventArgs e)
        {
            txtAlinanPara.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtAlinanPara.Text));
        }

        private void txtAlinanPara_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtParaUstu.Text = (Convert.ToDouble(lblToplam.Text) - Convert.ToDouble(txtAlinanPara.Text)).ToString();
            }
            catch
            {

            }
        }

        private void txtAlinanPara_Click(object sender, EventArgs e)
        {
            txtAlinanPara.SelectAll();
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(1);
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            paraHesap(0.5);
        }

        private void btnSatisRaporYazdir_Click(object sender, EventArgs e)
        {
            UCRaporlarDetaysiz ucrapor = new UCRaporlarDetaysiz();
            this.Controls.Add(ucrapor);
            ucrapor.Show();

            ucrapor.Location = new Point(
                this.ClientSize.Width / 2 - ucrapor.Size.Width / 2, this.ClientSize.Height / 2 - ucrapor.Size.Height / 2
                );
            ucrapor.BringToFront();
            ucrapor.hesapla(tblSatisRapor);
            tbMenu.Enabled = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "0";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "1";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "2";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "3";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "4";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "5";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "6";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "7";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "8";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtSifre.Text += "9";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (txtSifre.TextLength > 0)
            {
                txtSifre.Text = txtSifre.Text.Remove(txtSifre.TextLength - 1);
            }
        }

        private void btnYazdirSatisDetayi_Click(object sender, EventArgs e)
        {
            fistekrariyazdir fis = new fistekrariyazdir();
            fis.verilerial(txtPNLSatisFaturaNo.Text, Convert.ToDouble(txtPNLSatisDetayiTutar.Text), lblPNLSatisTarih.Text, tblSatisDetayi, lblSirketAdi.Text, txtSirketAdresi1.Text, txtSirketAdresi2.Text, txtSirketAdresi3.Text, txtSirketTelefonu.Text);
        }

        private void btnSatisRaporExcel_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblSatisRapor, true, "SATIŞ RAPORLARI");
        }

        public static void tbmenuac()
        {
            if (tbm.Enabled == false)
            {
                tbm.Enabled = true;
            }
            else tbm.Enabled = false;
        }

        void ayarGuncelle()
        {
            try
            {
                HizliSatis.Properties.Settings.Default.sirketAdi = txtSirketAdi.Text;
                HizliSatis.Properties.Settings.Default.adres1 = txtSirketAdresi1.Text;
                HizliSatis.Properties.Settings.Default.adres2 = txtSirketAdresi2.Text;
                HizliSatis.Properties.Settings.Default.adres3 = txtSirketAdresi3.Text;
                HizliSatis.Properties.Settings.Default.telefon = txtSirketTelefonu.Text;
                HizliSatis.Properties.Settings.Default.vergiDairesi = txtSirketVergiDairesi.Text;
                HizliSatis.Properties.Settings.Default.vergiNo = txtSirketVergiNo.Text;

                HizliSatis.Properties.Settings.Default.grup1Text = txtAyarGrup1.Text;
                HizliSatis.Properties.Settings.Default.grup2Text = txtAyarGrup2.Text;
                HizliSatis.Properties.Settings.Default.grup3Text = txtAyarGrup3.Text;
                HizliSatis.Properties.Settings.Default.grup4Text = txtAyarGrup4.Text;
                HizliSatis.Properties.Settings.Default.grup5Text = txtAyarGrup5.Text;
                HizliSatis.Properties.Settings.Default.grup6Text = txtAyarGrup6.Text;

                HizliSatis.Properties.Settings.Default.yonetimSifresi = txtYonetimSifresi.Text;
                HizliSatis.Properties.Settings.Default.guvenlikSorusu = txtGuvenlikSorusu.Text;
                HizliSatis.Properties.Settings.Default.guvenlikCevabi = txtGuvenlikCevabi.Text;

                try
                {
                    HizliSatis.Properties.Settings.Default.teraziCOM = cmbCOMAyarlar.SelectedItem.ToString();
                }
                catch
                {
                    HizliSatis.Properties.Settings.Default.teraziCOM = "";
                }

                try
                {
                    HizliSatis.Properties.Settings.Default.teraziAktarimMod = cmbCOMAyarlarAktarim.SelectedItem.ToString();
                }
                catch
                {
                    HizliSatis.Properties.Settings.Default.teraziAktarimMod = "";
                }

                HizliSatis.Properties.Settings.Default.teraziStokKodu = txtTartilabilirAyarlar.Text;
                HizliSatis.Properties.Settings.Default.teraziMinDeger = txtEnAzAgirlikAyarlar.Text;

                try
                {
                    HizliSatis.Properties.Settings.Default.ekranCOM = cmbCOMMusteriEkran.SelectedItem.ToString();
                    HizliSatis.Properties.Settings.Default.ekranBaud = cmbBaudMusteriEkran.SelectedItem.ToString();
                }
                catch
                {
                    HizliSatis.Properties.Settings.Default.ekranCOM = "";
                    HizliSatis.Properties.Settings.Default.ekranBaud = "";
                }

                try
                {
                    HizliSatis.Properties.Settings.Default.yaziciBoyut = cmbYaziciKagitBoyutu.SelectedItem.ToString();
                    HizliSatis.Properties.Settings.Default.yaziciAdi = cmbYazicilar.SelectedItem.ToString();
                }
                catch
                {
                    HizliSatis.Properties.Settings.Default.yaziciBoyut = "";
                    HizliSatis.Properties.Settings.Default.yaziciAdi = "";
                }


                if (rbKritikMesajA.Checked == true) HizliSatis.Properties.Settings.Default.kritikMesajUyari = true;
                else HizliSatis.Properties.Settings.Default.kritikMesajUyari = false;
                if (rbStokYetersizA.Checked == true) HizliSatis.Properties.Settings.Default.stokYetersizUyari = true;
                else HizliSatis.Properties.Settings.Default.stokYetersizUyari = false;
                if (rbKritikRenkIkazA.Checked == true) HizliSatis.Properties.Settings.Default.kritikRenkUyari = true;
                else HizliSatis.Properties.Settings.Default.kritikRenkUyari = false;
                if (rbUrunBulunamadiA.Checked == true) HizliSatis.Properties.Settings.Default.urunBulunamadiUyari = true;
                else HizliSatis.Properties.Settings.Default.urunBulunamadiUyari = false;

                if (cbAyarStokKilit.Checked == true) HizliSatis.Properties.Settings.Default.kilitStok = true;
                else HizliSatis.Properties.Settings.Default.kilitStok = false;
                if (cbAyarMusteriKilit.Checked == true) HizliSatis.Properties.Settings.Default.kilitMusteriler = true;
                else HizliSatis.Properties.Settings.Default.kilitMusteriler = false;
                if (cbAyarCariKilit.Checked == true) HizliSatis.Properties.Settings.Default.kilitCari = true;
                else HizliSatis.Properties.Settings.Default.kilitCari = false;
                if (cbAyarKasaKilit.Checked == true) HizliSatis.Properties.Settings.Default.kilitKasa = true;
                else HizliSatis.Properties.Settings.Default.kilitKasa = false;
                if (cbAyarRaporKilit.Checked == true) HizliSatis.Properties.Settings.Default.kilitRapor = true;
                else HizliSatis.Properties.Settings.Default.kilitRapor = false;
                if (cbAyarAyarlarKilit.Checked == true) HizliSatis.Properties.Settings.Default.kilitAyarlar = true;
                else HizliSatis.Properties.Settings.Default.kilitAyarlar = false;

                if (cbManuelResimCek.Checked == true) HizliSatis.Properties.Settings.Default.urunKisayolManuel = true;
                else HizliSatis.Properties.Settings.Default.urunKisayolManuel = false;

                HizliSatis.Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası:" + ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ayarCek()
        {
            try
            {
                txtSirketAdi.Text = HizliSatis.Properties.Settings.Default.sirketAdi;
                txtSirketAdresi1.Text = HizliSatis.Properties.Settings.Default.adres1;
                txtSirketAdresi2.Text = HizliSatis.Properties.Settings.Default.adres2;
                txtSirketAdresi3.Text = HizliSatis.Properties.Settings.Default.adres3;
                txtSirketTelefonu.Text = HizliSatis.Properties.Settings.Default.telefon;
                txtSirketVergiDairesi.Text = HizliSatis.Properties.Settings.Default.vergiDairesi;
                txtSirketVergiNo.Text = HizliSatis.Properties.Settings.Default.vergiNo;


                txtAyarGrup1.Text = HizliSatis.Properties.Settings.Default.grup1Text;
                txtAyarGrup2.Text = HizliSatis.Properties.Settings.Default.grup2Text;
                txtAyarGrup3.Text = HizliSatis.Properties.Settings.Default.grup3Text;
                txtAyarGrup4.Text = HizliSatis.Properties.Settings.Default.grup4Text;
                txtAyarGrup5.Text = HizliSatis.Properties.Settings.Default.grup5Text;
                txtAyarGrup6.Text = HizliSatis.Properties.Settings.Default.grup6Text;

                txtYonetimSifresi.Text = HizliSatis.Properties.Settings.Default.yonetimSifresi;
                txtGuvenlikSorusu.Text = HizliSatis.Properties.Settings.Default.guvenlikSorusu;
                txtGuvenlikCevabi.Text = HizliSatis.Properties.Settings.Default.guvenlikCevabi;

                cmbCOMAyarlar.SelectedItem = HizliSatis.Properties.Settings.Default.teraziCOM;
                cmbCOMAyarlarAktarim.SelectedItem = HizliSatis.Properties.Settings.Default.teraziAktarimMod;
                txtTartilabilirAyarlar.Text = HizliSatis.Properties.Settings.Default.teraziStokKodu;
                txtEnAzAgirlikAyarlar.Text = HizliSatis.Properties.Settings.Default.teraziMinDeger;

                cmbCOMMusteriEkran.SelectedItem = HizliSatis.Properties.Settings.Default.ekranCOM;
                cmbBaudMusteriEkran.SelectedItem = HizliSatis.Properties.Settings.Default.ekranBaud;

                cmbYaziciKagitBoyutu.SelectedItem = HizliSatis.Properties.Settings.Default.yaziciBoyut;
                cmbYazicilar.SelectedItem = HizliSatis.Properties.Settings.Default.yaziciAdi;

                if (HizliSatis.Properties.Settings.Default.kritikMesajUyari == true) rbKritikMesajA.Checked = true;
                else rbKritikMesajP.Checked = true;
                if (HizliSatis.Properties.Settings.Default.stokYetersizUyari == true) rbStokYetersizA.Checked = true;
                else rbStokYetersizP.Checked = true;
                if (HizliSatis.Properties.Settings.Default.kritikRenkUyari == true) rbKritikRenkIkazA.Checked = true;
                else rbKritikRenkIkazP.Checked = true;
                if (HizliSatis.Properties.Settings.Default.urunBulunamadiUyari == true) rbUrunBulunamadiA.Checked = true;
                else rbUrunBulunamadiP.Checked = true;

                if (HizliSatis.Properties.Settings.Default.kilitStok == true) cbAyarStokKilit.Checked = true;
                else cbAyarStokKilit.Checked = false;
                if (HizliSatis.Properties.Settings.Default.kilitMusteriler == true) cbAyarMusteriKilit.Checked = true;
                else cbAyarMusteriKilit.Checked = false;
                if (HizliSatis.Properties.Settings.Default.kilitCari == true) cbAyarCariKilit.Checked = true;
                else cbAyarCariKilit.Checked = false;
                if (HizliSatis.Properties.Settings.Default.kilitKasa == true) cbAyarKasaKilit.Checked = true;
                else cbAyarKasaKilit.Checked = false;
                if (HizliSatis.Properties.Settings.Default.kilitRapor == true) cbAyarRaporKilit.Checked = true;
                else cbAyarRaporKilit.Checked = false;
                if (HizliSatis.Properties.Settings.Default.kilitAyarlar == true) cbAyarAyarlarKilit.Checked = true;
                else cbAyarAyarlarKilit.Checked = false;

                if (HizliSatis.Properties.Settings.Default.urunKisayolManuel == true) cbManuelResimCek.Checked = true;
                else cbManuelResimCek.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ayar alma hatası:" + ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void ayarUygula()
        {
            btnGrp1.Text = txtAyarGrup1.Text;
            btnGrp2.Text = txtAyarGrup2.Text;
            btnGrp3.Text = txtAyarGrup3.Text;
            btnGrp4.Text = txtAyarGrup4.Text;
            btnGrp5.Text = txtAyarGrup5.Text;
            btnGrp6.Text = txtAyarGrup6.Text;
            lblSirketAdi.Text = txtSirketAdi.Text;
            if(cmbYaziciKagitBoyutu.SelectedIndex!=-1) yaziciMM = Convert.ToInt32(cmbYaziciKagitBoyutu.SelectedItem.ToString());
        }

        bool sifreKontrol()
        {
            bool dogru = false;
            if (txtSifre.Text == txtYonetimSifresi.Text)
            {
                dogru = true;
            }
            return dogru;
        }

        private void button13_Click(object sender, EventArgs e)//Sifre OK
        {

            if (sifreAcilis == "tbKasa")
            {
                if (sifreKontrol()) tbMenu.SelectedTab = tbKasa;
            }
            else if (sifreAcilis == "tbRapor")
            {
                if (sifreKontrol()) tbMenu.SelectedTab = tbRapor;
            }
            else if (sifreAcilis == "tbStok")
            {
                if (sifreKontrol()) tbMenu.SelectedTab = tbStok;
            }
            else if (sifreAcilis == "tbMusteri")
            {
                if (sifreKontrol()) tbMenu.SelectedTab = tbMusteriler;
            }
            else if (sifreAcilis == "tbCari")
            {
                if (sifreKontrol()) tbMenu.SelectedTab = tbCari;
            }
            else if (sifreAcilis == "tbAyarlar")
            {
                if (sifreKontrol()) tbMenu.SelectedTab = tbAyarlar;
            }

            APnlSifre.Visible = false;
            tbMenu.Enabled = true;
            sifreAcilis = "";
        }

        private void tbMenu_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tbMenu.SelectedTab == tbKasa && sifreAcilis == "" && cbAyarKasaKilit.Checked == true)
            {
                sifreAcilis = "tbKasa";
                tbMenu.SelectedIndex = seciliTabIndex;
                txtSifre.Clear();
                panelAc(APnlSifre);
            }
            else if (tbMenu.SelectedTab == tbRapor && sifreAcilis == "" && cbAyarRaporKilit.Checked == true)
            {
                sifreAcilis = "tbRapor";
                tbMenu.SelectedIndex = seciliTabIndex;
                txtSifre.Clear();
                panelAc(APnlSifre);
            }
            else if (tbMenu.SelectedTab == tbStok && sifreAcilis == "" && cbAyarStokKilit.Checked == true)
            {
                sifreAcilis = "tbStok";
                tbMenu.SelectedIndex = seciliTabIndex;
                txtSifre.Clear();
                panelAc(APnlSifre);
            }
            else if (tbMenu.SelectedTab == tbMusteriler && sifreAcilis == "" && cbAyarMusteriKilit.Checked == true)
            {
                sifreAcilis = "tbMusteri";
                tbMenu.SelectedIndex = seciliTabIndex;
                txtSifre.Clear();
                panelAc(APnlSifre);
            }
            else if (tbMenu.SelectedTab == tbCari && sifreAcilis == "" && cbAyarCariKilit.Checked == true)
            {
                sifreAcilis = "tbCari";
                tbMenu.SelectedIndex = seciliTabIndex;
                txtSifre.Clear();
                panelAc(APnlSifre);
            }
            else if (tbMenu.SelectedTab == tbKasa && sifreAcilis == "" && cbAyarKasaKilit.Checked == true)
            {
                sifreAcilis = "tbKasa";
                tbMenu.SelectedIndex = seciliTabIndex;
                txtSifre.Clear();
                panelAc(APnlSifre);
            }
            else if (tbMenu.SelectedTab == tbAyarlar && sifreAcilis == "" && cbAyarAyarlarKilit.Checked == true)
            {
                sifreAcilis = "tbAyarlar";
                tbMenu.SelectedIndex = seciliTabIndex;
                txtSifre.Clear();
                panelAc(APnlSifre);
            }
        }

        private void btnAyarIptalEt_Click(object sender, EventArgs e)
        {
            ayarCek();
        }

        private void btnAyarKaydet_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Ayar değişikliklerini kaydetmek istediğinize emin misiniz?", "Ayar GÜncelleme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                ayarGuncelle();
                ayarUygula();
            }
        }

        private void msItemUrunKisayolKaldir_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Ürün kısayolunu kaldırmak istediğinize emin misiniz?\nDaha sonra bu menüden tekrar ekleyebilirsiniz.", "Ürün Kısayolunu Kaldır", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (sonuc == DialogResult.Yes)
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    SqlCommand komut = new SqlCommand();
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "update Stok set KisayolNo=0 where Barkod='" + barkodlar[secilenKisayol % 20] + "'";
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            resimCek(grupID);
        }

        private void msItemUrunSec_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "select top 100 Adi,Barkod from Stok where ISNULL(KisayolNo,0)=0 order by StokID;";
                SqlDataReader dr = komut.ExecuteReader();
                msCmbUrunSec.Items.Clear();
                msCmbUrunSec.Text = "Ürün seçiniz...";
                while (dr.Read())
                {
                    msCmbUrunSec.Items.Add(dr.GetString(0) + " (" + dr.GetString(1) + ")");
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void msCmbUrunSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(baglantiadresi);
                SqlCommand komut = new SqlCommand();
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "update Stok set KisayolNo=" + (secilenKisayol + 1) + " where Barkod='" + IDBul(msCmbUrunSec.SelectedItem.ToString(), "(", ")") + "';";
                komut.ExecuteNonQuery();
                baglanti.Close();
                msUrunKisayol.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            resimCek(grupID);
        }

        private void msTxtUrunBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            if (e.KeyChar == 13)
            {
                int durum = 0;
                try
                {
                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    SqlCommand komut = new SqlCommand();
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "select ISNULL(KisayolNo,0) from stok where Barkod='" + msTxtUrunBarkod.Text + "';";
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetInt32(0) != 0)
                        {
                            durum = 1;
                            MessageBox.Show("Bu ürün zaten " + (dr.GetInt32(0) / 20 + 1).ToString() + ". Sayfa " + (dr.GetInt32(0) % 20).ToString() + ". Sırada tanımlı.\nÖnce ürün kısayolunu kaldırın.", "Ürün Zaten Tanımlı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        durum = 2;
                        MessageBox.Show("Bu barkod numarasına ait ürün bulunamadı!", "Barkod Numarasını Kontrol Edin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    dr.Close();
                    if (durum == 0)
                    {
                        komut.CommandText = "update Stok set KisayolNo=" + (secilenKisayol + 1) + " where Barkod='" + msTxtUrunBarkod.Text + "'";
                        komut.ExecuteNonQuery();
                        msUrunKisayol.Close();
                    }
                    baglanti.Close();
                    msTxtUrunBarkod.Clear();
                    resimCek(grupID);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void msUrunKisayol_Opening(object sender, CancelEventArgs e)
        {
            if (cbManuelResimCek.Checked == false) msUrunKisayol.Enabled = false;
            else msUrunKisayol.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (APnlSifre.Height == 366)
            {
                lblGuvenlikSorusu.Text = txtGuvenlikSorusu.Text;
                APnlSifre.Height = 495;
            }
            else APnlSifre.Height = 366;
        }

        private void APnlSifre_VisibleChanged(object sender, EventArgs e)
        {
            APnlSifre.Height = 366;
            txtSifre.Clear();
            txtSifre.Focus();
            txtSifreGuvenlikCevabi.Clear();
        }

        private void txtSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            if (e.KeyChar == 13) btnSifreOK.PerformClick();
        }

        private void btnGuvenlikOnayla_Click(object sender, EventArgs e)
        {
            if (txtSifreGuvenlikCevabi.Text == txtGuvenlikCevabi.Text)
            {
                MessageBox.Show("Yönetim şifresi sıfırlanmıştır. Şifre bölümünü boş bırakarak giriş yapabilirsiniz.", "Yönetim Şifresi Sıfırlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HizliSatis.Properties.Settings.Default.yonetimSifresi = "";
                txtYonetimSifresi.Clear();
                txtSifre.Clear();
                btnSifreOK.PerformClick();
            }
            else MessageBox.Show("Cevabınız doğru değil!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (txtYonetimSifresi.PasswordChar == '*')
            {
                txtYonetimSifresi.PasswordChar = '\0';
                txtGuvenlikCevabi.PasswordChar = '\0';
            }
            else
            {
                txtYonetimSifresi.PasswordChar = '*';
                txtGuvenlikCevabi.PasswordChar = '*';
            }
        }

        public void gunsonutabloalhesapla()
        {
            try
            {
                SqlConnection conn = new SqlConnection(Genesis.baglantiadresi);
                SqlCommand komut = new SqlCommand();
                komut.Connection = conn;
                komut.CommandText = "SELECT UrunAdi[URUN],SUM(Miktar)[SATIŞ],SUM(Tutar)[TOPLAM] FROM Fatura where Tarih BETWEEN @date1 AND @date2 group by UrunAdi;";
                komut.Parameters.AddWithValue("@date1", dtp1Kasa.Value.Date);
                komut.Parameters.AddWithValue("@date2", dtp2Kasa.Value.Date);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblgunsonu.DataSource = dt;
                conn.Close();
            }
            catch (Exception E)
            {
                MessageBox.Show("Yazdırma sırasında bir hata oluştu. Ayarlarınızı kontrol edin.\n" + E, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void BtnKasaYazdir_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show(dtp1Kasa.Value.Date.ToLongDateString() + " tarihine ait tüm satış işlemleri için raporlama yapılacaktır.\n İşleme devam etmek istiyor musunuz?", "Günsonu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                gunsonutabloalhesapla();
                gunsonuESCPOS gunsonu = new gunsonuESCPOS();
                gunsonu.verilerial(txtSirketAdi.Text, txtSirketAdresi1.Text, txtSirketAdresi2.Text, txtSirketAdresi3.Text, txtSirketTelefonu.Text, tblgunsonu, dtp1Kasa.Value.Date);
            }
            else if (sonuc == DialogResult.No)
            {
                MessageBox.Show("İşlem iptal edildi.", "Günsonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnTeraziyeBaglan_Click(object sender, EventArgs e)
        {
            teraziBaglan();
            baglantiKontrol();
        }

        private void btnTeraziBaglantiKes_Click(object sender, EventArgs e)
        {
            sp.Close();
            baglantiKontrol();
        }

        private void lblSirketAdi_Click(object sender, EventArgs e)
        {
            //yazdir();
        }

        private void tblKasa_DoubleClick(object sender, EventArgs e)
        {
            panelAc(ApnlSatisDetayi);
            satisDetayCek();
        }

        private void btnAdminKodCalistir_Click(object sender, EventArgs e)
        {
            if (txtAdminSifre.Text.Equals("genesis.66"))
            {
                try
                {
                    SqlConnection conn = new SqlConnection(Genesis.baglantiadresi);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = conn;
                    komut.CommandText = txtAdminKod.Text;
                    conn.Open();
                    komut.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Kod başarıyla çalıştı!", "Kod Çalıştırma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception E)
                {
                    MessageBox.Show("Kod çalıştırılamadı!.\n" + E, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen admin şifresinin doğru olduğundan emin olun!", "Admin Şifre", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblToplam_TextChanged(object sender, EventArgs e)
        {
            ekranTutarYaz();
        }

        public void ekranTutarYaz()
        {
            if (lblAlisToplamı.Visible == false)
            {
                string tutar = lblToplam.Text.Replace(",", ".");
                string bosluk = "";
                for (int i = 0; i < 8 - tutar.Replace(".", "").Length; i++)
                {
                    bosluk += " ";
                }
                tutar = bosluk + tutar;
                musteriEkranYaz(tutar);
            }
        }

        private void btnMusteriEkranBaglan_Click(object sender, EventArgs e)
        {
            musteriEkranBaglan();
        }

        private void Genesis_FormClosed(object sender, FormClosedEventArgs e)
        {
            lblToplam.Text = "0,00";
        }

        private void Genesis_Click(object sender, EventArgs e)
        {
            txtBarkod.Focus();
        }

        private void btnExcelSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ac = new OpenFileDialog();
            ac.Filter = " Xlsx Dosyaları |*.xlsx| Bütün Dosyalar|*.*";
            ac.ShowDialog();
            txtExcel.Text = ac.FileName.ToString();
        }

        private void btnExcelBaslat_Click(object sender, EventArgs e)
        {
            excelStokGiris exc = new excelStokGiris();
            if (txtExcel.Text.Trim().Length > 0)
            {
                List<string> excelliste = exc.dosyaAl(@txtExcel.Text);
                try
                {
                    SqlConnection baglanti = new SqlConnection(Genesis.baglantiadresi);
                    baglanti.Open();
                    int sayi = 0;

                    string adi = "";
                    string grubu = "";
                    string barkod = "";
                    string stokkodu = "";
                    string miktar = "";
                    string birim = "";
                    string kritikseviye = "";
                    string alis = "";
                    string satis1 = "";
                    string satis2 = "";
                    string kdv = "";
                    string otv = "";
                    foreach (var item in excelliste)
                    {
                        if (adi.Equals("")) adi = item;
                        else if (grubu.Equals("")) grubu = item;
                        else if (barkod.Equals("")) barkod = item;
                        else if (stokkodu.Equals("")) stokkodu = item;
                        else if (miktar.Equals("")) miktar = item;
                        else if (birim.Equals("")) birim = item;
                        else if (kritikseviye.Equals("")) kritikseviye = item;
                        else if (alis.Equals("")) alis = item;
                        else if (satis1.Equals("")) satis1 = item;
                        else if (satis2.Equals("")) satis2 = item;
                        else if (kdv.Equals("")) kdv = item;
                        else if (otv.Equals(""))
                        {
                            otv = item;
                            SqlCommand komut = new SqlCommand();
                            komut.Connection = baglanti;
                            komut.CommandText = "insert into Stok(Adi, Grubu, Barkod,StokKodu,Miktar,Birim,KritikSeviye,AlisFiyati,SatisFiyati1,SatisFiyati2,KDV,OTV) values (@adi,@grubu,@barkod,@stokkodu,@miktar,@birim,@kritikseviye,@alis,@satis1,@satis2,@kdv,@otv)";
                            komut.Parameters.AddWithValue("@adi", adi);
                            komut.Parameters.AddWithValue("@grubu", grubu);
                            komut.Parameters.AddWithValue("@barkod", barkod);
                            komut.Parameters.AddWithValue("@stokkodu", stokkodu);
                            komut.Parameters.AddWithValue("@miktar", Convert.ToDouble(miktar));
                            komut.Parameters.AddWithValue("@birim", birim);
                            komut.Parameters.AddWithValue("@kritikseviye", Convert.ToInt32(kritikseviye));
                            komut.Parameters.AddWithValue("@alis", Convert.ToDouble(alis));
                            komut.Parameters.AddWithValue("@satis1", Convert.ToDouble(satis1));
                            komut.Parameters.AddWithValue("@satis2", Convert.ToDouble(satis2));
                            komut.Parameters.AddWithValue("@kdv", Convert.ToInt32(kdv));
                            komut.Parameters.AddWithValue("@otv", Convert.ToInt32(otv));
                            komut.ExecuteNonQuery();

                            adi = ""; grubu= "";barkod = "";stokkodu = "";miktar = "";birim = "";kritikseviye = "";
                            alis = "";satis1 = "";satis2 = "";kdv = "";otv = "";
                            
                            sayi++;
                        }
                    }
                    MessageBox.Show((sayi).ToString() + " Adet stok kaydı işlemi başarıyla gerçekleşti!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    baglanti.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Excel seçmediniz.");
            }
        }

        private void tblStok_SelectionChanged(object sender, EventArgs e)
        {
            stokTabloSecim();
        }

        private void txtAlisFiyati_MouseClick(object sender, MouseEventArgs e)
        {
            txtAlisFiyati.SelectAll();
        }

        private void txtSatisFiyati1_MouseClick(object sender, MouseEventArgs e)
        {
            txtSatisFiyati1.SelectAll();
        }

        private void txtSatisFiyati2_MouseClick(object sender, MouseEventArgs e)
        {
            txtSatisFiyati2.SelectAll();
        }

        private void txtMiktarStokHareket_MouseClick(object sender, MouseEventArgs e)
        {
            txtMiktarStokHareket.SelectAll();
        }

        private void txtStokArama_MouseClick(object sender, MouseEventArgs e)
        {
            txtStokArama.SelectAll();
        }

        private void txtStokArama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                txtAlisFiyati.Focus();
                txtAlisFiyati.SelectAll();
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                stokArama();
            }
        }

        private void seçiliÜrünüSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = tblFis.CurrentRow.Index;
            tblFis.Rows.RemoveAt(index);
            stokmiktar.RemoveAt(index);
            kritikseviye.RemoveAt(index);
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tblFis.EditMode = DataGridViewEditMode.EditOnEnter;
            tblFis.CurrentCell = tblFis.Rows[tblFis.RowCount - 1].Cells[0];
        }

        private void tblFis_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double miktar, fiyat, tutar;
            for (int i = 0; i < tblFis.RowCount; i++)
            {
                miktar = Convert.ToDouble(tblFis.Rows[i].Cells[3].Value);
                fiyat = Convert.ToDouble(tblFis.Rows[i].Cells[6].Value);
                tutar = miktar * fiyat;
                tblFis.Rows[i].Cells[7].Value = tutar.ToString();
            }
            tutarHesapBitir(false);
        }

        private void tblFis_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            bool sonuc = true;
            if (e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 6) sonuc = false;
            e.Cancel = sonuc;
        }

        private void rbChartKar_CheckedChanged(object sender, EventArgs e)
        {
            grafikDoldurPasta();
        }

        private void rbChartGrup_CheckedChanged(object sender, EventArgs e)
        {
            grafikDoldurPasta();
        }

        private void swFis_OnValueChange(object sender, EventArgs e)
        {
            txtBarkod.Focus();
        }

        private void tblFis_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tblFis_Click(object sender, EventArgs e)
        {
            txtBarkod.Focus();
        }

        private void btnEksi_Click(object sender, EventArgs e)
        {
            txtBarkod.Focus();
        }

        private void btnArti_Click(object sender, EventArgs e)
        {
            txtBarkod.Focus();
        }

        private void btnCariDetayKapat_Click(object sender, EventArgs e)
        {
            ApnlCariDetayı.Visible = false;
            tbMenu.Enabled = true;
        }

        private void tblCariHareketler_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string faturano = IDBul(tblCariHareketler.CurrentRow.Cells[1].Value.ToString(), "(", ")");
                if (faturano != null)
                {
                    string aciklama = tblCariHareketler.CurrentRow.Cells[2].Value.ToString();
                    string sonodeme = tblCariHareketler.CurrentRow.Cells[3].Value.ToString();
                    string tutar = tblCariHareketler.CurrentRow.Cells[4].Value.ToString();
                    string islemtarihi = tblCariHareketler.CurrentRow.Cells[7].Value.ToString();

                    txtCariDetayFaturaNo.Text = faturano;
                    txtCariDetayTutar.Text = tutar;
                    lblCariDetayAciklama.Text = aciklama;
                    lblCariDetayTarih.Text = islemtarihi.Substring(0,16);
                    if(sonodeme.Length>0) lblCariDetaySonOdemeTarihi.Text = sonodeme.Substring(0, 10);
                    panelAc(ApnlCariDetayı);

                    SqlConnection baglanti = new SqlConnection(baglantiadresi);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "select BarkodNo[Barkod No],UrunAdi[Ürün Adı],Miktar,Birim,KDV,Fiyat,Tutar from Fatura where FaturaNo=@faturano";
                    komut.Parameters.AddWithValue("@faturano", faturano);
                    baglanti.Open();
                    SqlDataAdapter da = new SqlDataAdapter(komut);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    tblCariDetay.DataSource = dt;
                    baglanti.Close();
                    tblCariDetay.AutoResizeColumns();
                    tblCariDetay.AutoResizeColumnHeadersHeight();
                    tblCariDetay.Columns[1].Width = 277;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void cariHareketRenk()
        {
            for (int i = 0; i < tblCariHareketler.Rows.Count; i++)
            {
                if (IDBul(tblCariHareketler.Rows[i].Cells[1].Value.ToString(), "(", ")") != null)
                {
                    tblCariHareketler.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }

        public void DBYedekle()
        {
            FolderBrowserDialog Klasor = new FolderBrowserDialog();
            if (Klasor.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string KlasorYolu;
                    string tarih = DateTime.Today.ToShortDateString();
                    KlasorYolu = Klasor.SelectedPath + @"\GenesisHizlisatis-" + tarih + ".bak";


                    if (File.Exists(KlasorYolu)) File.Delete(KlasorYolu);
                    Backup bkpDBFull = new Backup();

                    bkpDBFull.Action = BackupActionType.Database;

                    bkpDBFull.Database = "Hizlisatis";//@"D:\Hizlisatis.mdf"

                    bkpDBFull.Devices.AddDevice(KlasorYolu, DeviceType.File);
                    bkpDBFull.BackupSetName = "Hizlisatis";
                    bkpDBFull.BackupSetDescription = "Hızlı Satış Yedek";

                    bkpDBFull.Initialize = false;

                    Server server = new Server(@"(LocalDB)\v11.0");
                    bkpDBFull.SqlBackup(server);
                    MessageBox.Show("Veritabanı yedekleme işlemi başarıyla tamamlandı.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Veritabanı yedekleme hatası! Hata Kodları:\n" + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void DBGeriyukle()
        {
            if (MessageBox.Show("Veritabanı geriyükleme işlemini çok dikkatli yapmalısınız! Yedekten geriyükleme yapıldıktan sonra bu işlem geri alınamaz!\nYedek dosyası seçme ekranına yönlendirileceksiniz, devam etmek istediğinize emin misiniz?", "DİKKAT!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Yedek Dosyası |*.bak";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string dbname = "Hizlisatis";//@"[D:\Hizlisatis.mdf]"

                        string sql = "Alter Database "+dbname+" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                        sql += "Restore Database " + dbname + " FROM DISK ='" + ofd.FileName + "' WITH REPLACE;";

                        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;Initial Catalog=master;Integrated Security=True;Connect Timeout=30");
                        SqlCommand command = new SqlCommand(sql, con);

                        con.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("Veritabanı geriyükleme işlemi başarıyla tamamlandı.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        con.Close();
                        con.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı geriyükleme hatası! Hata Kodları:\n" + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnVTYedekle_Click(object sender, EventArgs e)
        {
            DBYedekle();
        }

        private void btnDBYedekle_Click(object sender, EventArgs e)
        {
            DBYedekle();
        }

        private void btnDBGeriyukle_Click(object sender, EventArgs e)
        {
            DBGeriyukle();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.genesisteknoloji.com");
        }
    }
}
