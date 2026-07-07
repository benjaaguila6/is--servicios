using BE.Enum;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLDigitoVerificador
    {
        DALDigitoVerificador55CA dal = new DALDigitoVerificador55CA();
        BitacoraEventosService bit = new BitacoraEventosService();
        DAL.DALBackUpRestore55CA dalBackup = new DAL.DALBackUpRestore55CA();

        UsuarioService usuarioService = new UsuarioService();
        BLLRol rolService = new BLLRol();
        BLLFamilia familiaService = new BLLFamilia();
        BLLPatente patenteService = new BLLPatente();

        private long CalcularDVHControl(string nombreTabla, long dvv)
        {
            string cadena = nombreTabla + dvv;
            return Services.DigitoVerificador55CA.CalcularDVH(cadena);
        }

        private void GuardarOActualizarDVV(string nombreTabla, long dvv)
        {
            long dvh = CalcularDVHControl(nombreTabla, dvv);

            if (dal.ExisteTabla(nombreTabla))
                dal.ActualizarDVV(nombreTabla, dvv, dvh);
            else
                dal.GuardarDVV(nombreTabla, dvv, dvh);
        }

        private bool VerificarTabla(string nombreTabla, Func<long> obtenerSuma, Action reparar)
        {
            DataRow filaControl = dal.ObtenerFila(nombreTabla);

            if (filaControl == null)
            {
                reparar(); // completa los DVH que estén NULL o mal calculados
                long dvvInicial = obtenerSuma();
                GuardarOActualizarDVV(nombreTabla, dvvInicial);
                return true;
            }

            long dvvGuardado = Convert.ToInt64(filaControl["DVV"]);
            long dvhGuardado = Convert.ToInt64(filaControl["DVH"]);

            long dvhRecalculado = CalcularDVHControl(nombreTabla, dvvGuardado);
            if (dvhGuardado != dvhRecalculado)
            {
                RegistrarDeteccion(nombreTabla);
                return false;
            }

            long dvvActual = obtenerSuma();
            bool consistente = dvvGuardado == dvvActual;
            if (!consistente)
            {
                RegistrarDeteccion(nombreTabla);
            }

            return consistente;
        }


        private void RegistrarDeteccion(string tabla)
        {
            string dniAutor = Services_55CA.ServiceSessionManager55CA.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Se detectó una inconsistencia en la tabla {tabla}.", Criticidad55CA.Alto, Modulos55CA.Seguridad);
        }


        public bool VerificarUsuario()
        {
            return VerificarTabla("Usuario", usuarioService.ObtenerSumaDVHUsuario, usuarioService.RepararTodoUsuario);
        }

        public bool VerificarRol()
        {
            return VerificarTabla("Rol", rolService.ObtenerSumaDVH, rolService.RepararTodo);
        }

        public bool VerificarFamilia()
        {
            return VerificarTabla("Familia", familiaService.ObtenerSumaDVH, familiaService.RepararTodo);
        }

        public bool VerificarPatente()
        {
            return VerificarTabla("Patente", patenteService.ObtenerSumaDVH, patenteService.RepararTodo);
        }

        public void RepararUsuario()
        {
            usuarioService.RepararTodoUsuario();
            GuardarOActualizarDVV("Usuario", usuarioService.ObtenerSumaDVHUsuario());
            Registrar("Usuario");
        }

        public void RepararRol()
        {
            rolService.RepararTodo();
            GuardarOActualizarDVV("Rol", rolService.ObtenerSumaDVH());
            Registrar("Rol");
        }


        public void RepararFamilia()
        {
            familiaService.RepararTodo();
            GuardarOActualizarDVV("Familia", familiaService.ObtenerSumaDVH());
            Registrar("Familia");
        }

        public void RepararPatente()
        {
            patenteService.RepararTodo();
            GuardarOActualizarDVV("Patente", patenteService.ObtenerSumaDVH());
            Registrar("Patente");
        }

        private void Registrar(string tabla)
        {
            string dniAutor = Services_55CA.ServiceSessionManager55CA.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Se reparó la tabla {tabla}.", Criticidad55CA.Alto, Modulos55CA.Seguridad);
        }

        public void RealizarRestore(string ruta)
        {
            dalBackup.realizarRestore(ruta);
        }

    }
}
