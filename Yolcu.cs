using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rota_Planlama_sistemi
{
    public abstract class Yolcu
    {
        private string _ad;
        public string Ad
        {
            get { return _ad; }
            set { _ad = value; }
        }

        public abstract decimal IndirimliUcret(decimal normalUcret);
    }
}
