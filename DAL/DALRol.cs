using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALRol
    {
        DALAcceso55CA _dal = new DALAcceso55CA();

        public DataTable obtenerTodos()
        {
            string query = "SELECT * FROM Rol";

            DataTable dt = _dal.executeDataTable(query);

            return dt;
        }

        public int asignarFamiliaARol(int idFamilia, int idRol)
        {
            string query = "INSERT INTO Rol_Familia (IdRol, IdFamilia) VALUES (@idRol, @idFamilia)";

            var parametros = new Dictionary<string, object>
            {
                {"@idRol", idRol },
                {"@idFamilia", idFamilia }
            };

            int resultado = _dal.executeNonQuery(query, parametros);

            return resultado;
        }

        public int asignarPatenteARol(int idPatente, int idRol)
        {
            string query = "INSERT INTO Rol_Patente (IdRol, IdPatente) VALUES (@idRol, @idPatente)";

            var parametros = new Dictionary<string, object>
            {
                {"@idRol", idRol },
                {"@idFamilia", idPatente }
            };

            int resultado = _dal.executeNonQuery(query, parametros);

            return resultado;
        }

        public int insertarRol(string nombre)
        {
            string query = "INSERT INTO Rol (Nombre) VALUES (@nombre)";

            var parametros = new Dictionary<string, object>
            {
                {"@nombre", nombre }
            };

            int resultado = _dal.executeNonQuery(query, parametros);

            return resultado;
        }
    }
}
