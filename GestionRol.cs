using BLL;
using Services.Modelos;
using Services.Modelos.Idioma;
using Services_55CA;
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
    public partial class GestionRol : Form, IIdiomaObserver
    {
        BLLRol bllRol = new BLLRol();
        BLLFamilia bllFamilia = new BLLFamilia();
        List<RolModelo55CA> listaRol = new List<RolModelo55CA>();
        BLLPatente bllPatente = new BLLPatente();

        public GestionRol()
        {
            InitializeComponent();

            cargarDatos();
            btnCancelar.Enabled = false;
            btnAplicar.Enabled = false;

            ServiceSessionManager55CA.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        private void cargarDatos()
        {
            tvPermisosAsignados.Nodes.Clear();
            listaRol = bllRol.ObtenerRolesConJerarquia();
            dgvFamilias.DataSource = null;
            dgvFamilias.DataSource = listaRol;

            var todosLosComponentes = new List<Componente55CA>();

            todosLosComponentes.AddRange(bllFamilia.ObtenerTodos());
            todosLosComponentes.AddRange(bllPatente.obtenerTodos());

            checkListPermisosFamilias.DataSource = null;
            checkListPermisosFamilias.DataSource = todosLosComponentes;
        }

        private enum ModoOperacionFamilia
        {
            Ninguno,
            Crear,
            Asignar,
            Eliminar,
            Desasignar
        }

        private ModoOperacionFamilia modoActual = ModoOperacionFamilia.Ninguno;

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager55CA.getIntancia().Idioma;

            this.Text = t.Translate("GestionRol.formTitle");
            label1.Text = t.Translate("GestionRol.labelRoles");
            label2.Text = t.Translate("GestionRol.labelPermisosFamilias");
            label3.Text = t.Translate("GestionRol.labelAsignados");
            groupBox1.Text = t.Translate("GestionRol.groupBoxDatos");
            label4.Text = t.Translate("GestionRol.labelNombre");
            btnCrear.Text = t.Translate("GestionRol.btnCrear");
            btnAsignar.Text = t.Translate("GestionRol.btnAsignar");
            btnAplicar.Text = t.Translate("GestionRol.btnAplicar");
            btnEliminar.Text = t.Translate("GestionRol.btnEliminar");
            btnDesasginar.Text = t.Translate("GestionRol.btnDesasignar");
            btnCancelar.Text = t.Translate("GestionRol.btnCancelar");
            dgvFamilias.Columns["Nombre"].HeaderText = t.Translate("GestionUsuario.colNombre");

        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Crear;

            groupBox1.Visible = true;
            txtNombre.Focus();

            btnAplicar.Enabled = true;
            btnCrear.Enabled = false;
            btnEliminar.Enabled = false;
            btnAsignar.Enabled = false;
            btnCancelar.Enabled = true;
            btnDesasginar.Enabled = false;
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Asignar;

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = false;
            btnCrear.Enabled = false;
            btnDesasginar.Enabled = false;
        }

        private void dgvFamilias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFamilias.CurrentRow != null)
            {
                RolModelo55CA rolSeleccionado = (RolModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;
                MostrarArbolDelRol(rolSeleccionado);
            }
        }

        private void MostrarArbolDelRol(RolModelo55CA rol)
        {
            tvPermisosAsignados.Nodes.Clear(); 
            TreeNode nodoRaiz = new TreeNode($"Rol: {rol.Nombre}");
            tvPermisosAsignados.Nodes.Add(nodoRaiz);

            foreach (Componente55CA componente in rol.Permisos)
            {
                TreeNode nodoHijo = new TreeNode(componente.Nombre);
                nodoHijo.Tag = componente;
                nodoRaiz.Nodes.Add(nodoHijo);

                if (componente is FamiliaModelo55CA familia)
                {
                    ConstruirRamasFamilia(nodoHijo, familia); // si es familia, llamamos al método recursivo para abrirla
                }
                else if (componente is PermisoModelo55CA patente)
                {
                    nodoHijo.Text = $"{patente.Nombre}";
                }
            }

            tvPermisosAsignados.ExpandAll();
        }

        private void ConstruirRamasFamilia(TreeNode nodoPadre, FamiliaModelo55CA familia)
        {
            foreach (Componente55CA hijo in familia.obtenerPermisos())
            {
                TreeNode nodoHijo = new TreeNode();
                nodoPadre.Nodes.Add(nodoHijo);

                if (hijo is FamiliaModelo55CA subFamilia)
                {
                    nodoHijo.Text = $"{subFamilia.Nombre}";
                    ConstruirRamasFamilia(nodoHijo, subFamilia);
                }
                else
                {
                    nodoHijo.Text = $"{hijo.Nombre}";
                }
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                if (modoActual == ModoOperacionFamilia.Crear)
                {
                    string nombre = txtNombre.Text;

                    if (string.IsNullOrEmpty(nombre))
                    {
                        MessageBox.Show("El nombre no puede estar vacío.");
                        return;
                    }

                    if (checkListPermisosFamilias.CheckedItems.Count == 0)
                    {
                        MessageBox.Show("Debe marcar al menos un permiso o familia para crear el rol.");
                        return;
                    }

                    List<Componente55CA> componentesSeleccionados = new List<Componente55CA>();

                    foreach (Componente55CA componenteMarcado in checkListPermisosFamilias.CheckedItems)
                    {
                        componentesSeleccionados.Add(componenteMarcado);
                    }

                    bllRol.crearRol(nombre, componentesSeleccionados);

                    MessageBox.Show("El rol fue creado y sus permisos fueron asignados con éxito.");
                }

                else if (modoActual == ModoOperacionFamilia.Asignar)
                {
                    if (dgvFamilias.CurrentRow == null)
                    {
                        MessageBox.Show("Debe seleccionar un Rol de la lista de roles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (checkListPermisosFamilias.CheckedItems.Count == 0)
                    {
                        MessageBox.Show("Debe marcar al menos un Componente (Familia/Permiso) de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    RolModelo55CA rolDestino = (RolModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;

                    foreach (Componente55CA componenteMarcado in checkListPermisosFamilias.CheckedItems)
                    {
                        if (componenteMarcado is PermisoModelo55CA patente)
                        {
                            bllRol.AsignarPatente(rolDestino, patente);
                        }
                        else if (componenteMarcado is FamiliaModelo55CA familiaHija)
                        {
                            bllRol.AsignarFamilia(rolDestino, familiaHija);
                        }
                    }

                    MessageBox.Show("Los componentes marcados fueron evaluados y asignados.");
                }

                else if (modoActual == ModoOperacionFamilia.Eliminar)
                {
                    if (dgvFamilias.CurrentRow == null)
                    {
                        MessageBox.Show("Debe seleccionar un rol destino para eliminar.");
                        return;
                    }

                    RolModelo55CA rolSeleccionado = (RolModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;
                    bllRol.EliminarRol(rolSeleccionado.Id);
                    MessageBox.Show("Se eliminó correctamente el Rol.");
                }

                else if(modoActual == ModoOperacionFamilia.Desasignar)
                {
                    RolModelo55CA rolSeleccionado = (RolModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;
                    Componente55CA componenteAQuitar = (Componente55CA)tvPermisosAsignados.SelectedNode.Tag;

                    DialogResult respuesta = MessageBox.Show($"¿Seguro que desea quitar '{componenteAQuitar.Nombre}' del rol '{rolSeleccionado.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta == DialogResult.Yes)
                    {
                        if (componenteAQuitar is PermisoModelo55CA patente)
                        {
                            bllRol.DesasignarPatente(rolSeleccionado.Id, patente.Id);
                        }
                        else if (componenteAQuitar is FamiliaModelo55CA familia)
                        {
                            bllRol.DesasignarFamilia(rolSeleccionado.Id, familia.Id);
                        }

                        MessageBox.Show("Componente desasignado correctamente.");
                        cargarDatos(); // Recargamos la BD para actualizar el árbol
                    }
                }

                modoActual = ModoOperacionFamilia.Ninguno;
                groupBox1.Visible = false;
                btnCancelar.Enabled = false;
                btnAplicar.Enabled = false;
                btnCrear.Enabled = true;
                btnAsignar.Enabled = true;
                btnEliminar.Enabled = true;
                btnDesasginar.Enabled = true;

                DesmarcarCheckList();
                cargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DesmarcarCheckList()
        {
            for (int i = 0; i < checkListPermisosFamilias.Items.Count; i++)
            {
                checkListPermisosFamilias.SetItemChecked(i, false);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Ninguno;
            groupBox1.Visible = false;
            btnCancelar.Enabled = false;
            btnAplicar.Enabled = false;
            btnCrear.Enabled = true;
            btnAsignar.Enabled = true;
            btnEliminar.Enabled = true;
            btnDesasginar.Enabled = true;

            DesmarcarCheckList();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Eliminar;

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnAsignar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCrear.Enabled = false;
            btnDesasginar.Enabled = false;
        }

        private void btnDesasginar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Desasignar;

            if (dgvFamilias.CurrentRow == null) return;
            if (tvPermisosAsignados.SelectedNode == null)
            {
                MessageBox.Show("Debe seleccionar un componente del árbol para desasignarlo.");
                return;
            }

            if (tvPermisosAsignados.SelectedNode.Level != 1)
            {
                MessageBox.Show("Solo puede desasignar componentes directos del rol. Para quitar permisos internos, modifique la familia correspondiente.");
                return;
            }

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnAsignar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
            btnDesasginar.Enabled = false;
        }
    }
}
