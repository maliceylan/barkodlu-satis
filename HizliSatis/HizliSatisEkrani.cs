using System;
using System.Windows.Forms;

namespace HizliSatis
{
    class HizliSatisEkrani
    {
        public double toplamTutarHesap(DataGridView tbl, double toplamt)
        {
            toplamt = 0;
            for (int i = 0; i < tbl.RowCount; i++)
            {
                toplamt += double.Parse(tbl.Rows[i].Cells[7].Value.ToString());
                toplamt = Math.Round(toplamt, 2);
            }
            return toplamt;
        }
    }
}
