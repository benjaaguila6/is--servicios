using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALBitacora55CA
    {
        DALAcceso55CA acceso = new DALAcceso55CA();

        public void Registrar(BEBitacora55CA b)
        {
            string query = @"INSERT INTO Bitacora
                            (IdUsuario, Accion, FechaHora)
                            VALUES
                            (@id, @accion, @fecha)";

            List<SqlParameter> p = new List<SqlParameter>();

            p.Add(new SqlParameter("@id", b.IdUsuario));
            p.Add(new SqlParameter("@accion", b.Accion));
            p.Add(new SqlParameter("@fecha", b.FechaHora));

            acceso.executeNonQuery(query, p);
        }
    }
}
