using Services;
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
    public partial class GestionRespaldo : Form
    {
        BackUpRestore55CA serviceBackUpRestore = new BackUpRestore55CA();
        public GestionRespaldo()
        {
            InitializeComponent();
        }

        private void buscarCarpetaBackUp_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Seleccione la carpeta donde desea guardar el Backup";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtRutaBackUp.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnRealizarBackUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRutaBackUp.Text))
                {
                    MessageBox.Show("Por favor, seleccione una ruta primero.");
                    return;
                }

                serviceBackUpRestore.realizarBackUp(txtRutaBackUp.Text);
                MessageBox.Show("Backup generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtRutaBackUp.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el Backup: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buscarCarpetaRestore_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Seleccione el archivo de Backup a restaurar";
                ofd.Filter = "Archivos de Backup SQL (*.bak)|*.bak|Todos los archivos (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtRutaRestore.Text = ofd.FileName;
                }
            }
        }

        private void btnRealizarRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRutaRestore.Text))
                {
                    MessageBox.Show("Por favor, seleccione un archivo de backup primero.");
                    return;
                }

                DialogResult r = MessageBox.Show("¿Está seguro que desea restaurar la base de datos? Se perderán los datos actuales no respaldados.", "Advertencia Crítica", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (r == DialogResult.Yes)
                {
                    serviceBackUpRestore.realizarRestore(txtRutaRestore.Text);
                    MessageBox.Show("Restauración completada con éxito. El sistema se reiniciará por seguridad.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
