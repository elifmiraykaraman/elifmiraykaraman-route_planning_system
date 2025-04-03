using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Planlama_sistemi.Interfaces
{
    public interface IRotaBulucu
    {
        List<RotaAdim> RotaBul(Durak baslangic, Durak hedef);
    }
}
