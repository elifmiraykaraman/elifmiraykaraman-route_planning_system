using Rota_Planlama_sistemi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi
{
    public class Genel : Yolcu
    {
        // Yolcu sınıfındaki abstract metodu override etmek zorundayız.
        public override decimal IndirimliUcret(decimal normalUcret)
        {
            // Genel yolcuya hiçbir indirim uygulamıyoruz
            return normalUcret;
        }
    }
}
