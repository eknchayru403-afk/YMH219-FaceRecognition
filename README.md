# YMH 219 - OpenCV ile Yüz Algılama ve Tanıma Uygulaması

* **Öğrenci Adı Soyadı:** Hayrunnisa Ekinci
* **Öğrenci Numarası:** 240541043
* **Ders:** YMH 219 - Görsel Programlama / Nesne Yönelimli Programlama

## Proje Hakkında

Bu proje, C# Windows Forms (.NET 10.0) ve **OpenCvSharp4** kütüphanesi kullanılarak geliştirilmiş; canlı kamera akışı veya yerel görüntü dosyaları üzerinden yüz algılama yapan, yerel örnek kümesiyle etiketli yüz tanıma ve güven skoru tahmini gerçekleştiren nesne yönelimli bir masaüstü uygulamasıdır.

## Temel Özellikler & OOP Mimarisi

* **Yüz Algılama ve Tanıma Servisi (`FaceRecognitionService` Sınıfı):** `CascadeClassifier` (`haarcascade_frontalface_default.xml`) ile gri tonlamalı ve histogram eşitlenmiş görüntüler üzerinde çoklu yüz tespiti yapar; 100x100 normalize edilmiş yüz histogramları üzerinden korelasyon (`HistCompMethods.Correl`) hesaplayarak sınıf tahmini ve güven skoru (`%`) üretir.
* **Veri Modelleri (`FaceSample` ve `FaceDetectionResult` Sınıfları):** Eğitim kümesindeki her yüz örneğinin etiketi, histogram matrisi ve dosya yolu ile tespit edilen yüzlerin dikdörtgen koordinatları nesne tabanlı olarak kapsüllenmiştir.
* **Canlı Kamera & Dosya İşleme:** `VideoCapture` ile asenkron kamera akışı (`Kamera Başlat` / `Kamera Durdur`) ve `OpenFileDialog` ile yerel resim dosyaları üzerinden anlık yüz işaretleme desteklenir.
* **Yerel Eğitim Kümesi & Yüz Kaydetme:** Kullanıcı arayüzünden farklı kişi/sınıf etiketleriyle kamera karesinden veya fotoğraftan yeni örnekler eklenebilir; algılanan yüz bölgeleri tek tıkla kırpılarak ayrı klasöre kaydedilebilir.

## Kapsam ve Güvenlik Sınırı

Bu uygulama yalnızca **YMH 219** dersi kapsamında yerel ve gönüllü örnekler üzerinde eğitim amaçlı geliştirilmiştir; kamuya açık alanlarda izinsiz kimlik takibi veya gözetim amacıyla kullanılmamaktadır.

## Bilinen Sınırlamalar ve Geliştirme Notları

* Haar Cascade algoritması ve histogram korelasyonu tabanlı tanıma yöntemi; ortam ışığındaki ani değişimlerden ve aşırı yan profil açılarından etkilenebilir.
* Uygulama yalnızca ders kapsamında yerel ve gönüllü örnekler üzerinde çalışacak şekilde tasarlanmıştır.
* İlerleyen sürümlerde derin öğrenme tabanlı yüz gömme (face embedding) modelleri entegre edilerek tanıma doğruluğu artırılabilir.

## Kurulum ve Çalıştırma

1. Depoyu bilgisayarınıza klonlayın:
   ```bash
   git clone [https://github.com/eknchayru403-afk/YMH219-FaceRecognition.git](https://github.com/eknchayru403-afk/YMH219-FaceRecognition.git)
