using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace YMH219_FaceRecognition
{
    public class FaceSample
    {
        public string Label { get; set; }
        public Mat Histogram { get; set; }
        public string FilePath { get; set; }
    }

    public class FaceDetectionResult
    {
        public Rect Bounds { get; set; }
        public string PredictedLabel { get; set; }
        public double ConfidencePercent { get; set; }
    }

    public class FaceRecognitionService : IDisposable
    {
        private readonly CascadeClassifier _faceCascade;
        private readonly List<FaceSample> _samples = new List<FaceSample>();
        private readonly string _datasetDir;
        private readonly string _detectedDir;

        public IReadOnlyList<FaceSample> Samples => _samples;
        public string DetectedFacesDirectory => _detectedDir;

        public FaceRecognitionService(string cascadeFilePath)
        {
            if (!File.Exists(cascadeFilePath))
            {
                throw new FileNotFoundException($"Yüz algılama model dosyası bulunamadı:\n{cascadeFilePath}");
            }

            _faceCascade = new CascadeClassifier(cascadeFilePath);
            if (_faceCascade.Empty())
            {
                throw new InvalidOperationException("Haar Cascade model dosyası yüklenemedi veya bozuk.");
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _datasetDir = Path.Combine(baseDir, "EgitimVerileri");
            _detectedDir = Path.Combine(baseDir, "AlgilananYuzler");

            Directory.CreateDirectory(_datasetDir);
            Directory.CreateDirectory(_detectedDir);

            LoadExistingDataset();
        }

        private void LoadExistingDataset()
        {
            _samples.Clear();
            if (!Directory.Exists(_datasetDir)) return;

            foreach (string file in Directory.GetFiles(_datasetDir, "*.png"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string label = fileName.Split('_')[0];

                using Mat gray = Cv2.ImRead(file, ImreadModes.Grayscale);
                if (!gray.Empty())
                {
                    _samples.Add(new FaceSample
                    {
                        Label = label,
                        Histogram = ComputeNormalizedHistogram(gray),
                        FilePath = file
                    });
                }
            }
        }

        public Rect[] DetectFaces(Mat bgrFrame)
        {
            using Mat gray = new Mat();
            Cv2.CvtColor(bgrFrame, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);

            return _faceCascade.DetectMultiScale(
                image: gray,
                scaleFactor: 1.15,
                minNeighbors: 5,
                flags: HaarDetectionTypes.ScaleImage,
                minSize: new Size(50, 50)
            );
        }

        public List<FaceDetectionResult> AnalyzeFrame(Mat bgrFrame)
        {
            var results = new List<FaceDetectionResult>();
            Rect[] faces = DetectFaces(bgrFrame);

            using Mat gray = new Mat();
            Cv2.CvtColor(bgrFrame, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);

            foreach (Rect rect in faces)
            {
                using Mat faceRoi = new Mat(gray, rect);
                using Mat resized = new Mat();
                Cv2.Resize(faceRoi, resized, new Size(100, 100));

                var (label, score) = Predict(resized);
                results.Add(new FaceDetectionResult
                {
                    Bounds = rect,
                    PredictedLabel = label,
                    ConfidencePercent = score
                });
            }

            return results;
        }

        private (string Label, double Confidence) Predict(Mat normalizedGrayFace)
        {
            if (_samples.Count == 0)
            {
                return ("Bilinmiyor (Egitim Yok)", 0.0);
            }

            using Mat queryHist = ComputeNormalizedHistogram(normalizedGrayFace);

            string bestLabel = "Bilinmiyor";
            double bestCorrelation = -1.0;

            foreach (var sample in _samples)
            {
                double corr = Cv2.CompareHist(queryHist, sample.Histogram, HistCompMethods.Correl);
                if (corr > bestCorrelation)
                {
                    bestCorrelation = corr;
                    bestLabel = sample.Label;
                }
            }

            double percent = Math.Max(0, Math.Min(100, bestCorrelation * 100.0));
            if (percent < 45.0)
            {
                return ("Taninmayan Kisi", Math.Round(percent, 1));
            }

            return (bestLabel, Math.Round(percent, 1));
        }

        public int AddSamplesFromFrame(Mat bgrFrame, string label)
        {
            string cleanLabel = SanitizeLabel(label);
            Rect[] faces = DetectFaces(bgrFrame);
            if (faces.Length == 0) return 0;

            using Mat gray = new Mat();
            Cv2.CvtColor(bgrFrame, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);

            int added = 0;
            foreach (Rect rect in faces)
            {
                using Mat faceRoi = new Mat(gray, rect);
                using Mat resized = new Mat();
                Cv2.Resize(faceRoi, resized, new Size(100, 100));

                string filePath = Path.Combine(_datasetDir, $"{cleanLabel}_{DateTime.Now:yyyyMMdd_HHmmss_fff}_{added}.png");
                Cv2.ImWrite(filePath, resized);

                _samples.Add(new FaceSample
                {
                    Label = cleanLabel,
                    Histogram = ComputeNormalizedHistogram(resized),
                    FilePath = filePath
                });
                added++;
            }

            return added;
        }

        public int SaveDetectedFaces(Mat bgrFrame)
        {
            Rect[] faces = DetectFaces(bgrFrame);
            int saved = 0;
            foreach (Rect rect in faces)
            {
                using Mat crop = new Mat(bgrFrame, rect);
                string filePath = Path.Combine(_detectedDir, $"Yuz_{DateTime.Now:yyyyMMdd_HHmmss_fff}_{saved}.png");
                Cv2.ImWrite(filePath, crop);
                saved++;
            }
            return saved;
        }

        public void ClearDataset()
        {
            foreach (var s in _samples)
            {
                s.Histogram?.Dispose();
            }
            _samples.Clear();

            if (Directory.Exists(_datasetDir))
            {
                foreach (string file in Directory.GetFiles(_datasetDir, "*.png"))
                {
                    try { File.Delete(file); } catch { }
                }
            }
        }

        private Mat ComputeNormalizedHistogram(Mat gray100x100)
        {
            Mat hist = new Mat();
            int[] histSize = { 64 };
            Rangef[] ranges = { new Rangef(0, 256) };
            Cv2.CalcHist(new[] { gray100x100 }, new[] { 0 }, null, hist, 1, histSize, ranges);
            Cv2.Normalize(hist, hist, 0, 1, NormTypes.MinMax);
            return hist;
        }

        private string SanitizeLabel(string raw)
        {
            string trimmed = string.IsNullOrWhiteSpace(raw) ? "Sinif1" : raw.Trim();
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                trimmed = trimmed.Replace(c, '_');
            }
            return trimmed.Replace('_', '-');
        }

        public void Dispose()
        {
            foreach (var s in _samples)
            {
                s.Histogram?.Dispose();
            }
            _samples.Clear();
            _faceCascade?.Dispose();
        }
    }
}