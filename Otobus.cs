using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rota_Planlama_sistemi.Interfaces;

namespace Rota_Planlama_sistemi
{
    public class Otobus : Arac, IArac
    {
        private decimal _ucretPerKm = 0.50m;
        private double _ortalamaHiz = 30.0;

        public string AracTipi => "Otobüs";

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

