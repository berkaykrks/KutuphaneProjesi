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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlDataAdapter da;
        SqlDataReader dr;
        SqlCommand cmd;
        DataSet ds;
        public int kalanHak = 3;

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=DESKTOP-1BTGRPP;Initial Catalog=Berkay;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            cmd = new SqlCommand();
            ds = new DataSet();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM parola where kullanici_adi='" +txtbx_ya.Text + "'AND kullanici_parola='" + txtbx_sifre.Text + "'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Giriş başarili");
                Form2 frm = new Form2();
                frm.Show();
                this.Hide();
            }
            else
            {
                kalanHak--;
                MessageBox.Show("Giriş başarisiz");
                if (kalanHak == 0)
                    this.Close();
            }
        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {
            Form9 frm = new Form9();
            frm.Show();
            this.Hide();
        }

        private void txtbx_sifre_OnValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}
