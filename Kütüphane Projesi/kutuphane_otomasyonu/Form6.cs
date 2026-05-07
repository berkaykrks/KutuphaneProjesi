using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form6 : Form
    {
        private readonly string rol;
        private readonly string connectionString = "Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane; Integrated Security=True;";

        public Form6(string gelenRol)
        {
            InitializeComponent();
            rol = gelenRol;
        }

        // Verileri getiren ana metod
        public void VerileriYukle(string filtreSorgusu = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Eğer filtre sorgusu boşsa hepsini getir, doluysa filtreli getir
                    string query = string.IsNullOrEmpty(filtreSorgusu)
                                   ? "SELECT * FROM kitap_ekle"
                                   : filtreSorgusu;

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    // Arama işlemi varsa parametreyi ekle
                    if (!string.IsNullOrEmpty(bunifuMetroTextbox1.Text))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@aranan", "%" + bunifuMetroTextbox1.Text.Trim() + "%");
                    }

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // DataGridView başlıklarını güzelleştirelim
                    if (dataGridView1.Columns.Count > 0)
                    {
                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[1].HeaderText = "Kitap Adı";
                        dataGridView1.Columns[2].HeaderText = "Yazar";
                        dataGridView1.Columns[3].HeaderText = "Baskı Yılı";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yükleme hatası: " + ex.Message);
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            VerileriYukle();
            // Başlangıçta label'ları temizleyelim
            LabelTemizle();
        }

        private void btn_giris_Click(object sender, EventArgs e)
        {
            string arananKelime = bunifuMetroTextbox1.Text.Trim();

            if (string.IsNullOrEmpty(arananKelime))
            {
                VerileriYukle(); // Boşsa tüm listeyi tazele
                return;
            }

            // Arama sorgusunu DataGridView'e yansıtmak daha kullanıcı dostudur
            string query = "SELECT * FROM kitap_ekle WHERE kitap_ad LIKE @aranan";
            VerileriYukle(query);

            // Ayrıca tekil veri detaylarını label'lara da basalım (Senin mantığın)
            DetaylariGetir(arananKelime);
        }

        private void DetaylariGetir(string kitapAd)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 * FROM kitap_ekle WHERE kitap_ad LIKE @aranan";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@aranan", "%" + kitapAd + "%");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    label7.Text = "Kitap bulundu:";
                    label8.Text = "ID: " + dr["kitap_id"].ToString();
                    label9.Text = "Ad: " + dr["kitap_ad"].ToString();
                    label10.Text = "Yazar: " + dr["yazar_ad"].ToString();
                    label11.Text = "Yıl: " + dr["kitap_baski_yil"].ToString();
                    label12.Text = "Kategori: " + dr["kitap_kategori"].ToString();
                    label13.Text = "Yayınevi: " + dr["kitap_yayinevi"].ToString();
                }
                else
                {
                    LabelTemizle();
                    label7.Text = "Kitap bulunamadı.";
                }
            }
        }

        private void LabelTemizle()
        {
            label7.Text = label8.Text = label9.Text = label10.Text =
            label11.Text = label12.Text = label13.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(rol);
            frm.Show();
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e) => Application.Exit();
        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
    }
}