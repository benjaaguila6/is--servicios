using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public class PermisoModelo55CA : Componente55CA
    {
        public override List<Componente55CA> obtenerHijos()
        {
            return new List<Componente55CA>(); // Devuelvo una lista vacia. 
        }
    }
}
