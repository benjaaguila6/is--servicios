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
using System.Drawing.Printing;


namespace Servicios
{
    public partial class AuditoriaBitacora : Form
    {
        BitacoraEventosService bitService = new BitacoraEventosService();
        PrintDocument printDoc = new PrintDocument();
        public AuditoriaBitacora()
        {
            InitializeComponent();
            printDoc.PrintPage += printDoc_PrintPage;
        }

        private void AuditoriaBitacora_Load(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-3);
            dtpHasta.Value = DateTime.Now;

            CargarGrillaInicial();
        }

        private void CargarGrillaInicial()
        {
            dgvBitacora.DataSource = bitService.obtenerUltimos3Dias();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1);

            dgvBitacora.DataSource = bitService.obtenerBitacora(desde, hasta);
        }

       

        

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 20;
            int y = 50;
            int rowHeight = 25;

            Font font = new Font("Arial", 10);
            Font fontHeader = new Font("Arial", 10, FontStyle.Bold);

            e.Graphics.DrawString("Bitácora de Eventos", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, x, y);
            y += 40;

            foreach (DataGridViewColumn col in dgvBitacora.Columns)
            {
                e.Graphics.DrawString(col.HeaderText, fontHeader, Brushes.Black, x, y);
                x += 120; // espacio entre columnas
            }

            y += rowHeight;
            x = 20;

            foreach (DataGridViewRow row in dgvBitacora.Rows)
            {
                if (!row.IsNewRow)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        e.Graphics.DrawString(cell.Value?.ToString(), font, Brushes.Black, x, y);
                        x += 120;
                    }

                    y += rowHeight;
                    x = 20;
                }
            }
        }

        private void btnLimpiat_Click_1(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-3);
            dtpHasta.Value = DateTime.Now;

            CargarGrillaInicial();
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDoc;
            preview.ShowDialog();
        }
    }
}
