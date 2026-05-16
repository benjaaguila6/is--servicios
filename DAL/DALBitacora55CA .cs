using BE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            string query = @"INSERT INTO BitacoraEventos (DNI, Evento, Criticidad, Modulo) VALUES (@dni, @evento, @criticidad, @modulo)";

            List<SqlParameter> parametros = new List<SqlParameter>();

            foreach (var item in datos)
            {
                parametros.Add(new SqlParameter(item.Key, item.Value));
            }

            int resultado = acceso.executeNonQuery(query, parametros);

            return resultado;
        }
    }
}
