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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlDataAdapter da;
        SqlDataReader dr;
        SqlCommand cmd;
        DataSet ds;
        public void verigetir()
        {
            con = new SqlConnection("Data Source=DESKTOP-1BTGRPP;Initial Catalog=Berkay;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            da = new SqlDataAdapter("Select * From emanet_ekle", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "emanet_ekle");

            dataGridView1.DataSource = ds.Tables["emanet_ekle"];
            con.Close();

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

        private void Form8_Load(object sender, EventArgs e)
        {
            verigetir();
        }
    }
}
