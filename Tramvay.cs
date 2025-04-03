using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rota_Planlama_sistemi.Interfaces;

namespace Rota_Planlama_sistemi
{
    public class Tramvay : Arac, IArac
    {
        private decimal _ucretPerKm = 0.40m;
        private double _ortalamaHiz = 40.0;

        public string AracTipi => "Tramvay";

        public override decimal UcretHesapla(decimal mesafe, Yolcu yolcu)
        {
            decimal normalUcret = mesafe * _ucretPerKm;
            return yolcu.IndirimliUcret(normalUcret);
        }

        public override TimeSpan SureHesapla(decimal mesafe)
        {
            double saat = (double)mesafe / _ortalamaHiz;
            return TimeSpan.FromHours(saat);
        }
    }
}
