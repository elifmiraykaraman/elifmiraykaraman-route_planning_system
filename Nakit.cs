using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rota_Planlama_sistemi
{
    public class Nakit : Odeme
    {
        public override decimal Ode(decimal tutar)
        {
            // Nakit => ekstra komisyon/işlem yok ise direkt tutar
            return tutar;
        }
    }
}


