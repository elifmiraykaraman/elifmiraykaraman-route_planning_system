using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rota_Planlama_sistemi
{
    public class KrediKarti : Odeme
    {
        public override decimal Ode(decimal tutar)
        {
            // Örnek: Kredi kartında %2 komisyon
            decimal komisyon = tutar * 0.02m;
            return tutar + komisyon;
        }
    }
}
