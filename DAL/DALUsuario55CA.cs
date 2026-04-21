using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Microsoft.SqlServer.Server;

namespace DAL
{
    public class DALUsuario55CA
    {
        DALAcceso55CA acceso = new DALAcceso55CA();

        public int InsertarUsuario(BEUsuario55CA u)
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

            int resultado = acceso.executeNonQuery(query, parametros);

            return resultado;
        }

        #region ObtenerUsuarios
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

        public bool obtenerPorDNI(string dni)
        {
            string query = "SELECT * FROM Usuario WHERE DNI = @dni";

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@dni", dni));

            DataTable dt = acceso.executeDataTable(query, p);

            return dt.Rows.Count > 0;
        }

        public BEUsuario55CA obtenerPorEmail(string email)
        {
            string query = "SELECT * FROM USUARIO WHERE Email = @email";
            var parametros = new List<SqlParameter> { new SqlParameter("@email", email) };

            DataTable dt = acceso.executeDataTable(query, parametros);

            if(dt.Rows.Count > 0) // si hay un usuario con ese email
            {
                DataRow row = dt.Rows[0]; //agarramos el primero (deberia haber uno solo)

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

                return u;
            }

            return null;
        }

        public BEUsuario55CA obtenerPorUser(string user)
        {
            string query = "SELECT * FROM USUARIO WHERE User = @user";
            var parametros = new List<SqlParameter> { new SqlParameter("@user", user) };

            DataTable dt = acceso.executeDataTable(query, parametros);

            if (dt.Rows.Count > 0) // si hay un usuario con ese user
            {
                DataRow row = dt.Rows[0]; //agarramos el primero (deberia haber uno solo)

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

                return u;
            }

            return null;
        }

        #endregion ObtenerUsuarios

        #region IntentosFallidos

        public int aumentarIntento(string dni)
        {
            string query = "UPDATE Usuarios SET Intentos = Intentos + 1 WHERE DNI = @dni";
            var parametros = new List<SqlParameter> { new SqlParameter ("@dni", dni) };

            int resultado = acceso.executeNonQuery(query, parametros);

            return resultado;
        }

        public int reiniciarIntentos(string dni)
        {
            string query = "UPDATE Usuarios SET Intentos = 0 WHERE DNI = @dni";
            var parametros = new List<SqlParameter> { new SqlParameter("@dni", dni) };

            int resultado = acceso.executeNonQuery(query, parametros);

            return resultado;
        }

        public int bloquearUsuario(string dni)
        {
            string query = "UPDATE Usuarios SET Bloqueo = 1 WHERE DNI = @dni";
            var parametros = new List<SqlParameter> { new SqlParameter("@dni", dni) };

            int resultado = acceso.executeNonQuery(query, parametros);

            return resultado;
        }

        #endregion IntentosFallidos

    }
}
