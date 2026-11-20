namespace YMH219_FaceRecognition
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblDurum = new Label();
            picGoruntu = new PictureBox();
            btnKameraBaslat = new Button();
            btnKameraDurdur = new Button();
            btnGoruntuSec = new Button();
            btnYuzKaydet = new Button();
            grpEgitim = new GroupBox();
            lblEtiket = new Label();
            txtEtiket = new TextBox();
            btnKameradanEgit = new Button();
            btnDosyadanEgit = new Button();
            btnVeriTemizle = new Button();
            lblKayitliSiniflar = new Label();
            lstEgitimVerileri = new ListBox();
            grpSonuc = new GroupBox();
            lstSonuclar = new ListBox();
            ((System.ComponentModel.ISupportInitialize)picGoruntu).BeginInit();
            grpEgitim.SuspendLayout();
            grpSonuc.SuspendLayout();
            SuspendLayout();

            // lblDurum
            lblDurum.BackColor = Color.FromArgb(230, 240, 250);
            lblDurum.BorderStyle = BorderStyle.FixedSingle;
            lblDurum.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDurum.ForeColor = Color.DarkSlateBlue;
            lblDurum.Location = new Point(15, 12);
            lblDurum.Name = "lblDurum";
            lblDurum.Size = new Size(940, 30);
            lblDurum.Text = "● Durum: Hazır - Kamera başlatabilir veya görüntü seçebilirsiniz.";
            lblDurum.TextAlign = ContentAlignment.MiddleLeft;

            // picGoruntu
            picGoruntu.BackColor = Color.FromArgb(30, 30, 30);
            picGoruntu.BorderStyle = BorderStyle.FixedSingle;
            picGoruntu.Location = new Point(15, 52);
            picGoruntu.Name = "picGoruntu";
            picGoruntu.Size = new Size(580, 420);
            picGoruntu.SizeMode = PictureBoxSizeMode.Zoom;
            picGoruntu.TabStop = false;

            // btnKameraBaslat
            btnKameraBaslat.BackColor = Color.SeaGreen;
            btnKameraBaslat.FlatStyle = FlatStyle.Flat;
            btnKameraBaslat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnKameraBaslat.ForeColor = Color.White;
            btnKameraBaslat.Location = new Point(15, 482);
            btnKameraBaslat.Name = "btnKameraBaslat";
            btnKameraBaslat.Size = new Size(135, 36);
            btnKameraBaslat.Text = "Kamera Başlat";
            btnKameraBaslat.UseVisualStyleBackColor = false;
            btnKameraBaslat.Click += btnKameraBaslat_Click;

            // btnKameraDurdur
            btnKameraDurdur.BackColor = Color.IndianRed;
            btnKameraDurdur.Enabled = false;
            btnKameraDurdur.FlatStyle = FlatStyle.Flat;
            btnKameraDurdur.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnKameraDurdur.ForeColor = Color.White;
            btnKameraDurdur.Location = new Point(158, 482);
            btnKameraDurdur.Name = "btnKameraDurdur";
            btnKameraDurdur.Size = new Size(135, 36);
            btnKameraDurdur.Text = "Kamera Durdur";
            btnKameraDurdur.UseVisualStyleBackColor = false;
            btnKameraDurdur.Click += btnKameraDurdur_Click;

            // btnGoruntuSec
            btnGoruntuSec.BackColor = Color.SteelBlue;
            btnGoruntuSec.FlatStyle = FlatStyle.Flat;
            btnGoruntuSec.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGoruntuSec.ForeColor = Color.White;
            btnGoruntuSec.Location = new Point(301, 482);
            btnGoruntuSec.Name = "btnGoruntuSec";
            btnGoruntuSec.Size = new Size(135, 36);
            btnGoruntuSec.Text = "Görüntü Seç";
            btnGoruntuSec.UseVisualStyleBackColor = false;
            btnGoruntuSec.Click += btnGoruntuSec_Click;

            // btnYuzKaydet
            btnYuzKaydet.BackColor = Color.DarkSlateGray;
            btnYuzKaydet.FlatStyle = FlatStyle.Flat;
            btnYuzKaydet.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnYuzKaydet.ForeColor = Color.White;
            btnYuzKaydet.Location = new Point(444, 482);
            btnYuzKaydet.Name = "btnYuzKaydet";
            btnYuzKaydet.Size = new Size(151, 36);
            btnYuzKaydet.Text = "Algılanan Yüzü Kaydet";
            btnYuzKaydet.UseVisualStyleBackColor = false;
            btnYuzKaydet.Click += btnYuzKaydet_Click;

            // grpEgitim
            grpEgitim.Controls.Add(lstEgitimVerileri);
            grpEgitim.Controls.Add(lblKayitliSiniflar);
            grpEgitim.Controls.Add(btnVeriTemizle);
            grpEgitim.Controls.Add(btnDosyadanEgit);
            grpEgitim.Controls.Add(btnKameradanEgit);
            grpEgitim.Controls.Add(txtEtiket);
            grpEgitim.Controls.Add(lblEtiket);
            grpEgitim.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpEgitim.Location = new Point(610, 52);
            grpEgitim.Name = "grpEgitim";
            grpEgitim.Size = new Size(345, 255);
            grpEgitim.TabStop = false;
            grpEgitim.Text = "Yerel Örnek Ekleme (Eğitim Kümesi)";

            // lblEtiket
            lblEtiket.AutoSize = true;
            lblEtiket.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEtiket.Location = new Point(12, 30);
            lblEtiket.Name = "lblEtiket";
            lblEtiket.Size = new Size(92, 15);
            lblEtiket.Text = "Kişi / Sınıf Adı:";

            // txtEtiket
            txtEtiket.Font = new Font("Segoe UI", 9.5F);
            txtEtiket.Location = new Point(112, 26);
            txtEtiket.Name = "txtEtiket";
            txtEtiket.Size = new Size(220, 24);
            txtEtiket.Text = "Hayrunnisa";

            // btnKameradanEgit
            btnKameradanEgit.BackColor = Color.DarkOrange;
            btnKameradanEgit.FlatStyle = FlatStyle.Flat;
            btnKameradanEgit.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnKameradanEgit.ForeColor = Color.White;
            btnKameradanEgit.Location = new Point(12, 60);
            btnKameradanEgit.Name = "btnKameradanEgit";
            btnKameradanEgit.Size = new Size(156, 32);
            btnKameradanEgit.Text = "Mevcut Kareyi Ekle";
            btnKameradanEgit.UseVisualStyleBackColor = false;
            btnKameradanEgit.Click += btnKameradanEgit_Click;

            // btnDosyadanEgit
            btnDosyadanEgit.BackColor = Color.Teal;
            btnDosyadanEgit.FlatStyle = FlatStyle.Flat;
            btnDosyadanEgit.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnDosyadanEgit.ForeColor = Color.White;
            btnDosyadanEgit.Location = new Point(176, 60);
            btnDosyadanEgit.Name = "btnDosyadanEgit";
            btnDosyadanEgit.Size = new Size(156, 32);
            btnDosyadanEgit.Text = "Fotoğraftan Örnek Ekle";
            btnDosyadanEgit.UseVisualStyleBackColor = false;
            btnDosyadanEgit.Click += btnDosyadanEgit_Click;

            // lblKayitliSiniflar
            lblKayitliSiniflar.AutoSize = true;
            lblKayitliSiniflar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblKayitliSiniflar.Location = new Point(12, 102);
            lblKayitliSiniflar.Name = "lblKayitliSiniflar";
            lblKayitliSiniflar.Size = new Size(148, 15);
            lblKayitliSiniflar.Text = "Kayıtlı Eğitim Örnekleri:";

            // lstEgitimVerileri
            lstEgitimVerileri.Font = new Font("Consolas", 9F);
            lstEgitimVerileri.FormattingEnabled = true;
            lstEgitimVerileri.ItemHeight = 14;
            lstEgitimVerileri.Location = new Point(12, 122);
            lstEgitimVerileri.Name = "lstEgitimVerileri";
            lstEgitimVerileri.Size = new Size(320, 88);

            // btnVeriTemizle
            btnVeriTemizle.BackColor = Color.SlateGray;
            btnVeriTemizle.FlatStyle = FlatStyle.Flat;
            btnVeriTemizle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnVeriTemizle.ForeColor = Color.White;
            btnVeriTemizle.Location = new Point(12, 216);
            btnVeriTemizle.Name = "btnVeriTemizle";
            btnVeriTemizle.Size = new Size(320, 28);
            btnVeriTemizle.Text = "Eğitim Kümesini Sıfırla";
            btnVeriTemizle.UseVisualStyleBackColor = false;
            btnVeriTemizle.Click += btnVeriTemizle_Click;

            // grpSonuc
            grpSonuc.Controls.Add(lstSonuclar);
            grpSonuc.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpSonuc.Location = new Point(610, 315);
            grpSonuc.Name = "grpSonuc";
            grpSonuc.Size = new Size(345, 203);
            grpSonuc.TabStop = false;
            grpSonuc.Text = "Algılama ve Tanıma Sonuçları";

            // lstSonuclar
            lstSonuclar.Font = new Font("Consolas", 9F);
            lstSonuclar.FormattingEnabled = true;
            lstSonuclar.ItemHeight = 14;
            lstSonuclar.Location = new Point(12, 26);
            lstSonuclar.Name = "lstSonuclar";
            lstSonuclar.Size = new Size(320, 164);

            // Form1
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(970, 532);
            Controls.Add(grpSonuc);
            Controls.Add(grpEgitim);
            Controls.Add(btnYuzKaydet);
            Controls.Add(btnGoruntuSec);
            Controls.Add(btnKameraDurdur);
            Controls.Add(btnKameraBaslat);
            Controls.Add(picGoruntu);
            Controls.Add(lblDurum);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "YMH 219 - OpenCV ile Yüz Algılama ve Tanıma Uygulaması";
            FormClosing += Form1_FormClosing;
            ((System.ComponentModel.ISupportInitialize)picGoruntu).EndInit();
            grpEgitim.ResumeLayout(false);
            grpEgitim.PerformLayout();
            grpSonuc.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblDurum;
        private PictureBox picGoruntu;
        private Button btnKameraBaslat;
        private Button btnKameraDurdur;
        private Button btnGoruntuSec;
        private Button btnYuzKaydet;
        private GroupBox grpEgitim;
        private Label lblEtiket;
        private TextBox txtEtiket;
        private Button btnKameradanEgit;
        private Button btnDosyadanEgit;
        private Button btnVeriTemizle;
        private Label lblKayitliSiniflar;
        private ListBox lstEgitimVerileri;
        private GroupBox grpSonuc;
        private ListBox lstSonuclar;
    }
}