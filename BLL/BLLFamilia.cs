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
    internal class BLLFamilia
    {
        DALFamilia _dal = new DALFamilia();
        DALPatente dalPatente = new DALPatente();

        public List<FamiliaModelo55CA> ObtenerTodos()
        {
            DataTable dtFamilias = _dal.obtenerTodos();
            DataTable dtPatentes = dalPatente.obtenerTodos();
            DataTable dtRelFamiliaPatente = _dal.obtenerRelacionesFamiliaPatente();
            DataTable dtRelFamiliaFamilia = _dal.obtenerRelacionesFamiliaFamilia();

            var dictFamilias = MapearFamiliasBase(dtFamilias);
            var dictPatentes = MapearPatentesBase(dtPatentes);

            EnsamblarPatentesEnFamilias(dictFamilias, dictPatentes, dtRelFamiliaPatente);
            EnsamblarFamiliasEnFamilias(dictFamilias, dtRelFamiliaFamilia);

            return dictFamilias.Values.ToList();
        }
        public int CrearFamilia(string nombre)
        {
            DataTable dtFamilia = _dal.obtenerPorNombre(nombre);

            // Si la tabla contiene al menos una fila, significa que ya existe
            if (dtFamilia.Rows.Count > 0)
            {
                throw new Exception($"Ya existe una familia registrada con el nombre '{nombre}'.");
            }

            return _dal.insertarFamilia(nombre);
        }

        public void AsignarPatente(FamiliaModelo55CA familia, PermisoModelo55CA patente)
        {
            var permisosAplanados = familia.obtenerPermisos();

            if (permisosAplanados.Any(p => p.Id == patente.Id))
            {
                throw new Exception($"La familia:{familia.Nombre} ya contiene el permiso {patente.Nombre}.");
            }

            _dal.asignarPatenteAFamilia(patente.Id, familia.Id);
        }

        public void AsignarFamilia(FamiliaModelo55CA familiaPadre, FamiliaModelo55CA familiaHija)
        {
            if (familiaPadre.Id == familiaHija.Id)
            {
                throw new Exception("Una familia no puede asignarse a sí misma como hija.");
            }

            var permisosPadre = familiaPadre.obtenerPermisos();
            var permisosHija = familiaHija.obtenerPermisos();

            foreach (var permiso in permisosHija)
            {
                //si el padre ya tiene un permiso que la hija intenta aportar, hay redundancia
                if (permisosPadre.Any(p => p.Id == permiso.Id))
                {
                    throw new Exception($"La familia {familiaHija.Nombre} posee el permiso {permiso.Nombre} que la familia {familiaPadre.Nombre} ya posee.");
                }
            }

            _dal.asignarFamiliaAFamilia(familiaPadre.Id, familiaHija.Id);
        }

        public void EliminarFamilia(int idFamilia)
        {
            if (_dal.tieneDependencias(idFamilia)) 
            {
                throw new Exception("No se puede eliminar la familia porque está asignada a un Rol o es parte de otra Familia.");
            }

            _dal.eliminarFamilia(idFamilia);
        }

        #region Mapear y Ensamblar
        private Dictionary<int, FamiliaModelo55CA> MapearFamiliasBase(DataTable dt)
        {
            var diccionario = new Dictionary<int, FamiliaModelo55CA>();
            foreach (DataRow row in dt.Rows)
            {
                var familia = new FamiliaModelo55CA
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                };

                diccionario.Add(familia.Id, familia);
            }

            return diccionario;
        }

        private Dictionary<int, PermisoModelo55CA> MapearPatentesBase(DataTable dt)
        {
            var diccionario = new Dictionary<int, PermisoModelo55CA>();

            foreach (DataRow row in dt.Rows)
            {
                var patente = new PermisoModelo55CA
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                };

                diccionario.Add(patente.Id, patente);
            }

            return diccionario;
        }

        private void EnsamblarPatentesEnFamilias(Dictionary<int, FamiliaModelo55CA> familias, Dictionary<int, PermisoModelo55CA> patentes, DataTable dtRelaciones)
        {
            foreach (DataRow row in dtRelaciones.Rows)
            {
                int idFamilia = Convert.ToInt32(row["IdFamilia"]);
                int idPatente = Convert.ToInt32(row["IdPatente"]);

                if (familias.ContainsKey(idFamilia) && patentes.ContainsKey(idPatente))
                {
                    familias[idFamilia].agregarHijos(patentes[idPatente]);
                }
            }
        }

        private void EnsamblarFamiliasEnFamilias(Dictionary<int, FamiliaModelo55CA> familias, DataTable dtRelaciones)
        {
            foreach (DataRow row in dtRelaciones.Rows)
            {
                int idPadre = Convert.ToInt32(row["IdFamiliaPadre"]);
                int idHija = Convert.ToInt32(row["IdFamiliaHija"]);

                if (familias.ContainsKey(idPadre) && familias.ContainsKey(idHija))
                {
                    familias[idPadre].agregarHijos(familias[idHija]);
                }
            }
        }

        #endregion
    }
}
