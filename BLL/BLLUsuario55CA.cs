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
                throw new Exception("El DNI ingresado no es válido.");

            if (!ServiceValidacion55CA.EsEmailValido(u.Email))
                throw new Exception("El email ingresado no es válido.");

            if (dal.ExisteDNI(u.DNI))
                throw new Exception("Ya existe un usuario con ese DNI.");



            u.User = ServiceCredenciales55CA.GenerarUsuario(u.Nombre, u.DNI);
            u.Password = ServiceCredenciales55CA.GenerarPassword(u.Apellido, u.DNI);
            u.Password = ServiceSeguridad55CA.Hashear(u.Password);



            dal.InsertarUsuario(u);
        }

        public List<BEUsuario55CA> obtenerTodos()
        {
            return dal.obtenerTodos();
        }
    }
}
