using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form8 : Form
    {
        private readonly string rol;
        private readonly string connectionString = "Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane; Integrated Security=True;";

        public Form8(string gelenRol)
        {
            InitializeComponent();
            rol = gelenRol;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            EmanetListesiniYukle();
        }

        public void EmanetListesiniYukle()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // DataTable kullanımı DataSet'e göre daha performanslıdır
                    DataTable dt = new DataTable();
                    string query = "SELECT * FROM emanet_ekle";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    con.Open();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Görsel iyileştirmeleri yapalım
                    GridArayuzunuDuzenle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Emanet listesi yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridArayuzunuDuzenle()
        {
            if (dataGridView1.Columns.Count > 0)
            {
                // Kolon isimlerini kullanıcı dostu yapalım
                // Veritabanındaki sütun isimlerine göre buraları güncelleyebilirsin
                if (dataGridView1.Columns.Contains("emanet_kitap_adi"))
                    dataGridView1.Columns["emanet_kitap_adi"].HeaderText = "Kitap Adı";

                if (dataGridView1.Columns.Contains("emanet_tarih"))
                    dataGridView1.Columns["emanet_tarih"].HeaderText = "Teslim Tarihi";

                // Tabloyu ekrana yayalım
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Kullanıcının manuel müdahalesini engelleyelim
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;

                // Alternatif satır rengi (Okunabilirliği artırır)
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.Beige;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(rol);
            frm.Show();
            this.Close(); // Hide yerine Close daha sağlıklı bir bellek yönetimi sağlar
        }

        private void pictureBox4_Click(object sender, EventArgs e) => Application.Exit();

        private void label1_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}