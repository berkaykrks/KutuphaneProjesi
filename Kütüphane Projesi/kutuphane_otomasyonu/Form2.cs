using System;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form2 : Form
    {
        // Kullanıcının rolünü saklamak için bir değişken
        private readonly string _kullaniciRolu;

        // Constructor'ı rol alacak şekilde güncelledik
        public Form2(string rol = "admin")
        {
            InitializeComponent();
            _kullaniciRolu = rol.ToLower();
            YetkileriKontrolEt();
        }

        private void YetkileriKontrolEt()
        {
            // Eğer kullanıcı admin değilse, üye işlemlerini kısıtla
            if (_kullaniciRolu != "admin")
            {
                // Butonları pasif yapmak yerine gizlemek daha modern bir yaklaşımdır
                btn_ue.Visible = false; // Üye Ekle
                btn_ul.Visible = false; // Üye Listele
                pictureBox3.Visible = false; // Üye Ekle İkonu
                pictureBox5.Visible = false; // Üye Listele İkonu

                // Form başlığını güncelle
                this.Text = "Kütüphane Paneli (Kısıtlı Yetki)";
            }
        }

        #region Navigasyon Metotları (Formlar Arası Geçiş)

        // Ortak bir metod ile form açma işlemini merkezileştirebiliriz (Opsiyonel)
        private void FormAc(Form yeniForm)
        {
            yeniForm.Show();
            this.Close(); // Hide yerine Close kullanmak RAM yönetimi için daha iyidir (Eğer ana form değilse)
        }

        // Mevcut hatalı kodları şunlarla değiştir:
        private void btn_ke_Click(object sender, EventArgs e) => FormAc(new Form3(_kullaniciRolu)); // Kitap Ekle

        private void btn_ee_Click(object sender, EventArgs e) => FormAc(new Form5(_kullaniciRolu)); // Emanet Ekle

        private void btn_ue_Click(object sender, EventArgs e) => FormAc(new Form4(_kullaniciRolu)); // Üye Ekle

        private void btn_kl_Click(object sender, EventArgs e) => FormAc(new Form6(_kullaniciRolu)); // Kitap Listele

        private void btn_el_Click(object sender, EventArgs e) => FormAc(new Form8(_kullaniciRolu)); // Emanet Listele

        private void btn_ul_Click(object sender, EventArgs e) => FormAc(new Form7(_kullaniciRolu)); // Üye Listele

        #endregion

        #region Sistem Butonları

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Uygulamadan çıkmak istediğinize emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void pictureBox1_Click(object sender, EventArgs e) // Geri Dön butonu
        {
            Form1 frmGiris = new Form1();
            frmGiris.Show();
            this.Close();
        }

        #endregion

        // Kullanılmayan boş click event'lerini kod kirliliği olmaması için silebilirsin
        private void pictureBox2_Click(object sender, EventArgs e) { }

        private void btn_barkod_Click_Click(object sender, EventArgs e)
        {
            FormBarkod frm = new FormBarkod();
            frm.Show();
        }
    }
}