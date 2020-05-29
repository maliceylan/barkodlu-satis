using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace HizliSatis
{
    public class transferexcel
    {
        private Excel.Application XcellApp;
        private Excel.Workbook XcellBook;
        private Excel.Worksheet XcellSheet;

        public void exc(DataGridView tablo, bool tumu, string baslik)
        {
            if (tablo.RowCount > 0)
            {
                try
                {
                    tablo.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                    if (tumu) tablo.SelectAll();
                    DataObject dataObj = tablo.GetClipboardContent();
                    if (dataObj != null) Clipboard.SetDataObject(dataObj);
                    object misValue = System.Reflection.Missing.Value;
                    XcellApp = new Microsoft.Office.Interop.Excel.Application();
                    XcellApp.Visible = true;
                    XcellBook = XcellApp.Workbooks.Add(misValue);
                    XcellSheet = (Excel.Worksheet)XcellBook.Worksheets.get_Item(1);
                    Excel.Range alan = (Excel.Range)XcellSheet.Cells[3, 1];//!IMPORTANT 
                    alan.Select();
                    XcellSheet.PasteSpecial(alan, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                    Excel.Range tarihalani = XcellSheet.Range[XcellSheet.Cells[1, 1], XcellSheet.Cells[1, tablo.ColumnCount]];
                    Excel.Range headeralani = XcellSheet.Range[XcellSheet.Cells[3, 1], XcellSheet.Cells[3, tablo.ColumnCount]];
                    Excel.Range baslikalani = XcellSheet.Range[XcellSheet.Cells[2, 1], XcellSheet.Cells[2, tablo.ColumnCount]];
                    Excel.Range baslikharicalan = XcellSheet.Range[XcellSheet.Cells[3, 1], XcellSheet.Cells[tablo.RowCount, tablo.ColumnCount]];
                    //Tasarım
                    tarihalani.Merge();
                    tarihalani.FormulaR1C1 = "Rapor Tarihi: " + DateTime.Today.ToString("dd/MM/yyyy") + "  " + DateTime.Now.ToLongTimeString();

                    baslikalani.Merge();
                    baslikalani.Font.Size = 30;
                    baslikalani.HorizontalAlignment = 3;
                    baslikalani.FormulaR1C1 = baslik;

                    baslikalani.Interior.Color = Excel.XlRgbColor.rgbLightGreen;
                    headeralani.Interior.Color = Excel.XlRgbColor.rgbYellow;
                    headeralani.Font.Bold = true;

                    XcellSheet.UsedRange.EntireColumn.VerticalAlignment = 2;
                    baslikharicalan.EntireColumn.HorizontalAlignment = 3;
                    XcellSheet.UsedRange.Borders.Weight = 2;
                    XcellSheet.UsedRange.EntireColumn.RowHeight = 20;
                    baslikharicalan.EntireColumn.AutoFit();
                    baslikalani.RowHeight = 40;
                }
                catch (Exception s)
                {
                    MessageBox.Show("Hata: " + s, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Tablo boş!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
