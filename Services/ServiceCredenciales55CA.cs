using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceCredenciales55CA
    {
        public static string GenerarUsuario(string nombre, string dni)
        {
            return nombre.Trim().ToLower() + dni.Substring(dni.Length - 3);
        }

        public static string GenerarPassword(string apellido, string dni)
        {
            return apellido.Trim().ToLower() + dni.Substring(dni.Length - 3);
        }
    }
}
