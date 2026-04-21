using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEUsuario55CA
    {
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Intentos { get; set; }
        public bool Bloqueo { get; set; }
        public bool Activo { get; set; }
    }
}
