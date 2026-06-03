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
        public GestionRol()
        {
            InitializeComponent();

            ServiceSessionManager55CA.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

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
    }
}
