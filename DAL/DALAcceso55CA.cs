using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALAcceso55CA
    {
        private readonly string _stringConnection = "";

        public DataTable executeDataTable(string query, List<SqlParameter> parametros = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_stringConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {
                    if(parametros != null)
                    {
                        cmd.Parameters.AddRange(parametros.ToArray());  
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    try
                    {
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Error de lectura en la base de datos", ex);
                    }
                }
            }
            return dt;
        }

        public int executeNonQuery(string consulta, List<SqlParameter> parametros)
        {
            using (SqlConnection conn = new SqlConnection(_stringConnection))
            {
                using (SqlCommand cmd = new SqlCommand(consulta, conn))
                {
                    if (parametros != null)
                    {
                        cmd.Parameters.AddRange(parametros.ToArray());
                    }
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Error al escribir en la base de datos", ex);
                    }
                }
            }
        }
    }
}
