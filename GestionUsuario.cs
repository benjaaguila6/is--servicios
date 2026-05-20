using BE;
using BLL;
using Services;
using Services.Modelos;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
        BLLRol _bllRol = new BLLRol();

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
            cmbRol.DataSource = _bllRol.obtenerTodos();
            cmbRol.DisplayMember = "Nombre";
            cmbRol.ValueMember = "Id";

            btnCancelar.Enabled = false;
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
            dgvUsuarios.AutoGenerateColumns = false;

            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn{Name = "DNI", DataPropertyName = "DNI", HeaderText = "DNI"});
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn{Name = "Nombre", DataPropertyName = "Nombre", HeaderText = "Nombre"});
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn{Name = "Apellido", DataPropertyName = "Apellido", HeaderText = "Apellido"});
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn{Name = "Email", DataPropertyName = "Email", HeaderText = "Email"});
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn{Name = "Rol", DataPropertyName = "Rol", HeaderText = "Rol"});
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn{Name = "User", DataPropertyName = "User", HeaderText = "Username"});
            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn{Name = "Activo", DataPropertyName = "Activo", HeaderText = "Activo"});
            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn{Name = "Bloqueo", DataPropertyName = "Bloqueo", HeaderText = "Bloqueado"});

            listUsuarios = usuarioService.obtenerTodos();

            // usuario logueado
            string dniUsuarioActivo = ServiceSessionManager55CA.getIntancia().usuarioActivo.DNI;

            // para que no se pueda automodificar
            listUsuarios = listUsuarios.Where(u => u.DNI != dniUsuarioActivo).ToList();

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
            btnCancelar.Enabled = true;

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

            try
            {
                txtDNI.Text = fila.Cells["DNI"].Value.ToString();

                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();

                txtApellido.Text = fila.Cells["Apellido"].Value.ToString();

                txtEmail.Text = fila.Cells["Email"].Value.ToString();

                Rol55CA rol = (Rol55CA)fila.Cells["Rol"].Value;

                cmbRol.SelectedValue = rol.Id;

                btnCrear.Enabled = false;
                btnActDesact.Enabled = false;
                btnDesbloquear.Enabled = false;
                btnModificar.Enabled = false;
                btnCancelar.Enabled = true;

                // solo modificables
                txtDNI.Enabled = false;
                txtNombre.Enabled = false;
                txtApellido.Enabled = false;

                txtEmail.Enabled = true;

                cmbRol.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar usuario: " + ex.Message);
            }
        }

        private void rbTodos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void rbActivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
            dgvUsuarios.DataSource = listUsuarios.Where(u => u.Activo == true).ToList();
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (modoActual == ModoOperacion.Crear)
                {
                    string email = txtEmail.Text;
                    string nombre = txtNombre.Text;
                    string apellido = txtApellido.Text;
                    string dNI = txtDNI.Text;

                    Rol55CA rol = (Rol55CA)cmbRol.SelectedItem;

                    if (cmbRol.SelectedIndex == -1)
                    {
                        MessageBox.Show("Seleccione un rol.");
                        return;
                    }

                    if (email.Length <= 0 || nombre.Length <= 0 || apellido.Length <= 0 || dNI.Length <= 0)
                    {
                        MessageBox.Show("Debe completar todos los campos.");
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

                    usuarioService.CrearUsuario(dNI, nombre, apellido, email, rol);

                    MessageBox.Show("Usuario creado con éxito.");
                }

                else if (modoActual == ModoOperacion.Modificar)
                {
                    string email = txtEmail.Text;
                    string dNI = txtDNI.Text;

                    Rol55CA rol = (Rol55CA)cmbRol.SelectedItem;

                    usuarioService.ModificarUsuario(dNI, email, rol);

                    MessageBox.Show("Usuario modificado correctamente.");
                }

                else if (modoActual == ModoOperacion.Desbloquear)
                {
                    string dni = dgvUsuarios.CurrentRow.Cells["DNI"].Value.ToString();

                    bool bloqueado = Convert.ToBoolean(dgvUsuarios.CurrentRow.Cells["Bloqueo"].Value);

                    if (!bloqueado)
                    {
                        MessageBox.Show("El usuario no está bloqueado.");
                        return;
                    }

                    DialogResult r = MessageBox.Show("¿Seguro que desea desbloquear este usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (r != DialogResult.Yes)
                    {
                        return;
                    }

                    usuarioService.DesbloquearUsuario(dni);

                    MessageBox.Show("Usuario desbloqueado correctamente.");
                }

                gbDatos.Visible = false;

                modoActual = ModoOperacion.Ninguno;

                btnCrear.Enabled = true;
                btnActDesact.Enabled = true;
                btnDesbloquear.Enabled = true;
                btnModificar.Enabled = true;
                btnCancelar.Enabled = false;

                CargarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            btnCancelar.Enabled = false;
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
            try
            {
                string dniSeleccionado = dgvUsuarios.CurrentRow.Cells["DNI"].Value.ToString();
                usuarioService.activarDesactivar(dniSeleccionado);
                MessageBox.Show("El estado del usuario se actualizó correctamente.");

                CargarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacion.Desbloquear;

            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario.");
                return;
            }

            btnCrear.Enabled = false;
            btnActDesact.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnModificar.Enabled = false;

            btnCancelar.Enabled = true;
        }


    }
}
