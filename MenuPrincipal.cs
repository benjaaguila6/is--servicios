using Services;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;

namespace Servicios
{
    public partial class MenuPrincipal : Form
    {
        UsuarioModelo55CA usuarioActual = ServiceSessionManager55CA.getIntancia().usuarioActivo;

        public MenuPrincipal()
        {
            InitializeComponent();

            if (usuarioActual.Rol.Id == 2)
            {
                administradorToolStripMenuItem.Enabled = false;
            }

            label1.Text = $"Bienvenido: {usuarioActual.Nombre}, {usuarioActual.Apellido} !";
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambiarContraseña form = new CambiarContraseña();
            form.Show();
        }

        private void gestionUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionUsuario form = new GestionUsuario();
            form.Show();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Confirmar cierre de sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ServiceSessionManager55CA.getIntancia().Logout();

            base.OnFormClosing(e);
        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
        }
    }
}
