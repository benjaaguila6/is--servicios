using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class DigitoVerificador55CA
    {
        public static long CalcularDVH(string cadena)
        {
            long suma = 0;

            foreach (char c in cadena)
            {
                suma += c;
            }

            return suma;
        }

        public static long CalcularDVV(IEnumerable<long> dvhs)
        {
            long suma = 0;
            foreach (long dvh in dvhs)
            {
                suma += dvh;
            }
            return suma;
        }
    }
}
