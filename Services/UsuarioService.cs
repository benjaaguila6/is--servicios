using BE;
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
        BitacoraEventosService bit = new BitacoraEventosService();

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

        public void CrearUsuario(string dni, string nombre, string apellido, string email, TipoRol55CA rol)
        {

            if (dal.obtenerPorDNI(dni)) //true si existe
            {
                throw new Exception("Ya existe un usuario con ese DNI.");
            }

            string user = GenerarUsuario(nombre, dni);
            string password = GenerarPassword(apellido, dni);
            string passwordHash = ServiceSeguridad55CA.Hashear(password);

            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "@dni", dni },
                { "@nom", nombre },
                { "@ape", apellido },
                { "@mail", email },
                { "@rol", (int)rol }, //lo convertimos en int para que guarde el pk del rol,
                { "@user", user },
                { "@pass", passwordHash }
            }; //diccionario para que el metodo DAL no tenga muchos parametros

            dal.InsertarUsuario(datos);

            bit.RegistrarCreacionUsuario(1, u.User);
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
                Rol = (TipoRol55CA)Convert.ToInt32(row["IdRol"]), //Toma el numero del rol y automaticamente sabe que rol le corresponde
                User = row["User"].ToString(),
                Password = row["Password"].ToString(),
                Intentos = Convert.ToInt32(row["Intentos"]),
                Bloqueo = Convert.ToBoolean(row["Bloqueo"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        #region Credenciales
        public string GenerarUsuario(string nombre, string dni)
        {
            return nombre.Trim().ToLower() + dni;
        }

        public string GenerarPassword(string apellido, string dni)
        {
            return apellido.Trim().ToLower() + dni;
        }
        #endregion Credenciales
    }
}
