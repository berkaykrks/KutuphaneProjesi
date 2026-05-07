using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form5 : Form
    {
        private readonly string rol;
        private readonly string connectionString = "Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane; Integrated Security=True;";

        public Form5(string gelenRol)
        {
            InitializeComponent();
            rol = gelenRol;
        }

        private void btn_ke_Click(object sender, EventArgs e)
        {
            // 1. Temel Boşluk Kontrolü
            if (string.IsNullOrWhiteSpace(bunifuMetroTextbox1.Text) || string.IsNullOrWhiteSpace(bunifuMetroTextbox5.Text))
            {
                MessageBox.Show("Lütfen üye adı ve kitap adını girdiğinizden emin olun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Parametreli Sorgu
            // Not: Veritabanında tablo isminin 'emanet_ekle' olduğundan ve sütun isimlerinin doğruluğundan emin ol.
            string query = "INSERT INTO emanet_ekle (üye_ad, üye_soyad, üye_tel, üye_eposta, emanet_kitap_adi, emanet_tarih) " +
                           "VALUES (@ad, @soyad, @tel, @mail, @kitapAd, @tarih)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Üye Bilgileri
                        cmd.Parameters.AddWithValue("@ad", bunifuMetroTextbox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@soyad", bunifuMetroTextbox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@tel", bunifuMetroTextbox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@mail", bunifuMetroTextbox4.Text.Trim());

                        // Kitap ve Tarih Bilgileri
                        cmd.Parameters.AddWithValue("@kitapAd", bunifuMetroTextbox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@tarih", bunifuMetroTextbox6.Text.Trim());

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Emanet işlemi başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Temizle();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Emanet kaydı sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Temizle()
        {
            // Formu temizle ve tarih formatını geri getir
            bunifuMetroTextbox1.Text = "";
            bunifuMetroTextbox2.Text = "";
            bunifuMetroTextbox3.Text = "(___) ___.____";
            bunifuMetroTextbox4.Text = "";
            bunifuMetroTextbox5.Text = "";
            bunifuMetroTextbox6.Text = "/__/__/____/";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(rol);
            frm.Show();
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e) => Application.Exit();

        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void Form5_Load(object sender, EventArgs e) { }
    }
}