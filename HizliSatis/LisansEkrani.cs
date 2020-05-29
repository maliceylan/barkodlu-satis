using System;
using System.Windows.Forms;

namespace HizliSatis
{
    public partial class LisansEkrani : Form
    {
        public LisansEkrani()
        {
            InitializeComponent();
        }

        private void LisansEkrani_Load(object sender, EventArgs e)
        {
            txtkey.Text = Lisans.YeniKey();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Lisans.KeyDogrula(txtkey.Text, txtlic.Text);
            if (Lisans.LICKontrol())
            {
                MessageBox.Show("Program ömür boyu kullanım olarak lisanslanmıştır.\nGüle güle kullanın.", "Lisanslama Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Form frm = new Genesis();
                frm.Show();
            }
        }
        private void btnKopyala_Click(object sender, EventArgs e)
        {
            txtkey.SelectAll();
            txtkey.Copy();
        }

        private void txtlic_MouseClick(object sender, MouseEventArgs e)
        {
            txtlic.SelectAll();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
