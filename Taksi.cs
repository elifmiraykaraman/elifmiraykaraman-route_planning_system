using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rota_Planlama_sistemi.Interfaces;

namespace Rota_Planlama_sistemi

{
    public class Taksi : Arac, IArac
    {
        public decimal AcilisUcreti { get; set; } = 10m;  // Örnek: 10 TL
        public decimal KmBasiUcret { get; set; } = 4m;    // Örnek: 4 TL

        private double _ortalamaHiz = 50.0;

        public string AracTipi => "Taksi";

        public override decimal UcretHesapla(decimal mesafe, Yolcu yolcu)
        {
            decimal normalUcret = AcilisUcreti + (KmBasiUcret * mesafe);
            return yolcu.IndirimliUcret(normalUcret);
        }

        public override TimeSpan SureHesapla(decimal mesafe)
        {
            double saat = (double)mesafe / _ortalamaHiz;
            return TimeSpan.FromHours(saat);
        }
    }
}
