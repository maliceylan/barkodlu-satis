using System;
using System.Windows.Forms;

namespace HizliSatis
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.Diagnostics.Process.GetProcessesByName("HizliSatis").Length > 1)
            {
                MessageBox.Show("Bu program zaten çalışıyor");
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                bool isLicensed;
                isLicensed = Lisans.LICKontrol();
                if (isLicensed)
                {
                    Application.Run(new Genesis());
                }
                else
                {
                    //LİSANS SAYFASI AÇILIR...
                    Application.Run(new LisansEkrani());
                }
            }
        }
    }
}
