using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public class BitacoraEventosModelo55CA
    {
        public int IdBitacora { get; set; }
        public int DNI { get; set; }
        public string Evento { get; set; }
        public int Criticidad { get; set; }
        public DateTime FechaHora { get; set; }

        //ctor para bd
        public BitacoraEventosModelo55CA(int idBitacora, int dNI, string evento, int criticidad, DateTime fechaHora)
        {
            IdBitacora = idBitacora;
            DNI = dNI;
            Evento = evento;
            Criticidad = criticidad;
            FechaHora = fechaHora;
        }

        //ctor para hacer new
        public BitacoraEventosModelo55CA(int dNI, string evento, int criticidad, DateTime fechaHora)
        {
            DNI = dNI;
            Evento = evento;
            Criticidad = criticidad;
            FechaHora = fechaHora;
        }
    }
}
