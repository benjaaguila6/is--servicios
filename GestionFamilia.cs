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
    public partial class GestionFamilia : Form, IIdiomaObserver
    {
        private BLLFamilia bllFamilia = new BLLFamilia();
        private BLLPatente bllPermiso = new BLLPatente();
        private List<FamiliaModelo55CA> listaFamilias;

        private enum ModoOperacionFamilia
        {
            Ninguno,
            Crear,
            Asignar,
            Eliminar
        }

        private ModoOperacionFamilia modoActual = ModoOperacionFamilia.Ninguno;

        public GestionFamilia()
        {
            InitializeComponent();
            cargarDatos();

            ServiceSessionManager55CA.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        public void cargarDatos()
        {
            tvPermisosAsignados.Nodes.Clear();
            listaFamilias = bllFamilia.ObtenerTodos();
            dgvFamilias.DataSource = null;
            dgvFamilias.DataSource = listaFamilias;

            var todosLosComponentes = new List<Componente55CA>();
            todosLosComponentes.AddRange(listaFamilias);
            todosLosComponentes.AddRange(bllPermiso.obtenerTodos());

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

            btnAplicar.Enabled = false;
        }
        public void actualizarIdioma()
        {
            var t = ServiceSessionManager55CA.getIntancia().Idioma;

            this.Text = t.Translate("GestionFamilia.formTitle");
            label1.Text = t.Translate("GestionFamilia.labelFamilias");
            label2.Text = t.Translate("GestionFamilia.labelPermisosFamilias");
            label3.Text = t.Translate("GestionFamilia.labelAsignados");
            groupBox1.Text = t.Translate("GestionFamilia.groupBoxDatos");
            label4.Text = t.Translate("GestionFamilia.labelNombre");
            btnCrear.Text = t.Translate("GestionFamilia.btnCrear");
            btnAsignar.Text = t.Translate("GestionFamilia.btnAsignar");
            btnAplicar.Text = t.Translate("GestionFamilia.btnAplicar");
            btnEliminar.Text = t.Translate("GestionFamilia.btnEliminar");
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Asignar;

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = false;
            btnCrear.Enabled = false ;
            btnAsignar.Enabled = false;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Crear;


            groupBox1.Visible = true;
            txtNombre.Focus();

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnCrear.Enabled = false;
            btnEliminar.Enabled = false;
            btnAsignar.Enabled = false;
        }

        private void dgvFamilias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFamilias.CurrentRow != null)
            {
                FamiliaModelo55CA familiaSeleccionada = (FamiliaModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;
                MostrarArbol(familiaSeleccionada);
            }
        }

        private void MostrarArbol(FamiliaModelo55CA familia)
        {
            tvPermisosAsignados.Nodes.Clear(); 

            TreeNode nodoRaiz = new TreeNode(familia.Nombre);
            tvPermisosAsignados.Nodes.Add(nodoRaiz);
            ConstruirRamas(nodoRaiz, familia);

            tvPermisosAsignados.ExpandAll();
        }

        private void ConstruirRamas(TreeNode nodoPadre, FamiliaModelo55CA familia)
        {
            foreach (Componente55CA hijo in familia.obtenerPermisos())
            {
                TreeNode nodoHijo = new TreeNode(hijo.Nombre);
                nodoPadre.Nodes.Add(nodoHijo);

                if (hijo is FamiliaModelo55CA subFamilia)
                {
                    nodoHijo.NodeFont = new Font(tvPermisosAsignados.Font, FontStyle.Bold);
                    ConstruirRamas(nodoHijo, subFamilia);
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

                    if (nombre.Length <= 0)
                    {
                        MessageBox.Show("Debe ingresar un nombre para la familia.");
                        return;
                    }

                    bllFamilia.CrearFamilia(nombre);

                    MessageBox.Show("Familia creada con éxito.");
                }

                else if (modoActual == ModoOperacionFamilia.Asignar)
                {
                    if (dgvFamilias.CurrentRow == null || dgvPermisosFamilias.CurrentRow == null)
                    {
                        MessageBox.Show("Debe seleccionar una familia destino y un componente a asignar.");
                        return;
                    }

                    FamiliaModelo55CA familiaDestino = (FamiliaModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;
                    Componente55CA componenteAAsignar = (Componente55CA)dgvPermisosFamilias.CurrentRow.Cells["ObjetoReal"].Value;

                    if (componenteAAsignar is PermisoModelo55CA patente)
                    {
                        bllFamilia.AsignarPatente(familiaDestino, patente);
                    }
                    else if (componenteAAsignar is FamiliaModelo55CA familiaHija)
                    {
                        bllFamilia.AsignarFamilia(familiaDestino, familiaHija);
                    }

                    MessageBox.Show("Componente asignado correctamente a la familia.");
                }

                else if(modoActual == ModoOperacionFamilia.Eliminar)
                {
                    if (dgvFamilias.CurrentRow == null)
                    {
                        MessageBox.Show("Debe seleccionar una familia destino y un componente a asignar.");
                        return;
                    }

                    FamiliaModelo55CA familiaSeleccionada = (FamiliaModelo55CA)dgvFamilias.CurrentRow.DataBoundItem;

                    bllFamilia.EliminarFamilia(familiaSeleccionada.Id);

                    MessageBox.Show("Se elimino correctamente la familia.");
                }

                modoActual = ModoOperacionFamilia.Ninguno;

                txtNombre.Clear();
                groupBox1.Visible = false;

                btnCrear.Enabled = true;
                btnAsignar.Enabled = true;
                btnEliminar.Enabled = true;
                btnAplicar.Enabled = false;

                cargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Eliminar;

            btnCancelar.Enabled = true;
            btnAplicar.Enabled = true;
            btnAsignar.Enabled = false;
            btnCrear.Enabled = false;
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
    }
}
