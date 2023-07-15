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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kutuphane_otomasyonu
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlDataAdapter da;
        SqlDataReader dr;
        SqlCommand cmd;
        DataSet ds;
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

        private void btn_ke2_Click(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=DESKTOP-1BTGRPP;Initial Catalog=Berkay;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            cmd = new SqlCommand();
            ds = new DataSet();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO kitap_ekle (kitap_ad,yazar_ad,kitap_baski_yil,kitap_kategori,kitap_yayinevi,kitap_özet) " +
                "VALUES('" + bunifuMetroTextbox1.Text + "','" + bunifuMetroTextbox2.Text + "','" + bunifuMetroTextbox3.Text + "','" + bunifuMetroTextbox4.Text + "','" + bunifuMetroTextbox5.Text + "','" + bunifuMetroTextbox6.Text + "')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Veriler Veritabanına Eklendi.");
            con.Close();

        }
    }
}
