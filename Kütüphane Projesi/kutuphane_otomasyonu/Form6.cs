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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Hide();
        }
        SqlConnection con;
        SqlDataAdapter da;
        SqlDataReader dr;
        SqlCommand cmd;
        DataSet ds;
        public void verigetir()
        {
            con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Berkay;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            da = new SqlDataAdapter("Select * From kitap_ekle", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "kitap_ekle");

            dataGridView1.DataSource = ds.Tables["kitap_ekle"];
            con.Close();

        }
        public void veriGetir2()
        {
            /* con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Berkay;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            da = new SqlDataAdapter("Select * From kitap_ekle", con);
            ds = new DataSet();
            con.Open();
            da.Fill(ds, "kitap_ekle");

            dataGridView2.DataSource = ds.Tables["kitap_ekle"];
            con.Close(); */
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            verigetir();
            veriGetir2();

           

        }

        private void btn_giris_Click(object sender, EventArgs e)
        {

           
            string arananKelime = bunifuMetroTextbox1.Text;


            con = new SqlConnection("Data Source=DESKTOP-1BTGRPP;Initial Catalog=Berkay;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            con.Open();           
            string query = "SELECT * FROM kitap_ekle WHERE kitap_ad LIKE @arananKelime";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@arananKelime", "%" + arananKelime + "%");
      
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                
                    string[] row = { dr["kitap_id"].ToString(), dr["kitap_ad"].ToString(), dr["yazar_ad"].ToString(), dr["kitap_baski_yil"].ToString(), dr["kitap_kategori"].ToString(), dr["kitap_yayinevi"].ToString() };


                    

                    label7.Text = "Aradıgınız kitap veritabanında :";
                    label8.Text = row[0] + ". ID'de bulunmaktadır.";
                    label9.Text = row[1];
                    label10.Text = row[2];
                    label11.Text = row[3];
                    label12.Text = row[4];
                    label13.Text = row[5];



                
            }
            
            dr.Close();
            con.Close();

        }
    }
}
