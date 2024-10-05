using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kutuphane_otomasyonu
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlDataAdapter da;
        SqlDataReader dr;
        SqlCommand cmd;
        DataSet ds;
        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btn_ke_Click(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=DESKTOP-5UN8A75\\SQLEXPRESS;Initial Catalog=Berkay;Integrated Security=True;");
            cmd = new SqlCommand();
            ds = new DataSet();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO emanet_ekle (emanet_kitap_adi,emanet_tarih) " +
                "VALUES('" + bunifuMetroTextbox5.Text + "','" + bunifuMetroTextbox6.Text + "')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Veriler Veritabanına Eklendi.");
            con.Close();
        }
    }
}
