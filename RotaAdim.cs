using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi
{
    public class RotaAdim
    {
        public string BaslangicDurak { get; set; }     // Otogar vb.
        public string BitisDurak { get; set; }         // Sekapark vb.
        public string AracTipi { get; set; }           // "Otobüs", "Tramvay", "Yürüyüş", "Transfer" vs.
        public int Sure { get; set; }                  // dakika cinsinden
        public decimal Ucret { get; set; }             // TL cinsinden
                                                       // İsteğe bağlı ekstra alanlar:
        public bool OgrenciIndirimiVar { get; set; }
        public bool OzelGunMu { get; set; }
    }

}
