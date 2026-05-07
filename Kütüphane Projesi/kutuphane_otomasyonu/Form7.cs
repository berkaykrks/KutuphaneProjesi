using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form7 : Form
    {
        private readonly string rol;
        private readonly string connectionString = "Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane; Integrated Security=True;";

        public Form7(string gelenRol)
        {
            InitializeComponent();
            rol = gelenRol;
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            UyeListesiniGetir();
        }

        private void UyeListesiniGetir()
        {
            try
            {
                // 'using' kullanımı veritabanı bağlantısının güvenle kapanmasını sağlar
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM üye_ekle";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();

                    con.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // DataGridView'i görsel olarak iyileştirelim
                    DataGridDuzenle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Üye listesi yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridDuzenle()
        {
            if (dataGridView1.Columns.Count > 0)
            {
                // Kolon başlıklarını Türkçeleştirelim ve güzelleştirelim
                dataGridView1.Columns["üye_adi"].HeaderText = "Ad";
                dataGridView1.Columns["üye_soyadi"].HeaderText = "Soyad";
                dataGridView1.Columns["üye_telefon"].HeaderText = "Telefon";
                dataGridView1.Columns["üye_eposta"].HeaderText = "E-Posta";
                dataGridView1.Columns["üye_adres"].HeaderText = "Adres";

                // Otomatik genişlik ayarı (Tüm alanı kaplaması için)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Kullanıcının satır eklemesini engelleyelim (Sadece listeleme ekranı olduğu için)
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(rol);
            frm.Show();
            this.Close(); // Belleği temizlemek için Close kullanıyoruz
        }

        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void pictureBox4_Click(object sender, EventArgs e) => Application.Exit();

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}