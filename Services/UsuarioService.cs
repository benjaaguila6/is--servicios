using DAL;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsuarioService
    {
        DALUsuario55CA dal = new DALUsuario55CA();

        public List<UsuarioModelo55CA> obtenerTodos()
        {
            var dt = dal.obtenerTodos();
            List<UsuarioModelo55CA> lista = new List<UsuarioModelo55CA>();

            foreach(DataRow row in dt.Rows)
            {
                lista.Add(MapearUsuario(row));

                return lista;
            }

            return null;
        }

        public void login(string user, string password)
        {

            var usuario = MapearUsuario(dal.obtenerPorUser(user));

            //validaciones
            if (ServiceSessionManager55CA.getIntancia().estaLogueado())
            {
                throw new Exception("Ya existe una sesión activa.");
            }

            if (usuario == null)
            {
                throw new Exception("El usuario no existe.");
            }

            if (usuario.Bloqueo == true)
            {
                throw new Exception("El usuario esta bloqueado por intentos fallidos. Contacte a un administrador.");
            }

            string passwordHash = ServiceSeguridad55CA.Hashear(password);

            if (usuario.Password != passwordHash)
            {
                usuario.Intentos++;

                dal.aumentarIntento(usuario.DNI);

                if (usuario.Intentos >= 4)
                {
                    dal.bloquearUsuario(usuario.DNI);
                    throw new Exception("Su cuenta ha sido bloqueada tras 4 intentos fallidos. Contacte al administrador.");
                }

                throw new Exception($"Contraseña incorrecta. Intento {usuario.Intentos}. Al cuarto intento fallido se bloqueará la cuenta.");
            }

            //login ok
            ServiceSessionManager55CA.getIntancia().Login(usuario);
            dal.reiniciarIntentos(usuario.DNI);

        }

        private UsuarioModelo55CA MapearUsuario(DataRow row)
        {
            if(row == null)
            {
                return null;
            }
            return new UsuarioModelo55CA
            {
                DNI = row["DNI"].ToString(),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Email = row["Email"].ToString(),
                User = row["User"].ToString(),
                Password = row["Password"].ToString(),
                Intentos = Convert.ToInt32(row["Intentos"]),
                Bloqueo = Convert.ToBoolean(row["Bloqueo"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }
    }
}
