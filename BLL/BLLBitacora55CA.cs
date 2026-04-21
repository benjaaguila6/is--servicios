using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBitacora55CA
    {

        DALBitacora55CA dal = new DALBitacora55CA();

        public void RegistrarCreacionUsuario(int idUsuario, string user)
        {
            BEBitacora55CA b = new BEBitacora55CA();

            b.IdUsuario = idUsuario;
            b.Accion = "Se creó el usuario: " + user;
            b.FechaHora = DateTime.Now;

            dal.Registrar(b);
        }

    }
}
