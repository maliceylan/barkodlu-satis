using System;
using System.Windows.Forms;

namespace HizliSatis
{
    public partial class UCRaporlarDetaysiz : UserControl
    {
        public UCRaporlarDetaysiz()
        {
            InitializeComponent();

        }

        private void btnRaporDetaysizUCKapat_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Genesis.tbmenuac();
        }

        public void hesapla(DataGridView tablo)
        {
            tblDetaysizRapor.Rows.Clear();
            int varmi = 0;
            int satir = 0;
            for (int i = 0; i < tablo.RowCount; i++)
            {
                varmi = 0;
                satir = 0;
                for (int j = 0; j < tblDetaysizRapor.RowCount; j++)
                {
                    if (tablo.Rows[i].Cells[1].Value.ToString() == tblDetaysizRapor.Rows[j].Cells[0].Value.ToString())
                    {
                        varmi = 1;
                        satir = j;
                    }
                }
                if (varmi == 0)
                {
                    string adi = tablo.Rows[i].Cells[1].Value.ToString();
                    string miktar = tablo.Rows[i].Cells[5].Value.ToString();
                    string fiyat = tablo.Rows[i].Cells[7].Value.ToString();
                    string toplamTutar = (Convert.ToDouble(miktar) * Convert.ToDouble(fiyat)).ToString();
                    tblDetaysizRapor.Rows.Add(adi, miktar, fiyat, toplamTutar);
                }
                else
                {
                    tblDetaysizRapor.Rows[satir].Cells[1].Value = Convert.ToDouble(tblDetaysizRapor.Rows[satir].Cells[1].Value) + Convert.ToDouble(tablo.Rows[i].Cells[5].Value);
                    tblDetaysizRapor.Rows[satir].Cells[3].Value = Convert.ToDouble(tblDetaysizRapor.Rows[satir].Cells[3].Value) + Convert.ToDouble(tablo.Rows[i].Cells[5].Value) * Convert.ToDouble(tablo.Rows[i].Cells[7].Value);
                }
            }
        }

        private void btnSatisRaporExcel_Click(object sender, EventArgs e)
        {
            transferexcel exc = new transferexcel();
            exc.exc(tblDetaysizRapor, true, "SATIŞ ÖZETİ");
        }
    }
}
