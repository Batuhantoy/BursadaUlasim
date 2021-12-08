using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursadaUlaşım
{
    class durak
    {
        public string durakadi;
        public Single enlem;
        public Single boylam;
        public int durakid;

        public durak(string ad, Single E, Single B,int id)
        {
            this.durakadi = ad;
            this.enlem = E;
            this.boylam = B;
            this.durakid = id;
        }

        public override string ToString()
        {
            return durakadi;
        }
    }
}
