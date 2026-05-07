using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form1 : Form
    {
        // Bağlantı dizesini tek bir yerden yönetmek ileride değişiklik yapmanı kolaylaştırır.
        private readonly string connectionString = "Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane;Integrated Security=True;";
        public int kalanHak = 3;

        public Form1()
        {
            InitializeComponent();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            // Alanların boş olup olmadığını kontrol edelim
            if (string.IsNullOrWhiteSpace(txtbx_ya.Text) || string.IsNullOrWhiteSpace(txtbx_sifre.Text))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 'using' bloğu işlem bitince veya hata olunca bağlantıyı otomatik kapatır.
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT rol FROM parola WHERE kullanici_adi=@adi AND kullanici_parola=@sifre";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@adi", txtbx_ya.Text.Trim());
                        cmd.Parameters.AddWithValue("@sifre", txtbx_sifre.Text.Trim());

                        object result = cmd.ExecuteScalar(); // Tek bir değer (rol) döndüğü için ExecuteScalar daha hızlıdır.

                        if (result != null)
                        {
                            string rol = result.ToString();
                            MessageBox.Show($"Hoş geldiniz! Yetki: {rol}", "Giriş Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Form geçiş mantığı
                            HandleLoginSuccess(rol);
                        }
                        else
                        {
                            HandleLoginFailure();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleLoginSuccess(string rol)
        {
            this.Hide();
            // Not: Eğer Form2 ve Form3 içerik olarak çok benzerse, 
            // tek bir Dashboard formu yapıp yetkiye göre buton gizlemek daha profesyoneldir.
            if (rol.ToLower() == "admin")
            {
                Form2 adminForm = new Form2(rol);
                adminForm.Show();
            }
            else
            {
                Form3 userForm = new Form3(rol);
                userForm.Show();
            }
        }

        private void HandleLoginFailure()
        {
            kalanHak--;
            if (kalanHak > 0)
            {
                MessageBox.Show($"Hatalı giriş! Kalan hakkınız: {kalanHak}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtbx_sifre.Text = "";
            }
            else
            {
                MessageBox.Show("3 kez hatalı giriş yaptınız. Uygulama kapatılıyor.", "Sistem Kilitlendi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e) => Application.Exit();

        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {
            Form9 frm = new Form9();
            frm.Show();
            this.Hide();
        }
    }
}