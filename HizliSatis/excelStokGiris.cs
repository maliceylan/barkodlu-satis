using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace HizliSatis
{
    class excelStokGiris
    {
        public List<string> dosyaAl(string path)
        {
            //Dosyanın okunacağı dizin
            string filePath = path;

            //Dosyayı okuyacağımı ve gerekli izinlerin ayarlanması.
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader;
            List<string> liste = new List<string>();
            int counter = 0;

            //Gönderdiğim dosya xls'mi xlsx formatında mı kontrol ediliyor.
            if (Path.GetExtension(filePath).ToUpper() == ".XLS")
            {
                //Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            //Datasete atarken ilk satırın başlık olacağını belirtiyor.
            var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            //Veriler okunmaya başlıyor.
            while (excelReader.Read())
            {
                counter++;

                //ilk satır başlık olduğu için 2.satırdan okumaya başlıyorum.
                if (counter > 1)
                {
                    try
                    {
                        liste.Add(excelReader[0].ToString());
                        liste.Add(excelReader[1].ToString());
                        liste.Add(excelReader[2].ToString());
                        liste.Add(excelReader[3].ToString());
                        liste.Add(excelReader[4].ToString());
                        liste.Add(excelReader[5].ToString());
                        liste.Add(excelReader[6].ToString());
                        liste.Add(excelReader[7].ToString());
                        liste.Add(excelReader[8].ToString());
                        liste.Add(excelReader[9].ToString());
                        liste.Add(excelReader[10].ToString());
                        liste.Add(excelReader[11].ToString());
                    }
                    catch
                    {
                        MessageBox.Show(counter.ToString());
                    }
                }
            }
            //Okuma bitiriliyor.
            excelReader.Close();
            //Veriler ekrana basılıyor.
            return liste;
        }
    }
}
