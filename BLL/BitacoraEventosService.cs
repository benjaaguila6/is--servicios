using DAL;
using BE.Enum;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BitacoraEventosService
    {
        DALBitacora55CA dal = new DALBitacora55CA();
        public void registrarEvento(string dni, string evento, Criticidad55CA criticidad, Modulos55CA modulo)
        {

            Dictionary<string, object> datos = new Dictionary<string, object>
            {   
            { "@dni", dni },
            { "@evento", evento },
            { "@criticidad", (int)criticidad },
            { "@modulo", (int)modulo },
            { "@fecha", DateTime.Now }
            };  

            dal.insertarLog(datos);
        }


        public DataTable obtenerUltimos3Dias()
        {
            DateTime desde = DateTime.Now.AddDays(-3);
            DateTime hasta = DateTime.Now;

            return dal.obtenerBitacora(desde, hasta);
        }

        public DataTable obtenerBitacora(DateTime desde, DateTime hasta)
        {
            return dal.obtenerBitacora(desde, hasta);
        }
    }
}
