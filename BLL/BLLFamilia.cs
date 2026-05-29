using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    internal class BLLFamilia
    {
        // Las validaciones que debe tener son:
        //No permitir familias duplicadas
        //No permitir asignar una patente repetida
        //No permitir asignar una familia repetida
        //No permitir asignarse a sí misma o que ni aparezca
        //No permitir permisos duplicados indirectos, o sea, si la familia que quiero agregar tiene un permiso que familia ya tiene, no se deberia de duplicar
        //No poder eliminar si la familia tiene dependencias (Hay un metodo en la DAL que verifica si tiene alguna dependencia)
        //creo que con el metodo obtenerPermisos podes hacer bastantes verificaciones.
    }
}
