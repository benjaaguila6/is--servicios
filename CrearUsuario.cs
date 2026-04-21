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
using BLL;

namespace Servicios
{
    public partial class CrearUsuario : Form
    {
        public CrearUsuario()
        {
            InitializeComponent();
        }

        private void CrearUsuario_Load(object sender, EventArgs e)
        {
            cmbRol.Items.Add("Rol1");
            cmbRol.Items.Add("RRol2");


            cmbRol.SelectedIndex = 0;
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                BEUsuario55CA u = new BEUsuario55CA();

                u.Nombre = txtNombre.Text;
                u.Apellido = txtApellido.Text;
                u.DNI = txtDNI.Text;
                u.Email = txtEmail.Text;
                u.Rol = cmbRol.Text;

                BLLUsuario55CA bll = new BLLUsuario55CA();

                bll.CrearUsuario(u);

                MessageBox.Show("Usuario creado correctamente");
                CargarGrilla();

                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDNI.Clear();
            txtEmail.Clear();

            cmbRol.SelectedIndex = 0;
            txtNombre.Focus();
        }

        private void CargarGrilla()
        {
            BLLUsuario55CA bll = new BLLUsuario55CA();

            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = bll.obtenerTodos();
        }


    }
}
