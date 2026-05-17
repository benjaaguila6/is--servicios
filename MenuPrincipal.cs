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

namespace Servicios
{
    public partial class MenuPrincipal : Form
    {
        UsuarioModelo55CA usuarioActual = ServiceSessionManager55CA.getIntancia().usuarioActivo;

        public MenuPrincipal()
        {
            InitializeComponent();
            label1.Text = $"Bienvenido: {usuarioActual.Nombre}, {usuarioActual.Apellido} !";
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambiarContraseña form = new CambiarContraseña();
            form.ShowDialog();

            this.Hide();
        }

        private void gestionUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionUsuario form = new GestionUsuario();
            form.Show();
        }
    }
}
