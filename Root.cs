using System.Collections.Generic;

namespace Rota_Planlama_sistemi
{
    public class Root
    {
        public string city { get; set; }
        public TaxiInfo taxi { get; set; }
        public List<Durak> duraklar { get; set; }
    }

   
}
