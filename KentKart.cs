using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rota_Planlama_sistemi
{
    public class KentKart : Odeme
    {
        // Örnek bakiye
        public decimal Bakiye { get; set; } = 50m;

        public override decimal Ode(decimal tutar)
        {
            if (Bakiye < tutar)
                throw new Exception("KentKart bakiyesi yetersiz!");

            Bakiye -= tutar;
            // İsterseniz "taksit" veya "indirim" gibi ek mantık ekleyebilirsiniz
            return tutar;
        }
    }
}
