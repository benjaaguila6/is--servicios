using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using DAL;
using BE;

namespace BLL
{
    public class BLLUsuario55CA
    {
        public DALUsuario55CA dal = new DALUsuario55CA();
        BLLBitacora55CA bit = new BLLBitacora55CA();
        public void CrearUsuario(BEUsuario55CA u)
        {

            if (ServiceValidacion55CA.EsVacio(u.Nombre) ||
                ServiceValidacion55CA.EsVacio(u.Apellido) ||
                ServiceValidacion55CA.EsVacio(u.DNI) ||
                ServiceValidacion55CA.EsVacio(u.Email))
            {
                throw new Exception("Debe completar todos los campos obligatorios.");
            }

            if (!ServiceValidacion55CA.EsDNIValido(u.DNI))
            {
                throw new Exception("El DNI ingresado no es válido.");
            }
                

            if (!ServiceValidacion55CA.EsEmailValido(u.Email))
            {
                throw new Exception("El email ingresado no es válido.");
            }
                

            if (dal.obtenerPorDNI(u.DNI))
            {
                throw new Exception("Ya existe un usuario con ese DNI.");
            }
               
            u.User = ServiceCredenciales55CA.GenerarUsuario(u.Nombre, u.DNI);
            u.Password = ServiceCredenciales55CA.GenerarPassword(u.Apellido, u.DNI);
            u.Password = ServiceSeguridad55CA.Hashear(u.Password);


            dal.InsertarUsuario(u);

            bit.RegistrarCreacionUsuario(1, u.User);
        }

        public void login(string user, string password)
        {
           
            BEUsuario55CA usuario = dal.obtenerPorUser(user);
            
            //validaciones
            if (ServiceSessionManager55CA.getIntancia().estaLogueado())
            {
                throw new Exception("Ya existe una sesión activa.");
            }

            if(usuario == null)
            {
                throw new Exception("El usuario no existe.");
            }

            if(usuario.Bloqueo == true)
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
            
            //login exitoso
            ServiceSessionManager55CA.getIntancia().Login(usuario);
            dal.reiniciarIntentos(usuario.DNI);

        }

        public List<BEUsuario55CA> obtenerTodos()
        {
            return dal.obtenerTodos();
        }
    }
}
