using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi
{
    // nextStops listesinde duran veriyi temsil eder.
    public class NextStop
    {
        public string stopId { get; set; }
        public double mesafe { get; set; }  // km
        public int sure { get; set; }       // dakika
        public double ucret { get; set; }   // toplu taşıma ücreti (otobüs/tramvay)
    }
}
