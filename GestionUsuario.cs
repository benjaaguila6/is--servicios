using BE;
using Services;
using Services.Modelos;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servicios
{
    public partial class GestionUsuario : Form
    {
        UsuarioService usuarioService = new UsuarioService();
        List<UsuarioModelo55CA> listUsuarios = new List<UsuarioModelo55CA>();

        //un enum para que el boton guardar sepa que hacer
        private enum ModoOperacion
        { Ninguno,
          Crear,
          Modificar,
          ActDesact,
          Desbloquear
        }

        private ModoOperacion modoActual = ModoOperacion.Ninguno; //inicializamos el modo en Ninguno
        public GestionUsuario()
        {
            InitializeComponent();
            CargarGrilla();
        }

        private void CrearUsuario_Load(object sender, EventArgs e)
        {
            cmbRol.DataSource = Enum.GetValues(typeof(TipoRol55CA));
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
            dgvUsuarios.DataSource = null;
            
            listUsuarios = usuarioService.obtenerTodos();

            dgvUsuarios.DataSource = listUsuarios;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacion.Crear;
            gbDatos.Visible = true;

            btnCrear.Enabled = false;
            btnActDesact.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnModificar.Enabled = false;

            txtDNI.Enabled = true;
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtEmail.Enabled = true;
            cmbRol.Enabled = true;

            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario.");
                return;
            }


            modoActual = ModoOperacion.Modificar;
            gbDatos.Visible = true;

            DataGridViewRow fila = dgvUsuarios.CurrentRow;

            txtDNI.Text = fila.Cells["DNI"].Value.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
            txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
            txtEmail.Text = fila.Cells["Email"].Value.ToString();

            int rol = Convert.ToInt32(fila.Cells["Rol"].Value);
            cmbRol.SelectedIndex = rol - 1;

            btnCrear.Enabled = false;
            btnActDesact.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnModificar.Enabled = false;

            //se bloquea porque solo se puede modificar el rol y el email.
            txtDNI.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtEmail.Enabled = true;
            cmbRol.Enabled = true;

            //LimpiarCampos();
        }

        private void rbTodos_CheckedChanged(object sender, EventArgs e)
        {
            dgvUsuarios.DataSource = listUsuarios;
        }

        private void rbActivos_CheckedChanged(object sender, EventArgs e)
        {
            dgvUsuarios.DataSource = listUsuarios.Where(u => u.Activo == true);
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string dNI = txtDNI.Text;
            TipoRol55CA rol = (TipoRol55CA)cmbRol.SelectedItem;


            if (cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un rol.");
                return;
            }

            if (email.Length <= 0 || nombre.Length <= 0 || apellido.Length <= 0 || dNI.Length <= 0)
            {
                MessageBox.Show("Debe completar todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!EsEmailValido(email))
            {
                MessageBox.Show("El email no tiene el formato correcto.");
                return;
            }
            if (!EsDNIValido(dNI))
            {
                MessageBox.Show("El DNI no tiene el formato correcto.");
                return;
            }

            try
            {
                if (modoActual == ModoOperacion.Crear)
                {
                    usuarioService.CrearUsuario(dNI, nombre, apellido, email, rol);
                    MessageBox.Show("Usuario creado con éxito.");
                }
                else if (modoActual == ModoOperacion.Modificar)
                {
                    usuarioService.ModificarUsuario(dNI, email, rol);
                    MessageBox.Show("Usuario modificado correctamente.");
                }
                else if(modoActual == ModoOperacion.ActDesact)
                {
                    string dniSeleccionado = dgvUsuarios.CurrentRow.Cells["DNI"].Value.ToString();
                    usuarioService.activarDesactivar(dniSeleccionado);
                    MessageBox.Show("El estado del usuario se actualizó correctamente.");
                }

                gbDatos.Visible = false;
                modoActual = ModoOperacion.Ninguno;

                CargarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            gbDatos.Visible = false;
            modoActual = ModoOperacion.Ninguno;

            btnCrear.Enabled = true;
            btnActDesact.Enabled = true;
            btnDesbloquear.Enabled = true;
            btnModificar.Enabled = true;
        }

        #region Validaciones
        private bool EsEmailValido(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool EsDNIValido(string dni)
        {
            return Regex.IsMatch(dni, @"^\d{7,8}$");
        }

        #endregion Validaciones

        private void btnActDesact_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacion.ActDesact;
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario.");
                return;
            }

            string dni = dgvUsuarios.CurrentRow.Cells["DNI"].Value.ToString();
            bool bloqueado = Convert.ToBoolean(dgvUsuarios.CurrentRow.Cells["Bloqueo"].Value);

            if (!bloqueado)
            {
                MessageBox.Show("El usuario no está bloqueado.");
                return;
            }

            DialogResult r = MessageBox.Show(
                "¿Seguro que desea desbloquear este usuario?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (r != DialogResult.Yes)
                return;

            try
            {
                usuarioService.DesbloquearUsuario(dni);

                MessageBox.Show("Usuario desbloqueado correctamente.");

                CargarGrilla(); // refresca la tabla
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
