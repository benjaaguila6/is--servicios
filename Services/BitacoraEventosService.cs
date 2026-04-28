using DAL;
using Services.Enum;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
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
                { "@crit", (int)criticidad },
                { "@fecha", DateTime.Now },
                { "@mod", (int)modulo }
            }; //diccionario para que el metodo DAL no tenga muchos parametros

            dal.insertarLog(datos);
        }
    }
}
