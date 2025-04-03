
using System.Collections.Generic;

namespace Rota_Planlama_sistemi
{
    // JSON dosyasıyla birebir uyuşacak model.
    // "duraklar" dizisindeki her eleman bu sınıfa deserialize edilir.
    public class Durak
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }   // "bus" veya "tram"
        public double lat { get; set; }
        public double lon { get; set; }
        public bool sonDurak { get; set; }

        public List<NextStop> nextStops { get; set; }
        public Transfer transfer { get; set; }

        // Ek olarak, Koordinat bilgisini "Konum" tipine çevirir.
        public Konum Konum => new Konum { Lat = this.lat, Lon = this.lon };

        // Opsiyonel: Çözülmüş bağlantılar (algoritma içinde doldurulabilir)
        public List<(Durak durak, double mesafe, int sure, double ucret)> BagliDuraklar { get; set; } = new List<(Durak, double, int, double)>();
    }    
}