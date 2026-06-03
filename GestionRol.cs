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

            var vistaComponentes = todosLosComponentes.Select(c => new
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Tipo = c is FamiliaModelo55CA ? "Familia" : "Patente",
                ObjetoReal = c
            }).ToList();

            dgvPermisosFamilias.DataSource = null;
            dgvPermisosFamilias.DataSource = vistaComponentes;
            dgvPermisosFamilias.Columns["ObjetoReal"].Visible = false;

        }

        private enum ModoOperacionFamilia
        {
            Ninguno,
            Crear,
            Asignar,
            Eliminar
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
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Asignar;

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = false;
            btnCrear.Enabled = false;
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
                if(modoActual == ModoOperacionFamilia.Crear)
                {
                    string nombre = txtNombre.Text;

                    if (string.IsNullOrEmpty(nombre))
                    {
                        MessageBox.Show("El nombre no puede estar vacio.");
                        return;
                    }

                    bllRol.crearRol(nombre);

                    MessageBox.Show("El rol fue creado con exito");
                }

                else if(modoActual == ModoOperacionFamilia.Asignar)
                {
                    if (dgvFamilias.CurrentRow == null || dgvPermisosFamilias.CurrentRow == null)
                    {
                        MessageBox.Show("Debe seleccionar un Rol de la lista y un Componente (Familia/Permiso) para asignar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    RolModelo55CA rolDestino = (RolModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;
                    Componente55CA componenteAAsignar = (Componente55CA)dgvPermisosFamilias.CurrentRow.Cells["ObjetoReal"].Value;

                    if (componenteAAsignar is PermisoModelo55CA patente)
                    {
                        bllRol.AsignarPatente(rolDestino, patente);
                    }
                    else if (componenteAAsignar is FamiliaModelo55CA familiaHija)
                    {
                        bllRol.AsignarFamilia(rolDestino, familiaHija);
                    }

                    MessageBox.Show("El componente fue asignado correctamente al rol.");
                }

                else if(modoActual == ModoOperacionFamilia.Eliminar)
                {
                    if (dgvFamilias.CurrentRow == null)
                    {
                        MessageBox.Show("Debe seleccionar una familia destino y un componente a asignar.");
                        return;
                    }

                    RolModelo55CA rolSeleccionado = (RolModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;

                    bllRol.EliminarRol(rolSeleccionado.Id);

                    MessageBox.Show("Se elimino correctamente el Rol.");
                }

                modoActual = ModoOperacionFamilia.Ninguno;

                groupBox1.Visible = false;
                btnCancelar.Enabled = false;
                btnAplicar.Enabled = false;
                btnCrear.Enabled = true;
                btnAsignar.Enabled = true;
                btnEliminar.Enabled = true;

                cargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la operacion: " + ex.Message);
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
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Eliminar;

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnAsignar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCrear.Enabled = false;
        }
    }
}
