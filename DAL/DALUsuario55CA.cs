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
        public DataTable obtenerTodos()
        {
            string query = "SELECT * FROM Usuario";

            DataTable dt = acceso.executeDataTable(query);

            return dt;
        }

        public bool obtenerPorDNI(string dni)
        {
            string query = "SELECT * FROM Usuario WHERE DNI = @dni";

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@dni", dni));

            DataTable dt = acceso.executeDataTable(query, p);

            return dt.Rows.Count > 0;
        }

        public bool obtenerPorEmail(string email)
        {
            string query = "SELECT * FROM USUARIO WHERE Email = @email";

            var parametros = new List<SqlParameter> { new SqlParameter("@email", email) };

            DataTable dt = acceso.executeDataTable(query, parametros);

            return dt.Rows.Count > 0;
        }

        public DataRow obtenerPorUser(string user)
        {
            string query = "SELECT * FROM USUARIO WHERE User = @user";
            var parametros = new List<SqlParameter> { new SqlParameter("@user", user) };

            DataTable dt = acceso.executeDataTable(query, parametros);

            return dt;
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
