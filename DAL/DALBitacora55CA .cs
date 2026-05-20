using BE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALBitacora55CA
    {
        DALAcceso55CA acceso = new DALAcceso55CA();

        public int insertarLog(Dictionary<string, object> datos)
        {
            string query = @"INSERT INTO BitacoraEventos
            (DNI, Evento, Criticidad, Modulo, FechaHora) 
            VALUES (@dni, @evento, @criticidad, @modulo, @fecha)";

            List<SqlParameter> parametros = new List<SqlParameter>();

            foreach (var item in datos)
            {
                parametros.Add(new SqlParameter(item.Key, item.Value));
            }

            int resultado = acceso.executeNonQuery(query, parametros);

            return resultado;
        }

        public DataTable obtenerBitacora(DateTime desde, DateTime hasta)
        {
            string query = @"SELECT B.Id, B.DNI, U.Nombre, U.Apellido, B.Evento, B.Criticidad,
            B.Modulo, B.FechaHora FROM BitacoraEventos B LEFT JOIN Usuario U ON B.DNI = U.DNI
            WHERE B.FechaHora BETWEEN @desde AND @hasta";

            var parametros = new List<SqlParameter>
        {
        new SqlParameter("@desde", desde),
        new SqlParameter("@hasta", hasta)
        };

            return acceso.executeDataTable(query, parametros);
        }
    }
}
