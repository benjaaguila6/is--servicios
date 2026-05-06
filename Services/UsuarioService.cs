using BE;
using DAL;
using Services.Enum;
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

              

                if (usuario.Intentos >= 4)
                {
                    dal.bloquearUsuario(usuario.DNI);
                    bit.registrarEvento(usuario.DNI, $"Usuario: {usuario.User} bloqueado.", Criticidad55CA.Alto, Modulos55CA.Seguridad);
                    throw new Exception("Su cuenta ha sido bloqueada tras 4 intentos fallidos. Contacte al administrador.");
                }

                throw new Exception($"Contraseña incorrecta. Intento {usuario.Intentos}. Al cuarto intento fallido se bloqueará la cuenta.");
            }

            //login ok
            ServiceSessionManager55CA.getIntancia().Login(usuario);
            bit.registrarEvento(usuario.DNI, $"Realizo login exitoso.", Criticidad55CA.Alto, Modulos55CA.Usuario);
            dal.reiniciarIntentos(usuario.DNI);

        }

        public void CrearUsuario(string dni, string nombre, string apellido, string email, TipoRol55CA rol)
        {

            if (dal.obtenerPorDNI(dni) != null)
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

            string dniAutor = ServiceSessionManager55CA.getIntancia().usuarioActivo.DNI;

            bit.registrarEvento(dniAutor, "Se creo un usuario nuevo", Criticidad55CA.Medio, Modulos55CA.Usuario);
        }

        public void activarDesactivar(string dni)
        {
            List<UsuarioModelo55CA> todosLosUsuarios = obtenerTodos();
 
            UsuarioModelo55CA usuario = todosLosUsuarios.FirstOrDefault(u => u.DNI == dni);

            string evento = "";

            if (usuario.Activo == true)
            {
                dal.DesactivarUsuario(dni);
                evento = $"Se desactivó la cuenta del usuario: {usuario.User}";
            }
            else
            {
                dal.ActivarUsuario(dni);
                evento = $"Se activó la cuenta del usuario: {usuario.User}";
            }

            string dniAutor = ServiceSessionManager55CA.getIntancia().usuarioActivo.DNI;

            bit.registrarEvento(dniAutor, evento, Criticidad55CA.Alto, Modulos55CA.Usuario);
        }

        public void ModificarUsuario(string dni, string email, TipoRol55CA rol)
        {
            dal.ModificarUsuario(dni, email, (int)rol);

            string dniAutor = ServiceSessionManager55CA.getIntancia().usuarioActivo.DNI;

            bit.registrarEvento(
            dniAutor,
            $"Se modificó usuario DNI {dni}",
            Criticidad55CA.Medio,
            Modulos55CA.Usuario
            );
        }

        public void cambiarPassword(string passwordActual, string passwordNueva)
        {
            string passwordActualHash = ServiceSeguridad55CA.Hashear(passwordActual);

            UsuarioModelo55CA usuarioActivo = ServiceSessionManager55CA.getIntancia().usuarioActivo;

            if(passwordActualHash != usuarioActivo.Password)
            {
                throw new Exception("La contraseña actual es incorrecta.");
            }

            string passwordNuevaHash = ServiceSeguridad55CA.Hashear(passwordNueva);

            if(passwordActualHash == passwordNuevaHash)
            {
                throw new Exception("La contraesña nueva no puede ser igual a la actual.");
            }

            
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

        public void DesbloquearUsuario(string dni)
        {
            var row = dal.obtenerPorDNI(dni);
            var usuario = MapearUsuario(row);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (!usuario.Bloqueo)
                throw new Exception("El usuario no está bloqueado.");

            // password default
            string nuevaPass = GenerarPassword(usuario.Apellido, usuario.DNI);
            string nuevaPassHash = ServiceSeguridad55CA.Hashear(nuevaPass);

            // desbloqueo
            dal.desbloquearUsuario(dni, nuevaPassHash);

            // bitácora
            string dniAutor = ServiceSessionManager55CA.getIntancia().usuarioActivo.DNI;

            bit.registrarEvento(
                dniAutor,
                $"Se desbloqueó el usuario: {usuario.User}",
                Criticidad55CA.Alto,
                Modulos55CA.Usuario
            );
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
