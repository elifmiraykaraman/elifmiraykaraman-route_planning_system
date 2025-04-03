using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi.Interfaces
{
    public interface IArac
    {
        string AracTipi { get; }
        decimal UcretHesapla(decimal mesafe, Yolcu yolcu);
        TimeSpan SureHesapla(decimal mesafe);
    }
}
