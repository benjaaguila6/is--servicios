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
using Services.Modelos.Idioma;

namespace Servicios
{
    public partial class MenuPrincipal : Form, IIdiomaObserver
    {
        UsuarioModelo55CA usuarioActual = Services_55CA.ServiceSessionManager55CA.getIntancia().usuarioActivo;

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
            Services_55CA.ServiceSessionManager55CA.getIntancia().Logout();

            base.OnFormClosing(e);
        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
        }

        private void bitacoraEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuditoriaBitacora form = new AuditoriaBitacora();
            form.Show();
        }

        public void actualizarIdioma()
        {
            throw new NotImplementedException();
        }

        private void gestionFamiliaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionFamilia form = new GestionFamilia();
            form.Show();
        }

        private void gestionRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionRol form = new GestionRol();
            form.Show();
        }
    }
}
