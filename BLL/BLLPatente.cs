using DAL;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPatente
    {
        DALPatente dal = new DALPatente();
        public List<PermisoModelo55CA> obtenerTodos()
        {
            DataTable dt = dal.obtenerTodos();

            List<PermisoModelo55CA> lista = new List<PermisoModelo55CA>();

            foreach (DataRow row in dt.Rows)
            {
                var patente = new PermisoModelo55CA
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                };

                lista.Add(patente);
            }

            return lista;
        }

        private long CalcularDVHDeFila(DataRow row)
        {
            string cadena = row["Id"].ToString() + row["Nombre"].ToString();
            return Services.DigitoVerificador55CA.CalcularDVH(cadena);
        }

        public long ObtenerSumaDVH()
        {
            DataTable dt = dal.obtenerTodos();
            long suma = 0;

            foreach (DataRow row in dt.Rows)
            {
                suma += CalcularDVHDeFila(row);
            }

            return suma;
        }

        public void RepararTodo()
        {
            DataTable dt = dal.obtenerTodos();

            foreach (DataRow row in dt.Rows)
            {
                long dvhGuardado = row["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(row["DVH"]);
                long dvhCalculado = CalcularDVHDeFila(row);

                if (dvhGuardado != dvhCalculado)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    dal.ActualizarDVH(id, dvhCalculado);
                }
            }
        }
    }
}
