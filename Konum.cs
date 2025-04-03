using System;

namespace Rota_Planlama_sistemi
{
    public class Konum
    {
        public double Lat { get; set; }
        public double Lon { get; set; }

        public static double MesafeHesapla(Konum k1, Konum k2)
        {
            double R = 6371; // Dünya yarıçapı, km cinsinden
            double dLat = ToRadians(k2.Lat - k1.Lat);
            double dLon = ToRadians(k2.Lon - k1.Lon);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(k1.Lat)) * Math.Cos(ToRadians(k2.Lat)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
