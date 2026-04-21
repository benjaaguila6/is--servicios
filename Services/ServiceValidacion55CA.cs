using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceValidacion55CA
    {
        public static bool EsEmailValido(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool EsDNIValido(string dni)
        {
            return Regex.IsMatch(dni, @"^\d{7,8}$");
        }

        public static bool EsVacio(string valor)
        {
            return string.IsNullOrWhiteSpace(valor);
        }
    }
}
