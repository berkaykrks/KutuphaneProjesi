using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions; // E-posta kontrolü için gerekli

namespace kutuphane_otomasyonu
{
    public partial class Form4 : Form
    {
        private readonly string rol;
        private readonly string connectionString = "Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane; Integrated Security=True;";

        public Form4(string gelenRol)
        {
            InitializeComponent();
            rol = gelenRol;
        }

        private void btn_ke_Click(object sender, EventArgs e)
        {
            // 1. Temel Boşluk Kontrolü
            if (string.IsNullOrWhiteSpace(bunifuMetroTextbox1.Text) || string.IsNullOrWhiteSpace(bunifuMetroTextbox2.Text))
            {
                MessageBox.Show("Üye adı ve soyadı boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. E-posta Format Kontrolü (Opsiyonel ama profesyonel bir dokunuş)
            if (!string.IsNullOrWhiteSpace(bunifuMetroTextbox4.Text) && !IsValidEmail(bunifuMetroTextbox4.Text))
            {
                MessageBox.Show("Lütfen geçerli bir e-posta adresi giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Veritabanı İşlemi (Parametreli)
            string query = "INSERT INTO üye_ekle (üye_adi, üye_soyadi, üye_telefon, üye_eposta, üye_adres) " +
                           "VALUES (@ad, @soyadi, @tel, @mail, @adres)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ad", bunifuMetroTextbox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@soyadi", bunifuMetroTextbox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@tel", bunifuMetroTextbox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@mail", bunifuMetroTextbox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@adres", bunifuMetroTextbox5.Text.Trim());

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Üye başarıyla sisteme kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Temizle();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // E-posta formatını kontrol eden yardımcı metod
        private bool IsValidEmail(string email)
        {
            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            }
            catch { return false; }
        }

        private void Temizle()
        {
            bunifuMetroTextbox1.Text = "";
            bunifuMetroTextbox2.Text = "";
            bunifuMetroTextbox3.Text = "(___) ___.____";
            bunifuMetroTextbox4.Text = "";
            bunifuMetroTextbox5.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(rol);
            frm.Show();
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e) => Application.Exit();

        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void bunifuMetroTextbox3_OnValueChanged(object sender, EventArgs e) { }
    }
}