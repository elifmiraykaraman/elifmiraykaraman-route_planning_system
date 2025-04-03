using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rota_Planlama_sistemi
{
    // Soyutlama: Odeme => alt sınıflardan türetilir (Nakit, KrediKarti, KentKart vs.)
    // Polimorfizm: Ode(...) metodu her alt sınıfta farklı uygulanır.
    public abstract class Odeme
    {
        // Artık "bool" yerine "decimal" döndürüyoruz.
        // "decimal" => ödenecek nihai tutar veya ek masraf/indirimli tutar.
        // Exception fırlatarak hata durumunu yönetebilirsiniz (ör. bakiye yetersiz).
        public abstract decimal Ode(decimal tutar);
    }
}
