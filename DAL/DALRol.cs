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
    }
}
