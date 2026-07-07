using BLL;
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
    public partial class RepararInconsistencias : Form, IIdiomaObserver
    {
        private bool usuarioOk, rolOk, familiaOk, patenteOk;
        BLLDigitoVerificador bllDV = new BLLDigitoVerificador();

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            var idioma = ServiceSessionManager55CA.getIntancia().Idioma;

            try
            {
                if (!usuarioOk) bllDV.RepararUsuario();
                if (!rolOk) bllDV.RepararRol();
                if (!familiaOk) bllDV.RepararFamilia();
                if (!patenteOk) bllDV.RepararPatente();

                MessageBox.Show(idioma.Translate("MsgReparacionExitosa"));

                this.Hide();
                Login login = new Login();
                login.FormClosed += (s, args) => this.Close();
                login.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(idioma.Translate("MsgErrorReparar") + ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            var idioma = ServiceSessionManager55CA.getIntancia().Idioma;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = idioma.Translate("FiltroBackup");
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        bllDV.RealizarRestore(ofd.FileName);

                        MessageBox.Show(idioma.Translate("MsgRestoreExitoso"));
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(idioma.Translate("MsgErrorRestore") + ex.Message);
                    }
                }
            }
        }

        public RepararInconsistencias(bool usuarioOk, bool rolOk, bool familiaOk, bool patenteOk)
        {
            InitializeComponent();

            this.usuarioOk = usuarioOk;
            this.rolOk = rolOk;
            this.familiaOk = familiaOk;
            this.patenteOk = patenteOk;

            ServiceSessionManager55CA.getIntancia().Idioma.Suscribir(this);

            actualizarIdioma();

            MostrarTablasConError();
        }

        private void actualizarIdioma()
        {
            var idioma = ServiceSessionManager55CA.getIntancia().Idioma;

            this.Text = idioma.Translate("TituloRepararInconsistencias");
            btnRecalcular.Text = idioma.Translate("BtnRecalcular");
            btnRestore.Text = idioma.Translate("BtnRestore");
            btnSalir.Text = idioma.Translate("BtnSalir");

            MostrarTablasConError();
        }

        private void MostrarTablasConError()
        {
            var idioma = ServiceSessionManager55CA.getIntancia().Idioma;

            string mensaje = idioma.Translate("MensajeInconsistenciasDetectadas") + "\n";

            if (!usuarioOk) mensaje += "- Usuario\n";
            if (!rolOk) mensaje += "- Rol\n";
            if (!familiaOk) mensaje += "- Familia\n";
            if (!patenteOk) mensaje += "- Patente\n";

            lblMensaje.Text = mensaje;
        }


        
    }
}
