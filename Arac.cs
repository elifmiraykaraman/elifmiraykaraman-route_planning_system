using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rota_Planlama_sistemi
{
    public abstract class Arac
    {
        public string Ad { get; set; }

        public abstract decimal UcretHesapla(decimal mesafe, Yolcu yolcu);

        public abstract TimeSpan SureHesapla(decimal mesafe);
    }
}

