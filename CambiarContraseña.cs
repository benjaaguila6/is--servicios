using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servicios
{
    public partial class CambiarContraseña : Form
    {
        UsuarioService _usuarioService = new UsuarioService();
        ServiceSessionManager55CA instancia = ServiceSessionManager55CA.getIntancia();
        public CambiarContraseña()
        {
            InitializeComponent();
            txtUser.Text = instancia.usuarioActivo.User;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string passwordNueva = txtNueva.Text;
            string passwordActual = txtContraseñaActual.Text;
            string confirmacion = txtConfirmacion.Text;

            try
            {
                if (passwordNueva != confirmacion)
                {
                    MessageBox.Show("Las contraseñas no coinciden.");
                }
                else
                {
                    if(_usuarioService.cambiarPassword(passwordActual, passwordNueva))
                    {
                        MessageBox.Show("Contraseña modificada con exito.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar contraseña: " + ex.Message);
                return;
            }
        }
    }
}
