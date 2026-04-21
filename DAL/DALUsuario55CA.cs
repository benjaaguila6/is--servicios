using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class DALUsuario55CA
    {
        DALAcceso55CA acceso = new DALAcceso55CA();

        public bool ExisteDNI(string dni)
        {
            string query = "SELECT * FROM Usuario WHERE DNI = @dni";

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@dni", dni));

            DataTable dt = acceso.executeDataTable(query, p);

            return dt.Rows.Count > 0;
        }

        public void InsertarUsuario(BEUsuario55CA u)
        {
            string query = @"INSERT INTO Usuario
                    (DNI, Nombre, Apellido, Email, Rol, User, Password)
                    VALUES
                    (@dni, @nom, @ape, @mail, @rol, @user, @pass)";

            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("@dni", u.DNI));
            parametros.Add(new SqlParameter("@nom", u.Nombre));
            parametros.Add(new SqlParameter("@ape", u.Apellido));
            parametros.Add(new SqlParameter("@mail", u.Email));
            parametros.Add(new SqlParameter("@rol", u.Rol));
            parametros.Add(new SqlParameter("@user", u.User));
            parametros.Add(new SqlParameter("@pass", u.Password));

            acceso.executeNonQuery(query, parametros);
        }

        public List<BEUsuario55CA> obtenerTodos()
        {
            string query = "SELECT * FROM Usuario";

            List<BEUsuario55CA> lista = new List<BEUsuario55CA>();

            DataTable dt = acceso.executeDataTable(query);

            if(dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    BEUsuario55CA u = new BEUsuario55CA
                    {
                        DNI = row["DNI"].ToString(),
                        Nombre = row["Nombre"].ToString(),
                        Apellido = row["Apellido"].ToString(),
                        Email = row["Email"].ToString(),
                        User = row["User"].ToString(),
                        Password = row["Password"].ToString(),
                        Intentos = Convert.ToInt32(row["Intentos"]),
                        Bloqueo = Convert.ToBoolean(row["Bloqueo"]),
                        Activo = Convert.ToBoolean(row["Activo"])
                    };
                    lista.Add(u);
                }
                return lista;
            }
            return null;
        }
    }
}
