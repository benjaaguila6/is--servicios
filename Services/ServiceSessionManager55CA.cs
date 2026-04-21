using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace Services
{
    public sealed class ServiceSessionManager55CA
    {
        private ServiceSessionManager55CA() { }

        private static ServiceSessionManager55CA _instancia;

        public BEUsuario55CA usuarioActivo { get; private set; }

        public static ServiceSessionManager55CA getIntancia()
        {
            if( _instancia == null)
            {
                _instancia = new ServiceSessionManager55CA();
            }

            return _instancia;
        }

        public void Login(BEUsuario55CA usuario)
        {
            usuarioActivo = usuario;
        }

        public void Logout()
        {
            usuarioActivo = null;
        }

        public bool estaLogueado()
        {
            return usuarioActivo != null;
        }

    }
}
