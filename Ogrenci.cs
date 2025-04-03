using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi
{
    public class Ogrenci : Yolcu
    {
        public override decimal IndirimliUcret(decimal normalUcret)
        {
            // Öğrenci örnek: %50 indirim
            return normalUcret * 0.50m;
        }
    }
}
