using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace Rota_Planlama_sistemi
{
    public partial class Form1 : Form
    {
        private Root rootData;
        private RotaHesaplayici rotaCalc; // RotaHesaplayici sınıfınız

        private GMapOverlay markersOverlay;
        private GMapOverlay routeOverlay;

        private List<GMapMarker> allDurakMarkers = new List<GMapMarker>();
        private List<GMapRoute> allRoutes = new List<GMapRoute>();

        private bool selectingStart = true;
        private Durak startDurak = null;
        private Durak targetDurak = null;

        private List<RotaSecenek> guncelSecenekler = new List<RotaSecenek>();

        public Form1()
        {
            InitializeComponent();

            markersOverlay = new GMapOverlay("markers");
            routeOverlay = new GMapOverlay("routes");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                string jsonPath = "VeriSeti.json.txt";
                string jsonContent = File.ReadAllText(jsonPath);
                rootData = JsonSerializer.Deserialize<Root>(jsonContent);

                if (rootData != null && rootData.duraklar != null)
                {
                    // Aynı ID'li durakları ayıkla
                    rootData.duraklar = rootData.duraklar
                        .GroupBy(d => d.id)
                        .Select(g => g.First())
                        .ToList();

                    // RotaHesaplayici
                    rotaCalc = new RotaHesaplayici(
                        rootData.duraklar,
                        rootData.taxi.openingFee,
                        rootData.taxi.costPerKm
                    );

                    // Durakların BagliDuraklar listesini doldur
                    foreach (var durak in rootData.duraklar)
                    {
                        durak.BagliDuraklar = new List<(Durak, double, int, double)>();
                        if (durak.nextStops != null)
                        {
                            foreach (var ns in durak.nextStops)
                            {
                                var hedef = rootData.duraklar.FirstOrDefault(x => x.id == ns.stopId);
                                if (hedef != null)
                                {
                                    durak.BagliDuraklar.Add((hedef, ns.mesafe, ns.sure, ns.ucret));
                                }
                            }
                        }

                        // Transfer durakları
                        if (durak.transfer != null && !string.IsNullOrEmpty(durak.transfer.transferStopId))
                        {
                            var transferDurak = rootData.duraklar.FirstOrDefault(x => x.id == durak.transfer.transferStopId);
                            if (transferDurak != null)
                            {
                                durak.BagliDuraklar.Add((transferDurak, 0.1, durak.transfer.transferSure, durak.transfer.transferUcret));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("JSON okuma hatası: " + ex.Message);
            }

            // GMap ayarları
            gMapControl1.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;

            gMapControl1.Position = new PointLatLng(40.7652, 29.9410);
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 20;
            gMapControl1.Zoom = 12;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;

            gMapControl1.Overlays.Add(markersOverlay);
            gMapControl1.Overlays.Add(routeOverlay);

            // Durak Marker'larını ekle
            if (rootData?.duraklar != null)
            {
                foreach (var durak in rootData.duraklar)
                {
                    var marker = new GMarkerGoogle(
                        new PointLatLng(durak.lat, durak.lon),
                        GMarkerGoogleType.blue_dot
                    );
                    marker.ToolTipText = durak.name;
                    marker.Tag = durak;
                    markersOverlay.Markers.Add(marker);
                    allDurakMarkers.Add(marker);
                }

                // Duraklar arası çizgiler
                foreach (var durak in rootData.duraklar)
                {
                    if (durak.nextStops != null)
                    {
                        foreach (var ns in durak.nextStops)
                        {
                            var hedef = rootData.duraklar.FirstOrDefault(x => x.id == ns.stopId);
                            if (hedef != null)
                            {
                                var points = new List<PointLatLng>()
                                {
                                    new PointLatLng(durak.lat, durak.lon),
                                    new PointLatLng(hedef.lat, hedef.lon)
                                };
                                var route = new GMapRoute(points, durak.id + "->" + hedef.id);
                                route.Stroke = new Pen(Color.Blue, 2);
                                routeOverlay.Routes.Add(route);
                                allRoutes.Add(route);
                            }
                        }
                    }

                    // Transfer bağlantısı
                    if (durak.transfer != null && !string.IsNullOrEmpty(durak.transfer.transferStopId))
                    {
                        var transferDurak = rootData.duraklar.FirstOrDefault(x => x.id == durak.transfer.transferStopId);
                        if (transferDurak != null)
                        {
                            var points = new List<PointLatLng>()
                            {
                                new PointLatLng(durak.lat, durak.lon),
                                new PointLatLng(transferDurak.lat, transferDurak.lon)
                            };
                            var transferRoute = new GMapRoute(points, durak.id + "->" + transferDurak.id);
                            transferRoute.Stroke = new Pen(Color.Orange, 2);
                            routeOverlay.Routes.Add(transferRoute);
                            allRoutes.Add(transferRoute);
                        }
                    }
                }
            }

            // Harita tıklama
            gMapControl1.MouseClick += gMapControl1_MouseClick;

            // ListView ayarları
            lvAlternatifRotalar.View = View.Details;
            lvAlternatifRotalar.FullRowSelect = true;
            lvAlternatifRotalar.Columns.Add("Açıklama", 120);
            lvAlternatifRotalar.Columns.Add("Süre (dk)", 60);
            lvAlternatifRotalar.Columns.Add("Maliyet (TL)", 70);
            lvAlternatifRotalar.Columns.Add("Aktarma", 60);

            lvAlternatifRotalar.SelectedIndexChanged += lvAlternatifRotalar_SelectedIndexChanged;
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bool markerBulundu = false;

                foreach (var marker in markersOverlay.Markers)
                {
                    if (marker is GMarkerGoogle googleMarker &&
                        googleMarker.IsVisible &&
                        googleMarker.IsMouseOver)
                    {
                        markerBulundu = true;
                        var secilenDurak = googleMarker.Tag as Durak;
                        if (secilenDurak != null)
                        {
                            if (selectingStart)
                            {
                                startDurak = secilenDurak;
                                txtBaslangicLat.Text = startDurak.lat.ToString(CultureInfo.InvariantCulture);
                                txtBaslangicLon.Text = startDurak.lon.ToString(CultureInfo.InvariantCulture);
                                MessageBox.Show("Başlangıç seçildi (marker): " + startDurak.name);
                            }
                            else
                            {
                                targetDurak = secilenDurak;
                                txtHedefLat.Text = targetDurak.lat.ToString(CultureInfo.InvariantCulture);
                                txtHedefLon.Text = targetDurak.lon.ToString(CultureInfo.InvariantCulture);
                                MessageBox.Show("Hedef seçildi (marker): " + targetDurak.name);
                            }
                            selectingStart = !selectingStart;
                        }
                        break;
                    }
                }

                if (!markerBulundu)
                {
                    PointLatLng latLng = gMapControl1.FromLocalToLatLng(e.X, e.Y);

                    if (selectingStart)
                    {
                        txtBaslangicLat.Text = latLng.Lat.ToString(CultureInfo.InvariantCulture);
                        txtBaslangicLon.Text = latLng.Lng.ToString(CultureInfo.InvariantCulture);
                        MessageBox.Show("Başlangıç seçildi (boş harita noktası).");
                    }
                    else
                    {
                        txtHedefLat.Text = latLng.Lat.ToString(CultureInfo.InvariantCulture);
                        txtHedefLon.Text = latLng.Lng.ToString(CultureInfo.InvariantCulture);
                        MessageBox.Show("Hedef seçildi (boş harita noktası).");
                    }
                    selectingStart = !selectingStart;
                }
            }
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (rotaCalc == null)
            {
                MessageBox.Show("Rota hesaplama nesnesi yok.");
                return;
            }

            try
            {
                double basLat = double.Parse(txtBaslangicLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                double basLon = double.Parse(txtBaslangicLon.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                double hefLat = double.Parse(txtHedefLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                double hefLon = double.Parse(txtHedefLon.Text.Replace(',', '.'), CultureInfo.InvariantCulture);

                // Yolcu seçimi
                Yolcu secilenYolcu;
                switch (cmbYolcuTipi.SelectedItem.ToString())
                {
                    case "Öğrenci":
                        secilenYolcu = new Ogrenci();
                        break;
                    case "Yaşlı":
                        secilenYolcu = new Yasli();
                        break;
                    default:
                        secilenYolcu = new Genel();
                        break;
                }

                // Ödeme seçimi
                Odeme secilenOdeme;
                switch (cmbOdeme.SelectedItem.ToString())
                {
                    case "Nakit":
                        secilenOdeme = new Nakit();
                        break;
                    case "Kredi Kartı":
                        secilenOdeme = new KrediKarti();
                        break;
                    case "KentKart":
                        secilenOdeme = new KentKart();
                        break;
                    default:
                        secilenOdeme = new Nakit();
                        break;
                }

                var basKonum = new Konum { Lat = basLat, Lon = basLon };
                var hefKonum = new Konum { Lat = hefLat, Lon = hefLon };

                // Tüm senaryoları al
                var tumRotalar = rotaCalc.TumSecenekleriGetir(
                    basKonum,
                    hefKonum,
                    secilenYolcu,
                    secilenOdeme,
                    "Hepsi"
                );

                if (tumRotalar == null || tumRotalar.Count == 0)
                {
                    MessageBox.Show("Hiç rota bulunamadı.");
                    return;
                }

                // ListView'i temizle
                lvAlternatifRotalar.Items.Clear();
                guncelSecenekler.Clear();

                // Rotaları ekle
                foreach (var rota in tumRotalar)
                {
                    var lvi = new ListViewItem(rota.Aciklama);
                    lvi.SubItems.Add(rota.Sure.ToString());
                    lvi.SubItems.Add(rota.Maliyet.ToString("F2"));
                    lvi.SubItems.Add(rota.AktarmaSayisi.ToString());

                    lvi.Tag = rota;
                    lvAlternatifRotalar.Items.Add(lvi);
                    guncelSecenekler.Add(rota);
                }

                // İlk rota seçili olarak detay göster
                var ilkRota = tumRotalar.FirstOrDefault();
                if (ilkRota != null)
                {
                    string adimMetni = OlusturRotaAdimMetniWithNearestStops(ilkRota, basLat, basLon, hefLat, hefLon);
                    rtbSecilenRota.Text = adimMetni;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hesaplama hatası: " + ex.Message);
            }
        }

        private void lvAlternatifRotalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAlternatifRotalar.SelectedItems.Count == 0)
                return;

            var selectedItem = lvAlternatifRotalar.SelectedItems[0];
            var seciliRota = selectedItem.Tag as RotaSecenek;
            if (seciliRota == null) return;

            // Harita üzerindeki rotayı çiz
            routeOverlay.Routes.Clear();
            var route = new GMapRoute(seciliRota.YolKoordinatlari, seciliRota.Aciklama);
            route.Stroke = new Pen(Color.Blue, 3);
            routeOverlay.Routes.Add(route);

            // Harita pozisyonu
            if (seciliRota.YolKoordinatlari.Count > 0)
                gMapControl1.Position = seciliRota.YolKoordinatlari[0];

            // Sağ blok (lstSonuc) özet bilgisi
            lstSonuc.Items.Clear();
            lstSonuc.Items.Add("Seçilen Rota Bilgisi:");
            lstSonuc.Items.Add("Açıklama: " + seciliRota.Aciklama);
            lstSonuc.Items.Add("Süre (dk): " + seciliRota.Sure);
            lstSonuc.Items.Add("Maliyet (TL): " + seciliRota.Maliyet.ToString("F2"));
            lstSonuc.Items.Add("Aktarma: " + seciliRota.AktarmaSayisi);

            // Orta blok (rtbSecilenRota) -> En yakın duraklar + Araç adımları
            double basLat = double.Parse(txtBaslangicLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
            double basLon = double.Parse(txtBaslangicLon.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
            double hefLat = double.Parse(txtHedefLat.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
            double hefLon = double.Parse(txtHedefLon.Text.Replace(',', '.'), CultureInfo.InvariantCulture);

            string adimMetni = OlusturRotaAdimMetniWithNearestStops(seciliRota, basLat, basLon, hefLat, hefLon);
            rtbSecilenRota.Text = adimMetni;
        }

        /// <summary>
        /// Hem rota adımlarını hem de kullanıcının başlangıç/hedef konumuna en yakın durak bilgilerini ekrana basar.
        /// </summary>
        private string OlusturRotaAdimMetniWithNearestStops(RotaSecenek secenek, double userLat, double userLon, double targetLat, double targetLon)
        {
            var sb = new StringBuilder();

            // 1) Kullanıcı konumuna en yakın durağı bul
            var nearestStartStop = FindNearestStop(userLat, userLon, rootData.duraklar);
            if (nearestStartStop != null)
            {
                double distKm = Haversine(userLat, userLon, nearestStartStop.lat, nearestStartStop.lon);
                int walkTime = WalkingTimeInMinutes(distKm);
                sb.AppendLine("📍 Kullanıcı Konumuna En Yakın Durak:");
                sb.AppendLine($"   {nearestStartStop.name} ({distKm:0.00} km)");
                sb.AppendLine($"   🚶 Yürüme Süresi: {walkTime} dk, Ücret: 0 TL");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine("Kullanıcı konumuna yakın durak bulunamadı!");
                sb.AppendLine();
            }

            // 2) Rota araç adımları
            sb.AppendLine("🚌 Rota Araç Adımları:");
            if (secenek.RotaAdimlari == null || secenek.RotaAdimlari.Count == 0)
            {
                sb.AppendLine("   (Bu rota için adım bilgisi yok.)");
            }
            else
            {
                for (int i = 0; i < secenek.RotaAdimlari.Count; i++)
                {
                    var adim = secenek.RotaAdimlari[i];
                    string aracEmoji = GetAracEmoji(adim.AracTipi);

                    sb.AppendLine($"{i + 1}) {adim.BaslangicDurak} → {adim.BitisDurak} ({aracEmoji} {adim.AracTipi})");
                    sb.AppendLine($"   ⏳ Süre: {adim.Sure} dk");
                    sb.AppendLine($"   💰 Ücret: {adim.Ucret:0.##} TL");
                    sb.AppendLine();
                }
            }

            // 3) Kullanıcının hedef konumuna en yakın durağı bul
            var nearestEndStop = FindNearestStop(targetLat, targetLon, rootData.duraklar);
            if (nearestEndStop != null)
            {
                double distKm = Haversine(nearestEndStop.lat, nearestEndStop.lon, targetLat, targetLon);
                int walkTime = WalkingTimeInMinutes(distKm);

                sb.AppendLine("📍 Hedef Konumuna En Yakın Durak:");
                sb.AppendLine($"   {nearestEndStop.name} ({distKm:0.00} km)");
                sb.AppendLine($"   🚶 Yürüme Süresi: {walkTime} dk, Ücret: 0 TL");
            }
            else
            {
                sb.AppendLine("Hedef konumuna yakın durak bulunamadı!");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Belirtilen koordinata en yakın durağı döndürür (Haversine mesafesine göre).
        /// </summary>
        private Durak FindNearestStop(double lat, double lon, List<Durak> durakList)
        {
            Durak nearest = null;
            double minDist = double.MaxValue;

            foreach (var d in durakList)
            {
                double dist = Haversine(lat, lon, d.lat, d.lon);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = d;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Km cinsinden mesafeyi yürüyüş hızına (3 km/saat = 0.05 km/dk) göre dakikaya çevirir.
        /// </summary>
        private int WalkingTimeInMinutes(double distanceKm)
        {
            double walkingSpeedKmPerMin = 0.05; // 3 km/saat
            return (int)Math.Round(distanceKm / walkingSpeedKmPerMin);
        }

        /// <summary>
        /// İki nokta arasındaki mesafeyi (km) hesaplar (Haversine formülü).
        /// </summary>
        private double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Dünya yarıçapı (km)
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return distance;
        }

        /// <summary>
        /// Araç tipine göre emoji döndüren basit yardımcı metod.
        /// </summary>
        private string GetAracEmoji(string aracTipi)
        {
            if (string.IsNullOrEmpty(aracTipi)) return "➡";
            switch (aracTipi.ToLowerInvariant())
            {
                case "otobüs":
                case "otobus":
                    return "🚌";
                case "tramvay":
                    return "🚋";
                case "taksi":
                    return "🚕";
                case "transfer":
                    return "🔁";
                case "yürüyüş":
                case "yuruyus":
                    return "🚶";
                default:
                    return "➡";
            }
        }

        private void btnSecimYap_Click(object sender, EventArgs e)
        {
            // Seçimleri sıfırla
            startDurak = null;
            targetDurak = null;
            selectingStart = true;

            txtBaslangicLat.Text = "";
            txtBaslangicLon.Text = "";
            txtHedefLat.Text = "";
            txtHedefLon.Text = "";

            markersOverlay.Clear();
            routeOverlay.Routes.Clear();

            foreach (var marker in allDurakMarkers)
                markersOverlay.Markers.Add(marker);
            foreach (var r in allRoutes)
                routeOverlay.Routes.Add(r);

            gMapControl1.Position = new PointLatLng(40.7652, 29.9410);
            gMapControl1.Zoom = 12;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Boş
        }
    }
}
