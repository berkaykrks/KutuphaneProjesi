using AForge.Video;
using AForge.Video.DirectShow;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using ZXing;
using System.Threading.Tasks; // Donmayı önlemek için bu şart!

namespace kutuphane_otomasyonu
{
    public partial class FormBarkod : Form
    {
        FilterInfoCollection cameraDevices;
        VideoCaptureDevice cam;
        bool isReading = false;

        BarcodeReader reader = new BarcodeReader
        {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions
            {
                TryHarder = true,
                PossibleFormats = new[] { BarcodeFormat.EAN_13 }
            }
        };

        public FormBarkod()
        {
            InitializeComponent();
            // Google API bağlantısı için güvenlik protokolü
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void FormBarkod_Load_1(object sender, EventArgs e)
        {
            cameraDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            cmbKameralar.Items.Clear();
            if (cameraDevices.Count == 0) return;
            foreach (FilterInfo device in cameraDevices) cmbKameralar.Items.Add(device.Name);
            cmbKameralar.SelectedIndex = 0;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnKameraBaslat_Click(object sender, EventArgs e)
        {
            StopCamera();
            cam = new VideoCaptureDevice(cameraDevices[cmbKameralar.SelectedIndex].MonikerString);

            // DroidCam yeşil ekran fix
            var resolution = cam.VideoCapabilities.FirstOrDefault(v => v.FrameSize.Width <= 1280) ?? cam.VideoCapabilities.LastOrDefault();
            if (resolution != null) cam.VideoResolution = resolution;

            cam.NewFrame += Cam_NewFrame;
            cam.Start();
            isReading = false;
        }

        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (isReading) return;

            try
            {
                using (Bitmap rawFrame = (Bitmap)eventArgs.Frame.Clone())
                {
                    // Yeşil ekranı önlemek için 24bpp formatına çeviriyoruz
                    Bitmap fixedFrame = new Bitmap(rawFrame.Width, rawFrame.Height, PixelFormat.Format24bppRgb);
                    using (Graphics g = Graphics.FromImage(fixedFrame))
                    {
                        g.DrawImage(rawFrame, 0, 0, rawFrame.Width, rawFrame.Height);
                    }

                    pictureBox2.Invoke((MethodInvoker)delegate
                    {
                        pictureBox2.Image?.Dispose();
                        pictureBox2.Image = (Bitmap)fixedFrame.Clone();
                    });

                    var result = reader.Decode(fixedFrame);
                    if (result != null)
                    {
                        isReading = true; // Tekrar okumayı durdur

                        // --- DONMAYI ÇÖZEN ASIL KISIM BURASI ---
                        Task.Run(() => {
                            StopCamera(); // Kamerayı arka planda durdur, UI beklemesin

                            this.Invoke((MethodInvoker)delegate {
                                txtISBN.Text = result.Text;
                                KitapGetir(result.Text); // API'den bilgileri çek
                            });
                        });
                    }
                    fixedFrame.Dispose();
                }
            }
            catch { }
        }

        private void StopCamera()
        {
            if (cam != null)
            {
                cam.NewFrame -= Cam_NewFrame;
                if (cam.IsRunning)
                {
                    cam.SignalToStop();
                    // cam.WaitForStop(); // Donmaya sebep olan bu satırı sildik!
                }
                cam = null;
            }
        }

        private void KitapGetir(string isbn)
        {
            try
            {
                txtKitapAdi.Text = "Aranıyor...";
                string url = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}";

                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    string json = wc.DownloadString(url);
                    JObject data = JObject.Parse(json);

                    if (data["items"] != null)
                    {
                        var book = data["items"][0]["volumeInfo"];
                        txtKitapAdi.Text = book["title"]?.ToString() ?? "İsimsiz Kitap";
                        txtYazar.Text = book["authors"] != null ? string.Join(", ", book["authors"]) : "Bilinmiyor";
                        txtYayinevi.Text = book["publisher"]?.ToString() ?? "Bilinmiyor";
                    }
                    else
                    {
                        txtKitapAdi.Text = "";
                        MessageBox.Show("Kitap bulunamadı: " + isbn);
                    }
                }
            }
            catch (Exception ex)
            {
                txtKitapAdi.Text = "Hata.";
                MessageBox.Show("Bağlantı Hatası: " + ex.Message);
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKitapAdi.Text) || txtKitapAdi.Text == "Aranıyor...") return;

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-MVK6017\\SQLEXPRESS01;Initial Catalog=DB_Kutuphane;Integrated Security=True;"))
                {
                    con.Open();
                    string query = "INSERT INTO kitap_ekle (kitap_ad, yazar_ad, kitap_yayinevi) VALUES (@ad, @yazar, @yayinevi)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ad", txtKitapAdi.Text);
                        cmd.Parameters.AddWithValue("@yazar", txtYazar.Text);
                        cmd.Parameters.AddWithValue("@yayinevi", txtYayinevi.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Kitap kaydedildi!");
                txtISBN.Clear(); txtKitapAdi.Clear(); txtYazar.Clear(); txtYayinevi.Clear();
            }
            catch (Exception ex) { MessageBox.Show("Veritabanı Hatası: " + ex.Message); }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            base.OnFormClosing(e);
        }
    }
}