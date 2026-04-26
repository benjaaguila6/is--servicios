using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public class UsuarioModelo55CA
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


        public UsuarioModelo55CA(string dNI, string nombre, string apellido, string email, string rol, string user, string password, int intentos, bool bloqueo, bool activo)
        {
            DNI = dNI;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Rol = rol;
            User = user;
            Password = password;
            Intentos = intentos;
            Bloqueo = bloqueo;
            Activo = activo;
        }

        //ctor para new
        public UsuarioModelo55CA(string dNI, string nombre, string apellido, string email, string rol)
        {
            DNI = dNI;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Rol = rol;
        }
    }
}
