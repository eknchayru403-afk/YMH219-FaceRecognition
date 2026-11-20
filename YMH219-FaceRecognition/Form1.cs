using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;

namespace YMH219_FaceRecognition
{
    public partial class Form1 : Form
    {
        private FaceRecognitionService _faceService;
        private VideoCapture _capture;
        private CancellationTokenSource _cameraCts;
        private Mat _currentRawFrame;
        private readonly object _frameLock = new object();

        public Form1()
        {
            InitializeComponent();
            InitializeFaceService();
        }

        private void InitializeFaceService()
        {
            try
            {
                string cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml");
                _faceService = new FaceRecognitionService(cascadePath);
                RefreshDatasetList();
            }
            catch (Exception ex)
            {
                DurumGuncelle("Model dosyası yüklenemedi!", Color.Crimson);
                MessageBox.Show($"Yüz algılama modeli başlatılamadı:\n{ex.Message}", "Model Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKameraBaslat_Click(object sender, EventArgs e)
        {
            if (_faceService == null)
            {
                MessageBox.Show("Yüz algılama model dosyası (haarcascade_frontalface_default.xml) eksik.", "Model Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
                if (!_capture.IsOpened())
                {
                    _capture.Dispose();
                    _capture = new VideoCapture(0);
                }

                if (!_capture.IsOpened())
                {
                    _capture.Dispose();
                    _capture = null;
                    DurumGuncelle("Kamera bulunamadı veya açılamadı!", Color.Crimson);
                    MessageBox.Show("Bilgisayara bağlı aktif bir kamera bulunamadı veya başka bir uygulama tarafından kullanılıyor.", "Kamera Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnKameraBaslat.Enabled = false;
                btnKameraDurdur.Enabled = true;
                DurumGuncelle("Canlı kamera akışı aktif - Yüz algılama ve tanıma çalışıyor...", Color.SeaGreen);

                _cameraCts = new CancellationTokenSource();
                _ = CameraLoopAsync(_cameraCts.Token);
            }
            catch (Exception ex)
            {
                DurumGuncelle("Kamera başlatma hatası!", Color.Crimson);
                MessageBox.Show($"Kamera başlatılırken hata oluştu:\n{ex.Message}", "Kamera Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CameraLoopAsync(CancellationToken token)
        {
            using Mat frame = new Mat();
            try
            {
                while (!token.IsCancellationRequested && _capture != null && _capture.IsOpened())
                {
                    if (!_capture.Read(frame) || frame.Empty())
                    {
                        await Task.Delay(30, token);
                        continue;
                    }

                    lock (_frameLock)
                    {
                        _currentRawFrame?.Dispose();
                        _currentRawFrame = frame.Clone();
                    }

                    ProcessAndDisplayFrame(frame);
                    await Task.Delay(35, token);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    Invoke(new Action(() =>
                    {
                        StopCamera();
                        MessageBox.Show($"Kamera akışı kesildi:\n{ex.Message}", "Kamera Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
            }
        }

        private void btnKameraDurdur_Click(object sender, EventArgs e)
        {
            StopCamera();
            DurumGuncelle("Kamera durduruldu.", Color.DarkOrange);
        }

        private void StopCamera()
        {
            _cameraCts?.Cancel();
            _cameraCts = null;

            if (_capture != null)
            {
                try
                {
                    _capture.Release();
                    _capture.Dispose();
                }
                catch { }
                _capture = null;
            }

            btnKameraBaslat.Enabled = true;
            btnKameraDurdur.Enabled = false;
        }

        private void btnGoruntuSec_Click(object sender, EventArgs e)
        {
            if (_faceService == null)
            {
                MessageBox.Show("Yüz algılama model dosyası yüklü değil.", "Model Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StopCamera();

            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Yüz Algılama İçin Görüntü Seçin",
                Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                Mat img = Cv2.ImRead(ofd.FileName, ImreadModes.Color);
                if (img.Empty())
                {
                    MessageBox.Show("Seçilen görüntü dosyası okunamadı.", "Dosya Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lock (_frameLock)
                {
                    _currentRawFrame?.Dispose();
                    _currentRawFrame = img.Clone();
                }

                ProcessAndDisplayFrame(img);
                img.Dispose();
                DurumGuncelle($"Dosya işlendi: {Path.GetFileName(ofd.FileName)}", Color.SteelBlue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Görüntü işlenirken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessAndDisplayFrame(Mat bgrFrame)
        {
            var results = _faceService.AnalyzeFrame(bgrFrame);
            using Mat annotated = bgrFrame.Clone();

            foreach (var res in results)
            {
                Scalar boxColor = res.ConfidencePercent >= 45.0 ? Scalar.LimeGreen : Scalar.OrangeRed;
                Cv2.Rectangle(annotated, res.Bounds, boxColor, 2);

                string tag = res.ConfidencePercent > 0
                    ? $"{res.PredictedLabel} (%{res.ConfidencePercent:0.0})"
                    : res.PredictedLabel;

                int textY = Math.Max(22, res.Bounds.Y - 8);
                Cv2.PutText(annotated, tag, new Point(res.Bounds.X, textY), HersheyFonts.HersheySimplex, 0.65, boxColor, 2);
            }

            Bitmap bmp = BitmapConverter.ToBitmap(annotated);

            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUiFrame(bmp, results)));
            }
            else
            {
                UpdateUiFrame(bmp, results);
            }
        }

        private void UpdateUiFrame(Bitmap bmp, List<FaceDetectionResult> results)
        {
            var old = picGoruntu.Image;
            picGoruntu.Image = bmp;
            old?.Dispose();

            lstSonuclar.Items.Clear();
            if (results.Count == 0)
            {
                lstSonuclar.Items.Add("Görüntüde yüz algılanmadı.");
            }
            else
            {
                for (int i = 0; i < results.Count; i++)
                {
                    var r = results[i];
                    lstSonuclar.Items.Add($"Yüz #{i + 1}: {r.PredictedLabel} | Güven: %{r.ConfidencePercent:0.0}");
                }
            }
        }

        private void btnKameradanEgit_Click(object sender, EventArgs e)
        {
            if (_faceService == null) return;

            Mat snapshot = null;
            lock (_frameLock)
            {
                if (_currentRawFrame != null && !_currentRawFrame.Empty())
                {
                    snapshot = _currentRawFrame.Clone();
                }
            }

            if (snapshot == null)
            {
                MessageBox.Show("Önce kamerayı başlatın veya bir görüntü seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (snapshot)
            {
                int added = _faceService.AddSamplesFromFrame(snapshot, txtEtiket.Text);
                if (added == 0)
                {
                    MessageBox.Show("Mevcut karede yüz algılanamadığı için eğitim örneği eklenemedi.", "Yüz Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshDatasetList();
                ProcessAndDisplayFrame(snapshot);
                MessageBox.Show($"'{txtEtiket.Text}' etiketi için {added} adet yüz örneği yerel eğitim kümesine eklendi.", "Eğitim Güncellendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDosyadanEgit_Click(object sender, EventArgs e)
        {
            if (_faceService == null) return;

            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Eğitim Örneği Olarak Eklenecek Yüz Fotoğrafı Seçin",
                Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            using Mat img = Cv2.ImRead(ofd.FileName, ImreadModes.Color);
            if (img.Empty())
            {
                MessageBox.Show("Fotoğraf dosyası okunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int added = _faceService.AddSamplesFromFrame(img, txtEtiket.Text);
            if (added == 0)
            {
                MessageBox.Show("Seçilen fotoğrafta yüz tespit edilemedi.", "Yüz Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshDatasetList();
            MessageBox.Show($"'{txtEtiket.Text}' sınıfına {added} yeni örnek eklendi.", "Eğitim Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnVeriTemizle_Click(object sender, EventArgs e)
        {
            if (_faceService == null) return;

            _faceService.ClearDataset();
            RefreshDatasetList();
            MessageBox.Show("Yerel eğitim kümesi temizlendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnYuzKaydet_Click(object sender, EventArgs e)
        {
            if (_faceService == null) return;

            Mat snapshot = null;
            lock (_frameLock)
            {
                if (_currentRawFrame != null && !_currentRawFrame.Empty())
                {
                    snapshot = _currentRawFrame.Clone();
                }
            }

            if (snapshot == null)
            {
                MessageBox.Show("Kaydedilecek aktif bir görüntü bulunmuyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (snapshot)
            {
                int count = _faceService.SaveDetectedFaces(snapshot);
                if (count == 0)
                {
                    MessageBox.Show("Karede kaydedilecek yüz algılanmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"{count} adet yüz kırpılarak klasöre kaydedildi:\n{_faceService.DetectedFacesDirectory}", "Kaydedildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RefreshDatasetList()
        {
            lstEgitimVerileri.Items.Clear();
            var grouped = _faceService.Samples
                .GroupBy(s => s.Label)
                .Select(g => $"{g.Key} ({g.Count()} örnek)");

            foreach (var item in grouped)
            {
                lstEgitimVerileri.Items.Add(item);
            }

            if (lstEgitimVerileri.Items.Count == 0)
            {
                lstEgitimVerileri.Items.Add("Henüz eğitim örneği eklenmedi.");
            }
        }

        private void DurumGuncelle(string mesaj, Color renk)
        {
            lblDurum.Text = $"● Durum: {mesaj}";
            lblDurum.ForeColor = renk;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
            lock (_frameLock)
            {
                _currentRawFrame?.Dispose();
            }
            _faceService?.Dispose();
        }
    }
}