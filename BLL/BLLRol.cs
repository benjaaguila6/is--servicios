using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLRol
    {
        DALRol _dal = new DALRol();
        public List<Rol55CA> obtenerTodos()
        {
            List<Rol55CA> lista = new List<Rol55CA>();

            DataTable dt = _dal.obtenerTodos();

            foreach(DataRow r in dt.Rows)
            {
                lista.Add(new Rol55CA
                {
                    Id = Convert.ToInt32(r["Id"]),
                    Nombre = r["Nombre"].ToString()
                });
            }

            return lista;
        }

    }
}
