using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi
{
    public class Yasli : Yolcu
    {
        public override decimal IndirimliUcret(decimal normalUcret)
        {
            // Yaşlı örnek: %75 indirim (normal ücretin %25'ini öder)
            return normalUcret * 0.25m;
        }
    }
}
