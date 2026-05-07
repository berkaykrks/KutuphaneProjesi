using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form3 : Form
    {
        private readonly string rol;
        // Bağlantı cümlesini readonly olarak tanımlamak güvenlidir
        private readonly string connectionString = "Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane; Integrated Security=True;";

        public Form3(string gelenRol)
        {
            InitializeComponent();
            rol = gelenRol;
        }

        private void btn_ke2_Click(object sender, EventArgs e)
        {
            // Basit bir boş alan kontrolü
            if (string.IsNullOrWhiteSpace(bunifuMetroTextbox1.Text))
            {
                MessageBox.Show("Kitap adı boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parametreli sorgu kullanarak SQL Injection'ı engelliyoruz
            string query = "INSERT INTO kitap_ekle (kitap_ad, yazar_ad, kitap_baski_yil, kitap_kategori, kitap_yayinevi, kitap_özet) " +
                           "VALUES (@ad, @yazar, @yil, @kategori, @yayinevi, @ozet)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@ad", bunifuMetroTextbox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@yazar", bunifuMetroTextbox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@yil", bunifuMetroTextbox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@kategori", bunifuMetroTextbox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@yayinevi", bunifuMetroTextbox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@ozet", bunifuMetroTextbox6.Text.Trim());

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Kitap başarıyla veritabanına eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Temizle(); // Kayıttan sonra kutuları temizleyelim
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Temizle()
        {
            bunifuMetroTextbox1.Text = "";
            bunifuMetroTextbox2.Text = "";
            bunifuMetroTextbox3.Text = "";
            bunifuMetroTextbox4.Text = "";
            bunifuMetroTextbox5.Text = "";
            bunifuMetroTextbox6.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(rol);
            frm.Show();
            this.Close(); // Hide yerine Close kullanarak bellek tasarrufu yapıyoruz
        }

        private void pictureBox4_Click(object sender, EventArgs e) => Application.Exit();

        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void groupBox1_Enter(object sender, EventArgs e) { }
    }
}