using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi
{
    public class Transfer
    {
        // transfer objesi, hangi durağa transfer yapılacağını, süresini, ek ücretini tutar.
        public string transferStopId { get; set; }
        public int transferSure { get; set; }
        public double transferUcret { get; set; }
    }
}
