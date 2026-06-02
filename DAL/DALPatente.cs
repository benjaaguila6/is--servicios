using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALPatente
    {
        DALAcceso55CA _dal = new DALAcceso55CA();

        public DataTable obtenerTodos()
        {
            string query = "SELECT * FROM Patente";

            return _dal.executeDataTable(query);
        }
    }
}
