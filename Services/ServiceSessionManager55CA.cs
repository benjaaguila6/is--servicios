using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Services.Modelos;

namespace Services
{
    public sealed class ServiceSessionManager55CA
    {
        private ServiceSessionManager55CA() { }

        private static ServiceSessionManager55CA _instancia;

        public UsuarioModelo55CA usuarioActivo { get; private set; }

        public static ServiceSessionManager55CA getIntancia()
        {
            if( _instancia == null)
            {
                _instancia = new ServiceSessionManager55CA();
            }

            return _instancia;
        }

        public void Login(UsuarioModelo55CA usuario)
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
